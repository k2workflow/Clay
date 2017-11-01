#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using System;
using System.Json;
using System.Linq.Expressions;
using System.Reflection;

namespace SourceCode.Clay.Json
{
    public static class JsonExtensions
    {
        #region Try/GetValue

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

            return GetValue(json._json, key, expectedType, nullable);
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

            return TryGetValue(json._json, key, expectedType, nullable, out value);
        }

        #endregion

        #region Try/GetArray

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

            return GetArray(json._json, key);
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

            return TryGetArray(json._json, key, out value);
        }

        #endregion

        #region Try/GetObject

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

            return GetObject(json._json, key);
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

            return TryGetObject(json._json, key, out value);
        }

        #endregion

        #region Try/GetPrimitive

        public static JsonPrimitive GetPrimitive(this JsonObject json, string key)
        {
            if (string.IsNullOrWhiteSpace(key)) throw new ArgumentNullException(nameof(key));

            if (!json.TryGetValue(key, out var jv))
                throw new FormatException($"Expected Json key {key}");

#pragma warning disable S1168 // Empty arrays and collections should be returned instead of null
            if (jv == null)
                return null;
#pragma warning restore S1168 // Empty arrays and collections should be returned instead of null

            if (!(jv is JsonPrimitive jp))
                throw new FormatException($"Json key {key} should have type {nameof(JsonPrimitive)}");

            return jp;
        }

        public static JsonPrimitive GetPrimitive(this ReadOnlyJsonObject json, string key)
        {
            if (string.IsNullOrWhiteSpace(key)) throw new ArgumentNullException(nameof(key));

            return GetPrimitive(json._json, key);
        }

        public static bool TryGetPrimitive(this JsonObject json, string key, out JsonPrimitive value)
        {
            if (string.IsNullOrWhiteSpace(key)) throw new ArgumentNullException(nameof(key));

            value = null;

            if (!json.TryGetValue(key, out var jv))
                return false;

            if (jv == null)
                return true;

            if (!(jv is JsonPrimitive jp))
                throw new FormatException($"Json key {key} should have type {nameof(JsonPrimitive)}");

            value = jp;
            return true;
        }

        public static bool TryGetPrimitive(this ReadOnlyJsonObject json, string key, out JsonPrimitive value)
        {
            if (string.IsNullOrWhiteSpace(key)) throw new ArgumentNullException(nameof(key));

            return TryGetPrimitive(json._json, key, out value);
        }

        #endregion

        #region Parse

        public static JsonArray ParseJsonArray(this string json)
        {
            if (string.IsNullOrWhiteSpace(json) || json == "null") return default;

            var wire = JsonValue.Parse(json);

            if (!(wire is JsonArray ja))
                throw new FormatException($"Json should have type {nameof(JsonArray)}");

            return ja;
        }

        public static JsonObject ParseJsonObject(this string json)
        {
            if (string.IsNullOrWhiteSpace(json) || json == "null") return default;

            var wire = JsonValue.Parse(json);

            if (!(wire is JsonObject jo))
                throw new FormatException($"Json should have type {nameof(JsonObject)}");

            return jo;
        }

        public static JsonPrimitive ParseJsonPrimitive(this string json)
        {
            if (string.IsNullOrWhiteSpace(json) || json == "null") return default;

            var wire = JsonValue.Parse(json);

            if (!(wire is JsonPrimitive jp))
                throw new FormatException($"Json should have type {nameof(JsonPrimitive)}");

            return jp;
        }

        #endregion

        #region Clone

        public static JsonValue Clone(this JsonValue json)
        {
            if (json == null) return default;

            // TODO: Reliable but expensive considering the string allocation on the way out and the needless parsing/validation on the way back in.
            // Try to find a more efficient method.
            var str = json.ToString();
            var clone = JsonValue.Parse(str);
            return clone;
        }

        public static ReadOnlyJsonObject Clone(this ReadOnlyJsonObject json)
        {
            if (json == null) return default;

            var jo = (JsonObject)Clone(json._json); // Source is known to be a JsonObject
            var clone = new ReadOnlyJsonObject(jo);
            return clone;
        }

        #endregion

        #region Value

        internal static readonly Func<JsonPrimitive, object> GetValueFromPrimitive = GetValueFromPrimitiveImpl();

        private static Func<JsonPrimitive, object> GetValueFromPrimitiveImpl()
        {
            var prim = typeof(JsonPrimitive);
            var prop = prim.GetProperty("Value", BindingFlags.NonPublic | BindingFlags.Instance);

            var param = Expression.Parameter(prim, "primitive");
            var expr = Expression.Property(param, prop);

            var func = Expression.Lambda<Func<JsonPrimitive, object>>(expr, param);
            return func.Compile();
        }

        #endregion
    }
}
