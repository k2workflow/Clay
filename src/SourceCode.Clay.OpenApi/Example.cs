using System;

namespace SourceCode.Clay.OpenApi
{
    /// <summary>
    /// Contains an example of how to use an entity in an Open API document.
    /// </summary>
    /// <remarks>
    /// The <c>Value</c> property is implementation-specific. If it is required, create a new type
    /// that inherits from this one.
    /// </remarks>
    public class Example : IEquatable<Example>
    {
        #region Properties

        /// <summary>
        /// Gets the short description for the example.
        /// </summary>
        public string Summary { get; }

        /// <summary>
        /// Gets the long description for the example.
        /// </summary>
        /// <remarks>CommonMark syntax MAY be used for rich text representation.</remarks>
        public string Description { get; }

        /// <summary>
        /// Gets the URL that points to the literal example.
        /// </summary>
        public Uri ExternalValue { get; }

        #endregion

        #region Ctor

        /// <summary>
        /// Creates a new instance of the <see cref="Example"/> class.
        /// </summary>
        /// <param name="summary">The short description for the example.</param>
        /// <param name="description">The long description for the example.</param>
        /// <param name="externalValue">The URL that points to the literal example.</param>
        public Example(
            string summary = default,
            string description = default,
            Uri externalValue = default)
        {
            Summary = summary;
            Description = description;
            ExternalValue = externalValue;
        }

        #endregion

        #region IEquatable

        /// <summary>Determines whether the specified object is equal to the current object.</summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns>true if the specified object  is equal to the current object; otherwise, false.</returns>
        public override bool Equals(object obj) => Equals(obj as Example);

        /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>true if the current object is equal to the <paramref name="other">other</paramref> parameter; otherwise, false.</returns>
        public virtual bool Equals(Example other)
        {
            if (ReferenceEquals(other, null)) return false;
            if (ReferenceEquals(this, other)) return true;

            if (!StringComparer.Ordinal.Equals(Summary, other.Summary)) return false;
            if (!StringComparer.Ordinal.Equals(Description, other.Description)) return false;
            if (ExternalValue != other.ExternalValue) return false;

            return true;
        }

        /// <summary>Serves as the default hash function.</summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                var hc = 17L;

                if (Summary != null) hc = hc * 21 + StringComparer.Ordinal.GetHashCode(Summary);
                if (Description != null) hc = hc * 21 + StringComparer.Ordinal.GetHashCode(Description);
                if (ExternalValue != null) hc = hc * 21 + ExternalValue.GetHashCode();

                return ((int)(hc >> 32)) ^ (int)hc;
            }
        }

        public static bool operator ==(Example example1, Example example2)
        {
            if (ReferenceEquals(example1, null) && ReferenceEquals(example2, null)) return true;
            if (ReferenceEquals(example1, null) || ReferenceEquals(example2, null)) return false;
            return example1.Equals((object)example2);
        }

        public static bool operator !=(Example example1, Example example2) => !(example1 == example2);

        #endregion
    }
}
