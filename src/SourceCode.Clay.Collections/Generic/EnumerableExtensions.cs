#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using System.Collections.Generic;
using System.Diagnostics;

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
        public static bool NullableSequenceEquals<TSource>(this IEnumerable<TSource> xe, IEnumerable<TSource> ye, IEqualityComparer<TSource> comparer = null)
        {
            if (xe is null) return ye is null; // (null, null) or (null, y)
            if (ye is null) return false; // (x, null)
            if (ReferenceEquals(xe, ye)) return true; // (x, x)

            var cmpr = comparer ?? EqualityComparer<TSource>.Default;

            // ICollection is more common
            if (xe is ICollection<TSource> xc)
            {
                // IList is more likely
                if (xe is IList<TSource> xl)
                {
                    return ListEqualsImpl(xl, ye, cmpr);
                }

                // IReadOnlyList
                if (xe is IReadOnlyList<TSource> xr)
                {
                    return ListEqualsImpl(xr, ye, cmpr);
                }
            }

            // IReadOnlyCollection
            else if (xe is IReadOnlyCollection<TSource> xrc)
            {
                // IReadOnlyList is more likely
                if (xe is IReadOnlyList<TSource> xr)
                {
                    return ListEqualsImpl(xr, ye, cmpr);
                }

                // IList
                if (xe is IList<TSource> xl)
                {
                    return ListEqualsImpl(xl, ye, cmpr);
                }
            }

            // IEnumerable
            var equal = System.Linq.Enumerable.SequenceEqual(xe, ye, cmpr);
            return equal;
        }

        private static bool ListEqualsImpl<TSource>(IList<TSource> xl, IEnumerable<TSource> ye, IEqualityComparer<TSource> comparer)
        {
            Debug.Assert(xl != null);
            Debug.Assert(ye != null);
            Debug.Assert(comparer != null);

            // ICollection is more likely
            if (ye is ICollection<TSource> yc)
            {
                if (xl.Count != yc.Count) return false; // (n, m)
                if (xl.Count == 0) return true; // (0, 0)

                var eq = ItemsEqualsImpl(xl, ye, comparer);
                return eq;
            }

            // IReadOnlyCollection
            if (ye is IReadOnlyCollection<TSource> yrc)
            {
                if (xl.Count != yrc.Count) return false; // (n, m)
                if (xl.Count == 0) return true; // (0, 0)

                var eq = ItemsEqualsImpl(xl, ye, comparer);
                return eq;
            }

            // IEnumerable
            var equal = System.Linq.Enumerable.SequenceEqual(xl, ye, comparer);
            return equal;
        }

        private static bool ListEqualsImpl<TSource>(IReadOnlyList<TSource> xr, IEnumerable<TSource> ye, IEqualityComparer<TSource> comparer)
        {
            Debug.Assert(xr != null);
            Debug.Assert(ye != null);
            Debug.Assert(comparer != null);

            // IReadOnlyCollection is more likely
            if (ye is IReadOnlyCollection<TSource> yrc)
            {
                if (xr.Count != yrc.Count) return false; // (n, m)
                if (xr.Count == 0) return true; // (0, 0)

                var eq = ItemsEqualsImpl(xr, ye, comparer);
                return eq;
            }

            // ICollection
            if (ye is ICollection<TSource> yc)
            {
                if (xr.Count != yc.Count) return false; // (n, m)
                if (xr.Count == 0) return true; // (0, 0)

                var eq = ItemsEqualsImpl(xr, ye, comparer);
                return eq;
            }

            // IEnumerable
            var equal = System.Linq.Enumerable.SequenceEqual(xr, ye, comparer);
            return equal;
        }

        private static bool ItemsEqualsImpl<TSource>(IList<TSource> xl, IEnumerable<TSource> ye, IEqualityComparer<TSource> comparer)
        {
            Debug.Assert(xl != null);
            Debug.Assert(ye != null);
            Debug.Assert(comparer != null);

            // IReadOnlyList is more likely
            if (ye is IReadOnlyList<TSource> yr)
            {
                // Check items in sequential order
                for (var i = 0; i < xl.Count; i++)
                {
                    if (!comparer.Equals(xl[i], yr[i])) return false;
                }

                return true;
            }

            // IList
            if (ye is IList<TSource> yl)
            {
                // Check items in sequential order
                for (var i = 0; i < xl.Count; i++)
                {
                    if (!comparer.Equals(xl[i], yl[i])) return false;
                }

                return true;
            }

            return false;
        }

        private static bool ItemsEqualsImpl<TSource>(IReadOnlyList<TSource> xr, IEnumerable<TSource> ye, IEqualityComparer<TSource> comparer)
        {
            Debug.Assert(xr != null);
            Debug.Assert(ye != null);
            Debug.Assert(comparer != null);

            // IReadOnlyList is more likely
            if (ye is IReadOnlyList<TSource> yr)
            {
                // Check items in sequential order
                for (var i = 0; i < xr.Count; i++)
                {
                    if (!comparer.Equals(xr[i], yr[i])) return false;
                }

                return true;
            }

            // IList
            if (ye is IList<TSource> yl)
            {
                // Check items in sequential order
                for (var i = 0; i < xr.Count; i++)
                {
                    if (!comparer.Equals(xr[i], yl[i])) return false;
                }

                return true;
            }

            return false;
        }
    }
}
