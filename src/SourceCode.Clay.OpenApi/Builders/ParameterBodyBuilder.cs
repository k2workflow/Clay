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
    /// Describes a single operation parameter.
    /// </summary>
    public class ParameterBodyBuilder : IBuilder<ParameterBody>
    {
        #region Properties

        /// <summary>
        /// Gets or sets the brief description of the parameter.
        /// </summary>
        /// <remarks>
        /// CommonMark syntax MAY be used for rich text representation.
        /// </remarks>
        public String Description { get; set; }

        /// <summary>
        /// Gets or sets the parameter options.
        /// </summary>
        public ParameterOptions Options { get; set; }

        /// <summary>
        /// Gets or sets the value which indicates how the parameter value will be serialized depending on the type of the parameter value.
        /// </summary>
        public ParameterStyle Style { get; set; }

        /// <summary>
        /// Gets or sets the schema defining the type used for the parameter.
        /// </summary>
        public Referable<Schema> Schema { get; set; }

        /// <summary>
        /// Gets the list of examples of the parameter.
        /// </summary>
        public IDictionary<ContentType, Referable<Example>> Examples { get; }

        /// <summary>
        /// Gets the map containing the representations for the parameter.
        /// </summary>
        public IDictionary<ContentType, MediaType> Content { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of the <see cref="ParameterBodyBuilder"/> class.
        /// </summary>
        public ParameterBodyBuilder()
        {
        }

        /// <summary>
        /// Creates a new instance of the <see cref="ParameterBodyBuilder"/> class.
        /// </summary>
        /// <param name="value">The <see cref="ParameterBody"/> to copy values from.</param>
        public ParameterBodyBuilder(ParameterBody value)
        {
            if (ReferenceEquals(value, null)) throw new ArgumentNullException(nameof(value));
            Description = value.Description;
            Options = value.Options;
            Style = value.Style;
            Schema = value.Schema;
            Examples = new Dictionary<ContentType, Referable<Example>>(value.Examples);
            Content = new Dictionary<ContentType, MediaType>(value.Content);
        }

        #endregion

        #region Methods

        public static implicit operator ParameterBody(ParameterBodyBuilder builder) => builder?.Build();

        public static implicit operator ParameterBodyBuilder(ParameterBody value) => ReferenceEquals(value, null) ? null : new ParameterBodyBuilder(value);

        /// <summary>
        /// Creates the <see cref="ParameterBody"/> from this builder.
        /// </summary>
        /// <returns>The <see cref="ParameterBody"/>.</returns>
        public ParameterBody Build() => new ParameterBody(
            description: Description,
            options: Options,
            style: Style,
            schema: Schema,
            examples: new ReadOnlyDictionary<ContentType, Referable<Example>>(Examples),
            content: new ReadOnlyDictionary<ContentType, MediaType>(Content));

        #endregion
    }
}
