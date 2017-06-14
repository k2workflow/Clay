using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq.Expressions;

namespace SourceCode.Clay.Collections.Generic
{
    /// <summary>
    /// Represents extensions for lists.
    /// </summary>
    public static class ListExtensions
    {
        /// <summary>
        /// Builds an ordinal switch expression.
        /// </summary>
        /// <typeparam name="T">The type of the elements.</typeparam>
        /// <param name="items">The items.</param>
        /// <param name="keyExtractor">The key extractor.</param>
        /// <param name="ignoreCase">if set to <c>true</c> case will be ignored.</param>
        /// <returns>The compiled switch statement.</returns>
        public static Func<string, int> BuildOrdinalSwitchExpression<T>(this IReadOnlyList<T> items, Func<T, string> keyExtractor, bool ignoreCase)
        {
            if (items == null) throw new ArgumentNullException(nameof(items));
            if (keyExtractor == null) throw new ArgumentNullException(nameof(keyExtractor));

            // Return -1 if item is not found (per standard convention for IndexOf())
            var notFound = Expression.Constant(-1);

            // Exit early if no items
            if (items == null || items.Count == 0)
            {
                var noItems = Expression.Lambda<Func<string, int>>(notFound);
                return noItems.Compile();
            }

            // Define formal parameter & ensure value is UPPER
            var formalParam = Expression.Parameter(typeof(string), "key");

            // Format MUST match #1 below
            Expression switchValue = formalParam;
            if (ignoreCase)
            {
                switchValue = Expression.Call(formalParam, nameof(string.ToUpperInvariant), null);
            }

            // Create <Key, SwitchCase>[] list
            var switchCases = new SwitchCase[items.Count];
            for (var i = 0; i < switchCases.Length; i++)
            {
                // Extract Key from T
                var key = keyExtractor(items[i]);
                Debug.Assert(!string.IsNullOrWhiteSpace(key));

                // Format MUST match #1 above
                if (ignoreCase)
                {
                    key = key.ToUpperInvariant();
                }

                // Create Case Expression
                var body = Expression.Constant(i);
                switchCases[i] = Expression.SwitchCase(body, Expression.Constant(key));
            }

            // Create Switch Expression
            var switchExpr = Expression.Switch(switchValue, notFound, switchCases);

            // Compile Lambda
            var lambda = Expression.Lambda<Func<string, int>>(switchExpr, formalParam);
            return lambda.Compile();
        }

        /// <summary>
        /// Builds an ordinal switch expression.
        /// </summary>
        /// <typeparam name="TElement">The type of the elements.</typeparam>
        /// <typeparam name="TValue">The type of the return value.</typeparam>
        /// <param name="items">The items.</param>
        /// <param name="keyExtractor">The key extractor.</param>
        /// <param name="valueExtractor">The value extractor.</param>
        /// <returns>The compiled switch statement.</returns>

        public static Func<TKey, TValue> BuildSwitchExpression<TElement, TKey, TValue>(this IReadOnlyList<TElement> items, Func<TElement, TKey> keyExtractor, Func<TElement, TValue> valueExtractor)
        {
            if (items == null) throw new ArgumentNullException(nameof(items));
            if (keyExtractor == null) throw new ArgumentNullException(nameof(keyExtractor));
            if (valueExtractor == null) throw new ArgumentNullException(nameof(valueExtractor));

            var tValue = typeof(TValue);

            // Return default(TValue) if item is not found
            var notFound = Expression.Default(tValue);

            // Exit early if no items
            if (items == null || items.Count == 0)
            {
                var noItems = Expression.Lambda<Func<TKey, TValue>>(notFound);
                return noItems.Compile();
            }

            // Define formal parameter
            var formalParam = Expression.Parameter(typeof(TKey), "key");

            // Create <Key, SwitchCase>[] list
            var switchCases = new SwitchCase[items.Count];
            for (var i = 0; i < switchCases.Length; i++)
            {
                // Extract Key from T
                var key = keyExtractor(items[i]);
                var value = valueExtractor(items[i]);

                // Create Case Expression
                var body = Expression.Constant(value, tValue);
                switchCases[i] = Expression.SwitchCase(body, Expression.Constant(key));
            }

            // Create Switch Expression
            var switchExpr = Expression.Switch(formalParam, notFound, switchCases);

            // Compile Lambda
            var lambda = Expression.Lambda<Func<TKey, TValue>>(switchExpr, formalParam);
            return lambda.Compile();
        }

        /// <summary>
        /// Performs an efficient item-by-item comparison
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="x">List 1</param>
        /// <param name="y">List 2</param>
        /// <param name="colocated">Optimizes algorithm for cases when the inputs are expected to be ordered in the same manner.</param>
        /// <param name="comparer"></param>
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
        /// Performs an efficient item-by-item comparison
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="x">List 1</param>
        /// <param name="y">List 2</param>
        /// <param name="colocated">Optimizes algorithm for cases when the inputs are expected to be ordered in the same manner.</param>
        /// <param name="convert">Transforms each input before comparing it using the specified comparer.</param>
        /// <param name="comparer"></param>
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
        /// <typeparam name="T"></typeparam>
        /// <param name="x">List 1</param>
        /// <param name="y">List 2</param>
        /// <param name="colocated">Optimizes algorithm for cases when the inputs are expected to be ordered in the same manner.</param>
        /// <returns></returns>
        public static bool FastEquals<T>(this IReadOnlyList<T> x, IReadOnlyList<T> y, bool colocated)
            => FastEquals(x, y, EqualityComparer<T>.Default, colocated);
    }
}
