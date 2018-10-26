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
    /// A readonly version of <see cref="JObject"/>.
    /// </summary>
    public sealed class ReadOnlyJObject : IReadOnlyDictionary<string, JToken>
#pragma warning restore CA1710 // Identifiers should have correct suffix
    {
        internal readonly JObject _json; // Extension methods need direct access to this field

        /// <inheritdoc/>
        public int Count => _json.Count;

        /// <inheritdoc/>
        public JToken this[string key]
        {
            get
            {
                JToken json = _json[key];

                // Clone to avoid call-site mutation post-facto
                if (!(json is null))
                    json = json.DeepClone();

                return json;
            }
        }

        public ReadOnlyJObject(JObject source)
        {
            _json = new JObject();

            if (source is null) return;

            foreach (KeyValuePair<string, JToken> item in source)
            {
                JToken value = item.Value;

                // Clone to avoid call-site mutation post-facto
                if (!(value is null))
                    value = value.DeepClone();

                _json.Add(item.Key, value);
            }
        }

        public ReadOnlyJObject(params KeyValuePair<string, JToken>[] items)
        {
            _json = new JObject();

            if (items is null) return;

            for (var i = 0; i < items.Length; i++)
            {
                JToken value = items[i].Value;

                // Clone to avoid call-site mutation post-facto
                if (!(value is null))
                    value = value.DeepClone();

                _json.Add(items[i].Key, value);
            }
        }

        public ReadOnlyJObject(IEnumerable<KeyValuePair<string, JToken>> items)
        {
            _json = new JObject();

            if (items is null) return;

            foreach (KeyValuePair<string, JToken> kvp in items)
            {
                JToken value = kvp.Value;

                // Clone to avoid call-site mutation post-facto
                if (!(value is null))
                    value = value.DeepClone();

                _json.Add(kvp.Key, value);
            }
        }

        /// <inheritdoc/>
        bool IReadOnlyDictionary<string, JToken>.ContainsKey(string key) => _json.ContainsKey(key);

        /// <inheritdoc/>
        public bool TryGetValue(string key, out JToken value)
        {
            value = null;

            if (!_json.TryGetValue(key, out JToken json))
                return false;

            // Clone to avoid call-site mutation post-facto
            if (!(json is null))
                value = json.DeepClone();

            return true;
        }

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
        /// <param name="nodes"></param>
        public ReadOnlyJObject Merge(IEnumerable<KeyValuePair<string, JToken>> nodes)
        {
            if (nodes is null || !System.Linq.Enumerable.Any(nodes))
                return this;

            if (_json is null || _json.Count == 0)
                return new ReadOnlyJObject(nodes);

            // TODO: This is slow - this and the ctor both enumerate

            var merged = (JObject)_json.DeepClone(); // Source is known to be a JObject
            foreach (KeyValuePair<string, JToken> kvp in nodes)
                merged[kvp.Key] = kvp.Value;

            return new ReadOnlyJObject(merged);
        }

        /// <inheritdoc/>
        IEnumerable<string> IReadOnlyDictionary<string, JToken>.Keys => ((IDictionary<string, JToken>)_json).Keys;

        /// <inheritdoc/>
        IEnumerable<JToken> IReadOnlyDictionary<string, JToken>.Values => ((IDictionary<string, JToken>)_json).Values;

        /// <inheritdoc/>
        IEnumerator<KeyValuePair<string, JToken>> IEnumerable<KeyValuePair<string, JToken>>.GetEnumerator() => _json.GetEnumerator();

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator() => _json.GetEnumerator();

        public override bool Equals(object obj)
            => obj is ReadOnlyJObject other
            && _json.Equals(other._json);

        public override int GetHashCode() => _json.GetHashCode();

        /// <inheritdoc/>
        public override string ToString() => _json.ToString();
    }
}
