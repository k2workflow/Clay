using System;

namespace SourceCode.Clay.OpenApi
{
    /// <summary>
    ///   Represents options for a JSON schema.
    /// </summary>
    [Flags]
    public enum SchemaOptions : byte
    {
        /// <summary>
        ///   No options are set.
        /// </summary>
        None = 0,

        /// <summary>
        ///   The contained items must be unique.
        /// </summary>
        UniqueItems = 1,

        /// <summary>
        ///   This item is required.
        /// </summary>
        Required = 2,

        /// <summary>
        ///   This item is nullable.
        /// </summary>
        Nullable = 4,

        /// <summary>
        ///   This item is deprecated.
        /// </summary>
        Deprecated = 8
    }
}
