#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using System;
using System.Collections.Generic;

namespace SourceCode.Clay.Buffers
{
    /// <summary>
    /// Represents a way to compare binary buffers.
    /// </summary>
    public abstract class BufferComparer<T> : IEqualityComparer<T>, IComparer<T>
    {
        /// <summary>
        /// The prefix of the buffer that is considered for hashcode calculation.
        /// </summary>
        public int HashCodeFidelity { get; }

        /// <summary>
        /// Creates a new instance of the <see cref="BufferComparer"/> class.
        /// </summary>
        /// <param name="hashCodeFidelity">
        /// The maximum number of octets processed when calculating a hashcode.
        /// Pass zero to disable the limit.
        /// </param>
        private protected BufferComparer(int hashCodeFidelity)
        {
            if (hashCodeFidelity < 0) throw new ArgumentOutOfRangeException(nameof(hashCodeFidelity));

            HashCodeFidelity = hashCodeFidelity;
        }

        /// <summary>
        /// Creates a new instance of the <see cref="BufferComparer"/> class, that considers the full
        /// buffer when calculating the hashcode.
        /// </summary>
        private protected BufferComparer()
        {
            HashCodeFidelity = 0;
        }

        public abstract int Compare(T x, T y);

        /// <summary>
        /// Determines whether the specified objects are equal.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>
        /// true if the specified objects are equal; otherwise, false.
        /// </returns>
        public abstract bool Equals(T x, T y);

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.
        /// </returns>
        public abstract int GetHashCode(T obj);
    }
}
