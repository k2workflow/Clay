#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime;
using System.Runtime.CompilerServices;

namespace SourceCode.Clay
{
#pragma warning disable CA1815 // Override equals and operator equals on value types
#pragma warning disable S1206 // "Equals(Object)" and "GetHashCode()" should be overridden in pairs

    /// <summary>
    /// Represents a way to create high quality hashcodes.
    /// </summary>
    [DebuggerDisplay("{_length,nq}")]
    public unsafe struct HashCode
    {
        private const uint QueueOffset = 4;

        // TODO: Remove when https://github.com/dotnet/corefx/issues/14354 lands

        private const uint Prime1 = 2654435761U;
        private const uint Prime2 = 2246822519U;
        private const uint Prime3 = 3266489917U;
        private const uint Prime4 = 0668265263U;
        private const uint Prime5 = 0374761393U;

        #region Fields

        private readonly uint _seed;

        private ulong _length;
        private byte _queuePosition;

        // 0 -> 4 = v1 -> v4
        // 4 -> 7 = queue
        private fixed uint _state[7];

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new <see cref="HashCode"/> value.
        /// </summary>
        /// <param name="seed">The seed for the <see cref="HashCode"/>.</param>
        public HashCode(int seed)
            : this()
        {
            _seed = unchecked((uint)seed);
        }

        #endregion

        #region Add

        /// <summary>
        /// Adds the specified hashcode into this <see cref="HashCode"/>.
        /// </summary>
        /// <param name="value">The hashcode to add.</param>
        /// <returns>The <see cref="HashCode"/> value with the additional hashcode.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [TargetedPatchingOptOut("Performance critical for inlining across NGen images.")]
        public void Add(int value)
        {
            unchecked
            {
                var val = (uint)value;

                fixed (uint* v = _state)
                {
                    var q = v + QueueOffset;

                    if (_queuePosition < 3)
                    {
                        q[_queuePosition] = val;
                        ++_queuePosition;
                    }
                    else
                    {
                        Initialize(v);

                        v[0] = FullRound(v[0], q[0]);
                        v[1] = FullRound(v[1], q[1]);
                        v[2] = FullRound(v[2], q[2]);
                        v[3] = FullRound(v[3], val);

                        _queuePosition = 0;
                    }
                }

                ++_length;
            }
        }

        /// <summary>
        /// Initializes the four state variables.
        /// </summary>
        /// <param name="v">A pointer to the first state variable.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [TargetedPatchingOptOut("Performance critical for inlining across NGen images.")]
        private void Initialize(uint* v)
        {
            unchecked
            {
                if (_length > 3) return;

                var seed = _seed;
                v[0] = seed + Prime1 + Prime2;
                v[1] = seed + Prime2;
                v[2] = seed + 0;
                v[3] = seed - Prime1;
            }
        }

        /// <summary>
        /// Converts this <see cref="HashCode"/> to a <see cref="int"/> hashcode.
        /// </summary>
        /// <returns>The <see cref="int"/> hashcode.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [TargetedPatchingOptOut("Performance critical for inlining across NGen images.")]
        public int ToHashCode()
        {
            unchecked
            {
                fixed (uint* v = _state)
                {
                    var q = v + QueueOffset;

                    var hash = _length > 3
                        ? MixState(v)
                        : MixEmptyState();

                    for (var i = 0; i < _queuePosition; i++)
                        hash = PartialRound(hash, q[i]);

                    // Final mix

                    hash ^= hash >> 15;
                    hash *= Prime2;
                    hash ^= hash >> 13;
                    hash *= Prime3;
                    hash ^= hash >> 16;

                    return (int)hash;
                }
            }
        }

        /// <summary>
        /// Performs a mix on an empty state.
        /// </summary>
        /// <returns>The hash.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [TargetedPatchingOptOut("Performance critical for inlining across NGen images.")]
        private uint MixEmptyState()
        {
            var hash = _seed + Prime5;
            hash += (uint)(_length * 4);
            return hash;
        }

