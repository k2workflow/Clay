#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using SourceCode.Clay.Json.Pointers;
using System;
using System.Collections.Generic;
using System.Text;

namespace SourceCode.Clay.OpenApi.Expressions
{
    /// <summary>
    /// Represents an Open API expression.
    /// </summary>
    public sealed class OasFieldExpression : OasExpressionComponent, IEquatable<OasFieldExpression>
    {
        #region Properties

        /// <summary>
        /// Gets the type of the expression.
        /// </summary>
        public OasFieldExpressionType ExpressionType { get; }

        /// <summary>
        /// Gets the expression source.
        /// </summary>
        public OasFieldExpressionSource ExpressionSource { get; }

        /// <summary>
        /// Gets the name component of the source.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the <see cref="JsonPointer"/>.
        /// </summary>
        public JsonPointer Pointer { get; }

        /// <summary>Gets the component type.</summary>
        public override OasExpressionComponentType ComponentType => OasExpressionComponentType.Field;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new simple <see cref="OasFieldExpression"/>.
        /// </summary>
        /// <param name="expressionType">
        ///     The expression type. This must be <see cref="OasFieldExpressionType.Url"/>, <see cref="OasFieldExpressionType.Method"/>
        ///     or <see cref="OasFieldExpressionType.StatusCode"/>.
        /// </param>
        public OasFieldExpression(OasFieldExpressionType expressionType)
        {
            switch (expressionType)
            {
                case OasFieldExpressionType.Url:
                case OasFieldExpressionType.Method:
                case OasFieldExpressionType.StatusCode:
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(expressionType));
            }

            ExpressionType = expressionType;
            ExpressionSource = OasFieldExpressionSource.None;
            Name = null;
            Pointer = default;
        }

        /// <summary>
        /// Creates a new parameter <see cref="OasFieldExpression"/>.
        /// </summary>
        /// <param name="expressionType">
        ///     The expression type. This must be <see cref="OasFieldExpressionType.Request"/> or <see cref="OasFieldExpressionType.Response"/>.
        /// </param>
        /// <param name="expressionSource">
        ///     The expression source. This must be <see cref="OasFieldExpressionSource.Header"/>, <see cref="OasFieldExpressionSource.Query"/>
        ///     or <see cref="OasFieldExpressionSource.Path"/>.
        /// </param>
        /// <param name="name">
        ///     The name being referred to.
        /// </param>
        public OasFieldExpression(OasFieldExpressionType expressionType, OasFieldExpressionSource expressionSource, string name)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException(nameof(name));

