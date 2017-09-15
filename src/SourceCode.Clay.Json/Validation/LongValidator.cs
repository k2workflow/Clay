namespace SourceCode.Clay.Json.Validation
{
    public sealed class LongValidator
    {
        #region Properties

        public long? Min { get; }

        public long? Max { get; }

        public bool MinExclusive { get; }

        public bool MaxExclusive { get; }

        public bool Required { get; }

        public long? MultipleOf { get; }

        #endregion

        #region Constructors

        public LongValidator(long? min, long? max, bool minExclusive, bool maxExclusive, bool required, long? multipleOf)
        {
            Min = min;
            if (Min.HasValue && minExclusive)
                MinExclusive = true;

            Max = max;
            if (Max.HasValue && maxExclusive)
                MaxExclusive = true;

            Required = required;
            MultipleOf = multipleOf;
        }

        public LongValidator(long? min, long? max, bool required)
            : this(min, max, false, false, required, null)
        { }

        public LongValidator(long? min, long? max)
            : this(min, max, false, false, false, null)
        { }

        #endregion

        #region Methods

        public bool IsValid(long? value)
        {
            // Check Required
            if (!value.HasValue)
                return !(Required); // null + optional = true, null + required = false

            // Check Min
            if (Min.HasValue)
            {
                if (MinExclusive)
                {
                    if (value.Value <= Min.Value) return false;
                }
                else if (value.Value < Min.Value) return false;
            }

            // Check Max
            if (Max.HasValue)
            {
                if (MaxExclusive)
                {
                    if (value.Value >= Max.Value) return false;
                }
                else if (value.Value > Max.Value) return false;
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

        #endregion
    }
}
