#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using SourceCode.Clay.Collections.Generic;
using SourceCode.Clay.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Json;

namespace SourceCode.Clay.OpenApi
{
    /// <summary>
    ///
    /// </summary>
    internal sealed class VendorExtensions : IReadOnlyDictionary<string, JsonValue>, IEquatable<VendorExtensions>
    {
        #region Fields

        private readonly ReadOnlyJsonObject _json;

        #endregion

        #region Properties

        public int Count => _json.Count;

        public JsonValue this[string key]
        {
            get
            {
                var value = _json[key];
                var json = JsonValue.Parse(value);
                return json;
            }
        }

        #endregion

        #region Constructors

        internal VendorExtensions(JsonObject json, params string[] wellKnownKeys)
        {
            if (json == null || json.Count == 0)
            {
                _json = new ReadOnlyJsonObject();
                return;
            }

            var keys = new HashSet<string>(wellKnownKeys, StringComparer.Ordinal); // Expensive!

            var jobj = new JsonObject();
            foreach (var item in json)
            {
                // Don't add well-known properties
                if (keys.Contains(item.Key)) continue;

                // Leverage error handling in Add()
                jobj.Add(item.Key, item.Value);
            }

            _json = new ReadOnlyJsonObject(jobj);
        }

        #endregion

        #region Methods

        public bool ContainsKey(string key) => _json.ContainsKey(key);

        public bool TryGetValue(string key, out JsonValue value)
        {
            value = default;

            if (!_json.TryGetValue(key, out var str))
                return false;

            value = str;
            return true;
        }

        #endregion

        #region IEnumerable

        public IEnumerable<JsonValue> Values
        {
            get
            {
                foreach (var item in _json)
                    yield return item.Value;
            }
        }

        public IEnumerable<string> Keys => _json.Keys;

        public IEnumerator<KeyValuePair<string, JsonValue>> GetEnumerator()
        {
            foreach (var item in _json)
                yield return new KeyValuePair<string, JsonValue>(item.Key, item.Value);
        }

        IEnumerator IEnumerable.GetEnumerator() => _json.GetEnumerator();

        #endregion

        #region IEquatable

        /// <summary>
        /// Implements the operator == operator.
        /// </summary>
        /// <param name="x">The first item.</param>
        /// <param name="y">The second item.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator ==(VendorExtensions x, VendorExtensions y)
        {
            if (x is null) return y is null;
            if (y is null) return false;

            return x.Equals(y);
        }

        /// <summary>
        /// Implements the operator != operator.
        /// </summary>
        /// <param name="x">The first item.</param>
        /// <param name="y">The second item.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator !=(VendorExtensions x, VendorExtensions y) => !(x == y);

        /// <summary>Indicates whether this instance and a specified object are equal.</summary>
        /// <param name="obj">The object to compare with the current instance.</param>
        /// <returns>true if <paramref name="obj">obj</paramref> and this instance are the same type and represent the same value; otherwise, false.</returns>
        public override bool Equals(object obj)
            => obj is VendorExtensions other
            && Equals(other);

        /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>true if the current object is equal to the <paramref name="other">other</paramref> parameter; otherwise, false.</returns>
        public bool Equals(VendorExtensions other)
            => _json.NullableJsonEquals(other?._json);

        /// <summary>Returns the hash code for this instance.</summary>
        /// <returns>A 32-bit signed integer that is the hash code for this instance.</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                var hc = 17L;

                hc = (hc * 23) + _json.GetHashCode();

                return ((int)(hc >> 32)) ^ (int)hc;
            }
        }

        #endregion
    }
}
