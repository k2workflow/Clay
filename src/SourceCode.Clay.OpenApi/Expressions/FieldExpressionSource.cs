namespace SourceCode.Clay.OpenApi.Expressions
{
    /// <summary>
    /// Represents expression sources.
    /// </summary>
    public enum FieldExpressionSource : byte
    {
        /// <summary>
        /// The expression is simple and has no source parameter.
        /// </summary>
        None = 0,
        /// <summary>
        /// The expression extracts a value from the header.
        /// </summary>
        Header = 1,
        /// <summary>
        /// The expression extracts a value from the query.
        /// </summary>
        Query = 2,
        /// <summary>
        /// The expression extracts a value from the path.
        /// </summary>
        Path = 3,
        /// <summary>
        /// The expression extracts a value from the body.
        /// </summary>
        Body = 4
    }
}
