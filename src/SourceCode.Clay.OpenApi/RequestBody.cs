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
    /// Describes a single request body.
    /// </summary>
    public class RequestBody : IEquatable<RequestBody>
    {
        #region Properties

        /// <summary>
        /// Gets the brief description of the request body.
        /// </summary>
        /// <remarks>
        /// CommonMark syntax MAY be used for rich text representation.
        /// </remarks>
        public string Description { get; }

        /// <summary>
        /// Gets the content of the request body.
        /// </summary>
        public IReadOnlyDictionary<ContentType, MediaType> Content { get; }

        /// <summary>
        /// Gets the request body options.
        /// </summary>
        public RequestBodyOptions Options { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of the <see cref="RequestBody"/> class.
        /// </summary>
        /// <param name="description">The brief description of the request body.</param>
        /// <param name="content">The content of the request body.</param>
        /// <param name="options">The request body options.</param>
        public RequestBody(
            string description = default,
            IReadOnlyDictionary<ContentType, MediaType> content = default,
            RequestBodyOptions options = default)
        {
            Description = description;
            Content = content ?? ReadOnlyDictionary.Empty<ContentType, MediaType>();
            Options = options;
        }

        #endregion

        #region IEquatable

        public static bool operator ==(RequestBody requestBody1, RequestBody requestBody2)
        {
            if (ReferenceEquals(requestBody1, null) && ReferenceEquals(requestBody2, null)) return true;
            if (ReferenceEquals(requestBody1, null) || ReferenceEquals(requestBody2, null)) return false;
            return requestBody1.Equals((object)requestBody2);
        }

        public static bool operator !=(RequestBody requestBody1, RequestBody requestBody2)
                    => !(requestBody1 == requestBody2);

        /// <summary>Determines whether the specified object is equal to the current object.</summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns>true if the specified object  is equal to the current object; otherwise, false.</returns>
        public override bool Equals(object obj) => Equals(obj as RequestBody);

        /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>true if the current object is equal to the <paramref name="other">other</paramref> parameter; otherwise, false.</returns>
        public virtual bool Equals(RequestBody other)
        {
            if (ReferenceEquals(other, null)) return false;
            if (ReferenceEquals(this, other)) return true;

            if (!StringComparer.Ordinal.Equals(Description, other.Description)) return false;
            if (!Content.DictionaryEquals(other.Content)) return false;
            if (Options != other.Options) return false;

            return true;
        }

        /// <summary>Serves as the default hash function.</summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                var hc = 17L;

                if (Description != null)
                    hc = (hc * 23) + StringComparer.Ordinal.GetHashCode(Description);
                hc = (hc * 23) + Content.Count;
                hc = (hc * 23) + Options.GetHashCode();

                return ((int)(hc >> 32)) ^ (int)hc;
            }
        }

        #endregion
    }
}
