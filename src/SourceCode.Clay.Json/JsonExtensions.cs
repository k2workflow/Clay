#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using System;
using System.Json;

namespace SourceCode.Clay.Json
{
    public static class JsonExtensions
    {
        public static JsonValue GetValue(this JsonObject json, string key, JsonType expectedType, bool nullable)
        {
            if (string.IsNullOrWhiteSpace(key)) throw new ArgumentNullException(nameof(key));

            if (!json.TryGetValue(key, out var jv))
                throw new FormatException($"Expected Json key {key}");

            if (jv == null)
            {
#pragma warning disable S1168 // Empty arrays and collections should be returned instead of null
                if (nullable)
                    return null;
#pragma warning restore S1168 // Empty arrays and collections should be returned instead of null

                throw new FormatException($"Json key {key} should have non-null {expectedType} value");
            }

            if (jv.JsonType != expectedType)
                throw new FormatException($"Json key {key} should have type {expectedType}");

            return jv;
        }

        public static JsonValue GetValue(this ReadOnlyJsonObject json, string key, JsonType expectedType, bool nullable)
        {
            if (string.IsNullOrWhiteSpace(key)) throw new ArgumentNullException(nameof(key));

            return json._json.GetValue(key, expectedType, nullable);
        }

        public static bool TryGetValue(this JsonObject json, string key, JsonType expectedType, bool nullable, out JsonValue value)
        {
            if (string.IsNullOrWhiteSpace(key)) throw new ArgumentNullException(nameof(key));

            value = null;

            if (!json.TryGetValue(key, out var jv))
                return false;

            if (jv == null)
                return nullable;

            // Schema inconsistency should throw (unavoidably)
            if (jv.JsonType != expectedType)
                throw new FormatException($"Json key {key} should have type {expectedType}");

            value = jv;
            return true;
        }

        public static bool TryGetValue(this ReadOnlyJsonObject json, string key, JsonType expectedType, bool nullable, out JsonValue value)
        {
            if (string.IsNullOrWhiteSpace(key)) throw new ArgumentNullException(nameof(key));

            return json._json.TryGetValue(key, expectedType, nullable, out value);
        }

        public static JsonObject GetObject(this JsonObject json, string key)
        {
            if (string.IsNullOrWhiteSpace(key)) throw new ArgumentNullException(nameof(key));

            if (!json.TryGetValue(key, out var jv))
                throw new FormatException($"Expected Json key {key}");

#pragma warning disable S1168 // Empty arrays and collections should be returned instead of null
            if (jv == null)
                return null;
#pragma warning restore S1168 // Empty arrays and collections should be returned instead of null

            if (!(jv is JsonObject o))
                throw new FormatException($"Json key {key} should have type {nameof(JsonObject)}");

            return o;
        }

        public static JsonObject GetObject(this ReadOnlyJsonObject json, string key)
        {
            if (string.IsNullOrWhiteSpace(key)) throw new ArgumentNullException(nameof(key));

            return json._json.GetObject(key);
        }

        public static bool TryGetObject(this JsonObject json, string key, out JsonObject value)
        {
            if (string.IsNullOrWhiteSpace(key)) throw new ArgumentNullException(nameof(key));

            value = null;

            if (!json.TryGetValue(key, out var jv))
                return false;

            if (jv == null)
                return true;

            if (!(jv is JsonObject o))
                throw new FormatException($"Json key {key} should have type {nameof(JsonObject)}");

            value = o;
            return true;
        }

        public static bool TryGetObject(this ReadOnlyJsonObject json, string key, out JsonObject value)
        {
            if (string.IsNullOrWhiteSpace(key)) throw new ArgumentNullException(nameof(key));

            return json._json.TryGetObject(key, out value);
        }

        public static JsonArray GetArray(this JsonObject json, string key)
        {
            if (string.IsNullOrWhiteSpace(key)) throw new ArgumentNullException(nameof(key));

            if (!json.TryGetValue(key, out var jv))
                throw new FormatException($"Expected Json key {key}");

#pragma warning disable S1168 // Empty arrays and collections should be returned instead of null
            if (jv == null)
                return null;
#pragma warning restore S1168 // Empty arrays and collections should be returned instead of null

            if (!(jv is JsonArray ja))
                throw new FormatException($"Json key {key} should have type {nameof(JsonArray)}");

            return ja;
        }

        public static JsonArray GetArray(this ReadOnlyJsonObject json, string key)
        {
            if (string.IsNullOrWhiteSpace(key)) throw new ArgumentNullException(nameof(key));

            return json._json.GetArray(key);
        }

        public static bool TryGetArray(this JsonObject json, string key, out JsonArray value)
        {
            if (string.IsNullOrWhiteSpace(key)) throw new ArgumentNullException(nameof(key));

            value = null;

            if (!json.TryGetValue(key, out var jv))
                return false;

            if (jv == null)
                return true;

            if (!(jv is JsonArray ja))
                throw new FormatException($"Json key {key} should have type {nameof(JsonArray)}");

            value = ja;
            return true;
        }

        public static bool TryGetArray(this ReadOnlyJsonObject json, string key, out JsonArray value)
        {
            if (string.IsNullOrWhiteSpace(key)) throw new ArgumentNullException(nameof(key));

            return json._json.TryGetArray(key, out value);
        }

        public static JsonObject ParseJsonObject(this string json)
        {
            if (string.IsNullOrWhiteSpace(json) || json == "null") return default;

            var wire = JsonValue.Parse(json);

            if (!(wire is JsonObject jo))
                throw new FormatException($"Json should have type {nameof(JsonObject)}");

            return jo;
        }

        public static JsonArray ParseJsonArray(this string json)
        {
            if (string.IsNullOrWhiteSpace(json) || json == "null") return default;

            var wire = JsonValue.Parse(json);

            if (!(wire is JsonArray ja))
                throw new FormatException($"Json should have type {nameof(JsonArray)}");

            return ja;
        }

        public static JsonValue Clone(this JsonValue json)
        {
            if (json == null) return default;

            var str = json.ToString();
            var clone = JsonValue.Parse(str);
            return clone;
        }

        public static ReadOnlyJsonObject Clone(this ReadOnlyJsonObject json)
        {
            if (json == null) return default;

            var jo = (JsonObject)json._json.Clone();
            var clone = new ReadOnlyJsonObject(jo);
            return clone;
        }

        public static bool NullableJsonEquals(this JsonValue x, JsonValue y)
        {
            if (x is null) return y is null; // (null, null) or (null, y)
            if (y is null) return false; // (x, null)
            if (ReferenceEquals(x, y)) return true; // (x, x)

            // Expensive but reliable
            var xs = x.ToString();
            var ys = y.ToString();
            if (!StringComparer.Ordinal.Equals(xs, ys)) return false;

            return true;
        }
    }
}
