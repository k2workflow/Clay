#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using SourceCode.Clay.Collections.Generic;
using System;
using System.Collections.Generic;

namespace SourceCode.Clay.OpenApi
{
    /// <summary>
    /// Describes the operations available on a single path. A Path Item MAY be empty, due to ACL constraints.
    /// The path itself is still exposed to the documentation viewer but they will not know which operations
    /// and parameters are available.
    /// </summary>
    public class OasPath : IEquatable<OasPath>
    {
        #region Properties

        /// <summary>
        /// Gets the string summary intended to apply to all operations in this path.
        /// </summary>
        public string Summary { get; }

        /// <summary>
        /// Gets the string description intended to apply to all operations in this path.
        /// </summary>
        /// <remarks>
        /// CommonMark syntax MAY be used for rich text representation.
        /// </remarks>
        public string Description { get; }

        /// <summary>
        /// Gets the definition of a GET operation on this path.
        /// </summary>
        public OasOperation Get { get; }

        /// <summary>
        /// Gets the definition of a PUT operation on this path.
        /// </summary>
        public OasOperation Put { get; }

        /// <summary>
        /// Gets the definition of a POST operation on this path.
        /// </summary>
        public OasOperation Post { get; }

        /// <summary>
        /// Gets the definition of a DELETE operation on this path.
        /// </summary>
        public OasOperation Delete { get; }

        /// <summary>
        /// Gets the definition of a OPTIONS operation on this path.
        /// </summary>
        public OasOperation Options { get; }

        /// <summary>
        /// Gets the definition of a HEAD operation on this path.
        /// </summary>
        public OasOperation Head { get; }

        /// <summary>
        /// Gets the definition of a PATCH operation on this path.
        /// </summary>
        public OasOperation Patch { get; }

        /// <summary>
        /// Gets the definition of a TRACE operation on this path.
        /// </summary>
        public OasOperation Trace { get; }

        /// <summary>
        /// Gets the alternative server array to service all operations in this path.
        /// </summary>
        public IReadOnlyList<OasServer> Servers { get; }

        /// <summary>
        /// Gets the list of parameters that are applicable for all the operations described under this path.
        /// </summary>
        public IReadOnlyDictionary<OasParameterKey, OasReferable<OasParameterBody>> Parameters { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of the <see cref="OasPath"/> class.
        /// </summary>
        /// <param name="summary">The string summary intended to apply to all operations in this path.</param>
        /// <param name="description">The string description intended to apply to all operations in this path.</param>
        /// <param name="get">The definition of a GET operation on this path.</param>
        /// <param name="put">The definition of a PUT operation on this path.</param>
        /// <param name="post">The definition of a POST operation on this path.</param>
        /// <param name="delete">The definition of a DELETE operation on this path.</param>
        /// <param name="options">The definition of a OPTIONS operation on this path.</param>
        /// <param name="head">The definition of a HEAD operation on this path.</param>
        /// <param name="patch">The definition of a PATCH operation on this path.</param>
        /// <param name="trace">The definition of a TRACE operation on this path.</param>
        /// <param name="servers">The alternative server list to service all operations in this path.</param>
        /// <param name="parameters">The list of parameters that are applicable for all the operations described under this path.</param>
        public OasPath(
            string summary = default,
            string description = default,
            OasOperation get = default,
            OasOperation put = default,
            OasOperation post = default,
            OasOperation delete = default,
            OasOperation options = default,
            OasOperation head = default,
            OasOperation patch = default,
            OasOperation trace = default,
            IReadOnlyList<OasServer> servers = default,
            IReadOnlyDictionary<OasParameterKey, OasReferable<OasParameterBody>> parameters = default)
        {
            Summary = summary;
            Description = description;
            Get = get;
            Put = put;
            Post = post;
            Delete = delete;
            Options = options;
            Head = head;
            Patch = patch;
            Trace = trace;
            Servers = servers ?? Array.Empty<OasServer>();
            Parameters = parameters ?? ReadOnlyDictionary.Empty<OasParameterKey, OasReferable<OasParameterBody>>();
        }

        #endregion

        #region IEquatable

        /// <summary>
        /// Implements the operator == operator.
        /// </summary>
        /// <param name="path1">The path1.</param>
        /// <param name="path2">The path2.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator ==(OasPath path1, OasPath path2)
        {
            if (path1 is null) return path2 is null;
            return path1.Equals((object)path2);
        }

        /// <summary>
        /// Implements the operator != operator.
        /// </summary>
        /// <param name="path1">The path1.</param>
        /// <param name="path2">The path2.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator !=(OasPath path1, OasPath path2)
            => !(path1 == path2);

        /// <summary>Determines whether the specified object is equal to the current object.</summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns>true if the specified object  is equal to the current object; otherwise, false.</returns>
        public override bool Equals(object obj)
            => obj is OasPath other
            && Equals(other);

        /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>true if the current object is equal to the <paramref name="other">other</paramref> parameter; otherwise, false.</returns>
        public virtual bool Equals(OasPath other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;

            if (!StringComparer.Ordinal.Equals(Summary, other.Summary)) return false;
            if (!StringComparer.Ordinal.Equals(Description, other.Description)) return false;
            if (!Get.NullableEquals(other.Get)) return false;
            if (!Put.NullableEquals(other.Put)) return false;
            if (!Post.NullableEquals(other.Post)) return false;
            if (!Delete.NullableEquals(other.Delete)) return false;
            if (!Options.NullableEquals(other.Options)) return false;
            if (!Head.NullableEquals(other.Head)) return false;
            if (!Patch.NullableEquals(other.Patch)) return false;
            if (!Trace.NullableEquals(other.Trace)) return false;
            if (!Servers.NullableListEquals(other.Servers)) return false;
            if (!Parameters.NullableDictionaryEquals(other.Parameters)) return false;

            return true;
        }

        /// <summary>Serves as the default hash function.</summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode() => new HashCode()
            .Tally(Summary ?? string.Empty, StringComparer.Ordinal)
            .Tally(Description ?? string.Empty, StringComparer.Ordinal)
            .Tally(Get)
            .Tally(Put)
            .Tally(Post)
            .Tally(Delete)
            .Tally(Options)
            .Tally(Head)
            .Tally(Patch)
            .Tally(Trace)
            .TallyCount(Servers)
            .TallyCount(Parameters)
            .ToHashCode();

        #endregion
    }
}
