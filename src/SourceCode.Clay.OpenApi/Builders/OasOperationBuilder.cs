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
    /// Describes a single API operation on a path.
    /// </summary>
    public class OasOperationBuilder : IOasBuilder<OasOperation>
    {
        /// <summary>
        /// Gets the list of tags for API documentation control.
        /// </summary>
        public IList<string> Tags { get; }

        /// <summary>
        /// Gets or sets the short summary of what the operation does.
        /// </summary>
        public string Summary { get; set; }

        /// <summary>
        /// Gets or sets the verbose explanation of the operation behavior.
        /// </summary>
        /// <remarks>
        /// CommonMark syntax MAY be used for rich text representation.
        /// </remarks>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the additional external documentation for this operation.
        /// </summary>
        public OasExternalDocumentation ExternalDocumentation { get; set; }

        /// <summary>
        /// Gets or sets the unique string used to identify the operation.
        /// </summary>
        public string OperationIdentifier { get; set; }

        /// <summary>
        /// Gets the list of parameters that are applicable for this operation.
        /// </summary>
        public IDictionary<OasParameterKey, OasReferable<OasParameterBody>> Parameters { get; }

        /// <summary>
        /// Gets or sets the request body applicable for this operation.
        /// </summary>
        public OasReferable<OasRequestBody> RequestBody { get; set; }

        /// <summary>
        /// Gets the list of possible responses as they are returned from executing this operation.
        /// </summary>
        public IDictionary<OasResponseKey, OasReferable<OasResponse>> Responses { get; }

        /// <summary>
        /// Gets the map of possible out-of band callbacks related to the parent operation.
        /// </summary>
        public IDictionary<string, OasReferable<OasCallback>> Callbacks { get; }

        /// <summary>
        /// Gets or sets the flags that indicate what options are set on the operation.
        /// </summary>
        public OasOperationOptions Options { get; set; }

        /// <summary>
        /// Gets the declaration of which security mechanisms can be used for this operation.
        /// </summary>
        public IList<OasSecurityScheme> Security { get; }

        /// <summary>
        /// Gets the alternative server list to service this operation.
        /// </summary>
        public IList<OasServer> Servers { get; }

        /// <summary>
        /// Creates a new instance of the <see cref="OasOperationBuilder"/> class.
        /// </summary>
        public OasOperationBuilder()
        {
            Tags = new List<string>();
            Parameters = new Dictionary<OasParameterKey, OasReferable<OasParameterBody>>();
            Responses = new Dictionary<OasResponseKey, OasReferable<OasResponse>>();
            Callbacks = new Dictionary<string, OasReferable<OasCallback>>();
            Security = new List<OasSecurityScheme>();
            Servers = new List<OasServer>();
        }

        /// <summary>
        /// Creates a new instance of the <see cref="OasOperationBuilder"/> class.
        /// </summary>
        /// <param name="value">The <see cref="OasOperation"/> to copy values from.</param>
        public OasOperationBuilder(OasOperation value)
        {
            if (value is null) throw new ArgumentNullException(nameof(value));
            Tags = new List<string>(value.Tags);
            Summary = value.Summary;
            Description = value.Description;
            ExternalDocumentation = value.ExternalDocumentation;
            OperationIdentifier = value.OperationIdentifier;
            Parameters = new Dictionary<OasParameterKey, OasReferable<OasParameterBody>>(value.Parameters);
            RequestBody = value.RequestBody;
            Responses = new Dictionary<OasResponseKey, OasReferable<OasResponse>>(value.Responses);
            Callbacks = new Dictionary<string, OasReferable<OasCallback>>(value.Callbacks);
            Options = value.Options;
            Security = new List<OasSecurityScheme>(value.Security);
            Servers = new List<OasServer>(value.Servers);
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="OasOperationBuilder"/> to <see cref="OasOperation"/>.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator OasOperation(OasOperationBuilder builder) => builder?.Build();

        /// <summary>
        /// Performs an implicit conversion from <see cref="OasOperation"/> to <see cref="OasOperationBuilder"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator OasOperationBuilder(OasOperation value) => value is null ? null : new OasOperationBuilder(value);

        /// <summary>
        /// Creates the <see cref="OasLink"/> from this builder.
        /// </summary>
        /// <returns>The <see cref="OasLink"/>.</returns>
        public virtual OasOperation Build() => new OasOperation(
            tags: new ReadOnlyCollection<string>(Tags),
            summary: Summary,
            description: Description,
            externalDocumentation: ExternalDocumentation,
            operationIdentifier: OperationIdentifier,
            parameters: new ReadOnlyDictionary<OasParameterKey, OasReferable<OasParameterBody>>(Parameters),
            requestBody: RequestBody,
            responses: new ReadOnlyDictionary<OasResponseKey, OasReferable<OasResponse>>(Responses),
            callbacks: new ReadOnlyDictionary<string, OasReferable<OasCallback>>(Callbacks),
            options: Options,
            security: new ReadOnlyCollection<OasSecurityScheme>(Security),
            servers: new ReadOnlyCollection<OasServer>(Servers));
    }
}
