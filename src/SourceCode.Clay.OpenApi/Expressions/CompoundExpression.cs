#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace SourceCode.Clay.OpenApi.Expressions
{
    /// <summary>
    /// Represents an Open API expression.
    /// </summary>
    public struct CompoundExpression : IReadOnlyList<ExpressionComponent>, IEquatable<CompoundExpression>
    {
        #region Properties

        private readonly ExpressionComponent[] _components;

        /// <summary>Gets the number of elements in the collection.</summary>
        /// <returns>The number of elements in the collection.</returns>
        public int Count => _components?.Length ?? 0;

        /// <summary>Gets the element at the specified index in the read-only list.</summary>
        /// <param name="index">The zero-based index of the element to get.</param>
        /// <returns>The element at the specified index in the read-only list.</returns>
        public ExpressionComponent this[int index] => (_components ?? Array.Empty<ExpressionComponent>())[index];

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new <see cref="CompoundExpression"/> value.
        /// </summary>
        /// <param name="components">The expression components.</param>
        public CompoundExpression(params ExpressionComponent[] components)
        {
            _components = components ?? throw new ArgumentNullException(nameof(components));
            if (_components.Length == 0) _components = null;
        }

        #endregion

        #region IEnumerable

        /// <summary>Returns an enumerator that iterates through the collection.</summary>
        /// <returns>An enumerator that can be used to iterate through the collection.</returns>
        public IEnumerator<ExpressionComponent> GetEnumerator() => (IEnumerator<ExpressionComponent>)_components.GetEnumerator();

        /// <summary>Returns an enumerator that iterates through a collection.</summary>
        /// <returns>An <see cref="T:System.Collections.IEnumerator"></see> object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        #endregion

        #region IEquatable

        /// <summary>Indicates whether this instance and a specified object are equal.</summary>
        /// <param name="obj">The object to compare with the current instance.</param>
        /// <returns>true if <paramref name="obj">obj</paramref> and this instance are the same type and represent the same value; otherwise, false.</returns>
        public override bool Equals(object obj) => obj is CompoundExpression o && Equals(o);

        /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>true if the current object is equal to the <paramref name="other">other</paramref> parameter; otherwise, false.</returns>
        public bool Equals(CompoundExpression other)
        {
            if (ReferenceEquals(_components, other._components)) return true;
            if (_components is null || other._components is null) return false;
            if (_components.Length != other._components.Length) return false;

            for (var i = 0; i < _components.Length; i++)
            {
                if (!((IEquatable<ExpressionComponent>)_components[i]).Equals(other._components[i])) return false;
            }

            return true;
        }

        /// <summary>Returns the hash code for this instance.</summary>
        /// <returns>A 32-bit signed integer that is the hash code for this instance.</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                var hc = 17L;

                if (_components != null)
                {
                    hc = (hc * 23) + _components.Length;
                    hc = (hc * 23) + _components[0].GetHashCode();
                }

                return ((int)(hc >> 32)) ^ (int)hc;
            }
        }

        #endregion

        #region Operators

        /// <summary>
        /// Determines if <paramref name="x"/> is a similar value to <paramref name="y"/>.
        /// </summary>
        /// <param name="x">The first <see cref="CompoundExpression"/> to compare.</param>
        /// <param name="y">The second <see cref="CompoundExpression"/> to compare.</param>
        /// <returns>
        /// A value indicating whether the first <see cref="CompoundExpression"/> is equal to <see cref="CompoundExpression"/>.
        /// </returns>
        public static bool operator ==(CompoundExpression x, CompoundExpression y) => x.Equals(y);

        /// <summary>
        /// Determines if <paramref name="x"/> is not a similar version to <paramref name="y"/>.
        /// </summary>
        /// <param name="x">The first <see cref="CompoundExpression"/> to compare.</param>
        /// <param name="y">The second <see cref="CompoundExpression"/> to compare.</param>
        /// <returns>
        /// A value indicating whether the first <see cref="CompoundExpression"/> is not similar to <see cref="CompoundExpression"/>.
        /// </returns>
        public static bool operator !=(CompoundExpression x, CompoundExpression y) => !(x == y);

        #endregion

        #region String

        /// <summary>
        /// Converts the string representation of an expression to its structured equivalent.
        /// </summary>
        /// <param name="s">A string containing an expression to convert.</param>
        /// <returns>The structured equivalent of the expression contained in <paramref name="s"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="s"/> is null.</exception>
        /// <exception cref="FormatException"><paramref name="s"/> is not in a format compliant with the Open API specification.</exception>
        public static CompoundExpression Parse(string s)
        {
            if (s == null) throw new ArgumentNullException(nameof(s));
            if (!TryParse(s, out var result)) throw new FormatException();
            return result;
        }

        /// <summary>
        /// Converts the string representation of a expression to its structured equivalent.
        /// A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="s">A string containing a expression to convert.</param>
        /// <param name="result">
        /// When this method returns, contains the structured equivalent of the expression contained in <paramref name="s"/>,
        /// if the conversion succeeded, or default if the conversion failed. The conversion fails if the <paramref name="s"/> parameter
        /// is not in a format compliant with the Open API expression specification. This parameter is passed uninitialized;
        /// any value originally supplied in result will be overwritten.
        /// </param>
        /// <returns><c>true</c> if s was converted successfully; otherwise, <c>false</c>.</returns>
        public static bool TryParse(string s, out CompoundExpression result)
        {
            if (string.IsNullOrEmpty(s))
            {
                result = default;
                return s != null;
            }

            var currentComponent = new StringBuilder();
            var components = new List<ExpressionComponent>();
            var lastIndex = s.Length - 1;
            for (var i = 0; i < s.Length; i++)
            {
                var c = s[i];
                if (c == '{' || c == '}')
                {
                    if (i == lastIndex)
                    {
                        result = default;
                        return false;
                    }

                    i++;
                    if (s[i] == c)
                    {
                        currentComponent.Append(c);
                    }
                    else
                    {
                        if (c != '{' || !FieldExpression.TryParse(s, out var field, ref i, true) || i >= s.Length || s[i] != '}')
                        {
                            result = default;
                            return false;
                        }

                        if (currentComponent.Length != 0)
                        {
                            components.Add(new LiteralExpression(currentComponent.ToString()));
                            currentComponent.Clear();
                        }

                        components.Add(field);
                    }
                }
                else
                {
                    currentComponent.Append(c);
                }
            }

            if (currentComponent.Length != 0)
            {
                components.Add(new LiteralExpression(currentComponent.ToString()));
                currentComponent.Clear();
            }

            result = new CompoundExpression(components.ToArray());
            return true;
        }

        /// <summary>Returns the string format of the expression.</summary>
        /// <returns>The string format of the expression.</returns>
        public override string ToString()
        {
            if (_components == null) return string.Empty;

            var sb = new StringBuilder();
            for (var i = 0; i < _components.Length; i++)
            {
                var component = _components[i];

                if (component is LiteralExpression literal)
                {
                    for (var j = 0; j < literal.Value.Length; j++)
                    {
                        var c = literal.Value[j];
                        sb.Append(c);
                        if (c == '{' || c == '}') sb.Append(c);
                    }
                }
                else if (component is FieldExpression field)
                {
                    sb.Append("{");
                    field.ToString(sb);
                    sb.Append("}");
                }
            }

            return sb.ToString();
        }

        #endregion
    }
}
