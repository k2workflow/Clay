using System;

namespace SourceCode.Clay.Text
{
    /// <summary>
    /// A wrapper for <see cref="System.String"/> that automatically compares values using <see cref="System.StringComparison.Ordinal"/>.
    /// Uses implicit conversions to/from <see cref="System.String"/>.
    /// </summary>
    /// <seealso cref="IEquatable{T}"/>
    public struct OrdinalCaseSensitiveString : IEquatable<OrdinalCaseSensitiveString>, IEquatable<string>
    {
        private readonly string _str;

        public OrdinalCaseSensitiveString(string str)
        {
            _str = str;
        }

        public static implicit operator OrdinalCaseSensitiveString(string str)
            => new OrdinalCaseSensitiveString(str);

        public static explicit operator string(OrdinalCaseSensitiveString str)
            => str._str;

        /// <summary>
        /// Check equality using <see cref="System.StringComparison.Ordinal"/>.
        /// </summary>
        /// <param name="other">The other string.</param>
        /// <returns></returns>
        public bool Equals(string other)
            => string.Equals(_str, other, StringComparison.Ordinal);

        /// <summary>
        /// Check equality using <see cref="System.StringComparison.Ordinal"/>.
        /// </summary>
        /// <param name="other">The other string.</param>
        /// <returns></returns>
        public bool Equals(OrdinalCaseSensitiveString other)
            => string.Equals(_str, other._str, StringComparison.Ordinal);

        /// <summary>
        /// Check equality using <see cref="System.StringComparison.Ordinal"/>.
        /// </summary>
        /// <param name="obj">The other object.</param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(obj, null))
                return _str == null;

            if (obj is OrdinalCaseSensitiveString ics)
                return string.Equals(_str, ics._str, StringComparison.Ordinal);

            if (obj is string str)
                return string.Equals(_str, str, StringComparison.Ordinal);

            return false;
        }

        public override int GetHashCode()
            => _str?.GetHashCode() ?? 0;
    }
}
