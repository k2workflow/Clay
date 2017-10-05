using System;

namespace SourceCode.Clay.OpenApi
{
    /// <summary>
    ///   Defines a security scheme that can be used by the operations.
    /// </summary>
    public abstract class SecurityScheme : IEquatable<SecurityScheme>
    {
        #region Constructors

        /// <summary>
        ///   Creates a new instance of the <see cref="SecurityScheme"/> class.
        /// </summary>
        /// <param name="description">The short description for security scheme.</param>
        protected SecurityScheme(
            string description = default)
        {
            Description = description;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        ///   Gets the short description for security scheme.
        /// </summary>
        /// <remarks>CommonMark syntax MAY be used for rich text representation.</remarks>
        public string Description { get; }

        /// <summary>
        ///   Gets the type of the security scheme.
        /// </summary>
        public abstract SecuritySchemeType Type { get; }

        #endregion Properties

        #region Equatable

        public static bool operator !=(SecurityScheme scheme1, SecurityScheme scheme2) => !(scheme1 == scheme2);

        public static bool operator ==(SecurityScheme scheme1, SecurityScheme scheme2)
        {
            if (ReferenceEquals(scheme1, null) && ReferenceEquals(scheme2, null)) return true;
            if (ReferenceEquals(scheme1, null) || ReferenceEquals(scheme2, null)) return false;
            return scheme1.Equals((object)scheme2);
        }

        /// <summary>
        ///   Determines whether the specified object is equal to the current object.
        /// </summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns>
        ///   true if the specified object is equal to the current object; otherwise, false.
        /// </returns>
        public override abstract bool Equals(object obj);

        /// <summary>
        ///   Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        ///   true if the current object is equal to the <paramref name="other">other</paramref>
        ///   parameter; otherwise, false.
        /// </returns>
        public bool Equals(SecurityScheme other)
        {
            if (ReferenceEquals(this, other)) return true;
            if (ReferenceEquals(other, null)) return false;

            if (Type != other.Type) return false;
            if (!StringComparer.Ordinal.Equals(Description, other.Description)) return false;

            return true;
        }

        /// <summary>
        ///   Serves as the default hash function.
        /// </summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                var hc = 17L;

                hc = hc * 21 + Type.GetHashCode();
                if (Description != null) hc = hc * 21 + StringComparer.Ordinal.GetHashCode(Description);

                return ((int)(hc >> 32)) ^ (int)hc;
            }
        }

        #endregion Equatable
    }
}
