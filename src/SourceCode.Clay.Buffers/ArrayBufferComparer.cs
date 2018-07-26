#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using System;

namespace SourceCode.Clay.Buffers
{
    /// <summary>
    /// Represents a way to compare the contents of <see cref="Byte[]"/>.
    /// </summary>
    public sealed class ArrayBufferComparer : BufferComparer<byte[]>
    {
        /// <summary>
        /// Creates a new instance of the <see cref="ArrayBufferComparer"/> class, that considers the full
        /// buffer when calculating the hashcode.
        /// </summary>
        public ArrayBufferComparer()
        { }

        /// <summary>
        /// Creates a new instance of the <see cref="ArrayBufferComparer"/> class.
        /// </summary>
        /// <param name="hashCodeFidelity">
        /// The maximum number of octets processed when calculating a hashcode.
        /// Pass zero to disable the limit.
        /// </param>
        public ArrayBufferComparer(int hashCodeFidelity)
            : base(hashCodeFidelity)
        { }

        /// <summary>
        /// Compares two objects and returns a value indicating whether one is less than, equal to, or greater than the other.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>
        /// A signed integer that indicates the relative values of <paramref name="x" /> and <paramref name="y" />, as shown in the following table.Value Meaning Less than zero<paramref name="x" /> is less than <paramref name="y" />.Zero<paramref name="x" /> equals <paramref name="y" />.Greater than zero<paramref name="x" /> is greater than <paramref name="y" />.
        /// </returns>
        public override int Compare(byte[] x, byte[] y)
        {
            if (x == y) return 0; // (null, null) or (x, x)
            if (x is null) return -1; // (null, y)
            if (y is null) return 1; // (x, null)

            return MemoryBufferComparer.CompareSpan(x, y);
        }

        /// <inheritdoc/>
        public override bool Equals(byte[] x, byte[] y)
        {
            if (x == y) return true; // (null, null) or (x, x)
            if (x is null || y is null) return false; // (x, null) or (null, y)

            return MemoryBufferComparer.CompareSpan(x, y) == 0;
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.
        /// </returns>
        public override int GetHashCode(byte[] obj)
        {
            // Null
            if (obj is null) return 0;

            // Empty
            if (obj.Length == 0) return ByteHashCode.Empty;

            // Calculate on full length
            var span = obj.AsSpan();

            // Calculate on prefix
            if (HashCodeFidelity > 0 && obj.Length > HashCodeFidelity)
                span = span.Slice(0, HashCodeFidelity);

            var hc = ByteHashCode.Combine(span);
            return hc;
        }
    }
}
