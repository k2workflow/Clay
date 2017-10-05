#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using SourceCode.Clay.Collections.Generic;
using System;
using System.Collections.Generic;
using System.Net.Mime;

namespace SourceCode.Clay.OpenApi
{
    /// <summary>
    /// Describes a single response from an API Operation.
    /// </summary>
    public class Response : IEquatable<Response>
    {
        #region Properties

        /// <summary>
        /// Gets the short description of the response.
        /// </summary>
        public string Description { get; }

        /// <summary>
        /// Gets the map of headers to their definition.
        /// </summary>
        public IReadOnlyDictionary<string, Referable<ParameterBody>> Headers { get; }

        /// <summary>
        /// Gets the map containing descriptions of potential response payloads.
        /// </summary>
        public IReadOnlyDictionary<ContentType, MediaType> Content { get; }

        /// <summary>
        /// Gets the map of operations links that can be followed from the response.
        /// </summary>
        public IReadOnlyDictionary<string, Referable<Link>> Links { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of the <see cref="Response"/> class.
        /// </summary>
        /// <param name="description">Theshort description of the response.</param>
        /// <param name="headers">The map of headers to their definition.</param>
        /// <param name="content">The map containing descriptions of potential response payloads.</param>
        /// <param name="links">The map of operations links that can be followed from the response.</param>
        public Response(
            string description = default,
            IReadOnlyDictionary<string, Referable<ParameterBody>> headers = default,
            IReadOnlyDictionary<ContentType, MediaType> content = default,
            IReadOnlyDictionary<string, Referable<Link>> links = default)
        {
            Description = description;
            Headers = headers ?? ReadOnlyDictionary.Empty<string, Referable<ParameterBody>>();
            Content = content ?? ReadOnlyDictionary.Empty<ContentType, MediaType>();
            Links = links ?? ReadOnlyDictionary.Empty<string, Referable<Link>>();
        }

        #endregion

        #region IEquatable

        public static bool operator ==(Response response1, Response response2)
        {
            if (ReferenceEquals(response1, null) && ReferenceEquals(response2, null)) return true;
            if (ReferenceEquals(response1, null) || ReferenceEquals(response2, null)) return false;
            return response1.Equals((object)response2);
        }

        public static bool operator !=(Response response1, Response response2) => !(response1 == response2);

        /// <summary>Determines whether the specified object is equal to the current object.</summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns>true if the specified object  is equal to the current object; otherwise, false.</returns>
        public override bool Equals(object obj) => Equals(obj as Response);

        /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>true if the current object is equal to the <paramref name="other">other</paramref> parameter; otherwise, false.</returns>
        public virtual bool Equals(Response other)
        {
            if (ReferenceEquals(other, null)) return false;
            if (ReferenceEquals(this, other)) return true;

            if (!StringComparer.Ordinal.Equals(Description, other.Description)) return false;
            if (!Headers.DictionaryEquals(other.Headers)) return false;
            if (!Content.DictionaryEquals(other.Content)) return false;
            if (!Links.DictionaryEquals(other.Links)) return false;

            return true;
        }

        /// <summary>Serves as the default hash function.</summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                var hc = 17L;

                if (Description != null) hc = hc * 21 + StringComparer.Ordinal.GetHashCode(Description);
                hc = hc * 21 + Headers.Count;
                hc = hc * 21 + Content.Count;
                hc = hc * 21 + Links.Count;

                return ((int)(hc >> 32)) ^ (int)hc;
            }
        }

        #endregion
    }
}
