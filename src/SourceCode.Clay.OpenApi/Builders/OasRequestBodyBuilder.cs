#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Mime;

namespace SourceCode.Clay.OpenApi
{
    /// <summary>
    /// Describes a single request body.
    /// </summary>
    public class OasRequestBodyBuilder : IOasBuilder<OasRequestBody>
    {
        /// <summary>
        /// Gets or sets the brief description of the request body.
        /// </summary>
        /// <remarks>
        /// CommonMark syntax MAY be used for rich text representation.
        /// </remarks>
        public String Description { get; set; }

        /// <summary>
        /// Gets or sets the content of the request body.
        /// </summary>
        public IDictionary<ContentType, OasMediaType> Content { get; }

        /// <summary>
        /// Gets or sets the request body options.
        /// </summary>
        public OasRequestBodyOptions Options { get; set; }

        /// <summary>
        /// Creates a new instance of the <see cref="OasRequestBodyBuilder"/> class.
        /// </summary>
        public OasRequestBodyBuilder()
        {
            Content = new Dictionary<ContentType, OasMediaType>();
        }

        /// <summary>
        /// Creates a new instance of the <see cref="OasRequestBodyBuilder"/> class.
        /// </summary>
        /// <param name="value">The <see cref="OasRequestBody"/> to copy values from.</param>
        public OasRequestBodyBuilder(OasRequestBody value)
        {
            if (value is null) throw new ArgumentNullException(nameof(value));
            Description = value.Description;
            Content = new Dictionary<ContentType, OasMediaType>(value.Content);
            Options = value.Options;
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="OasRequestBodyBuilder"/> to <see cref="OasRequestBody"/>.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator OasRequestBody(OasRequestBodyBuilder builder) => builder?.Build();

        /// <summary>
        /// Performs an implicit conversion from <see cref="OasRequestBodyBuilder"/> to <see cref="OasReferable{RequestBody}"/>.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator OasReferable<OasRequestBody>(OasRequestBodyBuilder builder) => builder?.Build();

        /// <summary>
        /// Performs an implicit conversion from <see cref="OasRequestBody"/> to <see cref="OasRequestBodyBuilder"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator OasRequestBodyBuilder(OasRequestBody value) => value is null ? null : new OasRequestBodyBuilder(value);

        /// <summary>
        /// Creates the <see cref="OasRequestBody"/> from this builder.
        /// </summary>
        /// <returns>The <see cref="OasRequestBody"/>.</returns>
        public virtual OasRequestBody Build() => new OasRequestBody(
            description: Description,
            content: new ReadOnlyDictionary<ContentType, OasMediaType>(Content),
            options: Options);
    }
}
