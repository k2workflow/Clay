#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using SourceCode.Clay.Collections.Generic;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace SourceCode.Clay.OpenApi
{
    /// <summary>
    /// An object representing a Server.
    /// </summary>
    public class OasServer : IEquatable<OasServer>
    {
        #region Properties

        /// <summary>
        /// Gets the URL to the target host.
        /// </summary>
        public Uri Url { get; }

        /// <summary>
        /// Gets the optional string describing the host designated by the URL.
        /// </summary>
        /// <remarks>
        /// CommonMark syntax MAY be used for rich text representation.
        /// </remarks>
        public string Description { get; }

        /// <summary>
        /// Gets the map between a variable name and its value.
        /// </summary>
        public IReadOnlyDictionary<string, OasServerVariable> Variables { get; }

        #endregion

        #region Constructor

        /// <summary>
        /// Creates a new instance of the <see cref="OasServer"/> class.
        /// </summary>
        /// <param name="url">The URL to the target host.</param>
        /// <param name="description">The optional string describing the host designated by the URL.</param>
        /// <param name="variables">The map between a variable name and its value.</param>
        public OasServer(
            Uri url = default,
            string description = default,
            IReadOnlyDictionary<string, OasServerVariable> variables = default)
        {
            Url = url;
            Description = description;
            Variables = variables ?? ImmutableDictionary<string, OasServerVariable>.Empty;
        }

        #endregion

        #region IEquatable

        /// <summary>
        /// Implements the operator == operator.
        /// </summary>
        /// <param name="server1">The server1.</param>
        /// <param name="server2">The server2.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator ==(OasServer server1, OasServer server2)
        {
            if (server1 is null) return server2 is null;
            return server1.Equals((object)server2);
        }

        /// <summary>
        /// Implements the operator != operator.
        /// </summary>
        /// <param name="server1">The server1.</param>
        /// <param name="server2">The server2.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator !=(OasServer server1, OasServer server2) => !(server1 == server2);

        /// <summary>Determines whether the specified object is equal to the current object.</summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns>true if the specified object  is equal to the current object; otherwise, false.</returns>
        public override bool Equals(object obj)
            => obj is OasServer other
            && Equals(other);

        /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>true if the current object is equal to the <paramref name="other">other</paramref> parameter; otherwise, false.</returns>
        public virtual bool Equals(OasServer other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;

            if (Url != other.Url) return false;
            if (!StringComparer.Ordinal.Equals(Description, other.Description)) return false;
            if (!Variables.NullableDictionaryEquals(other.Variables)) return false;

            return true;
        }

        /// <summary>Serves as the default hash function.</summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode() => HashCode.Combine(
            Url,
            StringComparer.Ordinal.GetHashCode(Description ?? string.Empty),
            Variables.Count
        );

        #endregion
    }
}