            switch (expressionType)
            {
                case OasFieldExpressionType.Request:
                case OasFieldExpressionType.Response:
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(expressionType));
            }

            switch (expressionSource)
            {
                case OasFieldExpressionSource.Header:
                    ValidateToken(name);
                    break;

                case OasFieldExpressionSource.Query:
                case OasFieldExpressionSource.Path:
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(expressionSource));
            }

            ExpressionType = expressionType;
            ExpressionSource = expressionSource;
            Name = name;
            Pointer = default;
        }

        /// <summary>
        /// Creates a new parameter <see cref="OasFieldExpression"/>.
        /// </summary>
        /// <param name="expressionType">
        ///     The expression type. This must be <see cref="OasFieldExpressionType.Request"/> or <see cref="OasFieldExpressionType.Response"/>.
        /// </param>
        /// <param name="pointer">
        ///     The JSON pointer.
        /// </param>
        public OasFieldExpression(OasFieldExpressionType expressionType, JsonPointer pointer)
        {
            switch (expressionType)
            {
                case OasFieldExpressionType.Request:
                case OasFieldExpressionType.Response:
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(expressionType));
            }

            ExpressionType = expressionType;
            ExpressionSource = OasFieldExpressionSource.Body;
            Name = null;
            Pointer = pointer;
        }

        #endregion

        #region Factory

        /// <summary>
        /// Creates a new <see cref="OasFieldExpression"/> that retrieves the URL.
        /// </summary>
        /// <returns>The new <see cref="OasFieldExpression"/>.</returns>
        public static OasFieldExpression Url() => new OasFieldExpression(OasFieldExpressionType.Url);

        /// <summary>
        /// Creates a new <see cref="OasFieldExpression"/> that retrieves the HTTP method.
        /// </summary>
        /// <returns>The new <see cref="OasFieldExpression"/>.</returns>
        public static OasFieldExpression Method() => new OasFieldExpression(OasFieldExpressionType.Method);

        /// <summary>
        /// Creates a new <see cref="OasFieldExpression"/> that retrieves the HTTP status code.
        /// </summary>
        /// <returns>The new <see cref="OasFieldExpression"/>.</returns>
        public static OasFieldExpression StatusCode() => new OasFieldExpression(OasFieldExpressionType.StatusCode);

        /// <summary>
        /// Creates a new <see cref="OasFieldExpression"/> that retrieves a specific header from the request.
        /// </summary>
        /// <returns>The new <see cref="OasFieldExpression"/>.</returns>
        public static OasFieldExpression RequestHeader(string name) => new OasFieldExpression(OasFieldExpressionType.Request, OasFieldExpressionSource.Header, name);

        /// <summary>
        /// Creates a new <see cref="OasFieldExpression"/> that retrieves a specific query parameter from the request.
        /// </summary>
        /// <returns>The new <see cref="OasFieldExpression"/>.</returns>
        public static OasFieldExpression RequestQuery(string name) => new OasFieldExpression(OasFieldExpressionType.Request, OasFieldExpressionSource.Query, name);

        /// <summary>
        /// Creates a new <see cref="OasFieldExpression"/> that retrieves a specific path parameter from the request.
        /// </summary>
        /// <returns>The new <see cref="OasFieldExpression"/>.</returns>
        public static OasFieldExpression RequestPath(string name) => new OasFieldExpression(OasFieldExpressionType.Request, OasFieldExpressionSource.Path, name);

        /// <summary>
        /// Creates a new <see cref="OasFieldExpression"/> that retrieves a specific value from the request body.
        /// </summary>
        /// <returns>The new <see cref="OasFieldExpression"/>.</returns>
        public static OasFieldExpression RequestBody(JsonPointer pointer) => new OasFieldExpression(OasFieldExpressionType.Request, pointer);

        /// <summary>
        /// Creates a new <see cref="OasFieldExpression"/> that retrieves a specific header from the response.
        /// </summary>
        /// <returns>The new <see cref="OasFieldExpression"/>.</returns>
        public static OasFieldExpression ResponseHeader(string name) => new OasFieldExpression(OasFieldExpressionType.Response, OasFieldExpressionSource.Header, name);

        /// <summary>
        /// Creates a new <see cref="OasFieldExpression"/> that retrieves a specific query parameter from the response.
        /// </summary>
        /// <returns>The new <see cref="OasFieldExpression"/>.</returns>
        public static OasFieldExpression ResponseQuery(string name) => new OasFieldExpression(OasFieldExpressionType.Response, OasFieldExpressionSource.Query, name);

        /// <summary>
        /// Creates a new <see cref="OasFieldExpression"/> that retrieves a specific path parameter from the response.
        /// </summary>
        /// <returns>The new <see cref="OasFieldExpression"/>.</returns>
        public static OasFieldExpression ResponsePath(string name) => new OasFieldExpression(OasFieldExpressionType.Response, OasFieldExpressionSource.Path, name);

        /// <summary>
        /// Creates a new <see cref="OasFieldExpression"/> that retrieves a specific value from the response body.
        /// </summary>
        /// <returns>The new <see cref="OasFieldExpression"/>.</returns>
        public static OasFieldExpression ResponseBody(JsonPointer pointer) => new OasFieldExpression(OasFieldExpressionType.Response, pointer);

        #endregion

        #region Token - https://tools.ietf.org/html/rfc7230#section-3.2.6

        private static readonly HashSet<char> _tchar = new HashSet<char>(
            "!#$%&'*+-.^_`|~0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ"
        );

        private static void ValidateToken(string name)
        {
            for (var i = 0; i < name.Length; i++)
            {
                if (!_tchar.Contains(name[i]))
                    throw new ArgumentOutOfRangeException(nameof(name), $"'{name[i]}' is not a valid token character.");
            }
        }

        private static bool TryValidateToken(string name)
        {
            for (var i = 0; i < name.Length; i++)
            {
                if (!_tchar.Contains(name[i])) return false;
            }
            return true;
        }

        #endregion

        #region IEquatable

        /// <summary>Indicates whether this instance and a specified object are equal.</summary>
        /// <param name="obj">The object to compare with the current instance.</param>
        /// <returns>true if <paramref name="obj">obj</paramref> and this instance are the same type and represent the same value; otherwise, false.</returns>
        public override bool Equals(object obj)
            => obj is OasFieldExpression other
            && Equals(other);

        /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>true if the current object is equal to the <paramref name="other">other</paramref> parameter; otherwise, false.</returns>
        public bool Equals(OasFieldExpression other)
        {
            if (other is null) return false;
            if (ExpressionType != other.ExpressionType) return false;
            if (ExpressionSource != other.ExpressionSource) return false;
            if (!StringComparer.Ordinal.Equals(Name, other.Name)) return false;
            if (!Pointer.Equals(other.Pointer)) return false;

            return true;
        }

        /// <summary>Returns the hash code for this instance.</summary>
        /// <returns>A 32-bit signed integer that is the hash code for this instance.</returns>
        public override int GetHashCode() => new HashCode()
            .Tally(base.GetHashCode())
            .Tally(ExpressionType)
            .Tally(ExpressionSource)
            .Tally(Name ?? string.Empty, StringComparer.Ordinal)
            .Tally(Pointer)
            .ToHashCode();

        #endregion

        #region String

        private static bool StartsWith(string s, string cmp, int index)
        {
            for (var i = 0; i < cmp.Length; i++)
            {
                if (index >= s.Length) return false;
                if (s[index] != cmp[i]) return false;
                index++;
            }
            return true;
        }

        internal static bool TryParse(string s, out OasFieldExpression result, ref int index, bool respectDelimiter)
        {
            var i = index;

            if (s == null || !StartsWith(s, "$", i++))
            {
                result = default;
                return false;
            }

            if (StartsWith(s, "url", i))
            {
                i += 3;
                if (respectDelimiter && !StartsWith(s, "}", i))
                {
                    result = default;
                    return false;
                }

                index = i;
                result = Url();
                return true;
            }

            if (StartsWith(s, "method", i))
            {
                i += 6;
                if (respectDelimiter && !StartsWith(s, "}", i))
                {
                    result = default;
                    return false;
                }

                index = i;
                result = Method();
                return true;
            }

            if (StartsWith(s, "statusCode", i))
            {
                i += 10;
                if (respectDelimiter && !StartsWith(s, "}", i))
                {
                    result = default;
                    return false;
                }

                index = i;
                result = StatusCode();
                return true;
            }

            if (!StartsWith(s, "re", i))
            {
                result = default;
                return false;
            }
            i += 2;

            OasFieldExpressionType type;
            if (StartsWith(s, "quest.", i))
            {
                i += 6;
                type = OasFieldExpressionType.Request;
            }
            else if (StartsWith(s, "sponse.", i))
            {
                i += 7;
                type = OasFieldExpressionType.Response;
            }
            else
            {
                result = default;
                return false;
            }

            OasFieldExpressionSource source;

            if (StartsWith(s, "header.", i))
            {
                i += 7;
                source = OasFieldExpressionSource.Header;
            }
            else if (StartsWith(s, "query.", i))
            {
                i += 6;
                source = OasFieldExpressionSource.Query;
            }
            else if (StartsWith(s, "path.", i))
            {
                i += 5;
                source = OasFieldExpressionSource.Path;
            }
            else if (StartsWith(s, "body#", i))
            {
                i += 5;
                source = OasFieldExpressionSource.Body;
            }
            else
            {
                result = default;
                return false;
            }

            if (i == s.Length)
            {
                result = default;
                return false;
            }

            string name;
            if (respectDelimiter)
            {
                var j = s.IndexOf('}', i + 1);
                if (j < 0)
                {
                    result = default;
                    return false;
                }
                name = s.Substring(i, j - i);
                i = j;
            }
            else
            {
                name = s.Substring(i);
                i = s.Length;
            }

            if (source == OasFieldExpressionSource.Header)
            {
                if (!TryValidateToken(name))
                {
                    result = default;
                    return false;
                }
                result = new OasFieldExpression(type, source, name);
            }
            else if (source == OasFieldExpressionSource.Body)
            {
                if (!JsonPointer.TryParse(name, out var pointer))
                {
                    result = default;
                    return false;
                }
                result = new OasFieldExpression(type, pointer);
            }
            else
            {
                result = new OasFieldExpression(type, source, name);
            }

            index = i;
            return true;
        }

        internal void ToString(StringBuilder sb)
        {
            switch (ExpressionType)
            {
                case OasFieldExpressionType.Url: sb.Append("$url"); return;
                case OasFieldExpressionType.Method: sb.Append("$method"); return;
                case OasFieldExpressionType.StatusCode: sb.Append("$statusCode"); return;
                case OasFieldExpressionType.Request: sb.Append("$request."); break;
                case OasFieldExpressionType.Response: sb.Append("$response."); break;
            }

            switch (ExpressionSource)
            {
                case OasFieldExpressionSource.Header: sb.Append("header.").Append(Name); break;
                case OasFieldExpressionSource.Query: sb.Append("query.").Append(Name); break;
                case OasFieldExpressionSource.Path: sb.Append("path.").Append(Name); break;
                case OasFieldExpressionSource.Body: sb.Append("body#").Append(Pointer.ToString()); break;
            }
        }

        /// <summary>
        /// Converts the string representation of a field expression to its structured equivalent.
        /// </summary>
        /// <param name="s">A string containing a field expression to convert.</param>
        /// <returns>The structured equivalent of the field expression contained in <paramref name="s"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="s"/> is null.</exception>
        /// <exception cref="FormatException"><paramref name="s"/> is not in a format compliant with the Open API specification.</exception>
        public static OasFieldExpression Parse(string s)
        {
            if (!TryParse(s, out var result)) throw new FormatException("The expression is not a valid field expression.");
            return result;
        }

        /// <summary>
        /// Converts the string representation of a field expression to its structured equivalent.
        /// A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="s">A string containing a field expression to convert.</param>
        /// <param name="result">
        /// When this method returns, contains the structured equivalent of the field expression contained in <paramref name="s"/>,
        /// if the conversion succeeded, or default if the conversion failed. The conversion fails if the <paramref name="s"/> parameter
        /// is not in a format compliant with the Open API field expression specification. This parameter is passed uninitialized;
        /// any value originally supplied in result will be overwritten.
        /// </param>
        /// <returns><c>true</c> if s was converted successfully; otherwise, <c>false</c>.</returns>
        public static bool TryParse(string s, out OasFieldExpression result)
        {
            var i = 0;
            return TryParse(s, out result, ref i, false);
        }

        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            ToString(sb);
            return sb.ToString();
        }

        #endregion
    }
}
