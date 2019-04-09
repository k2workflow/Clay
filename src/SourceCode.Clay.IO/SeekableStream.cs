using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Threading;
using System.Threading.Tasks;

namespace SourceCode.Clay.IO
{
    public sealed class SeekableStream : Stream
    {
        #region Task Wrapper
        [ExcludeFromCodeCoverage]
        private sealed class ReadTask : IAsyncResult
        {
            public object AsyncState { get; }

            public WaitHandle AsyncWaitHandle => ((IAsyncResult)_task).AsyncWaitHandle;

            public bool CompletedSynchronously => ((IAsyncResult)_task).CompletedSynchronously;

            public bool IsCompleted => _task.IsCompleted;

            public int Result => _task.Result;

            private readonly Task<int> _task;

            public ReadTask(Task<int> task, object asyncState)
            {
                _task = task;
                AsyncState = asyncState;
            }
        }
        #endregion

        #region Mapped File
        private readonly struct MappedFile : IDisposable
        {
            private readonly MemoryMappedFile _file;
            private readonly string _path;
            private readonly long _offset;
            public long Length { get; }

            public MappedFile(MemoryMappedFile file, string path, long length)
            {
                _file = file;
                _path = path;
                Length = length;
                _offset = 0;
            }

            private MappedFile(MemoryMappedFile file, string path, long length, long offset)
            {
                _file = file;
                _path = path;
                Length = length;
                _offset = offset;
            }

            public MappedFile WithOffset(long offset) => new MappedFile(_file, _path, Length, offset);

            public async ValueTask<MappedFile> WriteAsync(byte[] buffer, Stream stream, CancellationToken cancellationToken)
            {
                var position = 0L;

                using (MemoryMappedViewStream target = _file.CreateViewStream(0, Length, MemoryMappedFileAccess.Write))
                {
                    while (true)
                    {
                        var copy = (int)Math.Min(buffer.Length, Length - position);
                        if (copy <= 0) break;

                        var length = await stream.ReadAsync(buffer, 0, copy, cancellationToken).ConfigureAwait(false);
                        if (length == 0) break;

                        await target.WriteAsync(buffer, 0, length, cancellationToken).ConfigureAwait(false);
                        position += length;
                    }
                }

                return new MappedFile(_file, _path, position);
            }

            public async ValueTask<long> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
            {
                if (_file is null) return 0;

                count = Math.Min(count, (int)(Length - _offset));
                if (count <= 0) return 0;

                var total = 0L;
                using (MemoryMappedViewStream target = _file.CreateViewStream(_offset, count, MemoryMappedFileAccess.Read))
                {
                    while (count > 0)
                    {
                        var read = await target.ReadAsync(buffer, offset, count, cancellationToken).ConfigureAwait(false);
                        if (read <= 0) break;

                        count -= read;
                        offset += read;
                        total += read;
                    }
                }

                return total;
            }

            public long Read(byte[] buffer, int offset, int count)
            {
                if (_file is null) return 0;

                count = Math.Min(count, (int)(Length - _offset));
                if (count <= 0) return 0;

                var total = 0L;
                using (MemoryMappedViewStream target = _file.CreateViewStream(_offset, count, MemoryMappedFileAccess.Read))
                {
                    while (count > 0)
                    {
                        var read = target.Read(buffer, offset, count);
                        if (read <= 0) break;

                        count -= read;
                        offset += read;
                        total += read;
                    }
                }

                return total;
            }

            public void Dispose()
            {
                _file?.Dispose();
                if (_path != null) File.Delete(_path);
            }
        }
        #endregion

        public const long DefaultWindowSize = 1024 * 4096; // 4 MB

        public override bool CanRead => true;

        public override bool CanSeek => true;

        public override bool CanWrite => false;

        public override bool CanTimeout => false;

        private readonly long _length;
        public override long Length => _length;

