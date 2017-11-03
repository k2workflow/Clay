#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace SourceCode.Clay.Json
{
    /// <summary>
    /// Represents a way to compare <see cref="JToken"/> and <see cref="ReadOnlyJObject"/> values.
    /// </summary>
    public abstract class JObjectComparer :
        IEqualityComparer<JToken>,
        IEqualityComparer<ReadOnlyJObject>
    {
        #region Constants

        private static readonly JTokenEqualityComparer jtokenComparer = new JTokenEqualityComparer();

        /// <summary>
        /// Gets a <see cref="JObjectComparer"/> that compares all fields of a <see cref="JToken"/>
        /// value in a strict manner (ordinal string comparisons, determinisitc ordering of members).
        /// </summary>
        public static JObjectComparer Default { get; } = new JsonStrictComparer();

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of the <see cref="JObjectComparer"/> class.
        /// </summary>
        protected JObjectComparer()
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
        public abstract bool Equals(JToken x, JToken y);

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
        public abstract int GetHashCode(JToken obj);

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
        /// Represents a way to compare two <see cref="JToken"/> or <see cref="ReadOnlyJObject"/> instances
        /// in a case-sensitive and order-sensitive manner.
        /// </summary>
        private sealed class JsonStrictComparer : JObjectComparer
        {
            #region Constructors

            /// <inheritdoc/>
            internal JsonStrictComparer()
            { }

            #endregion

            #region Methods

            /// <inheritdoc/>
            public override bool Equals(JToken x, JToken y) => jtokenComparer.Equals(x, y);

            /// <inheritdoc/>
            public override bool Equals(ReadOnlyJObject x, ReadOnlyJObject y) => jtokenComparer.Equals(x?._json, y?._json);

            /// <inheritdoc/>
            public override int GetHashCode(JToken obj) => jtokenComparer.GetHashCode(obj);

            /// <inheritdoc/>
            public override int GetHashCode(ReadOnlyJObject obj) => jtokenComparer.GetHashCode(obj?._json);

            #endregion
        }

        #endregion
    }
}
