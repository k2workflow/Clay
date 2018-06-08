#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using System;

namespace SourceCode.Clay.OpenApi
{
    /// <summary>
    /// Adds metadata to a single tag that is used by <see cref="OasOperation"/>.
    /// </summary>
    public class OasTagBuilder : IOasBuilder<OasTag>
    {
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
        public OasExternalDocumentation ExternalDocumentation { get; set; }

        /// <summary>
        /// Creates a new instance of the <see cref="OasTagBuilder"/> class.
        /// </summary>
        public OasTagBuilder()
        {
        }

        /// <summary>
        /// Creates a new instance of the <see cref="OasTagBuilder"/> class.
        /// </summary>
        /// <param name="value">The <see cref="OasTag"/> to copy values from.</param>
        public OasTagBuilder(OasTag value)
        {
            if (value is null) throw new ArgumentNullException(nameof(value));
            Name = value.Name;
            Description = value.Description;
            ExternalDocumentation = value.ExternalDocumentation;
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="OasTagBuilder"/> to <see cref="OasTag"/>.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator OasTag(OasTagBuilder builder) => builder?.Build();

        /// <summary>
        /// Performs an implicit conversion from <see cref="OasTag"/> to <see cref="OasTagBuilder"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator OasTagBuilder(OasTag value) => value is null ? null : new OasTagBuilder(value);

        /// <summary>
        /// Creates the <see cref="OasTag"/> from this builder.
        /// </summary>
        /// <returns>The <see cref="OasTag"/>.</returns>
        public virtual OasTag Build() => new OasTag(
            name: Name,
            description: Description,
            externalDocumentation: ExternalDocumentation);
    }
}