        private long _position;
        public override long Position
        {
            get => _position;
            set => Seek(value, SeekOrigin.Begin);
        }

        private readonly List<MappedFile> _files;
        private readonly long _windowSize;
        private readonly string _rootPath;

        private SeekableStream(string rootPath, List<MappedFile> files, long windowSize, long length)
        {
            _rootPath = rootPath;
            _windowSize = windowSize;
            _length = length;
            _files = files;
        }

        public override void Close()
        {
            lock (_files)
            {
                while (_files.Count > 0)
                {
                    var i = _files.Count - 1;
                    MappedFile file = _files[i];
                    _files.RemoveAt(i);
                    file.Dispose();
                }

                Directory.Delete(_rootPath, true);
            }
            base.Close();
        }

        #region Factory
        public static async ValueTask<Stream> OptionalCreateAsync(Stream source, long windowSize = default, CancellationToken cancellationToken = default)
        {
            var useRaw = false;
            try
            {
                useRaw = source.CanSeek && source.Length > 0;
                source.Seek(source.Position, SeekOrigin.Begin);
            }
            catch (NotSupportedException) { }

            if (useRaw) return source;
            return await CreateAsync(source, windowSize, cancellationToken).ConfigureAwait(false);
        }

        public static async ValueTask<SeekableStream> CreateAsync(Stream source, long windowSize = default, CancellationToken cancellationToken = default)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (windowSize < 0) throw new ArgumentNullException(nameof(windowSize));

            var useLength = false;
            try
            {
                useLength = source.CanSeek && source.Length > 0;
            }
            catch (NotSupportedException) { }

            if (windowSize == 0)
                windowSize = useLength ? source.Length : DefaultWindowSize;

            List<MappedFile> files = useLength
                ? new List<MappedFile>((int)((source.Length + windowSize - 1) / windowSize))
                : new List<MappedFile>();

            var length = 0L;
            var identifier = Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture);
            var rootPath = Path.Combine(Path.GetTempPath(), identifier);
            Directory.CreateDirectory(rootPath);

            try
            {
                var buffer = new byte[Environment.SystemPageSize];
                while (true)
                {
                    var fileIdentifier = files.Count.ToString(CultureInfo.InvariantCulture);
                    var mapName = string.Concat(identifier, fileIdentifier);
                    var filePath = Path.Combine(rootPath, fileIdentifier);
                    var file = MemoryMappedFile.CreateFromFile(
                        filePath,
                        FileMode.CreateNew,
                        mapName,
                        windowSize,
                        MemoryMappedFileAccess.ReadWrite);

                    var info = new MappedFile(file, filePath, windowSize);
                    info = await info.WriteAsync(buffer, source, cancellationToken);

                    if (info.Length == 0) info.Dispose();
                    else files.Add(info);

                    length += info.Length;
                    if (info.Length < windowSize) break;
                }
            }
            catch
            {
                try
                {
                    foreach (MappedFile item in files) item.Dispose();
                    Directory.Delete(rootPath, true);
                }
                catch (IOException) { }

                throw;
            }

