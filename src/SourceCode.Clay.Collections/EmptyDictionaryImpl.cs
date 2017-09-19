using System;
using System.Collections;
using System.Collections.Generic;

namespace SourceCode.Clay.Collections.Generic
{
    /// <summary>
    /// A memory-efficient internal implementation of a Dictionary that has zero members.
    /// Useful for scenarios where the equivalent of <see cref="Array.Empty{T}"/> is required
    /// for <see cref="IDictionary{TKey, TValue}"/> or <see cref="IReadOnlyDictionary{TKey, TValue}"/>.
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    internal sealed class EmptyDictionaryImpl<TKey, TValue> : IDictionary<TKey, TValue>, IReadOnlyDictionary<TKey, TValue>
    {
        #region Constants

        internal static readonly IDictionary<TKey, TValue> Value = new EmptyDictionaryImpl<TKey, TValue>();

        internal static readonly IReadOnlyDictionary<TKey, TValue> ReadOnlyValue = (IReadOnlyDictionary<TKey, TValue>)Value;

        #endregion

        #region Fields

        private readonly IReadOnlyDictionary<TKey, TValue> _dict = new Dictionary<TKey, TValue>(0);

        #endregion

        #region Properties

        public TValue this[TKey key]
        {
            get => _dict[key]; // Leverage the underlying exception
            set => throw new InvalidOperationException();
        }

        ICollection<TKey> IDictionary<TKey, TValue>.Keys => Array.Empty<TKey>();

        ICollection<TValue> IDictionary<TKey, TValue>.Values => Array.Empty<TValue>();

        IEnumerable<TKey> IReadOnlyDictionary<TKey, TValue>.Keys => Array.Empty<TKey>();

        IEnumerable<TValue> IReadOnlyDictionary<TKey, TValue>.Values => Array.Empty<TValue>();

        public int Count => 0;

        public bool IsReadOnly => true;

        #endregion

        #region Constructors

        private EmptyDictionaryImpl()
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
            value = default;
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
