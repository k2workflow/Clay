#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using Newtonsoft.Json.Linq;
using System;

namespace SourceCode.Clay.Json
{
    public static class JsonExtensions
    {
        #region Try/GetValue

        public static JToken GetValue(this JObject json, string key, JTokenType expectedType, bool nullable)
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

            if (jv.Type != expectedType)
                throw new FormatException($"Json key {key} should have type {expectedType}");

            return jv;
        }

        public static JToken GetValue(this ReadOnlyJObject json, string key, JTokenType expectedType, bool nullable)
        {
            if (string.IsNullOrWhiteSpace(key)) throw new ArgumentNullException(nameof(key));

            return GetValue(json._json, key, expectedType, nullable);
        }

        public static bool TryGetValue(this JObject json, string key, JTokenType expectedType, bool nullable, out JToken value)
        {
            if (string.IsNullOrWhiteSpace(key)) throw new ArgumentNullException(nameof(key));

            value = null;

            if (!json.TryGetValue(key, out var jv))
                return false;

            if (jv == null)
                return nullable;

            // Schema inconsistency should throw (unavoidably)
            if (jv.Type != expectedType)
                throw new FormatException($"Json key {key} should have type {expectedType}");

            value = jv;
            return true;
        }

        public static bool TryGetValue(this ReadOnlyJObject json, string key, JTokenType expectedType, bool nullable, out JToken value)
        {
            if (string.IsNullOrWhiteSpace(key)) throw new ArgumentNullException(nameof(key));

            return TryGetValue(json._json, key, expectedType, nullable, out value);
        }

        #endregion

        #region Try/GetArray

        public static JArray GetArray(this JObject json, string key)
        {
            if (string.IsNullOrWhiteSpace(key)) throw new ArgumentNullException(nameof(key));

            if (!json.TryGetValue(key, out var jv))
                throw new FormatException($"Expected Json key {key}");

#pragma warning disable S1168 // Empty arrays and collections should be returned instead of null
            if (jv == null)
                return null;
#pragma warning restore S1168 // Empty arrays and collections should be returned instead of null

            if (!(jv is JArray ja))
                throw new FormatException($"Json key {key} should have type {nameof(JArray)}");

            return ja;
        }

        public static JArray GetArray(this ReadOnlyJObject json, string key)
        {
            if (string.IsNullOrWhiteSpace(key)) throw new ArgumentNullException(nameof(key));

            return GetArray(json._json, key);
        }

        public static bool TryGetArray(this JObject json, string key, out JArray value)
        {
            if (string.IsNullOrWhiteSpace(key)) throw new ArgumentNullException(nameof(key));

            value = null;

            if (!json.TryGetValue(key, out var jv))
                return false;

            if (jv == null)
                return true;

            if (!(jv is JArray ja))
                throw new FormatException($"Json key {key} should have type {nameof(JArray)}");

            value = ja;
            return true;
        }

        public static bool TryGetArray(this ReadOnlyJObject json, string key, out JArray value)
        {
            if (string.IsNullOrWhiteSpace(key)) throw new ArgumentNullException(nameof(key));

            return TryGetArray(json._json, key, out value);
        }

        #endregion

        #region Try/GetObject

        public static JObject GetObject(this JObject json, string key)
        {
            if (string.IsNullOrWhiteSpace(key)) throw new ArgumentNullException(nameof(key));

            if (!json.TryGetValue(key, out var jv))
                throw new FormatException($"Expected Json key {key}");

#pragma warning disable S1168 // Empty arrays and collections should be returned instead of null
            if (jv == null)
                return null;
#pragma warning restore S1168 // Empty arrays and collections should be returned instead of null

            if (!(jv is JObject o))
                throw new FormatException($"Json key {key} should have type {nameof(JObject)}");

            return o;
        }

        public static JObject GetObject(this ReadOnlyJObject json, string key)
        {
            if (string.IsNullOrWhiteSpace(key)) throw new ArgumentNullException(nameof(key));

            return GetObject(json._json, key);
        }

        public static bool TryGetObject(this JObject json, string key, out JObject value)
        {
            if (string.IsNullOrWhiteSpace(key)) throw new ArgumentNullException(nameof(key));

            value = null;

            if (!json.TryGetValue(key, out var jv))
                return false;

            if (jv == null)
                return true;

            if (!(jv is JObject o))
                throw new FormatException($"Json key {key} should have type {nameof(JObject)}");

            value = o;
            return true;
        }

        public static bool TryGetObject(this ReadOnlyJObject json, string key, out JObject value)
        {
            if (string.IsNullOrWhiteSpace(key)) throw new ArgumentNullException(nameof(key));

            return TryGetObject(json._json, key, out value);
        }

        #endregion

        #region Try/GetPrimitive

        public static JValue GetPrimitive(this JObject json, string key)
        {
            if (string.IsNullOrWhiteSpace(key)) throw new ArgumentNullException(nameof(key));

            if (!json.TryGetValue(key, out var jv))
                throw new FormatException($"Expected Json key {key}");

#pragma warning disable S1168 // Empty arrays and collections should be returned instead of null
            if (jv == null)
                return null;
#pragma warning restore S1168 // Empty arrays and collections should be returned instead of null

            if (!(jv is JValue jp))
                throw new FormatException($"Json key {key} should have type {nameof(JValue)}");

            return jp;
        }

        public static JValue GetPrimitive(this ReadOnlyJObject json, string key)
        {
            if (string.IsNullOrWhiteSpace(key)) throw new ArgumentNullException(nameof(key));

            return GetPrimitive(json._json, key);
        }

        public static bool TryGetPrimitive(this JObject json, string key, out JValue value)
        {
            if (string.IsNullOrWhiteSpace(key)) throw new ArgumentNullException(nameof(key));

            value = null;

            if (!json.TryGetValue(key, out var jv))
                return false;

            if (jv == null)
                return true;

            if (!(jv is JValue jp))
                throw new FormatException($"Json key {key} should have type {nameof(JValue)}");

            value = jp;
            return true;
        }

        public static bool TryGetPrimitive(this ReadOnlyJObject json, string key, out JValue value)
        {
            if (string.IsNullOrWhiteSpace(key)) throw new ArgumentNullException(nameof(key));

            return TryGetPrimitive(json._json, key, out value);
        }

        #endregion

        #region Parse

        public static JArray ParseJArray(this string json)
        {
            if (string.IsNullOrWhiteSpace(json) || json == "null") return default;

            var wire = JArray.Parse(json);
            return wire;
        }

        public static JObject ParseJObject(this string json)
        {
            if (string.IsNullOrWhiteSpace(json) || json == "null") return default;

            var wire = JObject.Parse(json);
            return wire;
        }

        public static JValue ParseJValue(this string json)
        {
            if (string.IsNullOrWhiteSpace(json) || json == "null") return default;

            var wire = JToken.Parse(json);

            if (!(wire is JValue jp))
                throw new FormatException($"Json should have type {nameof(JValue)}");

            return jp;
        }

        #endregion
    }
}
