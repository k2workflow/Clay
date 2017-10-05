using SourceCode.Clay.Collections.Generic;
using SourceCode.Clay.OpenApi.Expressions;
using System;
using System.Collections;
using System.Collections.Generic;

namespace SourceCode.Clay.OpenApi
{
    /// <summary>
    /// A map of possible out-of band callbacks related to the parent operation. Each value in the map is a
    /// <see cref="Path"/> that describes a set of requests that may be initiated by the API provider and
    /// the expected responses. The key value used to identify the callback object is an expression, evaluated
    /// at runtime, that identifies a URL to use for the callback operation.
    /// </summary>
    public class Callback : IReadOnlyDictionary<CompoundExpression, Referable<Path>>, IEquatable<Callback>
    {
        #region Properties

        private readonly IReadOnlyDictionary<CompoundExpression, Referable<Path>> _dictionary;

        /// <summary>Gets an enumerable collection that contains the keys in the read-only dictionary.</summary>
        /// <returns>An enumerable collection that contains the keys in the read-only dictionary.</returns>
        public IEnumerable<CompoundExpression> Keys => _dictionary.Keys;

        /// <summary>Gets an enumerable collection that contains the values in the read-only dictionary.</summary>
        /// <returns>An enumerable collection that contains the values in the read-only dictionary.</returns>
        public IEnumerable<Referable<Path>> Values => _dictionary.Values;

        /// <summary>Gets the number of elements in the collection.</summary>
        /// <returns>The number of elements in the collection.</returns>
        public int Count => _dictionary.Count;

        /// <summary>Gets the element that has the specified key in the read-only dictionary.</summary>
        /// <param name="key">The key to locate.</param>
        /// <returns>The element that has the specified key in the read-only dictionary.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="key">key</paramref> is null.</exception>
        /// <exception cref="T:System.Collections.Generic.KeyNotFoundException">The property is retrieved and <paramref name="key">key</paramref> is not found.</exception>
        public Referable<Path> this[CompoundExpression key] => _dictionary[key];

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of the <see cref="Callback"/> class.
        /// </summary>
        /// <param name="paths">The dictionary containing expressions and paths.</param>
        public Callback(IReadOnlyDictionary<CompoundExpression, Referable<Path>> paths = default)
        {
            _dictionary = paths ?? Dictionary.ReadOnlyEmpty<CompoundExpression, Referable<Path>>();
        }

        #endregion

        #region Methods

        /// <summary>Determines whether the read-only dictionary contains an element that has the specified key.</summary>
        /// <param name="key">The key to locate.</param>
        /// <returns>true if the read-only dictionary contains an element that has the specified key; otherwise, false.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="key">key</paramref> is null.</exception>
        public bool ContainsKey(CompoundExpression key) => _dictionary.ContainsKey(key);

        /// <summary>Gets the value that is associated with the specified key.</summary>
        /// <param name="key">The key to locate.</param>
        /// <param name="value">When this method returns, the value associated with the specified key, if the key is found; otherwise, the default value for the type of the value parameter. This parameter is passed uninitialized.</param>
        /// <returns>true if the object that implements the <see cref="T:System.Collections.Generic.IReadOnlyDictionary`2"></see> interface contains an element that has the specified key; otherwise, false.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="key">key</paramref> is null.</exception>
        public bool TryGetValue(CompoundExpression key, out Referable<Path> value) => _dictionary.TryGetValue(key, out value);

        /// <summary>Returns an enumerator that iterates through the collection.</summary>
        /// <returns>An enumerator that can be used to iterate through the collection.</returns>
        public IEnumerator<KeyValuePair<CompoundExpression, Referable<Path>>> GetEnumerator() => _dictionary.GetEnumerator();

        /// <summary>Returns an enumerator that iterates through a collection.</summary>
        /// <returns>An <see cref="T:System.Collections.IEnumerator"></see> object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator() => _dictionary.GetEnumerator();

        #endregion

        #region IEquatable

        /// <summary>Determines whether the specified object is equal to the current object.</summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns>true if the specified object  is equal to the current object; otherwise, false.</returns>
        public override bool Equals(object obj) => Equals(obj as Callback);

        /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>true if the current object is equal to the <paramref name="other">other</paramref> parameter; otherwise, false.</returns>
        public virtual bool Equals(Callback other)
        {
            if (ReferenceEquals(other, null)) return false;
            if (ReferenceEquals(this, other)) return true;

            if (!this.DictionaryEquals(other)) return false;

            return true;
        }

        /// <summary>Gets as hash code suitable for storing this object in a hash table.</summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                var hc = 17L;

                hc = hc * 21 + Count;

                return ((int)(hc >> 32)) ^ (int)hc;
            }
        }

        public static bool operator ==(Callback callback1, Callback callback2)
        {
            if (ReferenceEquals(callback1, null) && ReferenceEquals(callback2, null)) return true;
            if (ReferenceEquals(callback1, null) || ReferenceEquals(callback2, null)) return false;
            return callback1.Equals((object)callback2);
        }

        public static bool operator !=(Callback callback1, Callback callback2) => !(callback1 == callback2);

        #endregion
    }
}
