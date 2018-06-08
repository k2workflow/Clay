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
    /// Provides schema and examples for the media type identified by its key.
    /// </summary>
    /// <remarks>
    /// The <c>Example</c> parameter is implementation-specific. If it is required, create a new type
    /// that inherits from this one.
    /// </remarks>
    public class OasMediaType : IEquatable<OasMediaType>
    {
        /// <summary>
        /// Gets the schema defining the type used for the request body.
        /// </summary>
        public OasReferable<OasSchema> Schema { get; }

        /// <summary>
        /// The examples of the media type.
        /// </summary>
        public IReadOnlyDictionary<string, OasReferable<OasExample>> Examples { get; }

        /// <summary>
        /// Gets the map between a property name and its encoding information.
        /// </summary>
        public IReadOnlyDictionary<string, OasPropertyEncoding> Encoding { get; }

        /// <summary>
        /// Creates a new instance of the <see cref="OasMediaType"/> class.
        /// </summary>
        /// <param name="schema">The schema defining the type used for the request body.</param>
        /// <param name="examples">Gets the examples of the media type.</param>
        /// <param name="encoding">Gets the map between a property name and its encoding information.</param>
        public OasMediaType(
            OasReferable<OasSchema> schema = default,
            IReadOnlyDictionary<string, OasReferable<OasExample>> examples = default,
            IReadOnlyDictionary<string, OasPropertyEncoding> encoding = default)
        {
            Schema = schema;
            Examples = examples ?? ImmutableDictionary<string, OasReferable<OasExample>>.Empty;
            Encoding = encoding ?? ImmutableDictionary<string, OasPropertyEncoding>.Empty;
        }

        /// <summary>
        /// Implements the operator == operator.
        /// </summary>
        /// <param name="mediaType1">The media type1.</param>
        /// <param name="mediaType2">The media type2.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator ==(OasMediaType mediaType1, OasMediaType mediaType2)
        {
            if (mediaType1 is null) return mediaType2 is null;
            return mediaType1.Equals((object)mediaType2);
        }

        /// <summary>
        /// Implements the operator != operator.
        /// </summary>
        /// <param name="mediaType1">The media type1.</param>
        /// <param name="mediaType2">The media type2.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator !=(OasMediaType mediaType1, OasMediaType mediaType2)
            => !(mediaType1 == mediaType2);

        /// <summary>Determines whether the specified object is equal to the current object.</summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns>true if the specified object  is equal to the current object; otherwise, false.</returns>
        public override bool Equals(object obj)
            => obj is OasMediaType other
            && Equals(other);

        /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>true if the current object is equal to the <paramref name="other">other</paramref> parameter; otherwise, false.</returns>
        public virtual bool Equals(OasMediaType other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;

            if (!Schema.Equals(other.Schema)) return false;
            if (!Examples.NullableDictionaryEquals(other.Examples)) return false;
            if (!Encoding.NullableDictionaryEquals(other.Encoding)) return false;

            return true;
        }

        /// <summary>Serves as the default hash function.</summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode() => HashCode.Combine(
            Schema,
            Examples.Count,
            Encoding.Count
        );
    }
}
