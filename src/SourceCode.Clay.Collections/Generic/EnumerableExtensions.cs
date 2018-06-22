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
        /// Performs an optimized item-by-item comparison, using a custom <see cref="IEqualityComparer{T}"/>.
        /// The lists are required to have corresponding items in the same ordinal position.
        /// </summary>
        /// <typeparam name="TSource">The type of items.</typeparam>
        /// <param name="xe">List 1</param>
        /// <param name="ye">List 2</param>
        /// <param name="comparer">The comparer to use to test for equality.</param>
        /// <returns></returns>
        public static bool NullableSequenceEqual<TSource>(this IEnumerable<TSource> xe, IEnumerable<TSource> ye, IEqualityComparer<TSource> comparer = null)
        {
            if (xe is null) return ye is null; // (null, null) or (null, y)
            if (ye is null) return false; // (x, null)
            if (ReferenceEquals(xe, ye)) return true; // (x, x)

            var cmpr = comparer ?? EqualityComparer<TSource>.Default;

            // Try get member counts
            var xCount = xe is ICollection<TSource> xc ? xc.Count : (xe is IReadOnlyCollection<TSource> xrc ? xrc.Count : (int?)null);
            var yCount = ye is ICollection<TSource> yc ? yc.Count : (ye is IReadOnlyCollection<TSource> yrc ? yrc.Count : (int?)null);

            // If both are some kind of collection
            if (xCount.HasValue && yCount.HasValue)
            {
                // Then we can immediately compare counts
                if (xCount.Value != yCount.Value) return false; // (n, m)
                if (xCount.Value == 0) return true; // (0, 0)

                // IList is more common
                if (xe is IList<TSource> xl)
                {
                    var eq = ItemsEqual(xl, ye, cmpr);
                    return eq;
                }

                // IReadOnlyList
                else if (xe is IReadOnlyList<TSource> xrl)
                {
                    var eq = ItemsEqual(xrl, ye, cmpr);
                    return eq;
                }
            }

            // Else resort to an IEnumerable comparison
            var equal = EnumerableEqual(xe, ye, cmpr);
            return equal;
        }

        // Check items in sequential order
        private static bool ItemsEqual<TSource>(IList<TSource> xl, IEnumerable<TSource> ye, IEqualityComparer<TSource> comparer)
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

        // Check items in sequential order
        private static bool ItemsEqual<TSource>(IReadOnlyList<TSource> xrl, IEnumerable<TSource> ye, IEqualityComparer<TSource> comparer)
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
