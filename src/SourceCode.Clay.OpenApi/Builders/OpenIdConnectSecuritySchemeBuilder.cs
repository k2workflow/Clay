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
    public class OpenIdConnectSecuritySchemeBuilder : SecuritySchemeBuilder, IOpenApiBuilder<OpenIdConnectSecurityScheme>
    {
        #region Properties

        /// <summary>
        /// Gets the OpenId Connect URL to discover OAuth2 configuration values.
        /// </summary>
        public Uri Url { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of the <see cref="OpenIdConnectSecuritySchemeBuilder"/> class.
        /// </summary>
        public OpenIdConnectSecuritySchemeBuilder()
        {
        }

        /// <summary>
        /// Creates a new instance of the <see cref="OpenIdConnectSecuritySchemeBuilder"/> class.
        /// </summary>
        /// <param name="value">The <see cref="OpenIdConnectSecurityScheme"/> to copy values from.</param>
        public OpenIdConnectSecuritySchemeBuilder(OpenIdConnectSecurityScheme value)
            : base(value)
        {
            if (ReferenceEquals(value, null)) throw new ArgumentNullException(nameof(value));
            Url = value.Url;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Performs an implicit conversion from <see cref="OpenIdConnectSecuritySchemeBuilder"/> to <see cref="OpenIdConnectSecurityScheme"/>.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator OpenIdConnectSecurityScheme(OpenIdConnectSecuritySchemeBuilder builder) => builder?.Build();

        /// <summary>
        /// Performs an implicit conversion from <see cref="OpenIdConnectSecuritySchemeBuilder"/> to <see cref="Referable{OpenIdConnectSecurityScheme}"/>.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator Referable<OpenIdConnectSecurityScheme>(OpenIdConnectSecuritySchemeBuilder builder) => builder?.Build();

        /// <summary>
        /// Performs an implicit conversion from <see cref="OpenIdConnectSecuritySchemeBuilder"/> to <see cref="Referable{SecurityScheme}"/>.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator Referable<SecurityScheme>(OpenIdConnectSecuritySchemeBuilder builder) => builder?.Build();

        /// <summary>
        /// Performs an implicit conversion from <see cref="OpenIdConnectSecurityScheme"/> to <see cref="OpenIdConnectSecuritySchemeBuilder"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator OpenIdConnectSecuritySchemeBuilder(OpenIdConnectSecurityScheme value) => ReferenceEquals(value, null) ? null : new OpenIdConnectSecuritySchemeBuilder(value);

        /// <summary>
        /// Creates the <see cref="OpenIdConnectSecurityScheme"/> from this builder.
        /// </summary>
        /// <returns>The <see cref="OpenIdConnectSecurityScheme"/>.</returns>
        public OpenIdConnectSecurityScheme Build() => new OpenIdConnectSecurityScheme(
            description: Description,
            url: Url);

        #endregion
    }
}
