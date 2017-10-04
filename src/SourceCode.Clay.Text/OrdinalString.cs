using System;

namespace SourceCode.Clay.Text
{
    /// <summary>
    /// A wrapper for <see cref="System.String"/> that automatically compares values using <see cref="System.StringComparison.Ordinal"/>.
    /// Uses implicit conversions to/from <see cref="System.String"/>.
    /// </summary>
    /// <seealso cref="IEquatable{T}"/>
    public struct OrdinalString : IEquatable<OrdinalString>, IComparable<OrdinalString>
    {
        #region Fields

        private readonly string _str;

        #endregion

        #region Constructors

        public OrdinalString(string str)
        {
            _str = str;
        }

        #endregion

        #region Operators

        public static implicit operator OrdinalString(string str) => new OrdinalString(str);

        public static explicit operator string(OrdinalString str) => str._str;

        public static bool operator ==(OrdinalString x, OrdinalString y) => x.Equals(y);

        public static bool operator !=(OrdinalString x, OrdinalString y) => !(x == y);

        public static bool operator <(OrdinalString x, OrdinalString y) => x.CompareTo(y) < 0;

        public static bool operator >(OrdinalString x, OrdinalString y) => x.CompareTo(y) > 0;

        public static bool operator <=(OrdinalString x, OrdinalString y) => x.CompareTo(y) <= 0;

        public static bool operator >=(OrdinalString x, OrdinalString y) => x.CompareTo(y) >= 0;

        #endregion

        #region IEquatable

        /// <summary>
        /// Check equality using <see cref="System.StringComparison.Ordinal"/>.
        /// </summary>
        /// <param name="other">The other string.</param>
        /// <returns></returns>
        public bool Equals(OrdinalString other) => StringComparer.Ordinal.Equals(_str, other._str);

        /// <summary>
        /// Check equality using <see cref="System.StringComparison.Ordinal"/>.
        /// </summary>
        /// <param name="obj">The other object.</param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(obj, null))
                return _str == null;

            if (obj is OrdinalString ics)
                return StringComparer.Ordinal.Equals(_str, ics._str);

            if (obj is string str)
                return StringComparer.Ordinal.Equals(_str, str);

            return false;
        }

        public override int GetHashCode() => StringComparer.Ordinal.GetHashCode(_str);

        #endregion

        #region IComparable

        public int CompareTo(OrdinalString other)
            => string.CompareOrdinal(_str, other._str);

        #endregion
    }
}
