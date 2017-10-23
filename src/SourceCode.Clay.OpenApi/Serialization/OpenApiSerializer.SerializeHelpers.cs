#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using System;
using System.Collections.Generic;
using System.Json;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;

namespace SourceCode.Clay.OpenApi.Serialization
{
#pragma warning disable S1168 // Empty arrays and collections should be returned instead of null
    // Null is significant in JSON.

    partial class OpenApiSerializer
    {
        #region JsonHelpers

        /// <summary>
        /// Converts the specified dictionary into its object representation.
        /// </summary>
        /// <typeparam name="T">The type of value in the dictionary.</typeparam>
        /// <param name="dictionary">The dictionary.</param>
        /// <param name="required">A value indicating whether an empty object is required.</param>
        /// <returns>The <see cref="JsonObject"/> representing the dictionary.</returns>
        protected virtual JsonObject ToJsonMap<T>(IEnumerable<ValueTuple<string, T>> dictionary, bool required = false)
        {
            if (dictionary == null)
            {
                if (required) return new JsonObject();
                return null;
            }

            using (var e = dictionary.GetEnumerator())
            {
                if (!e.MoveNext())
                {
                    if (required) return new JsonObject();
                    return null;
                }

                var result = new JsonObject();
                do
                {
                    result.Add(e.Current.Item1, Serialize(e.Current.Item2));
                } while (e.MoveNext());

                return result;
            }
        }

        /// <summary>
        /// Converts the specified dictionary into its object representation.
        /// </summary>
        /// <typeparam name="T">The type of value in the dictionary.</typeparam>
        /// <param name="dictionary">The dictionary.</param>
        /// <param name="required">A value indicating whether an empty object is required.</param>
        /// <returns>The <see cref="JsonObject"/> representing the dictionary.</returns>
        protected virtual JsonObject ToJsonMap<T>(IEnumerable<KeyValuePair<string, T>> dictionary, bool required = false)
        {
            if (dictionary == null)
            {
                if (required) return new JsonObject();
                return null;
            }

            using (var e = dictionary.GetEnumerator())
            {
                if (!e.MoveNext())
                {
                    if (required) return new JsonObject();
                    return null;
                }

                var result = new JsonObject();
                do
                {
                    result.Add(e.Current.Key, Serialize(e.Current.Value));
                } while (e.MoveNext());

                return result;
            }
        }

        /// <summary>
        /// Sets a property in the specified <see cref="JsonObject"/>.
        /// </summary>
        /// <typeparam name="T">The type of value in the dictionary.</typeparam>
        /// <param name="container">The object to set the property on.</param>
        /// <param name="key">The property name in <paramref name="container"/>.</param>
        /// <param name="dictionary">The dictionary value.</param>
        /// <param name="required">A value indicating whether an empty object is required.</param>
        protected virtual void SetJsonMap<T>(JsonObject container, string key, IEnumerable<ValueTuple<string, T>> dictionary, bool required = false)
        {
            var result = ToJsonMap(dictionary, required);
            if (result != null) container.Add(key, result);
        }

        /// <summary>
        /// Sets a property in the specified <see cref="JsonObject"/>.
        /// </summary>
        /// <typeparam name="T">The type of value in the dictionary.</typeparam>
        /// <param name="container">The object to set the property on.</param>
        /// <param name="key">The property name in <paramref name="container"/>.</param>
        /// <param name="dictionary">The dictionary value.</param>
        /// <param name="required">A value indicating whether an empty object is required.</param>
        protected virtual void SetJsonMap<T>(JsonObject container, string key, IEnumerable<KeyValuePair<string, T>> dictionary, bool required = false)
        {
            var result = ToJsonMap(dictionary, required);
            if (result != null) container.Add(key, result);
        }

        /// <summary>
        /// Converts the specified list into its array representation.
        /// </summary>
        /// <typeparam name="T">The type of value in the list.</typeparam>
        /// <param name="list">The type of elements in the list.</param>
        /// <param name="required">A value indicating whether an empty array is required.</param>
        /// <returns>The <see cref="JsonArray"/> containing the list.</returns>
        protected virtual JsonArray ToJsonArray<T>(IEnumerable<T> list, bool required = false)
        {
            if (list == null)
            {
                if (required) return new JsonArray();
                return null;
            }

            using (var e = list.GetEnumerator())
            {
                if (!e.MoveNext())
                {
                    if (required) return new JsonArray();
                    return null;
                }

                var result = new JsonArray();
                do
                {
                    result.Add(Serialize(e.Current));
                } while (e.MoveNext());

                return result;
            }
        }

