using System;
using System.Collections;
using System.Collections.Generic;

namespace SourceCode.Clay.Collections
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
        public static IDictionary<TKey, TValue> Empty<TKey, TValue>() => EmptyImpl<TKey, TValue>.Value;

        #endregion

        internal sealed class EmptyImpl<TKey, TValue> : IDictionary<TKey, TValue>
        {
            #region Constants

            internal static readonly IDictionary<TKey, TValue> Value = new EmptyImpl<TKey, TValue>();

            internal static readonly IReadOnlyDictionary<TKey, TValue> ReadOnlyValue = (IReadOnlyDictionary<TKey, TValue>)Value;

            #endregion

            #region Fields

            private readonly IReadOnlyDictionary<TKey, TValue> _dict = new Dictionary<TKey, TValue>(0);

            #endregion

            #region Properties

            public TValue this[TKey key]
            {
                get => _dict[key];
                set => throw new InvalidOperationException();
            }

            public ICollection<TKey> Keys => Array.Empty<TKey>();

            public ICollection<TValue> Values => Array.Empty<TValue>();

            public int Count => 0;

            public bool IsReadOnly => true;

            #endregion

            #region Constructors

            private EmptyImpl()
            { }

            #endregion

            #region Methods

            public void Add(TKey key, TValue value)
                => throw new InvalidOperationException();

            public void Add(KeyValuePair<TKey, TValue> item)
                => throw new InvalidOperationException();

            public void Clear()
            {
                // No-op
            }

            public bool Contains(KeyValuePair<TKey, TValue> item) => false;

            public bool ContainsKey(TKey key) => false;

            public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
            {
                // No-op
            }

            public bool Remove(TKey key) => false;

            public bool Remove(KeyValuePair<TKey, TValue> item) => false;

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
