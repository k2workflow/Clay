#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using System;

namespace SourceCode.Clay.OpenApi
{
    /// <summary>
    /// Defines an API key security scheme that can be used by the operations.
    /// </summary>
    public class OasOpenIdConnectSecuritySchemeBuilder : OasSecuritySchemeBuilder, IOasBuilder<OasOpenIdConnectSecurityScheme>
    {
        #region Properties

        /// <summary>
        /// Gets the OpenId Connect URL to discover OAuth2 configuration values.
        /// </summary>
        public Uri Url { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of the <see cref="OasOpenIdConnectSecuritySchemeBuilder"/> class.
        /// </summary>
        public OasOpenIdConnectSecuritySchemeBuilder()
        {
        }

        /// <summary>
        /// Creates a new instance of the <see cref="OasOpenIdConnectSecuritySchemeBuilder"/> class.
        /// </summary>
        /// <param name="value">The <see cref="OasOpenIdConnectSecurityScheme"/> to copy values from.</param>
        public OasOpenIdConnectSecuritySchemeBuilder(OasOpenIdConnectSecurityScheme value)
            : base(value)
        {
            if (value is null) throw new ArgumentNullException(nameof(value));
            Url = value.Url;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Performs an implicit conversion from <see cref="OasOpenIdConnectSecuritySchemeBuilder"/> to <see cref="OasOpenIdConnectSecurityScheme"/>.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator OasOpenIdConnectSecurityScheme(OasOpenIdConnectSecuritySchemeBuilder builder) => builder?.Build();

        /// <summary>
        /// Performs an implicit conversion from <see cref="OasOpenIdConnectSecuritySchemeBuilder"/> to <see cref="OasReferable{OpenIdConnectSecurityScheme}"/>.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator OasReferable<OasOpenIdConnectSecurityScheme>(OasOpenIdConnectSecuritySchemeBuilder builder) => builder?.Build();

        /// <summary>
        /// Performs an implicit conversion from <see cref="OasOpenIdConnectSecuritySchemeBuilder"/> to <see cref="OasReferable{SecurityScheme}"/>.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator OasReferable<OasSecurityScheme>(OasOpenIdConnectSecuritySchemeBuilder builder) => builder?.Build();

        /// <summary>
        /// Performs an implicit conversion from <see cref="OasOpenIdConnectSecurityScheme"/> to <see cref="OasOpenIdConnectSecuritySchemeBuilder"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator OasOpenIdConnectSecuritySchemeBuilder(OasOpenIdConnectSecurityScheme value) => value is null ? null : new OasOpenIdConnectSecuritySchemeBuilder(value);

        /// <summary>
        /// Creates the <see cref="OasOpenIdConnectSecurityScheme"/> from this builder.
        /// </summary>
        /// <returns>The <see cref="OasOpenIdConnectSecurityScheme"/>.</returns>
        public OasOpenIdConnectSecurityScheme Build() => new OasOpenIdConnectSecurityScheme(
            description: Description,
            url: Url);

        #endregion
    }
}
