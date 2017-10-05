using System;

namespace SourceCode.Clay.Text
{
    /// <summary>
    ///   A wrapper for <see cref="System.String"/> that automatically compares values using <see
    ///   cref="System.StringComparison.OrdinalIgnoreCase"/>. Uses implicit conversions to/from <see cref="System.String"/>.
    /// </summary>
    /// <seealso cref="IEquatable{T}"/>
    public struct OrdinalIgnoreCaseString : IEquatable<OrdinalIgnoreCaseString>, IComparable<OrdinalIgnoreCaseString>
    {
        #region Fields

        private readonly string _str;

        #endregion Fields

        #region Constructors

        public OrdinalIgnoreCaseString(string str)
        {
            _str = str;
        }

        #endregion Constructors

        #region Operators

        public static explicit operator string(OrdinalIgnoreCaseString str) => str._str;

        public static implicit operator OrdinalIgnoreCaseString(string str) => new OrdinalIgnoreCaseString(str);

        public static bool operator !=(OrdinalIgnoreCaseString x, OrdinalIgnoreCaseString y) => !(x == y);

        public static bool operator <(OrdinalIgnoreCaseString x, OrdinalIgnoreCaseString y) => x.CompareTo(y) < 0;

        public static bool operator <=(OrdinalIgnoreCaseString x, OrdinalIgnoreCaseString y) => x.CompareTo(y) <= 0;

        public static bool operator ==(OrdinalIgnoreCaseString x, OrdinalIgnoreCaseString y) => x.Equals(y);

        public static bool operator >(OrdinalIgnoreCaseString x, OrdinalIgnoreCaseString y) => x.CompareTo(y) > 0;

        public static bool operator >=(OrdinalIgnoreCaseString x, OrdinalIgnoreCaseString y) => x.CompareTo(y) >= 0;

        #endregion Operators

        #region IEquatable

        /// <summary>
        ///   Check equality using <see cref="System.StringComparison.OrdinalIgnoreCase"/>.
        /// </summary>
        /// <param name="other">The other string.</param>
        /// <returns></returns>
        public bool Equals(OrdinalIgnoreCaseString other) => StringComparer.OrdinalIgnoreCase.Equals(_str, other._str);

        /// <summary>
        ///   Check equality using <see cref="System.StringComparison.OrdinalIgnoreCase"/>.
        /// </summary>
        /// <param name="obj">The other object.</param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(obj, null))
                return _str == null;

            if (obj is OrdinalIgnoreCaseString ics)
                return StringComparer.OrdinalIgnoreCase.Equals(_str, ics._str);

            if (obj is string str)
                return StringComparer.OrdinalIgnoreCase.Equals(_str, str);

            return false;
        }

        public override int GetHashCode() => StringComparer.OrdinalIgnoreCase.GetHashCode(_str);

        #endregion IEquatable

        #region IComparable

        public int CompareTo(OrdinalIgnoreCaseString other) => StringComparer.OrdinalIgnoreCase.Compare(_str, other._str);

        #endregion IComparable
    }
}
