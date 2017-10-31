#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using System;
using System.Text;

namespace SourceCode.Clay.Data.SqlParser
{
    public struct SqlTokenInfo : IEquatable<SqlTokenInfo>
    {
        #region Properties

        public SqlTokenKind Kind { get; }

        public string Value { get; }

        #endregion

        #region Constructors

        internal SqlTokenInfo(SqlTokenKind kind, string value)
        {
            Kind = kind;
            Value = value;
        }

        internal SqlTokenInfo(SqlTokenKind kind, StringBuilder value)
        {
            Kind = kind;
            Value = value?.ToString();
        }

        internal SqlTokenInfo(SqlTokenKind kind, params char[] value)
        {
            Kind = kind;
            Value = value == null ? null : new string(value);
        }

        internal SqlTokenInfo(SqlTokenKind kind, char[] value, int offset, int count)
        {
            Kind = kind;
            Value = new string(value, offset, count);
        }

        #endregion

        #region IEquatable

        /// <summary>Indicates whether this instance and a specified object are equal.</summary>
        /// <param name="obj">The object to compare with the current instance.</param>
        /// <returns>true if <paramref name="obj">obj</paramref> and this instance are the same type and represent the same value; otherwise, false.</returns>
        public override bool Equals(object obj)
            => obj is SqlTokenInfo other
            && Equals(other);

        /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>true if the current object is equal to the <paramref name="other">other</paramref> parameter; otherwise, false.</returns>
        public bool Equals(SqlTokenInfo other)
        {
            if (Kind != other.Kind) return false;
            if (!StringComparer.Ordinal.Equals(Value, other.Value)) return false;

            return true;
        }

        /// <summary>Returns the hash code for this instance.</summary>
        /// <returns>A 32-bit signed integer that is the hash code for this instance.</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                var hc = 17L;

                hc = (hc * 23) + (int)Kind;
                if (Value != null)
                    hc = (hc * 23) + StringComparer.Ordinal.GetHashCode(Value ?? string.Empty);

                return ((int)(hc >> 32)) ^ (int)hc;
            }
        }

        #endregion

        #region Operators

        /// <summary>
        /// Determines if <paramref name="x"/> is a similar value to <paramref name="y"/>.
        /// </summary>
        /// <param name="x">The first <see cref="SqlTokenInfo"/> to compare.</param>
        /// <param name="y">The second <see cref="SqlTokenInfo"/> to compare.</param>
        /// <returns>
        /// A value indicating whether the first <see cref="SqlTokenInfo"/> is equal to <see cref="SqlTokenInfo"/>.
        /// </returns>
        public static bool operator ==(SqlTokenInfo x, SqlTokenInfo y) => x.Equals(y);

        /// <summary>
        /// Determines if <paramref name="x"/> is not a similar version to <paramref name="y"/>.
        /// </summary>
        /// <param name="x">The first <see cref="SqlTokenInfo"/> to compare.</param>
        /// <param name="y">The second <see cref="SqlTokenInfo"/> to compare.</param>
        /// <returns>
        /// A value indicating whether the first <see cref="SqlTokenInfo"/> is not similar to <see cref="SqlTokenInfo"/>.
        /// </returns>
        public static bool operator !=(SqlTokenInfo x, SqlTokenInfo y) => !(x == y);

        public override string ToString()
        {
            switch (Kind)
            {
                case SqlTokenKind.BlockComment:
                case SqlTokenKind.LineComment:
                case SqlTokenKind.Whitespace:
                    return $"{Kind}";

                case SqlTokenKind.Literal:
                case SqlTokenKind.QuotedString:
                case SqlTokenKind.SquareString:
                case SqlTokenKind.Symbol:
                    return $"{Kind} ({Value})";
            }

            return base.ToString();
        }

        #endregion
    }
}
