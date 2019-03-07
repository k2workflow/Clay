#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using System;
using System.Buffers;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace SourceCode.Clay.Data.SqlParser
{
    internal sealed class SqlCharReader : IDisposable
    {
        private readonly Stack<IMemoryOwner<char>> _undo = new Stack<IMemoryOwner<char>>();
        private readonly TextReader _reader;
        private readonly bool _ownsReader;

        public SqlCharReader(string sql)
        {
            if (string.IsNullOrWhiteSpace(sql)) throw new ArgumentNullException(nameof(sql));

            _reader = new StringReader(sql);
            _ownsReader = true;
        }

        public SqlCharReader(TextReader reader)
        {
            _reader = reader ?? throw new ArgumentNullException(nameof(reader));
            _ownsReader = false;
        }

        /// <summary>
        /// Attempty to fill the provided buffer with (at least) the specified number of characters.
        /// </summary>
        /// <param name="buffer">The buffer to fill.</param>
        /// <param name="minimumCount">The minimum number of characters required.</param>
        /// <param name="count">The number of characters filled. This may be more (or less, if the source is empty) than requested.</param>
        public void FillLength(Span<char> buffer, int minimumCount, out int count)
        {
            Debug.Assert(minimumCount >= 0);
            Debug.Assert(minimumCount <= buffer.Length);

            count = 0;
            if (buffer.Length == 0 || minimumCount == 0) return;

            var offset = 0;
            while (true)
            {
                FillRemaining(buffer.Slice(offset), out var len);

                count += len;
                offset += len;

                if (len == 0 || count >= minimumCount) return;
            }
        }

        /// <summary>
        /// Fill the provided buffer with as many characters as are available, from the given offset.
        /// </summary>
        /// <param name="buffer">The buffer to fill.</param>
        /// <param name="offset">The offset in the buffer from where to start filling.</param>
        /// <param name="count">The number of characters filled. This may or may not fill the rest of buffer depending on whether the source is empty.</param>
        public void FillRemaining(Span<char> buffer, out int count)
        {
            count = 0;
            if (buffer.Length == 0) return;

            var desiredCount = buffer.Length;
            if (desiredCount == 0) return;

            // If anything in undo stack
            if (_undo.Count > 0)
            {
                using (IMemoryOwner<char> pop = _undo.Pop())
                {
                    Span<char> span = pop.Memory.Span;

                    // If it's longer than what we require
                    count = span.Length;
                    if (span.Length > desiredCount)
                    {
                        count = desiredCount;

                        // Push back the difference to the stack
                        var n = span.Length - desiredCount;
                        Undo(span.Slice(desiredCount, n));
                    }

                    // Copy requested data to output buffer
                    span.Slice(0, count).CopyTo(buffer);
                }
            }
            else
            {
                // Else read directly from input
                count = _reader.Read(buffer.Slice(0, desiredCount));
            }
        }

        /// <summary>
        /// Return the specified number of characters to the pool.
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        /// <param name="length"></param>
        public void Undo(ReadOnlySpan<char> buffer)
        {
            if (buffer.Length == 0) return;

            IMemoryOwner<char> owner = MemoryPool<char>.Shared.Rent(buffer.Length); // Rent
            owner = new SliceOwner<char>(owner, 0, buffer.Length); // Clamp

            buffer.CopyTo(owner.Memory.Span);

            _undo.Push(owner);
        }

        private sealed class SliceOwner<T> : IMemoryOwner<T>
        {
            private IMemoryOwner<T> _owner;
            public Memory<T> Memory { get; private set; }

            public SliceOwner(IMemoryOwner<T> owner, int start, int length)
            {
                _owner = owner;
                Memory = _owner.Memory.Slice(start, length);
            }

            public SliceOwner(IMemoryOwner<T> owner, int start)
            {
                _owner = owner;
                Memory = _owner.Memory.Slice(start);
            }

            public void Dispose()
            {
                if (_owner != null)
                {
                    _owner.Dispose();
                    _owner = null;
                }

                Memory = default;
            }
        }

        private bool _disposed;

        private void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    if (_ownsReader)
                        _reader.Dispose();

                    foreach (IMemoryOwner<char> undo in _undo)
                        undo.Dispose();
                }

                _disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
    }
}
