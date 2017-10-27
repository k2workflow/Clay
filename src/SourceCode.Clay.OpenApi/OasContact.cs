#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using SourceCode.Clay.Json;
using SourceCode.Clay.OpenApi.Serialization;
using System;
using System.Collections.Generic;
using System.Json;
using System.Net.Mail;
using System.Runtime.CompilerServices;

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

        /// <summary>
        /// Gets custom properties.
        /// </summary>
        //public IReadOnlyDictionary<string, JsonValue> VendorExtensions { get; }

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
        //JsonObject vendorExtensions = default)
        {
            Name = name;
            Url = url;
            Email = email;

            // Vendor extensions
            //VendorExtensions = new VendorExtensions(vendorExtensions,
            //    OasSerializer.PropertyConstants.Name,
            //    OasSerializer.PropertyConstants.Url,
            //    OasSerializer.PropertyConstants.Email);
        }

        #endregion

        #region IEquatable

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool Equals(OasContact x, OasContact y)
        {
            if (x is null) return y is null; // (null, null) or (null, y)
            if (y is null) return false; // (x, null)
            if (ReferenceEquals(x, y)) return true; // (x, x)

            if (!StringComparer.Ordinal.Equals(x.Name, y.Name)) return false;
            if (x.Url != y.Url) return false;

            if (!ReferenceEquals(x.Email, y.Email))
            {
                if (x.Email is null || y.Email is null) return false;
                return x.Email.Equals(y.Email);
            }

            //if (x.VendorExtensions != y.VendorExtensions) return false;

            return true;
        }

        /// <summary>Determines whether the specified object is equal to the current object.</summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns>true if the specified object  is equal to the current object; otherwise, false.</returns>
        public override bool Equals(object obj)
            => obj is OasContact other
            && Equals(other);

        /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>true if the current object is equal to the <paramref name="other">other</paramref> parameter; otherwise, false.</returns>
        public bool Equals(OasContact other) => Equals(this, other);

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

                //if (VendorExtensions != null)
                //    hc = (hc * 23) + VendorExtensions.GetHashCode();

                return ((int)(hc >> 32)) ^ (int)hc;
            }
        }

        #endregion

        #region De/Serialization

        public static OasContact Parse(JsonObject json)
        {
            string name = null;
            if (json.TryGetValue(OasSerializer.PropertyConstants.Name, JsonType.String, true, out var jv))
                name = jv;

            Uri url = null;
            if (json.TryGetValue(OasSerializer.PropertyConstants.Url, JsonType.String, true, out jv))
                url = new Uri(jv);

            MailAddress email = null;
            if (json.TryGetValue(OasSerializer.PropertyConstants.Email, JsonType.String, true, out jv))
                email = new MailAddress(jv);

            var result = new OasContact(name, url, email);
            return result;
        }

        public override string ToString()
        {
            var json = new JsonObject
            {
                [OasSerializer.PropertyConstants.Name] = Name,
                [OasSerializer.PropertyConstants.Url] = Url.ToString(),
                [OasSerializer.PropertyConstants.Email] = Email.ToString()
            };

            //foreach (var item in VendorExtensions)
            //    json.Add(item);

            var str = json.ToString();
            return str;
        }

        #endregion

        #region Operators

        /// <summary>
        /// Implements the operator == operator.
        /// </summary>
        /// <param name="x">The contact1.</param>
        /// <param name="y">The contact2.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator ==(OasContact x, OasContact y) => Equals(x, y);

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