        /// <summary>
        /// Sets a property in the specified <see cref="JsonObject"/>.
        /// </summary>
        /// <typeparam name="T">The type of value in the list.</typeparam>
        /// <param name="container">The object to set the property on.</param>
        /// <param name="key">The property name in <paramref name="container"/>.</param>
        /// <param name="list">The list value.</param>
        /// <param name="required">A value indicating whether an empty array is required.</param>
        protected virtual void SetJsonArray<T>(JsonObject container, string key, IEnumerable<T> list, bool required = false)
        {
            var result = ToJsonArray(list);
            if (result != null) container.Add(key, result);
        }

        /// <summary>
        /// Sets a property in the specified <see cref="JsonObject"/>.
        /// </summary>
        /// <typeparam name="TEnum">The type of the enum.</typeparam>
        /// <param name="container">The object to set the property on.</param>
        /// <param name="key">The property name in <paramref name="container"/>.</param>
        /// <param name="value">The enum flags value.</param>
        /// <param name="flag">The flag to search for.</param>
        /// <param name="defaultState">The default state of the flag.</param>
        /// <param name="required">A value indicating whether the property is required.</param>
        /// <param name="invert">A value indicating whether to invert the flag check.</param>
        protected virtual void SetJsonFlag<TEnum>(JsonObject container, string key, TEnum value, TEnum flag, bool defaultState = false, bool required = false, bool invert = false)
            where TEnum : struct, IComparable, IConvertible, IFormattable
        {
            var v = EnumConvert.ToUInt64(value);
            var f = EnumConvert.ToUInt64(flag);
            var present = (v & f) == f;
            if (invert) present = !present;

            if (present != defaultState || required)
                container.Add(key, present);
        }

        /// <summary>
        /// Sets a property in the specified <see cref="JsonObject"/>.
        /// </summary>
        /// <param name="container">The object to set the property on.</param>
        /// <param name="key">The property name in <paramref name="container"/>.</param>
        /// <param name="value">The value.</param>
        /// <param name="required">A value indicating whether the property is required.</param>
        protected virtual void SetJsonValue(JsonObject container, string key, string value, bool required = false)
        {
            if (required && value == null)
                container.Add(key, null);
            else if (required || value != null)
                container.Add(key, value);
        }

        /// <summary>
        /// Sets a property in the specified <see cref="JsonObject"/>.
        /// </summary>
        /// <param name="container">The object to set the property on.</param>
        /// <param name="key">The property name in <paramref name="container"/>.</param>
        /// <param name="value">The value.</param>
        /// <param name="required">A value indicating whether the property is required.</param>
        protected virtual void SetJsonValue(JsonObject container, string key, JsonValue value, bool required = false)
        {
            if (required || value != null)
                container.Add(key, value);
        }

        /// <summary>
        /// Sets a property in the specified <see cref="JsonObject"/>.
        /// </summary>
        /// <param name="container">The object to set the property on.</param>
        /// <param name="key">The property name in <paramref name="container"/>.</param>
        /// <param name="value">The value.</param>
        /// <param name="required">A value indicating whether the property is required.</param>
        protected virtual void SetJsonValue(JsonObject container, string key, Uri value, bool required = false)
        {
            if (value == null && required)
                container.Add(key, null);
            else if (value != null)
                container.Add(key, value.ToString());
        }

        /// <summary>
        /// Sets a property in the specified <see cref="JsonObject"/>.
        /// </summary>
        /// <param name="container">The object to set the property on.</param>
        /// <param name="key">The property name in <paramref name="container"/>.</param>
        /// <param name="value">The value.</param>
        /// <param name="required">A value indicating whether the property is required.</param>
        protected virtual void SetJsonValue(JsonObject container, string key, ContentType value, bool required = false)
        {
            if (value == null && required)
                container.Add(key, null);
            else if (value != null)
                container.Add(key, value.ToString());
        }

        /// <summary>
        /// Sets a property in the specified <see cref="JsonObject"/>.
        /// </summary>
        /// <param name="container">The object to set the property on.</param>
        /// <param name="key">The property name in <paramref name="container"/>.</param>
        /// <param name="value">The value.</param>
        /// <param name="required">A value indicating whether the property is required.</param>
        protected virtual void SetJsonValue(JsonObject container, string key, MailAddress value, bool required = false)
        {
            if (value == null && required)
                container.Add(key, null);
            else if (value != null)
                container.Add(key, value.ToString());
        }

