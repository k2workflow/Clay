using System;

namespace SourceCode.Clay.Json.Validation
{
    public sealed class NumberValidator
    {
        #region Properties

        public Number Max { get; }
        public bool MaxExclusive { get; }
        public Number Min { get; }
        public bool MinExclusive { get; }
        public long? MultipleOf { get; }
        public bool Required { get; }

        #endregion Properties

        #region Constructors

        public NumberValidator(Number min, Number max, bool minExclusive, bool maxExclusive, bool required, long? multipleOf)
        {
            // Ensure min and max are of the same general type (integer or real)
            if (((min.Kind & NumberKind.Integer) > 0 && (max.Kind & NumberKind.Real) > 0)
            || ((min.Kind & NumberKind.Real) > 0 && (max.Kind & NumberKind.Integer) > 0))
                throw new ArgumentOutOfRangeException(nameof(max), $"{nameof(NumberValidator)} {nameof(min)} and {nameof(max)} should have the same {nameof(NumberKind)}");

            Min = min;
            if (Min.HasValue && minExclusive)
                MinExclusive = true;

            Max = max;
            if (Max.HasValue && maxExclusive)
                MaxExclusive = true;

            Required = required;
            MultipleOf = multipleOf;
        }

        public NumberValidator(Number min, Number max, bool required)
            : this(min, max, false, false, required, null)
        { }

        public NumberValidator(Number min, Number max)
            : this(min, max, false, false, false, null)
        { }

        #endregion Constructors

        #region Methods

        public bool IsValid(Number value)
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
                && !value.IsZero) // 0 % n == 0 (we already know value.HasValue is true)
            {
                if ((value.Kind & NumberKind.Integer) > 0)
                {
                    var val = value.ToInt64();
                    var zero = val % MultipleOf.Value == 0;
                    if (!zero) return false;
                }
                else if ((value.Kind & NumberKind.Real) > 0)
                {
                    var val = value.ToDouble();
                    var zero = val % MultipleOf.Value == 0.0; // Modulus(Double) is a well-defined operation
                    if (!zero) return false;
                }
            }

            return true;
        }

        public override string ToString()
        {
            // Use mathematical notation for open/closed boundaries

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

        #endregion Methods
    }
}
