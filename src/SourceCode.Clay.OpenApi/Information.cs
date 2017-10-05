using System;

namespace SourceCode.Clay.OpenApi
{
    /// <summary>
    ///   The object provides metadata about the API. The metadata MAY be used by the clients if
    ///   needed, and MAY be presented in editing or documentation generation tools for convenience.
    /// </summary>
    public class Information : IEquatable<Information>
    {
        #region Properties

        /// <summary>
        ///   Gets the contact information for the exposed API.
        /// </summary>
        public Contact Contact { get; }

        /// <summary>
        ///   Gets the short description of the exposed API.
        /// </summary>
        /// <remarks>CommonMark syntax MAY be used for rich text representation.</remarks>
        public string Description { get; }

        /// <summary>
        ///   Gets the license information for the exposed API.
        /// </summary>
        public License License { get; }

        /// <summary>
        ///   Gets the URL to the Terms of Service for the exposed API.
        /// </summary>
        public Uri TermsOfService { get; }

        /// <summary>
        ///   Gets the title of the exposed API.
        /// </summary>
        public string Title { get; }

        /// <summary>
        ///   Gets the version of the exposed API.
        /// </summary>
        public SemanticVersion Version { get; }

        #endregion Properties

        #region Constructors

        /// <summary>
        ///   Creates a new instance of the <see cref="Information"/> class.
        /// </summary>
        /// <param name="title">The title of the exposed API.</param>
        /// <param name="description">The short description of the exposed API.</param>
        /// <param name="termsOfService">The URL to the Terms of Service for the exposed API.</param>
        /// <param name="contact">The contact information for the exposed API.</param>
        /// <param name="license">The license information for the exposed API.</param>
        /// <param name="version">The version of the exposed API.</param>
        public Information(
            string title = default,
            string description = default,
            Uri termsOfService = default,
            Contact contact = default,
            License license = default,
            SemanticVersion version = default)
        {
            Title = title;
            Description = description;
            TermsOfService = termsOfService;
            Contact = contact;
            License = license;
            Version = version;
        }

        #endregion Constructors

        #region IEquatable

        public static bool operator !=(Information information1, Information information2)
                    => !(information1 == information2);

        public static bool operator ==(Information information1, Information information2)
        {
            if (ReferenceEquals(information1, null) && ReferenceEquals(information2, null)) return true;
            if (ReferenceEquals(information1, null) || ReferenceEquals(information2, null)) return false;
            return information1.Equals((object)information2);
        }

        /// <summary>
        ///   Determines whether the specified object is equal to the current object.
        /// </summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns>
        ///   true if the specified object is equal to the current object; otherwise, false.
        /// </returns>
        public override bool Equals(object obj) => Equals(obj as Information);

        /// <summary>
        ///   Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        ///   true if the current object is equal to the <paramref name="other">other</paramref>
        ///   parameter; otherwise, false.
        /// </returns>
        public virtual bool Equals(Information other)
        {
            if (ReferenceEquals(other, null)) return false;
            if (ReferenceEquals(this, other)) return true;

            if (!StringComparer.Ordinal.Equals(Title, other.Title)) return false;
            if (!StringComparer.Ordinal.Equals(Description, other.Description)) return false;
            if (TermsOfService != other.TermsOfService) return false;
            if (!Contact.NullableEquals(other.Contact)) return false;
            if (!License.NullableEquals(other.License)) return false;
            if (Version != other.Version) return false;

            return true;
        }

        /// <summary>
        ///   Serves as the default hash function.
        /// </summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode()
        {
            var hc = 17;

            unchecked
            {
                if (Title != null) hc = hc * 23 + StringComparer.Ordinal.GetHashCode(Title);
                hc = hc * 23 + Version.GetHashCode();
            }

            return hc;
        }

        #endregion IEquatable
    }
}
