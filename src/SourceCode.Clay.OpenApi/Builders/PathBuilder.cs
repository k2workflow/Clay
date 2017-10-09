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
    public class PathBuilder : IBuilder<Path>
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
        public Operation Get { get; set; }

        /// <summary>
        /// Gets or sets the definition of a PUT operation on this path.
        /// </summary>
        public Operation Put { get; set; }

        /// <summary>
        /// Gets or sets the definition of a POST operation on this path.
        /// </summary>
        public Operation Post { get; set; }

        /// <summary>
        /// Gets or sets the definition of a DELETE operation on this path.
        /// </summary>
        public Operation Delete { get; set; }

        /// <summary>
        /// Gets or sets the definition of a OPTIONS operation on this path.
        /// </summary>
        public Operation Options { get; set; }

        /// <summary>
        /// Gets or sets the definition of a HEAD operation on this path.
        /// </summary>
        public Operation Head { get; set; }

        /// <summary>
        /// Gets or sets the definition of a PATCH operation on this path.
        /// </summary>
        public Operation Patch { get; set; }

        /// <summary>
        /// Gets or sets the definition of a TRACE operation on this path.
        /// </summary>
        public Operation Trace { get; set; }

        /// <summary>
        /// Gets the alternative server array to service all operations in this path.
        /// </summary>
        public IList<Server> Servers { get; }

        /// <summary>
        /// Gets the list of parameters that are applicable for all the operations described under this path.
        /// </summary>
        public IDictionary<ParameterKey, Referable<ParameterBody>> Parameters { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of the <see cref="PathBuilder"/> class.
        /// </summary>
        public PathBuilder()
        {
        }

        /// <summary>
        /// Creates a new instance of the <see cref="PathBuilder"/> class.
        /// </summary>
        /// <param name="value">The <see cref="Path"/> to copy values from.</param>
        public PathBuilder(Path value)
        {
            if (ReferenceEquals(value, null)) throw new ArgumentNullException(nameof(value));
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
            Servers = new List<Server>(value.Servers);
            Parameters = new Dictionary<ParameterKey, Referable<ParameterBody>>(value.Parameters);
        }

        #endregion

        #region Methods

        public static implicit operator Path(PathBuilder builder) => builder?.Build();

        public static implicit operator PathBuilder(Path value) => ReferenceEquals(value, null) ? null : new PathBuilder(value);

        /// <summary>
        /// Creates the <see cref="Path"/> from this builder.
        /// </summary>
        /// <returns>The <see cref="Path"/>.</returns>
        public Path Build() => new Path(
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
            servers: new ReadOnlyCollection<Server>(Servers),
            parameters: new ReadOnlyDictionary<ParameterKey, Referable<ParameterBody>>(Parameters));

        #endregion
    }
}
