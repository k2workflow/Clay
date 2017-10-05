using System;

namespace SourceCode.Clay.OpenApi
{
    /// <summary>
    /// Defines an API key security scheme that can be used by the operations.
    /// </summary>
    public class OpenIdConnectSecurityScheme : SecurityScheme, IEquatable<OpenIdConnectSecurityScheme>
    {
        /// <summary>Gets the type of the security scheme.</summary>
        public override SecuritySchemeType Type => SecuritySchemeType.OpenIdConnect;

        /// <summary>
        /// Gets the OpenId Connect URL to discover OAuth2 configuration values.
        /// </summary>
        public Uri Url { get; }

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

        #region Equatable

        /// <summary>Determines whether the specified object is equal to the current object.</summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns>true if the specified object  is equal to the current object; otherwise, false.</returns>
        public override bool Equals(object obj) => Equals(obj as OpenIdConnectSecurityScheme);

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

                hc = hc * -1521134295 + base.GetHashCode();
                if (Url != null) hc = hc * -1521134295 + Url.GetHashCode();

                return ((int)(hc >> 32)) ^ (int)hc;
            }
        }

        public static bool operator ==(OpenIdConnectSecurityScheme scheme1, OpenIdConnectSecurityScheme scheme2)
        {
            if (ReferenceEquals(scheme1, null) && ReferenceEquals(scheme2, null)) return true;
            if (ReferenceEquals(scheme1, null) || ReferenceEquals(scheme2, null)) return false;
            return scheme1.Equals((object)scheme2);
        }

        public static bool operator !=(OpenIdConnectSecurityScheme scheme1, OpenIdConnectSecurityScheme scheme2)
            => !(scheme1 == scheme2); 

        #endregion
    }
}
