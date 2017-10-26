#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using System;
using System.Globalization;
using System.Text;

namespace SourceCode.Clay.OpenApi
{
    /// <summary>
    /// Represents a JSON count validation.
    /// </summary>
    public struct OasCountRange : IEquatable<OasCountRange>
    {
        #region Properties

        /// <summary>
        /// Gets the minimum value.
        /// </summary>
        public uint? Minimum { get; }

        /// <summary>
        /// Gets the maximum value.
        /// </summary>
        public uint? Maximum { get; }

        /// <summary>
        /// Gets the range options.
        /// </summary>
        public OasRangeOptions RangeOptions { get; }

        /// <summary>
        /// Gets a value indicating whether either <see cref="Minimum"/> or <see cref="Maximum"/>
        /// has a value.
        /// </summary>
        public bool HasValue => Minimum.HasValue || Maximum.HasValue;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new inclusive <see cref="OasNumberRange"/> value.
        /// </summary>
        /// <param name="minimum">The minimum value.</param>
        /// <param name="maximum">The maximum value.</param>
        public OasCountRange(uint? minimum, uint? maximum)
            : this(minimum, maximum, OasRangeOptions.Inclusive)
        {
        }

        /// <summary>
        /// Creates a new <see cref="OasNumberRange"/> value.
        /// </summary>
        /// <param name="minimum">The minimum count.</param>
        /// <param name="maximum">The maximum count.</param>
        /// <param name="rangeOptions">The range options.</param>
        public OasCountRange(uint? minimum, uint? maximum, OasRangeOptions rangeOptions)
        {
            rangeOptions &= OasRangeOptions.Inclusive;
            if (!minimum.HasValue) rangeOptions &= ~OasRangeOptions.MinimumInclusive;
            if (!maximum.HasValue) rangeOptions &= ~OasRangeOptions.MaximumInclusive;

            Minimum = minimum;
            Maximum = maximum;
            RangeOptions = rangeOptions;
        }

        #endregion

        #region IEquatable

        /// <summary>Indicates whether this instance and a specified object are equal.</summary>
        /// <param name="obj">The object to compare with the current instance.</param>
        /// <returns>true if <paramref name="obj">obj</paramref> and this instance are the same type and represent the same value; otherwise, false.</returns>
        public override bool Equals(object obj)
            => obj is OasNumberRange other
            && Equals(other);

        /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>true if the current object is equal to the <paramref name="other">other</paramref> parameter; otherwise, false.</returns>
        public bool Equals(OasCountRange other)
        {
            if (!NumberComparer.Default.Equals(Minimum, other.Minimum)) return false;
            if (!NumberComparer.Default.Equals(Maximum, other.Maximum)) return false;
            if (RangeOptions != other.RangeOptions) return false;

            return true;
        }

        /// <summary>Returns the hash code for this instance.</summary>
        /// <returns>A 32-bit signed integer that is the hash code for this instance.</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                var hc = 17L;

                if (Minimum.HasValue)
                    hc = (hc * 23) + Minimum.Value;
                if (Maximum.HasValue)
                    hc = (hc * 23) + Maximum.Value;
                hc = (hc * 23) + RangeOptions.GetHashCode();

                return ((int)(hc >> 32)) ^ (int)hc;
            }
        }

        /// <summary>Returns the fully qualified type name of this instance.</summary>
        /// <returns>The fully qualified type name.</returns>
        public override string ToString()
        {
            if (!HasValue) return string.Empty;

            var sb = new StringBuilder();

            if (RangeOptions.HasFlag(OasRangeOptions.MinimumInclusive)) sb.Append("[");
            else sb.Append("(");

            if (Minimum.HasValue) sb.Append(Minimum.Value.ToString(CultureInfo.InvariantCulture));

            sb.Append(", ");

            if (Maximum.HasValue) sb.Append(Maximum.Value.ToString(CultureInfo.InvariantCulture));

            if (RangeOptions.HasFlag(OasRangeOptions.MaximumInclusive)) sb.Append("]");
            else sb.Append(")");

            return sb.ToString();
        }

        #endregion

        #region Operators

        /// <summary>
        /// Determines if <paramref name="x"/> is a similar value to <paramref name="y"/>.
        /// </summary>
        /// <param name="x">The first <see cref="OasCountRange"/> to compare.</param>
        /// <param name="y">The second <see cref="OasCountRange"/> to compare.</param>
        /// <returns>
        /// A value indicating whether the first <see cref="OasCountRange"/> is equal to <see cref="OasCountRange"/>.
        /// </returns>
        public static bool operator ==(OasCountRange x, OasCountRange y) => x.Equals(y);

        /// <summary>
        /// Determines if <paramref name="x"/> is not a similar version to <paramref name="y"/>.
        /// </summary>
        /// <param name="x">The first <see cref="OasCountRange"/> to compare.</param>
        /// <param name="y">The second <see cref="OasCountRange"/> to compare.</param>
        /// <returns>
        /// A value indicating whether the first <see cref="OasCountRange"/> is not similar to <see cref="OasCountRange"/>.
        /// </returns>
        public static bool operator !=(OasCountRange x, OasCountRange y) => !(x == y);

        #endregion
    }
}
