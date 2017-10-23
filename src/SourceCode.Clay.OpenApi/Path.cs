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
    public class Path : IEquatable<Path>
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
        public Operation Get { get; }

        /// <summary>
        /// Gets the definition of a PUT operation on this path.
        /// </summary>
        public Operation Put { get; }

        /// <summary>
        /// Gets the definition of a POST operation on this path.
        /// </summary>
        public Operation Post { get; }

        /// <summary>
        /// Gets the definition of a DELETE operation on this path.
        /// </summary>
        public Operation Delete { get; }

        /// <summary>
        /// Gets the definition of a OPTIONS operation on this path.
        /// </summary>
        public Operation Options { get; }

        /// <summary>
        /// Gets the definition of a HEAD operation on this path.
        /// </summary>
        public Operation Head { get; }

        /// <summary>
        /// Gets the definition of a PATCH operation on this path.
        /// </summary>
        public Operation Patch { get; }

        /// <summary>
        /// Gets the definition of a TRACE operation on this path.
        /// </summary>
        public Operation Trace { get; }

        /// <summary>
        /// Gets the alternative server array to service all operations in this path.
        /// </summary>
        public IReadOnlyList<Server> Servers { get; }

        /// <summary>
        /// Gets the list of parameters that are applicable for all the operations described under this path.
        /// </summary>
        public IReadOnlyDictionary<ParameterKey, Referable<ParameterBody>> Parameters { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of the <see cref="Path"/> class.
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
        public Path(
            string summary = default,
            string description = default,
            Operation get = default,
            Operation put = default,
            Operation post = default,
            Operation delete = default,
            Operation options = default,
            Operation head = default,
            Operation patch = default,
            Operation trace = default,
            IReadOnlyList<Server> servers = default,
            IReadOnlyDictionary<ParameterKey, Referable<ParameterBody>> parameters = default)
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
            Servers = servers ?? Array.Empty<Server>();
            Parameters = parameters ?? ReadOnlyDictionary.Empty<ParameterKey, Referable<ParameterBody>>();
        }

        #endregion

        #region IEquatable

        /// <summary>
        /// Implements the operator == operator.
        /// </summary>
        /// <param name="path1">The path1.</param>
        /// <param name="path2">The path2.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator ==(Path path1, Path path2)
        {
            if (path1 is null && path2 is null) return true;
            if (path1 is null || path2 is null) return false;
            return path1.Equals((object)path2);
        }

        /// <summary>
        /// Implements the operator != operator.
        /// </summary>
        /// <param name="path1">The path1.</param>
        /// <param name="path2">The path2.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator !=(Path path1, Path path2)
            => !(path1 == path2);

        /// <summary>Determines whether the specified object is equal to the current object.</summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns>true if the specified object  is equal to the current object; otherwise, false.</returns>
        public override bool Equals(object obj)
            => obj is Path other
            && Equals(other);

        /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>true if the current object is equal to the <paramref name="other">other</paramref> parameter; otherwise, false.</returns>
        public virtual bool Equals(Path other)
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
        public override int GetHashCode()
        {
            unchecked
            {
                var hc = 17L;

                if (Summary != null)
                    hc = (hc * 23) + StringComparer.Ordinal.GetHashCode(Summary);
                if (Description != null)
                    hc = (hc * 23) + StringComparer.Ordinal.GetHashCode(Description);
                if (Get != null) hc = (hc * 23) + 1;
                if (Put != null) hc = (hc * 23) + 2;
                if (Post != null) hc = (hc * 23) + 3;
                if (Delete != null) hc = (hc * 23) + 4;
                if (Options != null) hc = (hc * 23) + 5;
                if (Head != null) hc = (hc * 23) + 6;
                if (Patch != null) hc = (hc * 23) + 7;
                if (Trace != null) hc = (hc * 23) + 8;
                hc = (hc * 23) + Servers.Count;
                hc = (hc * 23) + Parameters.Count;

                return ((int)(hc >> 32)) ^ (int)hc;
            }
        }

        #endregion
    }
}
