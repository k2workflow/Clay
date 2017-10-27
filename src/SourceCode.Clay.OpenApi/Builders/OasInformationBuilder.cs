#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using System;

namespace SourceCode.Clay.OpenApi
{
    /// <summary>
    /// The object provides metadata about the API. The metadata MAY be used by the
    /// clients if needed, and MAY be presented in editing or documentation generation
    /// tools for convenience.
    /// </summary>
    public class OasInformationBuilder : IOasBuilder<OasInformation>
    {
        #region Properties

        /// <summary>
        /// Gets or sets the title of the exposed API.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets the short description of the exposed API.
        /// </summary>
        /// <remarks>
        /// CommonMark syntax MAY be used for rich text representation.
        /// </remarks>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the URL to the Terms of Service for the exposed API.
        /// </summary>
        public Uri TermsOfService { get; set; }

        /// <summary>
        /// Gets or sets the contact information for the exposed API.
        /// </summary>
        public OasContact Contact { get; set; }

        /// <summary>
        /// Gets or sets the license information for the exposed API.
        /// </summary>
        public OasLicense License { get; set; }

        /// <summary>
        /// Gets or sets the version of the exposed API.
        /// </summary>
        public SemanticVersion Version { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of the <see cref="OasInformationBuilder"/> class.
        /// </summary>
        public OasInformationBuilder()
        {
        }

        /// <summary>
        /// Creates a new instance of the <see cref="OasInformationBuilder"/> class.
        /// </summary>
        /// <param name="value">The <see cref="OasInformation"/> to copy values from.</param>
        public OasInformationBuilder(OasInformation value)
        {
            if (value is null) throw new ArgumentNullException(nameof(value));
            Title = value.Title;
            Description = value.Description;
            TermsOfService = value.TermsOfService;
            Contact = value.Contact;
            License = value.License;
            Version = value.Version;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Performs an implicit conversion from <see cref="OasInformationBuilder"/> to <see cref="OasInformation"/>.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator OasInformation(OasInformationBuilder builder) => builder?.Build();

        /// <summary>
        /// Performs an implicit conversion from <see cref="OasInformation"/> to <see cref="OasInformationBuilder"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator OasInformationBuilder(OasInformation value) => value is null ? null : new OasInformationBuilder(value);

        /// <summary>
        /// Creates the <see cref="OasInformation"/> from this builder.
        /// </summary>
        /// <returns>The <see cref="OasInformation"/>.</returns>
        public OasInformation Build() => new OasInformation(
            title: Title,
            description: Description,
            termsOfService: TermsOfService,
            contact: Contact,
            license: License,
            version: Version);

        #endregion
    }
}
