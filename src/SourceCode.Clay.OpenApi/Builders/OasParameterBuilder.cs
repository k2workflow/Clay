#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using System;
using System.Collections.ObjectModel;
using System.Net.Mime;

namespace SourceCode.Clay.OpenApi
{
    /// <summary>
    /// Describes a single operation parameter.
    /// </summary>
    public class OasParameterBuilder : OasParameterBodyBuilder, IOasBuilder<OasParameter>
    {
        #region Properties

        /// <summary>
        /// Gets or sets the name of the parameter.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the location of the parameter.
        /// </summary>
        public OasParameterLocation Location { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of the <see cref="OasParameterBuilder"/> class.
        /// </summary>
        public OasParameterBuilder()
            : base()
        {
        }

        /// <summary>
        /// Creates a new instance of the <see cref="OasParameterBuilder"/> class.
        /// </summary>
        /// <param name="value">The <see cref="OasParameter"/> to copy values from.</param>
        public OasParameterBuilder(OasParameter value)
            : base(value)
        {
            if (value is null) throw new ArgumentNullException(nameof(value));
            Name = value.Name;
            Location = value.Location;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Performs an implicit conversion from <see cref="OasParameterBuilder"/> to <see cref="OasParameter"/>.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator OasParameter(OasParameterBuilder builder) => builder?.Build();

        /// <summary>
        /// Performs an implicit conversion from <see cref="OasParameterBuilder"/> to <see cref="OasReferable{Parameter}"/>.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator OasReferable<OasParameter>(OasParameterBuilder builder) => builder?.Build();

        /// <summary>
        /// Performs an implicit conversion from <see cref="OasParameter"/> to <see cref="OasParameterBuilder"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator OasParameterBuilder(OasParameter value) => value is null ? null : new OasParameterBuilder(value);

        /// <summary>
        /// Creates the <see cref="OasParameter"/> from this builder.
        /// </summary>
        /// <returns>The <see cref="OasParameter"/>.</returns>
        public new OasParameter Build() => new OasParameter(
            name: Name,
            location: Location,
            description: Description,
            options: Options,
            style: Style,
            schema: Schema,
            examples: new ReadOnlyDictionary<ContentType, OasReferable<OasExample>>(Examples),
            content: new ReadOnlyDictionary<ContentType, OasMediaType>(Content));

        #endregion
    }
}
