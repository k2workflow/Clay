using System;
using System.Collections.Generic;
using System.Net.Mime;

namespace SourceCode.Clay.OpenApi
{
    /// <summary>
    /// Describes a single operation parameter.
    /// </summary>
    public class Parameter : ParameterBody, IEquatable<Parameter>
    {
        /// <summary>
        /// Gets the name of the parameter.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the location of the parameter.
        /// </summary>
        public ParameterLocation Location { get; }

        /// <summary>
        /// Creates a new instance of the <see cref="ParameterBody"/> class.
        /// </summary>
        /// <param name="name">The name of the parameter.</param>
        /// <param name="location">The location of the parameter.</param>
        /// <param name="description">The brief description of the parameter.</param>
        /// <param name="options">The parameter options.</param>
        /// <param name="style">The value which indicates how the parameter value will be serialized depending on the type of the parameter value.</param>
        /// <param name="schema">The schema defining the type used for the parameter.</param>
        /// <param name="examples">The list of examples of the parameter.</param>
        /// <param name="content">The map containing the representations for the parameter.</param>
        public Parameter(
            string name = default,
            ParameterLocation location = default,
            string description = default,
            ParameterOptions options = default,
            ParameterStyle style = default,
            Referable<Schema> schema = default,
            IReadOnlyDictionary<ContentType, Referable<Example>> examples = default,
            IReadOnlyDictionary<ContentType, MediaType> content = default)
            : base(description, options, style, schema, examples, content)
        {
            Name = name;
            Location = location;
        }

        #region Equatable

        /// <summary>Determines whether the specified object is equal to the current object.</summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns>true if the specified object  is equal to the current object; otherwise, false.</returns>
        public override bool Equals(object obj) => Equals(obj as Parameter);

        /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>true if the current object is equal to the <paramref name="other">other</paramref> parameter; otherwise, false.</returns>
        public bool Equals(Parameter other)
        {
            if (ReferenceEquals(other, null)) return false;
            if (ReferenceEquals(this, other)) return true;

            if (Location != other.Location) return false;
            if (!StringComparer.Ordinal.Equals(Name, other.Name)) return false;
            if (!base.Equals(other)) return false;

            return true;
        }

        /// <summary>Serves as the default hash function.</summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                var hc = 21L;

                hc = hc * 17 + base.GetHashCode();
                hc = hc * 17 + EqualityComparer<string>.Default.GetHashCode(Name);
                hc = hc * 17 + Location.GetHashCode();

                return ((int)(hc >> 32)) ^ (int)hc;
            }
        }

        public static bool operator ==(Parameter parameter1, Parameter parameter2)
        {
            if (ReferenceEquals(parameter1, null) && ReferenceEquals(parameter2, null)) return true;
            if (ReferenceEquals(parameter1, null) || ReferenceEquals(parameter2, null)) return false;
            return parameter1.Equals((object)parameter2);
        }

        public static bool operator !=(Parameter parameter1, Parameter parameter2) => !(parameter1 == parameter2);

        #endregion
    }
}
