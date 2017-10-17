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
    public class PropertyEncodingBuilder : IOpenApiBuilder<PropertyEncoding>
    {
        #region Properties

        /// <summary>
        /// Gets or sets the Content-Type for encoding a specific property.
        /// </summary>
        public ContentType ContentType { get; set; }

        /// <summary>
        /// Gets the list of headers.
        /// </summary>
        public IDictionary<string, Referable<ParameterBody>> Headers { get; }

        /// <summary>
        /// Gets or sets the value describing how the property value will be serialized depending on its type.
        /// </summary>
        public ParameterStyle Style { get; set; }

        /// <summary>
        /// Gets or sets the property options.
        /// </summary>
        public PropertyEncodingOptions Options { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of the <see cref="PropertyEncodingBuilder"/> class.
        /// </summary>
        public PropertyEncodingBuilder()
        {
            Headers = new Dictionary<String, Referable<ParameterBody>>();
        }

        /// <summary>
        /// Creates a new instance of the <see cref="PropertyEncodingBuilder"/> class.
        /// </summary>
        /// <param name="value">The <see cref="PropertyEncoding"/> to copy values from.</param>
        public PropertyEncodingBuilder(PropertyEncoding value)
        {
            if (value is null) throw new ArgumentNullException(nameof(value));
            ContentType = value.ContentType;
            Headers = new Dictionary<String, Referable<ParameterBody>>(value.Headers);
            Style = value.Style;
            Options = value.Options;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Performs an implicit conversion from <see cref="PropertyEncodingBuilder"/> to <see cref="PropertyEncoding"/>.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator PropertyEncoding(PropertyEncodingBuilder builder) => builder?.Build();

        /// <summary>
        /// Performs an implicit conversion from <see cref="PropertyEncoding"/> to <see cref="PropertyEncodingBuilder"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator PropertyEncodingBuilder(PropertyEncoding value) => value is null ? null : new PropertyEncodingBuilder(value);

        /// <summary>
        /// Creates the <see cref="PropertyEncoding"/> from this builder.
        /// </summary>
        /// <returns>The <see cref="PropertyEncoding"/>.</returns>
        public PropertyEncoding Build() => new PropertyEncoding(
            contentType: ContentType,
            headers: new ReadOnlyDictionary<String, Referable<ParameterBody>>(Headers),
            style: Style,
            options: Options);

        #endregion
    }
}
