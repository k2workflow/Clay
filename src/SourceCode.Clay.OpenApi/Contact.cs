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
    public class Contact : IEquatable<Contact>
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
        /// Creates a new instance of the <see cref="Contact"/> class.
        /// </summary>
        /// <param name="name">The name of the contact.</param>
        /// <param name="url">The URL of the contact.</param>
        /// <param name="email">The email of the contact.</param>
        public Contact(
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

        public static bool operator ==(Contact contact1, Contact contact2)
        {
            if (ReferenceEquals(contact1, null) && ReferenceEquals(contact2, null)) return true;
            if (ReferenceEquals(contact1, null) || ReferenceEquals(contact2, null)) return false;
            return contact1.Equals((object)contact2);
        }

        public static bool operator !=(Contact contact1, Contact contact2) => !(contact1 == contact2);

        /// <summary>Determines whether the specified object is equal to the current object.</summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns>true if the specified object  is equal to the current object; otherwise, false.</returns>
        public override bool Equals(object obj) => Equals(obj as Contact);

        /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>true if the current object is equal to the <paramref name="other">other</paramref> parameter; otherwise, false.</returns>
        public virtual bool Equals(Contact other)
        {
            if (ReferenceEquals(other, null)) return false;
            if (ReferenceEquals(this, other)) return true;

            if (!StringComparer.Ordinal.Equals(Name, other.Name)) return false;
            if (Url != other.Url) return false;
            if (!ReferenceEquals(Email, other.Email))
            {
                if (ReferenceEquals(Email, null) || ReferenceEquals(other.Email, null)) return false;
                return Email.Equals(other.Email);
            }

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
                if (Url != null) hc = hc * 21 + Url.GetHashCode();
                if (Email != null) hc = hc * 21 + StringComparer.Ordinal.GetHashCode(Email);

                return ((int)(hc >> 32)) ^ (int)hc;
            }
        }

        #endregion
    }
}
