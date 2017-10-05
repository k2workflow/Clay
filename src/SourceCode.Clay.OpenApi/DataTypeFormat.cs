namespace SourceCode.Clay.OpenApi
{
    /// <summary>
    ///   Well-known data type formats.
    /// </summary>
    public static class DataTypeFormat
    {
        #region Classes

        /// <summary>
        ///   Formats such as "email", "uuid", and so on, MAY be used even though undefined by this specification.
        /// </summary>
        public static class Extended
        {
            #region Classes

            public static class String
            {
                #region Fields

                /// <summary>
                ///   A GUID.
                /// </summary>
                public const string Guid = "uuid";

                /// <summary>
                ///   A timestamp.
                /// </summary>
                public const string TimeStamp = "timestamp";

                #endregion Fields
            }

            #endregion Classes
        }

        /// <summary>
        ///   Standard formats as defined by the specification https://github.com/OAI/OpenAPI-Specification/blob/master/versions/3.0.0.md#data-types
        /// </summary>
        public static class Standard
        {
            #region Classes

            public static class Integer
            {
                #region Fields

                /// <summary>
                ///   Signed 32 bits.
                /// </summary>
                public const string Int32 = "int32";

                /// <summary>
                ///   Signed 64 bits.
                /// </summary>
                public const string Int64 = "int64";

                #endregion Fields
            }

            public static class Number
            {
                #region Fields

                /// <summary>
                ///   Real 64 bits.
                /// </summary>
                public const string Double = "double";

                /// <summary>
                ///   Real 32 bits.
                /// </summary>
                public const string Single = "float";

                #endregion Fields
            }

            public static class String
            {
                #region Fields

                /// <summary>
                ///   Any sequence of octets. https://en.wikipedia.org/wiki/Octet_(computing)
                /// </summary>
                public const string Binary = "binary";

                /// <summary>
                ///   Base64 encoded characters.
                /// </summary>
                public const string Byte = "byte";

                /// <summary>
                ///   As defined by full-date. https://xml2rfc.tools.ietf.org/public/rfc/html/rfc3339.html#anchor14
                /// </summary>
                public const string Date = "date";

                /// <summary>
                ///   As defined by date-time. https://xml2rfc.tools.ietf.org/public/rfc/html/rfc3339.html#anchor14
                /// </summary>
                public const string DateTime = "date-time";

                /// <summary>
                ///   A hint to UIs to obscured input.
                /// </summary>
                public const string Password = "pass" + "word";

                #endregion Fields

                // FxCop complains
            }

            #endregion Classes
        }

        #endregion Classes
    }
}
