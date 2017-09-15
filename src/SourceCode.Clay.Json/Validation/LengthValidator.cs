namespace SourceCode.Clay.Json.Validation
{
    public sealed class LengthValidator
    {
        #region Properties

        public long? Min { get; }

        public long? Max { get; }

        #endregion

        #region Constructors

        public LengthValidator(long? min, long? max)
        {
            Min = min;
            Max = max;
        }

        #endregion

        #region Methods

        public bool IsValid(long value)
        {
            // Check Min
            if (Min.HasValue
                && value < Min.Value)
                return false;

            // Check Max
            if (Max.HasValue
                && value > Max.Value)
                return false;

            return true;
        }

        public override string ToString()
        {
            // Use mathematical notation for open/closed boundaries

            // Min
            var s = "["
            + (Min.HasValue ? $"{Min.Value}" : "-∞")
            + ", "

            // Max
            + (Max.HasValue ? $"{Max.Value}" : "∞")
            + "]";

            return s;
        }

        #endregion
    }
}
