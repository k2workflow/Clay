#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using System.Collections.Generic;

namespace SourceCode.Clay.Collections.Generic
{
    /// <summary>
    /// Represents extensions for <see cref="ISet{T}"/>.
    /// </summary>
    public static class SetExtensions
    {
        /// <summary>
        /// Performs an efficient item-by-item comparison, using a custom <see cref="IEqualityComparer{T}"/>.
        /// </summary>
        /// <typeparam name="TSource">The type of items.</typeparam>
        /// <param name="xe">Set 1</param>
        /// <param name="ye">Set 2</param>
        /// <param name="comparer">The comparer to use to test for equality.</param>
        /// <returns></returns>
        public static bool NullableSetEquals<TSource>(this IEnumerable<TSource> xe, IEnumerable<TSource> ye, in IEqualityComparer<TSource> comparer = null)
        {
            if (xe is null) return ye is null; // (null, null) or (null, y)
            if (ye is null) return false; // (x, null)
            if (ReferenceEquals(xe, ye)) return true; // (x, x)

            // ICollection is more common
            if (xe is ICollection<TSource> xc)
            {
                var isEqual = CheckCount(xc.Count);
                if (isEqual.HasValue) return isEqual.Value;
            }

            // IReadOnlyCollection
            else if (xe is IReadOnlyCollection<TSource> xr)
            {
                var isEqual = CheckCount(xr.Count);
                if (isEqual.HasValue) return isEqual.Value;
            }

            var cmpr = comparer ?? EqualityComparer<TSource>.Default;

            // Build an ISet for each input collection, ensuring the same equality comparer
            var xss = new HashSet<TSource>(xe, cmpr);
            var yss = new HashSet<TSource>(ye, cmpr);

            // Use native comparison
            return xss.SetEquals(yss);

            // Local functions

            bool? CheckCount(int xCount)
            {
                // ICollection is more common
                if (ye is ICollection<TSource> yc)
                {
                    if (xCount != yc.Count) return false; // (n, m)
                    if (xCount == 0) return true; // (0, 0)
                }

                // IReadOnlyCollection
                else if (ye is IReadOnlyCollection<TSource> yr)
                {
                    if (xCount != yr.Count) return false; // (n, m)
                    if (xCount == 0) return true; // (0, 0)
                }

                return null;
            }
        }
    }
}
