#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using System;

namespace SourceCode.Clay.OpenApi
{
    /// <summary>
    /// Allows referencing an external resource for extended documentation.
    /// </summary>
    public class OasExternalDocumentationBuilder : IOasBuilder<OasExternalDocumentation>
    {
        #region Properties

        /// <summary>
        /// Get or sets the short description of the target documentation.
        /// </summary>
        /// <remarks>
        /// CommonMark syntax MAY be used for rich text representation.
        /// </remarks>
        public string Description { get; set; }

        /// <summary>
        /// Gets the URL for the target documentation.
        /// </summary>
        public Uri Url { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of the <see cref="OasExternalDocumentationBuilder"/> class.
        /// </summary>
        public OasExternalDocumentationBuilder()
        {
        }

        /// <summary>
        /// Creates a new instance of the <see cref="OasExternalDocumentationBuilder"/> class.
        /// </summary>
        /// <param name="value">The <see cref="OasExternalDocumentation"/> to copy values from.</param>
        public OasExternalDocumentationBuilder(OasExternalDocumentation value)
        {
            if (value is null) throw new ArgumentNullException(nameof(value));
            Description = value.Description;
            Url = value.Url;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Performs an implicit conversion from <see cref="OasExternalDocumentationBuilder"/> to <see cref="OasExternalDocumentation"/>.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator OasExternalDocumentation(OasExternalDocumentationBuilder builder) => builder?.Build();

        /// <summary>
        /// Performs an implicit conversion from <see cref="OasExternalDocumentation"/> to <see cref="OasExternalDocumentationBuilder"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator OasExternalDocumentationBuilder(OasExternalDocumentation value) => value is null ? null : new OasExternalDocumentationBuilder(value);

        /// <summary>
        /// Creates the <see cref="OasExternalDocumentation"/> from this builder.
        /// </summary>
        /// <returns>The <see cref="OasExternalDocumentation"/>.</returns>
        public OasExternalDocumentation Build() => new OasExternalDocumentation(
            description: Description,
            url: Url);

        #endregion
    }
}
