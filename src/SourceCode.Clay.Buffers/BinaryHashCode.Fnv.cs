#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security;

namespace SourceCode.Clay.Buffers
{
    /// <summary>
    /// Represents a way to calculate hash codes.
    /// </summary>
    public static partial class BinaryHashCode // .Fnv
    {
        #region Constants

        private const uint FnvOffsetBasis = 0x811C9DC5u;
        private const uint FnvPrime = 0x01000193u;

        // Same result as full calculation would give for an empty buffer
        internal const int FnvEmpty = unchecked((int)FnvOffsetBasis); // Marked internal for Unit visibility

        // Chosen hash for a null input. Thanks for all the fish
        internal const int FnvNull = -42; // Marked internal for Unit visibility

        #endregion

        #region Root Implementations

        /// <summary>
        /// Calculates a hash code from the specified data using the
        /// Fowler/Noll/Vo 1-a algorithm.
        /// </summary>
        /// <param name="buffer">The array containing the range of bytes to include in the hash code.</param>
        /// <returns>
        /// The hash code.
        /// </returns>
        public static int Fnv(params byte[] buffer)
        {
            if (buffer == null) return FnvNull;
            if (buffer.Length == 0) return FnvEmpty;

            var hc = FnvOffsetBasis;

            unchecked
            {
                for (var i = 0; i < buffer.Length; i++)
                {
                    var c = buffer[i];
                    hc = (hc ^ c) * FnvPrime;
                }

                return (int)hc;
            }
        }

        /// <summary>
        /// Calculates a hash code from the specified data using the
        /// Fowler/Noll/Vo 1-a algorithm.
        /// </summary>
        /// <param name="buffer">The <see cref="ReadOnlySpan{T}"/> containing the range of bytes to include in the hash code.</param>
        /// <returns>
        /// The hash code.
        /// </returns>
        public static int Fnv(ReadOnlySpan<byte> span)
        {
            // ReadOnly/Span is a struct, nor do its ctors permit null. So null check is redundant.
            if (span.Length == 0) return FnvEmpty;

            var hc = FnvOffsetBasis;

            unchecked
            {
                for (var i = 0; i < span.Length; i++)
                {
                    var c = span[i];
                    hc = (hc ^ c) * FnvPrime;
                }

                return (int)hc;
            }
        }

        /// <summary>
        /// Calculates a hash code from the specified data using the
        /// Fowler/Noll/Vo 1-a algorithm.
        /// </summary>
        /// <param name="buffer">A pointer to the initial octet in the data.</param>
        /// <param name="count">The number of octets to include in the hash code.</param>
        /// <returns>The hash code.</returns>
        [SecurityCritical]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe int Fnv(byte* buffer, int count)
        {
            if (count < 0) throw new ArgumentOutOfRangeException(nameof(count));

            if (buffer == default) return FnvNull;
            if (count == 0) return FnvEmpty;

            var hc = FnvOffsetBasis;

            unchecked
            {
                for (var i = 0; i < count; i++)
                {
                    var c = buffer[i];
                    hc = (hc ^ c) * FnvPrime;
                }

                return (int)hc;
            }
        }

        /// <summary>
        /// Calculates a hash code from the specified data using the
        /// Fowler/Noll/Vo 1-a algorithm.
        /// </summary>
        /// <param name="fields">The fields to include in the hash code.</param>
        /// <returns>
        /// The hash code.
        /// </returns>
        [SecurityCritical]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe int Fnv(IReadOnlyList<int> fields)
        {
            if (fields == null) return FnvNull;
            if (fields.Count == 0) return FnvEmpty;

            var data = stackalloc int[1]; // TODO: https://github.com/dotnet/corefx/pull/24212
            var bdata = (byte*)data;

            var hc = FnvOffsetBasis;

            unchecked
            {
                for (var i = 0; i < fields.Count; i++)
                {
                    data[0] = fields[i];

                    hc = (hc ^ bdata[0]) * FnvPrime;
                    hc = (hc ^ bdata[1]) * FnvPrime;
                    hc = (hc ^ bdata[2]) * FnvPrime;
                    hc = (hc ^ bdata[3]) * FnvPrime;
                }

                return (int)hc;
            }
        }

