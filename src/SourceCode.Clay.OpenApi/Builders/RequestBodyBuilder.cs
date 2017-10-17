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
    public class RequestBodyBuilder : IOpenApiBuilder<RequestBody>
    {
        #region Properties

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
        public IDictionary<ContentType, MediaType> Content { get; }

        /// <summary>
        /// Gets or sets the request body options.
        /// </summary>
        public RequestBodyOptions Options { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of the <see cref="RequestBodyBuilder"/> class.
        /// </summary>
        public RequestBodyBuilder()
        {
            Content = new Dictionary<ContentType, MediaType>();
        }

        /// <summary>
        /// Creates a new instance of the <see cref="RequestBodyBuilder"/> class.
        /// </summary>
        /// <param name="value">The <see cref="RequestBody"/> to copy values from.</param>
        public RequestBodyBuilder(RequestBody value)
        {
            if (value is null) throw new ArgumentNullException(nameof(value));
            Description = value.Description;
            Content = new Dictionary<ContentType, MediaType>(value.Content);
            Options = value.Options;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Performs an implicit conversion from <see cref="RequestBodyBuilder"/> to <see cref="RequestBody"/>.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator RequestBody(RequestBodyBuilder builder) => builder?.Build();

        /// <summary>
        /// Performs an implicit conversion from <see cref="RequestBodyBuilder"/> to <see cref="Referable{RequestBody}"/>.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator Referable<RequestBody>(RequestBodyBuilder builder) => builder?.Build();

        /// <summary>
        /// Performs an implicit conversion from <see cref="RequestBody"/> to <see cref="RequestBodyBuilder"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator RequestBodyBuilder(RequestBody value) => value is null ? null : new RequestBodyBuilder(value);

        /// <summary>
        /// Creates the <see cref="RequestBody"/> from this builder.
        /// </summary>
        /// <returns>The <see cref="RequestBody"/>.</returns>
        public RequestBody Build() => new RequestBody(
            description: Description,
            content: new ReadOnlyDictionary<ContentType, MediaType>(Content),
            options: Options);

        #endregion
    }
}
