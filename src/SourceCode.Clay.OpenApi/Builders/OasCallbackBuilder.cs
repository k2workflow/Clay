#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using SourceCode.Clay.OpenApi.Expressions;
using System;
using System.Collections;
using System.Collections.Generic;

namespace SourceCode.Clay.OpenApi
{
    /// <summary>
    /// A map of possible out-of band callbacks related to the parent operation. Each value in the map is a
    /// <see cref="OasPath"/> that describes a set of requests that may be initiated by the API provider and
    /// the expected responses. The key value used to identify the callback object is an expression, evaluated
    /// at runtime, that identifies a URL to use for the callback operation.
    /// </summary>
    public class OasCallbackBuilder : IDictionary<OasExpression, OasReferable<OasPath>>, IReadOnlyDictionary<OasExpression, OasReferable<OasPath>>, IOasBuilder<OasCallback>
    {
        #region Fields

        private readonly Dictionary<OasExpression, OasReferable<OasPath>> _dictionary;

        #endregion

        #region Properties

        /// <summary>Gets a value indicating whether the <see cref="T:System.Collections.Generic.ICollection`1"></see> is read-only.</summary>
        /// <returns>true if the <see cref="T:System.Collections.Generic.ICollection`1"></see> is read-only; otherwise, false.</returns>
        public bool IsReadOnly => false;

        /// <summary>Gets an <see cref="T:System.Collections.Generic.ICollection`1"></see> containing the keys of the <see cref="T:System.Collections.Generic.IDictionary`2"></see>.</summary>
        /// <returns>An <see cref="T:System.Collections.Generic.ICollection`1"></see> containing the keys of the object that implements <see cref="T:System.Collections.Generic.IDictionary`2"></see>.</returns>
        public ICollection<OasExpression> Keys
            => _dictionary.Keys;

        /// <summary>Gets an <see cref="T:System.Collections.Generic.ICollection`1"></see> containing the values in the <see cref="T:System.Collections.Generic.IDictionary`2"></see>.</summary>
        /// <returns>An <see cref="T:System.Collections.Generic.ICollection`1"></see> containing the values in the object that implements <see cref="T:System.Collections.Generic.IDictionary`2"></see>.</returns>
        public ICollection<OasReferable<OasPath>> Values
            => _dictionary.Values;

        /// <summary>Gets the number of elements in the collection.</summary>
        /// <returns>The number of elements in the collection.</returns>
        public int Count
            => _dictionary.Count;

        /// <summary>Gets an enumerable collection that contains the keys in the read-only dictionary.</summary>
        /// <returns>An enumerable collection that contains the keys in the read-only dictionary.</returns>
        IEnumerable<OasExpression> IReadOnlyDictionary<OasExpression, OasReferable<OasPath>>.Keys
            => _dictionary.Keys;

        /// <summary>Gets an enumerable collection that contains the values in the read-only dictionary.</summary>
        /// <returns>An enumerable collection that contains the values in the read-only dictionary.</returns>
        IEnumerable<OasReferable<OasPath>> IReadOnlyDictionary<OasExpression, OasReferable<OasPath>>.Values
            => _dictionary.Values;

        #endregion

        #region Indexers

