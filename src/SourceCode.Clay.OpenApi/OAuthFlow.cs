#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using SourceCode.Clay.Collections.Generic;
using System;
using System.Collections.Generic;

namespace SourceCode.Clay.OpenApi
{
    /// <summary>
    /// Configuration details for a supported OAuth Flow.
    /// </summary>
    public class OAuthFlow : IEquatable<OAuthFlow>
    {
        #region Properties

        /// <summary>
        /// Gets the authorization URL to be used for this flow.
        /// </summary>
        /// <remarks>
        /// Applies to <see cref="OAuth2SecurityScheme.ImplicitFlow"/> and <see cref="OAuth2SecurityScheme.AuthorizationCodeFlow"/>.
        /// </remarks>
        public Uri AuthorizationUrl { get; }

        /// <summary>
        /// Gets the token URL to be used for this flow.
        /// </summary>
        /// <remarks>
        /// Applies to <see cref="OAuth2SecurityScheme.PasswordFlow"/>, <see cref="OAuth2SecurityScheme.ClientCredentialsFlow"/>
        /// and <see cref="OAuth2SecurityScheme.AuthorizationCodeFlow"/>.
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

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of the <see cref="OAuthFlow"/> class.
        /// </summary>
        /// <param name="authorizationUrl">The authorization URL to be used for this flow.</param>
        /// <param name="tokenUrl">The token URL to be used for this flow.</param>
        /// <param name="refreshUrl">The URL to be used for obtaining refresh tokens.</param>
        /// <param name="scopes">The available scopes for the OAuth2 security scheme.</param>
        public OAuthFlow(
            Uri authorizationUrl = default,
            Uri tokenUrl = default,
            Uri refreshUrl = default,
            IReadOnlyDictionary<string, string> scopes = default)
        {
            AuthorizationUrl = authorizationUrl;
            TokenUrl = tokenUrl;
            RefreshUrl = refreshUrl;
            Scopes = scopes ?? ReadOnlyDictionary.Empty<string, string>();
        }

        #endregion

        #region Equatable

        public static bool operator ==(OAuthFlow flow1, OAuthFlow flow2)
        {
            if (ReferenceEquals(flow1, null) && ReferenceEquals(flow2, null)) return true;
            if (ReferenceEquals(flow1, null) || ReferenceEquals(flow2, null)) return false;
            return flow1.Equals((object)flow2);
        }

        public static bool operator !=(OAuthFlow flow1, OAuthFlow flow2) =>
                    !(flow1 == flow2);

        /// <summary>Determines whether the specified object is equal to the current object.</summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns>true if the specified object  is equal to the current object; otherwise, false.</returns>
        public override bool Equals(object obj) => Equals(obj as OAuthFlow);

        /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>true if the current object is equal to the <paramref name="other">other</paramref> parameter; otherwise, false.</returns>
        public bool Equals(OAuthFlow other)
        {
            if (ReferenceEquals(this, other)) return true;
            if (ReferenceEquals(other, null)) return false;

            if (AuthorizationUrl != other.AuthorizationUrl) return false;
            if (TokenUrl != other.TokenUrl) return false;
            if (RefreshUrl != other.RefreshUrl) return false;
            if (!Scopes.DictionaryEquals(other.Scopes)) return false;

            return true;
        }

        /// <summary>Serves as the default hash function.</summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                var hc = 17L;

                if (AuthorizationUrl != null)
                    hc = (hc * 23) + AuthorizationUrl.GetHashCode();
                if (TokenUrl != null)
                    hc = (hc * 23) + TokenUrl.GetHashCode();
                if (RefreshUrl != null)
                    hc = (hc * 23) + RefreshUrl.GetHashCode();
                hc = (hc * 23) + Scopes.Count;

                return ((int)(hc >> 32)) ^ (int)hc;
            }
        }

        #endregion
    }
}
