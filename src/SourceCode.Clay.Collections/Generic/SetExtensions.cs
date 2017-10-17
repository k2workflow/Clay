#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using System;
using System.Collections.Generic;

namespace SourceCode.Clay.Collections.Generic
{
    /// <summary>
    /// Represents extensions for <see cref="ISet{T}"/>.
    /// </summary>
    public static class SetExtensions
    {
        #region Methods

        /// <summary>
        /// Performs an efficient item-by-item comparison, using a custom <see cref="IEqualityComparer{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of items.</typeparam>
        /// <param name="x">Set 1</param>
        /// <param name="y">Set 2</param>
        /// <param name="comparer">The comparer to use to test for equality.</param>
        /// <returns></returns>
        public static bool NullableSetEquals<T>(this ISet<T> x, ISet<T> y, IEqualityComparer<T> comparer) // Naming avoids conflict with native ISet.SetEquals method
        {
            if (comparer == null) throw new ArgumentNullException(nameof(comparer));

            if (x is null ^ y is null) return false; // (x, null) or (null, y)
            if (x is null) return true; // (null, null)

            // Both are not null; we can now test their values
            if (ReferenceEquals(x, y)) return true; // (x, x)

            // If counts are different, not equal
            if (x.Count != y.Count) return false; // (n, m)

            // If first count is 0 then, due to previous check, the second is guaranteed to be 0 (and thus equal)
            if (x.Count == 0) return true;

            // Use native checks
            var xSet = new HashSet<T>(x);
            foreach (var yItem in y)
                if (!xSet.Remove(yItem))
                    return false;

            return xSet.Count == 0;
        }

        /// <summary>
        /// Performs an efficient item-by-item comparison, using the <see cref="IEqualityComparer{T}"/> from the first set.
        /// </summary>
        /// <typeparam name="T">The type of items.</typeparam>
        /// <param name="x">Set 1</param>
        /// <param name="y">Set 2</param>
        /// <returns></returns>
        public static bool NullableSetEquals<T>(this ISet<T> x, ISet<T> y) // Naming avoids conflict with native ISet.SetEquals method
        {
            if (x is null ^ y is null) return false; // (x, null) or (null, y)
            if (x is null) return true; // (null, null)

            // Both are not null; we can now test their values
            if (ReferenceEquals(x, y)) return true; // (x, x)

            // If counts are different, not equal
            if (x.Count != y.Count) return false; // (n, m)

            // If first count is 0 then, due to previous check, the second is guaranteed to be 0 (and thus equal)
            if (x.Count == 0) return true;

            // Use native checks
            return x.SetEquals(y); // Uses the equality comparer from the first set
        }

        #endregion
    }
}