        /// <summary>Gets the element that has the specified key in the read-only dictionary.</summary>
        /// <param name="key">The key to locate.</param>
        /// <returns>The element that has the specified key in the read-only dictionary.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="key">key</paramref> is null.</exception>
        /// <exception cref="T:System.Collections.Generic.KeyNotFoundException">The property is retrieved and <paramref name="key">key</paramref> is not found.</exception>
        public OasReferable<OasPath> this[OasExpression key]
        {
            get => _dictionary[key];
            set => _dictionary[key] = value;
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of the <see cref="OasCallbackBuilder"/> class.
        /// </summary>
        public OasCallbackBuilder()
        {
            _dictionary = new Dictionary<OasExpression, OasReferable<OasPath>>();
        }

        /// <summary>
        /// Creates a new instance of the <see cref="OasCallbackBuilder"/> class.
        /// </summary>
        /// <param name="value">The <see cref="OasCallback"/> to copy values from.</param>
        public OasCallbackBuilder(OasCallback value)
        {
            if (value is null) throw new ArgumentNullException(nameof(value));
            _dictionary = new Dictionary<OasExpression, OasReferable<OasPath>>(value);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Performs an implicit conversion from <see cref="OasCallbackBuilder"/> to <see cref="OasCallback"/>.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator OasCallback(OasCallbackBuilder builder) => builder?.Build();

        /// <summary>
        /// Performs an implicit conversion from <see cref="OasCallbackBuilder"/> to <see cref="OasReferable{Callback}"/>.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator OasReferable<OasCallback>(OasCallbackBuilder builder) => builder?.Build();

        /// <summary>
        /// Performs an implicit conversion from <see cref="OasCallback"/> to <see cref="OasCallbackBuilder"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator OasCallbackBuilder(OasCallback value) => value is null ? null : new OasCallbackBuilder(value);

        /// <summary>
        /// Creates the <see cref="OasCallback"/> from this builder.
        /// </summary>
        /// <returns>The <see cref="OasCallback"/>.</returns>
        public virtual OasCallback Build() => new OasCallback(
            paths: this);

        /// <summary>Adds an element with the provided key and value to the <see cref="T:System.Collections.Generic.IDictionary`2"></see>.</summary>
        /// <param name="key">The object to use as the key of the element to add.</param>
        /// <param name="value">The object to use as the value of the element to add.</param>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="key">key</paramref> is null.</exception>
        /// <exception cref="T:System.ArgumentException">An element with the same key already exists in the <see cref="T:System.Collections.Generic.IDictionary`2"></see>.</exception>
        /// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.IDictionary`2"></see> is read-only.</exception>
        public void Add(OasExpression key, OasReferable<OasPath> value)
            => _dictionary.Add(key, value);

        /// <summary>Determines whether the read-only dictionary contains an element that has the specified key.</summary>
        /// <param name="key">The key to locate.</param>
        /// <returns>true if the read-only dictionary contains an element that has the specified key; otherwise, false.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="key">key</paramref> is null.</exception>
        public bool ContainsKey(OasExpression key)
            => _dictionary.ContainsKey(key);

        /// <summary>Removes the element with the specified key from the <see cref="T:System.Collections.Generic.IDictionary`2"></see>.</summary>
        /// <param name="key">The key of the element to remove.</param>
        /// <returns>true if the element is successfully removed; otherwise, false.  This method also returns false if <paramref name="key">key</paramref> was not found in the original <see cref="T:System.Collections.Generic.IDictionary`2"></see>.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="key">key</paramref> is null.</exception>
        /// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.IDictionary`2"></see> is read-only.</exception>
        public bool Remove(OasExpression key)
            => _dictionary.Remove(key);

        /// <summary>Gets the value that is associated with the specified key.</summary>
        /// <param name="key">The key to locate.</param>
        /// <param name="value">When this method returns, the value associated with the specified key, if the key is found; otherwise, the default value for the type of the value parameter. This parameter is passed uninitialized.</param>
        /// <returns>true if the object that implements the <see cref="T:System.Collections.Generic.IReadOnlyDictionary`2"></see> interface contains an element that has the specified key; otherwise, false.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="key">key</paramref> is null.</exception>
        public bool TryGetValue(OasExpression key, out OasReferable<OasPath> value)
            => _dictionary.TryGetValue(key, out value);

        /// <summary>Removes all items from the <see cref="T:System.Collections.Generic.ICollection`1"></see>.</summary>
        /// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"></see> is read-only.</exception>
        public void Clear()
            => _dictionary.Clear();

        /// <summary>Returns an enumerator that iterates through the collection.</summary>
        /// <returns>An enumerator that can be used to iterate through the collection.</returns>
        public IEnumerator<KeyValuePair<OasExpression, OasReferable<OasPath>>> GetEnumerator()
            => _dictionary.GetEnumerator();

        /// <summary>Removes the first occurrence of a specific object from the <see cref="T:System.Collections.Generic.ICollection`1"></see>.</summary>
        /// <param name="item">The object to remove from the <see cref="T:System.Collections.Generic.ICollection`1"></see>.</param>
        /// <returns>true if <paramref name="item">item</paramref> was successfully removed from the <see cref="T:System.Collections.Generic.ICollection`1"></see>; otherwise, false. This method also returns false if <paramref name="item">item</paramref> is not found in the original <see cref="T:System.Collections.Generic.ICollection`1"></see>.</returns>
        /// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"></see> is read-only.</exception>
        bool ICollection<KeyValuePair<OasExpression, OasReferable<OasPath>>>.Remove(KeyValuePair<OasExpression, OasReferable<OasPath>> item)
            => ((ICollection<KeyValuePair<OasExpression, OasReferable<OasPath>>>)_dictionary).Remove(item);

        /// <summary>Copies the elements of the <see cref="T:System.Collections.Generic.ICollection`1"></see> to an <see cref="T:System.Array"></see>, starting at a particular <see cref="T:System.Array"></see> index.</summary>
        /// <param name="array">The one-dimensional <see cref="T:System.Array"></see> that is the destination of the elements copied from <see cref="T:System.Collections.Generic.ICollection`1"></see>. The <see cref="T:System.Array"></see> must have zero-based indexing.</param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="array">array</paramref> is null.</exception>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        ///   <paramref name="arrayIndex">arrayIndex</paramref> is less than 0.</exception>
        /// <exception cref="T:System.ArgumentException">The number of elements in the source <see cref="T:System.Collections.Generic.ICollection`1"></see> is greater than the available space from <paramref name="arrayIndex">arrayIndex</paramref> to the end of the destination <paramref name="array">array</paramref>.</exception>
        void ICollection<KeyValuePair<OasExpression, OasReferable<OasPath>>>.CopyTo(KeyValuePair<OasExpression, OasReferable<OasPath>>[] array, int arrayIndex)
            => ((ICollection<KeyValuePair<OasExpression, OasReferable<OasPath>>>)_dictionary).CopyTo(array, arrayIndex);

        /// <summary>Determines whether the <see cref="T:System.Collections.Generic.ICollection`1"></see> contains a specific value.</summary>
        /// <param name="item">The object to locate in the <see cref="T:System.Collections.Generic.ICollection`1"></see>.</param>
        /// <returns>true if <paramref name="item">item</paramref> is found in the <see cref="T:System.Collections.Generic.ICollection`1"></see>; otherwise, false.</returns>
        bool ICollection<KeyValuePair<OasExpression, OasReferable<OasPath>>>.Contains(KeyValuePair<OasExpression, OasReferable<OasPath>> item)
            => ((ICollection<KeyValuePair<OasExpression, OasReferable<OasPath>>>)_dictionary).Contains(item);

        /// <summary>Adds an item to the <see cref="T:System.Collections.Generic.ICollection`1"></see>.</summary>
        /// <param name="item">The object to add to the <see cref="T:System.Collections.Generic.ICollection`1"></see>.</param>
        /// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"></see> is read-only.</exception>
        void ICollection<KeyValuePair<OasExpression, OasReferable<OasPath>>>.Add(KeyValuePair<OasExpression, OasReferable<OasPath>> item)
            => _dictionary.Add(item.Key, item.Value);

        /// <summary>Returns an enumerator that iterates through a collection.</summary>
        /// <returns>An <see cref="T:System.Collections.IEnumerator"></see> object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator() =>
            _dictionary.GetEnumerator();

        #endregion
    }
}
