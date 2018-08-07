#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using Newtonsoft.Json.Linq;
using SourceCode.Clay.Json;
using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Net.Mime;

namespace SourceCode.Clay.OpenApi.Serialization
{
    partial class OasSerializer
    {
        /// <summary>
        /// Converts the specified dictionary into its object representation.
        /// </summary>
        /// <typeparam name="T">The type of value in the dictionary.</typeparam>
        /// <param name="dictionary">The dictionary.</param>
        /// <param name="required">A value indicating whether an empty object is required.</param>
        /// <returns>The <see cref="JObject"/> representing the dictionary.</returns>
        protected virtual JObject ToJsonMap<T>(IEnumerable<ValueTuple<string, T>> dictionary, bool required = false)
        {
            if (dictionary is null)
            {
                if (required) return new JObject();
                return null;
            }

            using (var e = dictionary.GetEnumerator())
            {
                if (!e.MoveNext())
                {
                    if (required) return new JObject();
                    return null;
                }

                var result = new JObject();
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
        /// <returns>The <see cref="JObject"/> representing the dictionary.</returns>
        protected virtual JObject ToJsonMap<T>(IEnumerable<KeyValuePair<string, T>> dictionary, bool required = false)
        {
            if (dictionary is null)
            {
                if (required) return new JObject();
                return null;
            }

            using (var e = dictionary.GetEnumerator())
            {
                if (!e.MoveNext())
                {
                    if (required) return new JObject();
                    return null;
                }

                var result = new JObject();
                do
                {
                    result.Add(e.Current.Key, Serialize(e.Current.Value));
                } while (e.MoveNext());

                return result;
            }
        }

        /// <summary>
        /// Sets a property in the specified <see cref="JObject"/>.
        /// </summary>
        /// <typeparam name="T">The type of value in the dictionary.</typeparam>
        /// <param name="container">The object to set the property on.</param>
        /// <param name="key">The property name in <paramref name="container"/>.</param>
        /// <param name="dictionary">The dictionary value.</param>
        /// <param name="required">A value indicating whether an empty object is required.</param>
        protected virtual void SetJsonMap<T>(JObject container, string key, IEnumerable<ValueTuple<string, T>> dictionary, bool required = false)
        {
            var result = ToJsonMap(dictionary, required);
            if (!(result is null)) container.Add(key, result);
        }

        /// <summary>
        /// Sets a property in the specified <see cref="JObject"/>.
        /// </summary>
        /// <typeparam name="T">The type of value in the dictionary.</typeparam>
        /// <param name="container">The object to set the property on.</param>
        /// <param name="key">The property name in <paramref name="container"/>.</param>
        /// <param name="dictionary">The dictionary value.</param>
        /// <param name="required">A value indicating whether an empty object is required.</param>
        protected virtual void SetJsonMap<T>(JObject container, string key, IEnumerable<KeyValuePair<string, T>> dictionary, bool required = false)
        {
            var result = ToJsonMap(dictionary, required);
            if (!(result is null)) container.Add(key, result);
        }

        /// <summary>
        /// Converts the specified list into its array representation.
        /// </summary>
        /// <typeparam name="T">The type of value in the list.</typeparam>
        /// <param name="list">The type of elements in the list.</param>
        /// <param name="required">A value indicating whether an empty array is required.</param>
        /// <returns>The <see cref="JArray"/> containing the list.</returns>
        protected virtual JArray ToJsonArray<T>(IEnumerable<T> list, bool required = false)
        {
            if (list is null)
            {
                if (required) return new JArray();
                return null;
            }

            using (var e = list.GetEnumerator())
            {
                if (!e.MoveNext())
                {
                    if (required) return new JArray();
                    return null;
                }

                var result = new JArray();
                do
                {
                    result.Add(Serialize(e.Current));
                } while (e.MoveNext());

                return result;
            }
        }

        /// <summary>
        /// Sets a property in the specified <see cref="JObject"/>.
        /// </summary>
        /// <typeparam name="T">The type of value in the list.</typeparam>
        /// <param name="container">The object to set the property on.</param>
        /// <param name="key">The property name in <paramref name="container"/>.</param>
        /// <param name="list">The list value.</param>
        /// <param name="required">A value indicating whether an empty array is required.</param>
        protected virtual void SetJsonArray<T>(JObject container, string key, IEnumerable<T> list, bool required = false)
        {
            var result = ToJsonArray(list);
            if (!(result is null)) container.Add(key, result);
        }

        /// <summary>
        /// Sets a property in the specified <see cref="JObject"/>.
        /// </summary>
        /// <typeparam name="TEnum">The type of the enum.</typeparam>
        /// <param name="container">The object to set the property on.</param>
        /// <param name="key">The property name in <paramref name="container"/>.</param>
        /// <param name="value">The enum flags value.</param>
        /// <param name="flag">The flag to search for.</param>
        /// <param name="defaultState">The default state of the flag.</param>
        /// <param name="required">A value indicating whether the property is required.</param>
        /// <param name="invert">A value indicating whether to invert the flag check.</param>
        protected virtual void SetJsonFlag<TEnum>(JObject container, string key, TEnum value, TEnum flag, bool defaultState = false, bool required = false, bool invert = false)
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
        /// Sets a property in the specified <see cref="JObject"/>.
        /// </summary>
        /// <param name="container">The object to set the property on.</param>
        /// <param name="key">The property name in <paramref name="container"/>.</param>
        /// <param name="value">The value.</param>
        /// <param name="required">A value indicating whether the property is required.</param>
        protected virtual void SetJsonValue(JObject container, string key, string value, bool required = false)
        {
            if (required && value is null)
                container.Add(key, null);
            else if (required || !(value is null))
                container.Add(key, value);
        }

        /// <summary>
        /// Sets a property in the specified <see cref="JObject"/>.
        /// </summary>
        /// <param name="container">The object to set the property on.</param>
        /// <param name="key">The property name in <paramref name="container"/>.</param>
        /// <param name="value">The value.</param>
        /// <param name="required">A value indicating whether the property is required.</param>
        protected virtual void SetJsonValue(JObject container, string key, JToken value, bool required = false)
        {
            if (required || !(value is null))
                container.Add(key, value);
        }

        /// <summary>
        /// Sets a property in the specified <see cref="JObject"/>.
        /// </summary>
        /// <param name="container">The object to set the property on.</param>
        /// <param name="key">The property name in <paramref name="container"/>.</param>
        /// <param name="value">The value.</param>
        /// <param name="required">A value indicating whether the property is required.</param>
        protected virtual void SetJsonValue(JObject container, string key, Uri value, bool required = false)
        {
            if (value is null && required)
                container.Add(key, null);
            else if (!(value is null))
                container.Add(key, value.ToString());
        }

        /// <summary>
        /// Sets a property in the specified <see cref="JObject"/>.
        /// </summary>
        /// <param name="container">The object to set the property on.</param>
        /// <param name="key">The property name in <paramref name="container"/>.</param>
        /// <param name="value">The value.</param>
        /// <param name="required">A value indicating whether the property is required.</param>
        protected virtual void SetJsonValue(JObject container, string key, ContentType value, bool required = false)
        {
            if (value is null && required)
                container.Add(key, null);
            else if (!(value is null))
                container.Add(key, value.ToString());
        }

        /// <summary>
        /// Sets a property in the specified <see cref="JObject"/>.
        /// </summary>
        /// <param name="container">The object to set the property on.</param>
        /// <param name="key">The property name in <paramref name="container"/>.</param>
        /// <param name="value">The value.</param>
        /// <param name="required">A value indicating whether the property is required.</param>
        protected virtual void SetJsonValue(JObject container, string key, MailAddress value, bool required = false)
        {
            if (value is null && required)
                container.Add(key, null);
            else if (!(value is null))
                container.Add(key, value.ToString());
        }

        /// <summary>
        /// Sets a property in the specified <see cref="JObject"/>.
        /// </summary>
        /// <param name="container">The object to set the property on.</param>
        /// <param name="key">The property name in <paramref name="container"/>.</param>
        /// <param name="value">The value.</param>
        /// <param name="required">A value indicating whether the property is required.</param>
        protected virtual void SetJsonNumber(JObject container, string key, Number? value, bool required = false)
        {
            if (value.HasValue)
                container.Add(key, value.Value.ToJson());
            else if (required)
                container.Add(key, null);
        }

        /// <summary>
        /// Sets a property in the specified <see cref="JObject"/>.
        /// </summary>
        /// <param name="container">The object to set the property on.</param>
        /// <param name="key">The property name in <paramref name="container"/>.</param>
        /// <param name="value">The value.</param>
        /// <param name="required">A value indicating whether the property is required.</param>
        protected virtual void SetJsonObject<T>(JObject container, string key, T value, bool required = false)
        {
            var js = Serialize(value);
            if (js is null)
            {
                if (required) container.Add(key, new JObject());
                return;
            }

            container.Add(key, js);
        }

        /// <summary>
        /// Sets a property in the specified <see cref="JObject"/>.
        /// </summary>
        /// <param name="container">The object to set the property on.</param>
        /// <param name="key">The property name in <paramref name="container"/>.</param>
        /// <param name="value">The value.</param>
        /// <param name="required">A value indicating whether the property is required.</param>
        protected virtual void SetJsonObject<T>(JObject container, string key, OasReferable<T> value, bool required = false)
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
        /// Converts the specified <see cref="OasParameterStyle"/> to a <see cref="JToken"/>.
        /// </summary>
        /// <param name="parameterStyle">The <see cref="OasParameterStyle"/> to convert.</param>
        /// <returns>The <see cref="JToken"/>.</returns>
        protected virtual JToken ToJsonValue(OasParameterStyle parameterStyle)
        {
            switch (parameterStyle)
            {
                case OasParameterStyle.Default: return null;
                case OasParameterStyle.Matrix: return EnumConstants.Matrix;
                case OasParameterStyle.Label: return EnumConstants.Label;
                case OasParameterStyle.Form: return EnumConstants.Form;
                case OasParameterStyle.Simple: return EnumConstants.Simple;
                case OasParameterStyle.SpaceDelimited: return EnumConstants.SpaceDelimited;
                case OasParameterStyle.PipeDelimited: return EnumConstants.PipeDelimited;
                case OasParameterStyle.DeepObject: return EnumConstants.DeepObject;
                default: throw new ArgumentOutOfRangeException(nameof(parameterStyle));
            }
        }

        /// <summary>
        /// Converts the specified <see cref="OasSchemaType"/> to a <see cref="JToken"/>.
        /// </summary>
        /// <param name="schemaType">The <see cref="OasSchemaType"/> to convert.</param>
        /// <returns>The <see cref="JToken"/>.</returns>
        protected virtual JToken ToJsonValue(OasSchemaType schemaType)
        {
            switch (schemaType)
            {
                case OasSchemaType.String: return EnumConstants.String;
                case OasSchemaType.Number: return EnumConstants.Number;
                case OasSchemaType.Object: return EnumConstants.Object;
                case OasSchemaType.Array: return EnumConstants.Array;
                case OasSchemaType.Boolean: return EnumConstants.Boolean;
                case OasSchemaType.Integer: return EnumConstants.Integer;
                default: throw new ArgumentOutOfRangeException(nameof(schemaType));
            }
        }

        /// <summary>
        /// Converts the specified <see cref="OasParameterLocation"/> to a <see cref="JToken"/>.
        /// </summary>
        /// <param name="parameterLocation">The <see cref="OasParameterLocation"/> to convert.</param>
        /// <returns>The <see cref="JToken"/>.</returns>
        protected virtual JToken ToJsonValue(OasParameterLocation parameterLocation)
        {
            switch (parameterLocation)
            {
                case OasParameterLocation.Query: return EnumConstants.Query;
                case OasParameterLocation.Header: return EnumConstants.Header;
                case OasParameterLocation.Path: return EnumConstants.Path;
                case OasParameterLocation.Cookie: return EnumConstants.Cookie;
                default: throw new ArgumentOutOfRangeException(nameof(parameterLocation));
            }
        }
    }
}
