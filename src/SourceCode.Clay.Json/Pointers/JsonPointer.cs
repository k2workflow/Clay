#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using System;
using System.Collections;
using System.Collections.Generic;
using System.Json;
using System.Text;

namespace SourceCode.Clay.Json.Pointers
{
    /// <summary>
    /// Represents a JSON pointer.
    /// </summary>
    public struct JsonPointer : IReadOnlyList<JsonPointerToken>, IEquatable<JsonPointer>
    {
        #region Constants

        private const int LengthHeuristic = 5;

        #endregion

        #region Properties

        private readonly JsonPointerToken[] _tokens;

        /// <summary>Gets the number of elements in the collection.</summary>
        /// <returns>The number of elements in the collection.</returns>
        public int Count => _tokens?.Length ?? 0;

        /// <summary>Gets the element at the specified index in the read-only list.</summary>
        /// <param name="index">The zero-based index of the element to get.</param>
        /// <returns>The element at the specified index in the read-only list.</returns>
        public JsonPointerToken this[int index] => (_tokens ?? Array.Empty<JsonPointerToken>())[index];

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new <see cref="JsonPointer"/> value.
        /// </summary>
        /// <param name="tokens">The pointer tokens.</param>
        public JsonPointer(params JsonPointerToken[] tokens)
        {
            _tokens = tokens ?? throw new ArgumentNullException(nameof(tokens));
            if (_tokens.Length == 0) _tokens = null;
        }

        #endregion

        #region IEnumerable

        /// <summary>Returns an enumerator that iterates through the collection.</summary>
        /// <returns>An enumerator that can be used to iterate through the collection.</returns>
        public IEnumerator<JsonPointerToken> GetEnumerator()
            => (IEnumerator<JsonPointerToken>)(_tokens ?? Array.Empty<JsonPointerToken>()).GetEnumerator();

        /// <summary>Returns an enumerator that iterates through a collection.</summary>
        /// <returns>An <see cref="T:System.Collections.IEnumerator"></see> object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        #endregion

        #region Conversion

        /// <summary>
        /// Converts the string representation of a JSON pointer to its structured equivalent.
        /// </summary>
        /// <param name="s">A string containing a JSON pointer to convert.</param>
        /// <returns>The structured equivalent of the JSON pointer contained in <paramref name="s"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="s"/> is null.</exception>
        /// <exception cref="FormatException"><paramref name="s"/> is not in a format compliant with the JSON pointer specification.</exception>
        public static JsonPointer Parse(string s)
        {
            if (s == null) throw new ArgumentNullException(nameof(s));
            if (!TryParse(s, out var result)) throw new FormatException("The specified value is not a valid JSON pointer.");
            return result;
        }

        /// <summary>
        /// Converts the string representation of a JSON pointer to its structured equivalent.
        /// A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="s">A string containing a JSON pointer to convert.</param>
        /// <param name="result">
        /// When this method returns, contains the structured equivalent of the JSON pointer contained in <paramref name="s"/>,
        /// if the conversion succeeded, or default if the conversion failed. The conversion fails if the <paramref name="s"/> parameter
        /// is not in a format compliant with the JSON pointer specification. This parameter is passed uninitialized;
        /// any value originally supplied in result will be overwritten.
        /// </param>
        /// <returns><c>true</c> if s was converted successfully; otherwise, <c>false</c>.</returns>
        public static bool TryParse(string s, out JsonPointer result)
        {
            if (s == null)
            {
                result = default;
                return false;
            }

            if (s == string.Empty)
            {
                result = new JsonPointer();
                return true;
            }

            if (s[0] != '/')
            {
                result = default;
                return false;
            }

            var tokens = new List<JsonPointerToken>(Math.Min(3, s.Length / LengthHeuristic));
            var currentToken = new StringBuilder();
            var slashSeen = true;

            for (var i = 1; i < s.Length; i++)
            {
                slashSeen = false;

                var c = s[i];
                if (c == '/')
                {
                    tokens.Add(currentToken.ToString());
                    currentToken.Clear();
                    slashSeen = true;
                }
                else if (c == '~')
                {
                    if (++i >= s.Length)
                    {
                        result = default;
                        return false;
                    }

                    c = s[i];
                    if (c == '0') currentToken.Append('~');
                    else if (c == '1') currentToken.Append('/');
                    else
                    {
                        result = default;
                        return false;
                    }
                }
                else
                {
                    currentToken.Append(c);
                }
            }

            if (slashSeen || currentToken.Length != 0) tokens.Add(currentToken.ToString());

            result = new JsonPointer(tokens.ToArray()); // TODO: Perf
            return true;
        }

