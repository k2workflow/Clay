#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using System;

namespace SourceCode.Clay.OpenApi.Expressions
{
    /// <summary>
    /// Represents a literal expression.
    /// </summary>
    public sealed class LiteralExpression : ExpressionComponent, IEquatable<LiteralExpression>
    {
        #region Properties

        /// <summary>
        /// Gets the literal value.
        /// </summary>
        public string Value { get; }

        /// <summary>Gets the component type.</summary>
        public override ExpressionComponentType ComponentType => ExpressionComponentType.Literal;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of the <see cref="LiteralExpression"/> class.
        /// </summary>
        /// <param name="value">The literal value.</param>
        public LiteralExpression(string value)
        {
            Value = value ?? throw new ArgumentNullException(nameof(value));
        }

        #endregion

        #region Methods

        /// <summary>Determines whether the specified object is equal to the current object.</summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns>true if the specified object  is equal to the current object; otherwise, false.</returns>
        public override bool Equals(object obj) => Equals(obj as LiteralExpression);

        /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>true if the current object is equal to the <paramref name="other">other</paramref> parameter; otherwise, false.</returns>
        public bool Equals(LiteralExpression other)
        {
            if (other is null) return false;
            if (!StringComparer.Ordinal.Equals(Value, other.Value)) return false;
            return true;
        }

        /// <summary>Returns the hash code for this instance.</summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                var hc = 17L;
                hc = (hc * 23) + base.GetHashCode();
                hc = (hc * 23) + StringComparer.Ordinal.GetHashCode(Value);
                return ((int)(hc >> 32)) ^ (int)hc;
            }
        }

        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString() => Value;

        #endregion
    }
}
