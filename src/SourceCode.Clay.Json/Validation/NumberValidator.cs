#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using System;

namespace SourceCode.Clay.Json.Validation
{
    public sealed class NumberValidator
    {
        #region Properties

        public Number? Min { get; }

        public Number? Max { get; }

        public bool MinExclusive { get; }

        public bool MaxExclusive { get; }

        public bool Required { get; }

        public long? MultipleOf { get; }

        #endregion

        #region Constructors

        public NumberValidator(Number? min, Number? max, bool minExclusive, bool maxExclusive, bool required, long? multipleOf)
        {
            // Ensure min and max are of the same general type (integer or real)
            if (min.HasValue
                && max.HasValue
                && (((min.Value.Kind & NumberKinds.Integer) > 0 && (max.Value.Kind & NumberKinds.Real) > 0) ||
                    ((min.Value.Kind & NumberKinds.Real) > 0 && (max.Value.Kind & NumberKinds.Integer) > 0)))
            {
                throw new ArgumentOutOfRangeException(nameof(max), $"{nameof(NumberValidator)} {nameof(min)} and {nameof(max)} should have the same {nameof(NumberKinds)}");
            }

            Min = min;
            if (Min.HasValue && minExclusive)
                MinExclusive = true;

            Max = max;
            if (Max.HasValue && maxExclusive)
                MaxExclusive = true;

            Required = required;
            MultipleOf = multipleOf;
        }

        public NumberValidator(Number? min, Number? max, bool required)
            : this(min, max, false, false, required, null)
        { }

        public NumberValidator(Number? min, Number? max)
            : this(min, max, false, false, false, null)
        { }

        #endregion

        #region Methods

        public bool IsValid(Number? value)
        {
            // Check Required
            if (!value.HasValue)
                return !(Required); // null + optional = true, null + required = false

            // Check Min
            if (Min.HasValue)
            {
                if (MinExclusive)
                {
                    if (value <= Min) return false;
                }
                else if (value < Min) return false;
            }

            // Check Max
            if (Max.HasValue)
            {
                if (MaxExclusive)
                {
                    if (value >= Max) return false;
                }
                else if (value > Max) return false;
            }

            // MultipleOf
            if (MultipleOf.HasValue
                && MultipleOf.Value != 0 // n % 0 == undefined
                && !value.Value.IsZero) // 0 % n == 0 (we already know value.HasValue is true)
            {
                if ((value.Value.Kind & NumberKinds.Integer) > 0)
                {
                    var val = value.Value.ToInt64();
                    var zero = val % MultipleOf.Value == 0;
                    if (!zero) return false;
                }
                else if ((value.Value.Kind & NumberKinds.Real) > 0)
                {
                    var val = value.Value.ToDouble();
                    var zero = val % MultipleOf.Value == 0.0; // Modulus(Double) is a well-defined operation
                    if (!zero) return false;
                }
            }

            return true;
        }

        public override string ToString()
        {
            // Use set notation for open/closed boundaries

            // Required
            var s = (Required ? "Required: " : string.Empty)

            // Min
            + (MinExclusive ? "(" : "[")
            + (Min.HasValue ? $"{Min.Value}" : "-∞")
            + ", "

            // Max
            + (Max.HasValue ? $"{Max.Value}" : "∞")
            + (MaxExclusive ? ")" : "]")

            // MultipleOf
            + (MultipleOf.HasValue && MultipleOf.Value != 0 ? $" (x{MultipleOf.Value})" : string.Empty);

            return s;
        }

        #endregion
    }
}
