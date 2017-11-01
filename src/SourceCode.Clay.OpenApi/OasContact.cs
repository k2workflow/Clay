#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using System;
using System.Net.Mail;

namespace SourceCode.Clay.OpenApi
{
    /// <summary>
    /// Contact information for the exposed API.
    /// </summary>
    public sealed class OasContact : IEquatable<OasContact>
    {
        #region Properties

        /// <summary>
        /// Gets the identifying name of the contact person/organization.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the URL pointing to the contact information.
        /// </summary>
        public Uri Url { get; }

        /// <summary>
        /// Gets email address of the contact person/organization.
        /// </summary>
        public MailAddress Email { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of the <see cref="OasContact"/> class.
        /// </summary>
        /// <param name="name">The name of the contact.</param>
        /// <param name="url">The URL of the contact.</param>
        /// <param name="email">The email of the contact.</param>
        public OasContact(
            string name = default,
            Uri url = default,
            MailAddress email = default)
        {
            Name = name;
            Url = url;
            Email = email;
        }

        #endregion

        #region IEquatable

        /// <summary>Determines whether the specified object is equal to the current object.</summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns>true if the specified object  is equal to the current object; otherwise, false.</returns>
        public override bool Equals(object obj)
            => obj is OasContact other
            && Equals(other);

        /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>true if the current object is equal to the <paramref name="other">other</paramref> parameter; otherwise, false.</returns>
        public bool Equals(OasContact other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;

            if (!StringComparer.Ordinal.Equals(Name, other.Name)) return false;
            if (Url != other.Url) return false;

            if (!ReferenceEquals(Email, other.Email))
            {
                if (Email is null || other.Email is null) return false;
                return Email.Equals(other.Email);
            }

            return true;
        }

        /// <summary>Serves as the default hash function.</summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode() => new HashCode()
            .Tally(Name ?? string.Empty, StringComparer.Ordinal)
            .Tally(Url)
            .Tally(Email)
            .ToHashCode();

        #endregion

        #region Operators

        /// <summary>
        /// Implements the operator == operator.
        /// </summary>
        /// <param name="contact1">The contact1.</param>
        /// <param name="contact2">The contact2.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator ==(OasContact contact1, OasContact contact2)
        {
            if (contact1 is null) return contact2 is null;
            return contact1.Equals((object)contact2);
        }

        /// <summary>
        /// Implements the operator != operator.
        /// </summary>
        /// <param name="x">The contact1.</param>
        /// <param name="y">The contact2.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator !=(OasContact x, OasContact y) => !(x == y);

        #endregion
    }
}
