#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using System;
using System.Collections.Generic;

namespace SourceCode.Clay.Collections.Generic
{
    /// <summary>
    /// Represents extensions for <see cref="IReadOnlyDictionary{TKey, TValue}{T}"/>.
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
        public static bool DictionaryEquals<TKey, TValue>(this IReadOnlyDictionary<TKey, TValue> x, IReadOnlyDictionary<TKey, TValue> y, IEqualityComparer<TValue> valueComparer)
        {
            if (valueComparer == null) throw new ArgumentNullException(nameof(valueComparer));

            if (x is null ^ y is null) return false; // (x, null) or (null, y)
            if (x is null) return true; // (null, null)

            // Both are not null; we can now test their values
            if (ReferenceEquals(x, y)) return true; // (x, x)

            // If counts are different, not equal
            if (x.Count != y.Count) return false; // (n, m)

            // If first count is 0 then, due to previous check, the second is guaranteed to be 0 (and thus equal)
            if (x.Count == 0) return true;

            // For each key in the first dictionary...
            foreach (var yvp in y)
            {
                // ...check if that same key exists in the second dictionary
                if (!x.TryGetValue(yvp.Key, out var xVal)) return false; // Key: Uses the equality comparer from the first dictionary

                // And if so, whether the corresponding values match
                if (!valueComparer.Equals(yvp.Value, xVal)) return false; // Value: Uses the specified equality comparer
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
        public static bool DictionaryEquals<TKey, TValue>(this IReadOnlyDictionary<TKey, TValue> x, IReadOnlyDictionary<TKey, TValue> y)
            => x.DictionaryEquals(y, EqualityComparer<TValue>.Default);

        #endregion
    }
}
