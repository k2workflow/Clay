using System;

namespace SourceCode.Clay.OpenApi
{
    /// <summary>
    /// Defines an HTTP security scheme that can be used by the operations.
    /// </summary>
    public class HttpSecurityScheme : SecurityScheme, IEquatable<HttpSecurityScheme>
    {
        /// <summary>Gets the type of the security scheme.</summary>
        public override SecuritySchemeType Type => SecuritySchemeType.Http;

        /// <summary>
        /// Gets the name of the HTTP Authorization scheme to be used in the Authorization header.
        /// </summary>
        public string Scheme { get; }

        /// <summary>
        /// Gets the hint to the client to identify how the bearer token is formatted.
        /// </summary>
        public string BearerFormat { get; }

        /// <summary>
        /// Creates a new instance of the <see cref="HttpSecurityScheme"/> class.
        /// </summary>
        /// <param name="description">The short description for security scheme.</param>
        /// <param name="scheme">The name of the HTTP Authorization scheme to be used in the Authorization header.</param>
        protected HttpSecurityScheme(
            string description = null,
            string scheme = null,
            string bearerFormat = null) 
            : base(description)
        {
            Scheme = scheme;
            BearerFormat = bearerFormat;
        }

        #region Equatable

        /// <summary>Determines whether the specified object is equal to the current object.</summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns>true if the specified object  is equal to the current object; otherwise, false.</returns>
        public override bool Equals(object obj) => Equals(obj as HttpSecurityScheme);

        /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>true if the current object is equal to the <paramref name="other">other</paramref> parameter; otherwise, false.</returns>
        public bool Equals(HttpSecurityScheme other)
        {
            if (!base.Equals(other)) return false;
            if (!StringComparer.Ordinal.Equals(Scheme, other.Scheme)) return false;
            if (!StringComparer.Ordinal.Equals(BearerFormat, other.BearerFormat)) return false;

            return true;
        }

        /// <summary>Serves as the default hash function.</summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                var hc = 17L;

                hc = hc * 21 + base.GetHashCode();
                if (Scheme != null) hc = hc * 21 + StringComparer.Ordinal.GetHashCode(Scheme);
                if (BearerFormat != null) hc = hc * 21 + StringComparer.Ordinal.GetHashCode(BearerFormat);

                return ((int)(hc >> 32)) ^ (int)hc;
            }
        }

        public static bool operator ==(HttpSecurityScheme scheme1, HttpSecurityScheme scheme2)
        {
            if (ReferenceEquals(scheme1, null) && ReferenceEquals(scheme2, null)) return true;
            if (ReferenceEquals(scheme1, null) || ReferenceEquals(scheme2, null)) return false;
            return scheme1.Equals((object)scheme2);
        }

        public static bool operator !=(HttpSecurityScheme scheme1, HttpSecurityScheme scheme2)
            => !(scheme1 == scheme2);

        #endregion
    }
}
