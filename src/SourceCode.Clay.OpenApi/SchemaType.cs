namespace SourceCode.Clay.OpenApi
{
    /// <summary>
    /// Represents the different JSON schema types.
    /// </summary>
    public enum SchemaType : byte
    {
        /// <summary>
        /// The type is a reference.
        /// </summary>
        Reference = 0,

        /// <summary>
        /// The type is a string.
        /// </summary>
        String = 1,

        /// <summary>
        /// The type is a number.
        /// </summary>
        Number = 2,

        /// <summary>
        /// The type is an object.
        /// </summary>
        Object = 3,

        /// <summary>
        /// The type is an array.
        /// </summary>
        Array = 4,

        /// <summary>
        /// The type is a boolean.
        /// </summary>
        Boolean = 5,

        /// <summary>
        /// The type is an integer.
        /// </summary>
        Integer = 6
    }
}
