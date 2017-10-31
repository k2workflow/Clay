#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using System;

namespace SourceCode.Clay.OpenApi
{
    /// <summary>
    /// License information for the exposed API.
    /// </summary>
    public class OasLicense : IEquatable<OasLicense>
    {
        #region Properties

        /// <summary>
        /// Gets the license name used for the API.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the URL to the license used for the API
        /// </summary>
        public Uri Url { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of the <see cref="OasLicense"/> class.
        /// </summary>
        /// <param name="name">The licese named used for the API.</param>
        /// <param name="url">The URL to the license used for the API.</param>
        public OasLicense(
            string name = default,
            Uri url = default)
        {
            Name = name;
            Url = url;
        }

        #endregion

        #region IEquatable

        /// <summary>
        /// Implements the operator == operator.
        /// </summary>
        /// <param name="license1">The license1.</param>
        /// <param name="license2">The license2.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator ==(OasLicense license1, OasLicense license2)
        {
            if (license1 is null) return license2 is null;
            return license1.Equals((object)license2);
        }

        /// <summary>
        /// Implements the operator != operator.
        /// </summary>
        /// <param name="license1">The license1.</param>
        /// <param name="license2">The license2.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator !=(OasLicense license1, OasLicense license2)
            => !(license1 == license2);

        /// <summary>Determines whether the specified object is equal to the current object.</summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns>true if the specified object  is equal to the current object; otherwise, false.</returns>
        public override bool Equals(object obj)
            => obj is OasLicense other
            && Equals(other);

        /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>true if the current object is equal to the <paramref name="other">other</paramref> parameter; otherwise, false.</returns>
        public virtual bool Equals(OasLicense other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;

            if (!StringComparer.Ordinal.Equals(Name, other.Name)) return false;
            if (Url != other.Url) return false;

            return true;
        }

        /// <summary>Serves as the default hash function.</summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode() => new HashCode()
            .Tally(Name ?? string.Empty, StringComparer.Ordinal)
            .Tally(Url)
            .ToHashCode();

        #endregion
    }
}
