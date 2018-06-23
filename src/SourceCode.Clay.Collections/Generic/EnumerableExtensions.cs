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
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Performs an optimized item-by-item comparison of two enumerables, using a custom <see cref="IEqualityComparer{T}"/>.
        /// The lists are required to have corresponding items in the same ordinal position.
        /// </summary>
        /// <typeparam name="TSource">The type of items.</typeparam>
        /// <param name="xe">Input 1</param>
        /// <param name="ye">Input 2</param>
        /// <param name="comparer">The comparer to use to test for equality.</param>
        /// <returns></returns>
        public static bool NullableSequenceEqual<TSource>(this IEnumerable<TSource> xe, IEnumerable<TSource> ye, IEqualityComparer<TSource> comparer = null)
        {
            if (xe is null) return ye is null; // (null, null) or (null, y)
            if (ye is null) return false; // (x, null)
            if (ReferenceEquals(xe, ye)) return true; // (x, x)

            var cmpr = comparer ?? EqualityComparer<TSource>.Default;

            // If both are some kind of collection
            if (BothAreCollections(xe, ye, out var xCount, out var yCount))
            {
                // Then we can immediately compare counts
                if (xCount != yCount) return false; // (n, m)
                if (xCount == 0) return true; // (0, 0)

                // IList is more common
                if (xe is IList<TSource> xl)
                {
                    var eq = MembersEqual(xl, ye, cmpr);
                    return eq;
                }

                // IReadOnlyList
                else if (xe is IReadOnlyList<TSource> xrl)
                {
                    var eq = MembersEqual(xrl, ye, cmpr);
                    return eq;
                }
            }

            // Else resort to an IEnumerable comparison
            var equal = EnumerableEqual(xe, ye, cmpr);
            return equal;
        }

        /// <summary>
        /// If both inputs are some kind of collection, then output their relative counts.
        /// </summary>
        /// <typeparam name="TSource">The type of items.</typeparam>
        /// <param name="xe">Input 1</param>
        /// <param name="ye">Input 2</param>
        /// <param name="xCount">The Count of members in input 1.</param>
        /// <param name="yCount">The Count of members in input 2.</param>
        /// <returns></returns>
        internal static bool BothAreCollections<TSource>(IEnumerable<TSource> xe, IEnumerable<TSource> ye, out int xCount, out int yCount)
        {
            // Try get item counts
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

        /// <summary>
        /// Check equality by iterating through two lists and comparing colocated members.
        /// </summary>
        /// <typeparam name="TSource">The type of items.</typeparam>
        /// <param name="xl">Input 1</param>
        /// <param name="ye">Input 2</param>
        /// <param name="comparer">The comparer to use to test for equality.</param>
        /// <returns></returns>
        private static bool MembersEqual<TSource>(IList<TSource> xl, IEnumerable<TSource> ye, IEqualityComparer<TSource> comparer)
        {
            // IList is more likely
            if (ye is IList<TSource> yl)
            {
                for (var i = 0; i < xl.Count; i++)
                    if (!comparer.Equals(xl[i], yl[i]))
                        return false;

                return true;
            }

            // IReadOnlyList
            if (ye is IReadOnlyList<TSource> yrl)
            {
                for (var i = 0; i < xl.Count; i++)
                    if (!comparer.Equals(xl[i], yrl[i]))
                        return false;

                return true;
            }

            return false;
        }

        /// <summary>
        /// Check equality by iterating through two lists and comparing colocated members.
        /// </summary>
        /// <typeparam name="TSource">The type of items.</typeparam>
        /// <param name="xrl">Input 1</param>
        /// <param name="ye">Input 2</param>
        /// <param name="comparer">The comparer to use to test for equality.</param>
        /// <returns></returns>
        private static bool MembersEqual<TSource>(IReadOnlyList<TSource> xrl, IEnumerable<TSource> ye, IEqualityComparer<TSource> comparer)
        {
            // IReadOnlyList is more likely
            if (ye is IReadOnlyList<TSource> yrl)
            {
                for (var i = 0; i < xrl.Count; i++)
                    if (!comparer.Equals(xrl[i], yrl[i]))
                        return false;

                return true;
            }

            // IList
            if (ye is IList<TSource> yl)
            {
                for (var i = 0; i < xrl.Count; i++)
                    if (!comparer.Equals(xrl[i], yl[i]))
                        return false;

                return true;
            }

            return false;
        }

        /// <summary>
        /// Check equality by enumerating two sequences and comparing colocated members.
        /// </summary>
        /// <typeparam name="TSource">The type of items.</typeparam>
        /// <param name="xe">Input 1</param>
        /// <param name="ye">Input 2</param>
        /// <param name="comparer">The comparer to use to test for equality.</param>
        /// <returns></returns>
        private static bool EnumerableEqual<TSource>(IEnumerable<TSource> xe, IEnumerable<TSource> ye, IEqualityComparer<TSource> comparer)
        {
            using (var xi = xe.GetEnumerator())
            using (var yi = ye.GetEnumerator())
            {
                while (xi.MoveNext())
                {
                    if (!yi.MoveNext()) return false;

                    if (!comparer.Equals(xi.Current, yi.Current)) return false;
                }

                return !yi.MoveNext();
            }
        }
    }
}
