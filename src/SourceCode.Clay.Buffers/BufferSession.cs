#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using SourceCode.Clay.IO;
using System;
using System.IO;

namespace SourceCode.Clay.Buffers
{
    /// <summary>
    /// A <see cref="System.IDisposable"/> wrapper for a <see cref="System.Byte"/> buffer,
    /// permitting automatic <see cref="ArrayPool{T}.Return(T[], bool)"/> behavior for
    /// buffers allocated via <see cref="ArrayPool{T}.Rent(int)"/>.
    /// </summary>
    /// <seealso cref="System.IDisposable" />
    public struct BufferSession : IEquatable<BufferSession>, IDisposable // MUST be a struct, in order to lessen GC load
    {
        private static readonly BufferSession _empty = new BufferSession(Array.Empty<byte>());

        /// <summary>
        /// Gets a reference to an empty <see cref="BufferSession"/>.
        /// </summary>
        public static ref readonly BufferSession Empty => ref _empty;

        #region Properties

        private ArraySegment<byte> _result;

        /// <summary>
        /// Gets the delineated result.
        /// </summary>
        /// <value>
        /// The result.
        /// </value>
        public ArraySegment<byte> Result => _result.Array == null
            ? new ArraySegment<byte>(Array.Empty<byte>())
            : _result;

        /// <summary>
        /// Returns the session as memory.
        /// </summary>
        public Memory<byte> Memory => Result;

        /// <summary>
        /// Returns the session as a span.
        /// </summary>
        public Span<byte> Span => Memory.Span;

        #endregion

        #region Constructors

        private BufferSession(ArraySegment<byte> result)
        {
            _result = result;
        }

        #endregion

        #region Factory

        /// <summary>
        /// Rents a buffer and wraps it in a <see cref="BufferSession"/>.
        /// </summary>
        /// <param name="minimumLength">The minimum length of the buffer.</param>
        /// <returns>The <see cref="BufferSession"/>.</returns>
        public static BufferSession Rent(int minimumLength)
        {
            if (minimumLength < 0) throw new ArgumentOutOfRangeException(nameof(minimumLength));
            if (minimumLength == 0) return Empty;

            var buffer = System.Buffers.ArrayPool<byte>.Shared.Rent(minimumLength);
            return new BufferSession(new ArraySegment<byte>(buffer, 0, minimumLength));
        }

        /// <summary>
        /// Wraps an external rented <see cref="ArraySegment{T}"/> in a <see cref="BufferSession"/>
        /// that will return the buffer to the pool when disposed.
        /// </summary>
        /// <param name="buffer">The rented buffer.</param>
        /// <returns>The <see cref="BufferSession"/>.</returns>
        public static BufferSession Rented(ArraySegment<byte> buffer)
        {
            return new BufferSession(buffer);
        }

        /// <summary>
        /// Wraps an external rented <see cref="ArraySegment{T}"/> in a <see cref="BufferSession"/>
        /// that will return the buffer to the pool when disposed.
        /// </summary>
        /// <param name="buffer">The rented buffer.</param>
        /// <param name="offset">The offset into <paramref name="buffer"/> where the result begins.</param>
        /// <param name="count">The length of the result.</param>
        /// <returns>The <see cref="BufferSession"/>.</returns>
        public static BufferSession Rented(byte[] buffer, int offset, int count)
        {
            if (buffer == null) throw new ArgumentNullException(nameof(buffer));
            return new BufferSession(new ArraySegment<byte>(buffer, offset, count));
        }

        #endregion

        #region IEquatable

        /// <inheritdoc/>
        public override bool Equals(object obj)
            => obj is BufferSession other
            && Equals(other);

        /// <inheritdoc/>
        public bool Equals(BufferSession other)
            => BufferComparer.Memory.Equals(Result, other.Result);

        /// <inheritdoc/>
        public override int GetHashCode()
            => BufferComparer.Memory.GetHashCode(Memory);

        #endregion

        #region Operators

        /// <summary>
        /// Transfers the buffer contained by this <see cref="BufferSession"/> to a
        /// new <see cref="Stream"/>.
        /// </summary>
        /// <returns>The stream containing the buffer.</returns>
        public Stream ToStream()
        {
            var stream = new BufferSessionStream(this);
            _result = default;
            return stream;
        }

        /// <summary>
        /// Transfers the buffer contained by this <see cref="BufferSession"/> to a
        /// new read-only <see cref="Stream"/>.
        /// </summary>
        /// <returns>The stream containing the buffer.</returns>
        public Stream ToReadOnlyStream()
        {
            var stream = new BufferSessionStream(this).MakeReadOnly();
            _result = default;
            return stream;
        }

        public static bool operator ==(BufferSession a, BufferSession b) => a.Equals(b);

        public static bool operator !=(BufferSession a, BufferSession b) => !(a == b);

#pragma warning disable CA2225 // Operator overloads have named alternates

        // Provided by properties and constructors

        public static implicit operator ArraySegment<byte>(BufferSession session) => session.Result;

        public static implicit operator Memory<byte>(BufferSession session) => session.Memory;

        public static implicit operator ReadOnlyMemory<byte>(BufferSession session) => session.Memory;

        public static implicit operator Span<byte>(BufferSession session) => session.Span;

        public static implicit operator ReadOnlySpan<byte>(BufferSession session) => session.Span;

#pragma warning restore CA2225 // Operator overloads have named alternates

        #endregion

        #region IDisposable

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            if (_result.Array == null) return;

            System.Buffers.ArrayPool<byte>.Shared.Return(_result.Array);
            _result = default;
        }

        #endregion
    }
}
