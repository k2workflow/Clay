using System.Collections.Generic;

namespace SourceCode.Clay.Collections.Generic
{
    /// <summary>
    /// Represents <see cref="IEnumerable{T}"/> extensions.
    /// </summary>
    public static class IEnumerableExtensions
    {
        public static IEnumerable<T> OrEmptyIfNull<T>(this IEnumerable<T> source)
            => source ?? System.Linq.Enumerable.Empty<T>();

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
    }
}
