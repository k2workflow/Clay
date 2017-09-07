using System;
using System.Collections;
using System.Collections.Generic;

namespace SourceCode.Clay.Collections.Generic
{
    /// <summary>
    /// Represents extensions for <see cref="IList{T}"/>.
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
        public static bool NullableEquals<T>(this IReadOnlyList<T> x, IReadOnlyList<T> y, IEqualityComparer<T> comparer, bool colocated)
        {
            if (comparer == null) throw new ArgumentNullException(nameof(comparer));

            var xNull = ReferenceEquals(x, null);
            if (xNull ^ ReferenceEquals(y, null)) return false; // One is null but not the other
            if (xNull) return true; // Both are null

            // Both are not null; we can now test their values
            if (ReferenceEquals(x, y)) return true; // Same reference

            // If counts are different, not equal
            if (x.Count != y.Count) return false;

            switch (x.Count)
            {
                // If first count is 0 then, due to previous check, the second is guaranteed to be 0 (and thus equal)
                case 0: return true;

                // If there is only 1 item, short-circuit the comparison
                case 1: return comparer.Equals(x[0], y[0]);

                default: break;
            }

            var min = 0;
            var max = x.Count - 1;
            var bit = new BitArray(x.Count); // Optimize looping by tracking which positions have been matched

            for (int i = 0; i < x.Count; i++)
            {
                // Colocated comparisons should be at the same position
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
        public static bool NullableEquals<T, U>(this IReadOnlyList<T> x, IReadOnlyList<T> y, Func<T, U> convert, IEqualityComparer<U> comparer, bool colocated)
        {
            if (convert == null) throw new ArgumentNullException(nameof(convert));
            if (comparer == null) throw new ArgumentNullException(nameof(comparer));

            var xNull = ReferenceEquals(x, null);
            if (xNull ^ ReferenceEquals(y, null)) return false; // One is null but not the other
            if (xNull) return true; // Both are null

            // Both are not null; we can now test their values
            if (ReferenceEquals(x, y)) return true; // Same reference

            // If counts are different, not equal
            if (x.Count != y.Count) return false;

            switch (x.Count)
            {
                // If first count is 0 then, due to previous check, the second is guaranteed to be 0 (and thus equal)
                case 0: return true;

                // If there is only 1 item, short-circuit the comparison
                case 1:
                    {
                        var x0 = convert(x[0]);
                        var y0 = convert(y[0]);

                        return comparer.Equals(x0, y0);
                    }

                default: break;
            }

            var min = 0;
            var max = x.Count - 1;
            var bit = new BitArray(x.Count); // Optimize looping by tracking which positions have been matched

            for (int i = 0; i < x.Count; i++)
            {
                var xi = convert(x[i]);
                var yi = convert(y[i]);

                // Colocated comparisons should be at the same position
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
        /// Performs an efficient item-by-item comparison, using the default comparer for the type.
        /// </summary>
        /// <typeparam name="T">The type of items.</typeparam>
        /// <param name="x">List 1</param>
        /// <param name="y">List 2</param>
        /// <param name="colocated">Optimizes algorithm for cases when the inputs are expected to be ordered in the same manner.</param>
        /// <returns></returns>
        public static bool NullableEquals<T>(this IReadOnlyList<T> x, IReadOnlyList<T> y, bool colocated)
            => x.NullableEquals(y, EqualityComparer<T>.Default, colocated);
    }
}
