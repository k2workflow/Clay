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
        /// <summary>
        /// Performs an efficient item-by-item comparison of two enumerables
        /// using the <see cref="IEqualityComparer{T}"/> from the first dictionary for Key comparisons
        /// and the specified <see cref="IEqualityComparer{T}"/> for Value comparisons.
        /// </summary>
        /// <typeparam name="T">The type of items.</typeparam>
        /// <param name="xe">Input 1</param>
        /// <param name="ye">Input 2</param>
        /// <param name="valueComparer">The comparer to use to test for Value equality.</param>
        /// <returns></returns>
        public static bool NullableDictionaryEqual<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, TValue>> xe, IEnumerable<KeyValuePair<TKey, TValue>> ye, IEqualityComparer<TValue> valueComparer = null)
        {
            if (xe is null) return ye is null; // (null, null) or (null, y)
            if (ye is null) return false; // (x, null)
            if (ReferenceEquals(xe, ye)) return true; // (x, x)

            var cmpr = valueComparer ?? EqualityComparer<TValue>.Default;

            // If both are some kind of collection
            if (EnumerableExtensions.BothAreCollections(xe, ye, out var xCount, out var yCount))
            {
                // Then we can immediately compare counts
                if (xCount != yCount) return false; // (n, m)
                if (xCount == 0) return true; // (0, 0)

                // IDictionary is more common
                if (xe is IDictionary<TKey, TValue> xd)
                {
                    // For each key in the second dictionary...
                    foreach (var yi in ye)
                    {
                        // ...check if that same key exists in the first dictionary
                        if (!xd.TryGetValue(yi.Key, out var xVal)) return false; // Key: Uses the equality comparer from the first dictionary

                        // And if so, whether the corresponding values match
                        if (!cmpr.Equals(yi.Value, xVal)) return false; // Value: Uses the specified equality comparer
                    }

                    return true;
                }

                // IReadOnlyDictionary
                if (xe is IReadOnlyDictionary<TKey, TValue> xrd)
                {
                    // For each key in the second dictionary...
                    foreach (var yi in ye)
                    {
                        // ...check if that same key exists in the first dictionary
                        if (!xrd.TryGetValue(yi.Key, out var xVal)) return false; // Key: Uses the equality comparer from the first dictionary

                        // And if so, whether the corresponding values match
                        if (!cmpr.Equals(yi.Value, xVal)) return false; // Value: Uses the specified equality comparer
                    }

                    return true;
                }
            }

            // Synthesize an IDictionary
            var xdd = new Dictionary<TKey, TValue>(xe);

            // For each key in the second dictionary...
            foreach (var yvp in ye)
            {
                // ...check if that same key exists in the first dictionary
                if (!xdd.TryGetValue(yvp.Key, out var xVal)) return false; // Key: Uses the equality comparer from the first dictionary

                // And if so, whether the corresponding values match
                if (!cmpr.Equals(yvp.Value, xVal)) return false; // Value: Uses the specified equality comparer
            }

            return true;
        }
    }
}
