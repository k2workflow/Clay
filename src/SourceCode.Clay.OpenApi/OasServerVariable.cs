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
    /// An object representing a Server Variable for server URL template substitution.
    /// </summary>
    public class OasServerVariable : IEquatable<OasServerVariable>
    {
        #region Properties

        /// <summary>
        /// Gets the enumeration of string values to be used if the substitution options are from a limited set.
        /// </summary>
        public IReadOnlyList<string> Enum { get; }

        /// <summary>
        /// Gets the default value to use for substitution, and to send, if an alternate value is not supplied.
        /// </summary>
        public string Default { get; }

        /// <summary>
        /// Gets the description for the server variable.
        /// </summary>
        /// <remarks>
        /// CommonMark syntax MAY be used for rich text representation.
        /// </remarks>
        public string Description { get; }

        #endregion

        #region Constructor

        /// <summary>
        /// Creates a new instance of the <see cref="OasServerVariable"/> class.
        /// </summary>
        /// <param name="enum">The enumeration of string values to be used if the substitution options are from a limited set.</param>
        /// <param name="default">The default value to use for substitution, and to send, if an alternate value is not supplied. </param>
        /// <param name="description">The description for the server variable.</param>
        public OasServerVariable(
            IReadOnlyList<string> @enum = default,
            string @default = default,
            string description = default)
        {
            Enum = @enum ?? Array.Empty<string>();
            Default = @default;
            Description = description;
        }

        #endregion

        #region IEquatable

        /// <summary>
        /// Implements the operator == operator.
        /// </summary>
        /// <param name="serverVariable1">The server variable1.</param>
        /// <param name="serverVariable2">The server variable2.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator ==(OasServerVariable serverVariable1, OasServerVariable serverVariable2)
        {
            if (serverVariable1 is null) return serverVariable2 is null;
            return serverVariable1.Equals((object)serverVariable2);
        }

        /// <summary>
        /// Implements the operator != operator.
        /// </summary>
        /// <param name="serverVariable1">The server variable1.</param>
        /// <param name="serverVariable2">The server variable2.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator !=(OasServerVariable serverVariable1, OasServerVariable serverVariable2)
                            => !(serverVariable1 == serverVariable2);

        /// <summary>Determines whether the specified object is equal to the current object.</summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns>true if the specified object  is equal to the current object; otherwise, false.</returns>
        public override bool Equals(object obj)
            => obj is OasServerVariable other
            && Equals(other);

        /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>true if the current object is equal to the <paramref name="other">other</paramref> parameter; otherwise, false.</returns>
        public virtual bool Equals(OasServerVariable other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;

            if (!Enum.NullableSetEquals(other.Enum)) return false;
            if (!StringComparer.Ordinal.Equals(Default, other.Default)) return false;
            if (!StringComparer.Ordinal.Equals(Description, other.Description)) return false;

            return true;
        }

        /// <summary>Serves as the default hash function.</summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode() => HashCode.Combine(
            Enum.Count,
            StringComparer.Ordinal.GetHashCode(Default ?? string.Empty),
            StringComparer.Ordinal.GetHashCode(Description ?? string.Empty)
        );

        #endregion
    }
}
