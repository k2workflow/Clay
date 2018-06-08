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
    /// Represents a Json range constraint.
    /// </summary>
    public readonly struct NumberConstraint : IEquatable<NumberConstraint>
    {
        private static readonly NumberConstraint _empty;
        private static readonly NumberConstraint _forByte = new NumberConstraint(byte.MinValue, byte.MaxValue);
        private static readonly NumberConstraint _forSByte = new NumberConstraint(sbyte.MinValue, sbyte.MaxValue);
        private static readonly NumberConstraint _forInt16 = new NumberConstraint(short.MinValue, short.MaxValue);
        private static readonly NumberConstraint _forUInt16 = new NumberConstraint(ushort.MinValue, ushort.MaxValue);
        private static readonly NumberConstraint _forInt32 = new NumberConstraint(int.MinValue, int.MaxValue);
        private static readonly NumberConstraint _forUInt32 = new NumberConstraint(uint.MinValue, uint.MaxValue);
        private static readonly NumberConstraint _forInt64 = new NumberConstraint(long.MinValue, long.MaxValue);
        private static readonly NumberConstraint _forUInt64 = new NumberConstraint(ulong.MinValue, ulong.MaxValue);
        private static readonly NumberConstraint _forSingle = new NumberConstraint(float.MinValue, float.MaxValue);

        public static ref readonly NumberConstraint Empty => ref _empty;

        public static ref readonly NumberConstraint ForByte => ref _forByte;

        public static ref readonly NumberConstraint ForSByte => ref _forSByte;

        public static ref readonly NumberConstraint ForInt16 => ref _forInt16;

        public static ref readonly NumberConstraint ForUInt16 => ref _forUInt16;

        public static ref readonly NumberConstraint ForInt32 => ref _forInt32;

        public static ref readonly NumberConstraint ForUInt32 => ref _forUInt32;

        public static ref readonly NumberConstraint ForInt64 => ref _forInt64;

        public static ref readonly NumberConstraint ForUInt64 => ref _forUInt64;

        public static ref readonly NumberConstraint ForSingle => ref _forSingle;

        /// <summary>
        /// Gets the minimum value.
        /// </summary>
        public Number? Minimum { get; }

        /// <summary>
        /// Gets the maximum value.
        /// </summary>
        public Number? Maximum { get; }

        /// <summary>
        /// Gets the range options.
        /// </summary>
        public RangeOptions RangeOptions { get; }

        /// <summary>
        /// Gets a value that the range should be a multiple of.
        /// </summary>
        public Number? MultipleOf { get; }

        /// <summary>
        /// Gets a value specifying whether a value is required.
        /// </summary>
        public bool Required { get; }

        /// <summary>
        /// Gets a value indicating whether any of the constraints are specified.
        /// </summary>
        public bool IsConstrained => Minimum.HasValue || Maximum.HasValue || MultipleOf.HasValue || Required;

        /// <summary>
        /// Creates a new <see cref="NumberConstraint"/> value.
        /// </summary>
        /// <param name="minimum">The minimum value.</param>
        /// <param name="maximum">The maximum value.</param>
        /// <param name="rangeOptions">The range options.</param>
        /// <param name="multipleOf">The value which the range should be a multiple of.</param>
        public NumberConstraint(Number? minimum, Number? maximum, RangeOptions rangeOptions, Number? multipleOf, bool required)
        {
            // Validate min/max/multiple
            rangeOptions &= RangeOptions.Inclusive;
            if (!minimum.HasValue) rangeOptions &= ~RangeOptions.MinimumInclusive;
            if (!maximum.HasValue) rangeOptions &= ~RangeOptions.MaximumInclusive;

            if (maximum.HasValue)
            {
                if (minimum.HasValue)
                {
                    // Ensure min and max are of the same general type (integer, real or decimal)
                    if (minimum.Value.Kind != maximum.Value.Kind)
                        throw new ArgumentOutOfRangeException(nameof(maximum), $"{nameof(NumberConstraint)} {nameof(minimum)} and {nameof(maximum)} should have the same {nameof(NumberKinds)}");

                    if (minimum.Value > maximum.Value)
                        throw new ArgumentOutOfRangeException(nameof(maximum));

                    if (minimum.Value == maximum.Value
                        && rangeOptions != RangeOptions.Inclusive)
                        throw new ArgumentOutOfRangeException(nameof(maximum));
                }

                if (multipleOf.HasValue)
                {
                    // Ensure min and max are of the same general type (integer, real or decimal)
                    if (multipleOf.Value.Kind != maximum.Value.Kind)
                        throw new ArgumentOutOfRangeException(nameof(multipleOf), $"{nameof(NumberConstraint)} {nameof(multipleOf)} and {nameof(maximum)} should have the same {nameof(NumberKinds)}");

                    if (multipleOf.Value > maximum.Value)
                        throw new ArgumentOutOfRangeException(nameof(multipleOf));
                }
            }

            Minimum = minimum;
            Maximum = maximum;
            MultipleOf = multipleOf;
            RangeOptions = rangeOptions;
            Required = required;
        }

        /// <summary>
        /// Creates a new <see cref="NumberConstraint"/> value.
        /// </summary>
        /// <param name="minimum">The minimum value.</param>
        /// <param name="maximum">The maximum value.</param>
        /// <param name="rangeOptions">The range options.</param>
        public NumberConstraint(Number? minimum, Number? maximum, RangeOptions rangeOptions)
            : this(minimum, maximum, rangeOptions, default, default)
        { }

        /// <summary>
        /// Creates a new <see cref="NumberConstraint"/> value.
        /// </summary>
        /// <param name="minimum">The minimum value.</param>
        /// <param name="maximum">The maximum value.</param>
        public NumberConstraint(Number? minimum, Number? maximum)
            : this(minimum, maximum, default, default, default)
        { }

        public bool IsValid(Number? value)
        {
            // Check Required
            if (!value.HasValue)
                return !(Required); // null + optional = true, null + required = false

            // Check Min
            if (Minimum.HasValue)
            {
                if ((RangeOptions & RangeOptions.MinimumInclusive) > 0)
                {
                    if (value < Minimum) return false;
                }
                else if (value <= Minimum) return false;
            }

            // Check Max
            if (Maximum.HasValue)
            {
                if ((RangeOptions & RangeOptions.MaximumInclusive) > 0)
                {
                    if (value > Maximum) return false;
                }
                else if (value >= Maximum) return false;
            }

            // MultipleOf
            if (MultipleOf.HasValue
                && MultipleOf.Value != 0 // n % 0 == undefined
                && !value.Value.IsZero) // 0 % n == 0 (we already know value.HasValue is true)
            {
                if ((value.Value.Kind & NumberKinds.Integer) > 0)
                {
                    var val = value.Value.ToInt64();
                    var zero = val % MultipleOf.Value.ToInt64() == 0;
                    if (!zero) return false;
                }
                else if ((value.Value.Kind & NumberKinds.Real) > 0)
                {
                    var val = value.Value.ToDouble();
                    var zero = val % MultipleOf.Value.ToDouble() == 0.0; // Modulus(Double) is a well-defined operation
                    if (!zero) return false;
                }
                else if ((value.Value.Kind & NumberKinds.Decimal) > 0)
                {
                    var val = value.Value.ToDecimal();
                    var zero = val % MultipleOf.Value.ToDecimal() == 0; // Modulus(Decimal) is a well-defined operation
                    if (!zero) return false;
                }
            }

            return true;
        }

        /// <summary>Indicates whether this instance and a specified object are equal.</summary>
        /// <param name="obj">The object to compare with the current instance.</param>
        /// <returns>true if <paramref name="obj">obj</paramref> and this instance are the same type and represent the same value; otherwise, false.</returns>
        public override bool Equals(object obj)
            => obj is NumberConstraint other
            && Equals(other);

        /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>true if the current object is equal to the <paramref name="other">other</paramref> parameter; otherwise, false.</returns>
        public bool Equals(NumberConstraint other)
        {
            if (!NumberComparer.Default.Equals(Minimum, other.Minimum)) return false;
            if (!NumberComparer.Default.Equals(Maximum, other.Maximum)) return false;
            if (RangeOptions != other.RangeOptions) return false;
            if (MultipleOf != other.MultipleOf) return false;
            if (Required != other.Required) return false;

            return true;
        }

        /// <summary>Returns the hash code for this instance.</summary>
        /// <returns>A 32-bit signed integer that is the hash code for this instance.</returns>
        public override int GetHashCode()
            => HashCode.Combine(Minimum ?? (default), Maximum ?? (default), RangeOptions, MultipleOf ?? (default), Required);

        /// <summary>
        /// Determines if <paramref name="x"/> is a similar value to <paramref name="y"/>.
        /// </summary>
        /// <param name="x">The first <see cref="NumberConstraint"/> to compare.</param>
        /// <param name="y">The second <see cref="NumberConstraint"/> to compare.</param>
        /// <returns>
        /// A value indicating whether the first <see cref="NumberConstraint"/> is equal to <see cref="NumberConstraint"/>.
        /// </returns>
        public static bool operator ==(NumberConstraint x, NumberConstraint y) => x.Equals(y);

        /// <summary>
        /// Determines if <paramref name="x"/> is not a similar version to <paramref name="y"/>.
        /// </summary>
        /// <param name="x">The first <see cref="NumberConstraint"/> to compare.</param>
        /// <param name="y">The second <see cref="NumberConstraint"/> to compare.</param>
        /// <returns>
        /// A value indicating whether the first <see cref="NumberConstraint"/> is not similar to <see cref="NumberConstraint"/>.
        /// </returns>
        public static bool operator !=(NumberConstraint x, NumberConstraint y) => !(x == y);

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
                && !MultipleOf.Value.IsZero)
                sb.Append(" * ").Append(MultipleOf.Value.ToString(CultureInfo.InvariantCulture));

            return sb.ToString();
        }
    }
}
