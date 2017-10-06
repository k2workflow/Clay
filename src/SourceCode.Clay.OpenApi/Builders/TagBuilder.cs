#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using System;

namespace SourceCode.Clay.OpenApi
{
    /// <summary>
    /// Adds metadata to a single tag that is used by <see cref="Operation"/>.
    /// </summary>
    public class TagBuilder
    {
        #region Properties

        /// <summary>
        /// Gets or sets the name of the tag.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the short description for the tag.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the additional external documentation for the tag.
        /// </summary>
        public ExternalDocumentation ExternalDocumentation { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of the <see cref="TagBuilder"/> class.
        /// </summary>
        public TagBuilder()
        {
        }

        /// <summary>
        /// Creates a new instance of the <see cref="TagBuilder"/> class.
        /// </summary>
        /// <param name="value">The <see cref="Tag"/> to copy values from.</param>
        public TagBuilder(Tag value)
        {
            if (ReferenceEquals(value, null)) throw new ArgumentNullException(nameof(value));
            Name = value.Name;
            Description = value.Description;
            ExternalDocumentation = value.ExternalDocumentation;
        }

        #endregion

        #region Methods

        public static implicit operator Tag(TagBuilder builder) => builder?.Build();

        public static implicit operator TagBuilder(Tag value) => ReferenceEquals(value, null) ? null : new TagBuilder(value);

        /// <summary>
        /// Creates the <see cref="Tag"/> from this builder.
        /// </summary>
        /// <returns>The <see cref="Tag"/>.</returns>
        public Tag Build() => new Tag(
            name: Name,
            description: Description,
            externalDocumentation: ExternalDocumentation);

        #endregion
    }
}
