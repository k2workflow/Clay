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
    /// A single encoding definition applied to a single schema property.
    /// </summary>
    public class OasPropertyEncoding : IEquatable<OasPropertyEncoding>
    {
        #region Properties

        /// <summary>
        /// Gets the Content-Type for encoding a specific property.
        /// </summary>
        public ContentType ContentType { get; }

        /// <summary>
        /// Gets the list of headers.
        /// </summary>
        public IReadOnlyDictionary<string, OasReferable<OasParameterBody>> Headers { get; }

        /// <summary>
        /// Gets the value describing how the property value will be serialized depending on its type.
        /// </summary>
        public OasParameterStyle Style { get; }

        /// <summary>
        /// Gets the property options.
        /// </summary>
        public OasPropertyEncodingOptions Options { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of the <see cref="OasPropertyEncoding"/> class.
        /// </summary>
        /// <param name="contentType">The Content-Type for encoding a specific property.</param>
        /// <param name="headers">The list of headers.</param>
        /// <param name="style"></param>
        /// <param name="options"></param>
        public OasPropertyEncoding(
            ContentType contentType = default,
            IReadOnlyDictionary<string, OasReferable<OasParameterBody>> headers = default,
            OasParameterStyle style = default,
            OasPropertyEncodingOptions options = default)
        {
            ContentType = contentType;
            Headers = headers ?? ImmutableDictionary<string, OasReferable<OasParameterBody>>.Empty;
            Style = style;
            Options = options;
        }

        #endregion

        #region IEquatable

        /// <summary>
        /// Implements the operator == operator.
        /// </summary>
        /// <param name="propertyEncoding1">The property encoding1.</param>
        /// <param name="propertyEncoding2">The property encoding2.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator ==(OasPropertyEncoding propertyEncoding1, OasPropertyEncoding propertyEncoding2)
        {
            if (propertyEncoding1 is null) return propertyEncoding2 is null;
            return propertyEncoding1.Equals((object)propertyEncoding2);
        }

        /// <summary>
        /// Implements the operator != operator.
        /// </summary>
        /// <param name="propertyEncoding1">The property encoding1.</param>
        /// <param name="propertyEncoding2">The property encoding2.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator !=(OasPropertyEncoding propertyEncoding1, OasPropertyEncoding propertyEncoding2)
            => !(propertyEncoding1 == propertyEncoding2);

        /// <summary>Determines whether the specified object is equal to the current object.</summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns>true if the specified object  is equal to the current object; otherwise, false.</returns>
        public override bool Equals(object obj)
            => obj is OasPropertyEncoding other
            && Equals(other);

        /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>true if the current object is equal to the <paramref name="other">other</paramref> parameter; otherwise, false.</returns>
        public virtual bool Equals(OasPropertyEncoding other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;

            if (!Equals(ContentType, other.ContentType)) return false;
            if (!Headers.NullableDictionaryEquals(other.Headers)) return false;
            if (Style != other.Style) return false;
            if (Options != other.Options) return false;

            return true;
        }

        /// <summary>Serves as the default hash function.</summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode() => HashCode.Combine(
            ContentType,
            Headers.Count,
            Style,
            Options
        );

        #endregion
    }
}
