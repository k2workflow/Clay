namespace SourceCode.Clay.OpenApi
{
    /// <summary>
    /// Represents options for a <see cref="Operation"/>.
    /// </summary>
    public enum OperationOptions : byte
    {
        /// <summary>
        /// No options are set.
        /// </summary>
        None = 0,
        /// <summary>
        /// The operation is deprecated.
        /// </summary>
        Deprecated = 1
    }
}
