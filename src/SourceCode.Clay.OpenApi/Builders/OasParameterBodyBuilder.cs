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
    public class OasParameterBodyBuilder : IOasBuilder<OasParameterBody>
    {
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
        public OasParameterOptions Options { get; set; }

        /// <summary>
        /// Gets or sets the value which indicates how the parameter value will be serialized depending on the type of the parameter value.
        /// </summary>
        public OasParameterStyle Style { get; set; }

        /// <summary>
        /// Gets or sets the schema defining the type used for the parameter.
        /// </summary>
        public OasReferable<OasSchema> Schema { get; set; }

        /// <summary>
        /// Gets the list of examples of the parameter.
        /// </summary>
        public IDictionary<ContentType, OasReferable<OasExample>> Examples { get; }

        /// <summary>
        /// Gets the map containing the representations for the parameter.
        /// </summary>
        public IDictionary<ContentType, OasMediaType> Content { get; }

        /// <summary>
        /// Creates a new instance of the <see cref="OasParameterBodyBuilder"/> class.
        /// </summary>
        public OasParameterBodyBuilder()
        {
            Examples = new Dictionary<ContentType, OasReferable<OasExample>>();
            Content = new Dictionary<ContentType, OasMediaType>();
        }

        /// <summary>
        /// Creates a new instance of the <see cref="OasParameterBodyBuilder"/> class.
        /// </summary>
        /// <param name="value">The <see cref="OasParameterBody"/> to copy values from.</param>
        public OasParameterBodyBuilder(OasParameterBody value)
        {
            if (value is null) throw new ArgumentNullException(nameof(value));
            Description = value.Description;
            Options = value.Options;
            Style = value.Style;
            Schema = value.Schema;
            Examples = new Dictionary<ContentType, OasReferable<OasExample>>(value.Examples);
            Content = new Dictionary<ContentType, OasMediaType>(value.Content);
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="OasParameterBodyBuilder"/> to <see cref="OasParameterBody"/>.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator OasParameterBody(OasParameterBodyBuilder builder) => builder?.Build();

        /// <summary>
        /// Performs an implicit conversion from <see cref="OasParameterBodyBuilder"/> to <see cref="OasReferable{ParameterBody}"/>.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator OasReferable<OasParameterBody>(OasParameterBodyBuilder builder) => builder?.Build();

        /// <summary>
        /// Performs an implicit conversion from <see cref="OasParameterBody"/> to <see cref="OasParameterBodyBuilder"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator OasParameterBodyBuilder(OasParameterBody value) => value is null ? null : new OasParameterBodyBuilder(value);

        /// <summary>
        /// Creates the <see cref="OasParameterBody"/> from this builder.
        /// </summary>
        /// <returns>The <see cref="OasParameterBody"/>.</returns>
        public virtual OasParameterBody Build() => new OasParameterBody(
            description: Description,
            options: Options,
            style: Style,
            schema: Schema,
            examples: new ReadOnlyDictionary<ContentType, OasReferable<OasExample>>(Examples),
            content: new ReadOnlyDictionary<ContentType, OasMediaType>(Content));
    }
}
