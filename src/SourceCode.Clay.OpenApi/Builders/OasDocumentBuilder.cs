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
    public class OasDocumentBuilder : IOasBuilder<OasDocument>
    {
        #region Properties

        /// <summary>
        /// Gets or sets the semantic version number of the OpenAPI Specification version that the OpenAPI document uses.
        /// </summary>
        public SemanticVersion? Version { get; set; }

        /// <summary>
        /// Gets or sets the metadata about the API.
        /// </summary>
        public OasInformation Info { get; set; }

        /// <summary>
        /// Gets the list of <see cref="OasServer"/> instances, which provide connectivity information to a target server.
        /// </summary>
        public IList<OasServer> Servers { get; }

        /// <summary>
        /// Gets the available paths and operations for the API.
        /// </summary>
        public IDictionary<string, OasReferable<OasPath>> Paths { get; }

        /// <summary>
        /// Gets or sets the list that holds various schemas for the specification.
        /// </summary>
        public OasComponents Components { get; set; }

        /// <summary>
        /// Gets the declaration of which security mechanisms can be used across the API.
        /// </summary>
        public IList<OasReferable<OasSecurityScheme>> Security { get; }

        /// <summary>
        /// Gets the list of tags used by the specification with additional metadata.
        /// </summary>
        public IList<OasTag> Tags { get; }

        /// <summary>
        /// Gets or sets the external documentation.
        /// </summary>
        public OasExternalDocumentation ExternalDocumentation { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of the <see cref="OasDocumentBuilder"/> class.
        /// </summary>
        public OasDocumentBuilder()
        {
            Servers = new List<OasServer>();
            Paths = new Dictionary<string, OasReferable<OasPath>>();
            Security = new List<OasReferable<OasSecurityScheme>>();
            Tags = new List<OasTag>();
            Version = new SemanticVersion(3, 0, 0);
        }

        /// <summary>
        /// Creates a new instance of the <see cref="OasDocumentBuilder"/> class.
        /// </summary>
        /// <param name="value">The <see cref="OasDocument"/> to copy values from.</param>
        public OasDocumentBuilder(OasDocument value)
        {
            if (value is null) throw new ArgumentNullException(nameof(value));
            Version = value.Version;
            Info = value.Info;
            Servers = new List<OasServer>(value.Servers);
            Paths = new Dictionary<string, OasReferable<OasPath>>(value.Paths);
            Components = value.Components;
            Security = new List<OasReferable<OasSecurityScheme>>(value.Security);
            Tags = new List<OasTag>(value.Tags);
            ExternalDocumentation = value.ExternalDocumentation;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Performs an implicit conversion from <see cref="OasDocumentBuilder"/> to <see cref="OasDocument"/>.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator OasDocument(OasDocumentBuilder builder) => builder?.Build();

        /// <summary>
        /// Performs an implicit conversion from <see cref="OasDocument"/> to <see cref="OasDocumentBuilder"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator OasDocumentBuilder(OasDocument value) => value is null ? null : new OasDocumentBuilder(value);

        /// <summary>
        /// Creates the <see cref="OasDocument"/> from this builder.
        /// </summary>
        /// <returns>The <see cref="OasDocument"/>.</returns>
        public OasDocument Build() => new OasDocument(
            version: Version,
            info: Info,
            servers: new ReadOnlyCollection<OasServer>(Servers),
            paths: new ReadOnlyDictionary<string, OasReferable<OasPath>>(Paths),
            components: Components,
            security: new ReadOnlyCollection<OasReferable<OasSecurityScheme>>(Security),
            tags: new ReadOnlyCollection<OasTag>(Tags),
            externalDocumentation: ExternalDocumentation);

        #endregion
    }
}
