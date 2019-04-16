#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using System.Collections.Generic;

namespace SourceCode.Clay.Collections.Generic
{
    /// <summary>
    /// Represents extensions for <see cref="IEnumerable{T}"/>.
    /// </summary>
    internal static class EnumerableExtensions
    {
        /// <summary>
        /// Performs an optimized item-by-item comparison of two enumerables, using a custom <see cref="IEqualityComparer{T}"/>.
        /// The lists are required to have corresponding items in the same ordinal position.
        /// </summary>
        /// <typeparam name="TSource">The type of items.</typeparam>
        /// <param name="first">Input 1</param>
        /// <param name="second">Input 2</param>
        /// <param name="comparer">The comparer to use to test for equality.</param>
        public static bool NullableSequenceEqual<TSource>(this IEnumerable<TSource> first, IEnumerable<TSource> second, IEqualityComparer<TSource> comparer = null)
        {
            if (first is null) return second is null; // (null, null) or (null, y)
            if (second is null) return false; // (x, null)
            if (ReferenceEquals(first, second)) return true; // (x, x)

            return System.Linq.Enumerable.SequenceEqual(first, second, comparer);
        }

        /// <summary>
        /// If both inputs are some kind of collection, then output their relative counts.
        /// </summary>
        /// <typeparam name="TSource">The type of items.</typeparam>
        /// <param name="xe">Input 1</param>
        /// <param name="ye">Input 2</param>
        /// <param name="xCount">The Count of members in input 1.</param>
        /// <param name="yCount">The Count of members in input 2.</param>
        internal static bool BothAreCollections<TSource>(IEnumerable<TSource> xe, IEnumerable<TSource> ye, out int xCount, out int yCount)
        {
            // Try get item counts
            var xN = xe is ICollection<TSource> xc ? xc.Count : (int?)null;
            var yN = ye is ICollection<TSource> yc ? yc.Count : (int?)null;

            if (xN.HasValue && yN.HasValue)
            {
                xCount = xN.Value;
                yCount = yN.Value;
                return true;
            }

            xCount = 0;
            yCount = 0;
            return false;
        }
    }
}
