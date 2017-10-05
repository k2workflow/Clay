using SourceCode.Clay.Collections.Generic;
using System;
using System.Collections.Generic;

namespace SourceCode.Clay.OpenApi
{
    /// <summary>
    /// Describes a single API operation on a path.
    /// </summary>
    public class Operation : IEquatable<Operation>
    {
        #region Properties

        /// <summary>
        /// Gets the list of tags for API documentation control.
        /// </summary>
        public IReadOnlyList<string> Tags { get; }

        /// <summary>
        /// Gets the short summary of what the operation does.
        /// </summary>
        public string Summary { get; set; }

        /// <summary>
        /// Gets the verbose explanation of the operation behavior.
        /// </summary>
        /// <remarks>
        /// CommonMark syntax MAY be used for rich text representation.
        /// </remarks>
        public string Description { get; set; }

        /// <summary>
        /// Gets the additional external documentation for this operation.
        /// </summary>
        public ExternalDocumentation ExternalDocumentation { get; set; }

        /// <summary>
        /// Gets the unique string used to identify the operation.
        /// </summary>
        public string OperationIdentifier { get; set; }

        /// <summary>
        /// Get the list of parameters that are applicable for this operation.
        /// </summary>
        public IReadOnlyDictionary<ParameterKey, Referable<Parameter>> Parameters { get; }

        /// <summary>
        /// Gets the request body applicable for this operation.
        /// </summary>
        public Referable<RequestBody> RequestBody { get; set; }

        /// <summary>
        /// Gets the list of possible responses as they are returned from executing this operation.
        /// </summary>
        public IReadOnlyDictionary<ResponseKey, Referable<Response>> Responses { get; }

        /// <summary>
        /// Gets the map of possible out-of band callbacks related to the parent operation.
        /// </summary>
        public IReadOnlyDictionary<string, Referable<Callback>> Callbacks { get; }

        /// <summary>
        /// Gets the flags that indicate what options are set on the operation.
        /// </summary>
        public OperationOptions Options { get; }

        /// <summary>
        /// Gets the declaration of which security mechanisms can be used for this operation.
        /// </summary>
        public IReadOnlyList<SecurityScheme> Security { get; }

        /// <summary>
        /// Gets the alternative server list to service this operation.
        /// </summary>
        public IReadOnlyList<Server> Servers { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of the <see cref="Operation"/> class.
        /// </summary>
        /// <param name="tags">The list of tags for API documentation control.</param>
        /// <param name="summary">The short summary of what the operation does.</param>
        /// <param name="description">The verbose explanation of the operation behavior.</param>
        /// <param name="externalDocumentation">The additional external documentation for this operation.</param>
        /// <param name="operationIdentifier">The unique string used to identify the operation.</param>
        /// <param name="parameters">The list of parameters that are applicable for this operation.</param>
        /// <param name="requestBody">The request body applicable for this operation.</param>
        /// <param name="responses">The list of possible responses as they are returned from executing this operation.</param>
        /// <param name="callbacks">The map of possible out-of band callbacks related to the parent operation.</param>
        /// <param name="options">The flags that indicate what options are set on the operation.</param>
        /// <param name="security">The declaration of which security mechanisms can be used for this operation.</param>
        /// <param name="servers">The alternative server list to service this operation.</param>
        public Operation(
            IReadOnlyList<string> tags = default,
            string summary = default,
            string description = default,
            ExternalDocumentation externalDocumentation = default,
            string operationIdentifier = default,
            IReadOnlyDictionary<ParameterKey, Referable<Parameter>> parameters = default,
            Referable<RequestBody> requestBody = default,
            IReadOnlyDictionary<ResponseKey, Referable<Response>> responses = default,
            IReadOnlyDictionary<string, Referable<Callback>> callbacks = default,
            OperationOptions options = default,
            IReadOnlyList<SecurityScheme> security = default,
            IReadOnlyList<Server> servers = default)
        {
            Tags = tags ?? Array.Empty<string>();
            Summary = summary;
            Description = description;
            ExternalDocumentation = externalDocumentation;
            OperationIdentifier = operationIdentifier;
            Parameters = parameters ?? ReadOnlyDictionary.Empty<ParameterKey, Referable<Parameter>>();
            RequestBody = requestBody;
            Responses = responses ?? ReadOnlyDictionary.Empty<ResponseKey, Referable<Response>>();
            Callbacks = callbacks ?? ReadOnlyDictionary.Empty<string, Referable<Callback>>();
            Options = options;
            Security = security;
            Servers = servers;
        }

        #endregion

        #region IEquatable

        /// <summary>Determines whether the specified object is equal to the current object.</summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns>true if the specified object  is equal to the current object; otherwise, false.</returns>
        public override bool Equals(object obj) => Equals(obj as Operation);

        /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>true if the current object is equal to the <paramref name="other">other</paramref> parameter; otherwise, false.</returns>
        public virtual bool Equals(Operation other)
        {
            if (ReferenceEquals(other, null)) return false;
            if (ReferenceEquals(this, other)) return true;

            if (!Tags.ListEquals(other.Tags, false)) return false;
            if (!StringComparer.Ordinal.Equals(Summary, other.Summary)) return false;
            if (!StringComparer.Ordinal.Equals(Description, other.Description)) return false;
            if (!ExternalDocumentation.NullableEquals(other.ExternalDocumentation)) return false;
            if (!StringComparer.Ordinal.Equals(OperationIdentifier, other.OperationIdentifier)) return false;
            if (!Parameters.DictionaryEquals(other.Parameters)) return false;
            if (!RequestBody.Equals(other.RequestBody)) return false;
            if (!Responses.DictionaryEquals(other.Responses)) return false;
            if (!Callbacks.DictionaryEquals(other.Callbacks)) return false;
            if (Options != other.Options) return false;
            if (!Security.ListEquals(other.Security, false)) return false;
            if (!Servers.ListEquals(other.Servers, true)) return false;

            return true;
        }

        /// <summary>Serves as the default hash function.</summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                var hc = 17L;

                hc = hc * 21 + Tags.Count;
                if (Summary != null) hc = hc * 21 + StringComparer.Ordinal.GetHashCode(Summary);
                if (Description != null) hc = hc * 21 + StringComparer.Ordinal.GetHashCode(Description);
                if (OperationIdentifier != null) hc = hc * 21 + StringComparer.Ordinal.GetHashCode(OperationIdentifier);
                hc = hc * 21 + Parameters.Count;
                hc = hc * 21 + Responses.Count;
                hc = hc * 21 + Callbacks.Count;
                hc = hc * 21 + Security.Count;
                hc = hc * 21 + Servers.Count;

                return ((int)(hc >> 32)) ^ (int)hc;
            }
        }

        public static bool operator ==(Operation operation1, Operation operation2)
        {
            if (ReferenceEquals(operation1, null) && ReferenceEquals(operation2, null)) return true;
            if (ReferenceEquals(operation1, null) || ReferenceEquals(operation2, null)) return false;
            return operation1.Equals((object)operation2);
        }

        public static bool operator !=(Operation operation1, Operation operation2) => !(operation1 == operation2);

        #endregion
    }
}
