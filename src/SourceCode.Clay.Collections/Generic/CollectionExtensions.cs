#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using System.Collections.Generic;

namespace SourceCode.Clay.Collections.Generic
{
    /// <summary>
    /// Represents extensions for <see cref="ICollection{T}"/> and <see cref="IReadOnlyCollection{T}"/>.
    /// </summary>
    public static class CollectionExtensions
    {
        #region Methods

        /// <summary>
        /// Performs an optimized item-by-item comparison, using a custom <see cref="IEqualityComparer{T}"/>.
        /// The collections are required to have corresponding items in the same ordinal position.
        /// </summary>
        /// <typeparam name="TSource">The type of items.</typeparam>
        /// <param name="x">Collection 1</param>
        /// <param name="y">Collection 2</param>
        /// <param name="comparer">The comparer to use to test for equality.</param>
        /// <returns></returns>
        public static bool NullableCollectionEquals<TSource>(this ICollection<TSource> x, in IEnumerable<TSource> y, in IEqualityComparer<TSource> comparer)
        {
            if (x is null) return y is null; // (null, null) or (null, y)
            if (y is null) return false; // (x, null)
            if (ReferenceEquals(x, y)) return true; // (x, x)

            var cmpr = comparer ?? EqualityComparer<TSource>.Default;

            // IList is more likely
            if (x is IList<TSource> xl)
            {
                return xl.NullableListEquals(y, cmpr);
            }

            // IReadOnlyList
            if (x is IReadOnlyList<TSource> xrl)
            {
                return xrl.NullableListEquals(y, cmpr);
            }

            // IEnumerable
            var equal = EnumerableExtensions.CheckEnumerable(x, y, cmpr);
            return equal;
        }

        /// <summary>
        /// Performs an optimized item-by-item comparison, using the default comparer for the type.
        /// The collections are required to have corresponding items in the same ordinal position.
        /// </summary>
        /// <typeparam name="TSource">The type of items.</typeparam>
        /// <param name="x">Collection 1</param>
        /// <param name="y">Collection 2</param>
        /// <returns></returns>
        public static bool NullableCollectionEquals<TSource>(this ICollection<TSource> x, in IEnumerable<TSource> y)
            => NullableCollectionEquals(x, y, null);

        /// <summary>
        /// Performs an optimized item-by-item comparison, using a custom <see cref="IEqualityComparer{T}"/>.
        /// The collections are required to have corresponding items in the same ordinal position.
        /// </summary>
        /// <typeparam name="TSource">The type of items.</typeparam>
        /// <param name="x">Collection 1</param>
        /// <param name="y">Collection 2</param>
        /// <param name="comparer">The comparer to use to test for equality.</param>
        /// <returns></returns>
        public static bool NullableCollectionEquals<TSource>(this IReadOnlyCollection<TSource> x, in IEnumerable<TSource> y, in IEqualityComparer<TSource> comparer)
        {
            if (x is null) return y is null; // (null, null) or (null, y)
            if (y is null) return false; // (x, null)
            if (ReferenceEquals(x, y)) return true; // (x, x)

            var cmpr = comparer ?? EqualityComparer<TSource>.Default;

            // IReadOnlyList is more likely
            if (x is IReadOnlyList<TSource> xl)
            {
                return xl.NullableListEquals(y, cmpr);
            }

            // IList
            if (x is IList<TSource> xrl)
            {
                return xrl.NullableListEquals(y, cmpr);
            }

            // IEnumerable
            var equal = EnumerableExtensions.CheckEnumerable(x, y, cmpr);
            return equal;
        }

        /// <summary>
        /// Performs an optimized item-by-item comparison, using the default comparer for the type.
        /// The collections are required to have corresponding items in the same ordinal position.
        /// </summary>
        /// <typeparam name="TSource">The type of items.</typeparam>
        /// <param name="x">Collection 1</param>
        /// <param name="y">Collection 2</param>
        /// <returns></returns>
        public static bool NullableCollectionEquals<TSource>(this IReadOnlyCollection<TSource> x, in IEnumerable<TSource> y)
            => NullableCollectionEquals(x, y, null);

        #endregion
    }
}
