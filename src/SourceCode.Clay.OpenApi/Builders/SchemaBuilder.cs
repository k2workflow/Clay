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
    /// Allows the definition of input and output data types.
    /// </summary>
    public class SchemaBuilder : IOpenApiBuilder<Schema>
    {
        #region Properties

        /// <summary>
        /// Gets or sets the general type of the schema.
        /// </summary>
        public SchemaType Type { get; set; }

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
        public NumberRange NumberRange { get; set; }

        /// <summary>
        /// Gets or sets the item count validation details.
        /// </summary>
        public CountRange ItemsRange { get; set; }

        /// <summary>
        /// Gets or sets the length validation details.
        /// </summary>
        public CountRange LengthRange { get; set; }

        /// <summary>
        /// Gets or sets the property count validation details.
        /// </summary>
        public CountRange PropertiesRange { get; set; }

        /// <summary>
        /// Gets or sets the schema options.
        /// </summary>
        public SchemaOptions Options { get; set; }

        /// <summary>
        /// Gets or sets the string validation pattern.
        /// </summary>
        public string Pattern { get; set; }

        /// <summary>
        /// Gets the valid enum values.
        /// </summary>
        public IList<ScalarValue> Enum { get; }

        /// <summary>
        /// Gets the list of schemas that this schema must conform to.
        /// </summary>
        public IList<Referable<Schema>> AllOf { get; }

        /// <summary>
        /// Gets the list of schemas that, from which exactly one, this schema must conform to.
        /// </summary>
        public IList<Referable<Schema>> OneOf { get; }

        /// <summary>
        /// Gets the list of schemas that, from which one or more, this schema must conform to.
        /// </summary>
        public IList<Referable<Schema>> AnyOf { get; }

        /// <summary>
        /// Gets the list of schemas that this schema must not conform to.
        /// </summary>
        public IList<Referable<Schema>> Not { get; }

        /// <summary>
        /// Gets or sets the list of schemas that represent the array items that this schema must contain.
        /// </summary>
        public Referable<Schema> Items { get; set; }

        /// <summary>
        /// Gets the list of valid properties.
        /// </summary>
        public IDictionary<string, Referable<Schema>> Properties { get; }

        /// <summary>
        /// Gets the list of valid properties for children.
        /// </summary>
        public IDictionary<string, Referable<Schema>> AdditionalProperties { get; }

        /// <summary>
        /// Gets or sets the external documentation.
        /// </summary>
        public ExternalDocumentation ExternalDocumentation { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of the <see cref="SchemaBuilder"/> class.
        /// </summary>
        public SchemaBuilder()
        {
            Enum = new List<ScalarValue>();
            AllOf = new List<Referable<Schema>>();
            OneOf = new List<Referable<Schema>>();
            AnyOf = new List<Referable<Schema>>();
            Not = new List<Referable<Schema>>();
            Properties = new Dictionary<string, Referable<Schema>>();
            AdditionalProperties = new Dictionary<string, Referable<Schema>>();
        }

        /// <summary>
        /// Creates a new instance of the <see cref="SchemaBuilder"/> class.
        /// </summary>
        /// <param name="value">The <see cref="Schema"/> to copy values from.</param>
        public SchemaBuilder(Schema value)
        {
            if (value is null) throw new ArgumentNullException(nameof(value));
            Type = value.Type;
            Format = value.Format;
            Title = value.Title;
            Description = value.Description;
            NumberRange = value.NumberRange;
            ItemsRange = value.ItemsRange;
            LengthRange = value.LengthRange;
            PropertiesRange = value.PropertiesRange;
            Options = value.Options;
            Pattern = value.Pattern;
            Enum = new List<ScalarValue>(value.Enum);
            AllOf = new List<Referable<Schema>>(value.AllOf);
            OneOf = new List<Referable<Schema>>(value.OneOf);
            AnyOf = new List<Referable<Schema>>(value.AnyOf);
            Not = new List<Referable<Schema>>(value.Not);
            Items = value.Items;
            Properties = new Dictionary<string, Referable<Schema>>(value.Properties);
            AdditionalProperties = new Dictionary<string, Referable<Schema>>(value.AdditionalProperties);
            ExternalDocumentation = value.ExternalDocumentation;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Performs an implicit conversion from <see cref="SchemaBuilder"/> to <see cref="Schema"/>.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator Schema(SchemaBuilder builder) => builder?.Build();

        /// <summary>
        /// Performs an implicit conversion from <see cref="SchemaBuilder"/> to <see cref="Referable{Schema}"/>.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator Referable<Schema>(SchemaBuilder builder) => builder?.Build();

        /// <summary>
        /// Performs an implicit conversion from <see cref="Schema"/> to <see cref="SchemaBuilder"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator SchemaBuilder(Schema value) => value is null ? null : new SchemaBuilder(value);

        /// <summary>
        /// Creates the <see cref="Schema"/> from this builder.
        /// </summary>
        /// <returns>The <see cref="Schema"/>.</returns>
        public Schema Build() => new Schema(
            type: Type,
            format: Format,
            title: Title,
            description: Description,
            numberRange: NumberRange,
            itemsRange: ItemsRange,
            lengthRange: LengthRange,
            propertiesRange: PropertiesRange,
            options: Options,
            pattern: Pattern,
            @enum: new ReadOnlyCollection<ScalarValue>(Enum),
            allOf: new ReadOnlyCollection<Referable<Schema>>(AllOf),
            oneOf: new ReadOnlyCollection<Referable<Schema>>(OneOf),
            anyOf: new ReadOnlyCollection<Referable<Schema>>(AnyOf),
            not: new ReadOnlyCollection<Referable<Schema>>(Not),
            items: Items,
            properties: new ReadOnlyDictionary<string, Referable<Schema>>(Properties),
            additionalProperties: new ReadOnlyDictionary<string, Referable<Schema>>(AdditionalProperties),
            externalDocumentation: ExternalDocumentation);

        #endregion
    }
}
