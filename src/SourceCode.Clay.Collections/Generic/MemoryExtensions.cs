#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using System;
using System.Collections;
using System.Collections.Generic;

namespace SourceCode.Clay.Collections.Generic
{
    /// <summary>
    /// Represents extensions for <see cref="ReadOnlyMemory{T}"/>.
    /// </summary>
    public static class MemoryExtensions
    {
        #region Methods

        /// <summary>
        /// Performs an optimized item-by-item comparison, using a custom <see cref="IEqualityComparer{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of items.</typeparam>
        /// <param name="x">Memory 1</param>
        /// <param name="y">Memory 2</param>
        /// <param name="comparer">The comparer to use to test for equality.</param>
        /// <param name="sequential">Optimizes the algorithm for cases when the inputs are expected to be ordered in the same manner.</param>
        /// <returns></returns>
        public static bool MemoryEquals<T>(this ReadOnlyMemory<T> x, ReadOnlyMemory<T> y, IEqualityComparer<T> comparer, bool sequential)
        {
            // Memory is a struct
            if (comparer == null) throw new ArgumentNullException(nameof(comparer));

            // If counts are different, not equal
            if (x.Length != y.Length) return false;

            // If first count is 0 then, due to previous check, the second is guaranteed to be 0 (and thus equal)
            if (x.IsEmpty) return true;

            // Memory<T>.Span throws if IsEmpty == true
            var xs = x.Span;
            var ys = y.Span;

            // Optimize for cases 0, 1, 2, N
            switch (xs.Length)
            {
                // If there is only 1 item, short-circuit
                case 1: return comparer.Equals(xs[0], ys[0]);

                // If there are 2 items, short-circuit
                case 2:
                    {
                        // Horizontal
                        if (comparer.Equals(xs[0], ys[0]))
                            return comparer.Equals(xs[1], ys[1]);

                        // Diagonal
                        if (comparer.Equals(xs[0], ys[1]))
                            return comparer.Equals(xs[1], ys[0]);
                    }
                    return false;

                // Else we need to do more work
                default: break;
            }

            var min = 0;
            var max = xs.Length - 1;
            var bit = new BitArray(xs.Length); // Optimize looping by tracking which positions have been matched

            for (var i = 0; i < xs.Length; i++)
            {
                // Colocated comparisons should be at the same position
                if (sequential
                    && !bit[i]
                    && comparer.Equals(xs[i], ys[i]))
                {
                    bit[i] = true;
                    if (i == min) min++;

                    continue;
                }

                var found = false;

                var j = min;
                for (; j <= max; j++)
                {
                    // Skip positions where a match was previously found
                    if (bit[j]) continue;

                    if (comparer.Equals(xs[i], ys[j]))
                    {
                        found = true;

                        bit[j] = true;
                        if (j == min) min++;
                        if (j == max) max--;

                        break;
                    }
                }

                if (!found) return false;
            }

            return true;
        }

        /// <summary>
        /// Performs an optimized item-by-item comparison, using the default comparer for the type.
        /// </summary>
        /// <typeparam name="T">The type of items.</typeparam>
        /// <param name="x">Memory 1</param>
        /// <param name="y">Memory 2</param>
        /// <param name="sequential">Optimizes the algorithm for cases when the inputs are expected to be ordered in the same manner.</param>
        /// <returns></returns>
        public static bool MemoryEquals<T>(this ReadOnlyMemory<T> x, ReadOnlyMemory<T> y, bool sequential)
            => x.MemoryEquals(y, EqualityComparer<T>.Default, sequential);

        #endregion
    }
}
