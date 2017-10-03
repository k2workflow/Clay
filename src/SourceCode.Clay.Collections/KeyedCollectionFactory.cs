using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace SourceCode.Clay.Collections.Generic
{
    /// <summary>
    /// Factory method that enable the creation of KeyedCollections.
    /// </summary>
    public static class KeyedCollectionFactory
    {
        /// <summary>
        /// Creates a Dictionary that stores values containing embedded keys.
        /// </summary>
        /// <typeparam name="TKey">The type of keys in the collection.</typeparam>
        /// <typeparam name="TItem">The type of items in the collection.</typeparam>
        /// <param name="items">The items to populate the dictionary with.</param>
        /// <param name="keyExtractor">A delegate that extracts the embedded key from each item.</param>
        /// <param name="comparer">The comparer to use.</param>
        /// <param name="dictionaryCreationThreshold">The number of elements the collection can hold without creating a lookup dictionary.
        /// (0 creates the lookup dictionary when the first item is added), or –1 to specify that a lookup dictionary is never created.
        /// </param>
        /// <returns>An instance of the Dictionary.</returns>
        public static KeyedCollection<TKey, TItem> Create<TKey, TItem>(IEnumerable<TItem> items, Func<TItem, TKey> keyExtractor, IEqualityComparer<TKey> comparer, int dictionaryCreationThreshold)
        {
            if (items == null) throw new ArgumentNullException(nameof(items));
            if (keyExtractor == null) throw new ArgumentNullException(nameof(keyExtractor));
            if (comparer == null) throw new ArgumentNullException(nameof(comparer));
            if (dictionaryCreationThreshold < -1) throw new ArgumentOutOfRangeException(nameof(dictionaryCreationThreshold));

            var impl = new KeyedCollectionImpl<TKey, TItem>(keyExtractor, comparer, dictionaryCreationThreshold);

            foreach (var item in items)
                impl.Add(item);

            return impl;
        }

        /// <summary>
        /// Creates a Dictionary that stores values containing embedded keys.
        /// </summary>
        /// <typeparam name="TKey">The type of keys in the collection.</typeparam>
        /// <typeparam name="TItem">The type of items in the collection.</typeparam>
        /// <param name="keyExtractor">A delegate that extracts the embedded key from each item.</param>
        /// <param name="comparer">The comparer to use.</param>
        /// <param name="dictionaryCreationThreshold">The number of elements the collection can hold without creating a lookup dictionary.
        /// (0 creates the lookup dictionary when the first item is added), or –1 to specify that a lookup dictionary is never created.
        /// </param>
        /// <returns>An instance of the Dictionary.</returns>
        public static KeyedCollection<TKey, TItem> Create<TKey, TItem>(Func<TItem, TKey> keyExtractor, IEqualityComparer<TKey> comparer, int dictionaryCreationThreshold)
        {
            if (keyExtractor == null) throw new ArgumentNullException(nameof(keyExtractor));
            if (comparer == null) throw new ArgumentNullException(nameof(comparer));
            if (dictionaryCreationThreshold < -1) throw new ArgumentOutOfRangeException(nameof(dictionaryCreationThreshold));

            var impl = new KeyedCollectionImpl<TKey, TItem>(keyExtractor, comparer, dictionaryCreationThreshold);
            return impl;
        }

        /// <summary>
        /// Creates a Dictionary that stores values containing embedded keys.
        /// </summary>
        /// <typeparam name="TKey">The type of keys in the collection.</typeparam>
        /// <typeparam name="TItem">The type of items in the collection.</typeparam>
        /// <param name="items">The items to populate the dictionary with.</param>
        /// <param name="keyExtractor">A delegate that extracts the embedded key from each item.</param>
        /// <param name="comparer">The comparer to use.</param>
        /// </param>
        /// <returns>An instance of the Dictionary.</returns>
        public static KeyedCollection<TKey, TItem> Create<TKey, TItem>(IEnumerable<TItem> items, Func<TItem, TKey> keyExtractor, IEqualityComparer<TKey> comparer)
        {
            if (items == null) throw new ArgumentNullException(nameof(items));
            if (keyExtractor == null) throw new ArgumentNullException(nameof(keyExtractor));
            if (comparer == null) throw new ArgumentNullException(nameof(comparer));

            var impl = new KeyedCollectionImpl<TKey, TItem>(keyExtractor, comparer);

            foreach (var item in items)
                impl.Add(item);

            return impl;
        }

        /// <summary>
        /// Creates a Dictionary that stores values containing embedded keys.
        /// </summary>
        /// <typeparam name="TKey">The type of keys in the collection.</typeparam>
        /// <typeparam name="TItem">The type of items in the collection.</typeparam>
        /// <param name="keyExtractor">A delegate that extracts the embedded key from each item.</param>
        /// <param name="comparer">The comparer to use.</param>
        /// </param>
        /// <returns>An instance of the Dictionary.</returns>
        public static KeyedCollection<TKey, TItem> Create<TKey, TItem>(Func<TItem, TKey> keyExtractor, IEqualityComparer<TKey> comparer)
        {
            if (keyExtractor == null) throw new ArgumentNullException(nameof(keyExtractor));
            if (comparer == null) throw new ArgumentNullException(nameof(comparer));

            var impl = new KeyedCollectionImpl<TKey, TItem>(keyExtractor, comparer);
            return impl;
        }

        /// <summary>
        /// Creates a Dictionary that stores values containing embedded keys.
        /// </summary>
        /// <typeparam name="TKey">The type of keys in the collection.</typeparam>
        /// <typeparam name="TItem">The type of items in the collection.</typeparam>
        /// <param name="items">The items to populate the dictionary with.</param>
        /// <param name="keyExtractor">A delegate that extracts the embedded key from each item.</param>
        /// </param>
        /// <returns>An instance of the Dictionary.</returns>
        public static KeyedCollection<TKey, TItem> Create<TKey, TItem>(IEnumerable<TItem> items, Func<TItem, TKey> keyExtractor)
        {
            if (keyExtractor == null) throw new ArgumentNullException(nameof(keyExtractor));

            var impl = new KeyedCollectionImpl<TKey, TItem>(keyExtractor);

            foreach (var item in items)
                impl.Add(item);

            return impl;
        }

        /// <summary>
        /// Creates a Dictionary that stores values containing embedded keys.
        /// </summary>
        /// <typeparam name="TKey">The type of keys in the collection.</typeparam>
        /// <typeparam name="TItem">The type of items in the collection.</typeparam>
        /// <param name="keyExtractor">A delegate that extracts the embedded key from each item.</param>
        /// </param>
        /// <returns>An instance of the Dictionary.</returns>
        public static KeyedCollection<TKey, TItem> Create<TKey, TItem>(Func<TItem, TKey> keyExtractor)
        {
            if (keyExtractor == null) throw new ArgumentNullException(nameof(keyExtractor));

            var impl = new KeyedCollectionImpl<TKey, TItem>(keyExtractor);
            return impl;
        }

        #region Implementation

        private sealed class KeyedCollectionImpl<TKey, TValue> : KeyedCollection<TKey, TValue>
        {
            private readonly Func<TValue, TKey> _keyExtractor;

            public KeyedCollectionImpl(Func<TValue, TKey> keyExtractor, IEqualityComparer<TKey> comparer, int dictionaryCreationThreshold)
                : base(comparer, dictionaryCreationThreshold)
            {
                if (comparer == null) throw new ArgumentNullException(nameof(comparer));
                if (dictionaryCreationThreshold < -1) throw new ArgumentOutOfRangeException(nameof(dictionaryCreationThreshold));

                _keyExtractor = keyExtractor ?? throw new ArgumentNullException(nameof(keyExtractor));
            }

            public KeyedCollectionImpl(Func<TValue, TKey> keyExtractor, IEqualityComparer<TKey> comparer)
                : base(comparer)
            {
                if (comparer == null) throw new ArgumentNullException(nameof(comparer));

                _keyExtractor = keyExtractor ?? throw new ArgumentNullException(nameof(keyExtractor));
            }

            public KeyedCollectionImpl(Func<TValue, TKey> keyExtractor)
            {
                _keyExtractor = keyExtractor ?? throw new ArgumentNullException(nameof(keyExtractor));
            }

            protected sealed override TKey GetKeyForItem(TValue item) => _keyExtractor(item);
        }

        #endregion
    }
}
