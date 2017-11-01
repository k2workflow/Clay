#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using System;
using System.Collections.Generic;
using System.Json;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;

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

        #region Equals

        public static bool NullableJsonEquals(this JsonArray x, JsonArray y)
        {
            if (x is null) return y is null; // (null, null) or (null, y)
            if (y is null) return false; // (x, null)
            if (ReferenceEquals(x, y)) return true; // (x, x)

            // Item count
            if (x.Count != y.Count) return false; // (n, m)
            if (x.Count == 0) return true; // (0, 0)

            // Values
            // Avoid string allocs by enumerating the array
            for (var i = 0; i < x.Count; i++)
            {
                if (!JsonEqualsImpl(x[i], y[i])) return false;
            }

            return true;
        }

        public static bool NullableJsonEquals(this JsonObject x, JsonObject y)
        {
            if (x is null) return y is null; // (null, null) or (null, y)
            if (y is null) return false; // (x, null)
            if (ReferenceEquals(x, y)) return true; // (x, x)

            // Property count
            if (x.Count != y.Count) return false; // (n, m)
            if (x.Count == 0) return true; // (0, 0)

            // Value
            // Avoid string allocs by enumerating the properties
            using (var xe = x.GetEnumerator())
            using (var ye = y.GetEnumerator())
            {
                while (xe.MoveNext())
                {
                    if (!ye.MoveNext()) return false;

                    if (!JsonEqualsImpl(xe.Current, ye.Current)) return false;
                }

                return !ye.MoveNext();
            }
        }

        public static bool NullableJsonEquals(this JsonPrimitive x, JsonPrimitive y)
        {
            if (x is null) return y is null; // (null, null) or (null, y)
            if (y is null) return false; // (x, null)
            if (ReferenceEquals(x, y)) return true; // (x, x)

            // JsonType
            if (x.JsonType != y.JsonType) return false;

            // We could also compare the ToString() values here, though that incurs at
            // least 1 string alloc, the size of which will depend on the specific primitive.
            // On the other hand Linq expressions are slower than native code, and the
            // implementation requires runtime resolution of Equals(obj, obj).
            // TODO: May be worth benchmarking (if not a micro-optimization)

            // Value
            var xv = GetValueFromPrimitive(x);
            var yv = GetValueFromPrimitive(y);
            if (!xv.Equals(yv)) return false; // Runtime Object comparison

            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool JsonEqualsImpl(KeyValuePair<string, JsonValue> x, KeyValuePair<string, JsonValue> y)
        {
            if (x.Equals(default)) return y.Equals(default); // (null, null) or (null, y)
            if (y.Equals(default)) return false; // (x, null)

            // Key
            if (!StringComparer.Ordinal.Equals(x.Key, y.Key)) return false;

            // Value
            if (!JsonEqualsImpl(x.Value, y.Value)) return false;

            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool JsonEqualsImpl(JsonValue x, JsonValue y)
        {
            if (x is null) return y is null; // (null, null) or (null, y)
            if (y is null) return false; // (x, null)
            if (ReferenceEquals(x, y)) return true; // (x, x)

            // We could also compare the ToString() values here, though that incurs at
            // least 1 string alloc, the size of which will depend on the specific type.
            // For example a JsonArray(n) would incur at least n allocs, while a
            // JsonPrimitive would incur at least 1.

            // Most likely
            if (x is JsonPrimitive xp)
                return (y is JsonPrimitive yp) && xp.NullableJsonEquals(yp);

            if (x is JsonObject xo)
                return (y is JsonObject yo) && xo.NullableJsonEquals(yo);

            // Least likely
            if (x is JsonArray xa)
                return (y is JsonArray ya) && xa.NullableJsonEquals(ya);

            return true;
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
