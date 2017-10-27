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
    public class OasServerBuilder : IOasBuilder<OasServer>
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
        public IDictionary<String, OasServerVariable> Variables { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of the <see cref="OasServerBuilder"/> class.
        /// </summary>
        public OasServerBuilder()
        {
            Variables = new Dictionary<String, OasServerVariable>();
        }

        /// <summary>
        /// Creates a new instance of the <see cref="OasServerBuilder"/> class.
        /// </summary>
        /// <param name="value">The <see cref="OasServer"/> to copy values from.</param>
        public OasServerBuilder(OasServer value)
        {
            if (value is null) throw new ArgumentNullException(nameof(value));
            Url = value.Url;
            Description = value.Description;
            Variables = new Dictionary<String, OasServerVariable>(value.Variables);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Performs an implicit conversion from <see cref="OasServerBuilder"/> to <see cref="OasServer"/>.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator OasServer(OasServerBuilder builder) => builder?.Build();

        /// <summary>
        /// Performs an implicit conversion from <see cref="OasServer"/> to <see cref="OasServerBuilder"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator OasServerBuilder(OasServer value) => value is null ? null : new OasServerBuilder(value);

        /// <summary>
        /// Creates the <see cref="OasServer"/> from this builder.
        /// </summary>
        /// <returns>The <see cref="OasServer"/>.</returns>
        public OasServer Build() => new OasServer(
            url: Url,
            description: Description,
            variables: new ReadOnlyDictionary<String, OasServerVariable>(Variables));

        #endregion
    }
}
