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
    public class OasOAuth2SecurityScheme : OasSecurityScheme, IEquatable<OasOAuth2SecurityScheme>
    {
        /// <summary>Gets the type of the security scheme.</summary>
        public override OasSecuritySchemeType SchemeType => OasSecuritySchemeType.OAuth2;

        /// <summary>
        /// Gets the configuration for the OAuth Implicit flow.
        /// </summary>
        public OasOAuthFlow ImplicitFlow { get; }

        /// <summary>
        /// Gets the configuration for the OAuth Password flow.
        /// </summary>
        public OasOAuthFlow PasswordFlow { get; }

        /// <summary>
        /// Gets the configuration for the OAuth Credentials flow.
        /// </summary>
        public OasOAuthFlow ClientCredentialsFlow { get; }

        /// <summary>
        /// Gets the configuration for the OAuth Authorization flow.
        /// </summary>
        public OasOAuthFlow AuthorizationCodeFlow { get; }

        /// <summary>
        /// Creates a new instance of the <see cref="OasOAuth2SecurityScheme"/> class.
        /// </summary>
        /// <param name="description">The short description for security scheme.</param>
        /// <param name="implicitFlow">The configuration for the OAuth Implicit flow.</param>
        /// <param name="passwordFlow">The configuration for the OAuth Password flow.</param>
        /// <param name="clientCredentialsFlow">The configuration for the OAuth Credentials flow.</param>
        /// <param name="authorizationCodeFlow">The configuration for the OAuth Authorization flow.</param>
        public OasOAuth2SecurityScheme(
            string description = default,
            OasOAuthFlow implicitFlow = default,
            OasOAuthFlow passwordFlow = default,
            OasOAuthFlow clientCredentialsFlow = default,
            OasOAuthFlow authorizationCodeFlow = default)
            : base(description)
        {
            ImplicitFlow = implicitFlow;
            PasswordFlow = passwordFlow;
            ClientCredentialsFlow = clientCredentialsFlow;
            AuthorizationCodeFlow = authorizationCodeFlow;
        }

        /// <summary>
        /// Implements the operator == operator.
        /// </summary>
        /// <param name="scheme1">The scheme1.</param>
        /// <param name="scheme2">The scheme2.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator ==(OasOAuth2SecurityScheme scheme1, OasOAuth2SecurityScheme scheme2)
        {
            if (scheme1 is null) return scheme2 is null;
            return scheme1.Equals((object)scheme2);
        }

        /// <summary>
        /// Implements the operator != operator.
        /// </summary>
        /// <param name="scheme1">The scheme1.</param>
        /// <param name="scheme2">The scheme2.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator !=(OasOAuth2SecurityScheme scheme1, OasOAuth2SecurityScheme scheme2)
            => !(scheme1 == scheme2);

        /// <summary>Determines whether the specified object is equal to the current object.</summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns>true if the specified object  is equal to the current object; otherwise, false.</returns>
        public override bool Equals(object obj)
            => obj is OasOAuth2SecurityScheme other
            && Equals(other);

        /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>true if the current object is equal to the <paramref name="other">other</paramref> parameter; otherwise, false.</returns>
        public bool Equals(OasOAuth2SecurityScheme other)
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
        public override int GetHashCode() => HashCode.Combine(
            base.GetHashCode(),
            ImplicitFlow,
            PasswordFlow,
            ClientCredentialsFlow,
            AuthorizationCodeFlow
        );
    }
}