        /// <summary>Returns the the JSON pointer as a string value.</summary>
        /// <returns>The JSON pointer.</returns>
        public override string ToString()
        {
            if (_tokens == null) return string.Empty;

            var sb = new StringBuilder(_tokens.Length * LengthHeuristic);
            for (var i = 0; i < _tokens.Length; i++)
            {
                var token = _tokens[i].Value;
                sb.Append('/');
                for (var j = 0; j < token.Length; j++)
                {
                    var c = token[j];
                    if (c == '/') sb.Append("~1");
                    else if (c == '~') sb.Append("~0");
                    else sb.Append(c);
                }
            }
            return sb.ToString();
        }

        #endregion

        #region Evaluate

        /// <summary>
        /// Evaluates the current <see cref="JsonPointer"/> against the specified target.
        /// </summary>
        /// <param name="target">The target <see cref="JsonValue"/>.</param>
        /// <param name="options">The evaluation options.</param>
        /// <returns>The result of the evaluation.</returns>
        public JsonValue Evaluate(JsonValue target, JsonPointerEvaluationOptions options = default)
        {
            if (_tokens == null) return target;

            for (var i = 0; i < _tokens.Length; i++)
            {
                var token = _tokens[i];

                if (target == null)
                {
                    if (options.HasFlag(JsonPointerEvaluationOptions.NullCoalescing)) return target;
                    throw new InvalidOperationException($"Cannot evaluate the token '{token.Value}' on a null value.");
                }

                switch (target.JsonType)
                {
                    case JsonType.String:
                    case JsonType.Number:
                    case JsonType.Boolean:
                        if (options.HasFlag(JsonPointerEvaluationOptions.PrimitiveMembersAndIndiciesAreNull)) target = null;
                        else throw new InvalidOperationException($"Cannot evaluate the token '{token.Value}' against the primitive value {target}.");

                        break;

                    case JsonType.Object:
                        var o = (JsonObject)target;
                        if (!o.ContainsKey(token.Value))
                        {
                            if (options.HasFlag(JsonPointerEvaluationOptions.MissingMembersAreNull)) target = null;
                            else throw new InvalidOperationException($"The current evaluated object does not contain the member '{token.Value}'.");
                            continue;
                        }

                        target = o[token.Value];
                        break;

                    case JsonType.Array:
                        if (token.Value == "-")
                        {
                            if (options.HasFlag(JsonPointerEvaluationOptions.InvalidIndiciesAreNull)) target = null;
                            else throw new InvalidOperationException($"The new index token ('{token.Value}') is not supported.");
                            continue;
                        }

                        var a = (JsonArray)target;
                        var index = token.ArrayIndex;
                        if (!index.HasValue)
                        {
                            if (options.HasFlag(JsonPointerEvaluationOptions.ArrayMembersAreNull)) target = null;
                            else throw new InvalidOperationException($"An array cannot be indexed by the member '{token.Value}'.");
                            continue;
                        }

                        if (index.Value >= a.Count)
                        {
                            if (options.HasFlag(JsonPointerEvaluationOptions.InvalidIndiciesAreNull)) target = null;
                            else throw new InvalidOperationException($"The index {index.Value} falls outside the array length ({a.Count}).");
                            continue;
                        }

                        target = a[index.Value];
                        break;

                    default:
                        throw new InvalidOperationException($"Unsupported JSON type {target.GetType()}.");
                }
            }

            return target;
        }

        #endregion

        #region IEquatable

        /// <summary>Indicates whether this instance and a specified object are equal.</summary>
        /// <param name="obj">The object to compare with the current instance.</param>
        /// <returns>true if <paramref name="obj">obj</paramref> and this instance are the same type and represent the same value; otherwise, false.</returns>
        public override bool Equals(object obj)
            => obj is JsonPointer jp
            && Equals(jp);

        /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>true if the current object is equal to the <paramref name="other">other</paramref> parameter; otherwise, false.</returns>
        public bool Equals(JsonPointer other)
        {
            if (_tokens is null ^ other._tokens is null) return false; // (x, null) or (null, y)
            if (_tokens is null) return true; // (null, null)
            if (ReferenceEquals(_tokens, other._tokens)) return true; // (x, x)

            if (_tokens.Length != other._tokens.Length) return false; // (n, m)
            if (_tokens.Length == 0) return true; // (0, 0)

            // Check items in sequential order
            for (var i = 0; i < _tokens.Length; i++)
            {
                if (!_tokens[i].Equals(other._tokens[i])) return false;
            }

            return true;
        }

        /// <summary>Returns the hash code for this instance.</summary>
        /// <returns>A 32-bit signed integer that is the hash code for this instance.</returns>
        public override int GetHashCode()
        {
            var hc = 17L;

            unchecked
            {
                if (_tokens != null)
                    hc = (hc * 23) + _tokens.Length;
            }

            return ((int)(hc >> 32)) ^ (int)hc;
        }

        #endregion

        #region Operators

        public static bool operator ==(JsonPointer x, JsonPointer y) => x.Equals(y);

        public static bool operator !=(JsonPointer x, JsonPointer y) => !x.Equals(y);

        #endregion
    }
}
