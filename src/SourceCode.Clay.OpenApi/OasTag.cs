#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using System;

namespace SourceCode.Clay.OpenApi
{
    /// <summary>
    /// Adds metadata to a single tag that is used by <see cref="OasOperation"/>.
    /// </summary>
    public class OasTag : IEquatable<OasTag>
    {
        #region Properties

        /// <summary>
        /// Gets the name of the tag.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the short description for the tag.
        /// </summary>
        public string Description { get; }

        /// <summary>
        /// Gets the additional external documentation for the tag.
        /// </summary>
        public OasExternalDocumentation ExternalDocumentation { get; }

        #endregion

        #region Constructor

        /// <summary>
        /// Creates a new instance of the <see cref="OasTag"/> class.
        /// </summary>
        /// <param name="name">The name of the tag.</param>
        /// <param name="description">The short description for the tag.</param>
        /// <param name="externalDocumentation">The additional external documentation for the tag.</param>
        public OasTag(
            string name = default,
            string description = default,
            OasExternalDocumentation externalDocumentation = default)
        {
            Name = name;
            Description = description;
            ExternalDocumentation = externalDocumentation;
        }

        #endregion

        #region IEquatable

        /// <summary>
        /// Implements the operator == operator.
        /// </summary>
        /// <param name="tag1">The tag1.</param>
        /// <param name="tag2">The tag2.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator ==(OasTag tag1, OasTag tag2)
        {
            if (tag1 is null) return tag2 is null;
            return tag1.Equals((object)tag2);
        }

        /// <summary>
        /// Implements the operator != operator.
        /// </summary>
        /// <param name="tag1">The tag1.</param>
        /// <param name="tag2">The tag2.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator !=(OasTag tag1, OasTag tag2) => !(tag1 == tag2);

        /// <summary>Determines whether the specified object is equal to the current object.</summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns>true if the specified object  is equal to the current object; otherwise, false.</returns>
        public override bool Equals(object obj)
            => obj is OasTag other
            && Equals(other);

        /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>true if the current object is equal to the <paramref name="other">other</paramref> parameter; otherwise, false.</returns>
        public virtual bool Equals(OasTag other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;

            if (!StringComparer.Ordinal.Equals(Name, other.Name)) return false;
            if (!StringComparer.Ordinal.Equals(Description, other.Description)) return false;
            if (!ExternalDocumentation.NullableEquals(other.ExternalDocumentation)) return false;

            return true;
        }

        /// <summary>Serves as the default hash function.</summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode() => HashCode.Combine
        (
            StringComparer.Ordinal.GetHashCode(Name ?? string.Empty),
            StringComparer.Ordinal.GetHashCode(Description ?? string.Empty),
            ExternalDocumentation
        );

        #endregion
    }
}
