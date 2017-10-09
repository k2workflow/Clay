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
    public class License : IEquatable<License>
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

        #region Ctor

        /// <summary>
        /// Creates a new instance of the <see cref="License"/> class.
        /// </summary>
        /// <param name="name">The licese named used for the API.</param>
        /// <param name="url">The URL to the license used for the API.</param>
        public License(
            string name = default,
            Uri url = default)
        {
            Name = name;
            Url = url;
        }

        #endregion

        #region IEquatable

        public static bool operator ==(License license1, License license2)
        {
            if (ReferenceEquals(license1, null) && ReferenceEquals(license2, null)) return true;
            if (ReferenceEquals(license1, null) || ReferenceEquals(license2, null)) return false;
            return license1.Equals((object)license2);
        }

        public static bool operator !=(License license1, License license2) => !(license1 == license2);

        /// <summary>Determines whether the specified object is equal to the current object.</summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns>true if the specified object  is equal to the current object; otherwise, false.</returns>
        public override bool Equals(object obj) => Equals(obj as License);

        /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>true if the current object is equal to the <paramref name="other">other</paramref> parameter; otherwise, false.</returns>
        public virtual bool Equals(License other)
        {
            if (ReferenceEquals(other, null)) return false;
            if (ReferenceEquals(this, other)) return true;

            if (!StringComparer.Ordinal.Equals(Name, other.Name)) return false;
            if (Url != other.Url) return false;

            return true;
        }

        /// <summary>Serves as the default hash function.</summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                var hc = 17L;

                if (Name != null) hc = hc * 23 + StringComparer.Ordinal.GetHashCode(Name);
                if (Url != null) hc = hc * 23 + Url.GetHashCode();

                return ((int)(hc >> 32)) ^ (int)hc;
            }
        }

        #endregion
    }
}
