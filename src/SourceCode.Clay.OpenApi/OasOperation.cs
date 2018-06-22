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
    /// Describes a single API operation on a path.
    /// </summary>
    public class OasOperation : IEquatable<OasOperation>
    {
        /// <summary>
        /// Gets the list of tags for API documentation control.
        /// </summary>
        public IReadOnlyList<string> Tags { get; }

        /// <summary>
        /// Gets the short summary of what the operation does.
        /// </summary>
        public string Summary { get; }

        /// <summary>
        /// Gets the verbose explanation of the operation behavior.
        /// </summary>
        /// <remarks>
        /// CommonMark syntax MAY be used for rich text representation.
        /// </remarks>
        public string Description { get; }

        /// <summary>
        /// Gets the additional external documentation for this operation.
        /// </summary>
        public OasExternalDocumentation ExternalDocumentation { get; }

        /// <summary>
        /// Gets the unique string used to identify the operation.
        /// </summary>
        public string OperationIdentifier { get; }

        /// <summary>
        /// Gets the list of parameters that are applicable for this operation.
        /// </summary>
        public IReadOnlyDictionary<OasParameterKey, OasReferable<OasParameterBody>> Parameters { get; }

        /// <summary>
        /// Gets the request body applicable for this operation.
        /// </summary>
        public OasReferable<OasRequestBody> RequestBody { get; }

        /// <summary>
        /// Gets the list of possible responses as they are returned from executing this operation.
        /// </summary>
        public IReadOnlyDictionary<OasResponseKey, OasReferable<OasResponse>> Responses { get; }

        /// <summary>
        /// Gets the map of possible out-of band callbacks related to the parent operation.
        /// </summary>
        public IReadOnlyDictionary<string, OasReferable<OasCallback>> Callbacks { get; }

        /// <summary>
        /// Gets the flags that indicate what options are set on the operation.
        /// </summary>
        public OasOperationOptions Options { get; }

        /// <summary>
        /// Gets the declaration of which security mechanisms can be used for this operation.
        /// </summary>
        public IReadOnlyList<OasSecurityScheme> Security { get; }

        /// <summary>
        /// Gets the alternative server list to service this operation.
        /// </summary>
        public IReadOnlyList<OasServer> Servers { get; }

        /// <summary>
        /// Creates a new instance of the <see cref="OasOperation"/> class.
        /// </summary>
        /// <param name="tags">The list of tags for API documentation control.</param>
        /// <param name="summary">The short summary of what the operation does.</param>
        /// <param name="description">The verbose explanation of the operation behavior.</param>
        /// <param name="externalDocumentation">The additional external documentation for this operation.</param>
        /// <param name="operationIdentifier">The unique string used to identify the operation.</param>
        /// <param name="parameters">The list of parameters that are applicable for this operation.</param>
        /// <param name="requestBody">The request body applicable for this operation.</param>
        /// <param name="responses">The list of possible responses as they are returned from executing this operation.</param>
        /// <param name="callbacks">The map of possible out-of band callbacks related to the parent operation.</param>
        /// <param name="options">The flags that indicate what options are set on the operation.</param>
        /// <param name="security">The declaration of which security mechanisms can be used for this operation.</param>
        /// <param name="servers">The alternative server list to service this operation.</param>
        public OasOperation(
            IReadOnlyList<string> tags = default,
            string summary = default,
            string description = default,
            OasExternalDocumentation externalDocumentation = default,
            string operationIdentifier = default,
            IReadOnlyDictionary<OasParameterKey, OasReferable<OasParameterBody>> parameters = default,
            OasReferable<OasRequestBody> requestBody = default,
            IReadOnlyDictionary<OasResponseKey, OasReferable<OasResponse>> responses = default,
            IReadOnlyDictionary<string, OasReferable<OasCallback>> callbacks = default,
            OasOperationOptions options = default,
            IReadOnlyList<OasSecurityScheme> security = default,
            IReadOnlyList<OasServer> servers = default)
        {
            Tags = tags ?? Array.Empty<string>();
            Summary = summary;
            Description = description;
            ExternalDocumentation = externalDocumentation;
            OperationIdentifier = operationIdentifier;
            Parameters = parameters ?? ImmutableDictionary<OasParameterKey, OasReferable<OasParameterBody>>.Empty;
            RequestBody = requestBody;
            Responses = responses ?? ImmutableDictionary<OasResponseKey, OasReferable<OasResponse>>.Empty;
            Callbacks = callbacks ?? ImmutableDictionary<string, OasReferable<OasCallback>>.Empty;
            Options = options;
            Security = security ?? Array.Empty<OasSecurityScheme>();
            Servers = servers ?? Array.Empty<OasServer>();
        }

        /// <summary>
        /// Implements the operator == operator.
        /// </summary>
        /// <param name="operation1">The operation1.</param>
        /// <param name="operation2">The operation2.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator ==(OasOperation operation1, OasOperation operation2)
        {
            if (operation1 is null) return operation2 is null;
            return operation1.Equals((object)operation2);
        }

        /// <summary>
        /// Implements the operator != operator.
        /// </summary>
        /// <param name="operation1">The operation1.</param>
        /// <param name="operation2">The operation2.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator !=(OasOperation operation1, OasOperation operation2)
            => !(operation1 == operation2);

        /// <summary>Determines whether the specified object is equal to the current object.</summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns>true if the specified object  is equal to the current object; otherwise, false.</returns>
        public override bool Equals(object obj)
            => obj is OasOperation other
            && Equals(other);

        /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>true if the current object is equal to the <paramref name="other">other</paramref> parameter; otherwise, false.</returns>
        public virtual bool Equals(OasOperation other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;

            if (!Tags.NullableSetEquals(other.Tags)) return false;
            if (!StringComparer.Ordinal.Equals(Summary, other.Summary)) return false;
            if (!StringComparer.Ordinal.Equals(Description, other.Description)) return false;
            if (!ExternalDocumentation.NullableEquals(other.ExternalDocumentation)) return false;
            if (!StringComparer.Ordinal.Equals(OperationIdentifier, other.OperationIdentifier)) return false;
            if (!Parameters.NullableDictionaryEquals(other.Parameters)) return false;
            if (!RequestBody.Equals(other.RequestBody)) return false;
            if (!Responses.NullableDictionaryEquals(other.Responses)) return false;
            if (!Callbacks.NullableDictionaryEquals(other.Callbacks)) return false;
            if (Options != other.Options) return false;
            if (!Security.NullableSetEquals(other.Security)) return false;
            if (!Servers.NullableSequenceEqual(other.Servers)) return false;

            return true;
        }

        /// <summary>Serves as the default hash function.</summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode()
        {
            var hc = new HashCode();

            hc.Add(Tags.Count);
            hc.Add(Summary ?? string.Empty, StringComparer.Ordinal);
            hc.Add(Description ?? string.Empty, StringComparer.Ordinal);
            hc.Add(OperationIdentifier ?? string.Empty, StringComparer.Ordinal);
            hc.Add(Parameters.Count);
            hc.Add(Responses.Count);
            hc.Add(Callbacks.Count);
            hc.Add(Security.Count);
            hc.Add(Servers.Count);

            return hc.ToHashCode();
        }
    }
}
