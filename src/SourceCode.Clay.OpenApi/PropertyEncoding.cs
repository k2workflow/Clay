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
    public class PropertyEncoding : IEquatable<PropertyEncoding>
    {
        #region Properties

        /// <summary>
        /// Gets the Content-Type for encoding a specific property.
        /// </summary>
        public ContentType ContentType { get; }

        /// <summary>
        /// Gets the list of headers.
        /// </summary>
        public IReadOnlyDictionary<string, Referable<ParameterBody>> Headers { get; }

        /// <summary>
        /// Gets the value describing how the property value will be serialized depending on its type.
        /// </summary>
        public ParameterStyle Style { get; }

        /// <summary>
        /// Gets the property options.
        /// </summary>
        public PropertyEncodingOptions Options { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of the <see cref="PropertyEncoding"/> class.
        /// </summary>
        /// <param name="contentType">The Content-Type for encoding a specific property.</param>
        /// <param name="headers">The list of headers.</param>
        /// <param name="style"></param>
        /// <param name="options"></param>
        public PropertyEncoding(
            ContentType contentType = default,
            IReadOnlyDictionary<string, Referable<ParameterBody>> headers = default,
            ParameterStyle style = default,
            PropertyEncodingOptions options = default)
        {
            ContentType = contentType;
            Headers = headers ?? ReadOnlyDictionary.Empty<string, Referable<ParameterBody>>();
            Style = style;
            Options = options;
        }

        #endregion

        #region IEquatable

        public static bool operator ==(PropertyEncoding propertyEncoding1, PropertyEncoding propertyEncoding2)
        {
            if (ReferenceEquals(propertyEncoding1, null) && ReferenceEquals(propertyEncoding2, null)) return true;
            if (ReferenceEquals(propertyEncoding1, null) || ReferenceEquals(propertyEncoding2, null)) return false;
            return propertyEncoding1.Equals((object)propertyEncoding2);
        }

        public static bool operator !=(PropertyEncoding propertyEncoding1, PropertyEncoding propertyEncoding2)
                    => !(propertyEncoding1 == propertyEncoding2);

        /// <summary>Determines whether the specified object is equal to the current object.</summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns>true if the specified object  is equal to the current object; otherwise, false.</returns>
        public override bool Equals(object obj) => Equals(obj as PropertyEncoding);

        /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>true if the current object is equal to the <paramref name="other">other</paramref> parameter; otherwise, false.</returns>
        public virtual bool Equals(PropertyEncoding other)
        {
            if (ReferenceEquals(other, null)) return false;
            if (ReferenceEquals(this, other)) return true;

            if (!StringComparer.Ordinal.Equals(ContentType, other.ContentType)) return false;
            if (!Headers.DictionaryEquals(other.Headers)) return false;
            if (Style != other.Style) return false;
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

                if (ContentType != null) hc = hc * 21 + ContentType.GetHashCode();
                hc = hc * 21 + Headers.Count;
                hc = hc * 21 + Style.GetHashCode();
                hc = hc * 21 + Options.GetHashCode();

                return ((int)(hc >> 32)) ^ (int)hc;
            }
        }

        #endregion
    }
}
