#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using SourceCode.Clay.Collections.Generic;
using System;
using System.Collections.Generic;

namespace SourceCode.Clay.OpenApi
{
    /// <summary>
    /// Represents a JSON schema.
    /// </summary>
    public class Schema : IEquatable<Schema>
    {
        #region Properties

        /// <summary>
        /// Gets the general type of the schema.
        /// </summary>
        public SchemaType Type { get; }

        /// <summary>
        /// Gets the title of the schema.
        /// </summary>
        public string Title { get; }

        /// <summary>
        /// Gets the format of the JSON schema.
        /// </summary>
        /// <remarks>
        /// This conforms to the <a href="https://swagger.io/specification/#data-types-13">Open API specification</a>.
        /// </remarks>
        public string Format { get; }

        /// <summary>
        /// Gets the schema description.
        /// </summary>
        public string Description { get; }

        /// <summary>
        /// Gets the number range validation details.
        /// </summary>
        public NumberRange NumberRange { get; }

        /// <summary>
        /// Gets the item count validation details.
        /// </summary>
        public CountRange ItemsRange { get; }

        /// <summary>
        /// Gets the length validation details.
        /// </summary>
        public CountRange LengthRange { get; }

        /// <summary>
        /// Gets the property count validation details.
        /// </summary>
        public CountRange PropertiesRange { get; }

        /// <summary>
        /// Gets the schema options.
        /// </summary>
        public SchemaOptions Options { get; }

        /// <summary>
        /// Gets the valid enum values.
        /// </summary>
        public IReadOnlyList<ScalarValue> Enum { get; }

        /// <summary>
        /// Gets the list of schemas that this schema must conform to.
        /// </summary>
        public IReadOnlyList<Referable<Schema>> AllOf { get; }

        /// <summary>
        /// Gets the list of schemas that, from which exactly one, this schema must conform to.
        /// </summary>
        public IReadOnlyList<Referable<Schema>> OneOf { get; }

        /// <summary>
        /// Gets the list of schemas that, from which one or more, this schema must conform to.
        /// </summary>
        public IReadOnlyList<Referable<Schema>> AnyOf { get; }

        /// <summary>
        /// Gets the list of schemas that this schema must not conform to.
        /// </summary>
        public IReadOnlyList<Referable<Schema>> Not { get; }

        /// <summary>
        /// Gets the list of schemas that represent the array items that this schema must contain.
        /// </summary>
        public Referable<Schema> Items { get; }

        /// <summary>
        /// Gets the list of valid properties.
        /// </summary>
        public IReadOnlyDictionary<string, Referable<Schema>> Properties { get; }

        /// <summary>
        /// Gets the list of valid properties for children.
        /// </summary>
        public IReadOnlyDictionary<string, Referable<Schema>> AdditionalProperties { get; }

        /// <summary>
        /// Gets the external documentation.
        /// </summary>
        public ExternalDocumentation ExternalDocs { get; }

