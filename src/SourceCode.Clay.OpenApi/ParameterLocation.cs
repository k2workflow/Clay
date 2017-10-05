namespace SourceCode.Clay.OpenApi
{
    /// <summary>
    /// Represents the different locations a parameter can occur.
    /// </summary>
    public enum ParameterLocation : byte
    {
        /// <summary>
        /// The parameter occurs in the query.
        /// </summary>
        Query = 0,

        /// <summary>
        /// The parameter occurs in the header.
        /// </summary>
        Header = 1,

        /// <summary>
        /// The parameter occurs in the path.
        /// </summary>
        Path = 2,

        /// <summary>
        /// The parameter occurs in a cookie.
        /// </summary>
        Cookie = 3
    }
}
