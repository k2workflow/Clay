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
    /// <remarks>
    /// The <c>Parameters</c> and <c>RequestBody</c> parameters are implementation-specific. If they are required, create a new type
    /// that inherits from this one.
    /// </remarks>
    public class Link : IEquatable<Link>
    {
        #region Properties

        /// <summary>
        /// Gets the relative or absolute reference to an OAS operation.
        /// </summary>
        public Reference OperationReference { get; }

        /// <summary>
        /// Gets the name of an existing, resolvable OAS operation, as defined with a unique operation identifier.
        /// </summary>
        public string OperationIdentifier { get; set; }

        /// <summary>
        /// Get the description of the link.
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
        /// Creates a new instance of the <see cref="Link"/> class.
        /// </summary>
        /// <param name="operationReference">The relative or absolute reference to an OAS operation.</param>
        /// <param name="operationIdentifier">The name of an existing, resolvable OAS operation, as defined with a unique operation identifier.</param>
        /// <param name="description">The description of the link.</param>
        /// <param name="server">The server object to be used by the target operation.</param>
        public Link(
            Reference operationReference = default,
            string operationIdentifier = default,
            string description = default,
            Server server = default)
        {
            OperationReference = operationReference;
            OperationIdentifier = operationIdentifier;
            Description = description;
            Server = server;
        }

        #endregion

        #region IEquatable

        /// <summary>
        /// Implements the operator == operator.
        /// </summary>
        /// <param name="link1">The link1.</param>
        /// <param name="link2">The link2.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator ==(Link link1, Link link2)
        {
            if (link1 is null && link2 is null) return true;
            if (link1 is null || link2 is null) return false;
            return link1.Equals((object)link2);
        }

        /// <summary>
        /// Implements the operator != operator.
        /// </summary>
        /// <param name="link1">The link1.</param>
        /// <param name="link2">The link2.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator !=(Link link1, Link link2)
            => !(link1 == link2);

        /// <summary>Determines whether the specified object is equal to the current object.</summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns>true if the specified object  is equal to the current object; otherwise, false.</returns>
        public override bool Equals(object obj)
            => obj is Link other
            && Equals(other);

        /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>true if the current object is equal to the <paramref name="other">other</paramref> parameter; otherwise, false.</returns>
        public virtual bool Equals(Link other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;

            if (!OperationReference.Equals(other.OperationReference)) return false;
            if (!StringComparer.Ordinal.Equals(OperationIdentifier, other.OperationIdentifier)) return false;
            if (!StringComparer.Ordinal.Equals(Description, other.Description)) return false;
            if (!Server.NullableEquals(other.Server)) return false;

            return true;
        }

        /// <summary>Serves as the default hash function.</summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                var hc = 21L;

                hc = hc * 17 + OperationReference.GetHashCode();
                if (OperationIdentifier != null)
                    hc = hc * 17 + StringComparer.Ordinal.GetHashCode(OperationIdentifier);
                if (Description != null)
                    hc = hc * 17 + StringComparer.Ordinal.GetHashCode(Description);
                if (Server != null)
                    hc = hc * 17 + Server.GetHashCode();

                return ((int)(hc >> 32)) ^ (int)hc;
            }
        }

        #endregion
    }
}