        /// <summary>
        /// Gets the string validation pattern.
        /// </summary>
        public string Pattern { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new <see cref="Schema"/> value.
        /// </summary>
        /// <param name="type">The general type of the schema.</param>
        /// <param name="format">The specific type of the schema/</param>
        /// <param name="title">The title of the schema.</param>
        /// <param name="description">The description of the schema.</param>
        /// <param name="numberRange">The range of valid numbers.</param>
        /// <param name="itemsRange">The range of valid item counts.</param>
        /// <param name="lengthRange">The range of valid lengths.</param>
        /// <param name="propertiesRange">The range of valid property counts.</param>
        /// <param name="options">The schema options. <see cref="SchemaOptions.ItemsIsArray"/> will be automatically set.</param>
        /// <param name="pattern">The regex validation for string values.</param>
        /// <param name="enum">The valid enum values.</param>
        /// <param name="allOf">The list of schemas that this schema must conform to.</param>
        /// <param name="oneOf">The list of schemas that, from which exactly one, this schema must conform to.</param>
        /// <param name="anyOf">The list of schemas that, from which one or more, this schema must conform to.</param>
        /// <param name="not">The list of schemas that this schema must not conform to.</param>
        /// <param name="items">The list of schemas that represent the array items that this schema must contain.</param>
        /// <param name="properties">The list of valid properties.</param>
        /// <param name="additionalProperties">The list of valid properties for children.</param>
        /// <param name="externalDocs">The external documentation.</param>
        public Schema(
            SchemaType type = default,
            string format = null,
            string title = null,
            string description = null,
            NumberRange numberRange = default,
            CountRange itemsRange = default,
            CountRange lengthRange = default,
            CountRange propertiesRange = default,
            SchemaOptions options = default,
            string pattern = default,
            IReadOnlyList<ScalarValue> @enum = default,
            IReadOnlyList<Referable<Schema>> allOf = default,
            IReadOnlyList<Referable<Schema>> oneOf = default,
            IReadOnlyList<Referable<Schema>> anyOf = default,
            IReadOnlyList<Referable<Schema>> not = default,
            Referable<Schema> items = default,
            IReadOnlyDictionary<string, Referable<Schema>> properties = default,
            IReadOnlyDictionary<string, Referable<Schema>> additionalProperties = default,
            ExternalDocumentation externalDocs = default)
        {
            Type = type;
            Title = title;
            Format = format;
            Description = description;
            NumberRange = numberRange;
            ItemsRange = itemsRange;
            LengthRange = lengthRange;
            PropertiesRange = propertiesRange;
            Options = options;
            Pattern = pattern;
            Enum = @enum ?? Array.Empty<ScalarValue>();
            AllOf = allOf ?? Array.Empty<Referable<Schema>>();
            OneOf = oneOf ?? Array.Empty<Referable<Schema>>();
            AnyOf = anyOf ?? Array.Empty<Referable<Schema>>();
            Not = not ?? Array.Empty<Referable<Schema>>();
            Items = items;
            Properties = properties ?? Dictionary.ReadOnlyEmpty<string, Referable<Schema>>();
            AdditionalProperties = additionalProperties ?? Dictionary.ReadOnlyEmpty<string, Referable<Schema>>();
            ExternalDocs = externalDocs;
        }

        #endregion

        #region Methods

        /// <summary>Returns the fully qualified type name of this instance.</summary>
        /// <returns>The fully qualified type name.</returns>
        public override string ToString()
            => $"{Type}";

        #endregion

        #region Equatable

        public static bool operator ==(Schema schema1, Schema schema2)
        {
            if (ReferenceEquals(schema1, null) && ReferenceEquals(schema2, null)) return true;
            if (ReferenceEquals(schema1, null) || ReferenceEquals(schema2, null)) return false;
            return schema1.Equals((object)schema2);
        }

        public static bool operator !=(Schema schema1, Schema schema2) => !(schema1 == schema2);

        /// <summary>Indicates whether this instance and a specified object are equal.</summary>
        /// <param name="obj">The object to compare with the current instance.</param>
        /// <returns>true if <paramref name="obj">obj</paramref> and this instance are the same type and represent the same value; otherwise, false.</returns>
        public override bool Equals(object obj) => Equals(obj as Schema);

        /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>true if the current object is equal to the <paramref name="other">other</paramref> parameter; otherwise, false.</returns>
        public bool Equals(Schema other)
        {
            if (ReferenceEquals(other, null)) return false;
            if (ReferenceEquals(this, other)) return true;

            if (Type != other.Type) return false;
            if (Options != other.Options) return false;
            if (!NumberRange.Equals(other.NumberRange)) return false;
            if (!ItemsRange.Equals(other.ItemsRange)) return false;
            if (!LengthRange.Equals(other.LengthRange)) return false;
            if (!PropertiesRange.Equals(other.PropertiesRange)) return false;
            if (!ExternalDocs.Equals(other.ExternalDocs)) return false;
            if (!StringComparer.Ordinal.Equals(Title, other.Title)) return false;
            if (!StringComparer.Ordinal.Equals(Description, other.Description)) return false;
            if (!StringComparer.Ordinal.Equals(Pattern, other.Pattern)) return false;
            if (!Enum.ListEquals(other.Enum, false)) return false;
            if (!AllOf.ListEquals(other.AllOf, false)) return false;
            if (!OneOf.ListEquals(other.OneOf, false)) return false;
            if (!AnyOf.ListEquals(other.AnyOf, false)) return false;
            if (!Not.ListEquals(other.Not, false)) return false;
            if (!Items.NullableEquals(other.Items)) return false;
            if (!Properties.DictionaryEquals(other.Properties)) return false;
            if (!AdditionalProperties.DictionaryEquals(other.AdditionalProperties)) return false;

            return true;
        }

        /// <summary>Returns the hash code for this instance.</summary>
        /// <returns>A 32-bit signed integer that is the hash code for this instance.</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                var hc = 17L;

                hc = hc * 21 + Type.GetHashCode();
                hc = hc * 21 + Options.GetHashCode();
                hc = hc * 21 + NumberRange.GetHashCode();
                hc = hc * 21 + ItemsRange.GetHashCode();
                hc = hc * 21 + LengthRange.GetHashCode();
                hc = hc * 21 + PropertiesRange.GetHashCode();
                hc = hc * 21 + ExternalDocs.GetHashCode();
                if (Title != null) hc = hc * 21 + StringComparer.Ordinal.GetHashCode(Title);
                if (Format != null) hc = hc * 21 + StringComparer.Ordinal.GetHashCode(Format);
                if (Description != null) hc = hc * 21 + StringComparer.Ordinal.GetHashCode(Description);
                if (Pattern != null) hc = hc * 21 + StringComparer.Ordinal.GetHashCode(Pattern);
                hc = hc * 21 + Enum.Count;
                hc = hc * 21 + AllOf.Count;
                hc = hc * 21 + OneOf.Count;
                hc = hc * 21 + AnyOf.Count;
                hc = hc * 21 + Not.Count;
                hc = hc * 21 + Items.GetHashCode();
                hc = hc * 21 + Properties.Count;
                hc = hc * 21 + AdditionalProperties.Count;

                return ((int)(hc >> 32)) ^ (int)hc;
            }
        }

        #endregion
    }
}
