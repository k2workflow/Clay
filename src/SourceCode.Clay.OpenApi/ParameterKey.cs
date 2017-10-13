#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using System;

namespace SourceCode.Clay.OpenApi
{
    /// <summary>
    /// Represents a parameter key.
    /// </summary>
    public struct ParameterKey : IEquatable<ParameterKey>
    {
        #region Properties

        /// <summary>
        /// Gets the name of the parameter.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the location of the parameter.
        /// </summary>
        public ParameterLocation Location { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new <see cref="ParameterKey"/> value.
        /// </summary>
        /// <param name="name">The name of the parameter.</param>
        /// <param name="location">The location of the parameter.</param>
        public ParameterKey(
            string name = default,
            ParameterLocation location = default)
            : this()
        {
            Name = name;
            Location = location;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Implements the operator == operator.
        /// </summary>
        /// <param name="key1">The key1.</param>
        /// <param name="key2">The key2.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator ==(ParameterKey key1, ParameterKey key2) => key1.Equals(key2);

        /// <summary>
        /// Implements the operator != operator.
        /// </summary>
        /// <param name="key1">The key1.</param>
        /// <param name="key2">The key2.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator !=(ParameterKey key1, ParameterKey key2)
            => !(key1 == key2);

        /// <summary>Indicates whether this instance and a specified object are equal.</summary>
        /// <param name="obj">The object to compare with the current instance.</param>
        /// <returns>true if <paramref name="obj">obj</paramref> and this instance are the same type and represent the same value; otherwise, false.</returns>
        public override bool Equals(object obj) => obj is ParameterKey o && Equals(o);

        /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>true if the current object is equal to the <paramref name="other">other</paramref> parameter; otherwise, false.</returns>
        public bool Equals(ParameterKey other)
        {
            if (Location != other.Location) return false;
            if (!StringComparer.Ordinal.Equals(Name, other.Name)) return false;

            return true;
        }

        /// <summary>Returns the hash code for this instance.</summary>
        /// <returns>A 32-bit signed integer that is the hash code for this instance.</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                var hc = 17L;

                if (Name != null)
                    hc = (hc * 23) + StringComparer.Ordinal.GetHashCode(Name);
                hc = hc * 17 + Location.GetHashCode();

                return ((int)(hc >> 32)) ^ (int)hc;
            }
        }

        #endregion
    }
}
