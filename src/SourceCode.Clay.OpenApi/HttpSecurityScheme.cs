#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using System;

namespace SourceCode.Clay.OpenApi
{
    /// <summary>
    /// Defines an HTTP security scheme that can be used by the operations.
    /// </summary>
    public class HttpSecurityScheme : SecurityScheme, IEquatable<HttpSecurityScheme>
    {
        #region Constructors

        /// <summary>
        /// Creates a new instance of the <see cref="HttpSecurityScheme"/> class.
        /// </summary>
        /// <param name="description">The short description for security scheme.</param>
        /// <param name="scheme">The name of the HTTP Authorization scheme to be used in the Authorization header.</param>
        /// <param name="bearerFormat">The hint to the client to identify how the bearer token is formatted.</param>
        public HttpSecurityScheme(
            string description = default,
            string scheme = default,
            string bearerFormat = default)
            : base(description)
        {
            Scheme = scheme;
            BearerFormat = bearerFormat;
        }

        #endregion

        #region Properties

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

        #endregion

        #region Equatable

        /// <summary>
        /// Implements the operator == operator.
        /// </summary>
        /// <param name="scheme1">The scheme1.</param>
        /// <param name="scheme2">The scheme2.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator ==(HttpSecurityScheme scheme1, HttpSecurityScheme scheme2)
        {
            if (ReferenceEquals(scheme1, null) && ReferenceEquals(scheme2, null)) return true;
            if (ReferenceEquals(scheme1, null) || ReferenceEquals(scheme2, null)) return false;
            return scheme1.Equals((object)scheme2);
        }

        /// <summary>
        /// Implements the operator != operator.
        /// </summary>
        /// <param name="scheme1">The scheme1.</param>
        /// <param name="scheme2">The scheme2.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator !=(HttpSecurityScheme scheme1, HttpSecurityScheme scheme2)
            => !(scheme1 == scheme2);

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

                hc = (hc * 23) + base.GetHashCode();
                if (Scheme != null)
                    hc = (hc * 23) + StringComparer.Ordinal.GetHashCode(Scheme);
                if (BearerFormat != null)
                    hc = (hc * 23) + StringComparer.Ordinal.GetHashCode(BearerFormat);

                return ((int)(hc >> 32)) ^ (int)hc;
            }
        }

        #endregion
    }
}
