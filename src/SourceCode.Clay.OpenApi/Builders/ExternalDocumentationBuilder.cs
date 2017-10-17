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
    public class ExternalDocumentationBuilder : IOpenApiBuilder<ExternalDocumentation>
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
        /// Creates a new instance of the <see cref="ExternalDocumentationBuilder"/> class.
        /// </summary>
        public ExternalDocumentationBuilder()
        {
        }

        /// <summary>
        /// Creates a new instance of the <see cref="ExternalDocumentationBuilder"/> class.
        /// </summary>
        /// <param name="value">The <see cref="ExternalDocumentation"/> to copy values from.</param>
        public ExternalDocumentationBuilder(ExternalDocumentation value)
        {
            if (value is null) throw new ArgumentNullException(nameof(value));
            Description = value.Description;
            Url = value.Url;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Performs an implicit conversion from <see cref="ExternalDocumentationBuilder"/> to <see cref="ExternalDocumentation"/>.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator ExternalDocumentation(ExternalDocumentationBuilder builder) => builder?.Build();

        /// <summary>
        /// Performs an implicit conversion from <see cref="ExternalDocumentation"/> to <see cref="ExternalDocumentationBuilder"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator ExternalDocumentationBuilder(ExternalDocumentation value) => value is null ? null : new ExternalDocumentationBuilder(value);

        /// <summary>
        /// Creates the <see cref="ExternalDocumentation"/> from this builder.
        /// </summary>
        /// <returns>The <see cref="ExternalDocumentation"/>.</returns>
        public ExternalDocumentation Build() => new ExternalDocumentation(
            description: Description,
            url: Url);

        #endregion
    }
}
