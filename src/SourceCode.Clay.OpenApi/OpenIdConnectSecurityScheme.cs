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
    public class OpenIdConnectSecurityScheme : SecurityScheme, IEquatable<OpenIdConnectSecurityScheme>
    {
        #region Properties

        /// <summary>Gets the type of the security scheme.</summary>
        public override SecuritySchemeType Type => SecuritySchemeType.OpenIdConnect;

        /// <summary>
        /// Gets the OpenId Connect URL to discover OAuth2 configuration values.
        /// </summary>
        public Uri Url { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of the <see cref="OpenIdConnectSecurityScheme"/>.
        /// </summary>
        /// <param name="description">The short description for security scheme.</param>
        /// <param name="url">The OpenId Connect URL to discover OAuth2 configuration values.</param>
        public OpenIdConnectSecurityScheme(
            string description = default,
            Uri url = default)
            : base(description)
        {
            Url = url;
        }

        #endregion

        #region Equatable

        /// <summary>
        /// Implements the operator == operator.
        /// </summary>
        /// <param name="scheme1">The scheme1.</param>
        /// <param name="scheme2">The scheme2.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator ==(OpenIdConnectSecurityScheme scheme1, OpenIdConnectSecurityScheme scheme2)
        {
            if (scheme1 is null && scheme2 is null) return true;
            if (scheme1 is null || scheme2 is null) return false;
            return scheme1.Equals((object)scheme2);
        }

        /// <summary>
        /// Implements the operator != operator.
        /// </summary>
        /// <param name="scheme1">The scheme1.</param>
        /// <param name="scheme2">The scheme2.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator !=(OpenIdConnectSecurityScheme scheme1, OpenIdConnectSecurityScheme scheme2)
            => !(scheme1 == scheme2);

        /// <summary>Determines whether the specified object is equal to the current object.</summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns>true if the specified object  is equal to the current object; otherwise, false.</returns>
        public override bool Equals(object obj)
            => obj is OpenIdConnectSecurityScheme other
            && Equals(other);

        /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>true if the current object is equal to the <paramref name="other">other</paramref> parameter; otherwise, false.</returns>
        public bool Equals(OpenIdConnectSecurityScheme other)
        {
            if (!base.Equals(other)) return false;
            if (Url != other.Url) return false;

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
                if (Url != null)
                    hc = (hc * 23) + Url.GetHashCode();

                return ((int)(hc >> 32)) ^ (int)hc;
            }
        }

        #endregion
    }
}
