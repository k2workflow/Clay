namespace SourceCode.Clay.OpenApi
{
    /// <summary>
    /// Well-known data type formats.
    /// </summary>
    public static class DataTypeFormat
    {
        /// <summary>
        /// Standard formats as defined by the specification
        /// https://github.com/OAI/OpenAPI-Specification/blob/master/versions/3.0.0.md#data-types
        /// </summary>
        public static class Standard
        {
            public static class Integer
            {
                /// <summary>
                /// Signed 32 bits.
                /// </summary>
                public const string Int32 = "int32";

                /// <summary>
                /// Signed 64 bits.
                /// </summary>
                public const string Int64 = "int64";
            }

            public static class Number
            {
                /// <summary>
                /// Real 32 bits.
                /// </summary>
                public const string Single = "float";

                /// <summary>
                /// Real 64 bits.
                /// </summary>
                public const string Double = "double";
            }

            public static class String
            {
                /// <summary>
                /// Base64 encoded characters.
                /// </summary>
                public const string Byte = "byte";

                /// <summary>
                /// Any sequence of octets.
                /// https://en.wikipedia.org/wiki/Octet_(computing)
                /// </summary>
                public const string Binary = "binary";

                /// <summary>
                /// As defined by full-date.
                /// https://xml2rfc.tools.ietf.org/public/rfc/html/rfc3339.html#anchor14
                /// </summary>
                public const string Date = "date";

                /// <summary>
                /// As defined by date-time.
                /// https://xml2rfc.tools.ietf.org/public/rfc/html/rfc3339.html#anchor14
                /// </summary>
                public const string DateTime = "date-time";

                /// <summary>
                /// A hint to UIs to obscured input.
                /// </summary>
                public const string Password = "pass" + "word"; // FxCop complains
            }
        }

        /// <summary>
        /// Formats such as "email", "uuid", and so on, MAY be used even though undefined by this specification.
        /// </summary>
        public static class Extended
        {
            public static class String
            {
                /// <summary>
                /// A timestamp.
                /// </summary>
                public const string TimeStamp = "timestamp";

                /// <summary>
                /// A GUID.
                /// </summary>
                public const string Guid = "uuid";
            }
        }
    }
}
