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
    public class OasRequestBody : IEquatable<OasRequestBody>
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
        public IReadOnlyDictionary<ContentType, OasMediaType> Content { get; }

        /// <summary>
        /// Gets the request body options.
        /// </summary>
        public OasRequestBodyOptions Options { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of the <see cref="OasRequestBody"/> class.
        /// </summary>
        /// <param name="description">The brief description of the request body.</param>
        /// <param name="content">The content of the request body.</param>
        /// <param name="options">The request body options.</param>
        public OasRequestBody(
            string description = default,
            IReadOnlyDictionary<ContentType, OasMediaType> content = default,
            OasRequestBodyOptions options = default)
        {
            Description = description;
            Content = content ?? ReadOnlyDictionary.Empty<ContentType, OasMediaType>();
            Options = options;
        }

        #endregion

        #region IEquatable

        /// <summary>
        /// Implements the operator == operator.
        /// </summary>
        /// <param name="requestBody1">The request body1.</param>
        /// <param name="requestBody2">The request body2.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator ==(OasRequestBody requestBody1, OasRequestBody requestBody2)
        {
            if (requestBody1 is null) return requestBody2 is null;
            return requestBody1.Equals((object)requestBody2);
        }

        /// <summary>
        /// Implements the operator != operator.
        /// </summary>
        /// <param name="requestBody1">The request body1.</param>
        /// <param name="requestBody2">The request body2.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator !=(OasRequestBody requestBody1, OasRequestBody requestBody2)
                            => !(requestBody1 == requestBody2);

        /// <summary>Determines whether the specified object is equal to the current object.</summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns>true if the specified object  is equal to the current object; otherwise, false.</returns>
        public override bool Equals(object obj)
            => obj is OasRequestBody other
            && Equals(other);

        /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>true if the current object is equal to the <paramref name="other">other</paramref> parameter; otherwise, false.</returns>
        public virtual bool Equals(OasRequestBody other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;

            if (!StringComparer.Ordinal.Equals(Description, other.Description)) return false;
            if (!Content.NullableDictionaryEquals(other.Content)) return false;
            if (Options != other.Options) return false;

            return true;
        }

        /// <summary>Serves as the default hash function.</summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode() => new HashCode()
            .Tally(Description ?? string.Empty, StringComparer.Ordinal)
            .TallyCount(Content)
            .Tally(Options)
            .ToHashCode();

        #endregion
    }
}
