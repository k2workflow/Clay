#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using System;
using System.Buffers;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace SourceCode.Clay.IO
{
#if NETCOREAPP2_2
    public sealed class MemoryBufferStream : Stream
    {
        private readonly ReadOnlyMemory<byte> _memory;

        private readonly object _lock;

        private int _position;

        public override bool CanRead => true;

        public override bool CanSeek => true;

        public override bool CanTimeout => false;

        public override bool CanWrite => false;

        public override long Position
        {
            get => _position;
            set
            {
                if ((uint)value >= (uint)_memory.Length) throw new ArgumentOutOfRangeException(nameof(value));
                _position = (int)value;
            }
        }

        public override long Length => _memory.Length;

        public MemoryBufferStream(ReadOnlyMemory<byte> memory)
        {
            _memory = memory;
            _lock = new object();
        }

        public override void Flush()
        {
        }

        public override Task FlushAsync(CancellationToken cancellationToken)
            => FlushAsync();

        public override long Seek(long offset, SeekOrigin origin)
        {
            lock (_lock)
            {
                switch (origin)
                {
                    case SeekOrigin.Begin:

                        if (offset < 0) return _position = 0;
                        return _position = (int)Math.Min(offset, _memory.Length);

                    case SeekOrigin.Current:

                        return _position = (int)Math.Max(
                            Math.Min(_position + offset, _memory.Length),
                            0
                        );

                    case SeekOrigin.End:

                        if (offset > 0) return _position = _memory.Length;
                        return _position = (int)Math.Max(_memory.Length + offset, 0);

                    default: throw new ArgumentOutOfRangeException(nameof(origin));
                }
            }
        }

        public override void Close()
        {
        }

        public override int Read(Span<byte> buffer)
        {
            lock (_lock)
            {
                if (buffer.IsEmpty) return 0;

                var remaining = _memory.Length - _position;
                if (remaining == 0) return 0;

                var toCopy = Math.Min(remaining, buffer.Length);
                _memory.Span.Slice(_position, toCopy).CopyTo(buffer);
                _position += toCopy;
                return toCopy;
            }
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            if (buffer is null) throw new ArgumentNullException(nameof(buffer));
            return Read(new Span<byte>(buffer, offset, count));
        }

        public override Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
            => Task.FromResult(Read(buffer, offset, count));

        public override ValueTask<int> ReadAsync(Memory<byte> buffer, CancellationToken cancellationToken = default)
            => new ValueTask<int>(Read(buffer.Span));

        public override IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
        {
            if (buffer is null) throw new ArgumentNullException(nameof(buffer));
            return BeginRead(new Memory<byte>(buffer, offset, count), callback, state);
        }

        public IAsyncResult BeginRead(Memory<byte> buffer, AsyncCallback callback, object state)
        {
            var read = Read(buffer.Span);
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
                if (_position == _memory.Length) return -1;
                var result = _memory.Span[_position++];
                return result;
            }
        }

        public override void CopyTo(Stream destination, int bufferSize)
        {
            lock (_lock)
            {
                if (_position == _memory.Length) return;

                var rented = ArrayPool<byte>.Shared.Rent(bufferSize);
                try
                {
                    ReadOnlySpan<byte> source = _memory.Span.Slice(_position);
                    for (var i = _position; i < _memory.Length; i += rented.Length)
                    {
                        var toCopy = Math.Min(source.Length, rented.Length);
                        source.Slice(0, toCopy).CopyTo(rented);
                        source = source.Slice(toCopy);

                        destination.Write(rented, 0, toCopy);
                        _position = i;
                    }

                    _position = _memory.Length;
                }
                finally
                {
                    ArrayPool<byte>.Shared.Return(rented);
                }
            }
        }

        public override async Task CopyToAsync(Stream destination, int bufferSize, CancellationToken cancellationToken)
        {
            var position = _position;
            if (position == _memory.Length) return;

            var rented = ArrayPool<byte>.Shared.Rent(bufferSize);
            try
            {
                ReadOnlyMemory<byte> source = _memory.Slice(position);
                for (var i = position; i < _memory.Length; i += rented.Length)
                {
                    var toCopy = Math.Min(source.Length, rented.Length);
                    source.Slice(0, toCopy).Span.CopyTo(rented);
                    source = source.Slice(toCopy);

                    await destination.WriteAsync(rented, 0, toCopy, cancellationToken).ConfigureAwait(false);
                    _position = i;
                }

                _position = _memory.Length;
            }
            finally
            {
                ArrayPool<byte>.Shared.Return(rented);
            }
        }

        private static Exception CreateNotSupportedException()
            => new NotSupportedException("The stream is not writeable.");

        public override void SetLength(long value)
            => throw CreateNotSupportedException();

        public override void Write(byte[] buffer, int offset, int count)
            => throw CreateNotSupportedException();

        public override Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
            => throw CreateNotSupportedException();

        public override void Write(ReadOnlySpan<byte> buffer)
            => throw CreateNotSupportedException();

        public override ValueTask WriteAsync(ReadOnlyMemory<byte> buffer, CancellationToken cancellationToken = default)
            => throw CreateNotSupportedException();

        public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
            => throw CreateNotSupportedException();

        public override void EndWrite(IAsyncResult asyncResult)
            => throw CreateNotSupportedException();

        public override void WriteByte(byte value)
            => throw CreateNotSupportedException();
    }
#endif
}
