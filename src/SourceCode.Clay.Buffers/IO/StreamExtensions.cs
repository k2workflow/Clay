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
    /// <summary>
    /// Represents buffer extensions for <see cref="Stream"/>.
    /// </summary>
    public static class StreamExtensions
    {
        #region Write

        /// <summary>
        /// Writes the specified <see cref="ReadOnlyMemory{T}" /> to the specified
        /// <see cref="Stream" />.
        /// </summary>
        /// <param name="stream">The stream to write to.</param>
        /// <param name="memory">The memory to write.</param>
        /// <param name="bufferLength">The maximum length of the buffer. The default is 81920.</param>
        public static void Write(this Stream stream, in ReadOnlyMemory<byte> memory, int bufferLength = 81920)
            => Write(stream, memory.Span, bufferLength);

        /// <summary>
        /// Writes the specified <see cref="ReadOnlySpan{T}" /> to the specified
        /// <see cref="Stream" />.
        /// </summary>
        /// <param name="stream">The stream to write to.</param>
        /// <param name="span">The span to write.</param>
        /// <param name="bufferLength">The maximum length of the buffer. The default is 81920.</param>
        public static void Write(this Stream stream, in ReadOnlySpan<byte> span, int bufferLength = 81920)
        {
            if (stream == null) throw new ArgumentNullException(nameof(stream));
            if (bufferLength < 1) throw new ArgumentOutOfRangeException(nameof(bufferLength));
            if (span.Length == 0) return;

            bufferLength = Math.Min(bufferLength, span.Length);
            var ba = ArrayPool<byte>.Shared.Rent(bufferLength);
            try
            {
                var offset = 0;
                while (offset < span.Length)
                {
                    var toCopy = Math.Min(bufferLength, span.Length - offset);
                    span.Slice(offset, toCopy).CopyTo(ba);
                    stream.Write(ba, 0, toCopy);
                    offset += toCopy;
                }
            }
            finally
            {
                ArrayPool<byte>.Shared.Return(ba);
            }
        }

        /// <summary>
        /// Asynchronously writes the specified <see cref="ReadOnlyMemory{T}{T}" /> to the specified
        /// <see cref="Stream" />.
        /// </summary>
        /// <param name="stream">The stream to write to.</param>
        /// <param name="memory">The memory to write.</param>
        /// <param name="bufferLength">The maximum length of the buffer. The default is 81920.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        public static Task WriteAsync(this Stream stream, ReadOnlyMemory<byte> memory, int bufferLength = 81920, CancellationToken cancellationToken = default)
        {
            if (stream == null) throw new ArgumentNullException(nameof(stream));
            if (bufferLength < 1) throw new ArgumentOutOfRangeException(nameof(bufferLength));

            async Task Impl()
            {
                bufferLength = Math.Min(bufferLength, memory.Length);
                var ba = ArrayPool<byte>.Shared.Rent(bufferLength);
                try
                {
                    var offset = 0;
                    while (offset < memory.Length)
                    {
                        var toCopy = Math.Min(bufferLength, memory.Length - offset);
                        memory.Span.Slice(offset, toCopy).CopyTo(ba);
                        await stream.WriteAsync(ba, 0, toCopy, cancellationToken);
                        offset += toCopy;
                    }
                }
                finally
                {
                    ArrayPool<byte>.Shared.Return(ba);
                }
            }

            if (memory.Length == 0) return Task.CompletedTask;
            return Impl();
        }

        #endregion
    }
}
