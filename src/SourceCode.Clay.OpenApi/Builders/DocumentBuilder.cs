#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace SourceCode.Clay.OpenApi
{
    /// <summary>
    /// This is the root document object of the OpenAPI document.
    /// </summary>
    public class DocumentBuilder : IOpenApiBuilder<Document>
    {
        #region Properties

        /// <summary>
        /// Gets or sets the semantic version number of the OpenAPI Specification version that the OpenAPI document uses.
        /// </summary>
        public SemanticVersion? Version { get; set; }

        /// <summary>
        /// Gets or sets the metadata about the API.
        /// </summary>
        public Information Info { get; set; }

        /// <summary>
        /// Gets the list of <see cref="Server"/> instances, which provide connectivity information to a target server.
        /// </summary>
        public IList<Server> Servers { get; }

        /// <summary>
        /// Gets the available paths and operations for the API.
        /// </summary>
        public IDictionary<string, Referable<Path>> Paths { get; }

        /// <summary>
        /// Gets or sets the list that holds various schemas for the specification.
        /// </summary>
        public Components Components { get; set; }

        /// <summary>
        /// Gets the declaration of which security mechanisms can be used across the API.
        /// </summary>
        public IList<Referable<SecurityScheme>> Security { get; }

        /// <summary>
        /// Gets the list of tags used by the specification with additional metadata.
        /// </summary>
        public IList<Tag> Tags { get; }

        /// <summary>
        /// Gets or sets the external documentation.
        /// </summary>
        public ExternalDocumentation ExternalDocumentation { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of the <see cref="DocumentBuilder"/> class.
        /// </summary>
        public DocumentBuilder()
        {
            Servers = new List<Server>();
            Paths = new Dictionary<string, Referable<Path>>();
            Security = new List<Referable<SecurityScheme>>();
            Tags = new List<Tag>();
        }

        /// <summary>
        /// Creates a new instance of the <see cref="DocumentBuilder"/> class.
        /// </summary>
        /// <param name="value">The <see cref="Document"/> to copy values from.</param>
        public DocumentBuilder(Document value)
        {
            if (ReferenceEquals(value, null)) throw new ArgumentNullException(nameof(value));
            Version = value.Version;
            Info = value.Info;
            Servers = new List<Server>(value.Servers);
            Paths = new Dictionary<string, Referable<Path>>(value.Paths);
            Components = value.Components;
            Security = new List<Referable<SecurityScheme>>(value.Security);
            Tags = new List<Tag>(value.Tags);
            ExternalDocumentation = value.ExternalDocumentation;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Performs an implicit conversion from <see cref="DocumentBuilder"/> to <see cref="Document"/>.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator Document(DocumentBuilder builder) => builder?.Build();

        /// <summary>
        /// Performs an implicit conversion from <see cref="Document"/> to <see cref="DocumentBuilder"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator DocumentBuilder(Document value) => ReferenceEquals(value, null) ? null : new DocumentBuilder(value);

        /// <summary>
        /// Creates the <see cref="Document"/> from this builder.
        /// </summary>
        /// <returns>The <see cref="Document"/>.</returns>
        public Document Build() => new Document(
            version: Version,
            info: Info,
            servers: new ReadOnlyCollection<Server>(Servers),
            paths: new ReadOnlyDictionary<string, Referable<Path>>(Paths),
            components: Components,
            security: new ReadOnlyCollection<Referable<SecurityScheme>>(Security),
            tags: new ReadOnlyCollection<Tag>(Tags),
            externalDocumentation: ExternalDocumentation);

        #endregion
    }
}
