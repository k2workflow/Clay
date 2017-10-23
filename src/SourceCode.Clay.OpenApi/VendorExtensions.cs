#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using System;
using System.Collections;
using System.Collections.Generic;
using System.Json;

namespace SourceCode.Clay.OpenApi
{
    /// <summary>
    ///
    /// </summary>
    public sealed class VendorExtensions : IReadOnlyDictionary<string, JsonValue>
    {
        #region Fields

        private readonly JsonObject _json;

        #endregion

        #region Properties

        public int Count => _json.Count;

        public JsonValue this[string key] => _json[key];

        #endregion

        #region Constructors

        public VendorExtensions(JsonObject json, params string[] wellKnownKeys)
        {
            if (json == null || json.Count == 0)
            {
                _json = null;
                return;
            }

            var keys = new HashSet<string>(wellKnownKeys, StringComparer.Ordinal); // Expensive!

            _json = new JsonObject();
            foreach (var item in json)
            {
                // Don't add well-known properties
                if (!keys.Contains(item.Key))
                {
                    var str = item.Value.ToString();
                    var clone = JsonValue.Parse(str); // Expensive!

                    _json.Add(item.Key, clone); // Leverage error handling in Add()
                }
            }
        }

        #endregion

        #region Methods

        public bool ContainsKey(string key) => _json.ContainsKey(key);

        public bool TryGetValue(string key, out JsonValue value) => _json.TryGetValue(key, out value);

        #endregion

        #region IEnumerable

        public IEnumerable<string> Keys => _json.Keys;

        public IEnumerable<JsonValue> Values => _json.Values;

        public IEnumerator<KeyValuePair<string, JsonValue>> GetEnumerator() => _json.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => _json.GetEnumerator();

        #endregion
    }
}
