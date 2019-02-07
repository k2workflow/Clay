#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using System;
using System.Globalization;
using System.Text;

namespace SourceCode.Clay.Json.Validation
{
    /// <summary>
    /// Represents a Json count constraint.
    /// </summary>
    public readonly struct CountConstraint : IEquatable<CountConstraint>
    {
        private static readonly CountConstraint s_empty;
        private static readonly CountConstraint s_forByte = new CountConstraint(byte.MinValue, byte.MaxValue);
        private static readonly CountConstraint s_forUInt16 = new CountConstraint(ushort.MinValue, ushort.MaxValue);

        public static ref readonly CountConstraint Empty => ref s_empty;

        public static ref readonly CountConstraint ForByte => ref s_forByte;

        public static ref readonly CountConstraint ForUInt16 => ref s_forUInt16;

        /// <summary>
        /// Gets the minimum value.
        /// </summary>
        public uint Minimum { get; }

        /// <summary>
        /// Gets the maximum value.
        /// </summary>
        public uint? Maximum { get; }

        /// <summary>
        /// Gets a value indicating whether <see cref="Maximum"/> has a value.
        /// </summary>
        public bool IsBounded => Maximum.HasValue; // Minimum is not nullable

        /// <summary>
        /// Creates a new <see cref="CountConstraint"/> value.
        /// </summary>
        /// <param name="minimum">The minimum count.</param>
        /// <param name="maximum">The maximum count.</param>
        public CountConstraint(uint? minimum, uint? maximum)
        {
            var min = minimum ?? 0;

            if (maximum.HasValue
                && min > maximum.Value)
                throw new ArgumentOutOfRangeException(nameof(maximum));

            Minimum = min;
            Maximum = maximum;
        }

        /// <summary>
        /// Creates a new <see cref="CountConstraint"/> value with an exact value.
        /// </summary>
        /// <param name="exact">The minimum count.</param>
        public CountConstraint(uint exact)
            : this(exact, exact)
        { }

        /// <summary>
        /// Implicitly converts from a <see cref="uint"/> to an exact <see cref="CountConstraint"/>.
        /// </summary>
        /// <param name="count">The minimum and maximum value.</param>
        public static CountConstraint Exact(uint count) => new CountConstraint(count, count);

        public bool IsValid(long value)
        {
            // Check Min
            if (value < Minimum)
                return false;

            // Check Max
            return !Maximum.HasValue
                || value <= Maximum.Value;
        }

        /// <summary>Indicates whether this instance and a specified object are equal.</summary>
        /// <param name="obj">The object to compare with the current instance.</param>
        /// <returns>true if <paramref name="obj">obj</paramref> and this instance are the same type and represent the same value; otherwise, false.</returns>
        public override bool Equals(object obj)
            => obj is CountConstraint other
            && Equals(other);

        /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>true if the current object is equal to the <paramref name="other">other</paramref> parameter; otherwise, false.</returns>
        public bool Equals(CountConstraint other)
            => (Minimum, Maximum)
            == (other.Minimum, other.Maximum);

        /// <summary>Returns the hash code for this instance.</summary>
        /// <returns>A 32-bit signed integer that is the hash code for this instance.</returns>
        public override int GetHashCode()
#if NETCOREAPP2_2
            => HashCode.Combine(Minimum, Maximum ?? 0);
#else
        {
            int hc = 11;
            unchecked
            {
                hc = hc * 7 + (int)Minimum;
                hc = hc * 7 + (int?)Maximum ?? 0;
            }
            return hc;
        }
#endif

        /// <summary>
        /// Determines if <paramref name="x"/> is a similar value to <paramref name="y"/>.
        /// </summary>
        /// <param name="x">The first <see cref="CountConstraint"/> to compare.</param>
        /// <param name="y">The second <see cref="CountConstraint"/> to compare.</param>
        /// <returns>
        /// A value indicating whether the first <see cref="CountConstraint"/> is equal to <see cref="CountConstraint"/>.
        /// </returns>
        public static bool operator ==(CountConstraint x, CountConstraint y) => x.Equals(y);

        /// <summary>
        /// Determines if <paramref name="x"/> is not a similar version to <paramref name="y"/>.
        /// </summary>
        /// <param name="x">The first <see cref="CountConstraint"/> to compare.</param>
        /// <param name="y">The second <see cref="CountConstraint"/> to compare.</param>
        /// <returns>
        /// A value indicating whether the first <see cref="CountConstraint"/> is not similar to <see cref="CountConstraint"/>.
        /// </returns>
        public static bool operator !=(CountConstraint x, CountConstraint y) => !(x == y);

#pragma warning disable CA2225 // Operator overloads have named alternates

        /// <summary>
        /// Implicitly converts from a <see cref="uint"/> to an exact <see cref="CountConstraint"/>.
        /// </summary>
        /// <param name="exact">The minimum and maximum value.</param>
        public static implicit operator CountConstraint(uint exact) => Exact(exact);

#pragma warning restore CA2225 // Operator overloads have named alternates

        /// <summary>Returns the fully qualified type name of this instance.</summary>
        /// <returns>The fully qualified type name.</returns>
        public override string ToString()
        {
            if (!IsBounded) return string.Empty;

            var sb = new StringBuilder();

            sb.Append("[0, ");

            if (Maximum.HasValue)
                sb.Append(Maximum.Value.ToString(CultureInfo.InvariantCulture));
            else
                sb.Append("∞");

            sb.Append("]");

            return sb.ToString();
        }
    }
}
