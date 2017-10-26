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
    public class OasContactBuilder : IOasBuilder<OasContact>
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
        /// Creates a new instance of the <see cref="OasContactBuilder"/> class.
        /// </summary>
        public OasContactBuilder()
        {
        }

        /// <summary>
        /// Creates a new instance of the <see cref="OasContactBuilder"/> class.
        /// </summary>
        /// <param name="value">The <see cref="OasContact"/> to copy values from.</param>
        public OasContactBuilder(OasContact value)
        {
            if (value is null) throw new ArgumentNullException(nameof(value));
            Name = value.Name;
            Url = value.Url;
            Email = value.Email;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Performs an implicit conversion from <see cref="OasContactBuilder"/> to <see cref="OasContact"/>.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator OasContact(OasContactBuilder builder) => builder?.Build();

        /// <summary>
        /// Performs an implicit conversion from <see cref="OasContact"/> to <see cref="OasContactBuilder"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator OasContactBuilder(OasContact value) => value is null ? null : new OasContactBuilder(value);

        /// <summary>
        /// Creates the <see cref="OasContact"/> from this builder.
        /// </summary>
        /// <returns>The <see cref="OasContact"/>.</returns>
        public OasContact Build() => new OasContact(
            name: Name,
            url: Url,
            email: Email);

        #endregion
    }
}
