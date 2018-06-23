#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using SourceCode.Clay.Collections.Generic;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace SourceCode.Clay.OpenApi
{
    /// <summary>
    /// Configuration details for a supported OAuth Flow.
    /// </summary>
    public class OasOAuthFlow : IEquatable<OasOAuthFlow>
    {
        /// <summary>
        /// Gets the authorization URL to be used for this flow.
        /// </summary>
        /// <remarks>
        /// Applies to <see cref="OasOAuth2SecurityScheme.ImplicitFlow"/> and <see cref="OasOAuth2SecurityScheme.AuthorizationCodeFlow"/>.
        /// </remarks>
        public Uri AuthorizationUrl { get; }

        /// <summary>
        /// Gets the token URL to be used for this flow.
        /// </summary>
        /// <remarks>
        /// Applies to <see cref="OasOAuth2SecurityScheme.PasswordFlow"/>, <see cref="OasOAuth2SecurityScheme.ClientCredentialsFlow"/>
        /// and <see cref="OasOAuth2SecurityScheme.AuthorizationCodeFlow"/>.
        /// </remarks>
        public Uri TokenUrl { get; }

        /// <summary>
        /// Gets the URL to be used for obtaining refresh tokens.
        /// </summary>
        public Uri RefreshUrl { get; }

        /// <summary>
        /// Get the available scopes for the OAuth2 security scheme.
        /// </summary>
        public IReadOnlyDictionary<string, string> Scopes { get; }

        /// <summary>
        /// Creates a new instance of the <see cref="OasOAuthFlow"/> class.
        /// </summary>
        /// <param name="authorizationUrl">The authorization URL to be used for this flow.</param>
        /// <param name="tokenUrl">The token URL to be used for this flow.</param>
        /// <param name="refreshUrl">The URL to be used for obtaining refresh tokens.</param>
        /// <param name="scopes">The available scopes for the OAuth2 security scheme.</param>
        public OasOAuthFlow(
            Uri authorizationUrl = default,
            Uri tokenUrl = default,
            Uri refreshUrl = default,
            IReadOnlyDictionary<string, string> scopes = default)
        {
            AuthorizationUrl = authorizationUrl;
            TokenUrl = tokenUrl;
            RefreshUrl = refreshUrl;
            Scopes = scopes ?? ImmutableDictionary<string, string>.Empty;
        }

        /// <summary>
        /// Implements the operator == operator.
        /// </summary>
        /// <param name="flow1">The flow1.</param>
        /// <param name="flow2">The flow2.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator ==(OasOAuthFlow flow1, OasOAuthFlow flow2)
        {
            if (flow1 is null) return flow2 is null;
            return flow1.Equals((object)flow2);
        }

        /// <summary>
        /// Implements the operator != operator.
        /// </summary>
        /// <param name="flow1">The flow1.</param>
        /// <param name="flow2">The flow2.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator !=(OasOAuthFlow flow1, OasOAuthFlow flow2)
            => !(flow1 == flow2);

        /// <summary>Determines whether the specified object is equal to the current object.</summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns>true if the specified object  is equal to the current object; otherwise, false.</returns>
        public override bool Equals(object obj)
            => obj is OasOAuthFlow other
            && Equals(other);

        /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>true if the current object is equal to the <paramref name="other">other</paramref> parameter; otherwise, false.</returns>
        public bool Equals(OasOAuthFlow other)
        {
            if (ReferenceEquals(this, other)) return true;
            if (other is null) return false;

            if (AuthorizationUrl != other.AuthorizationUrl) return false;
            if (TokenUrl != other.TokenUrl) return false;
            if (RefreshUrl != other.RefreshUrl) return false;
            if (!Scopes.NullableDictionaryEqual(other.Scopes)) return false;

            return true;
        }

        /// <summary>Serves as the default hash function.</summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode() => HashCode.Combine(
            AuthorizationUrl,
            TokenUrl,
            RefreshUrl,
            Scopes.Count
        );
    }
}
