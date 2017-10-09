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
    public class OAuth2SecuritySchemeBuilder : SecuritySchemeBuilder, IBuilder<OAuth2SecurityScheme>
    {
        #region Properties

        /// <summary>
        /// Gets or sets the configuration for the OAuth Implicit flow.
        /// </summary>
        public OAuthFlow ImplicitFlow { get; set; }

        /// <summary>
        /// Gets or sets the configuration for the OAuth Password flow.
        /// </summary>
        public OAuthFlow PasswordFlow { get; set; }

        /// <summary>
        /// Gets or sets the configuration for the OAuth Credentials flow.
        /// </summary>
        public OAuthFlow ClientCredentialsFlow { get; set; }

        /// <summary>
        /// Gets or sets the configuration for the OAuth Authorization flow.
        /// </summary>
        public OAuthFlow AuthorizationCodeFlow { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of the <see cref="OAuth2SecuritySchemeBuilder"/> class.
        /// </summary>
        public OAuth2SecuritySchemeBuilder()
        {
        }

        /// <summary>
        /// Creates a new instance of the <see cref="OAuth2SecuritySchemeBuilder"/> class.
        /// </summary>
        /// <param name="value">The <see cref="OAuth2SecurityScheme"/> to copy values from.</param>
        public OAuth2SecuritySchemeBuilder(OAuth2SecurityScheme value)
            : base(value)
        {
            if (ReferenceEquals(value, null)) throw new ArgumentNullException(nameof(value));
            ImplicitFlow = value.ImplicitFlow;
            PasswordFlow = value.PasswordFlow;
            ClientCredentialsFlow = value.ClientCredentialsFlow;
            AuthorizationCodeFlow = value.AuthorizationCodeFlow;
        }

        #endregion

        #region Methods

        public static implicit operator OAuth2SecurityScheme(OAuth2SecuritySchemeBuilder builder) => builder?.Build();

        public static implicit operator OAuth2SecuritySchemeBuilder(OAuth2SecurityScheme value) => ReferenceEquals(value, null) ? null : new OAuth2SecuritySchemeBuilder(value);

        /// <summary>
        /// Creates the <see cref="OAuth2SecurityScheme"/> from this builder.
        /// </summary>
        /// <returns>The <see cref="OAuth2SecurityScheme"/>.</returns>
        public OAuth2SecurityScheme Build() => new OAuth2SecurityScheme(
            description: Description,
            implicitFlow: ImplicitFlow,
            passwordFlow: PasswordFlow,
            clientCredentialsFlow: ClientCredentialsFlow,
            authorizationCodeFlow: AuthorizationCodeFlow);

        #endregion
    }
}
