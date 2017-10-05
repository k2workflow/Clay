namespace SourceCode.Clay.OpenApi.Expressions
{
    /// <summary>
    /// Represents the different type of expression components.
    /// </summary>
    public enum ExpressionComponentType : byte
    {
        /// <summary>
        /// A literal value.
        /// </summary>
        Literal = 0,
        /// <summary>
        /// A field getter.
        /// </summary>
        Field = 1
    }
}
