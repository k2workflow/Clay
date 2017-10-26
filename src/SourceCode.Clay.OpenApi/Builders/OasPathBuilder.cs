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
    /// Describes the operations available on a single path. A Path Item MAY be empty, due to ACL constraints.
    /// The path itself is still exposed to the documentation viewer but they will not know which operations
    /// and parameters are available.
    /// </summary>
    public class OasPathBuilder : IOasBuilder<OasPath>
    {
        #region Properties

        /// <summary>
        /// Gets or sets the string summary intended to apply to all operations in this path.
        /// </summary>
        public string Summary { get; set; }

        /// <summary>
        /// Gets or sets the string description intended to apply to all operations in this path.
        /// </summary>
        /// <remarks>
        /// CommonMark syntax MAY be used for rich text representation.
        /// </remarks>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the definition of a GET operation on this path.
        /// </summary>
        public OasOperation Get { get; set; }

        /// <summary>
        /// Gets or sets the definition of a PUT operation on this path.
        /// </summary>
        public OasOperation Put { get; set; }

        /// <summary>
        /// Gets or sets the definition of a POST operation on this path.
        /// </summary>
        public OasOperation Post { get; set; }

        /// <summary>
        /// Gets or sets the definition of a DELETE operation on this path.
        /// </summary>
        public OasOperation Delete { get; set; }

        /// <summary>
        /// Gets or sets the definition of a OPTIONS operation on this path.
        /// </summary>
        public OasOperation Options { get; set; }

        /// <summary>
        /// Gets or sets the definition of a HEAD operation on this path.
        /// </summary>
        public OasOperation Head { get; set; }

        /// <summary>
        /// Gets or sets the definition of a PATCH operation on this path.
        /// </summary>
        public OasOperation Patch { get; set; }

        /// <summary>
        /// Gets or sets the definition of a TRACE operation on this path.
        /// </summary>
        public OasOperation Trace { get; set; }

        /// <summary>
        /// Gets the alternative server array to service all operations in this path.
        /// </summary>
        public IList<OasServer> Servers { get; }

        /// <summary>
        /// Gets the list of parameters that are applicable for all the operations described under this path.
        /// </summary>
        public IDictionary<OasParameterKey, OasReferable<OasParameterBody>> Parameters { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of the <see cref="OasPathBuilder"/> class.
        /// </summary>
        public OasPathBuilder()
        {
            Servers = new List<OasServer>();
            Parameters = new Dictionary<OasParameterKey, OasReferable<OasParameterBody>>();
        }

        /// <summary>
        /// Creates a new instance of the <see cref="OasPathBuilder"/> class.
        /// </summary>
        /// <param name="value">The <see cref="OasPath"/> to copy values from.</param>
        public OasPathBuilder(OasPath value)
        {
            if (value is null) throw new ArgumentNullException(nameof(value));
            Summary = value.Summary;
            Description = value.Description;
            Get = value.Get;
            Put = value.Put;
            Post = value.Post;
            Delete = value.Delete;
            Options = value.Options;
            Head = value.Head;
            Patch = value.Patch;
            Trace = value.Trace;
            Servers = new List<OasServer>(value.Servers);
            Parameters = new Dictionary<OasParameterKey, OasReferable<OasParameterBody>>(value.Parameters);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Performs an implicit conversion from <see cref="OasPathBuilder"/> to <see cref="OasPath"/>.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator OasPath(OasPathBuilder builder) => builder?.Build();

        /// <summary>
        /// Performs an implicit conversion from <see cref="OasPathBuilder"/> to <see cref="OasReferable{Path}"/>.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator OasReferable<OasPath>(OasPathBuilder builder) => builder?.Build();

        /// <summary>
        /// Performs an implicit conversion from <see cref="OasPath"/> to <see cref="OasPathBuilder"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator OasPathBuilder(OasPath value) => value is null ? null : new OasPathBuilder(value);

        /// <summary>
        /// Creates the <see cref="OasPath"/> from this builder.
        /// </summary>
        /// <returns>The <see cref="OasPath"/>.</returns>
        public OasPath Build() => new OasPath(
            summary: Summary,
            description: Description,
            get: Get,
            put: Put,
            post: Post,
            delete: Delete,
            options: Options,
            head: Head,
            patch: Patch,
            trace: Trace,
            servers: new ReadOnlyCollection<OasServer>(Servers),
            parameters: new ReadOnlyDictionary<OasParameterKey, OasReferable<OasParameterBody>>(Parameters));

        #endregion
    }
}
