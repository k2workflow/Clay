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
        private readonly Stack<ArraySegment<char>> _undo = new Stack<ArraySegment<char>>();
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
        public void FillLength(char[] buffer, int minimumCount, out int count)
        {
            Debug.Assert(!(buffer is null));
            Debug.Assert(minimumCount >= 0);
            Debug.Assert(minimumCount <= buffer.Length);

            count = 0;
            if (buffer.Length == 0 || minimumCount == 0) return;

            var offset = 0;
            while (true)
            {
                FillRemaining(buffer, offset, out var len);

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
        public void FillRemaining(char[] buffer, int offset, out int count)
        {
            Debug.Assert(!(buffer is null));
            Debug.Assert(offset >= 0);
            Debug.Assert(offset < buffer.Length);

            count = 0;
            if (buffer.Length == 0) return;

            var desiredCount = buffer.Length - offset;
            if (desiredCount == 0) return;

            // If anything in undo stack
            if (_undo.Count > 0)
            {
                // Pop top undo
                var pop = _undo.Pop();

                // If it's longer than what we require
                count = pop.Count;
                if (pop.Count > desiredCount)
                {
                    count = desiredCount;

                    // Push back the difference to the stack
                    var n = pop.Count - desiredCount;
                    Undo(pop.Array, desiredCount, n);
                }

                // Copy requested data to output buffer
                Array.Copy(pop.Array, 0, buffer, offset, count);

                // Return the rental
                ArrayPool<char>.Shared.Return(pop.Array);
            }
            else
            {
                // Else read directly from input
                count = _reader.Read(buffer, offset, desiredCount);
            }
        }

        /// <summary>
        /// Return the specified number of characters to the pool.
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        /// <param name="length"></param>
        public void Undo(char[] buffer, int offset, int length)
        {
            if (length == 0) return;

            Debug.Assert(!(buffer is null));
            Debug.Assert(buffer.Length > 0);
            Debug.Assert(offset >= 0);
            Debug.Assert(length >= 0);
            Debug.Assert(offset + length <= buffer.Length);

            var rented = ArrayPool<char>.Shared.Rent(length); // Rental may be longer than requested...
            Array.Copy(buffer, offset, rented, 0, length);

            var seg = new ArraySegment<char>(rented, 0, length); // ...so delineate it
            _undo.Push(seg);
        }

        private bool _disposed;

        private void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing
                    && _ownsReader)
                {
                    _reader.Dispose();
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