        /// <summary>
        /// Calculates a hash code from the specified data using the
        /// Fowler/Noll/Vo 1-a algorithm.
        /// </summary>
        /// <param name="fields">The fields to include in the hash code.</param>
        /// <returns>
        /// The hash code.
        /// </returns>
        [SecurityCritical]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe int Fnv(IEnumerable<int> fields)
        {
            if (fields == null) return FnvNull;
            if (!fields.Any()) return FnvEmpty;

            var data = stackalloc int[1]; // TODO: https://github.com/dotnet/corefx/pull/24212
            var bdata = (byte*)data;

            var hc = FnvOffsetBasis;

            unchecked
            {
                foreach (var field in fields)
                {
                    data[0] = field;

                    hc = (hc ^ bdata[0]) * FnvPrime;
                    hc = (hc ^ bdata[1]) * FnvPrime;
                    hc = (hc ^ bdata[2]) * FnvPrime;
                    hc = (hc ^ bdata[3]) * FnvPrime;
                }

                return (int)hc;
            }
        }

        #endregion

        #region Overloads

        /// <summary>
        /// Calculates a hash code from the specified data using the
        /// Fowler/Noll/Vo 1-a algorithm.
        /// </summary>
        /// <param name="buffer">The array containing the range of bytes to include in the hash code.</param>
        /// <param name="offset">The zero-based index of the first element in the range.</param>
        /// <param name="count">The number of elements in the range.</param>
        /// <returns>
        /// The hash code.
        /// </returns>
        [SecurityCritical]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe int Fnv(byte[] buffer, int offset, int count)
        {
            if (buffer == null) return FnvNull;
            if (count == 0) return FnvEmpty;

            if (offset < 0) throw new ArgumentOutOfRangeException(nameof(offset));
            if (count < 0) throw new ArgumentOutOfRangeException(nameof(count));
            if (offset > buffer.Length) throw new ArgumentOutOfRangeException(nameof(offset));
            if (offset + count > buffer.Length) throw new ArgumentOutOfRangeException(nameof(offset));

            fixed (byte* b = &buffer[offset])
            {
                var hc = Fnv(b, count);
                return hc;
            }
        }

        /// <summary>
        /// Calculates a hash code from the specified data using the
        /// Fowler/Noll/Vo 1-a algorithm.
        /// </summary>
        /// <param name="buffer">The range of bytes to incldue in the hash code.</param>
        /// <returns>
        /// The hash code.
        /// </returns>
        [SecurityCritical]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe int Fnv(ArraySegment<byte> buffer)
        {
            if (buffer.Array == null) return FnvNull;
            if (buffer.Count == 0) return FnvEmpty;

            fixed (byte* b = &buffer.Array[buffer.Offset])
            {
                var hc = Fnv(b, buffer.Count);
                return hc;
            }
        }

        /// <summary>
        /// Calculates a hash code from the specified data using the
        /// Fowler/Noll/Vo 1-a algorithm.
        /// </summary>
        /// <param name="buffer">The range of bytes to incldue in the hash code.</param>
        /// <returns>
        /// The hash code.
        /// </returns>
        [SecurityCritical]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe int Fnv(IReadOnlyList<byte> buffer)
        {
            if (buffer == null) return FnvNull;
            if (buffer.Count == 0) return FnvEmpty;

            var hc = FnvOffsetBasis;

            unchecked
            {
                for (var i = 0; i < buffer.Count; i++)
                {
                    var c = buffer[i];
                    hc = (hc ^ c) * FnvPrime;
                }

                return (int)hc;
            }
        }

        /// <summary>
        /// Calculates a hash code from the specified data using the
        /// Fowler/Noll/Vo 1-a algorithm.
        /// </summary>
        /// <param name="buffer">The range of bytes to incldue in the hash code.</param>
        /// <returns>
        /// The hash code.
        /// </returns>
        [SecurityCritical]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe int Fnv(IList<byte> buffer)
        {
            if (buffer == null) return FnvNull;
            if (buffer.Count == 0) return FnvEmpty;

            var hc = FnvOffsetBasis;

            unchecked
            {
                for (var i = 0; i < buffer.Count; i++)
                {
                    var c = buffer[i];
                    hc = (hc ^ c) * FnvPrime;
                }

                return (int)hc;
            }
        }

