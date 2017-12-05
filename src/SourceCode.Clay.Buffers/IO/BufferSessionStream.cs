#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using SourceCode.Clay.Buffers;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace SourceCode.Clay.IO
{
    internal sealed class BufferSessionStream : Stream
    {
        #region Fields

        private readonly object _lock;
        private BufferSession _session;
        private int _position;
        private bool _isReadOnly;

        #endregion

        #region Properties

        public override bool CanRead => true;

        public override bool CanSeek => true;

        public override bool CanTimeout => false;

        public override bool CanWrite => !_isReadOnly;

        public override long Position
        {
            get => _position;
            set
            {
                if (value < 0 || value > _session.Result.Count) throw new ArgumentOutOfRangeException(nameof(value));
                _position = (int)value;
            }
        }

        public override long Length => _session.Result.Count;

        #endregion

        #region Constructors

        public BufferSessionStream(BufferSession session)
        {
            _session = session;
            _lock = new object();
        }

        #endregion

        #region Methods

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (!disposing) return;

            lock (_lock)
            {
                _session.Dispose();
            }
        }

        /// <summary>
        /// Marks this <see cref="BufferSessionStream"/> as read-only.
        /// </summary>
        public BufferSessionStream MakeReadOnly()
        {
            _isReadOnly = true;
            return this;
        }

        public override void Flush()
        {
        }

        public override Task FlushAsync(CancellationToken cancellationToken)
            => Task.CompletedTask;

        public override long Seek(long offset, SeekOrigin origin)
        {
            lock (_lock)
            {
                switch (origin)
                {
                    case SeekOrigin.Begin:

                        if (offset < 0) return _position = 0;
                        return _position = (int)Math.Min(offset, _session.Result.Count);

                    case SeekOrigin.Current:

                        return _position = (int)Math.Max(
                            Math.Min(_position + offset, _session.Result.Count),
                            0
                        );

                    case SeekOrigin.End:

                        if (offset > 0) return _position = _session.Result.Count;
                        return _position = (int)Math.Max(_session.Result.Count + offset, 0);

                    default: throw new ArgumentOutOfRangeException(nameof(origin));
                }
            }
        }

        public override void Close()
        {
        }

        #endregion

        #region Read

        public int Read(Span<byte> buffer)
        {
            lock (_lock)
            {
                if (buffer.IsEmpty) return 0;

                var remaining = _session.Result.Count - _position;
                if (remaining == 0) return 0;

                var toCopy = Math.Min(remaining, buffer.Length);
                _session.Span.Slice(_position, toCopy).CopyTo(buffer);
                _position += toCopy;
                return toCopy;
            }
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            if (buffer == null) throw new ArgumentNullException(nameof(buffer));
            return Read(new Span<byte>(buffer, offset, count));
        }

        public override Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
            => Task.FromResult(Read(buffer, offset, count));

        public override IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
        {
            if (buffer == null) throw new ArgumentNullException(nameof(buffer));

            var read = Read(buffer, offset, count);
            var result = new SyncAsyncResult(state, read, callback);
            ThreadPool.QueueUserWorkItem(result.ThreadPoolWorkItem);
            return result;
        }

        public override int EndRead(IAsyncResult asyncResult)
        {
            if (asyncResult is SyncAsyncResult sync)
                return sync.BytesCopied;
            throw new ArgumentOutOfRangeException(nameof(asyncResult), "asyncResult was not created by this stream.");
        }

        public override int ReadByte()
        {
            lock (_lock)
            {
                if (_position == _session.Result.Count) return -1;
                var result = _session.Span[_position++];
                return result;
            }
        }

        public override void CopyTo(Stream destination, int bufferSize)
        {
            lock (_lock)
            {
                if (_position == _session.Result.Count) return;

                destination.Write(
                    _session.Result.Array,
                    _session.Result.Offset + _position,
                    _session.Result.Count - _position);

                _position = _session.Result.Count;
            }
        }

        public override async Task CopyToAsync(Stream destination, int bufferSize, CancellationToken cancellationToken)
        {
            var session = _session;
            var position = _position;
            if (_position == session.Result.Count) return;
            _position = session.Result.Count;

            await destination.WriteAsync(
                session.Result.Array,
                session.Result.Offset + position,
                session.Result.Count - position,
                cancellationToken
            ).ConfigureAwait(false);
        }

        #endregion

        #region Write

        public void Write(ReadOnlySpan<byte> buffer)
        {
            if (_isReadOnly) throw CreateInvalidOperationException();

            lock (_lock)
            {
                if (buffer.IsEmpty) return;

                var remaining = _session.Result.Count - _position;
                if (remaining == 0) throw new EndOfStreamException();

                var toCopy = Math.Min(remaining, buffer.Length);
                buffer.Slice(0, toCopy).CopyTo(_session.Span.Slice(_position));
                _position += toCopy;

                if (remaining != buffer.Length) throw new EndOfStreamException();
            }
        }

        public override void Write(byte[] buffer, int offset, int count)
            => Write(new ReadOnlySpan<byte>(buffer, offset, count));

        public override Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
        {
            Write(buffer, offset, count);
            return Task.CompletedTask;
        }

        public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
        {
            Write(buffer, offset, count);
            var result = new SyncAsyncResult(state, count, callback);
            ThreadPool.QueueUserWorkItem(result.ThreadPoolWorkItem);
            return result;
        }

        public override void EndWrite(IAsyncResult asyncResult)
        {
            if (!(asyncResult is SyncAsyncResult))
                throw new ArgumentOutOfRangeException(nameof(asyncResult), "asyncResult was not created by this stream.");
        }

        public override void WriteByte(byte value)
        {
            if (_isReadOnly) throw CreateInvalidOperationException();

            lock (_lock)
            {
                var remaining = _session.Result.Count - _position;
                if (remaining == 0) throw new EndOfStreamException();
                _session.Span[_position++] = value;
            }
        }

        #endregion

        #region Length

        public override void SetLength(long value)
            => throw new NotSupportedException("Changing the length of this stream is not supported.");

        #endregion

        #region Not Supported

        private static Exception CreateInvalidOperationException()
            => new InvalidOperationException("The stream is read-only.");

        #endregion
    }
}
