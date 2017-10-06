#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using System;

namespace SourceCode.Clay.OpenApi
{
    /// <summary>
    /// Represents a possible design-time link for a response.
    /// </summary>
    public class LinkBuilder
    {
        #region Properties

        /// <summary>
        /// Gets or sets the relative or absolute reference to an OAS operation.
        /// </summary>
        public Reference OperationReference { get; set; }

        /// <summary>
        /// Gets or sets the name of an existing, resolvable OAS operation, as defined with a unique operation identifier.
        /// </summary>
        public string OperationIdentifier { get; set; }

        /// <summary>
        /// Get or sets the description of the link.
        /// </summary>
        /// <remarks>
        /// CommonMark syntax MAY be used for rich text representation.
        /// </remarks>
        public string Description { get; set; }

        /// <summary>
        /// A server object to be used by the target operation.
        /// </summary>
        public Server Server { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of the <see cref="LinkBuilder"/> class.
        /// </summary>
        public LinkBuilder()
        {
        }

        /// <summary>
        /// Creates a new instance of the <see cref="LinkBuilder"/> class.
        /// </summary>
        /// <param name="value">The <see cref="Link"/> to copy values from.</param>
        public LinkBuilder(Link value)
        {
            if (ReferenceEquals(value, null)) throw new ArgumentNullException(nameof(value));
            OperationReference = value.OperationReference;
            OperationIdentifier = value.OperationIdentifier;
            Description = value.Description;
            Server = value.Server;
        }

        #endregion

        #region Methods

        public static implicit operator Link(LinkBuilder builder) => builder?.Build();

        public static implicit operator LinkBuilder(Link value) => ReferenceEquals(value, null) ? null : new LinkBuilder(value);

        /// <summary>
        /// Creates the <see cref="Link"/> from this builder.
        /// </summary>
        /// <returns>The <see cref="Link"/>.</returns>
        public Link Build() => new Link(
            operationReference: OperationReference,
            operationIdentifier: OperationIdentifier,
            description: Description,
            server: Server);

        #endregion
    }
}
