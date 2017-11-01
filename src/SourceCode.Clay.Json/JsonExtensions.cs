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

        private static readonly Type typeGuid = typeof(Guid);
        private static readonly Type typeUri = typeof(Uri);
        private static readonly Type typeDateTimeOffset = typeof(DateTimeOffset);
        private static readonly Type typeTimeSpan = typeof(TimeSpan);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static JsonValue CloneImpl(JsonValue x)
        {
            if (x is null) return default;

            // We could roundtrip via ToString() here, though that incurs at
            // least 1 string alloc, the size of which will depend on the specific type.
            // For example a JsonArray(n) would incur at least n allocs, while a
            // JsonPrimitive would incur at least 1.
            // So instead we walk the tree, cloning each item. Note that ToString()
            // walks the tree regardless, so we can be sure this is not significantly
            // more expensive than the latter method.

            // Heuristic: Check for most common first
            if (x is JsonPrimitive xp)
                return ClonePrimitive(xp);

            if (x is JsonObject xo)
                return CloneObject(xo);

            // Least common last
            if (x is JsonArray xa)
                return CloneArray(xa);

            return true;

            // Local functions

            JsonArray CloneArray(JsonArray a)
            {
                if (a is null) return default;

                // Item count
                if (a.Count == 0) return new JsonArray(); // Hopefully JsonArray can one day allocate zero length internally

                // Values
                // Avoid string allocs by enumerating array members
                var list = new JsonValue[a.Count];
                for (var i = 0; i < a.Count; i++)
                {
                    var clone = CloneImpl(a[i]); // Recurse
                    list[i] = clone;
                }

                var array = new JsonArray(list); // Hopefully JsonArray will one day allocate according to input length
                return array;
            }

            JsonObject CloneObject(JsonObject a)
            {
                if (a is null) return default;

                // Property count
                if (a.Count == 0) return new JsonObject(); // Hopefully JsonObject can one day allocate zero length internally

                // Value
                // Avoid string allocs by enumerating properties
                var list = new KeyValuePair<string, JsonValue>[a.Count];
                var i = 0;
                foreach (var ae in a)
                {
                    var clone = CloneImpl(ae.Value); // Recurse
                    list[i++] = new KeyValuePair<string, JsonValue>(ae.Key, clone);
                }

                var obj = new JsonObject(list); // Hopefully JsonObject will one day allocate according to input length
                return obj;
            }

            JsonPrimitive ClonePrimitive(JsonPrimitive a)
            {
                if (a is null) return default;

                // We could also roundtrip via ToString() here, though that incurs at
                // least 1 string alloc, the size of which will depend on the specific primitive.

                // Value
                var av = GetValueFromPrimitive(a);
                var type = av.GetType();
                var typeCode = Type.GetTypeCode(type);

                switch (typeCode)
                {
                    // Scalar
                    case TypeCode.Boolean: return new JsonPrimitive((bool)av);

                    // Signed
                    case TypeCode.SByte: return new JsonPrimitive((sbyte)av);
                    case TypeCode.Int16: return new JsonPrimitive((short)av);
                    case TypeCode.Int32: return new JsonPrimitive((int)av);
                    case TypeCode.Int64: return new JsonPrimitive((long)av);

                    // Unsigned
                    case TypeCode.Byte: return new JsonPrimitive((byte)av);
                    case TypeCode.UInt16: return new JsonPrimitive((ushort)av);
                    case TypeCode.UInt32: return new JsonPrimitive((uint)av);
                    case TypeCode.UInt64: return new JsonPrimitive((ulong)av);

                    // Numeric
                    case TypeCode.Single: return new JsonPrimitive((float)av);
                    case TypeCode.Double: return new JsonPrimitive((double)av);
                    case TypeCode.Decimal: return new JsonPrimitive((decimal)av);

                    // Text
                    case TypeCode.Char: return new JsonPrimitive((char)av);
                    case TypeCode.String: return new JsonPrimitive((string)av);

                    // Temporal
                    case TypeCode.DateTime: return new JsonPrimitive((DateTime)av);

                    default:
                        {
                            // Scalar
                            if (type == typeGuid) return new JsonPrimitive((Guid)av);

                            // Uri
                            if (type == typeUri) return new JsonPrimitive((Uri)av);

                            // Temporal
                            if (type == typeDateTimeOffset) return new JsonPrimitive((DateTimeOffset)av);
                            if (type == typeTimeSpan) return new JsonPrimitive((TimeSpan)av);

                            throw new InvalidCastException($"Unexpected {nameof(JsonPrimitive)} {nameof(Type)} {type}");
                        }
                }
            }
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