        /// <summary>
        /// Sets a property in the specified <see cref="JsonObject"/>.
        /// </summary>
        /// <param name="container">The object to set the property on.</param>
        /// <param name="key">The property name in <paramref name="container"/>.</param>
        /// <param name="value">The value.</param>
        /// <param name="required">A value indicating whether the property is required.</param>
        protected virtual void SetJsonNumber(JsonObject container, string key, Number? value, bool required = false)
        {
            if (value.HasValue)
                container.Add(key, value.Value.ToValue());
            else if (required)
                container.Add(key, null);
        }

        /// <summary>
        /// Sets a property in the specified <see cref="JsonObject"/>.
        /// </summary>
        /// <param name="container">The object to set the property on.</param>
        /// <param name="key">The property name in <paramref name="container"/>.</param>
        /// <param name="value">The value.</param>
        /// <param name="required">A value indicating whether the property is required.</param>
        protected virtual void SetJsonObject<T>(JsonObject container, string key, T value, bool required = false)
        {
            var js = Serialize(value);
            if (js == null)
            {
                if (required) container.Add(key, new JsonObject());
                return;
            }

            container.Add(key, js);
        }

        /// <summary>
        /// Sets a property in the specified <see cref="JsonObject"/>.
        /// </summary>
        /// <param name="container">The object to set the property on.</param>
        /// <param name="key">The property name in <paramref name="container"/>.</param>
        /// <param name="value">The value.</param>
        /// <param name="required">A value indicating whether the property is required.</param>
        protected virtual void SetJsonObject<T>(JsonObject container, string key, Referable<T> value, bool required = false)
            where T : class, IEquatable<T>
        {
            if (!value.HasValue)
            {
                if (required) container.Add(key, null);
            }
            else if (value.IsReference)
            {
                container.Add(key, SerializeReferable(value));
            }
            else
            {
                container.Add(key, Serialize(value.Value));
            }
        }

        /// <summary>
        /// Converts the specified <see cref="ParameterStyle"/> to a <see cref="JsonValue"/>.
        /// </summary>
        /// <param name="parameterStyle">The <see cref="ParameterStyle"/> to convert.</param>
        /// <returns>The <see cref="JsonValue"/>.</returns>
        protected virtual JsonValue ToJsonValue(ParameterStyle parameterStyle)
        {
            switch (parameterStyle)
            {
                case ParameterStyle.Default: return null;
                case ParameterStyle.Matrix: return EnumConstants.Matrix;
                case ParameterStyle.Label: return EnumConstants.Label;
                case ParameterStyle.Form: return EnumConstants.Form;
                case ParameterStyle.Simple: return EnumConstants.Simple;
                case ParameterStyle.SpaceDelimited: return EnumConstants.SpaceDelimited;
                case ParameterStyle.PipeDelimited: return EnumConstants.PipeDelimited;
                case ParameterStyle.DeepObject: return EnumConstants.DeepObject;
                default: throw new ArgumentOutOfRangeException(nameof(parameterStyle));
            }
        }

        /// <summary>
        /// Converts the specified <see cref="SchemaType"/> to a <see cref="JsonValue"/>.
        /// </summary>
        /// <param name="schemaType">The <see cref="SchemaType"/> to convert.</param>
        /// <returns>The <see cref="JsonValue"/>.</returns>
        protected virtual JsonValue ToJsonValue(SchemaType schemaType)
        {
            switch (schemaType)
            {
                case SchemaType.String: return EnumConstants.String;
                case SchemaType.Number: return EnumConstants.Number;
                case SchemaType.Object: return EnumConstants.Object;
                case SchemaType.Array: return EnumConstants.Array;
                case SchemaType.Boolean: return EnumConstants.Boolean;
                case SchemaType.Integer: return EnumConstants.Integer;
                default: throw new ArgumentOutOfRangeException(nameof(schemaType));
            }
        }

        /// <summary>
        /// Converts the specified <see cref="ParameterLocation"/> to a <see cref="JsonValue"/>.
        /// </summary>
        /// <param name="parameterLocation">The <see cref="ParameterLocation"/> to convert.</param>
        /// <returns>The <see cref="JsonValue"/>.</returns>
        protected virtual JsonValue ToJsonValue(ParameterLocation parameterLocation)
        {
            switch (parameterLocation)
            {
                case ParameterLocation.Query: return EnumConstants.Query;
                case ParameterLocation.Header: return EnumConstants.Header;
                case ParameterLocation.Path: return EnumConstants.Path;
                case ParameterLocation.Cookie: return EnumConstants.Cookie;
                default: throw new ArgumentOutOfRangeException(nameof(parameterLocation));
            }
        }

        #endregion
    }

#pragma warning restore S1168 // Empty arrays and collections should be returned instead of null
}
