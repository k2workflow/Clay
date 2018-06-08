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
        /// <param name="x">List 1</param>
        /// <param name="y">List 2</param>
        /// <param name="comparer">The comparer to use to test for equality.</param>
        /// <returns></returns>
        public static bool NullableSequenceEquals<TSource>(this IEnumerable<TSource> x, IEnumerable<TSource> y, IEqualityComparer<TSource> comparer)
        {
            if (x is null) return y is null; // (null, null) or (null, y)
            if (y is null) return false; // (x, null)
            if (ReferenceEquals(x, y)) return true; // (x, x)

            var cmpr = comparer ?? EqualityComparer<TSource>.Default;

            // ICollection is more common
            if (x is ICollection<TSource> xc)
            {
                return xc.NullableCollectionEquals(y, cmpr);
            }

            // IReadOnlyCollection
            if (x is IReadOnlyCollection<TSource> xrc)
            {
                return xrc.NullableCollectionEquals(y, cmpr);
            }

            // IEnumerable
            var equal = EnumerableEquals(x, y, cmpr);
            return equal;
        }

        /// <summary>
        /// Performs an optimized item-by-item comparison, using the default comparer for the type.
        /// The lists are required to have corresponding items in the same ordinal position.
        /// </summary>
        /// <typeparam name="TSource">The type of items.</typeparam>
        /// <param name="x">List 1</param>
        /// <param name="y">List 2</param>
        /// <returns></returns>
        public static bool NullableSequenceEquals<TSource>(this IEnumerable<TSource> x, IEnumerable<TSource> y)
            => NullableSequenceEquals(x, y, null);

        internal static bool EnumerableEquals<TSource>(in IEnumerable<TSource> x, in IEnumerable<TSource> y, in IEqualityComparer<TSource> comparer)
        {
            Debug.Assert(x != null);
            Debug.Assert(y != null);
            Debug.Assert(comparer != null);

            using (var xe = x.GetEnumerator())
            using (var ye = y.GetEnumerator())
            {
                while (xe.MoveNext())
                {
                    if (!ye.MoveNext()) return false;

                    if (!comparer.Equals(xe.Current, ye.Current)) return false;
                }

                return !ye.MoveNext();
            }
        }
    }
}