        /// <summary>
        /// Performs a mix on a state where chunks have been consumed.
        /// </summary>
        /// <param name="v">A pointer to the first state variable.</param>
        /// <returns>The hash.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [TargetedPatchingOptOut("Performance critical for inlining across NGen images.")]
        private uint MixState(uint* v)
        {
            var hash = RotateLeft(v[0], 1) + RotateLeft(v[1], 7) + RotateLeft(v[2], 12) + RotateLeft(v[3], 18);
            hash += (uint)(_length * 4);
            return hash;
        }

        /// <summary>
        /// Performs a full round. A full round occurs in chunks of 16 bytes.
        /// </summary>
        /// <param name="seed">The current seed.</param>
        /// <param name="input">The input value.</param>
        /// <returns>The new seed.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [TargetedPatchingOptOut("Performance critical for inlining across NGen images.")]
        private uint FullRound(uint seed, uint input)
        {
            unchecked
            {
                seed += input * Prime2;
                seed = RotateLeft(seed, 13);
                seed *= Prime1;
                return seed;
            }
        }

        /// <summary>
        /// Performs a partial round. A full round occurs outside of chunks of 16 bytes.
        /// </summary>
        /// <param name="seed">The current seed.</param>
        /// <param name="input">The input value.</param>
        /// <returns>The new seed.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [TargetedPatchingOptOut("Performance critical for inlining across NGen images.")]
        private uint PartialRound(uint seed, uint input)
        {
            unchecked
            {
                seed += input * Prime3;
                seed = RotateLeft(seed, 17) * Prime4;
                return seed;
            }
        }

        /// <summary>
        /// Rotates the specified integer left.
        /// </summary>
        /// <param name="value">The value to rotate.</param>
        /// <param name="count">The number of bits to rotate by.</param>
        /// <returns>The rotated value.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [TargetedPatchingOptOut("Performance critical for inlining across NGen images.")]
        private static uint RotateLeft(uint value, int count)
            => (value << count) | (value >> (32 - count));

        /// <summary>
        /// Adds the hashcode of the specified value into this <see cref="HashCode"/>.
        /// </summary>
        /// <param name="value">The value to hash and add.</param>
        /// <returns>The <see cref="HashCode"/> value with the additional hashcode.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [TargetedPatchingOptOut("Performance critical for inlining across NGen images.")]
        public void Add<T>(T value) => Add(value, EqualityComparer<T>.Default);

        /// <summary>
        /// Adds the hashcode of the specified value into this <see cref="HashCode"/>.
        /// </summary>
        /// <param name="value">The value to hash and add.</param>
        /// <param name="comparer">The equality comparer to use.</param>
        /// <returns>The <see cref="HashCode"/> value with the additional hash code.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [TargetedPatchingOptOut("Performance critical for inlining across NGen images.")]
        public void Add<T>(T value, IEqualityComparer<T> comparer)
        {
            if (comparer is null) throw new ArgumentNullException(nameof(comparer));
            Add(comparer.GetHashCode(value));
        }

        #endregion

        #region Static Methods

#       pragma warning disable S2436 // Classes and methods should not have too many generic parameters

        /// <summary>
        /// Creates a hash code from the specified object.
        /// </summary>
        /// <typeparam name="T1">The type of the object.</typeparam>
        /// <param name="value1">The object.</param>
        /// <returns>The hash code.</returns>
        public static int Combine<T1>(T1 value1)
        {
            var hc = new HashCode();
            hc.Add(value1);
            return hc.ToHashCode();
        }

        /// <summary>
        /// Creates a hash code from the specified objects.
        /// </summary>
        /// <typeparam name="T1">The type of the first object.</typeparam>
        /// <typeparam name="T2">The type of the second object.</typeparam>
        /// <param name="value1">The first object.</param>
        /// <param name="value2">The second object.</param>
        /// <returns>The hash code.</returns>
        public static int Combine<T1, T2>(T1 value1, T2 value2)
        {
            var hc = new HashCode();
            hc.Add(value1);
            hc.Add(value2);
            return hc.ToHashCode();
        }

