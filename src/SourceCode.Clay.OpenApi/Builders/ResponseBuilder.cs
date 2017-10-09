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
    public class ResponseBuilder : IOpenApiBuilder<Response>
    {
        #region Properties

        /// <summary>
        /// Gets or sets the short description of the response.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets the map of headers to their definition.
        /// </summary>
        public IDictionary<string, Referable<ParameterBody>> Headers { get; }

        /// <summary>
        /// Gets the map containing descriptions of potential response payloads.
        /// </summary>
        public IDictionary<ContentType, MediaType> Content { get; }

        /// <summary>
        /// Gets the map of operations links that can be followed from the response.
        /// </summary>
        public IDictionary<string, Referable<Link>> Links { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of the <see cref="ResponseBuilder"/> class.
        /// </summary>
        public ResponseBuilder()
        {
            Headers = new Dictionary<string, Referable<ParameterBody>>();
            Content = new Dictionary<ContentType, MediaType>();
            Links = new Dictionary<string, Referable<Link>>();
        }

        /// <summary>
        /// Creates a new instance of the <see cref="ResponseBuilder"/> class.
        /// </summary>
        /// <param name="value">The <see cref="Response"/> to copy values from.</param>
        public ResponseBuilder(Response value)
        {
            if (ReferenceEquals(value, null)) throw new ArgumentNullException(nameof(value));
            Description = value.Description;
            Headers = new Dictionary<string, Referable<ParameterBody>>(value.Headers);
            Content = new Dictionary<ContentType, MediaType>(value.Content);
            Links = new Dictionary<string, Referable<Link>>(value.Links);
        }

        #endregion

        #region Methods

        public static implicit operator Response(ResponseBuilder builder) => builder?.Build();

        public static implicit operator ResponseBuilder(Response value) => ReferenceEquals(value, null) ? null : new ResponseBuilder(value);

        /// <summary>
        /// Creates the <see cref="Link"/> from this builder.
        /// </summary>
        /// <returns>The <see cref="Link"/>.</returns>
        public Response Build() => new Response(
            description: Description,
            headers: new ReadOnlyDictionary<string, Referable<ParameterBody>>(Headers),
            content: new ReadOnlyDictionary<ContentType, MediaType>(Content),
            links: new ReadOnlyDictionary<string, Referable<Link>>(Links));

        #endregion
    }
}
