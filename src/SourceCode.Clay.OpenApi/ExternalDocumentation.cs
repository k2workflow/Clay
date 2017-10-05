using System;

namespace SourceCode.Clay.OpenApi
{
    /// <summary>
    /// Allows referencing an external resource for extended documentation.
    /// </summary>
    public class ExternalDocumentation : IEquatable<ExternalDocumentation>
    {
        #region Properties

        /// <summary>
        /// Get the short description of the target documentation.
        /// </summary>
        /// <remarks>
        /// CommonMark syntax MAY be used for rich text representation.
        /// </remarks>
        public string Description { get; }

        /// <summary>
        /// Gets the URL for the target documentation.
        /// </summary>
        public Uri Url { get; }

        #endregion

        /// <summary>
        /// Creates a new instance of the <see cref="ExternalDocumentation"/> class.
        /// </summary>
        /// <param name="description">The short description of the target documentation.</param>
        /// <param name="url">The URL for the target documentation.</param>
        public ExternalDocumentation(
            string description = default,
            Uri url = default)
        {
            Description = description;
            Url = url;
        }

        #region IEquatable

        /// <summary>Indicates whether this instance and a specified object are equal.</summary>
        /// <param name="obj">The object to compare with the current instance.</param>
        /// <returns>true if <paramref name="obj">obj</paramref> and this instance are the same type and represent the same value; otherwise, false.</returns>
        public override bool Equals(object obj) => Equals(obj as ExternalDocumentation);

        /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>true if the current object is equal to the <paramref name="other">other</paramref> parameter; otherwise, false.</returns>
        public bool Equals(ExternalDocumentation other)
        {
            if (ReferenceEquals(other, null)) return false;
            if (ReferenceEquals(this, other)) return true;

            if (!StringComparer.Ordinal.Equals(Description, other.Description)) return false;
            if (Url != other.Url) return false;

            return true;
        }

        /// <summary>Returns the hash code for this instance.</summary>
        /// <returns>A 32-bit signed integer that is the hash code for this instance.</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                var hc = 17L;

                if (Description != null) hc = hc * 23 + StringComparer.Ordinal.GetHashCode(Description);
                if (Url != null) hc = hc * 23 + Url.GetHashCode();

                return ((int)(hc >> 32)) ^ (int)hc;
            }
        }

        public static bool operator ==(ExternalDocumentation externalDocumentation1, ExternalDocumentation externalDocumentation2)
        {
            if (ReferenceEquals(externalDocumentation1, null) && ReferenceEquals(externalDocumentation2, null)) return true;
            if (ReferenceEquals(externalDocumentation1, null) || ReferenceEquals(externalDocumentation2, null)) return false;
            return externalDocumentation1.Equals((object)externalDocumentation2);
        }

        public static bool operator !=(ExternalDocumentation externalDocumentation1, ExternalDocumentation externalDocumentation2)
            => !(externalDocumentation1 == externalDocumentation2);

        #endregion
    }
}