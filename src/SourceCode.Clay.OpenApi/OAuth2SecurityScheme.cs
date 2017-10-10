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
    public class OAuth2SecurityScheme : SecurityScheme, IEquatable<OAuth2SecurityScheme>
    {
        #region Properties

        /// <summary>Gets the type of the security scheme.</summary>
        public override SecuritySchemeType Type => SecuritySchemeType.OAuth2;

        /// <summary>
        /// Gets the configuration for the OAuth Implicit flow.
        /// </summary>
        public OAuthFlow ImplicitFlow { get; }

        /// <summary>
        /// Gets the configuration for the OAuth Password flow.
        /// </summary>
        public OAuthFlow PasswordFlow { get; }

        /// <summary>
        /// Gets the configuration for the OAuth Credentials flow.
        /// </summary>
        public OAuthFlow ClientCredentialsFlow { get; }

        /// <summary>
        /// Gets the configuration for the OAuth Authorization flow.
        /// </summary>
        public OAuthFlow AuthorizationCodeFlow { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of the <see cref="OAuth2SecurityScheme"/> class.
        /// </summary>
        /// <param name="description">The short description for security scheme.</param>
        /// <param name="implicitFlow">The configuration for the OAuth Implicit flow.</param>
        /// <param name="passwordFlow">The configuration for the OAuth Password flow.</param>
        /// <param name="clientCredentialsFlow">The configuration for the OAuth Credentials flow.</param>
        /// <param name="authorizationCodeFlow">The configuration for the OAuth Authorization flow.</param>
        public OAuth2SecurityScheme(
            string description = default,
            OAuthFlow implicitFlow = default,
            OAuthFlow passwordFlow = default,
            OAuthFlow clientCredentialsFlow = default,
            OAuthFlow authorizationCodeFlow = default)
            : base(description)
        {
            ImplicitFlow = implicitFlow;
            PasswordFlow = passwordFlow;
            ClientCredentialsFlow = clientCredentialsFlow;
            AuthorizationCodeFlow = authorizationCodeFlow;
        }

        #endregion

        #region Equatable

        public static bool operator ==(OAuth2SecurityScheme scheme1, OAuth2SecurityScheme scheme2)
        {
            if (ReferenceEquals(scheme1, null) && ReferenceEquals(scheme2, null)) return true;
            if (ReferenceEquals(scheme1, null) || ReferenceEquals(scheme2, null)) return false;
            return scheme1.Equals((object)scheme2);
        }

        public static bool operator !=(OAuth2SecurityScheme scheme1, OAuth2SecurityScheme scheme2)
                    => !(scheme1 == scheme2);

        /// <summary>Determines whether the specified object is equal to the current object.</summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns>true if the specified object  is equal to the current object; otherwise, false.</returns>
        public override bool Equals(object obj) => Equals(obj as OAuth2SecurityScheme);

        /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>true if the current object is equal to the <paramref name="other">other</paramref> parameter; otherwise, false.</returns>
        public bool Equals(OAuth2SecurityScheme other)
        {
            if (!base.Equals(other)) return false;
            if (!ImplicitFlow.NullableEquals(other.ImplicitFlow)) return false;
            if (!PasswordFlow.NullableEquals(other.PasswordFlow)) return false;
            if (!ClientCredentialsFlow.NullableEquals(other.ClientCredentialsFlow)) return false;
            if (!AuthorizationCodeFlow.NullableEquals(other.AuthorizationCodeFlow)) return false;

            return true;
        }

        /// <summary>Serves as the default hash function.</summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                var hc = 17L;

                hc = (hc * 23) + base.GetHashCode();
                if (ImplicitFlow != null)
                    hc = (hc * 23) + ImplicitFlow.GetHashCode();
                if (PasswordFlow != null)
                    hc = (hc * 23) + PasswordFlow.GetHashCode();
                if (ClientCredentialsFlow != null)
                    hc = (hc * 23) + ClientCredentialsFlow.GetHashCode();
                if (AuthorizationCodeFlow != null)
                    hc = (hc * 23) + AuthorizationCodeFlow.GetHashCode();

                return ((int)(hc >> 32)) ^ (int)hc;
            }
        }

        #endregion
    }
}
