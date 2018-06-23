#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using SourceCode.Clay.Collections.Generic;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace SourceCode.Clay.OpenApi
{
    /// <summary>
    /// Holds a set of reusable objects for different aspects of the OAS. All objects
    /// defined within the components object will have no effect on the API unless they
    /// are explicitly referenced from properties outside the components object.
    /// </summary>
    public class OasComponents : IEquatable<OasComponents>
    {
        /// <summary>
        /// Gets the object that holds reusable <see cref="OasSchema"/> instances.
        /// </summary>
        public IReadOnlyDictionary<string, OasReferable<OasSchema>> Schemas { get; }

        /// <summary>
        /// Gets the object that holds reusable <see cref="OasResponse"/> instances.
        /// </summary>
        public IReadOnlyDictionary<string, OasReferable<OasResponse>> Responses { get; }

        /// <summary>
        /// Gets the object that holds reusable <see cref="OasParameter"/> instances.
        /// </summary>
        public IReadOnlyDictionary<string, OasReferable<OasParameter>> Parameters { get; }

        /// <summary>
        /// Gets the object that holds reusable <see cref="OasExample"/> instances.
        /// </summary>
        public IReadOnlyDictionary<string, OasReferable<OasExample>> Examples { get; }

        /// <summary>
        /// Gets the object that holds reusable <see cref="OasRequestBody"/> instances.
        /// </summary>
        public IReadOnlyDictionary<string, OasReferable<OasRequestBody>> RequestBodies { get; }

        /// <summary>
        /// Gets the object that holds reusable header <see cref="OasParameterBody"/> instances.
        /// </summary>
        public IReadOnlyDictionary<string, OasReferable<OasParameterBody>> Headers { get; }

        /// <summary>
        /// Gets the object that holds reusable <see cref="OasSecurityScheme"/> instances.
        /// </summary>
        public IReadOnlyDictionary<string, OasReferable<OasSecurityScheme>> SecuritySchemes { get; }

        /// <summary>
        /// Gets the object that holds reusable <see cref="OasLink"/> instances.
        /// </summary>
        public IReadOnlyDictionary<string, OasReferable<OasLink>> Links { get; }

        /// <summary>
        /// Gets the object that holds reusable <see cref="OasCallback"/> instances.
        /// </summary>
        public IReadOnlyDictionary<string, OasReferable<OasCallback>> Callbacks { get; }

        /// <summary>
        /// Creates a new instance of the <see cref="OasComponents"/> class.
        /// </summary>
        /// <param name="schemas">The object that holds reusable <see cref="OasSchema"/> instances.</param>
        /// <param name="responses">The object that holds reusable <see cref="OasResponse"/> instances.</param>
        /// <param name="parameters">The object that holds reusable <see cref="OasParameterBody"/> instances.</param>
        /// <param name="examples">The object that holds reusable <see cref="OasExample"/> instances.</param>
        /// <param name="requestBodies">The object that holds reusable <see cref="OasRequestBody"/> instances.</param>
        /// <param name="headers">The object that holds reusable header <see cref="OasParameterBody"/> instances.</param>
        /// <param name="securitySchemes">The object that holds reusable <see cref="OasSecurityScheme"/> instances.</param>
        /// <param name="links">The object that holds reusable <see cref="OasLink"/> instances.</param>
        /// <param name="callbacks">The object that holds reusable <see cref="OasCallback"/> instances.</param>
        public OasComponents(
            IReadOnlyDictionary<string, OasReferable<OasSchema>> schemas = default,
            IReadOnlyDictionary<string, OasReferable<OasResponse>> responses = default,
            IReadOnlyDictionary<string, OasReferable<OasParameter>> parameters = default,
            IReadOnlyDictionary<string, OasReferable<OasExample>> examples = default,
            IReadOnlyDictionary<string, OasReferable<OasRequestBody>> requestBodies = default,
            IReadOnlyDictionary<string, OasReferable<OasParameterBody>> headers = default,
            IReadOnlyDictionary<string, OasReferable<OasSecurityScheme>> securitySchemes = default,
            IReadOnlyDictionary<string, OasReferable<OasLink>> links = default,
            IReadOnlyDictionary<string, OasReferable<OasCallback>> callbacks = default)
        {
            Schemas = schemas ?? ImmutableDictionary<string, OasReferable<OasSchema>>.Empty;
            Responses = responses ?? ImmutableDictionary<string, OasReferable<OasResponse>>.Empty;
            Parameters = parameters ?? ImmutableDictionary<string, OasReferable<OasParameter>>.Empty;
            Examples = examples ?? ImmutableDictionary<string, OasReferable<OasExample>>.Empty;
            RequestBodies = requestBodies ?? ImmutableDictionary<string, OasReferable<OasRequestBody>>.Empty;
            Headers = headers ?? ImmutableDictionary<string, OasReferable<OasParameterBody>>.Empty;
            SecuritySchemes = securitySchemes ?? ImmutableDictionary<string, OasReferable<OasSecurityScheme>>.Empty;
            Links = links ?? ImmutableDictionary<string, OasReferable<OasLink>>.Empty;
            Callbacks = callbacks ?? ImmutableDictionary<string, OasReferable<OasCallback>>.Empty;
        }

        /// <summary>
        /// Implements the operator == operator.
        /// </summary>
        /// <param name="components1">The components1.</param>
        /// <param name="components2">The components2.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator ==(OasComponents components1, OasComponents components2)
        {
            if (components1 is null) return components2 is null;
            return components1.Equals((object)components2);
        }

        /// <summary>
        /// Implements the operator != operator.
        /// </summary>
        /// <param name="components1">The components1.</param>
        /// <param name="components2">The components2.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator !=(OasComponents components1, OasComponents components2)
            => !(components1 == components2);

        /// <summary>Determines whether the specified object is equal to the current object.</summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns>true if the specified object  is equal to the current object; otherwise, false.</returns>
        public override bool Equals(object obj)
            => obj is OasComponents other
            && Equals(other);

        /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>true if the current object is equal to the <paramref name="other">other</paramref> parameter; otherwise, false.</returns>
        public virtual bool Equals(OasComponents other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;

            if (!Schemas.NullableDictionaryEqual(other.Schemas)) return false;
            if (!Responses.NullableDictionaryEqual(other.Responses)) return false;
            if (!Parameters.NullableDictionaryEqual(other.Parameters)) return false;
            if (!Examples.NullableDictionaryEqual(other.Examples)) return false;
            if (!RequestBodies.NullableDictionaryEqual(other.RequestBodies)) return false;
            if (!Headers.NullableDictionaryEqual(other.Headers)) return false;
            if (!SecuritySchemes.NullableDictionaryEqual(other.SecuritySchemes)) return false;
            if (!Links.NullableDictionaryEqual(other.Links)) return false;
            if (!Callbacks.NullableDictionaryEqual(other.Callbacks)) return false;

            return true;
        }

        /// <summary>Serves as the default hash function.</summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode()
        {
            var hc = new HashCode();

            hc.Add(Schemas.Count);
            hc.Add(Responses.Count);
            hc.Add(Parameters.Count);
            hc.Add(Examples.Count);
            hc.Add(RequestBodies.Count);
            hc.Add(Headers.Count);
            hc.Add(SecuritySchemes.Count);
            hc.Add(Links.Count);
            hc.Add(Callbacks.Count);

            return hc.ToHashCode();
        }
    }
}
