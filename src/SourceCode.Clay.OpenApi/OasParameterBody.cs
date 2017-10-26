#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using SourceCode.Clay.Collections.Generic;
using System;
using System.Collections.Generic;
using System.Net.Mime;

namespace SourceCode.Clay.OpenApi
{
    /// <summary>
    /// Describes a single operation parameter.
    /// </summary>
    public class OasParameterBody : IEquatable<OasParameterBody>
    {
        #region Properties

        /// <summary>
        /// Gets the brief description of the parameter.
        /// </summary>
        /// <remarks>
        /// CommonMark syntax MAY be used for rich text representation.
        /// </remarks>
        public string Description { get; }

        /// <summary>
        /// Gets the parameter options.
        /// </summary>
        public OasParameterOptions Options { get; }

        /// <summary>
        /// Gets the value which indicates how the parameter value will be serialized depending on the type of the parameter value.
        /// </summary>
        public OasParameterStyle Style { get; }

        /// <summary>
        /// Gets the schema defining the type used for the parameter.
        /// </summary>
        public OasReferable<OasSchema> Schema { get; }

        /// <summary>
        /// Gets the list of examples of the parameter.
        /// </summary>
        public IReadOnlyDictionary<ContentType, OasReferable<OasExample>> Examples { get; }

        /// <summary>
        /// Gets the map containing the representations for the parameter.
        /// </summary>
        public IReadOnlyDictionary<ContentType, OasMediaType> Content { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of the <see cref="OasParameterBody"/> class.
        /// </summary>
        /// <param name="description">The brief description of the parameter.</param>
        /// <param name="options">The parameter options.</param>
        /// <param name="style">The value which indicates how the parameter value will be serialized depending on the type of the parameter value.</param>
        /// <param name="schema">The schema defining the type used for the parameter.</param>
        /// <param name="examples">The list of examples of the parameter.</param>
        /// <param name="content">The map containing the representations for the parameter.</param>
        public OasParameterBody(
            string description = default,
            OasParameterOptions options = default,
            OasParameterStyle style = default,
            OasReferable<OasSchema> schema = default,
            IReadOnlyDictionary<ContentType, OasReferable<OasExample>> examples = default,
            IReadOnlyDictionary<ContentType, OasMediaType> content = default)
        {
            Description = description;
            Options = options;
            Style = style;
            Schema = schema;
            Examples = examples ?? ReadOnlyDictionary.Empty<ContentType, OasReferable<OasExample>>();
            Content = content ?? ReadOnlyDictionary.Empty<ContentType, OasMediaType>();
        }

        #endregion

        #region IEquatable

        /// <summary>
        /// Implements the operator == operator.
        /// </summary>
        /// <param name="parameterBody1">The parameter body1.</param>
        /// <param name="parameterBody2">The parameter body2.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator ==(OasParameterBody parameterBody1, OasParameterBody parameterBody2)
        {
            if (parameterBody1 is null && parameterBody2 is null) return true;
            if (parameterBody1 is null || parameterBody2 is null) return false;
            return parameterBody1.Equals((object)parameterBody2);
        }

        /// <summary>
        /// Implements the operator != operator.
        /// </summary>
        /// <param name="parameterBody1">The parameter body1.</param>
        /// <param name="parameterBody2">The parameter body2.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator !=(OasParameterBody parameterBody1, OasParameterBody parameterBody2)
            => !(parameterBody1 == parameterBody2);

        /// <summary>Determines whether the specified object is equal to the current object.</summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns>true if the specified object  is equal to the current object; otherwise, false.</returns>
        public override bool Equals(object obj)
            => obj is OasParameterBody other
            && Equals(other);

        /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>true if the current object is equal to the <paramref name="other">other</paramref> parameter; otherwise, false.</returns>
        public virtual bool Equals(OasParameterBody other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;

            if (!StringComparer.Ordinal.Equals(Description, other.Description)) return false;
            if (Options != other.Options) return false;
            if (Style != other.Style) return false;
            if (!Schema.Equals(other.Schema)) return false;
            if (!Examples.NullableDictionaryEquals(other.Examples)) return false;
            if (!Content.NullableDictionaryEquals(other.Content)) return false;

            return true;
        }

        /// <summary>Serves as the default hash function.</summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                var hc = 17L;

                if (Description != null)
                    hc = (hc * 23) + StringComparer.Ordinal.GetHashCode(Description);
                hc = (hc * 23) + Options.GetHashCode();
                hc = (hc * 23) + Style.GetHashCode();
                hc = (hc * 23) + Schema.GetHashCode();
                hc = (hc * 23) + Examples.Count;
                hc = (hc * 23) + Content.Count;

                return ((int)(hc >> 32)) ^ (int)hc;
            }
        }

        #endregion
    }
}
