using System;
using System.Collections;
using System.Collections.Generic;

namespace SourceCode.Clay.Collections.Generic
{
    /// <summary>
    /// Represents extensions for <see cref="Span{T}"/> and <see cref="ReadOnlySpan{T}{T}"/>.
    /// </summary>
    public static class SpanExtensions
    {
        /// <summary>
        /// Performs an optimized item-by-item comparison, using a custom <see cref="IEqualityComparer{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of items.</typeparam>
        /// <param name="x">Span 1</param>
        /// <param name="y">Span 2</param>
        /// <param name="comparer">The comparer to use to test for equality.</param>
        /// <param name="sequential">Optimizes the algorithm for cases when the inputs are expected to be ordered in the same manner.</param>
        /// <returns></returns>
        public static bool SpanEquals<T>(this ReadOnlySpan<T> x, ReadOnlySpan<T> y, IEqualityComparer<T> comparer, bool sequential)
        {
            // Span is a struct
            if (comparer == null) throw new ArgumentNullException(nameof(comparer));

            // If counts are different, not equal
            if (x.Length != y.Length) return false;

            // Optimize for cases 0, 1, 2, N
            switch (x.Length)
            {
                // If first count is 0 then, due to previous check, the second is guaranteed to be 0 (and thus equal)
                case 0: return true; // Same as IsEmpty

                // If there is only 1 item, short-circuit
                case 1: return comparer.Equals(x[0], y[0]);

                // If there are 2 items, short-circuit
                case 2:
                    {
                        // Horizontal
                        if (comparer.Equals(x[0], y[0]))
                            return comparer.Equals(x[1], y[1]);

                        // Diagonal
                        if (comparer.Equals(x[0], y[1]))
                            return comparer.Equals(x[1], y[0]);
                    }
                    return false;

                // Else we need to do more work
                default: break;
            }

            var min = 0;
            var max = x.Length - 1;
            var bit = new BitArray(x.Length); // Optimize looping by tracking which positions have been matched

            for (var i = 0; i < x.Length; i++)
            {
                // Colocated comparisons should be at the same position
                if (sequential
                    && !bit[i]
                    && comparer.Equals(x[i], y[i]))
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
        /// Performs an optimized item-by-item comparison.
        /// </summary>
        /// <typeparam name="T">The type of items.</typeparam>
        /// <param name="x">List 1</param>
        /// <param name="y">List 2</param>
        /// <param name="extractor">A delegate that extracts an embedded value from each item before comparing it using the specified comparer.</param>
        /// <param name="comparer">The comparer to use to test for equality.</param>
        /// <param name="sequential">Optimizes the algorithm for cases when the inputs are expected to be ordered in the same manner.</param>
        /// <returns></returns>
        public static bool SpanEquals<T, U>(this ReadOnlySpan<T> x, ReadOnlySpan<T> y, Func<T, U> extractor, IEqualityComparer<U> comparer, bool sequential)
        {
            if (extractor == null) throw new ArgumentNullException(nameof(extractor));
            if (comparer == null) throw new ArgumentNullException(nameof(comparer));

            // If counts are different, not equal
            if (x.Length != y.Length) return false;

            // Optimize for cases 0, 1, 2, N
            switch (x.Length)
            {
                // If first count is 0 then, due to previous check, the second is guaranteed to be 0 (and thus equal)
                case 0: return true; // Same as IsEmpty

                // If there is only 1 item, short-circuit
                case 1:
                    {
                        var x0 = extractor(x[0]);
                        var y0 = extractor(y[0]);

                        return comparer.Equals(x0, y0);
                    }

                // If there are 2 items, short-circuit
                case 2:
                    {
                        var x0 = extractor(x[0]);
                        var y0 = extractor(y[0]);

                        var x1 = extractor(x[1]);
                        var y1 = extractor(y[1]);

                        // Horizontal
                        if (comparer.Equals(x0, y0))
                            return comparer.Equals(x1, y1);

                        // Diagonal
                        if (comparer.Equals(x0, y1))
                            return comparer.Equals(x1, y0);
                    }
                    return false;

                // Else we need to do more work
                default: break;
            }

            var min = 0;
            var max = x.Length - 1;
            var bit = new BitArray(x.Length); // Optimize looping by tracking which positions have been matched

            for (var i = 0; i < x.Length; i++)
            {
                var xi = extractor(x[i]);
                var yi = extractor(y[i]);

                // Colocated comparisons should be at the same position
                if (sequential
                    && !bit[i]
                    && comparer.Equals(xi, yi))
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
        /// Performs an optimized item-by-item comparison, using the default comparer for the type.
        /// </summary>
        /// <typeparam name="T">The type of items.</typeparam>
        /// <param name="x">Span 1</param>
        /// <param name="y">Span 2</param>
        /// <param name="sequential">Optimizes the algorithm for cases when the inputs are expected to be ordered in the same manner.</param>
        /// <returns></returns>
        public static bool SpanEquals<T>(this ReadOnlySpan<T> x, ReadOnlySpan<T> y, bool sequential)
            => x.SpanEquals(y, EqualityComparer<T>.Default, sequential);
    }
}