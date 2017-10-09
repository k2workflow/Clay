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
    public class MediaTypeBuilder : IBuilder<MediaType>
    {
        #region Properties

        /// <summary>
        /// Gets or sets the schema defining the type used for the request body.
        /// </summary>
        public Referable<Schema> Schema { get; set; }

        /// <summary>
        /// The examples of the media type.
        /// </summary>
        public IDictionary<string, Referable<Example>> Examples { get; }

        /// <summary>
        /// Gets the map between a property name and its encoding information.
        /// </summary>
        public IDictionary<string, PropertyEncoding> Encoding { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of the <see cref="MediaTypeBuilder"/> class.
        /// </summary>
        public MediaTypeBuilder()
        {
        }

        /// <summary>
        /// Creates a new instance of the <see cref="MediaTypeBuilder"/> class.
        /// </summary>
        /// <param name="value">The <see cref="MediaType"/> to copy values from.</param>
        public MediaTypeBuilder(MediaType value)
        {
            if (ReferenceEquals(value, null)) throw new ArgumentNullException(nameof(value));
            Schema = value.Schema;
            Examples = new Dictionary<string, Referable<Example>>(value.Examples);
            Encoding = new Dictionary<string, PropertyEncoding>(value.Encoding);
        }

        #endregion

        #region Methods

        public static implicit operator MediaType(MediaTypeBuilder builder) => builder?.Build();

        public static implicit operator MediaTypeBuilder(MediaType value) => ReferenceEquals(value, null) ? null : new MediaTypeBuilder(value);

        /// <summary>
        /// Creates the <see cref="MediaType"/> from this builder.
        /// </summary>
        /// <returns>The <see cref="MediaType"/>.</returns>
        public MediaType Build() => new MediaType(
            schema: Schema,
            examples: new ReadOnlyDictionary<string, Referable<Example>>(Examples),
            encoding: new ReadOnlyDictionary<string, PropertyEncoding>(Encoding));

        #endregion
    }
}
