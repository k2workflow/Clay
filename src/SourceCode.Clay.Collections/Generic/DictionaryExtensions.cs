#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using System.Collections.Generic;

namespace SourceCode.Clay.Collections.Generic
{
    /// <summary>
    /// Represents extensions for <see cref="Dictionary{TKey, TValue}"/> and <see cref="IReadOnlyDictionary{TKey, TValue}{T}"/>.
    /// </summary>
    public static class DictionaryExtensions
    {
        #region Methods

        /// <summary>
        /// Performs an efficient item-by-item comparison
        /// using the <see cref="IEqualityComparer{T}"/> from the first dictionary for Key comparisons
        /// and the specified <see cref="IEqualityComparer{T}"/> for Value comparisons.
        /// </summary>
        /// <typeparam name="T">The type of items.</typeparam>
        /// <param name="x">Dictionary 1</param>
        /// <param name="y">Dictionary 2</param>
        /// <param name="valueComparer">The comparer to use to test for Value equality.</param>
        /// <returns></returns>
        public static bool NullableDictionaryEquals<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, TValue>> x, IEnumerable<KeyValuePair<TKey, TValue>> y, in IEqualityComparer<TValue> valueComparer)
        {
            if (x is null) return y is null; // (null, null) or (null, y)
            if (y is null) return false; // (x, null)
            if (ReferenceEquals(x, y)) return true; // (x, x)

            var cmpr = valueComparer ?? EqualityComparer<TValue>.Default;

            // IDictionary is more likely
            if (x is IDictionary<TKey, TValue> xd)
            {
                var isEqual = CheckCount(xd.Count);
                if (isEqual.HasValue) return isEqual.Value;

                // For each key in the second dictionary...
                foreach (var yvp in y)
                {
                    // ...check if that same key exists in the first dictionary
                    if (!xd.TryGetValue(yvp.Key, out var xVal)) return false; // Key: Uses the equality comparer from the first dictionary

                    // And if so, whether the corresponding values match
                    if (!cmpr.Equals(yvp.Value, xVal)) return false; // Value: Uses the specified equality comparer
                }

                return true;
            }

            // IReadOnlyDictionary
            if (x is IReadOnlyDictionary<TKey, TValue> xrd)
            {
                var isEqual = CheckCount(xrd.Count);
                if (isEqual.HasValue) return isEqual.Value;

                // For each key in the second dictionary...
                foreach (var yvp in y)
                {
                    // ...check if that same key exists in the first dictionary
                    if (!xrd.TryGetValue(yvp.Key, out var xVal)) return false; // Key: Uses the equality comparer from the first dictionary

                    // And if so, whether the corresponding values match
                    if (!cmpr.Equals(yvp.Value, xVal)) return false; // Value: Uses the specified equality comparer
                }

                return true;
            }

            // Synthesize an IDictionary
            var xdd = new Dictionary<TKey, TValue>(x);

            // For each key in the second dictionary...
            foreach (var yvp in y)
            {
                // ...check if that same key exists in the first dictionary
                if (!xdd.TryGetValue(yvp.Key, out var xVal)) return false; // Key: Uses the equality comparer from the first dictionary

                // And if so, whether the corresponding values match
                if (!cmpr.Equals(yvp.Value, xVal)) return false; // Value: Uses the specified equality comparer
            }

            return true;

            // Local functions
            bool? CheckCount(int xCount)
            {
                // ICollection is more common
                if (y is ICollection<KeyValuePair<TKey, TValue>> yc)
                {
                    if (xCount != yc.Count) return false; // (n, m)
                    if (xCount == 0) return true; // (0, 0)
                }
                // IReadOnlyCollection
                else if (y is IReadOnlyCollection<KeyValuePair<TKey, TValue>> yrc)
                {
                    if (xCount != yrc.Count) return false; // (n, m)
                    if (xCount == 0) return true; // (0, 0)
                }

                return null;
            }
        }

        /// <summary>
        /// Performs an efficient item-by-item comparison,
        /// using the <see cref="IEqualityComparer{T}"/> from the first dictionary for Key comparisons
        /// and the default <see cref="IEqualityComparer{T}"/> for Value comparisons.
        /// </summary>
        /// <typeparam name="T">The type of items.</typeparam>
        /// <param name="x">Dictionary 1</param>
        /// <param name="y">Dictionary 2</param>
        /// <returns></returns>
        public static bool NullableDictionaryEquals<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, TValue>> x, IEnumerable<KeyValuePair<TKey, TValue>> y)
            => NullableDictionaryEquals(x, y, null);

        #endregion
    }
}
