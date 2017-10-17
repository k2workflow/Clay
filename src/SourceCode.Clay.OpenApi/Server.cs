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
    /// An object representing a Server.
    /// </summary>
    public class Server : IEquatable<Server>
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
        public IReadOnlyDictionary<string, ServerVariable> Variables { get; }

        #endregion

        #region Constructor

        /// <summary>
        /// Creates a new instance of the <see cref="Server"/> class.
        /// </summary>
        /// <param name="url">The URL to the target host.</param>
        /// <param name="description">The optional string describing the host designated by the URL.</param>
        /// <param name="variables">The map between a variable name and its value.</param>
        public Server(
            Uri url = default,
            string description = default,
            IReadOnlyDictionary<string, ServerVariable> variables = default)
        {
            Url = url;
            Description = description;
            Variables = variables ?? ReadOnlyDictionary.Empty<string, ServerVariable>();
        }

        #endregion

        #region IEquatable

        /// <summary>
        /// Implements the operator == operator.
        /// </summary>
        /// <param name="server1">The server1.</param>
        /// <param name="server2">The server2.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator ==(Server server1, Server server2)
        {
            if (server1 is null && server2 is null) return true;
            if (server1 is null || server2 is null) return false;
            return server1.Equals((object)server2);
        }

        /// <summary>
        /// Implements the operator != operator.
        /// </summary>
        /// <param name="server1">The server1.</param>
        /// <param name="server2">The server2.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator !=(Server server1, Server server2) => !(server1 == server2);

        /// <summary>Determines whether the specified object is equal to the current object.</summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns>true if the specified object  is equal to the current object; otherwise, false.</returns>
        public override bool Equals(object obj) => Equals(obj as Server);

        /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>true if the current object is equal to the <paramref name="other">other</paramref> parameter; otherwise, false.</returns>
        public virtual bool Equals(Server other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;

            if (Url != other.Url) return false;
            if (!StringComparer.Ordinal.Equals(Description, other.Description)) return false;
            if (!Variables.DictionaryEquals(other.Variables)) return false;

            return true;
        }

        /// <summary>Serves as the default hash function.</summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                var hc = 17L;

                if (Url != null)
                    hc = (hc * 23) + Url.GetHashCode();
                if (Description != null)
                    hc = (hc * 23) + StringComparer.Ordinal.GetHashCode(Description);
                hc = (hc * 23) + Variables.Count;

                return ((int)(hc >> 32)) ^ (int)hc;
            }
        }

        #endregion
    }
}
