using System;

namespace SourceCode.Clay.OpenApi
{
    /// <summary>
    /// Adds metadata to a single tag that is used by <see cref="Operation"/>.
    /// </summary>
    public class Tag : IEquatable<Tag>
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
        public ExternalDocumentation ExternalDocumentation { get; }

        #endregion

        #region Constructor

        /// <summary>
        /// Creates a new instance of the <see cref="Tag"/> class.
        /// </summary>
        /// <param name="name">The name of the tag.</param>
        /// <param name="description">The short description for the tag.</param>
        /// <param name="externalDocumentation">The additional external documentation for the tag.</param>
        public Tag(
            string name = null, 
            string description = null, 
            ExternalDocumentation externalDocumentation = null)
        {
            Name = name;
            Description = description;
            ExternalDocumentation = externalDocumentation;
        }

        #endregion

        #region IEquatable

        /// <summary>Determines whether the specified object is equal to the current object.</summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns>true if the specified object  is equal to the current object; otherwise, false.</returns>
        public override bool Equals(object obj) => Equals(obj as Tag);

        /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>true if the current object is equal to the <paramref name="other">other</paramref> parameter; otherwise, false.</returns>
        public virtual bool Equals(Tag other)
        {
            if (ReferenceEquals(other, null)) return false;
            if (ReferenceEquals(this, other)) return true;

            if (!StringComparer.Ordinal.Equals(Name, other.Name)) return false;
            if (!StringComparer.Ordinal.Equals(Description, other.Description)) return false;
            if (!ExternalDocumentation.NullableEquals(other.ExternalDocumentation)) return false;

            return true;
        }

        /// <summary>Serves as the default hash function.</summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                var hc = 17L;

                if (Name != null) hc = hc * 21 + StringComparer.Ordinal.GetHashCode(Name);
                if (Description != null) hc = hc * 21 + StringComparer.Ordinal.GetHashCode(Description);
                if (ExternalDocumentation != null) hc = hc * 21 + ExternalDocumentation.GetHashCode();

                return ((int)(hc >> 32)) ^ (int)hc;
            }
        }

        public static bool operator ==(Tag tag1, Tag tag2)
        {
            if (ReferenceEquals(tag1, null) && ReferenceEquals(tag2, null)) return true;
            if (ReferenceEquals(tag1, null) || ReferenceEquals(tag2, null)) return false;
            return tag1.Equals((object)tag2);
        }

        public static bool operator !=(Tag tag1, Tag tag2) => !(tag1 == tag2);

        #endregion
    }
}
