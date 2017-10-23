#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using SourceCode.Clay.OpenApi.Serialization;
using System;
using System.Json;
using System.Net.Mail;

namespace SourceCode.Clay.OpenApi
{
    /// <summary>
    /// Contact information for the exposed API.
    /// </summary>
    public sealed class Contact : IEquatable<Contact>
    {
        #region Fields

        private readonly JsonObject _json;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the identifying name of the contact person/organization.
        /// </summary>
        public string Name
        {
            get
            {
                if (_json.TryGetValue(OpenApiSerializer.PropertyConstants.Name, out var jv))
                    return jv;

                return null;
            }
        }

        /// <summary>
        /// Gets the URL pointing to the contact information.
        /// </summary>
        public Uri Url
        {
            get
            {
                if (_json.TryGetValue(OpenApiSerializer.PropertyConstants.Url, out var jv))
                    return new Uri(jv);

                return null;
            }
        }

        /// <summary>
        /// Gets email address of the contact person/organization.
        /// </summary>
        public MailAddress Email
        {
            get
            {
                if (_json.TryGetValue(OpenApiSerializer.PropertyConstants.Email, out var jv))
                    return new MailAddress(jv);

                return null;
            }
        }

        /// <summary>
        /// Gets custom properties.
        /// </summary>
        public VendorExtensions VendorExtensions { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of the <see cref="Contact"/> class.
        /// </summary>
        /// <param name="name">The name of the contact.</param>
        /// <param name="url">The URL of the contact.</param>
        /// <param name="email">The email of the contact.</param>
        /// <param name="vendorExtensions">Custom vendor properties.</param>
        public Contact(
            string name = default,
            Uri url = default,
            MailAddress email = default,
            JsonObject vendorExtensions = default)
        {
            _json = new JsonObject
            {
                [OpenApiSerializer.PropertyConstants.Name] = name,
                [OpenApiSerializer.PropertyConstants.Url] = url.ToString(),
                [OpenApiSerializer.PropertyConstants.Email] = email.ToString()
            };

            // Vendor extensions
            if (vendorExtensions != null && vendorExtensions.Count > 0)
            {
                VendorExtensions = new VendorExtensions(_json, OpenApiSerializer.PropertyConstants.Name, OpenApiSerializer.PropertyConstants.Url, OpenApiSerializer.PropertyConstants.Email);
            }
        }

        #endregion

        #region IEquatable

        /// <summary>Determines whether the specified object is equal to the current object.</summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns>true if the specified object  is equal to the current object; otherwise, false.</returns>
        public override bool Equals(object obj)
            => obj is Contact other
            && Equals(other);

        /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>true if the current object is equal to the <paramref name="other">other</paramref> parameter; otherwise, false.</returns>
        public bool Equals(Contact other)
        {
            if (this is null) return other is null; // (null, null) or (null, y)
            if (other is null) return false; // (x, null)
            if (ReferenceEquals(this, other)) return true; // (x, x)

            if (_json is null) return other._json is null; // (null, null) or (null, y)
            if (other._json is null) return false; // (x, null)

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
        public override int GetHashCode()
        {
            unchecked
            {
                var hc = 17L;

                if (Name != null)
                    hc = (hc * 23) + StringComparer.Ordinal.GetHashCode(Name);
                if (Url != null)
                    hc = (hc * 23) + Url.GetHashCode();
                if (Email != null)
                    hc = (hc * 23) + StringComparer.Ordinal.GetHashCode(Email);

                return ((int)(hc >> 32)) ^ (int)hc;
            }
        }

        #endregion

        #region Operators

        /// <summary>
        /// Implements the operator == operator.
        /// </summary>
        /// <param name="contact1">The contact1.</param>
        /// <param name="contact2">The contact2.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator ==(Contact contact1, Contact contact2)
        {
            if (contact1 is null && contact2 is null) return true;
            if (contact1 is null || contact2 is null) return false;
            return contact1.Equals((object)contact2);
        }

        /// <summary>
        /// Implements the operator != operator.
        /// </summary>
        /// <param name="contact1">The contact1.</param>
        /// <param name="contact2">The contact2.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator !=(Contact contact1, Contact contact2) => !(contact1 == contact2);

        #endregion
    }
}
