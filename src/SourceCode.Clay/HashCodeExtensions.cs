#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using System.Collections.Generic;
using System.Runtime;
using System.Runtime.CompilerServices;

namespace SourceCode.Clay
{
    /// <summary>
    /// Represents fluent extensions for <see cref="HashCode"/>.
    /// </summary>
    public static class HashCodeExtensions
    {
        #region Methods

        /// <summary>
        /// Adds a hash code to the specified <see cref="HashCode"/> and
        /// returns the changed <see cref="HashCode"/> value.
        /// </summary>
        /// <param name="hashCode">The <see cref="HashCode"/> to change.</param>
        /// <param name="value">The hash code to add to <paramref name="hashCode"/>.</param>
        /// <returns>The changed <see cref="HashCode"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [TargetedPatchingOptOut("Performance critical for inlining across NGen images.")]
        public static HashCode Tally(this HashCode hashCode, int value)
        {
            hashCode.Add(value);
            return hashCode;
        }

        /// <summary>
        /// Computes a hash code for the specified value and adds it to the specified
        /// <see cref="HashCode"/>, then returns the changed <see cref="HashCode"/> value.
        /// </summary>
        /// <typeparam name="T">The type of value.</typeparam>
        /// <param name="hashCode">The <see cref="HashCode"/> to change.</param>
        /// <param name="value">The value to hash and add to <paramref name="hashCode"/>.</param>
        /// <returns>The changed <see cref="HashCode"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [TargetedPatchingOptOut("Performance critical for inlining across NGen images.")]
        public static HashCode Tally<T>(this HashCode hashCode, T value)
        {
            hashCode.Add(value);
            return hashCode;
        }

        /// <summary>
        /// Computes a hash code for the specified value and adds it to the specified
        /// <see cref="HashCode"/>, then returns the changed <see cref="HashCode"/> value.
        /// </summary>
        /// <typeparam name="T">The type of value.</typeparam>
        /// <param name="hashCode">The <see cref="HashCode"/> to change.</param>
        /// <param name="value">The value to hash and add to <paramref name="hashCode"/>.</param>
        /// <param name="comparer">The comparer to use.</param>
        /// <returns>The changed <see cref="HashCode"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [TargetedPatchingOptOut("Performance critical for inlining across NGen images.")]
        public static HashCode Tally<T>(this HashCode hashCode, T value, IEqualityComparer<T> comparer)
        {
            hashCode.Add(value, comparer);
            return hashCode;
        }

        /// <summary>
        /// Adds the number of items in the specified collection to the specified <see cref="HashCode"/>.
        /// </summary>
        /// <param name="hashCode">The <see cref="HashCode"/> to change.</param>
        /// <param name="collection">The collection to add.</param>
        /// <returns>The changed <see cref="HashCode"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [TargetedPatchingOptOut("Performance critical for inlining across NGen images.")]
        public static HashCode TallyCount(this HashCode hashCode, System.Collections.ICollection collection)
        {
            if (collection == null) hashCode.Add(0);
            else hashCode.Add(collection.Count);
            return hashCode;
        }

        /// <summary>
        /// Adds the number of items in the specified array to the specified <see cref="HashCode"/>.
        /// </summary>
        /// <typeparam name="T">The type of items in the collection.</typeparam>
        /// <param name="hashCode">The <see cref="HashCode"/> to change.</param>
        /// <param name="array">The array to add.</param>
        /// <returns>The changed <see cref="HashCode"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [TargetedPatchingOptOut("Performance critical for inlining across NGen images.")]
        public static HashCode TallyCount<T>(this HashCode hashCode, T[] array)
        {
            if (array == null) hashCode.Add(0);
            else hashCode.Add(array.Length);
            return hashCode;
        }

        /// <summary>
        /// Adds the number of items in the specified collection to the specified <see cref="HashCode"/>.
        /// </summary>
        /// <typeparam name="T">The type of items in the collection.</typeparam>
        /// <param name="hashCode">The <see cref="HashCode"/> to change.</param>
        /// <param name="collection">The collection to add.</param>
        /// <returns>The changed <see cref="HashCode"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [TargetedPatchingOptOut("Performance critical for inlining across NGen images.")]
        public static HashCode TallyCount<T>(this HashCode hashCode, ICollection<T> collection)
        {
            if (collection == null) hashCode.Add(0);
            else hashCode.Add(collection.Count);
            return hashCode;
        }

        /// <summary>
        /// Adds the number of items in the specified collection to the specified <see cref="HashCode"/>.
        /// </summary>
        /// <typeparam name="T">The type of items in the collection.</typeparam>
        /// <param name="hashCode">The <see cref="HashCode"/> to change.</param>
        /// <param name="collection">The collection to add.</param>
        /// <returns>The changed <see cref="HashCode"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [TargetedPatchingOptOut("Performance critical for inlining across NGen images.")]
        public static HashCode TallyCount<T>(this HashCode hashCode, IReadOnlyCollection<T> collection)
        {
            if (collection == null) hashCode.Add(0);
            else hashCode.Add(collection.Count);
            return hashCode;
        }

        #endregion
    }
}
