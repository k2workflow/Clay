namespace SourceCode.Clay.OpenApi
{
    /// <summary>
    /// Represents the different security scheme types.
    /// </summary>
    public enum SecuritySchemeType : byte
    {
        /// <summary>
        /// The HTTP authorization security scheme.
        /// </summary>
        Http = 0,

        /// <summary>
        /// The API key authorization scheme.
        /// </summary>
        ApiKey = 1,

        /// <summary>
        /// The OAuth 2 authorization scheme.
        /// </summary>
        OAuth2 = 2,

        /// <summary>
        /// The OpenID Connect authorization scheme.
        /// </summary>
        OpenIdConnect = 3
    }
}
