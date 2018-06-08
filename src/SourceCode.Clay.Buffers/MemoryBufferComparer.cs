#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using System;

namespace SourceCode.Clay.Buffers
{
    /// <summary>
    /// Represents a way to compare the contents of <see cref="ReadOnlyMemory{T}"/> buffers.
    /// </summary>
    public sealed class MemoryBufferComparer : BufferComparer<ReadOnlyMemory<byte>>
    {
        /// <summary>
        /// Creates a new instance of the <see cref="MemoryBufferComparer"/> class, that considers the full
        /// buffer when calculating the hashcode.
        /// </summary>
        public MemoryBufferComparer()
        { }

        /// <summary>
        /// Creates a new instance of the <see cref="MemoryBufferComparer"/> class.
        /// </summary>
        /// <param name="hashCodeFidelity">
        /// The maximum number of octets processed when calculating a hashcode.
        /// Pass zero to disable the limit.
        /// </param>
        public MemoryBufferComparer(int hashCodeFidelity)
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
        public override int Compare(ReadOnlyMemory<byte> x, ReadOnlyMemory<byte> y) => CompareSpan(x.Span, y.Span);

        /// <inheritdoc/>
        public override bool Equals(ReadOnlyMemory<byte> x, ReadOnlyMemory<byte> y) => CompareSpan(x.Span, y.Span) == 0;

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.
        /// </returns>
        public override int GetHashCode(ReadOnlyMemory<byte> obj)
        {
            // Empty
            if (obj.Length == 0) return ArrayBufferComparer.EmptyHashCode;

            // Calculate on full length
            var span = obj.Span;

            // Calculate on prefix
            if (HashCodeFidelity > 0 && obj.Length > HashCodeFidelity)
                span = span.Slice(0, HashCodeFidelity);

            var hc = ByteHashCode.Combine(span);
            return hc;
        }

        /// <summary>
        /// Compare the contexts of two <see cref="ReadOnlySpan{T}"/> buffers.
        /// </summary>
        /// <param name="x">Span 1</param>
        /// <param name="y">Span 2</param>
        /// <returns></returns>
        public static int CompareSpan(in ReadOnlySpan<byte> x, in ReadOnlySpan<byte> y)
        {
            // From https://github.com/dotnet/corefx/blob/master/src/System.Memory/src/System/ReadOnlySpan.cs
            // public static bool operator ==
            // Returns true if left and right point at the same memory and have the same length.
            // Note that this does *not* check to see if the *contents* are equal.
            if (x == y) return 0;

            var cmp = x.Length.CompareTo(y.Length);
            if (cmp != 0) return cmp; // ([n], [m])

            // ([n], [n])
            switch (x.Length)
            {
                case 0:
                    return 0;

                case 1:
                    cmp = x[0].CompareTo(y[0]);
                    return cmp;

                case 2:
                    cmp = x[0].CompareTo(y[0]);
                    if (cmp != 0) return cmp;

                    cmp = x[1].CompareTo(y[1]);
                    return cmp;

                case 3:
                    cmp = x[0].CompareTo(y[0]);
                    if (cmp != 0) return cmp;

                    cmp = x[1].CompareTo(y[1]);
                    if (cmp != 0) return cmp;

                    cmp = x[2].CompareTo(y[2]);
                    return cmp;

                default:
                    {
                        unsafe
                        {
                            fixed (byte* xp = x)
                            fixed (byte* yp = y)
                            {
                                cmp = NativeMethods.MemCompare(xp, yp, x.Length);
                                return cmp;
                            }
                        }
                    }
            }
        }
    }
}
