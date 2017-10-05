using System;

namespace SourceCode.Clay.Buffers
{
    /// <summary>
    ///   A <see cref="System.IDisposable"/> wrapper for a <see cref="System.Byte"/> buffer,
    ///   permitting automatic <see cref="ArrayPool{T}.Return(T[], bool)"/> behavior for buffers
    ///   allocated via <see cref="ArrayPool{T}.Rent(int)"/>.
    /// </summary>
    /// <seealso cref="System.IDisposable"/>
    public struct BufferSession : IDisposable // MUST be a struct, in order to lessen GC load
    {
        #region Properties

        /// <summary>
        ///   Gets the original rented buffer.
        /// </summary>
        /// <value>The buffer.</value>
        public byte[] Buffer { get; private set; }

        /// <summary>
        ///   Gets the delineated result ( <see cref="BufferSession.Buffer"/> may be overallocated).
        /// </summary>
        /// <value>The result.</value>
        public ArraySegment<byte> Result { get; }

        #endregion Properties

        #region Constructors

        /// <summary>
        ///   Initializes a new instance of the <see cref="BufferSession"/> struct.
        /// </summary>
        /// <param name="buffer">The buffer.</param>
        /// <param name="result">The result.</param>
        public BufferSession(byte[] buffer, ArraySegment<byte> result)
        {
            Buffer = buffer ?? throw new ArgumentNullException(nameof(buffer));
            Result = result;
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="BufferSession"/> struct.
        /// </summary>
        /// <param name="result">The result.</param>
        public BufferSession(ArraySegment<byte> result)
        {
            Buffer = null;
            Result = result;
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="BufferSession"/> struct.
        /// </summary>
        /// <param name="minimumLength">The minimum length.</param>
        public BufferSession(int minimumLength)
        {
            if (minimumLength < 0) throw new ArgumentOutOfRangeException(nameof(minimumLength));

            Buffer = RentBuffer(minimumLength);
            Result = default;
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        ///   Rents the buffer.
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
        ///   Returns the buffer.
        /// </summary>
        /// <param name="buffer">The buffer.</param>
        public static void ReturnBuffer(byte[] buffer)
        {
            if (buffer == null) throw new ArgumentNullException(nameof(buffer));

            System.Buffers.ArrayPool<byte>.Shared.Return(buffer);
        }

        #endregion Methods

        #region IDisposable

        /// <summary>
        ///   Performs application-defined tasks associated with freeing, releasing, or resetting
        ///   unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            if (Buffer == null)
                return;

            System.Buffers.ArrayPool<byte>.Shared.Return(Buffer);
            Buffer = null;
        }

        #endregion IDisposable
    }
}
