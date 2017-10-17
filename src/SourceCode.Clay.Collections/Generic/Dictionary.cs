#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using System.Collections.Generic;

namespace SourceCode.Clay.Collections.Generic
{
    /// <summary>
    /// Useful properties and methods for <see cref="IDictionary{TKey, TValue}"/>.
    /// </summary>
    public static class Dictionary
    {
        #region Constants

        /// <summary>
        /// Returns an empty dictionary that is immutable.
        /// </summary>
        /// <typeparam name="TKey">The type of keys in the dictionary.</typeparam>
        /// <typeparam name="TValue">The type of values in the dictionary.</typeparam>
        /// <returns>Returns an empty <see cref="IDictionary{TKey, TValue}".</returns>
        public static IDictionary<TKey, TValue> Empty<TKey, TValue>() => EmptyDictionaryImpl<TKey, TValue>.Value;

        /// <summary>
        /// Returns an empty dictionary that is immutable.
        /// </summary>
        /// <typeparam name="TKey">The type of keys in the dictionary.</typeparam>
        /// <typeparam name="TValue">The type of values in the dictionary.</typeparam>
        /// <returns>Returns an empty <see cref="IReadOnlyDictionary{TKey, TValue}".</returns>
        public static IReadOnlyDictionary<TKey, TValue> ReadOnlyEmpty<TKey, TValue>() => EmptyDictionaryImpl<TKey, TValue>.ReadOnlyValue;

        #endregion
    }
}
