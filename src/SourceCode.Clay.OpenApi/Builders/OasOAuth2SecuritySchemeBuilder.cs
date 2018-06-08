#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using System;

namespace SourceCode.Clay.OpenApi
{
    /// <summary>
    /// Defines an OAuth 2 security scheme that can be used by the operations.
    /// </summary>
    public class OasOAuth2SecuritySchemeBuilder : OasSecuritySchemeBuilder, IOasBuilder<OasOAuth2SecurityScheme>
    {
        /// <summary>
        /// Gets or sets the configuration for the OAuth Implicit flow.
        /// </summary>
        public OasOAuthFlow ImplicitFlow { get; set; }

        /// <summary>
        /// Gets or sets the configuration for the OAuth Password flow.
        /// </summary>
        public OasOAuthFlow PasswordFlow { get; set; }

        /// <summary>
        /// Gets or sets the configuration for the OAuth Credentials flow.
        /// </summary>
        public OasOAuthFlow ClientCredentialsFlow { get; set; }

        /// <summary>
        /// Gets or sets the configuration for the OAuth Authorization flow.
        /// </summary>
        public OasOAuthFlow AuthorizationCodeFlow { get; set; }

        /// <summary>Gets the security scheme type.</summary>
        public override OasSecuritySchemeType SchemeType => OasSecuritySchemeType.OAuth2;

        /// <summary>
        /// Creates a new instance of the <see cref="OasOAuth2SecuritySchemeBuilder"/> class.
        /// </summary>
        public OasOAuth2SecuritySchemeBuilder()
        {
        }

        /// <summary>
        /// Creates a new instance of the <see cref="OasOAuth2SecuritySchemeBuilder"/> class.
        /// </summary>
        /// <param name="value">The <see cref="OasOAuth2SecurityScheme"/> to copy values from.</param>
        public OasOAuth2SecuritySchemeBuilder(OasOAuth2SecurityScheme value)
            : base(value)
        {
            if (value is null) throw new ArgumentNullException(nameof(value));
            ImplicitFlow = value.ImplicitFlow;
            PasswordFlow = value.PasswordFlow;
            ClientCredentialsFlow = value.ClientCredentialsFlow;
            AuthorizationCodeFlow = value.AuthorizationCodeFlow;
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="OasOAuth2SecuritySchemeBuilder"/> to <see cref="OasOAuth2SecurityScheme"/>.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator OasOAuth2SecurityScheme(OasOAuth2SecuritySchemeBuilder builder) => (OasOAuth2SecurityScheme)builder?.Build();

        /// <summary>
        /// Performs an implicit conversion from <see cref="OasOAuth2SecuritySchemeBuilder"/> to <see cref="OasReferable{OAuth2SecurityScheme}"/>.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator OasReferable<OasOAuth2SecurityScheme>(OasOAuth2SecuritySchemeBuilder builder) => (OasOAuth2SecurityScheme)builder?.Build();

        /// <summary>
        /// Performs an implicit conversion from <see cref="OasOAuth2SecurityScheme"/> to <see cref="OasOAuth2SecuritySchemeBuilder"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator OasOAuth2SecuritySchemeBuilder(OasOAuth2SecurityScheme value) => value is null ? null : new OasOAuth2SecuritySchemeBuilder(value);

        /// <summary>
        /// Creates the <see cref="OasOAuth2SecurityScheme"/> from this builder.
        /// </summary>
        /// <returns>The <see cref="OasOAuth2SecurityScheme"/>.</returns>
        public override OasSecurityScheme Build() => new OasOAuth2SecurityScheme(
            description: Description,
            implicitFlow: ImplicitFlow,
            passwordFlow: PasswordFlow,
            clientCredentialsFlow: ClientCredentialsFlow,
            authorizationCodeFlow: AuthorizationCodeFlow);

        /// <summary>
        /// Creates the <see cref="OasOAuth2SecurityScheme"/> from this builder.
        /// </summary>
        /// <returns>The <see cref="OasOAuth2SecurityScheme"/>.</returns>
        OasOAuth2SecurityScheme IOasBuilder<OasOAuth2SecurityScheme>.Build() => (OasOAuth2SecurityScheme)Build();
    }
}
