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
    public class ContactBuilder
    {
        #region Properties

        /// <summary>
        /// Gets or sets the identifying name of the contact person/organization.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the URL pointing to the contact information.
        /// </summary>
        public Uri Url { get; set; }

        /// <summary>
        /// Gets or sets email address of the contact person/organization.
        /// </summary>
        public MailAddress Email { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of the <see cref="ContactBuilder"/> class.
        /// </summary>
        public ContactBuilder()
        {
        }

        /// <summary>
        /// Creates a new instance of the <see cref="ContactBuilder"/> class.
        /// </summary>
        /// <param name="value">The <see cref="Contact"/> to copy values from.</param>
        public ContactBuilder(Contact value)
        {
            if (ReferenceEquals(value, null)) throw new ArgumentNullException(nameof(value));
            Name = value.Name;
            Url = value.Url;
            Email = value.Email;
        }

        #endregion

        #region Methods

        public static implicit operator Contact(ContactBuilder builder) => builder?.Build();

        public static implicit operator ContactBuilder(Contact value) => ReferenceEquals(value, null) ? null : new ContactBuilder(value);

        /// <summary>
        /// Creates the <see cref="Contact"/> from this builder.
        /// </summary>
        /// <returns>The <see cref="Contact"/>.</returns>
        public Contact Build() => new Contact(
            name: Name,
            url: Url,
            email: Email);

        #endregion
    }
}
