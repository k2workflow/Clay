#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace SourceCode.Clay.OpenApi
{
    /// <summary>
    /// Provides schema and examples for the media type identified by a key.
    /// </summary>
    public class OasMediaTypeBuilder : IOasBuilder<OasMediaType>
    {
        /// <summary>
        /// Gets or sets the schema defining the type used for the request body.
        /// </summary>
        public OasReferable<OasSchema> Schema { get; set; }

        /// <summary>
        /// The examples of the media type.
        /// </summary>
        public IDictionary<string, OasReferable<OasExample>> Examples { get; }

        /// <summary>
        /// Gets the map between a property name and its encoding information.
        /// </summary>
        public IDictionary<string, OasPropertyEncoding> Encoding { get; }

        /// <summary>
        /// Creates a new instance of the <see cref="OasMediaTypeBuilder"/> class.
        /// </summary>
        public OasMediaTypeBuilder()
        {
            Examples = new Dictionary<string, OasReferable<OasExample>>();
            Encoding = new Dictionary<string, OasPropertyEncoding>();
        }

        /// <summary>
        /// Creates a new instance of the <see cref="OasMediaTypeBuilder"/> class.
        /// </summary>
        /// <param name="value">The <see cref="OasMediaType"/> to copy values from.</param>
        public OasMediaTypeBuilder(OasMediaType value)
        {
            if (value is null) throw new ArgumentNullException(nameof(value));
            Schema = value.Schema;
            Examples = new Dictionary<string, OasReferable<OasExample>>(value.Examples);
            Encoding = new Dictionary<string, OasPropertyEncoding>(value.Encoding);
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="OasMediaTypeBuilder"/> to <see cref="OasMediaType"/>.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator OasMediaType(OasMediaTypeBuilder builder) => builder?.Build();

        /// <summary>
        /// Performs an implicit conversion from <see cref="OasMediaType"/> to <see cref="OasMediaTypeBuilder"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator OasMediaTypeBuilder(OasMediaType value) => value is null ? null : new OasMediaTypeBuilder(value);

        /// <summary>
        /// Creates the <see cref="OasMediaType"/> from this builder.
        /// </summary>
        /// <returns>The <see cref="OasMediaType"/>.</returns>
        public virtual OasMediaType Build() => new OasMediaType(
            schema: Schema,
            examples: new ReadOnlyDictionary<string, OasReferable<OasExample>>(Examples),
            encoding: new ReadOnlyDictionary<string, OasPropertyEncoding>(Encoding));
    }
}
