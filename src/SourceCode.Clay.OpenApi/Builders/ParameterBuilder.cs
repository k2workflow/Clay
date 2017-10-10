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
    public class ParameterBuilder : ParameterBodyBuilder, IOpenApiBuilder<Parameter>
    {
        #region Properties

        /// <summary>
        /// Gets or sets the name of the parameter.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the location of the parameter.
        /// </summary>
        public ParameterLocation Location { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of the <see cref="ParameterBuilder"/> class.
        /// </summary>
        public ParameterBuilder()
            : base()
        {
        }

        /// <summary>
        /// Creates a new instance of the <see cref="ParameterBuilder"/> class.
        /// </summary>
        /// <param name="value">The <see cref="Parameter"/> to copy values from.</param>
        public ParameterBuilder(Parameter value)
            : base(value)
        {
            if (ReferenceEquals(value, null)) throw new ArgumentNullException(nameof(value));
            Name = value.Name;
            Location = value.Location;
        }

        #endregion

        #region Methods

        public static implicit operator Parameter(ParameterBuilder builder) => builder?.Build();

        public static implicit operator Referable<Parameter>(ParameterBuilder builder) => builder?.Build();

        public static implicit operator ParameterBuilder(Parameter value) => ReferenceEquals(value, null) ? null : new ParameterBuilder(value);

        /// <summary>
        /// Creates the <see cref="Parameter"/> from this builder.
        /// </summary>
        /// <returns>The <see cref="Parameter"/>.</returns>
        public new Parameter Build() => new Parameter(
            name: Name,
            location: Location,
            description: Description,
            options: Options,
            style: Style,
            schema: Schema,
            examples: new ReadOnlyDictionary<ContentType, Referable<Example>>(Examples),
            content: new ReadOnlyDictionary<ContentType, MediaType>(Content));

        #endregion
    }
}
