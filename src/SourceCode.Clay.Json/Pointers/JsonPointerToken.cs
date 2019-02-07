#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using System;
using System.Globalization;

namespace SourceCode.Clay.Json.Pointers
{
    /// <summary>
    /// Represents a Json pointer token.
    /// </summary>
    public readonly struct JsonPointerToken : IEquatable<JsonPointerToken>
    {
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
                if (_value is null) return null;
                if (!ushort.TryParse(_value, NumberStyles.None, CultureInfo.InvariantCulture, out var value)) return null;
                return value;
            }
        }

        /// <summary>
        /// Creates a new <see cref="JsonPointerToken"/> value.
        /// </summary>
        /// <param name="value">The token value.</param>
        public JsonPointerToken(string value)
        {
            _value = value ?? throw new ArgumentNullException(nameof(value));
            if (_value.Length == 0) _value = null;
        }

        /// <summary>Indicates whether this instance and a specified object are equal.</summary>
        /// <param name="obj">The object to compare with the current instance.</param>
        /// <returns>true if <paramref name="obj">obj</paramref> and this instance are the same type and represent the same value; otherwise, false.</returns>
        public override bool Equals(object obj)
            => obj is JsonPointerToken other
            && Equals(other);

        /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>true if the current object is equal to the <paramref name="other">other</paramref> parameter; otherwise, false.</returns>
        public bool Equals(JsonPointerToken other)
            => StringComparer.Ordinal.Equals(Value, other.Value);

        /// <summary>Returns the hash code for this instance.</summary>
        /// <returns>A 32-bit signed integer that is the hash code for this instance.</returns>
        public override int GetHashCode()
#if NETCOREAPP2_2
            => HashCode.Combine(Value ?? string.Empty, StringComparer.Ordinal);
#else
        {
            int hc = 11;
            unchecked
            {
                hc = hc * 7 + Value?.GetHashCode() ?? 0;
            }
            return hc;
        }
#endif

        public static bool operator ==(JsonPointerToken x, JsonPointerToken y) => x.Equals(y);

        public static bool operator !=(JsonPointerToken x, JsonPointerToken y) => !x.Equals(y);

#pragma warning disable CA2225 // Operator overloads have named alternates

        public static implicit operator JsonPointerToken(string value) => new JsonPointerToken(value);

#pragma warning restore CA2225 // Operator overloads have named alternates

        /// <summary>Returns the fully qualified type name of this instance.</summary>
        /// <returns>The fully qualified type name.</returns>
        public override string ToString() => Value;
    }
}
