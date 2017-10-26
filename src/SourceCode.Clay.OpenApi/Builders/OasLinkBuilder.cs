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
    public class OasLinkBuilder : IOasBuilder<OasLink>
    {
        #region Properties

        /// <summary>
        /// Gets or sets the relative or absolute reference to an OAS operation.
        /// </summary>
        public OasReference OperationReference { get; set; }

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
        public OasServer Server { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of the <see cref="OasLinkBuilder"/> class.
        /// </summary>
        public OasLinkBuilder()
        {
        }

        /// <summary>
        /// Creates a new instance of the <see cref="OasLinkBuilder"/> class.
        /// </summary>
        /// <param name="value">The <see cref="OasLink"/> to copy values from.</param>
        public OasLinkBuilder(OasLink value)
        {
            if (value is null) throw new ArgumentNullException(nameof(value));
            OperationReference = value.OperationReference;
            OperationIdentifier = value.OperationIdentifier;
            Description = value.Description;
            Server = value.Server;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Performs an implicit conversion from <see cref="OasLinkBuilder"/> to <see cref="OasLink"/>.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator OasLink(OasLinkBuilder builder) => builder?.Build();

        /// <summary>
        /// Performs an implicit conversion from <see cref="OasLinkBuilder"/> to <see cref="OasReferable{Link}"/>.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator OasReferable<OasLink>(OasLinkBuilder builder) => builder?.Build();

        /// <summary>
        /// Performs an implicit conversion from <see cref="OasLink"/> to <see cref="OasLinkBuilder"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator OasLinkBuilder(OasLink value) => value is null ? null : new OasLinkBuilder(value);

        /// <summary>
        /// Creates the <see cref="OasLink"/> from this builder.
        /// </summary>
        /// <returns>The <see cref="OasLink"/>.</returns>
        public OasLink Build() => new OasLink(
            operationReference: OperationReference,
            operationIdentifier: OperationIdentifier,
            description: Description,
            server: Server);

        #endregion
    }
}
