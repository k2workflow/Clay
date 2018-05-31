#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using System;
using System.Runtime.InteropServices;

namespace SourceCode.Clay.Buffers
{
    /// <summary>
    /// Represents a way to calculate hash codes using the Fnv-1 algorithm.
    /// </summary>
    public static class FnvHashCode
    {
        #region Constants

        private const uint FnvOffsetBasis = 0x811C9DC5u;
        private const uint FnvPrime = 0x01000193u;

        // Same result as full calculation would give for an empty buffer
        internal const int FnvEmpty = unchecked((int)FnvOffsetBasis); // Marked internal for Unit visibility

        // Chosen hash for a null input.
        internal const int FnvNull = 0; // Marked internal for Unit visibility

        #endregion

        #region Core

        /// <summary>
        /// Calculates a hash code from the specified data using the
        /// Fowler/Noll/Vo 1-a algorithm.
        /// </summary>
        /// <param name="buffer">The <see cref="ReadOnlySpan{T}"/> containing the range of bytes to include in the hash code.</param>
        /// <returns>
        /// The hash code.
        /// </returns>
        public static int Combine(in ReadOnlySpan<byte> span)
        {
            if (span.Length == 0) return FnvEmpty;

            unchecked
            {
                var hc = FnvOffsetBasis;

                for (var i = 0; i < span.Length; i++)
                {
                    var c = span[i];
                    hc = (hc ^ c) * FnvPrime;
                }

                return (int)hc;
            }
        }

        #endregion

        #region Overloads

        /// <summary>
        /// Calculates a hash code from the specified data using the
        /// Fowler/Noll/Vo 1-a algorithm.
        /// </summary>
        /// <param name="a">The first hash code to include in the hash code.</param>
        /// <param name="b">The second hash code to include in the hash code.</param>
        /// <returns>
        /// The hash code.
        /// </returns>
        public static int Combine(int a, int b)
        {
            Span<int> data = stackalloc int[2];
            data[0] = a;
            data[1] = b;

            var hc = Combine(MemoryMarshal.Cast<int, byte>(data));
            return hc;
        }

        /// <summary>
        /// Calculates a hash code from the specified data using the
        /// Fowler/Noll/Vo 1-a algorithm.
        /// </summary>
        /// <param name="a">The first hash code to include in the hash code.</param>
        /// <param name="b">The second hash code to include in the hash code.</param>
        /// <param name="c">The third hash code to include in the hash code.</param>
        /// <returns>
        /// The hash code.
        /// </returns>
        public static int Combine(int a, int b, int c)
        {
            Span<int> data = stackalloc int[3];
            data[0] = a;
            data[1] = b;
            data[2] = c;

            var hc = Combine(MemoryMarshal.Cast<int, byte>(data));
            return hc;
        }

        /// <summary>
        /// Calculates a hash code from the specified data using the
        /// Fowler/Noll/Vo 1-a algorithm.
        /// </summary>
        /// <param name="a">The first hash code to include in the hash code.</param>
        /// <param name="b">The second hash code to include in the hash code.</param>
        /// <param name="c">The third hash code to include in the hash code.</param>
        /// <param name="d">The fourth hash code to include in the hash code.</param>
        /// <returns>
        /// The hash code.
        /// </returns>
        public static int Combine(int a, int b, int c, int d)
        {
            Span<int> data = stackalloc int[4];
            data[0] = a;
            data[1] = b;
            data[2] = c;
            data[3] = d;

            var hc = Combine(MemoryMarshal.Cast<int, byte>(data));
            return hc;
        }

        /// <summary>
        /// Calculates a hash code from the specified data using the
        /// Fowler/Noll/Vo 1-a algorithm.
        /// </summary>
        /// <param name="a">The first hash code to include in the hash code.</param>
        /// <param name="b">The second hash code to include in the hash code.</param>
        /// <param name="c">The third hash code to include in the hash code.</param>
        /// <param name="d">The fourth hash code to include in the hash code.</param>
        /// <param name="e">The fifth hash code to include in the hash code.</param>
        /// <returns>
        /// The hash code.
        /// </returns>
        public static int Combine(int a, int b, int c, int d, int e)
        {
            Span<int> data = stackalloc int[5];
            data[0] = a;
            data[1] = b;
            data[2] = c;
            data[3] = d;
            data[4] = e;

            var hc = Combine(MemoryMarshal.Cast<int, byte>(data));
            return hc;
        }

        #endregion
    }
}
