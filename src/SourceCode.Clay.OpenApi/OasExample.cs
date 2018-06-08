#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using System;

namespace SourceCode.Clay.OpenApi
{
    /// <summary>
    /// Contains an example of how to use an entity in an Open API document.
    /// </summary>
    /// <remarks>
    /// The <c>Value</c> property is implementation-specific. If it is required, create a new type
    /// that inherits from this one.
    /// </remarks>
    public class OasExample : IEquatable<OasExample>
    {
        /// <summary>
        /// Gets the short description for the example.
        /// </summary>
        public string Summary { get; }

        /// <summary>
        /// Gets the long description for the example.
        /// </summary>
        /// <remarks>CommonMark syntax MAY be used for rich text representation.</remarks>
        public string Description { get; }

        /// <summary>
        /// Gets the URL that points to the literal example.
        /// </summary>
        public Uri ExternalValue { get; }

        /// <summary>
        /// Creates a new instance of the <see cref="OasExample"/> class.
        /// </summary>
        /// <param name="summary">The short description for the example.</param>
        /// <param name="description">The long description for the example.</param>
        /// <param name="externalValue">The URL that points to the literal example.</param>
        public OasExample(
            string summary = default,
            string description = default,
            Uri externalValue = default)
        {
            Summary = summary;
            Description = description;
            ExternalValue = externalValue;
        }

        /// <summary>
        /// Implements the operator == operator.
        /// </summary>
        /// <param name="example1">The example1.</param>
        /// <param name="example2">The example2.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator ==(OasExample example1, OasExample example2)
        {
            if (example1 is null) return example2 is null;
            return example1.Equals((object)example2);
        }

        /// <summary>
        /// Implements the operator != operator.
        /// </summary>
        /// <param name="example1">The example1.</param>
        /// <param name="example2">The example2.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator !=(OasExample example1, OasExample example2)
            => !(example1 == example2);

        /// <summary>Determines whether the specified object is equal to the current object.</summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns>true if the specified object  is equal to the current object; otherwise, false.</returns>
        public override bool Equals(object obj)
            => obj is OasExample other
            && Equals(other);

        /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>true if the current object is equal to the <paramref name="other">other</paramref> parameter; otherwise, false.</returns>
        public virtual bool Equals(OasExample other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;

            if (!StringComparer.Ordinal.Equals(Summary, other.Summary)) return false;
            if (!StringComparer.Ordinal.Equals(Description, other.Description)) return false;
            if (ExternalValue != other.ExternalValue) return false;

            return true;
        }

        /// <summary>Serves as the default hash function.</summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode() => HashCode.Combine(
            StringComparer.Ordinal.GetHashCode(Summary ?? string.Empty),
            StringComparer.Ordinal.GetHashCode(Description ?? string.Empty),
            ExternalValue
        );
    }
}