        /// <summary>
        /// Creates a hash code from the specified objects.
        /// </summary>
        /// <typeparam name="T1">The type of the first object.</typeparam>
        /// <typeparam name="T2">The type of the second object.</typeparam>
        /// <typeparam name="T3">The type of the third object.</typeparam>
        /// <param name="value1">The first object.</param>
        /// <param name="value2">The second object.</param>
        /// <param name="value3">The third object.</param>
        /// <returns>The hash code.</returns>
        public static int Combine<T1, T2, T3>(T1 value1, T2 value2, T3 value3)
        {
            var hc = new HashCode();
            hc.Add(value1);
            hc.Add(value2);
            hc.Add(value3);
            return hc.ToHashCode();
        }

        /// <summary>
        /// Creates a hash code from the specified objects.
        /// </summary>
        /// <typeparam name="T1">The type of the first object.</typeparam>
        /// <typeparam name="T2">The type of the second object.</typeparam>
        /// <typeparam name="T3">The type of the third object.</typeparam>
        /// <typeparam name="T4">The type of the fourth object.</typeparam>
        /// <param name="value1">The first object.</param>
        /// <param name="value2">The second object.</param>
        /// <param name="value3">The third object.</param>
        /// <param name="value4">The fourth object.</param>
        /// <returns>The hash code.</returns>
        public static int Combine<T1, T2, T3, T4>(T1 value1, T2 value2, T3 value3, T4 value4)
        {
            var hc = new HashCode();
            hc.Add(value1);
            hc.Add(value2);
            hc.Add(value3);
            hc.Add(value4);
            return hc.ToHashCode();
        }

        /// <summary>
        /// Creates a hash code from the specified objects.
        /// </summary>
        /// <typeparam name="T1">The type of the first object.</typeparam>
        /// <typeparam name="T2">The type of the second object.</typeparam>
        /// <typeparam name="T3">The type of the third object.</typeparam>
        /// <typeparam name="T4">The type of the fourth object.</typeparam>
        /// <typeparam name="T5">The type of the fifth object.</typeparam>
        /// <param name="value1">The first object.</param>
        /// <param name="value2">The second object.</param>
        /// <param name="value3">The third object.</param>
        /// <param name="value4">The fourth object.</param>
        /// <param name="value5">The fifth object.</param>
        /// <returns>The hash code.</returns>
        public static int Combine<T1, T2, T3, T4, T5>(T1 value1, T2 value2, T3 value3, T4 value4, T5 value5)
        {
            var hc = new HashCode();
            hc.Add(value1);
            hc.Add(value2);
            hc.Add(value3);
            hc.Add(value4);
            hc.Add(value5);
            return hc.ToHashCode();
        }

        /// <summary>
        /// Creates a hash code from the specified objects.
        /// </summary>
        /// <typeparam name="T1">The type of the first object.</typeparam>
        /// <typeparam name="T2">The type of the second object.</typeparam>
        /// <typeparam name="T3">The type of the third object.</typeparam>
        /// <typeparam name="T4">The type of the fourth object.</typeparam>
        /// <typeparam name="T5">The type of the fifth object.</typeparam>
        /// <typeparam name="T6">The type of the sixth object.</typeparam>
        /// <param name="value1">The first object.</param>
        /// <param name="value2">The second object.</param>
        /// <param name="value3">The third object.</param>
        /// <param name="value4">The fourth object.</param>
        /// <param name="value5">The fifth object.</param>
        /// <param name="value6">The sixth object.</param>
        /// <returns>The hash code.</returns>
        public static int Combine<T1, T2, T3, T4, T5, T6>(T1 value1, T2 value2, T3 value3, T4 value4, T5 value5, T6 value6)
        {
            var hc = new HashCode();
            hc.Add(value1);
            hc.Add(value2);
            hc.Add(value3);
            hc.Add(value4);
            hc.Add(value5);
            hc.Add(value6);
            return hc.ToHashCode();
        }

