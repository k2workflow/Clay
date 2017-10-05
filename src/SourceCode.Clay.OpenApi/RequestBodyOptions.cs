namespace SourceCode.Clay.OpenApi
{
    /// <summary>
    /// Represents options for <see cref="RequestBody"/>.
    /// </summary>
    public enum RequestBodyOptions : byte
    {
        /// <summary>
        /// The default options.
        /// </summary>
        None = 0,

        /// <summary>
        /// The the request body is required in the request.
        /// </summary>
        Required = 1
    }
}
