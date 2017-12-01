#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;

namespace SourceCode.Clay.Json
{
#pragma warning disable CA1710 // Identifiers should have correct suffix

    /// <summary>
    /// A readonly version of <see cref="JsonObject"/>.
    /// </summary>
    public sealed class ReadOnlyJObject : IReadOnlyDictionary<string, JToken>
#pragma warning restore CA1710 // Identifiers should have correct suffix
    {
        #region Fields

        internal readonly JObject _json; // Extension methods need direct access to this field

        #endregion

        #region Properties

        /// <inheritdoc/>
        public int Count => _json.Count;

        /// <inheritdoc/>
        public JToken this[string key]
        {
            get
            {
                var json = _json[key];

                // Clone to avoid call-site mutation post-facto
                if (json != null)
                    json = json.DeepClone();

                return json;
            }
        }

        #endregion

        #region Constructors

        public ReadOnlyJObject(JObject source)
        {
            _json = new JObject();

            if (source == null) return;

            foreach (var item in source)
            {
                var value = item.Value;

                // Clone to avoid call-site mutation post-facto
                if (value != null)
                    value = value.DeepClone();

                _json.Add(item.Key, value);
            }
        }

        public ReadOnlyJObject(params KeyValuePair<string, JToken>[] items)
        {
            _json = new JObject();

            if (items == null) return;

            for (var i = 0; i < items.Length; i++)
            {
                var value = items[i].Value;

                // Clone to avoid call-site mutation post-facto
                if (value != null)
                    value = value.DeepClone();

                _json.Add(items[i].Key, value);
            }
        }

        public ReadOnlyJObject(IEnumerable<KeyValuePair<string, JToken>> items)
        {
            _json = new JObject();

            if (items == null) return;

            foreach (var kvp in items)
            {
                var value = kvp.Value;

                // Clone to avoid call-site mutation post-facto
                if (value != null)
                    value = value.DeepClone();

                _json.Add(kvp.Key, value);
            }
        }

        #endregion

        #region IReadOnlyDictionary

        /// <inheritdoc/>
        bool IReadOnlyDictionary<string, JToken>.ContainsKey(string key) => ((IDictionary<string, JToken>)_json).ContainsKey(key);

        /// <inheritdoc/>
        public bool TryGetValue(string key, out JToken value)
        {
            value = null;

            if (!_json.TryGetValue(key, out var json))
                return false;

            // Clone to avoid call-site mutation post-facto
            if (json != null)
                value = json.DeepClone();

            return true;
        }

        #endregion

        #region Methods

        public ReadOnlyJObject DeepClone()
        {
            var json = (JObject)_json.DeepClone(); // Source is known to be a JObject
            var clone = new ReadOnlyJObject(json);
            return clone;
        }

        /// <summary>
        /// Make a copy of the <see cref="ReadOnlyJObject"/> as a <see cref="JObject"/>.
        /// </summary>
        /// <returns></returns>
        public JObject ToJObject() => (JObject)_json.DeepClone(); // Source is known to be a JObject

        /// <summary>
        /// Merge the specified nodes into the current <see cref="ReadOnlyJObject"/>.
        /// </summary>
        /// <param name="extra"></param>
        public ReadOnlyJObject Merge(IEnumerable<KeyValuePair<string, JToken>> nodes)
        {
            if (nodes == null || !System.Linq.Enumerable.Any(nodes))
                return this;

            if (_json == null || _json.Count == 0)
                return new ReadOnlyJObject(nodes);

            // TODO: This is slow - this and the ctor both enumerate

            var merged = (JObject)_json.DeepClone(); // Source is known to be a JObject
            foreach (var kvp in nodes)
                merged[kvp.Key] = kvp.Value;

            return new ReadOnlyJObject(merged);
        }

        #endregion

        #region IEnumerable

        /// <inheritdoc/>
        IEnumerable<string> IReadOnlyDictionary<string, JToken>.Keys => ((IDictionary<string, JToken>)_json).Keys;

        /// <inheritdoc/>
        IEnumerable<JToken> IReadOnlyDictionary<string, JToken>.Values => ((IDictionary<string, JToken>)_json).Values;

        /// <inheritdoc/>
        IEnumerator<KeyValuePair<string, JToken>> IEnumerable<KeyValuePair<string, JToken>>.GetEnumerator() => _json.GetEnumerator();

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator() => _json.GetEnumerator();

        #endregion

        #region Equals

        public override bool Equals(object obj)
            => obj is ReadOnlyJObject other
            && _json.Equals(other._json);

        public override int GetHashCode() => _json.GetHashCode();

        #endregion

        #region Operators

        /// <inheritdoc/>
        public override string ToString() => _json.ToString();

        #endregion
    }
}
