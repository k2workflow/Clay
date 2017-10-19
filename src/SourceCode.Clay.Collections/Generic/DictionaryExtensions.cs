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
        public static bool NullableDictionaryEquals<TKey, TValue>(this IReadOnlyDictionary<TKey, TValue> x, IReadOnlyDictionary<TKey, TValue> y, IEqualityComparer<TValue> valueComparer)
        {
            if (x is null ^ y is null) return false; // (x, null) or (null, y)
            if (x is null) return true; // (null, null)
            if (ReferenceEquals(x, y)) return true; // (x, x)

            if (x.Count != y.Count) return false; // (n, m)
            if (x.Count == 0) return true; // (0, 0)

            var cmpr = valueComparer ?? EqualityComparer<TValue>.Default;

            // For each key in the first dictionary...
            foreach (var yvp in y)
            {
                // ...check if that same key exists in the second dictionary
                if (!x.TryGetValue(yvp.Key, out var xVal)) return false; // Key: Uses the equality comparer from the first dictionary

                // And if so, whether the corresponding values match
                if (!cmpr.Equals(yvp.Value, xVal)) return false; // Value: Uses the specified equality comparer
            }

            return true;
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
        public static bool NullableDictionaryEquals<TKey, TValue>(this IReadOnlyDictionary<TKey, TValue> x, IReadOnlyDictionary<TKey, TValue> y)
            => NullableDictionaryEquals(x, y, null);

        #endregion
    }
}
