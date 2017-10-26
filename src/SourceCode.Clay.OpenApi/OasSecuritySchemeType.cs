#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

namespace SourceCode.Clay.OpenApi
{
    /// <summary>
    /// Represents the different security scheme types.
    /// </summary>
#pragma warning disable CA1028 // Enum Storage should be Int32

    public enum OasSecuritySchemeType : byte
#pragma warning restore CA1028 // Enum Storage should be Int32
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