        /// <summary>
        /// Creates a hash code from the specified objects.
        /// </summary>
        /// <typeparam name="T1">The type of the first object.</typeparam>
        /// <typeparam name="T2">The type of the second object.</typeparam>
        /// <typeparam name="T3">The type of the third object.</typeparam>
        /// <typeparam name="T4">The type of the fourth object.</typeparam>
        /// <typeparam name="T5">The type of the fifth object.</typeparam>
        /// <typeparam name="T6">The type of the fifth object.</typeparam>
        /// <typeparam name="T7">The type of the seventh object.</typeparam>
        /// <param name="value1">The first object.</param>
        /// <param name="value2">The second object.</param>
        /// <param name="value3">The third object.</param>
        /// <param name="value4">The fourth object.</param>
        /// <param name="value5">The fifth object.</param>
        /// <param name="value6">The sixth object.</param>
        /// <param name="value7">The seventh object.</param>
        /// <returns>The hash code.</returns>
        public static int Combine<T1, T2, T3, T4, T5, T6, T7>(T1 value1, T2 value2, T3 value3, T4 value4, T5 value5, T6 value6, T7 value7)
        {
            var hc = new HashCode();
            hc.Add(value1);
            hc.Add(value2);
            hc.Add(value3);
            hc.Add(value4);
            hc.Add(value5);
            hc.Add(value6);
            hc.Add(value7);
            return hc.ToHashCode();
        }

        /// <summary>
        /// Creates a hash code from the specified objects.
        /// </summary>
        /// <typeparam name="T1">The type of the first object.</typeparam>
        /// <typeparam name="T2">The type of the second object.</typeparam>
        /// <typeparam name="T3">The type of the third object.</typeparam>
        /// <typeparam name="T4">The type of the fourth object.</typeparam>
        /// <typeparam name="T5">The type of the fifth object.</typeparam>
        /// <typeparam name="T6">The type of the fifth object.</typeparam>
        /// <typeparam name="T7">The type of the seventh object.</typeparam>
        /// <typeparam name="T8">The type of the eighth object.</typeparam>
        /// <param name="value1">The first object.</param>
        /// <param name="value2">The second object.</param>
        /// <param name="value3">The third object.</param>
        /// <param name="value4">The fourth object.</param>
        /// <param name="value5">The fifth object.</param>
        /// <param name="value6">The sixth object.</param>
        /// <param name="value7">The seventh object.</param>
        /// <param name="value8">The eighth object.</param>
        /// <returns>The hash code.</returns>
        public static int Combine<T1, T2, T3, T4, T5, T6, T7, T8>(T1 value1, T2 value2, T3 value3, T4 value4, T5 value5, T6 value6, T7 value7, T8 value8)
        {
            var hc = new HashCode();
            hc.Add(value1);
            hc.Add(value2);
            hc.Add(value3);
            hc.Add(value4);
            hc.Add(value5);
            hc.Add(value6);
            hc.Add(value7);
            hc.Add(value8);
            return hc.ToHashCode();
        }

#       pragma warning restore S2436 // Classes and methods should not have too many generic parameters

        #endregion

        #region Operators

#pragma warning disable CS0809 // Obsolete member overrides non-obsolete member

        /// <summary>
        /// Converts this <see cref="HashCode"/> to a <see cref="int"/> hashcode.
        /// </summary>
        /// <returns>The <see cref="int"/> hashcode.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [TargetedPatchingOptOut("Performance critical for inlining across NGen images.")]
        [Obsolete("Use ToHashCode to retrieve the computed hash code.", error: true)]
        public override int GetHashCode() => ToHashCode();

#pragma warning restore CS0809 // Obsolete member overrides non-obsolete member

        #endregion
    }

#pragma warning restore S1206 // "Equals(Object)" and "GetHashCode()" should be overridden in pairs
#pragma warning restore CA1815 // Override equals and operator equals on value types
}
