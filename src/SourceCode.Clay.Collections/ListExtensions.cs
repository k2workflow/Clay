using System;
using System.Collections;
using System.Collections.Generic;

namespace SourceCode.Clay.Collections.Generic
{
    /// <summary>
    /// Represents extensions for lists.
    /// </summary>
    public static class ListExtensions
    {
        /// <summary>
        /// Performs an efficient item-by-item comparison.
        /// </summary>
        /// <typeparam name="T">The type of items.</typeparam>
        /// <param name="x">List 1</param>
        /// <param name="y">List 2</param>
        /// <param name="comparer">The comparer to use to test for equality.</param>
        /// <param name="colocated">Optimizes algorithm for cases when the inputs are expected to be ordered in the same manner.</param>
        /// <returns></returns>
        public static bool FastEquals<T>(this IReadOnlyList<T> x, IReadOnlyList<T> y, IEqualityComparer<T> comparer, bool colocated)
        {
            if (comparer == null) throw new ArgumentNullException(nameof(comparer));

            // If one is null but not the other (xor), then not equal
            if (ReferenceEquals(x, null) ^ ReferenceEquals(y, null)) return false;

            // If first is null then, due to previous check, the second is guaranteed to be null (and thus equal)
            if (ReferenceEquals(x, null)) return true;

            // If counts are different, not equal
            if (x.Count != y.Count) return false;

            // If first count is 0 then, due to previous check, the second is guaranteed to be 0 (and thus equal)
            if (x.Count == 0) return true;

            var min = 0;
            var max = x.Count - 1;
            var bit = new BitArray(x.Count); // Optimize looping by tracking which positions have been matched

            for (int i = 0; i < x.Count; i++)
            {
                // Monotonic comparisons should be at the same position
                if (colocated && !bit[i] && comparer.Equals(x[i], y[i]))
                {
                    bit[i] = true;
                    if (i == min) min++;

                    continue;
                }

                var found = false;

                var j = min;
                for (; j <= max; j++)
                {
                    // Skip positions where a match was previously found
                    if (bit[j]) continue;

                    if (comparer.Equals(x[i], y[j]))
                    {
                        found = true;

                        bit[j] = true;
                        if (j == min) min++;
                        if (j == max) max--;

                        break;
                    }
                }

                if (!found) return false;
            }

            return true;
        }

        /// <summary>
        /// Performs an efficient item-by-item comparison.
        /// </summary>
        /// <typeparam name="T">The type of items.</typeparam>
        /// <param name="x">List 1</param>
        /// <param name="y">List 2</param>
        /// <param name="convert">Transforms each input before comparing it using the specified comparer.</param>
        /// <param name="comparer">The comparer to use to test for equality.</param>
        /// <param name="colocated">Optimizes algorithm for cases when the inputs are expected to be ordered in the same manner.</param>
        /// <returns></returns>
        public static bool FastEquals<T, U>(this IReadOnlyList<T> x, IReadOnlyList<T> y, Func<T, U> convert, IEqualityComparer<U> comparer, bool colocated)
        {
            if (convert == null) throw new ArgumentNullException(nameof(convert));
            if (comparer == null) throw new ArgumentNullException(nameof(comparer));

            // If one is null but not the other (xor), then not equal
            if (ReferenceEquals(x, null) ^ ReferenceEquals(y, null)) return false;

            // If first is null then, due to previous check, the second is guaranteed to be null (and thus equal)
            if (ReferenceEquals(x, null)) return true;

            // If counts are different, not equal
            if (x.Count != y.Count) return false;

            // If first count is 0 then, due to previous check, the second is guaranteed to be 0 (and thus equal)
            if (x.Count == 0) return true;

            var min = 0;
            var max = x.Count - 1;
            var bit = new BitArray(x.Count); // Optimize looping by tracking which positions have been matched

            for (int i = 0; i < x.Count; i++)
            {
                var xi = convert(x[i]);
                var yi = convert(y[i]);

                // Monotonic comparisons should be at the same position
                if (colocated && !bit[i] && comparer.Equals(xi, yi))
                {
                    bit[i] = true;
                    if (i == min) min++;

                    continue;
                }

                var found = false;

                var j = min;
                for (; j <= max; j++)
                {
                    // Skip positions where a match was previously found
                    if (bit[j]) continue;

                    if (comparer.Equals(xi, yi))
                    {
                        found = true;

                        bit[j] = true;
                        if (j == min) min++;
                        if (j == max) max--;

                        break;
                    }
                }

                if (!found) return false;
            }

            return true;
        }

        /// <summary>
        /// Performs an efficient item-by-item comparison, used the default comparer for the type.
        /// </summary>
        /// <typeparam name="T">The type of items.</typeparam>
        /// <param name="x">List 1</param>
        /// <param name="y">List 2</param>
        /// <param name="colocated">Optimizes algorithm for cases when the inputs are expected to be ordered in the same manner.</param>
        /// <returns></returns>
        public static bool FastEquals<T>(this IReadOnlyList<T> x, IReadOnlyList<T> y, bool colocated)
            => FastEquals(x, y, EqualityComparer<T>.Default, colocated);
    }
}
