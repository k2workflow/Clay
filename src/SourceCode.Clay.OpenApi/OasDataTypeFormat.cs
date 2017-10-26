#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

namespace SourceCode.Clay.OpenApi
{
    /// <summary>
    /// Well-known data type formats.
    /// </summary>
    public static class OasDataTypeFormat
    {
        #region Classes

        /// <summary>
        /// Standard formats as defined by the specification
        /// https://github.com/OAI/OpenAPI-Specification/blob/master/versions/3.0.0.md#data-types
        /// </summary>
        public static class Standard
        {
            #region Classes

            /// <summary>
            /// Integer formats.
            /// </summary>
            public static class Integral
            {
                #region Constants

                /// <summary>
                /// Signed 32 bits.
                /// </summary>
                public const string Integer32 = "int32";

                /// <summary>
                /// Signed 64 bits.
                /// </summary>
                public const string Integer64 = "int64";

                #endregion
            }

            /// <summary>
            /// Number formats.
            /// </summary>
            public static class Numeric
            {
                #region Constants

                /// <summary>
                /// Real 32 bits.
                /// </summary>
                public const string SinglePrecision = "float";

                /// <summary>
                /// Real 64 bits.
                /// </summary>
                public const string DoublePrecision = "double";

                #endregion
            }

            /// <summary>
            /// String formats.
            /// </summary>
            public static class Text
            {
                #region Constants

                /// <summary>
                /// Base64 encoded characters.
                /// </summary>
                public const string Base64 = "byte";

                /// <summary>
                /// Any sequence of octets.
                /// https://en.wikipedia.org/wiki/Octet_(computing)
                /// </summary>
                public const string BinaryOctet = "binary";

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

                #endregion
            }

            #endregion
        }

        /// <summary>
        /// Formats such as "email", "uuid", and so on, MAY be used even though undefined by this specification.
        /// </summary>
        public static class Extended
        {
            #region Classes

            /// <summary>
            /// String formats.
            /// </summary>
            public static class Text
            {
                #region Constants

                /// <summary>
                /// A timestamp.
                /// </summary>
                public const string Timestamp = "timestamp";

                /// <summary>
                /// A guid.
                /// </summary>
                public const string Uuid = "uuid";

                #endregion
            }

            #endregion
        }

        #endregion
    }
}
