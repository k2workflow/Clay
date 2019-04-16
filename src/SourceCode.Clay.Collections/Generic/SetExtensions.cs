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
    internal static class SetExtensions
    {
        /// <summary>
        /// Performs an efficient item-by-item comparison of two enumerables, using a custom <see cref="IEqualityComparer{T}"/>.
        /// </summary>
        /// <typeparam name="TSource">The type of items.</typeparam>
        /// <param name="xe">Input 1</param>
        /// <param name="ye">Input 2</param>
        /// <param name="comparer">The comparer to use to test for equality.</param>
        public static bool NullableSetEqual<TSource>(this IEnumerable<TSource> xe, IEnumerable<TSource> ye, IEqualityComparer<TSource> comparer = null)
        {
            if (xe is null) return ye is null; // (null, null) or (null, y)
            if (ye is null) return false; // (x, null)
            if (ReferenceEquals(xe, ye)) return true; // (x, x)

            // If both are some kind of collection
            if (xe is ICollection<TSource> xc
                && ye is ICollection<TSource> yc)
            {
                // Then we can short-circuit on counts
                if (xc.Count != yc.Count) return false; // (n, m)
                if (xc.Count == 0) return true; // (0, 0)
            }

            // If both are a HashSet and comparer is not specified
            if (comparer == null
                && xe is HashSet<TSource> xs
                && ye is HashSet<TSource> ys)
            {
                return xs.SetEquals(ys);
            }

            IEqualityComparer<TSource> cmpr = comparer ?? EqualityComparer<TSource>.Default;

            // Build an ISet for each input collection, ensuring the same equality comparer
            var xss = new HashSet<TSource>(xe, cmpr);
            var yss = new HashSet<TSource>(ye, cmpr);

            // Use native comparison
            var equal = xss.SetEquals(yss);
            return equal;
        }
    }
}
