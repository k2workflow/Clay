#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace SourceCode.Clay.OpenApi
{
    /// <summary>
    /// Configuration details for a supported OAuth Flow.
    /// </summary>
    public class OAuthFlowBuilder : IOpenApiBuilder<OAuthFlow>
    {
        #region Properties

        /// <summary>
        /// Gets or sets the authorization URL to be used for this flow.
        /// </summary>
        /// <remarks>
        /// Applies to <see cref="OAuth2SecurityScheme.ImplicitFlow"/> and <see cref="OAuth2SecurityScheme.AuthorizationCodeFlow"/>.
        /// </remarks>
        public Uri AuthorizationUrl { get; set; }

        /// <summary>
        /// Gets or sets the token URL to be used for this flow.
        /// </summary>
        /// <remarks>
        /// Applies to <see cref="OAuth2SecurityScheme.PasswordFlow"/>, <see cref="OAuth2SecurityScheme.ClientCredentialsFlow"/>
        /// and <see cref="OAuth2SecurityScheme.AuthorizationCodeFlow"/>.
        /// </remarks>
        public Uri TokenUrl { get; set; }

        /// <summary>
        /// Gets or sets the URL to be used for obtaining refresh tokens.
        /// </summary>
        public Uri RefreshUrl { get; set; }

        /// <summary>
        /// Get the available scopes for the OAuth2 security scheme.
        /// </summary>
        public IDictionary<String, String> Scopes { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of the <see cref="OAuthFlowBuilder"/> class.
        /// </summary>
        public OAuthFlowBuilder()
        {
            Scopes = new Dictionary<String, String>();
        }

        /// <summary>
        /// Creates a new instance of the <see cref="OAuthFlowBuilder"/> class.
        /// </summary>
        /// <param name="value">The <see cref="OAuthFlow"/> to copy values from.</param>
        public OAuthFlowBuilder(OAuthFlow value)
        {
            if (ReferenceEquals(value, null)) throw new ArgumentNullException(nameof(value));
            AuthorizationUrl = value.AuthorizationUrl;
            TokenUrl = value.TokenUrl;
            RefreshUrl = value.RefreshUrl;
            Scopes = new Dictionary<String, String>(value.Scopes);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Performs an implicit conversion from <see cref="OAuthFlowBuilder"/> to <see cref="OAuthFlow"/>.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator OAuthFlow(OAuthFlowBuilder builder) => builder?.Build();

        /// <summary>
        /// Performs an implicit conversion from <see cref="OAuthFlow"/> to <see cref="OAuthFlowBuilder"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator OAuthFlowBuilder(OAuthFlow value) => ReferenceEquals(value, null) ? null : new OAuthFlowBuilder(value);

        /// <summary>
        /// Creates the <see cref="OAuthFlow"/> from this builder.
        /// </summary>
        /// <returns>The <see cref="OAuthFlow"/>.</returns>
        public OAuthFlow Build() => new OAuthFlow(
            authorizationUrl: AuthorizationUrl,
            tokenUrl: TokenUrl,
            refreshUrl: RefreshUrl,
            scopes: new ReadOnlyDictionary<String, String>(Scopes));

        #endregion
    }
}
