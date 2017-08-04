using System;
using System.Collections;
using System.Collections.Generic;

namespace SourceCode.Clay.Collections
{
    /// <summary>
    /// Useful properties and methods for <see cref="IReadOnlyDictionary{TKey, TValue}"/>.
    /// </summary>
    public static class ReadOnlyDictionary
    {
        #region Constants

        /// <summary>
        /// Returns an empty readonly dictionary.
        /// </summary>
        /// <typeparam name="TKey">The type of keys in the dictionary.</typeparam>
        /// <typeparam name="TValue">The type of values in the dictionary.</typeparam>
        /// <returns>Returns an empty <see cref="IReadOnlyDictionary{TKey, TValue}".</returns>
        public static IReadOnlyDictionary<TKey, TValue> Empty<TKey, TValue>() => EmptyImpl<TKey, TValue>.Value;

        #endregion

        private sealed class EmptyImpl<TKey, TValue> : IReadOnlyDictionary<TKey, TValue>
        {
            #region Constants

            internal static readonly IReadOnlyDictionary<TKey, TValue> Value = new EmptyImpl<TKey, TValue>();

            #endregion

            #region Fields

            private readonly IReadOnlyDictionary<TKey, TValue> _dict = new Dictionary<TKey, TValue>(0);

            #endregion

            #region Properties

            public TValue this[TKey key] => _dict[key];

            public IEnumerable<TKey> Keys => Array.Empty<TKey>();

            public IEnumerable<TValue> Values => Array.Empty<TValue>();

            public int Count => 0;

            #endregion

            #region Constructors

            private EmptyImpl()
            { }

            #endregion

            #region Methods

            public bool ContainsKey(TKey key) => false;

            public bool TryGetValue(TKey key, out TValue value)
            {
                value = default(TValue);
                return false;
            }

            #endregion

            #region IEnumerable

            IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator()
            {
                yield break;
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                yield break;
            }

            #endregion
        }
    }
}
