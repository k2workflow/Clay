#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using SourceCode.Clay.Collections.Generic;
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
    public class OasCallback : IReadOnlyDictionary<OasExpression, OasReferable<OasPath>>, IEquatable<OasCallback>
    {
        #region Properties

        private readonly IReadOnlyDictionary<OasExpression, OasReferable<OasPath>> _dictionary;

        /// <summary>Gets an enumerable collection that contains the keys in the read-only dictionary.</summary>
        /// <returns>An enumerable collection that contains the keys in the read-only dictionary.</returns>
        public IEnumerable<OasExpression> Keys => _dictionary.Keys;

        /// <summary>Gets an enumerable collection that contains the values in the read-only dictionary.</summary>
        /// <returns>An enumerable collection that contains the values in the read-only dictionary.</returns>
        public IEnumerable<OasReferable<OasPath>> Values => _dictionary.Values;

        /// <summary>Gets the number of elements in the collection.</summary>
        /// <returns>The number of elements in the collection.</returns>
        public int Count => _dictionary.Count;

        /// <summary>Gets the element that has the specified key in the read-only dictionary.</summary>
        /// <param name="key">The key to locate.</param>
        /// <returns>The element that has the specified key in the read-only dictionary.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="key">key</paramref> is null.</exception>
        /// <exception cref="T:System.Collections.Generic.KeyNotFoundException">The property is retrieved and <paramref name="key">key</paramref> is not found.</exception>
        public OasReferable<OasPath> this[OasExpression key] => _dictionary[key];

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of the <see cref="OasCallback"/> class.
        /// </summary>
        /// <param name="paths">The dictionary containing expressions and paths.</param>
        public OasCallback(IReadOnlyDictionary<OasExpression, OasReferable<OasPath>> paths = default)
        {
            _dictionary = paths ?? Dictionary.ReadOnlyEmpty<OasExpression, OasReferable<OasPath>>();
        }

        #endregion

        #region Methods

        /// <summary>Determines whether the read-only dictionary contains an element that has the specified key.</summary>
        /// <param name="key">The key to locate.</param>
        /// <returns>true if the read-only dictionary contains an element that has the specified key; otherwise, false.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="key">key</paramref> is null.</exception>
        public bool ContainsKey(OasExpression key) => _dictionary.ContainsKey(key);

        /// <summary>Gets the value that is associated with the specified key.</summary>
        /// <param name="key">The key to locate.</param>
        /// <param name="value">When this method returns, the value associated with the specified key, if the key is found; otherwise, the default value for the type of the value parameter. This parameter is passed uninitialized.</param>
        /// <returns>true if the object that implements the <see cref="T:System.Collections.Generic.IReadOnlyDictionary`2"></see> interface contains an element that has the specified key; otherwise, false.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="key">key</paramref> is null.</exception>
        public bool TryGetValue(OasExpression key, out OasReferable<OasPath> value) => _dictionary.TryGetValue(key, out value);

        /// <summary>Returns an enumerator that iterates through the collection.</summary>
        /// <returns>An enumerator that can be used to iterate through the collection.</returns>
        public IEnumerator<KeyValuePair<OasExpression, OasReferable<OasPath>>> GetEnumerator() => _dictionary.GetEnumerator();

        /// <summary>Returns an enumerator that iterates through a collection.</summary>
        /// <returns>An <see cref="T:System.Collections.IEnumerator"></see> object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator() => _dictionary.GetEnumerator();

        #endregion

        #region IEquatable

        /// <summary>
        /// Implements the operator == operator.
        /// </summary>
        /// <param name="callback1">The callback1.</param>
        /// <param name="callback2">The callback2.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator ==(OasCallback callback1, OasCallback callback2)
        {
            if (callback1 is null) return callback2 is null;
            return callback1.Equals((object)callback2);
        }

        /// <summary>
        /// Implements the operator != operator.
        /// </summary>
        /// <param name="callback1">The callback1.</param>
        /// <param name="callback2">The callback2.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator !=(OasCallback callback1, OasCallback callback2)
            => !(callback1 == callback2);

        /// <summary>Determines whether the specified object is equal to the current object.</summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns>true if the specified object  is equal to the current object; otherwise, false.</returns>
        public override bool Equals(object obj)
            => obj is OasCallback other
            && Equals(other);

        /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>true if the current object is equal to the <paramref name="other">other</paramref> parameter; otherwise, false.</returns>
        public virtual bool Equals(OasCallback other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;

            if (!this.NullableDictionaryEquals(other)) return false;

            return true;
        }

        /// <summary>Gets as hash code suitable for storing this object in a hash table.</summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode()
        {
            var hc = new HashCode();

            hc.Add(Count);
            if (Count != 0) hc.Add(0);

            return hc.ToHashCode();
        }

        #endregion
    }
}
