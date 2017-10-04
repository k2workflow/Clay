using System;
using System.Globalization;

namespace SourceCode.Clay.Json.Pointers
{
    /// <summary>
    /// Represents a JSON pointer token.
    /// </summary>
    public struct JsonPointerToken : IEquatable<JsonPointerToken>
    {
        #region Properties

        private readonly string _value;
        /// <summary>
        /// Gets the member name.
        /// </summary>
        public string Value => _value ?? string.Empty;

        /// <summary>
        /// Gets the array index.
        /// </summary>
        public ushort? ArrayIndex
        {
            get
            {
                if (_value == null) return null;
                if (!ushort.TryParse(_value, NumberStyles.None, CultureInfo.InvariantCulture, out var value)) return null;
                return value;
            }
        }

        #endregion

        #region Constructor
        /// <summary>
        /// Creates a new <see cref="JsonPointerToken"/> value.
        /// </summary>
        /// <param name="value">The token value.</param>
        public JsonPointerToken(string value)
        {
            _value = value ?? throw new ArgumentNullException(nameof(value));
            if (_value == string.Empty) _value = null;
        }
        #endregion

        #region Equatable

        /// <summary>Indicates whether this instance and a specified object are equal.</summary>
        /// <param name="obj">The object to compare with the current instance.</param>
        /// <returns>true if <paramref name="obj">obj</paramref> and this instance are the same type and represent the same value; otherwise, false.</returns>
        public override bool Equals(object obj) => obj is JsonPointerToken o && Equals(o);

        /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>true if the current object is equal to the <paramref name="other">other</paramref> parameter; otherwise, false.</returns>
        public bool Equals(JsonPointerToken other)
        {
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
                if (Value != null) hc = hc * 21 + StringComparer.Ordinal.GetHashCode(Value);
                return ((int)(hc >> 32)) ^ (int)hc;
            }
        }

        public static bool operator ==(JsonPointerToken token1, JsonPointerToken token2) => token1.Equals(token2);
        public static bool operator !=(JsonPointerToken token1, JsonPointerToken token2) => !token1.Equals(token2);

        #endregion

        #region Conversion

        /// <summary>Returns the fully qualified type name of this instance.</summary>
        /// <returns>The fully qualified type name.</returns>
        public override string ToString() => Value;

        public static implicit operator JsonPointerToken(string value) => new JsonPointerToken(value);

        #endregion
    }
}
