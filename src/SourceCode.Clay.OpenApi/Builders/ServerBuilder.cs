#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace SourceCode.Clay.OpenApi
{
    /// <summary>
    /// An object representing a Server.
    /// </summary>
    public class ServerBuilder : IOpenApiBuilder<Server>
    {
        #region Properties

        /// <summary>
        /// Gets or sets the URL to the target host.
        /// </summary>
        public Uri Url { get; set; }

        /// <summary>
        /// Gets or sets the optional string describing the host designated by the URL.
        /// </summary>
        /// <remarks>
        /// CommonMark syntax MAY be used for rich text representation.
        /// </remarks>
        public string Description { get; set; }

        /// <summary>
        /// Gets the map between a variable name and its value.
        /// </summary>
        public IDictionary<String, ServerVariable> Variables { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of the <see cref="ServerBuilder"/> class.
        /// </summary>
        public ServerBuilder()
        {
            Variables = new Dictionary<String, ServerVariable>();
        }

        /// <summary>
        /// Creates a new instance of the <see cref="ServerBuilder"/> class.
        /// </summary>
        /// <param name="value">The <see cref="Server"/> to copy values from.</param>
        public ServerBuilder(Server value)
        {
            if (ReferenceEquals(value, null)) throw new ArgumentNullException(nameof(value));
            Url = value.Url;
            Description = value.Description;
            Variables = new Dictionary<String, ServerVariable>(value.Variables);
        }

        #endregion

        #region Methods

        public static implicit operator Server(ServerBuilder builder) => builder?.Build();

        public static implicit operator ServerBuilder(Server value) => ReferenceEquals(value, null) ? null : new ServerBuilder(value);

        /// <summary>
        /// Creates the <see cref="Server"/> from this builder.
        /// </summary>
        /// <returns>The <see cref="Server"/>.</returns>
        public Server Build() => new Server(
            url: Url,
            description: Description,
            variables: new ReadOnlyDictionary<String, ServerVariable>(Variables));

        #endregion
    }
}
