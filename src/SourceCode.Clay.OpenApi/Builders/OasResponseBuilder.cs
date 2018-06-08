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
    /// Describes a single response from an API Operation.
    /// </summary>
    public class OasResponseBuilder : IOasBuilder<OasResponse>
    {
        /// <summary>
        /// Gets or sets the short description of the response.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets the map of headers to their definition.
        /// </summary>
        public IDictionary<string, OasReferable<OasParameterBody>> Headers { get; }

        /// <summary>
        /// Gets the map containing descriptions of potential response payloads.
        /// </summary>
        public IDictionary<ContentType, OasMediaType> Content { get; }

        /// <summary>
        /// Gets the map of operations links that can be followed from the response.
        /// </summary>
        public IDictionary<string, OasReferable<OasLink>> Links { get; }

        /// <summary>
        /// Creates a new instance of the <see cref="OasResponseBuilder"/> class.
        /// </summary>
        public OasResponseBuilder()
        {
            Headers = new Dictionary<string, OasReferable<OasParameterBody>>();
            Content = new Dictionary<ContentType, OasMediaType>();
            Links = new Dictionary<string, OasReferable<OasLink>>();
        }

        /// <summary>
        /// Creates a new instance of the <see cref="OasResponseBuilder"/> class.
        /// </summary>
        /// <param name="value">The <see cref="OasResponse"/> to copy values from.</param>
        public OasResponseBuilder(OasResponse value)
        {
            if (value is null) throw new ArgumentNullException(nameof(value));
            Description = value.Description;
            Headers = new Dictionary<string, OasReferable<OasParameterBody>>(value.Headers);
            Content = new Dictionary<ContentType, OasMediaType>(value.Content);
            Links = new Dictionary<string, OasReferable<OasLink>>(value.Links);
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="OasResponseBuilder"/> to <see cref="OasResponse"/>.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator OasResponse(OasResponseBuilder builder) => builder?.Build();

        /// <summary>
        /// Performs an implicit conversion from <see cref="OasResponseBuilder"/> to <see cref="OasReferable{Response}"/>.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator OasReferable<OasResponse>(OasResponseBuilder builder) => builder?.Build();

        /// <summary>
        /// Performs an implicit conversion from <see cref="OasResponse"/> to <see cref="OasResponseBuilder"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator OasResponseBuilder(OasResponse value) => value is null ? null : new OasResponseBuilder(value);

        /// <summary>
        /// Creates the <see cref="OasLink"/> from this builder.
        /// </summary>
        /// <returns>The <see cref="OasLink"/>.</returns>
        public virtual OasResponse Build() => new OasResponse(
            description: Description,
            headers: new ReadOnlyDictionary<string, OasReferable<OasParameterBody>>(Headers),
            content: new ReadOnlyDictionary<ContentType, OasMediaType>(Content),
            links: new ReadOnlyDictionary<string, OasReferable<OasLink>>(Links));
    }
}
