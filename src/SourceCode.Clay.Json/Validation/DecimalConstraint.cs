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
    /// Represents a Json Decimal value constraint.
    /// </summary>
    public struct DecimalConstraint : IEquatable<DecimalConstraint>
    {
        #region Properties

        /// <summary>
        /// Gets the minimum value.
        /// </summary>
        public decimal? Minimum { get; }

        /// <summary>
        /// Gets the maximum value.
        /// </summary>
        public decimal? Maximum { get; }

        /// <summary>
        /// Gets the range options.
        /// </summary>
        public RangeOptions RangeOptions { get; }

        /// <summary>
        /// Gets a value that the value should be a multiple of.
        /// </summary>
        public decimal? MultipleOf { get; }

        /// <summary>
        /// Gets a value specifying whether a value is required.
        /// </summary>
        public bool Required { get; }

        /// <summary>
        /// Gets a value indicating whether any of the constraints are specified.
        /// </summary>
        public bool IsConstrained => Minimum.HasValue || Maximum.HasValue || MultipleOf.HasValue || Required;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new <see cref="DecimalConstraint"/> value.
        /// </summary>
        /// <param name="minimum">The minimum value.</param>
        /// <param name="maximum">The maximum value.</param>
        /// <param name="rangeOptions">The range options.</param>
        /// <param name="multipleOf">The value which the value should be a multiple of.</param>
        /// <param name="required">Whether or not a value is required.</param>
        public DecimalConstraint(decimal? minimum, decimal? maximum, RangeOptions rangeOptions, decimal? multipleOf, bool required)
        {
            rangeOptions &= RangeOptions.Inclusive;
            if (!minimum.HasValue) rangeOptions &= ~RangeOptions.MinimumInclusive;
            if (!maximum.HasValue) rangeOptions &= ~RangeOptions.MaximumInclusive;

            // Validate min/max/multiple
            if (maximum.HasValue)
            {
                if (minimum.HasValue)
                {
                    if (minimum.Value > maximum.Value)
                        throw new ArgumentOutOfRangeException(nameof(maximum));

                    if (minimum.Value == maximum.Value
                        && rangeOptions != RangeOptions.Inclusive)
                        throw new ArgumentOutOfRangeException(nameof(maximum));
                }

                if (multipleOf.HasValue
                    && multipleOf.Value > maximum.Value)
                    throw new ArgumentOutOfRangeException(nameof(multipleOf));
            }

            Minimum = minimum;
            Maximum = maximum;
            RangeOptions = rangeOptions;
            MultipleOf = multipleOf;
            Required = required;
        }

        /// <summary>
        /// Creates a new <see cref="DecimalConstraint"/> value.
        /// </summary>
        /// <param name="minimum">The minimum value.</param>
        /// <param name="maximum">The maximum value.</param>
        /// <param name="rangeOptions">The range options.</param>
        public DecimalConstraint(decimal? minimum, decimal? maximum, RangeOptions rangeOptions)
            : this(minimum, maximum, rangeOptions, default, default)
        { }

        /// <summary>
        /// Creates a new <see cref="DecimalConstraint"/> value.
        /// </summary>
        /// <param name="minimum">The minimum value.</param>
        /// <param name="maximum">The maximum value.</param>
        public DecimalConstraint(decimal? minimum, decimal? maximum)
            : this(minimum, maximum, default, default, default)
        { }

        #endregion

        #region Methods

        public bool IsValid(decimal? value)
        {
            // Check Required
            if (!value.HasValue)
                return !(Required); // null + optional = true, null + required = false

            // Check Min
            if (Minimum.HasValue)
            {
                if ((RangeOptions & RangeOptions.MinimumInclusive) > 0)
                {
                    if (value.Value < Minimum.Value) return false;
                }
                else if (value.Value <= Minimum.Value) return false;
            }

            // Check Max
            if (Maximum.HasValue)
            {
                if ((RangeOptions & RangeOptions.MaximumInclusive) > 0)
                {
                    if (value.Value > Maximum.Value) return false;
                }
                else if (value.Value >= Maximum.Value) return false;
            }

            // MultipleOf
            if (MultipleOf.HasValue
                && MultipleOf.Value != 0 // n % 0 == undefined
                && value.Value != 0) // 0 % n == 0 (we already know value.HasValue is true)
            {
                var zero = value.Value % MultipleOf.Value == 0;
                if (!zero) return false;
            }

            return true;
        }

        #endregion

        #region IEquatable

        /// <summary>Indicates whether this instance and a specified object are equal.</summary>
        /// <param name="obj">The object to compare with the current instance.</param>
        /// <returns>true if <paramref name="obj">obj</paramref> and this instance are the same type and represent the same value; otherwise, false.</returns>
        public override bool Equals(object obj)
            => obj is DecimalConstraint other
            && Equals(other);

        /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>true if the current object is equal to the <paramref name="other">other</paramref> parameter; otherwise, false.</returns>
        public bool Equals(DecimalConstraint other)
        {
            if (Minimum != other.Minimum) return false;
            if (Maximum != other.Maximum) return false;
            if (RangeOptions != other.RangeOptions) return false;
            if (MultipleOf != other.MultipleOf) return false;
            if (Required != other.Required) return false;

            return true;
        }

        /// <summary>Returns the hash code for this instance.</summary>
        /// <returns>A 32-bit signed integer that is the hash code for this instance.</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                decimal hc = 17;

                if (Minimum.HasValue)
                    hc = (hc * 23) + Minimum.Value;

                if (Maximum.HasValue)
                    hc = (hc * 23) + Maximum.Value;

                hc = (hc * 23) + (int)RangeOptions;

                if (MultipleOf.HasValue)
                    hc = (hc * 23) + MultipleOf.Value;

                hc = (hc * 23) + (Required ? 1 : 0);

                return ((int)((long)hc >> 32)) ^ (int)hc;
            }
        }

        #endregion

        #region Operators

        /// <summary>
        /// Determines if <paramref name="x"/> is a similar value to <paramref name="y"/>.
        /// </summary>
        /// <param name="x">The first <see cref="DecimalConstraint"/> to compare.</param>
        /// <param name="y">The second <see cref="DecimalConstraint"/> to compare.</param>
        /// <returns>
        /// A value indicating whether the first <see cref="DecimalConstraint"/> is equal to <see cref="DecimalConstraint"/>.
        /// </returns>
        public static bool operator ==(DecimalConstraint x, DecimalConstraint y) => x.Equals(y);

        /// <summary>
        /// Determines if <paramref name="x"/> is not a similar version to <paramref name="y"/>.
        /// </summary>
        /// <param name="x">The first <see cref="DecimalConstraint"/> to compare.</param>
        /// <param name="y">The second <see cref="DecimalConstraint"/> to compare.</param>
        /// <returns>
        /// A value indicating whether the first <see cref="DecimalConstraint"/> is not similar to <see cref="DecimalConstraint"/>.
        /// </returns>
        public static bool operator !=(DecimalConstraint x, DecimalConstraint y) => !(x == y);

        /// <summary>Returns the fully qualified type name of this instance.</summary>
        /// <returns>The fully qualified type name.</returns>
        public override string ToString()
        {
            if (!IsConstrained) return string.Empty;

            // Use set notation for open/closed boundaries
            var sb = new StringBuilder();

            if (RangeOptions.HasFlag(RangeOptions.MinimumInclusive))
                sb.Append("[");
            else
                sb.Append("(");

            if (Minimum.HasValue)
                sb.Append(Minimum.Value.ToString(CultureInfo.InvariantCulture));
            else
                sb.Append("-∞");

            sb.Append(", ");

            if (Maximum.HasValue)
                sb.Append(Maximum.Value.ToString(CultureInfo.InvariantCulture));
            else
                sb.Append("∞");

            if (RangeOptions.HasFlag(RangeOptions.MaximumInclusive))
                sb.Append("]");
            else
                sb.Append(")");

            if (MultipleOf.HasValue
                && MultipleOf.Value != 0)
                sb.Append(" * ").Append(MultipleOf.Value.ToString(CultureInfo.InvariantCulture));

            return sb.ToString();
        }

        #endregion
    }
}
