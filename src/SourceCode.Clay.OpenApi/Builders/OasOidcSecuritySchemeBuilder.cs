#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using System;

namespace SourceCode.Clay.OpenApi
{
    /// <summary>
    /// Defines an OpenId Connect API key security scheme that can be used by the operations.
    /// </summary>
    public class OasOidcSecuritySchemeBuilder : OasSecuritySchemeBuilder, IOasBuilder<OasOidcSecurityScheme>
    {
        #region Properties

        /// <summary>
        /// Gets the OpenId Connect URL to discover OAuth2 configuration values.
        /// </summary>
        public Uri Url { get; set; }

        /// <summary>Gets the security scheme type.</summary>
        public override OasSecuritySchemeType SchemeType => OasSecuritySchemeType.OpenIdConnect;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of the <see cref="OasOidcSecuritySchemeBuilder"/> class.
        /// </summary>
        public OasOidcSecuritySchemeBuilder()
        {
        }

        /// <summary>
        /// Creates a new instance of the <see cref="OasOidcSecuritySchemeBuilder"/> class.
        /// </summary>
        /// <param name="value">The <see cref="OasOidcSecurityScheme"/> to copy values from.</param>
        public OasOidcSecuritySchemeBuilder(OasOidcSecurityScheme value)
            : base(value)
        {
            if (value is null) throw new ArgumentNullException(nameof(value));
            Url = value.Url;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Performs an implicit conversion from <see cref="OasOidcSecuritySchemeBuilder"/> to <see cref="OasOidcSecurityScheme"/>.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator OasOidcSecurityScheme(OasOidcSecuritySchemeBuilder builder) => (OasOidcSecurityScheme)builder?.Build();

        /// <summary>
        /// Performs an implicit conversion from <see cref="OasOidcSecuritySchemeBuilder"/> to <see cref="OasReferable{OpenIdConnectSecurityScheme}"/>.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator OasReferable<OasOidcSecurityScheme>(OasOidcSecuritySchemeBuilder builder) => (OasOidcSecurityScheme)builder?.Build();

        /// <summary>
        /// Performs an implicit conversion from <see cref="OasOidcSecurityScheme"/> to <see cref="OasOidcSecuritySchemeBuilder"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator OasOidcSecuritySchemeBuilder(OasOidcSecurityScheme value) => value is null ? null : new OasOidcSecuritySchemeBuilder(value);

        /// <summary>
        /// Creates the <see cref="OasOidcSecurityScheme"/> from this builder.
        /// </summary>
        /// <returns>The <see cref="OasOidcSecurityScheme"/>.</returns>
        public override OasSecurityScheme Build() => new OasOidcSecurityScheme(
            description: Description,
            url: Url);

        /// <summary>
        /// Creates the <see cref="OasOidcSecurityScheme"/> from this builder.
        /// </summary>
        /// <returns>The <see cref="OasOidcSecurityScheme"/>.</returns>
        OasOidcSecurityScheme IOasBuilder<OasOidcSecurityScheme>.Build() => (OasOidcSecurityScheme)Build();

        #endregion
    }
}
