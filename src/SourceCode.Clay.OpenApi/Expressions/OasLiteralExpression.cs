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
    public sealed class OasLiteralExpression : OasExpressionComponent, IEquatable<OasLiteralExpression>
    {
        #region Properties

        /// <summary>
        /// Gets the literal value.
        /// </summary>
        public string Value { get; }

        /// <summary>Gets the component type.</summary>
        public override OasExpressionComponentType ComponentType => OasExpressionComponentType.Literal;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of the <see cref="OasLiteralExpression"/> class.
        /// </summary>
        /// <param name="value">The literal value.</param>
        public OasLiteralExpression(string value)
        {
            Value = value ?? throw new ArgumentNullException(nameof(value));
        }

        #endregion

        #region Methods

        /// <summary>Determines whether the specified object is equal to the current object.</summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns>true if the specified object  is equal to the current object; otherwise, false.</returns>
        public override bool Equals(object obj)
            => obj is OasLiteralExpression other
            && Equals(other);

        /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>true if the current object is equal to the <paramref name="other">other</paramref> parameter; otherwise, false.</returns>
        public bool Equals(OasLiteralExpression other)
        {
            if (other is null) return false;
            if (!StringComparer.Ordinal.Equals(Value, other.Value)) return false;
            return true;
        }

        /// <summary>Returns the hash code for this instance.</summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode() => new HashCode()
            .Tally(base.GetHashCode())
            .Tally(Value ?? string.Empty, StringComparer.Ordinal)
            .ToHashCode();

        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString() => Value;

        #endregion
    }
}
