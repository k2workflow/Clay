using SourceCode.Clay.Collections.Generic;
using System;
using System.Collections.Generic;

namespace SourceCode.Clay.OpenApi
{
    /// <summary>
    /// Holds a set of reusable objects for different aspects of the OAS. All objects
    /// defined within the components object will have no effect on the API unless they
    /// are explicitly referenced from properties outside the components object.
    /// </summary>
    public class Components : IEquatable<Components>
    {
        #region Properties

        /// <summary>
        /// Gets the object that holds reusable <see cref="Schema"/> instances.
        /// </summary>
        public IReadOnlyDictionary<string, Referable<Schema>> Schemas { get; }

        /// <summary>
        /// Gets the object that holds reusable <see cref="Response"/> instances.
        /// </summary>
        public IReadOnlyDictionary<string, Referable<Response>> Responses { get; }

        /// <summary>
        /// Gets the object that holds reusable <see cref="Parameter"/> instances.
        /// </summary>
        public IReadOnlyDictionary<string, Referable<Parameter>> Parameters { get; }

        /// <summary>
        /// Gets the object that holds reusable <see cref="Example"/> instances.
        /// </summary>
        public IReadOnlyDictionary<string, Referable<Example>> Examples { get; }

        /// <summary>
        /// Gets the object that holds reusable <see cref="RequestBody"/> instances.
        /// </summary>
        public IReadOnlyDictionary<string, Referable<RequestBody>> RequestBodies { get; }

        /// <summary>
        /// Gets the object that holds reusable header <see cref="ParameterBody"/> instances.
        /// </summary>
        public IReadOnlyDictionary<string, Referable<ParameterBody>> Headers { get; }

        /// <summary>
        /// Gets the object that holds reusable <see cref="SecurityScheme"/> instances.
        /// </summary>
        public IReadOnlyDictionary<string, Referable<SecurityScheme>> SecuritySchemes { get; }

        /// <summary>
        /// Gets the object that holds reusable <see cref="Link"/> instances.
        /// </summary>
        public IReadOnlyDictionary<string, Referable<Link>> Links { get; }

        /// <summary>
        /// Gets the object that holds reusable <see cref="Callback"/> instances.
        /// </summary>
        public IReadOnlyDictionary<string, Referable<Callback>> Callbacks { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of the <see cref="Components"/> class.
        /// </summary>
        /// <param name="schemas">The object that holds reusable <see cref="Schema"/> instances.</param>
        /// <param name="responses">The object that holds reusable <see cref="Response"/> instances.</param>
        /// <param name="parameters">The object that holds reusable <see cref="ParameterBody"/> instances.</param>
        /// <param name="examples">The object that holds reusable <see cref="Example"/> instances.</param>
        /// <param name="requestBodies">The object that holds reusable <see cref="RequestBody"/> instances.</param>
        /// <param name="headers">The object that holds reusable header <see cref="ParameterBody"/> instances.</param>
        /// <param name="securitySchemes">The object that holds reusable <see cref="SecurityScheme"/> instances.</param>
        /// <param name="links">The object that holds reusable <see cref="Link"/> instances.</param>
        /// <param name="callbacks">The object that holds reusable <see cref="Callback"/> instances.</param>
        public Components(
            IReadOnlyDictionary<string, Referable<Schema>> schemas = default,
            IReadOnlyDictionary<string, Referable<Response>> responses = default,
            IReadOnlyDictionary<string, Referable<Parameter>> parameters = default,
            IReadOnlyDictionary<string, Referable<Example>> examples = default,
            IReadOnlyDictionary<string, Referable<RequestBody>> requestBodies = default,
            IReadOnlyDictionary<string, Referable<ParameterBody>> headers = default,
            IReadOnlyDictionary<string, Referable<SecurityScheme>> securitySchemes = default,
            IReadOnlyDictionary<string, Referable<Link>> links = default,
            IReadOnlyDictionary<string, Referable<Callback>> callbacks = default)
        {
            Schemas = schemas ?? Dictionary.ReadOnlyEmpty<string, Referable<Schema>>();
            Responses = responses ?? Dictionary.ReadOnlyEmpty<string, Referable<Response>>();
            Parameters = parameters ?? Dictionary.ReadOnlyEmpty<string, Referable<Parameter>>();
            Examples = examples ?? Dictionary.ReadOnlyEmpty<string, Referable<Example>>();
            RequestBodies = requestBodies ?? Dictionary.ReadOnlyEmpty<string, Referable<RequestBody>>();
            Headers = headers ?? Dictionary.ReadOnlyEmpty<string, Referable<ParameterBody>>();
            SecuritySchemes = securitySchemes ?? Dictionary.ReadOnlyEmpty<string, Referable<SecurityScheme>>();
            Links = links ?? Dictionary.ReadOnlyEmpty<string, Referable<Link>>();
            Callbacks = callbacks ?? Dictionary.ReadOnlyEmpty<string, Referable<Callback>>();
        }

        #endregion

        #region IEquatable

        /// <summary>Determines whether the specified object is equal to the current object.</summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns>true if the specified object  is equal to the current object; otherwise, false.</returns>
        public override bool Equals(object obj) => Equals(obj as Components);

        /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>true if the current object is equal to the <paramref name="other">other</paramref> parameter; otherwise, false.</returns>
        public virtual bool Equals(Components other)
        {
            if (ReferenceEquals(other, null)) return false;
            if (ReferenceEquals(this, other)) return true;

            if (!Schemas.DictionaryEquals(other.Schemas)) return false;
            if (!Responses.DictionaryEquals(other.Responses)) return false;
            if (!Parameters.DictionaryEquals(other.Parameters)) return false;
            if (!Examples.DictionaryEquals(other.Examples)) return false;
            if (!RequestBodies.DictionaryEquals(other.RequestBodies)) return false;
            if (!Headers.DictionaryEquals(other.Headers)) return false;
            if (!SecuritySchemes.DictionaryEquals(other.SecuritySchemes)) return false;
            if (!Links.DictionaryEquals(other.Links)) return false;
            if (!Callbacks.DictionaryEquals(other.Callbacks)) return false;

            return true;
        }

        /// <summary>Serves as the default hash function.</summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                var hc = 17L;

                hc = hc * 21 + Schemas.Count;
                hc = hc * 21 + Responses.Count;
                hc = hc * 21 + Parameters.Count;
                hc = hc * 21 + Examples.Count;
                hc = hc * 21 + RequestBodies.Count;
                hc = hc * 21 + Headers.Count;
                hc = hc * 21 + SecuritySchemes.Count;
                hc = hc * 21 + Links.Count;
                hc = hc * 21 + Callbacks.Count;

                return ((int)(hc >> 32)) ^ (int)hc;
            }
        }

        public static bool operator ==(Components components1, Components components2)
        {
            if (ReferenceEquals(components1, null) && ReferenceEquals(components2, null)) return true;
            if (ReferenceEquals(components1, null) || ReferenceEquals(components2, null)) return false;
            return components1.Equals((object)components2);
        }

        public static bool operator !=(Components components1, Components components2) => !(components1 == components2);

        #endregion
    }
}