            return new SeekableStream(rootPath, files, windowSize, length);
        }
        #endregion

        #region Seek
        public override long Seek(long offset, SeekOrigin origin)
        {
            var newPosition = 0L;
            switch (origin)
            {
                case SeekOrigin.Begin:
                    newPosition = offset;
                    break;
                case SeekOrigin.Current:
                    newPosition = _position + offset;
                    break;
                case SeekOrigin.End:
                    newPosition = _length + offset;
                    break;
                default: throw new ArgumentOutOfRangeException(nameof(origin));
            }

            _position = Math.Min(_length, Math.Max(0, newPosition));
            return _position;
        }
        #endregion

        #region Calc
        private MappedFile GetFile(long position)
        {
            lock (_files)
            {
                var index = position / _windowSize;
                if (index >= _files.Count) return default;

                // NB: Not floating-point
                var start = index * _windowSize;
                var offset = position - start;
                return _files[(int)index].WithOffset(offset);
            }
        }
        #endregion

        #region Read
        public override int Read(byte[] buffer, int offset, int count)
        {
            if (buffer is null) throw new ArgumentNullException(nameof(buffer));
            if (offset < 0) throw new ArgumentOutOfRangeException(nameof(offset));
            if (count < 0 || (offset + count) > buffer.Length) throw new ArgumentOutOfRangeException(nameof(count));

            var total = 0L;
            while (true)
            {
                MappedFile file = GetFile(_position);
                var read = file.Read(buffer, offset, count);
                if (read == 0) break;

                offset += (int)read;
                count -= (int)read;
                total += read;
                _position += read;
            }

            return (int)total;
        }

        public override async Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
        {
            if (buffer is null) throw new ArgumentNullException(nameof(buffer));
            if (offset < 0) throw new ArgumentOutOfRangeException(nameof(offset));
            if (count < 0 || (offset + count) > buffer.Length) throw new ArgumentOutOfRangeException(nameof(count));

            var total = 0L;
            while (true)
            {
                MappedFile file = GetFile(_position);
                var read = await file.ReadAsync(buffer, offset, count, cancellationToken).ConfigureAwait(false);
                if (read == 0) break;

                offset += (int)read;
                count -= (int)read;
                total += read;
                _position += read;
            }

            return (int)total;
        }

        [ExcludeFromCodeCoverage]
        public override IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
        {
            Task<int> task = ReadAsync(buffer, offset, count);
            var iar = new ReadTask(task, state);
            task.ConfigureAwait(false).GetAwaiter().OnCompleted(() => callback(iar));
            return iar;
        }

        [ExcludeFromCodeCoverage]
        public override int EndRead(IAsyncResult asyncResult)
        {
            if (asyncResult is null) throw new ArgumentNullException(nameof(asyncResult));
            if (asyncResult is ReadTask readTask) return readTask.Result;
            throw new ArgumentOutOfRangeException(nameof(asyncResult));
        }
        #endregion

        #region Copy
        public override async Task CopyToAsync(Stream destination, int bufferSize, CancellationToken cancellationToken)
        {
            if (destination is null) throw new ArgumentNullException(nameof(destination));
            if (!destination.CanWrite) throw new ArgumentOutOfRangeException(nameof(destination));
            if (bufferSize <= 0) throw new ArgumentOutOfRangeException(nameof(bufferSize));

            var buffer = new byte[bufferSize];
            for (var i = 0; i < _files.Count; i++)
            {
                MappedFile file = _files[i];
                var count = file.Length;
                while (count > 0)
                {
                    var read = await file.ReadAsync(buffer, 0, buffer.Length, cancellationToken).ConfigureAwait(false);
                    if (read == 0) break;

                    await destination.WriteAsync(buffer, 0, (int)read).ConfigureAwait(false);
                    count -= read;
                }
            }
        }
        #endregion

        #region NOP
        [ExcludeFromCodeCoverage]
        public override void Flush() { }
        [ExcludeFromCodeCoverage]
        public override Task FlushAsync(CancellationToken cancellationToken) => Task.CompletedTask;
        #endregion

        #region Not Supported
        [ExcludeFromCodeCoverage]
        public override void SetLength(long value) => throw new NotSupportedException();
        [ExcludeFromCodeCoverage]
        public override void Write(byte[] buffer, int offset, int count) => throw new NotSupportedException();
        [ExcludeFromCodeCoverage]
        public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback callback, object state) => throw new NotSupportedException();
        [ExcludeFromCodeCoverage]
        public override void EndWrite(IAsyncResult asyncResult) => throw new NotSupportedException();
        [ExcludeFromCodeCoverage]
        public override Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken) => throw new NotSupportedException();
        [ExcludeFromCodeCoverage]
        public override void WriteByte(byte value) => throw new NotSupportedException();
        #endregion
    }
}
