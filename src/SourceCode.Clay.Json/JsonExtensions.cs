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

        public static JsonValue Clone(this JsonValue json) => CloneImpl(json);

        public static ReadOnlyJsonObject Clone(this ReadOnlyJsonObject json)
        {
            if (json == null) return default; // Null
            if (json._json == null) return new ReadOnlyJsonObject(); // Empty

            var jo = (JsonObject)CloneImpl(json._json); // Source is known to be a JsonObject
            var clone = new ReadOnlyJsonObject(jo);
            return clone;
        }

        /*
        We could roundtrip via ToString() here, though that incurs at
        least 1 string alloc, the size of which will depend on the specific type.
        For example a JsonArray(n) would incur at least n allocs, while a
        JsonPrimitive would incur at least 1.

        So instead we walk the tree, cloning each item. Note that ToString()
        walks the tree regardless, so we can be sure this is not significantly
        more expensive than the latter method.

        Benchmarks validate this approach: 2-3x improvement in both time & memory
        https://github.com/dotnet/corefx/issues/25022

                 Method |     Mean |     Error |    StdDev | Scaled | ScaledSD |     Gen 0 |    Gen 1 |   Gen 2 | Allocated |
        --------------- |---------:|----------:|----------:|-------:|---------:|----------:|---------:|--------:|----------:|
          ToStringClone | 29.21 ms | 0.5792 ms | 0.5948 ms |   1.00 |     0.00 | 1562.5000 | 812.5000 | 62.5000 |   9.92 MB |
        NewtonDeepClone | 19.16 ms | 0.3727 ms | 0.5101 ms |   0.66 |     0.02 | 1218.7500 | 562.5000 | 62.5000 |   7.02 MB |
             SmartClone | 10.14 ms | 0.1964 ms | 0.4311 ms |   0.35 |     0.02 |  671.8750 | 312.5000 |       - |   3.94 MB |
        */

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static JsonValue CloneImpl(JsonValue json)
        {
            if (json is null) return default;

            // Heuristic: We check in order of most-to-least likely/abundant

            if (json is JsonPrimitive jp)
            {
                // We could also roundtrip via ToString() here, though that incurs at
                // least 1 string alloc, the size of which will depend on the specific primitive.

                var jv = GetValueFromPrimitive(jp); // Might cause extra allocs
                switch (jv)
                {
                    // Most likely
                    case string st: return new JsonPrimitive(st);
                    case bool tf: return new JsonPrimitive(tf);
                    case int i32: return new JsonPrimitive(i32);
                    case double f64: return new JsonPrimitive(f64);
                    case DateTime dt: return new JsonPrimitive(dt);
                    case DateTimeOffset dto: return new JsonPrimitive(dto);

                    // Scalar
                    case Guid gd: return new JsonPrimitive(gd);

                    // Signed
                    case sbyte i8: return new JsonPrimitive(i8);
                    case short i16: return new JsonPrimitive(i16);
                    case long i64: return new JsonPrimitive(i64);

                    // Unsigned
                    case byte u8: return new JsonPrimitive(u8);
                    case ushort u16: return new JsonPrimitive(u16);
                    case uint u32: return new JsonPrimitive(u32);
                    case ulong u64: return new JsonPrimitive(u64);

                    // Numeric
                    case float f32: return new JsonPrimitive(f32);
                    case decimal dc: return new JsonPrimitive(dc);

                    // Text
                    case char ch: return new JsonPrimitive(ch);

                    // Temporal
                    case TimeSpan ts: return new JsonPrimitive(ts);

                    // Uri
                    case Uri uri: return new JsonPrimitive(uri);

                    default:
                        throw new InvalidCastException($"Unexpected {nameof(JsonPrimitive)} {nameof(Type)} {jv.GetType()}");
                }
            }

            if (json is JsonObject jo)
            {
                var jCount = jo.Count;
                if (jCount == 0) return new JsonObject(); // Hopefully JsonObject can one day allocate zero length internally

                var array = new KeyValuePair<string, JsonValue>[jCount];
                var i = 0;
                foreach (var xi in jo)
                {
                    // Recurse
                    var clone = CloneImpl(xi.Value);
                    array[i++] = new KeyValuePair<string, JsonValue>(xi.Key, clone);
                }

                return new JsonObject(array); // Hopefully JsonObject will one day allocate according to input length
            }

            if (json is JsonArray ja)
            {
                var jCount = ja.Count;
                if (jCount == 0) return new JsonArray(); // Hopefully JsonArray can one day allocate zero length internally

                var array = new JsonValue[jCount];
                for (var i = 0; i < jCount; i++)
                {
                    // Recurse
                    array[i] = CloneImpl(ja[i]);
                }

                return new JsonArray(array); // Hopefully JsonArray will one day allocate according to input length
            }

            // Fallback for unknown subclasses of JsonValue
            return JsonValue.Parse(json.ToString());
        }

        #endregion

        #region Value

        // These helpers would not be necessary if JsonPrimitive.Value was public

        internal static readonly Func<JsonPrimitive, object> GetValueFromPrimitive = GetValueFromPrimitiveImpl();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
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
