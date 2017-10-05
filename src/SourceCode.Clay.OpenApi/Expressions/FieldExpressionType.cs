namespace SourceCode.Clay.OpenApi.Expressions
{
    /// <summary>
    ///   Represents the different expression types.
    /// </summary>
    public enum FieldExpressionType : byte
    {
        /// <summary>
        ///   The expression retrieves a value from a URL.
        /// </summary>
        Url = 0,

        /// <summary>
        ///   The expression retrieves a value from the method.
        /// </summary>
        Method = 1,

        /// <summary>
        ///   The expression retrieves a value from the status code.
        /// </summary>
        StatusCode = 2,

        /// <summary>
        ///   The expression retrieves a value from the request.
        /// </summary>
        Request = 3,

        /// <summary>
        ///   The expression retrieves a value from the response.
        /// </summary>
        Response = 4
    }
}
