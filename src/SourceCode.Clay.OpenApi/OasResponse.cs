#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using SourceCode.Clay.Collections.Generic;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Net.Mime;

namespace SourceCode.Clay.OpenApi
{
    /// <summary>
    /// Describes a single response from an API Operation.
    /// </summary>
    public class OasResponse : IEquatable<OasResponse>
    {
        #region Properties

        /// <summary>
        /// Gets the short description of the response.
        /// </summary>
        public string Description { get; }

        /// <summary>
        /// Gets the map of headers to their definition.
        /// </summary>
        public IReadOnlyDictionary<string, OasReferable<OasParameterBody>> Headers { get; }

        /// <summary>
        /// Gets the map containing descriptions of potential response payloads.
        /// </summary>
        public IReadOnlyDictionary<ContentType, OasMediaType> Content { get; }

        /// <summary>
        /// Gets the map of operations links that can be followed from the response.
        /// </summary>
        public IReadOnlyDictionary<string, OasReferable<OasLink>> Links { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of the <see cref="OasResponse"/> class.
        /// </summary>
        /// <param name="description">Theshort description of the response.</param>
        /// <param name="headers">The map of headers to their definition.</param>
        /// <param name="content">The map containing descriptions of potential response payloads.</param>
        /// <param name="links">The map of operations links that can be followed from the response.</param>
        public OasResponse(
            string description = default,
            IReadOnlyDictionary<string, OasReferable<OasParameterBody>> headers = default,
            IReadOnlyDictionary<ContentType, OasMediaType> content = default,
            IReadOnlyDictionary<string, OasReferable<OasLink>> links = default)
        {
            Description = description;
            Headers = headers ?? ImmutableDictionary<string, OasReferable<OasParameterBody>>.Empty;
            Content = content ?? ImmutableDictionary<ContentType, OasMediaType>.Empty;
            Links = links ?? ImmutableDictionary<string, OasReferable<OasLink>>.Empty;
        }

        #endregion

        #region IEquatable

        /// <summary>
        /// Implements the operator == operator.
        /// </summary>
        /// <param name="response1">The response1.</param>
        /// <param name="response2">The response2.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator ==(OasResponse response1, OasResponse response2)
        {
            if (response1 is null) return response2 is null;
            return response1.Equals((object)response2);
        }

        /// <summary>
        /// Implements the operator != operator.
        /// </summary>
        /// <param name="response1">The response1.</param>
        /// <param name="response2">The response2.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator !=(OasResponse response1, OasResponse response2) => !(response1 == response2);

        /// <summary>Determines whether the specified object is equal to the current object.</summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns>true if the specified object  is equal to the current object; otherwise, false.</returns>
        public override bool Equals(object obj)
            => obj is OasResponse other
            && Equals(other);

        /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>true if the current object is equal to the <paramref name="other">other</paramref> parameter; otherwise, false.</returns>
        public virtual bool Equals(OasResponse other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;

            if (!StringComparer.Ordinal.Equals(Description, other.Description)) return false;
            if (!Headers.NullableDictionaryEquals(other.Headers)) return false;
            if (!Content.NullableDictionaryEquals(other.Content)) return false;
            if (!Links.NullableDictionaryEquals(other.Links)) return false;

            return true;
        }

        /// <summary>Serves as the default hash function.</summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode() => HashCode.Combine(
            StringComparer.Ordinal.GetHashCode(Description ?? string.Empty),
            Headers.Count,
            Content.Count,
            Links.Count
        );

        #endregion
    }
}
