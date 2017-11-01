#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using System.Collections;
using System.Collections.Generic;
using System.Json;

namespace SourceCode.Clay.Json
{
#pragma warning disable CA1710 // Identifiers should have correct suffix

    /// <summary>
    /// A readonly version of <see cref="JsonObject"/>.
    /// </summary>
    public sealed class ReadOnlyJsonObject : IReadOnlyDictionary<string, JsonValue>
#pragma warning restore CA1710 // Identifiers should have correct suffix
    {
        #region Fields

        internal readonly JsonObject _json; // Extension methods need direct access to this field

        #endregion

        #region Properties

        /// <inheritdoc/>
        public int Count => _json.Count;

        /// <inheritdoc/>
        public JsonValue this[string key]
        {
            get
            {
                var json = _json[key];

                // Clone to avoid call-site mutation post-facto
                if (json != null)
                    json = json.Clone();

                return json;
            }
        }

        #endregion

        #region Constructors

        public ReadOnlyJsonObject(params KeyValuePair<string, JsonValue>[] items)
        {
            _json = new JsonObject();

            if (items == null) return;

            for (var i = 0; i < items.Length; i++)
            {
                var value = items[i].Value;

                // Clone to avoid call-site mutation post-facto
                if (value != null)
                    value = value.Clone();

                _json.Add(items[i].Key, value);
            }
        }

        public ReadOnlyJsonObject(IEnumerable<KeyValuePair<string, JsonValue>> items)
        {
            _json = new JsonObject();

            if (items == null) return;

            foreach (var kvp in items)
            {
                var value = kvp.Value;

                // Clone to avoid call-site mutation post-facto
                if (value != null)
                    value = value.Clone();

                _json.Add(kvp.Key, value);
            }
        }

        #endregion

        #region IReadOnlyDictionary

        /// <inheritdoc/>
        public bool ContainsKey(string key) => _json.ContainsKey(key);

        /// <inheritdoc/>
        public bool TryGetValue(string key, out JsonValue value)
        {
            value = null;

            if (!_json.TryGetValue(key, out var json))
                return false;

            // Clone to avoid call-site mutation post-facto
            if (json != null)
                json = json.Clone();

            value = json;
            return true;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Make a copy of the <see cref="ReadOnlyJsonObject"/> as a <see cref="JsonObject"/>.
        /// </summary>
        /// <returns></returns>
        public JsonObject ToJsonObject() => (JsonObject)_json.Clone(); // Source is known to be a JsonObject

        #endregion

        #region IEnumerable

        /// <inheritdoc/>
        public IEnumerable<string> Keys => _json.Keys;

        /// <inheritdoc/>
        public IEnumerable<JsonValue> Values
        {
            get
            {
                // Clone to avoid call-site mutation post-facto
                foreach (var item in _json)
                    yield return item.Value.Clone();
            }
        }

        /// <inheritdoc/>
        public IEnumerator<KeyValuePair<string, JsonValue>> GetEnumerator()
        {
            // Clone to avoid call-site mutation post-facto
            foreach (var item in _json)
                yield return new KeyValuePair<string, JsonValue>(item.Key, item.Value.Clone());
        }

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        #endregion

        #region Equals

        public override bool Equals(object obj)
            => obj is ReadOnlyJsonObject other
            && JsonValueComparer.Default.Equals(this, other);

        public override int GetHashCode() => _json.GetHashCode();

        #endregion

        #region Operators

        /// <inheritdoc/>
        public override string ToString() => _json.ToString();

        #endregion
    }
}
