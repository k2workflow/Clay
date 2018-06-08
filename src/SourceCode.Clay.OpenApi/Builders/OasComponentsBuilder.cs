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
    public class OasComponentsBuilder : IOasBuilder<OasComponents>
    {
        /// <summary>
        /// Gets the object that holds reusable <see cref="OasSchema"/> instances.
        /// </summary>
        public IDictionary<string, OasReferable<OasSchema>> Schemas { get; }

        /// <summary>
        /// Gets the object that holds reusable <see cref="OasResponse"/> instances.
        /// </summary>
        public IDictionary<string, OasReferable<OasResponse>> Responses { get; }

        /// <summary>
        /// Gets the object that holds reusable <see cref="OasParameter"/> instances.
        /// </summary>
        public IDictionary<string, OasReferable<OasParameter>> Parameters { get; }

        /// <summary>
        /// Gets the object that holds reusable <see cref="OasExample"/> instances.
        /// </summary>
        public IDictionary<string, OasReferable<OasExample>> Examples { get; }

        /// <summary>
        /// Gets the object that holds reusable <see cref="OasRequestBody"/> instances.
        /// </summary>
        public IDictionary<string, OasReferable<OasRequestBody>> RequestBodies { get; }

        /// <summary>
        /// Gets the object that holds reusable header <see cref="OasParameterBody"/> instances.
        /// </summary>
        public IDictionary<string, OasReferable<OasParameterBody>> Headers { get; }

        /// <summary>
        /// Gets the object that holds reusable <see cref="OasSecurityScheme"/> instances.
        /// </summary>
        public IDictionary<string, OasReferable<OasSecurityScheme>> SecuritySchemes { get; }

        /// <summary>
        /// Gets the object that holds reusable <see cref="OasLink"/> instances.
        /// </summary>
        public IDictionary<string, OasReferable<OasLink>> Links { get; }

        /// <summary>
        /// Gets the object that holds reusable <see cref="OasCallback"/> instances.
        /// </summary>
        public IDictionary<string, OasReferable<OasCallback>> Callbacks { get; }

        /// <summary>
        /// Creates a new instance of the <see cref="OasComponentsBuilder"/> class.
        /// </summary>
        public OasComponentsBuilder()
        {
            Schemas = new Dictionary<string, OasReferable<OasSchema>>(OasComponentKeyStringComparer.ComponentKey);
            Responses = new Dictionary<string, OasReferable<OasResponse>>(OasComponentKeyStringComparer.ComponentKey);
            Parameters = new Dictionary<string, OasReferable<OasParameter>>(OasComponentKeyStringComparer.ComponentKey);
            Examples = new Dictionary<string, OasReferable<OasExample>>(OasComponentKeyStringComparer.ComponentKey);
            RequestBodies = new Dictionary<string, OasReferable<OasRequestBody>>(OasComponentKeyStringComparer.ComponentKey);
            Headers = new Dictionary<string, OasReferable<OasParameterBody>>(OasComponentKeyStringComparer.ComponentKey);
            SecuritySchemes = new Dictionary<string, OasReferable<OasSecurityScheme>>(OasComponentKeyStringComparer.ComponentKey);
            Links = new Dictionary<string, OasReferable<OasLink>>(OasComponentKeyStringComparer.ComponentKey);
            Callbacks = new Dictionary<string, OasReferable<OasCallback>>(OasComponentKeyStringComparer.ComponentKey);
        }

        /// <summary>
        /// Creates a new instance of the <see cref="OasComponentsBuilder"/> class.
        /// </summary>
        /// <param name="value">The <see cref="OasComponents"/> to copy values from.</param>
        public OasComponentsBuilder(OasComponents value)
        {
            if (value is null) throw new ArgumentNullException(nameof(value));
            Schemas = new Dictionary<string, OasReferable<OasSchema>>(value.Schemas);
            Responses = new Dictionary<string, OasReferable<OasResponse>>(value.Responses);
            Parameters = new Dictionary<string, OasReferable<OasParameter>>(value.Parameters);
            Examples = new Dictionary<string, OasReferable<OasExample>>(value.Examples);
            RequestBodies = new Dictionary<string, OasReferable<OasRequestBody>>(value.RequestBodies);
            Headers = new Dictionary<string, OasReferable<OasParameterBody>>(value.Headers);
            SecuritySchemes = new Dictionary<string, OasReferable<OasSecurityScheme>>(value.SecuritySchemes);
            Links = new Dictionary<string, OasReferable<OasLink>>(value.Links);
            Callbacks = new Dictionary<string, OasReferable<OasCallback>>(value.Callbacks);
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="OasComponentsBuilder"/> to <see cref="OasComponents"/>.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator OasComponents(OasComponentsBuilder builder) => builder?.Build();

        /// <summary>
        /// Performs an implicit conversion from <see cref="OasComponents"/> to <see cref="OasComponentsBuilder"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator OasComponentsBuilder(OasComponents value) => value is null ? null : new OasComponentsBuilder(value);

        /// <summary>
        /// Creates the <see cref="OasComponents"/> from this builder.
        /// </summary>
        /// <returns>The <see cref="OasComponents"/>.</returns>
        public virtual OasComponents Build() => new OasComponents(
            schemas: new ReadOnlyDictionary<string, OasReferable<OasSchema>>(Schemas),
            responses: new ReadOnlyDictionary<string, OasReferable<OasResponse>>(Responses),
            parameters: new ReadOnlyDictionary<string, OasReferable<OasParameter>>(Parameters),
            examples: new ReadOnlyDictionary<string, OasReferable<OasExample>>(Examples),
            requestBodies: new ReadOnlyDictionary<string, OasReferable<OasRequestBody>>(RequestBodies),
            headers: new ReadOnlyDictionary<string, OasReferable<OasParameterBody>>(Headers),
            securitySchemes: new ReadOnlyDictionary<string, OasReferable<OasSecurityScheme>>(SecuritySchemes),
            links: new ReadOnlyDictionary<string, OasReferable<OasLink>>(Links),
            callbacks: new ReadOnlyDictionary<string, OasReferable<OasCallback>>(Callbacks));
    }
}
