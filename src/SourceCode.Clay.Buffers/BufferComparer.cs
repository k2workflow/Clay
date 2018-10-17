#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using System;

namespace SourceCode.Clay.Buffers
{
    /// <summary>
    /// Represents a way to compare binary buffers.
    /// </summary>
    public static class BufferComparer
    {
        /// <summary>
        /// The prefix of octets processed when calculating a hashcode.
        /// </summary>
        public const int DefaultHashCodeFidelity = 512;

        /// <summary>
        /// Gets the default instance of the byte[] buffer comparer that uses FNV with default fidelity.
        /// </summary>
        /// <value>
        /// The default instance of the buffer comparer that uses FNV.
        /// </value>
        public static BufferComparer<byte[]> Array { get; } = new ArrayBufferComparer(DefaultHashCodeFidelity);

        /// <summary>
        /// Gets the default instance of the <see cref="ReadOnlyMemory{T}"/> buffer comparer that uses FNV with default fidelity.
        /// This also supports comparison of byte[] and ArraySegment due to their implicit conversion to <see cref="ReadOnlyMemory{T}"/>.
        /// </summary>
        /// <value>
        /// The default instance of the buffer comparer that uses FNV.
        /// </value>
        public static BufferComparer<ReadOnlyMemory<byte>> Memory { get; } = new MemoryBufferComparer(DefaultHashCodeFidelity);
    }
}
