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
        /// <param name="xe">Input 1</param>
        /// <param name="ye">Input 2</param>
        /// <param name="comparer">The comparer to use to test for equality.</param>
        public static bool NullableSequenceEqual<TSource>(this IEnumerable<TSource> xe, IEnumerable<TSource> ye, IEqualityComparer<TSource> comparer = null)
        {
            if (xe is null) return ye is null; // (null, null) or (null, y)
            if (ye is null) return false; // (x, null)
            if (ReferenceEquals(xe, ye)) return true; // (x, x)

            return System.Linq.Enumerable.SequenceEqual(xe, ye, comparer);
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
            // TODO: Casting to covariant interface is up to 200x slower: https://github.com/dotnet/coreclr/issues/603
            var xN = xe is ICollection<TSource> xc ? xc.Count : (xe is IReadOnlyCollection<TSource> xrc ? xrc.Count : (int?)null);
            var yN = ye is ICollection<TSource> yc ? yc.Count : (ye is IReadOnlyCollection<TSource> yrc ? yrc.Count : (int?)null);

            // If both are some kind of collection
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
