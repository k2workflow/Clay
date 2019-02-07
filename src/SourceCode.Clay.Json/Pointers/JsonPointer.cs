#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;

namespace SourceCode.Clay.Json.Pointers
{
#pragma warning disable CA1710 // Identifiers should have correct suffix

    /// <summary>
    /// Represents a Json pointer.
    /// </summary>
    public readonly struct JsonPointer : IReadOnlyList<JsonPointerToken>, IEquatable<JsonPointer>
    {
#pragma warning restore CA1710 // Identifiers should have correct suffix

        private const int LengthHeuristic = 5;

        private readonly JsonPointerToken[] _tokens;

        /// <summary>Gets the number of elements in the collection.</summary>
        /// <returns>The number of elements in the collection.</returns>
        public int Count => _tokens?.Length ?? 0;

        /// <summary>Gets the element at the specified index in the read-only list.</summary>
        /// <param name="index">The zero-based index of the element to get.</param>
        /// <returns>The element at the specified index in the read-only list.</returns>
        public JsonPointerToken this[int index] => (_tokens ?? Array.Empty<JsonPointerToken>())[index];

        /// <summary>
        /// Creates a new <see cref="JsonPointer"/> value.
        /// </summary>
        /// <param name="tokens">The pointer tokens.</param>
        public JsonPointer(params JsonPointerToken[] tokens)
        {
            _tokens = tokens ?? throw new ArgumentNullException(nameof(tokens));
            if (_tokens.Length == 0) _tokens = null;
        }

        /// <summary>Returns an enumerator that iterates through the collection.</summary>
        /// <returns>An enumerator that can be used to iterate through the collection.</returns>
        public IEnumerator<JsonPointerToken> GetEnumerator()
            => (IEnumerator<JsonPointerToken>)(_tokens ?? Array.Empty<JsonPointerToken>()).GetEnumerator();

        /// <summary>Returns an enumerator that iterates through a collection.</summary>
        /// <returns>An <see cref="System.Collections.IEnumerator"></see> object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        /// <summary>
        /// Create a new <see cref="JsonPointer"/> instance by appending
        /// a <see cref="JsonPointerToken"/> to the current <see cref="JsonPointer"/>.
        /// </summary>
        /// <param name="token">The <see cref="JsonPointerToken"/> instance to append.</param>
        /// <returns></returns>
        public JsonPointer Add(in JsonPointerToken token)
        {
            var array = new JsonPointerToken[Count + 1];

            for (var i = 0; i < Count; i++)
                array[i] = _tokens[i];

            array[Count] = token;

            var result = new JsonPointer(array);
            return result;
        }

        /// <summary>
        /// Create a new <see cref="JsonPointer"/> instance by appending
        /// multiple <see cref="JsonPointerToken"/> instances to the current <see cref="JsonPointer"/>.
        /// </summary>
        /// <param name="tokens">The <see cref="JsonPointerToken"/> instances to append.</param>
        /// <returns></returns>
        public JsonPointer Merge(params JsonPointerToken[] tokens)
        {
            if (tokens is null || tokens.Length == 0)
                return this;

            var array = new JsonPointerToken[Count + tokens.Length];

            for (var i = 0; i < Count; i++)
                array[i] = _tokens[i];

            for (var i = 0; i < tokens.Length; i++)
                array[Count + i] = tokens[i];

            var result = new JsonPointer(array);
            return result;
        }

        /// <summary>
        /// Create a new <see cref="JsonPointer"/> instance by appending
        /// multiple <see cref="JsonPointerToken"/> instances to the current <see cref="JsonPointer"/>.
        /// </summary>
        /// <param name="pointer">The <see cref="JsonPointerToken"/> instances to append.</param>
        /// <returns></returns>
        public JsonPointer Merge(in JsonPointer pointer)
        {
            if (pointer.Count == 0)
                return this;

            if (Count == 0)
                return pointer;

            var array = new JsonPointerToken[Count + pointer.Count];

            for (var i = 0; i < Count; i++)
                array[i] = _tokens[i];

            for (var i = 0; i < pointer.Count; i++)
                array[Count + i] = pointer._tokens[i];

            var result = new JsonPointer(array);
            return result;
        }

        /// <summary>
        /// Converts the string representation of a Json pointer to its structured equivalent.
        /// </summary>
        /// <param name="s">A string containing a Json pointer to convert.</param>
        /// <returns>The structured equivalent of the Json pointer contained in <paramref name="s"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="s"/> is null.</exception>
        /// <exception cref="FormatException"><paramref name="s"/> is not in a format compliant with the Json pointer specification.</exception>
        public static JsonPointer Parse(string s)
        {
            if (s is null) throw new ArgumentNullException(nameof(s));
            if (!TryParse(s, out JsonPointer result)) throw new FormatException("The specified value is not a valid Json pointer.");
            return result;
        }

        /// <summary>
        /// Converts the string representation of a Json pointer to its structured equivalent.
        /// A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="s">A string containing a Json pointer to convert.</param>
        /// <param name="result">
        /// When this method returns, contains the structured equivalent of the Json pointer contained in <paramref name="s"/>,
        /// if the conversion succeeded, or default if the conversion failed. The conversion fails if the <paramref name="s"/> parameter
        /// is not in a format compliant with the Json pointer specification. This parameter is passed uninitialized;
        /// any value originally supplied in result will be overwritten.
        /// </param>
        /// <returns><c>true</c> if s was converted successfully; otherwise, <c>false</c>.</returns>
        public static bool TryParse(string s, out JsonPointer result)
        {
            if (s is null)
            {
                result = default;
                return false;
            }

            if (s.Length == 0)
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

        /// <summary>
        /// Evaluates the current <see cref="JsonPointer"/> against the specified target.
        /// </summary>
        /// <param name="target">The target <see cref="JToken"/>.</param>
        /// <param name="options">The evaluation options.</param>
        /// <returns>The result of the evaluation.</returns>
        public JToken Evaluate(JToken target, JsonPointerEvaluationOptions options = default)
        {
            if (_tokens is null) return target;

            for (var i = 0; i < _tokens.Length; i++)
            {
                JsonPointerToken token = _tokens[i];

                if (target is null || target.Type == JTokenType.Null)
                {
                    if (options.HasFlag(JsonPointerEvaluationOptions.NullCoalescing)) return target;
                    throw new InvalidOperationException($"Cannot evaluate the token '{token.Value}' on a null value.");
                }

                switch (target.Type)
                {
                    case JTokenType.String:
                    case JTokenType.Integer:
                    case JTokenType.Float:
                    case JTokenType.Boolean:
                        if (options.HasFlag(JsonPointerEvaluationOptions.PrimitiveMembersAndIndiciesAreNull)) target = null;
                        else throw new InvalidOperationException($"Cannot evaluate the token '{token.Value}' against the primitive value {target}.");

                        break;

                    case JTokenType.Object:
                        var o = (JObject)target;
                        if (!o.TryGetValue(token.Value, out target))
                        {
                            if (options.HasFlag(JsonPointerEvaluationOptions.MissingMembersAreNull)) target = null;
                            else throw new InvalidOperationException($"The current evaluated object does not contain the member '{token.Value}'.");
                            continue;
                        }

                        break;

                    case JTokenType.Array:
                        if (token.Value == "-")
                        {
                            if (options.HasFlag(JsonPointerEvaluationOptions.InvalidIndiciesAreNull)) target = null;
                            else throw new InvalidOperationException($"The new index token ('{token.Value}') is not supported.");
                            continue;
                        }

                        var a = (JArray)target;
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
                        throw new InvalidOperationException($"Unsupported Json type {target.GetType()}.");
                }
            }

            return target;
        }

        /// <summary>Indicates whether this instance and a specified object are equal.</summary>
        /// <param name="obj">The object to compare with the current instance.</param>
        /// <returns>true if <paramref name="obj">obj</paramref> and this instance are the same type and represent the same value; otherwise, false.</returns>
        public override bool Equals(object obj)
            => obj is JsonPointer other
            && Equals(other);

        /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>true if the current object is equal to the <paramref name="other">other</paramref> parameter; otherwise, false.</returns>
        public bool Equals(JsonPointer other)
        {
            if (_tokens is null) return other._tokens is null; // (null, null) or (null, y)
            if (other._tokens is null) return false; // (x, null)
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
#if NETCOREAPP2_2
            => _tokens is null ? 0 : HashCode.Combine(_tokens.Length, _tokens[0]);
#else
        {
            if (_tokens is null) return 0;

            int hc = 11;
            unchecked
            {
                hc = hc * 7 + _tokens.Length;
                hc = hc * 7 + _tokens[0].GetHashCode();
            }
            return hc;
        }
#endif

        public static bool operator ==(JsonPointer x, JsonPointer y) => x.Equals(y);

        public static bool operator !=(JsonPointer x, JsonPointer y) => !x.Equals(y);

        public static JsonPointer operator +(JsonPointer x, JsonPointerToken y) => x.Add(y);

        /// <summary>Returns the the Json pointer as a string value.</summary>
        /// <returns>The Json pointer.</returns>
        public override string ToString()
        {
            if (_tokens is null) return string.Empty;

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
    }
}