        /// <summary>
        /// Calculates a hash code from the specified data using the
        /// Fowler/Noll/Vo 1-a algorithm.
        /// </summary>
        /// <param name="buffer">The range of bytes to incldue in the hash code.</param>
        /// <returns>
        /// The hash code.
        /// </returns>
        [SecurityCritical]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe int Fnv(IEnumerable<byte> buffer)
        {
            if (buffer == null) return FnvNull;
            if (!buffer.Any()) return FnvEmpty;

            var hc = FnvOffsetBasis;

            unchecked
            {
                foreach (var c in buffer)
                {
                    hc = (hc ^ c) * FnvPrime;
                }

                return (int)hc;
            }
        }

        /// <summary>
        /// Calculates a hash code from the specified data using the
        /// Fowler/Noll/Vo 1-a algorithm.
        /// </summary>
        /// <param name="fields">The fields to include in the hash code.</param>
        /// <returns>
        /// The hash code.
        /// </returns>
        [SecurityCritical]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe int Fnv(params int[] fields)
        {
            if (fields == null) return FnvNull;
            if (fields.Length == 0) return FnvEmpty;

            fixed (int* data = &fields[0])
            {
                var hc = Fnv((byte*)data, fields.Length * sizeof(int));
                return hc;
            }
        }

        /// <summary>
        /// Calculates a hash code from the specified data using the
        /// Fowler/Noll/Vo 1-a algorithm.
        /// </summary>
        /// <param name="a">The first hash code to include in the hash code.</param>
        /// <param name="b">The second hash code to include in the hash code.</param>
        /// <returns>
        /// The hash code.
        /// </returns>
        [SecurityCritical]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe int Fnv(int a, int b)
        {
            var data = stackalloc int[2]; // TODO: https://github.com/dotnet/corefx/pull/24212
            data[0] = a;
            data[1] = b;

            var hc = Fnv((byte*)data, 2 * sizeof(int));
            return hc;
        }

        /// <summary>
        /// Calculates a hash code from the specified data using the
        /// Fowler/Noll/Vo 1-a algorithm.
        /// </summary>
        /// <param name="a">The first hash code to include in the hash code.</param>
        /// <param name="b">The second hash code to include in the hash code.</param>
        /// <param name="c">The third hash code to include in the hash code.</param>
        /// <returns>
        /// The hash code.
        /// </returns>
        [SecurityCritical]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe int Fnv(int a, int b, int c)
        {
            var data = stackalloc int[3]; // TODO: https://github.com/dotnet/corefx/pull/24212
            data[0] = a;
            data[1] = b;
            data[2] = c;

            var hc = Fnv((byte*)data, 3 * sizeof(int));
            return hc;
        }

        /// <summary>
        /// Calculates a hash code from the specified data using the
        /// Fowler/Noll/Vo 1-a algorithm.
        /// </summary>
        /// <param name="a">The first hash code to include in the hash code.</param>
        /// <param name="b">The second hash code to include in the hash code.</param>
        /// <param name="c">The third hash code to include in the hash code.</param>
        /// <param name="d">The fourth hash code to include in the hash code.</param>
        /// <returns>
        /// The hash code.
        /// </returns>
        [SecurityCritical]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe int Fnv(int a, int b, int c, int d)
        {
            var data = stackalloc int[4]; // TODO: https://github.com/dotnet/corefx/pull/24212
            data[0] = a;
            data[1] = b;
            data[2] = c;
            data[3] = d;

            var hc = Fnv((byte*)data, 4 * sizeof(int));
            return hc;
        }

        /// <summary>
        /// Calculates a hash code from the specified data using the
        /// Fowler/Noll/Vo 1-a algorithm.
        /// </summary>
        /// <param name="a">The first hash code to include in the hash code.</param>
        /// <param name="b">The second hash code to include in the hash code.</param>
        /// <param name="c">The third hash code to include in the hash code.</param>
        /// <param name="d">The fourth hash code to include in the hash code.</param>
        /// <param name="e">The fifth hash code to include in the hash code.</param>
        /// <returns>
        /// The hash code.
        /// </returns>
        [SecurityCritical]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe int Fnv(int a, int b, int c, int d, int e)
        {
            var data = stackalloc int[5]; // TODO: https://github.com/dotnet/corefx/pull/24212
            data[0] = a;
            data[1] = b;
            data[2] = c;
            data[3] = d;
            data[4] = e;

            var hc = Fnv((byte*)data, 5 * sizeof(int));
            return hc;
        }

        #endregion
    }
}
