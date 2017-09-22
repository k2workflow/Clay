using System;
using System.Collections.Generic;

namespace SourceCode.Clay.Buffers
{
    /// <summary>
    /// Represents a way to compare binary buffers.
    /// </summary>
    public static class BufferComparer
    {
        #region Constants

        /// <summary>
        /// Gets the number of octets that will be processed when calculating a hashcode.
        /// </summary>
        public const int DefaultHashCodeFidelity = 512;

        /// <summary>
        /// Gets the default instance of the <see cref="ReadOnlySpan{T}"/> buffer comparer that uses FNV.
        /// </summary>
        /// <value>
        /// The default instance of the buffer comparer that uses FNV.
        /// </value>
        public static BufferComparer<ReadOnlySpan<byte>> DefaultComparer { get; } = new ReadOnlySpanComparer(DefaultHashCodeFidelity);

        /// <summary>
        /// Gets the default instance of the <see cref="byte[]"/> buffer comparer that uses FNV.
        /// </summary>
        /// <value>
        /// The default instance of the buffer comparer that uses FNV.
        /// </value>
        public static BufferComparer<byte[]> DefaultArrayComparer { get; } = new ArrayComparer(DefaultHashCodeFidelity);

        /// <summary>
        /// Gets the default instance of the <see cref="ArraySegment{T}"/> buffer comparer that uses FNV.
        /// </summary>
        /// <value>
        /// The default instance of the buffer comparer that uses FNV.
        /// </value>
        public static BufferComparer<ArraySegment<byte>> DefaultArraySegmentComparer { get; } = new ArraySegmentComparer(DefaultHashCodeFidelity);

        /// <summary>
        /// Gets the default instance of the <see cref="IList{T}"/> buffer comparer that uses FNV.
        /// </summary>
        /// <value>
        /// The default instance of the buffer comparer that uses FNV.
        /// </value>
        public static BufferComparer<IList<byte>> DefaultListComparer { get; } = new ListComparer(DefaultHashCodeFidelity);

        /// <summary>
        /// Gets the default instance of the <see cref="IReadOnlyList{T}"/> buffer comparer that uses FNV.
        /// </summary>
        /// <value>
        /// The default instance of the buffer comparer that uses FNV.
        /// </value>
        public static BufferComparer<IReadOnlyList<byte>> DefaultReadOnlyListComparer { get; } = new ReadOnlyListComparer(DefaultHashCodeFidelity);

        /// <summary>
        /// Gets the default instance of the <see cref="IEnumerable{T}"/> buffer comparer that uses FNV.
        /// </summary>
        /// <value>
        /// The default instance of the buffer comparer that uses FNV.
        /// </value>
        public static BufferComparer<IEnumerable<byte>> DefaultEnumerableComparer { get; } = new EnumerableComparer(DefaultHashCodeFidelity);

        #endregion
    }
}
