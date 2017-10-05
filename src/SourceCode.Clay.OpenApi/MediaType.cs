using SourceCode.Clay.Collections.Generic;
using System;
using System.Collections.Generic;

namespace SourceCode.Clay.OpenApi
{
    /// <summary>
    /// Provides schema and examples for the media type identified by its key.
    /// </summary>
    /// <remarks>
    /// The <c>Example</c> parameter is implementation-specific. If it is required, create a new type
    /// that inherits from this one.
    /// </remarks>
    public class MediaType : IEquatable<MediaType>
    {
        #region Properties

        /// <summary>
        /// Gets the schema defining the type used for the request body.
        /// </summary>
        public Referable<Schema> Schema { get; }

        /// <summary>
        /// The examples of the media type.
        /// </summary>
        public IReadOnlyDictionary<string, Referable<Example>> Examples { get; }

        /// <summary>
        /// Gets the map between a property name and its encoding information.
        /// </summary>
        public IReadOnlyDictionary<string, PropertyEncoding> Encoding { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of the <see cref="MediaType"/> class.
        /// </summary>
        /// <param name="schema">The schema defining the type used for the request body.</param>
        /// <param name="examples">Gets the examples of the media type.</param>
        /// <param name="encoding">Gets the map between a property name and its encoding information.</param>
        public MediaType(
            Referable<Schema> schema = default,
            IReadOnlyDictionary<string, Referable<Example>> examples = default,
            IReadOnlyDictionary<string, PropertyEncoding> encoding = default)
        {
            Schema = schema;
            Examples = examples ?? ReadOnlyDictionary.Empty<string, Referable<Example>>();
            Encoding = encoding ?? ReadOnlyDictionary.Empty<string, PropertyEncoding>();
        }

        #endregion

        #region IEquatable

        /// <summary>Determines whether the specified object is equal to the current object.</summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns>true if the specified object  is equal to the current object; otherwise, false.</returns>
        public override bool Equals(object obj) => Equals(obj as MediaType);

        /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>true if the current object is equal to the <paramref name="other">other</paramref> parameter; otherwise, false.</returns>
        public virtual bool Equals(MediaType other)
        {
            if (ReferenceEquals(other, null)) return false;
            if (ReferenceEquals(this, other)) return true;

            if (!Schema.Equals(other.Schema)) return false;
            if (!Examples.DictionaryEquals(other.Examples)) return false;
            if (!Encoding.DictionaryEquals(other.Encoding)) return false;

            return true;
        }

        /// <summary>Serves as the default hash function.</summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                var hc = 17L;

                hc = hc * 21 + Schema.GetHashCode();
                hc = hc * 21 + Examples.Count;
                hc = hc * 21 + Encoding.Count;

                return ((int)(hc >> 32)) ^ (int)hc;
            }
        }

        public static bool operator ==(MediaType mediaType1, MediaType mediaType2)
        {
            if (ReferenceEquals(mediaType1, null) && ReferenceEquals(mediaType2, null)) return true;
            if (ReferenceEquals(mediaType1, null) || ReferenceEquals(mediaType2, null)) return false;
            return mediaType1.Equals((object)mediaType2);
        }

        public static bool operator !=(MediaType mediaType1, MediaType mediaType2) => !(mediaType1 == mediaType2);

        #endregion
    }
}
