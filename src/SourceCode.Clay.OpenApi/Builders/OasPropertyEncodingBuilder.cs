#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Mime;

namespace SourceCode.Clay.OpenApi
{
    /// <summary>
    /// A single encoding definition applied to a single schema property.
    /// </summary>
    public class OasPropertyEncodingBuilder : IOasBuilder<OasPropertyEncoding>
    {
        /// <summary>
        /// Gets or sets the Content-Type for encoding a specific property.
        /// </summary>
        public ContentType ContentType { get; set; }

        /// <summary>
        /// Gets the list of headers.
        /// </summary>
        public IDictionary<string, OasReferable<OasParameterBody>> Headers { get; }

        /// <summary>
        /// Gets or sets the value describing how the property value will be serialized depending on its type.
        /// </summary>
        public OasParameterStyle Style { get; set; }

        /// <summary>
        /// Gets or sets the property options.
        /// </summary>
        public OasPropertyEncodingOptions Options { get; set; }

        /// <summary>
        /// Creates a new instance of the <see cref="OasPropertyEncodingBuilder"/> class.
        /// </summary>
        public OasPropertyEncodingBuilder()
        {
            Headers = new Dictionary<String, OasReferable<OasParameterBody>>();
        }

        /// <summary>
        /// Creates a new instance of the <see cref="OasPropertyEncodingBuilder"/> class.
        /// </summary>
        /// <param name="value">The <see cref="OasPropertyEncoding"/> to copy values from.</param>
        public OasPropertyEncodingBuilder(OasPropertyEncoding value)
        {
            if (value is null) throw new ArgumentNullException(nameof(value));
            ContentType = value.ContentType;
            Headers = new Dictionary<String, OasReferable<OasParameterBody>>(value.Headers);
            Style = value.Style;
            Options = value.Options;
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="OasPropertyEncodingBuilder"/> to <see cref="OasPropertyEncoding"/>.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator OasPropertyEncoding(OasPropertyEncodingBuilder builder) => builder?.Build();

        /// <summary>
        /// Performs an implicit conversion from <see cref="OasPropertyEncoding"/> to <see cref="OasPropertyEncodingBuilder"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator OasPropertyEncodingBuilder(OasPropertyEncoding value) => value is null ? null : new OasPropertyEncodingBuilder(value);

        /// <summary>
        /// Creates the <see cref="OasPropertyEncoding"/> from this builder.
        /// </summary>
        /// <returns>The <see cref="OasPropertyEncoding"/>.</returns>
        public virtual OasPropertyEncoding Build() => new OasPropertyEncoding(
            contentType: ContentType,
            headers: new ReadOnlyDictionary<String, OasReferable<OasParameterBody>>(Headers),
            style: Style,
            options: Options);
    }
}
