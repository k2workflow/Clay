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
    public class OasHttpSecurityScheme : OasSecurityScheme, IEquatable<OasHttpSecurityScheme>
    {
        #region Constructors

        /// <summary>
        /// Creates a new instance of the <see cref="OasHttpSecurityScheme"/> class.
        /// </summary>
        /// <param name="description">The short description for security scheme.</param>
        /// <param name="scheme">The name of the HTTP Authorization scheme to be used in the Authorization header.</param>
        /// <param name="bearerFormat">The hint to the client to identify how the bearer token is formatted.</param>
        public OasHttpSecurityScheme(
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
        public override OasSecuritySchemeType SchemeType => OasSecuritySchemeType.Http;

        /// <summary>
        /// Gets the name of the HTTP Authorization scheme to be used in the Authorization header.
        /// </summary>
        public string Scheme { get; }

        /// <summary>
        /// Gets the hint to the client to identify how the bearer token is formatted.
        /// </summary>
        public string BearerFormat { get; }

        #endregion

        #region IEquatable

        /// <summary>
        /// Implements the operator == operator.
        /// </summary>
        /// <param name="scheme1">The scheme1.</param>
        /// <param name="scheme2">The scheme2.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator ==(OasHttpSecurityScheme scheme1, OasHttpSecurityScheme scheme2)
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
        public static bool operator !=(OasHttpSecurityScheme scheme1, OasHttpSecurityScheme scheme2)
            => !(scheme1 == scheme2);

        /// <summary>Determines whether the specified object is equal to the current object.</summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns>true if the specified object  is equal to the current object; otherwise, false.</returns>
        public override bool Equals(object obj)
            => obj is OasHttpSecurityScheme other
            && Equals(other);

        /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>true if the current object is equal to the <paramref name="other">other</paramref> parameter; otherwise, false.</returns>
        public bool Equals(OasHttpSecurityScheme other)
        {
            if (!base.Equals(other)) return false;

            // https://github.com/dotnet/csharplang/issues/39#issuecomment-291602520
            //if (EqualityContract != other.EqualityContract) return false;

            if (!StringComparer.Ordinal.Equals(Scheme, other.Scheme)) return false;
            if (!StringComparer.Ordinal.Equals(BearerFormat, other.BearerFormat)) return false;

            return true;
        }

        /// <summary>Serves as the default hash function.</summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode() => HashCode.Combine(
            base.GetHashCode(),
            StringComparer.Ordinal.GetHashCode(Scheme ?? string.Empty),
            StringComparer.Ordinal.GetHashCode(BearerFormat ?? string.Empty)
        );

        #endregion
    }
}
