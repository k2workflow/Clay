#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using System.Collections.Generic;

namespace SourceCode.Clay.Json
{
    /// <summary>
    /// Represents a way to compare <see cref="ReadOnlyJObject"/> values.
    /// </summary>
    public abstract class ReadOnlyJObjectComparer : IEqualityComparer<ReadOnlyJObject>
    {
        #region Constants

        /// <summary>
        /// Gets a <see cref="ReadOnlyJObjectComparer"/> that compares all fields of a <see cref="ReadOnlyJObject"/>
        /// value in a strict manner (ordinal string comparisons, deterministic ordering of members).
        /// </summary>
        public static IEqualityComparer<ReadOnlyJObject> Default { get; } = new StrictComparer();

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of the <see cref="ReadOnlyJObjectComparer"/> class.
        /// </summary>
        protected ReadOnlyJObjectComparer()
        { }

        #endregion

        #region IEqualityComparer

        /// <summary>
        /// Determines whether the specified objects are equal.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>
        /// true if the specified objects are equal; otherwise, false.
        /// </returns>
        public abstract bool Equals(ReadOnlyJObject x, ReadOnlyJObject y);

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.
        /// </returns>
        public abstract int GetHashCode(ReadOnlyJObject obj);

        #endregion

        #region Concrete

        /// <summary>
        /// Represents a way to compare two <see cref="JObject"/> or <see cref="ReadOnlyJObject"/> instances
        /// in a case-sensitive and order-sensitive manner.
        /// </summary>
        private sealed class StrictComparer : ReadOnlyJObjectComparer
        {
            #region Constructors

            /// <inheritdoc/>
            internal StrictComparer()
            { }

            #endregion

            #region Methods

            /// <inheritdoc/>
            public override bool Equals(ReadOnlyJObject x, ReadOnlyJObject y) => JTokenComparer.Default.Equals(x?._json, y?._json);

            /// <inheritdoc/>
            public override int GetHashCode(ReadOnlyJObject obj) => JTokenComparer.Default.GetHashCode(obj?._json);

            #endregion
        }

        #endregion
    }
}
