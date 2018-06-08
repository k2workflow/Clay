#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using SourceCode.Clay.Json.Validation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace SourceCode.Clay.OpenApi
{
    /// <summary>
    /// Allows the definition of input and output data types.
    /// </summary>
    public class OasSchemaBuilder : IOasBuilder<OasSchema>
    {
        /// <summary>
        /// Gets or sets the general type of the schema.
        /// </summary>
        public OasSchemaType JsonType { get; set; }

        /// <summary>
        /// Gets or sets the title of the schema.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the format of the JSON schema.
        /// </summary>
        /// <remarks>
        /// This conforms to the <a href="https://swagger.io/specification/#data-types-13">Open API specification</a>.
        /// </remarks>
        public string Format { get; set; }

        /// <summary>
        /// Gets or sets the schema description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the number range validation details.
        /// </summary>
        public NumberConstraint NumberRange { get; set; }

        /// <summary>
        /// Gets or sets the item count validation details.
        /// </summary>
        public CountConstraint ItemsRange { get; set; }

        /// <summary>
        /// Gets or sets the length validation details.
        /// </summary>
        public CountConstraint LengthRange { get; set; }

        /// <summary>
        /// Gets or sets the property count validation details.
        /// </summary>
        public CountConstraint PropertiesRange { get; set; }

        /// <summary>
        /// Gets or sets the schema options.
        /// </summary>
        public OasSchemaOptions Options { get; set; }

        /// <summary>
        /// Gets or sets the string validation pattern.
        /// </summary>
        public string Pattern { get; set; }

        /// <summary>
        /// Gets the valid enum values.
        /// </summary>
        public IList<OasScalarValue> Enum { get; }

        /// <summary>
        /// Gets the list of schemas that this schema must conform to.
        /// </summary>
        public IList<OasReferable<OasSchema>> AllOf { get; }

        /// <summary>
        /// Gets the list of schemas that, from which exactly one, this schema must conform to.
        /// </summary>
        public IList<OasReferable<OasSchema>> OneOf { get; }

        /// <summary>
        /// Gets the list of schemas that, from which one or more, this schema must conform to.
        /// </summary>
        public IList<OasReferable<OasSchema>> AnyOf { get; }

        /// <summary>
        /// Gets the list of schemas that this schema must not conform to.
        /// </summary>
        public IList<OasReferable<OasSchema>> Not { get; }

        /// <summary>
        /// Gets or sets the list of schemas that represent the array items that this schema must contain.
        /// </summary>
        public OasReferable<OasSchema> Items { get; set; }

        /// <summary>
        /// Gets the list of valid properties.
        /// </summary>
        public IDictionary<string, OasReferable<OasSchema>> Properties { get; }

        /// <summary>
        /// Gets the list of valid properties for children.
        /// </summary>
        public IDictionary<string, OasReferable<OasSchema>> AdditionalProperties { get; }

        /// <summary>
        /// Gets or sets the external documentation.
        /// </summary>
        public OasExternalDocumentation ExternalDocumentation { get; set; }

        /// <summary>
        /// Creates a new instance of the <see cref="OasSchemaBuilder"/> class.
        /// </summary>
        public OasSchemaBuilder()
        {
            Enum = new List<OasScalarValue>();
            AllOf = new List<OasReferable<OasSchema>>();
            OneOf = new List<OasReferable<OasSchema>>();
            AnyOf = new List<OasReferable<OasSchema>>();
            Not = new List<OasReferable<OasSchema>>();
            Properties = new Dictionary<string, OasReferable<OasSchema>>();
            AdditionalProperties = new Dictionary<string, OasReferable<OasSchema>>();
        }

        /// <summary>
        /// Creates a new instance of the <see cref="OasSchemaBuilder"/> class.
        /// </summary>
        /// <param name="value">The <see cref="OasSchema"/> to copy values from.</param>
        public OasSchemaBuilder(OasSchema value)
        {
            if (value is null) throw new ArgumentNullException(nameof(value));
            JsonType = value.JsonType;
            Format = value.Format;
            Title = value.Title;
            Description = value.Description;
            NumberRange = value.NumberRange;
            ItemsRange = value.ItemsRange;
            LengthRange = value.LengthRange;
            PropertiesRange = value.PropertiesRange;
            Options = value.Options;
            Pattern = value.Pattern;
            Enum = new List<OasScalarValue>(value.Enum);
            AllOf = new List<OasReferable<OasSchema>>(value.AllOf);
            OneOf = new List<OasReferable<OasSchema>>(value.OneOf);
            AnyOf = new List<OasReferable<OasSchema>>(value.AnyOf);
            Not = new List<OasReferable<OasSchema>>(value.Not);
            Items = value.Items;
            Properties = new Dictionary<string, OasReferable<OasSchema>>(value.Properties);
            AdditionalProperties = new Dictionary<string, OasReferable<OasSchema>>(value.AdditionalProperties);
            ExternalDocumentation = value.ExternalDocumentation;
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="OasSchemaBuilder"/> to <see cref="OasSchema"/>.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator OasSchema(OasSchemaBuilder builder) => builder?.Build();

        /// <summary>
        /// Performs an implicit conversion from <see cref="OasSchemaBuilder"/> to <see cref="OasReferable{Schema}"/>.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator OasReferable<OasSchema>(OasSchemaBuilder builder) => builder?.Build();

        /// <summary>
        /// Performs an implicit conversion from <see cref="OasSchema"/> to <see cref="OasSchemaBuilder"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator OasSchemaBuilder(OasSchema value) => value is null ? null : new OasSchemaBuilder(value);

        /// <summary>
        /// Creates the <see cref="OasSchema"/> from this builder.
        /// </summary>
        /// <returns>The <see cref="OasSchema"/>.</returns>
        public virtual OasSchema Build() => new OasSchema(
            type: JsonType,
            format: Format,
            title: Title,
            description: Description,
            numberRange: NumberRange,
            itemsRange: ItemsRange,
            lengthRange: LengthRange,
            propertiesRange: PropertiesRange,
            options: Options,
            pattern: Pattern,
            @enum: new ReadOnlyCollection<OasScalarValue>(Enum),
            allOf: new ReadOnlyCollection<OasReferable<OasSchema>>(AllOf),
            oneOf: new ReadOnlyCollection<OasReferable<OasSchema>>(OneOf),
            anyOf: new ReadOnlyCollection<OasReferable<OasSchema>>(AnyOf),
            not: new ReadOnlyCollection<OasReferable<OasSchema>>(Not),
            items: Items,
            properties: new ReadOnlyDictionary<string, OasReferable<OasSchema>>(Properties),
            additionalProperties: new ReadOnlyDictionary<string, OasReferable<OasSchema>>(AdditionalProperties),
            externalDocumentation: ExternalDocumentation);
    }
}
