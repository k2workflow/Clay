#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using System;

namespace SourceCode.Clay.Buffers
{
    /// <summary>
    /// A <see cref="System.IDisposable"/> wrapper for a <see cref="System.Byte"/> buffer,
    /// permitting automatic <see cref="ArrayPool{T}.Return(T[], bool)"/> behavior for
    /// buffers allocated via <see cref="ArrayPool{T}.Rent(int)"/>.
    /// </summary>
    /// <seealso cref="System.IDisposable" />
    public readonly struct BufferSession : IEquatable<BufferSession>, IDisposable // MUST be a struct, in order to lessen GC load
    {
        #region Properties

        /// <summary>
        /// Gets the delineated result (<see cref="BufferSession.Buffer"/> may be overallocated).
        /// </summary>
        /// <value>
        /// The result.
        /// </value>
        public ArraySegment<byte> Result { get; }

        /// <summary>
        /// Gets the original rented buffer.
        /// </summary>
        /// <value>
        /// The buffer.
        /// </value>
        public byte[] Buffer { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BufferSession"/> struct.
        /// </summary>
        /// <param name="buffer">The buffer.</param>
        /// <param name="result">The result.</param>
        public BufferSession(byte[] buffer, ArraySegment<byte> result)
        {
            Buffer = buffer ?? throw new ArgumentNullException(nameof(buffer));
            Result = result;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BufferSession"/> struct.
        /// </summary>
        /// <param name="result">The result.</param>
        public BufferSession(ArraySegment<byte> result)
        {
            Buffer = null;
            Result = result;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BufferSession" /> struct.
        /// </summary>
        /// <param name="minimumLength">The minimum length.</param>
        public BufferSession(int minimumLength)
        {
            if (minimumLength < 0) throw new ArgumentOutOfRangeException(nameof(minimumLength));

            Buffer = RentBuffer(minimumLength);
            Result = default;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Rents the buffer.
        /// </summary>
        /// <param name="minimumLength">The minimum length.</param>
        /// <returns></returns>
        public static byte[] RentBuffer(int minimumLength)
        {
            if (minimumLength < 0) throw new ArgumentOutOfRangeException(nameof(minimumLength));

            var buffer = System.Buffers.ArrayPool<byte>.Shared.Rent(minimumLength);
            return buffer;
        }

        /// <summary>
        /// Returns the buffer.
        /// </summary>
        /// <param name="buffer">The buffer.</param>
        public static void ReturnBuffer(byte[] buffer)
        {
            if (buffer == null) throw new ArgumentNullException(nameof(buffer));

            System.Buffers.ArrayPool<byte>.Shared.Return(buffer);
        }

        #endregion

        #region IEquatable

        /// <inheritdoc/>
        public override bool Equals(object obj)
            => obj is BufferSession other
            && Equals(other);

        /// <inheritdoc/>
        public bool Equals(BufferSession other)
        {
            if (!BufferComparer.Array.Equals(Buffer, other.Buffer)) return false;
            if (!BufferComparer.Memory.Equals(Result, other.Result)) return false;

            return true;
        }

        /// <inheritdoc/>
        public override int GetHashCode() => BufferComparer.Array.GetHashCode(Buffer);

        #endregion

        #region Operators

        public static bool operator ==(BufferSession a, BufferSession b) => a.Equals(b);

        public static bool operator !=(BufferSession a, BufferSession b) => !(a == b);

        #endregion

        #region IDisposable

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            if (Buffer == null)
                return;

            System.Buffers.ArrayPool<byte>.Shared.Return(Buffer);
        }

        #endregion
    }
}
