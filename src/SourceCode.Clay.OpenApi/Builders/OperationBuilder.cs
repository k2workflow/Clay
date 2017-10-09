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
    public class OperationBuilder : IBuilder<Operation>
    {
        #region Properties

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
        public ExternalDocumentation ExternalDocumentation { get; set; }

        /// <summary>
        /// Gets or sets the unique string used to identify the operation.
        /// </summary>
        public string OperationIdentifier { get; set; }

        /// <summary>
        /// Gets the list of parameters that are applicable for this operation.
        /// </summary>
        public IDictionary<ParameterKey, Referable<Parameter>> Parameters { get; }

        /// <summary>
        /// Gets or sets the request body applicable for this operation.
        /// </summary>
        public Referable<RequestBody> RequestBody { get; set; }

        /// <summary>
        /// Gets the list of possible responses as they are returned from executing this operation.
        /// </summary>
        public IDictionary<ResponseKey, Referable<Response>> Responses { get; }

        /// <summary>
        /// Gets the map of possible out-of band callbacks related to the parent operation.
        /// </summary>
        public IDictionary<string, Referable<Callback>> Callbacks { get; }

        /// <summary>
        /// Gets or sets the flags that indicate what options are set on the operation.
        /// </summary>
        public OperationOptions Options { get; set; }

        /// <summary>
        /// Gets the declaration of which security mechanisms can be used for this operation.
        /// </summary>
        public IList<SecurityScheme> Security { get; }

        /// <summary>
        /// Gets the alternative server list to service this operation.
        /// </summary>
        public IList<Server> Servers { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of the <see cref="OperationBuilder"/> class.
        /// </summary>
        public OperationBuilder()
        {
            Tags = new List<string>();
            Parameters = new Dictionary<ParameterKey, Referable<Parameter>>();
            Responses = new Dictionary<ResponseKey, Referable<Response>>();
            Callbacks = new Dictionary<string, Referable<Callback>>();
            Security = new List<SecurityScheme>();
            Servers = new List<Server>();
        }

        /// <summary>
        /// Creates a new instance of the <see cref="OperationBuilder"/> class.
        /// </summary>
        /// <param name="value">The <see cref="Operation"/> to copy values from.</param>
        public OperationBuilder(Operation value)
        {
            if (ReferenceEquals(value, null)) throw new ArgumentNullException(nameof(value));
            Tags = new List<string>(value.Tags);
            Summary = value.Summary;
            Description = value.Description;
            ExternalDocumentation = value.ExternalDocumentation;
            OperationIdentifier = value.OperationIdentifier;
            Parameters = new Dictionary<ParameterKey, Referable<Parameter>>(value.Parameters);
            RequestBody = value.RequestBody;
            Responses = new Dictionary<ResponseKey, Referable<Response>>(value.Responses);
            Callbacks = new Dictionary<string, Referable<Callback>>(value.Callbacks);
            Options = value.Options;
            Security = new List<SecurityScheme>(value.Security);
            Servers = new List<Server>(value.Servers);
        }

        #endregion

        #region Methods

        public static implicit operator Operation(OperationBuilder builder) => builder?.Build();

        public static implicit operator OperationBuilder(Operation value) => ReferenceEquals(value, null) ? null : new OperationBuilder(value);

        /// <summary>
        /// Creates the <see cref="Link"/> from this builder.
        /// </summary>
        /// <returns>The <see cref="Link"/>.</returns>
        public Operation Build() => new Operation(
            tags: new ReadOnlyCollection<string>(Tags),
            summary: Summary,
            description: Description,
            externalDocumentation: ExternalDocumentation,
            operationIdentifier: OperationIdentifier,
            parameters: new ReadOnlyDictionary<ParameterKey, Referable<Parameter>>(Parameters),
            requestBody: RequestBody,
            responses: new ReadOnlyDictionary<ResponseKey, Referable<Response>>(Responses),
            callbacks: new ReadOnlyDictionary<string, Referable<Callback>>(Callbacks),
            options: Options,
            security: new ReadOnlyCollection<SecurityScheme>(Security),
            servers: new ReadOnlyCollection<Server>(Servers));

        #endregion
    }
}
