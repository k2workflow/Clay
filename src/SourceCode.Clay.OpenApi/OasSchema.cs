#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using SourceCode.Clay.Collections.Generic;
using SourceCode.Clay.Json.Validation;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace SourceCode.Clay.OpenApi
{
    /// <summary>
    /// Represents a JSON schema.
    /// </summary>
    public class OasSchema : IEquatable<OasSchema>
    {
        /// <summary>
        /// Gets the general type of the schema.
        /// </summary>
        public OasSchemaType JsonType { get; }

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
        public NumberConstraint NumberRange { get; }

        /// <summary>
        /// Gets the item count validation details.
        /// </summary>
        public CountConstraint ItemsRange { get; }

        /// <summary>
        /// Gets the length validation details.
        /// </summary>
        public CountConstraint LengthRange { get; }

        /// <summary>
        /// Gets the property count validation details.
        /// </summary>
        public CountConstraint PropertiesRange { get; }

        /// <summary>
        /// Gets the schema options.
        /// </summary>
        public OasSchemaOptions Options { get; }

        /// <summary>
        /// Gets the valid enum values.
        /// </summary>
        public IReadOnlyList<OasScalarValue> Enum { get; }

        /// <summary>
        /// Gets the list of schemas that this schema must conform to.
        /// </summary>
        public IReadOnlyList<OasReferable<OasSchema>> AllOf { get; }

        /// <summary>
        /// Gets the list of schemas that, from which exactly one, this schema must conform to.
        /// </summary>
        public IReadOnlyList<OasReferable<OasSchema>> OneOf { get; }

        /// <summary>
        /// Gets the list of schemas that, from which one or more, this schema must conform to.
        /// </summary>
        public IReadOnlyList<OasReferable<OasSchema>> AnyOf { get; }

        /// <summary>
        /// Gets the list of schemas that this schema must not conform to.
        /// </summary>
        public IReadOnlyList<OasReferable<OasSchema>> Not { get; }

        /// <summary>
        /// Gets the list of schemas that represent the array items that this schema must contain.
        /// </summary>
        public OasReferable<OasSchema> Items { get; }

        /// <summary>
        /// Gets the list of valid properties.
        /// </summary>
        public IReadOnlyDictionary<string, OasReferable<OasSchema>> Properties { get; }

        /// <summary>
        /// Gets the list of valid properties for children.
        /// </summary>
        public IReadOnlyDictionary<string, OasReferable<OasSchema>> AdditionalProperties { get; }

        /// <summary>
        /// Gets the external documentation.
        /// </summary>
        public OasExternalDocumentation ExternalDocumentation { get; }

        /// <summary>
        /// Gets the string validation pattern.
        /// </summary>
        public string Pattern { get; }

        /// <summary>
        /// Creates a new <see cref="OasSchema"/> value.
        /// </summary>
        /// <param name="type">The general type of the schema.</param>
        /// <param name="format">The specific type of the schema/</param>
        /// <param name="title">The title of the schema.</param>
        /// <param name="description">The description of the schema.</param>
        /// <param name="numberRange">The range of valid numbers.</param>
        /// <param name="itemsRange">The range of valid item counts.</param>
        /// <param name="lengthRange">The range of valid lengths.</param>
        /// <param name="propertiesRange">The range of valid property counts.</param>
        /// <param name="options">The schema options.</param>
        /// <param name="pattern">The regex validation for string values.</param>
        /// <param name="enum">The valid enum values.</param>
        /// <param name="allOf">The list of schemas that this schema must conform to.</param>
        /// <param name="oneOf">The list of schemas that, from which exactly one, this schema must conform to.</param>
        /// <param name="anyOf">The list of schemas that, from which one or more, this schema must conform to.</param>
        /// <param name="not">The list of schemas that this schema must not conform to.</param>
        /// <param name="items">The list of schemas that represent the array items that this schema must contain.</param>
        /// <param name="properties">The list of valid properties.</param>
        /// <param name="additionalProperties">The list of valid properties for children.</param>
        /// <param name="externalDocumentation">The external documentation.</param>
        public OasSchema(
            OasSchemaType type = default,
            string format = null,
            string title = null,
            string description = null,
            NumberConstraint numberRange = default,
            CountConstraint itemsRange = default,
            CountConstraint lengthRange = default,
            CountConstraint propertiesRange = default,
            OasSchemaOptions options = default,
            string pattern = default,
            IReadOnlyList<OasScalarValue> @enum = default,
            IReadOnlyList<OasReferable<OasSchema>> allOf = default,
            IReadOnlyList<OasReferable<OasSchema>> oneOf = default,
            IReadOnlyList<OasReferable<OasSchema>> anyOf = default,
            IReadOnlyList<OasReferable<OasSchema>> not = default,
            OasReferable<OasSchema> items = default,
            IReadOnlyDictionary<string, OasReferable<OasSchema>> properties = default,
            IReadOnlyDictionary<string, OasReferable<OasSchema>> additionalProperties = default,
            OasExternalDocumentation externalDocumentation = default)
        {
            JsonType = type;
            Title = title;
            Format = format;
            Description = description;
            NumberRange = numberRange;
            ItemsRange = itemsRange;
            LengthRange = lengthRange;
            PropertiesRange = propertiesRange;
            Options = options;
            Pattern = pattern;
            Enum = @enum ?? Array.Empty<OasScalarValue>();
            AllOf = allOf ?? Array.Empty<OasReferable<OasSchema>>();
            OneOf = oneOf ?? Array.Empty<OasReferable<OasSchema>>();
            AnyOf = anyOf ?? Array.Empty<OasReferable<OasSchema>>();
            Not = not ?? Array.Empty<OasReferable<OasSchema>>();
            Items = items;
            Properties = properties ?? ImmutableDictionary<string, OasReferable<OasSchema>>.Empty;
            AdditionalProperties = additionalProperties ?? ImmutableDictionary<string, OasReferable<OasSchema>>.Empty;
            ExternalDocumentation = externalDocumentation;
        }

        /// <summary>Returns the fully qualified type name of this instance.</summary>
        /// <returns>The fully qualified type name.</returns>
        public override string ToString()
            => $"{JsonType}";

        /// <summary>
        /// Implements the operator == operator.
        /// </summary>
        /// <param name="schema1">The schema1.</param>
        /// <param name="schema2">The schema2.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator ==(OasSchema schema1, OasSchema schema2)
        {
            if (schema1 is null) return schema2 is null;
            return schema1.Equals((object)schema2);
        }

        /// <summary>
        /// Implements the operator != operator.
        /// </summary>
        /// <param name="schema1">The schema1.</param>
        /// <param name="schema2">The schema2.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator !=(OasSchema schema1, OasSchema schema2) => !(schema1 == schema2);

        /// <summary>Indicates whether this instance and a specified object are equal.</summary>
        /// <param name="obj">The object to compare with the current instance.</param>
        /// <returns>true if <paramref name="obj">obj</paramref> and this instance are the same type and represent the same value; otherwise, false.</returns>
        public override bool Equals(object obj)
            => obj is OasSchema other
            && Equals(other);

        /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>true if the current object is equal to the <paramref name="other">other</paramref> parameter; otherwise, false.</returns>
        public bool Equals(OasSchema other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;

            if (JsonType != other.JsonType) return false;
            if (Format != other.Format) return false;
            if (Options != other.Options) return false;
            if (!NumberRange.Equals(other.NumberRange)) return false;
            if (!ItemsRange.Equals(other.ItemsRange)) return false;
            if (!LengthRange.Equals(other.LengthRange)) return false;
            if (!PropertiesRange.Equals(other.PropertiesRange)) return false;
            if (!ExternalDocumentation.Equals(other.ExternalDocumentation)) return false;
            if (!StringComparer.Ordinal.Equals(Title, other.Title)) return false;
            if (!StringComparer.Ordinal.Equals(Description, other.Description)) return false;
            if (!StringComparer.Ordinal.Equals(Pattern, other.Pattern)) return false;
            if (!Enum.NullableSetEquals(other.Enum)) return false;
            if (!AllOf.NullableSetEquals(other.AllOf)) return false;
            if (!OneOf.NullableSetEquals(other.OneOf)) return false;
            if (!AnyOf.NullableSetEquals(other.AnyOf)) return false;
            if (!Not.NullableSetEquals(other.Not)) return false;
            if (Items != other.Items) return false;
            if (!Properties.NullableDictionaryEquals(other.Properties)) return false;
            if (!AdditionalProperties.NullableDictionaryEquals(other.AdditionalProperties)) return false;

            return true;
        }

        /// <summary>Returns the hash code for this instance.</summary>
        /// <returns>A 32-bit signed integer that is the hash code for this instance.</returns>
        public override int GetHashCode()
        {
            var hc = new HashCode();

            hc.Add(JsonType);
            hc.Add(Options);
            hc.Add(NumberRange);
            hc.Add(ItemsRange);
            hc.Add(LengthRange);
            hc.Add(PropertiesRange);
            hc.Add(ExternalDocumentation);
            hc.Add(Title ?? string.Empty, StringComparer.Ordinal);
            hc.Add(Format ?? string.Empty, StringComparer.Ordinal);
            hc.Add(Description ?? string.Empty, StringComparer.Ordinal);
            hc.Add(Pattern ?? string.Empty, StringComparer.Ordinal);
            hc.Add(Enum.Count);
            hc.Add(AllOf.Count);
            hc.Add(OneOf.Count);
            hc.Add(AnyOf.Count);
            hc.Add(Not.Count);
            hc.Add(Items);
            hc.Add(Properties.Count);
            hc.Add(AdditionalProperties.Count);

            return hc.ToHashCode();
        }
    }
}
