#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using System;

namespace SourceCode.Clay.OpenApi
{
    /// <summary>
    /// Represents a JSON scalar value.
    /// </summary>
    public struct OasScalarValue : IEquatable<OasScalarValue>, IFormattable
    {
        #region Fields

        private readonly byte _typeCode;
        private readonly Number _number;
        private readonly string _string;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the <see cref="Number"/> value.
        /// </summary>
        public Number Number
        {
            get
            {
                if (_typeCode == 0) return default;
                else if (_typeCode == (byte)TypeCode.Boolean) throw new InvalidOperationException("This scalar contains a Boolean.");
                else if (_typeCode == (byte)TypeCode.String) throw new InvalidOperationException("This scalar contains a String.");
                else return _number;
            }
        }

        /// <summary>
        /// Gets the <see cref="bool"/> value.
        /// </summary>
        public bool Boolean
        {
            get
            {
                if (_typeCode == 0) throw new InvalidOperationException("This scalar contains a null value.");
                else if (_typeCode == (byte)TypeCode.Boolean) return _number.Int32 != 0;
                else if (_typeCode == (byte)TypeCode.String) throw new InvalidOperationException("This scalar contains a String.");
                else throw new InvalidOperationException("This scalar contains a Number.");
            }
        }

        /// <summary>
        /// Gets the <see cref="bool"/> value.
        /// </summary>
        public string String
        {
            get
            {
                if (_typeCode == 0) return null;
                else if (_typeCode == (byte)TypeCode.Boolean) throw new InvalidOperationException("This scalar contains a Boolean.");
                else if (_typeCode == (byte)TypeCode.String) return _string;
                else throw new InvalidOperationException("This scalar contains a Number.");
            }
        }

        /// <summary>
        /// Gets the value <see cref="TypeCode"/>;
        /// </summary>
        public TypeCode ValueTypeCode => (TypeCode)_typeCode;

        /// <summary>
        /// Gets a value indicating whether this instance contains a value.
        /// </summary>
        public bool HasValue => _typeCode != 0;

        /// <summary>
        /// Gets the contained value as a boxed value.
        /// </summary>
        public object Value
        {
            get
            {
                switch (ValueTypeCode)
                {
                    case TypeCode.Boolean: return Number.Int32 != 0;
                    case TypeCode.Empty: return null;
                    case TypeCode.String: return _string;
                    default: return _number;
                }
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new <see cref="OasScalarValue"/> containing a <see cref="Number"/>.
        /// </summary>
        /// <param name="number">The number.</param>
        public OasScalarValue(Number? number)
        {
            if (number.HasValue)
            {
                _typeCode = (byte)number.Value.ValueTypeCode;
                _number = number.Value;
                _string = null;
            }
            else
            {
                _typeCode = 0;
                _number = default;
                _string = default;
            }
        }

        /// <summary>
        /// Creates a new <see cref="OasScalarValue"/> containing a <see cref="string"/>.
        /// </summary>
        /// <param name="string">The string.</param>
        public OasScalarValue(string @string)
        {
            if (@string == null)
            {
                _typeCode = 0;
                _number = default;
                _string = default;
            }
            else
            {
                _typeCode = (byte)TypeCode.String;
                _number = default;
                _string = @string;
            }
        }

        /// <summary>
        /// Creates a new <see cref="OasScalarValue"/> containing a <see cref="bool"/>.
        /// </summary>
        /// <param name="boolean">The boolean.</param>
        public OasScalarValue(bool boolean)
        {
            _typeCode = (byte)TypeCode.Boolean;
            _number = new Number(boolean ? 1 : 0);
            _string = null;
        }

        #endregion

        #region IEquatable

        /// <summary>Indicates whether this instance and a specified object are equal.</summary>
        /// <param name="obj">The object to compare with the current instance.</param>
        /// <returns>true if <paramref name="obj">obj</paramref> and this instance are the same type and represent the same value; otherwise, false.</returns>
        public override bool Equals(object obj) => obj is OasScalarValue o && Equals(o);

        /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>true if the current object is equal to the <paramref name="other">other</paramref> parameter; otherwise, false.</returns>
        public bool Equals(OasScalarValue other)
        {
            if (_typeCode != other._typeCode) return false;
            if (_typeCode == 0) return true;

            switch (_typeCode)
            {
                case (byte)TypeCode.String: return StringComparer.Ordinal.Equals(_string, other._string);
                default: return _number.Equals(other._number);
            }
        }

        /// <summary>Returns the hash code for this instance.</summary>
        /// <returns>A 32-bit signed integer that is the hash code for this instance.</returns>
        public override int GetHashCode()
        {
            var hc = new HashCode();

            hc.Add(_typeCode);
            switch (_typeCode)
            {
                case 0: break;
                case (byte)TypeCode.String: hc.Add(_string ?? string.Empty, StringComparer.Ordinal); break;
                default: hc.Add(_number); break;
            }

            return hc.ToHashCode();
        }

        #endregion

        #region Operators

        /// <summary>
        /// Determines if <paramref name="x"/> is a similar value to <paramref name="y"/>.
        /// </summary>
        /// <param name="x">The first <see cref="OasScalarValue"/> to compare.</param>
        /// <param name="y">The second <see cref="OasScalarValue"/> to compare.</param>
        /// <returns>
        /// A value indicating whether the first <see cref="OasScalarValue"/> is equal to <see cref="OasScalarValue"/>.
        /// </returns>
        public static bool operator ==(OasScalarValue x, OasScalarValue y) => x.Equals(y);

        /// <summary>
        /// Determines if <paramref name="x"/> is not a similar version to <paramref name="y"/>.
        /// </summary>
        /// <param name="x">The first <see cref="OasScalarValue"/> to compare.</param>
        /// <param name="y">The second <see cref="OasScalarValue"/> to compare.</param>
        /// <returns>
        /// A value indicating whether the first <see cref="OasScalarValue"/> is not similar to <see cref="OasScalarValue"/>.
        /// </returns>
        public static bool operator !=(OasScalarValue x, OasScalarValue y) => !(x == y);

        #endregion

        #region String

        /// <summary>Formats the value of the current instance.</summary>
        /// <returns>The value of the current instance.</returns>
        public override string ToString() => ToString(null, null);

        /// <summary>Formats the value of the current instance using the specified format.</summary>
        /// <param name="format">The format to use.   -or-   A null reference (Nothing in Visual Basic) to use the default format defined for the type of the <see cref="T:System.IFormattable"></see> implementation.</param>
        /// <returns>The value of the current instance in the specified format.</returns>
        public string ToString(string format) => ToString(format, null);

        /// <summary>Formats the value of the current instance using the specified format.</summary>
        /// <param name="format">The format to use.   -or-   A null reference (Nothing in Visual Basic) to use the default format defined for the type of the <see cref="T:System.IFormattable"></see> implementation.</param>
        /// <param name="formatProvider">The provider to use to format the value.   -or-   A null reference (Nothing in Visual Basic) to obtain the numeric format information from the current locale setting of the operating system.</param>
        /// <returns>The value of the current instance in the specified format.</returns>
        public string ToString(string format, IFormatProvider formatProvider)
        {
            switch (ValueTypeCode)
            {
                case TypeCode.Boolean: return (Number.Int32 != 0).ToString(formatProvider);
                case TypeCode.Empty: return string.Empty;
                case TypeCode.String: return _string;
                default: return _number.ToString(format, formatProvider);
            }
        }

        #endregion
    }
}
