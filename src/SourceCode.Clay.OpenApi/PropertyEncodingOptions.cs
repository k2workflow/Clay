namespace SourceCode.Clay.OpenApi
{
    /// <summary>
    /// Represents the options for <see cref="PropertyEncoding"/>.
    /// </summary>
    public enum PropertyEncodingOptions : byte
    {
        /// <summary>
        /// The default options.
        /// </summary>
        None = 0,

        /// <summary>
        /// Indicates that parameter values of type array or object generate separate parameters for each value of the array or
        /// key-value pair of the map.
        /// </summary>
        Explode = ParameterOptions.Explode,

#       pragma warning disable S4016 // Enumeration members should not be named "Reserved"
        // Reasoning: enum member is not actually called "Reserved".

        /// <summary>
        /// Indicates that the parameter value allows reserved characters.
        /// </summary>
        AllowReserved = ParameterOptions.AllowReserved

#       pragma warning restore S4016 // Enumeration members should not be named "Reserved"
    }
}
