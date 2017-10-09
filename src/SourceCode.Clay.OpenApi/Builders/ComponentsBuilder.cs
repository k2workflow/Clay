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
    /// Holds a set of reusable objects for different aspects of the OAS. All objects
    /// defined within the components object will have no effect on the API unless they
    /// are explicitly referenced from properties outside the components object.
    /// </summary>
    public class ComponentsBuilder : IBuilder<Components>
    {
        #region Properties

        /// <summary>
        /// Gets the object that holds reusable <see cref="Schema"/> instances.
        /// </summary>
        public IDictionary<string, Referable<Schema>> Schemas { get; }

        /// <summary>
        /// Gets the object that holds reusable <see cref="Response"/> instances.
        /// </summary>
        public IDictionary<string, Referable<Response>> Responses { get; }

        /// <summary>
        /// Gets the object that holds reusable <see cref="Parameter"/> instances.
        /// </summary>
        public IDictionary<string, Referable<Parameter>> Parameters { get; }

        /// <summary>
        /// Gets the object that holds reusable <see cref="Example"/> instances.
        /// </summary>
        public IDictionary<string, Referable<Example>> Examples { get; }

        /// <summary>
        /// Gets the object that holds reusable <see cref="RequestBody"/> instances.
        /// </summary>
        public IDictionary<string, Referable<RequestBody>> RequestBodies { get; }

        /// <summary>
        /// Gets the object that holds reusable header <see cref="ParameterBody"/> instances.
        /// </summary>
        public IDictionary<string, Referable<ParameterBody>> Headers { get; }

        /// <summary>
        /// Gets the object that holds reusable <see cref="SecurityScheme"/> instances.
        /// </summary>
        public IDictionary<string, Referable<SecurityScheme>> SecuritySchemes { get; }

        /// <summary>
        /// Gets the object that holds reusable <see cref="Link"/> instances.
        /// </summary>
        public IDictionary<string, Referable<Link>> Links { get; }

        /// <summary>
        /// Gets the object that holds reusable <see cref="Callback"/> instances.
        /// </summary>
        public IDictionary<string, Referable<Callback>> Callbacks { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of the <see cref="ComponentsBuilder"/> class.
        /// </summary>
        public ComponentsBuilder()
        {
        }

        /// <summary>
        /// Creates a new instance of the <see cref="ComponentsBuilder"/> class.
        /// </summary>
        /// <param name="value">The <see cref="Components"/> to copy values from.</param>
        public ComponentsBuilder(Components value)
        {
            if (ReferenceEquals(value, null)) throw new ArgumentNullException(nameof(value));
            Schemas = new Dictionary<string, Referable<Schema>>(value.Schemas);
            Responses = new Dictionary<string, Referable<Response>>(value.Responses);
            Parameters = new Dictionary<string, Referable<Parameter>>(value.Parameters);
            Examples = new Dictionary<string, Referable<Example>>(value.Examples);
            RequestBodies = new Dictionary<string, Referable<RequestBody>>(value.RequestBodies);
            Headers = new Dictionary<string, Referable<ParameterBody>>(value.Headers);
            SecuritySchemes = new Dictionary<string, Referable<SecurityScheme>>(value.SecuritySchemes);
            Links = new Dictionary<string, Referable<Link>>(value.Links);
            Callbacks = new Dictionary<string, Referable<Callback>>(value.Callbacks);
        }

        #endregion

        #region Methods

        public static implicit operator Components(ComponentsBuilder builder) => builder?.Build();

        public static implicit operator ComponentsBuilder(Components value) => ReferenceEquals(value, null) ? null : new ComponentsBuilder(value);

        /// <summary>
        /// Creates the <see cref="Components"/> from this builder.
        /// </summary>
        /// <returns>The <see cref="Components"/>.</returns>
        public Components Build() => new Components(
            schemas: new ReadOnlyDictionary<string, Referable<Schema>>(Schemas),
            responses: new ReadOnlyDictionary<string, Referable<Response>>(Responses),
            parameters: new ReadOnlyDictionary<string, Referable<Parameter>>(Parameters),
            examples: new ReadOnlyDictionary<string, Referable<Example>>(Examples),
            requestBodies: new ReadOnlyDictionary<string, Referable<RequestBody>>(RequestBodies),
            headers: new ReadOnlyDictionary<string, Referable<ParameterBody>>(Headers),
            securitySchemes: new ReadOnlyDictionary<string, Referable<SecurityScheme>>(SecuritySchemes),
            links: new ReadOnlyDictionary<string, Referable<Link>>(Links),
            callbacks: new ReadOnlyDictionary<string, Referable<Callback>>(Callbacks));

        #endregion
    }
}
