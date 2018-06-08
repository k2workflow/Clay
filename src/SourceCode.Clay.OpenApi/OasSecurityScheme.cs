#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using System;

namespace SourceCode.Clay.OpenApi
{
    /// <summary>
    /// Defines a security scheme that can be used by the operations.
    /// </summary>
    public abstract class OasSecurityScheme : IEquatable<OasSecurityScheme>
    {
        /// <summary>
        /// Creates a new instance of the <see cref="OasSecurityScheme"/> class.
        /// </summary>
        /// <param name="description">The short description for security scheme.</param>
        protected OasSecurityScheme(
            string description = default)
        {
            Description = description;
        }

        /// <summary>
        /// Gets the type of the security scheme.
        /// </summary>
        public abstract OasSecuritySchemeType SchemeType { get; }

        /// <summary>
        /// Gets the short description for security scheme.
        /// </summary>
        /// <remarks>
        /// CommonMark syntax MAY be used for rich text representation.
        /// </remarks>
        public string Description { get; }

        /// <summary>
        /// Implements the operator == operator.
        /// </summary>
        /// <param name="scheme1">The scheme1.</param>
        /// <param name="scheme2">The scheme2.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator ==(OasSecurityScheme scheme1, OasSecurityScheme scheme2)
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
        public static bool operator !=(OasSecurityScheme scheme1, OasSecurityScheme scheme2) => !(scheme1 == scheme2);

        /// <summary>Determines whether the specified object is equal to the current object.</summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns>true if the specified object  is equal to the current object; otherwise, false.</returns>
        public override abstract bool Equals(object obj);

        /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>true if the current object is equal to the <paramref name="other">other</paramref> parameter; otherwise, false.</returns>
        public bool Equals(OasSecurityScheme other)
        {
            if (ReferenceEquals(this, other)) return true;
            if (other is null) return false;

            // https://github.com/dotnet/csharplang/issues/39#issuecomment-291602520
            //if (EqualityContract != other.EqualityContract) return false;

            if (SchemeType != other.SchemeType) return false;
            if (!StringComparer.Ordinal.Equals(Description, other.Description)) return false;

            return true;
        }

        /// <summary>Serves as the default hash function.</summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode() => HashCode.Combine(
            SchemeType,
            StringComparer.Ordinal.GetHashCode(Description ?? string.Empty));
    }
}
