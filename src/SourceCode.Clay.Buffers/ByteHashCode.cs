#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using System;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace SourceCode.Clay.Buffers
{
    /*
    The xxHash32 implementation is based on the code published by Yann Collet:

    https://raw.githubusercontent.com/Cyan4973/xxHash/5c174cfa4e45a42f94082dc0d4539b39696afea1/xxhash.c

      xxHash - Fast Hash algorithm
      Copyright (C) 2012-2016, Yann Collet

      BSD 2-Clause License (http://www.opensource.org/licenses/bsd-license.php)

      Redistribution and use in source and binary forms, with or without
      modification, are permitted provided that the following conditions are
      met:

      * Redistributions of source code must retain the above copyright
      notice, this list of conditions and the following disclaimer.
      * Redistributions in binary form must reproduce the above
      copyright notice, this list of conditions and the following disclaimer
      in the documentation and/or other materials provided with the
      distribution.

      THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS
      "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT
      LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR
      A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT
      OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL,
      SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT
      LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE,
      DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY
      THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
      (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE
      OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

      You can contact the author at :
      - xxHash homepage: http://www.xxhash.com
      - xxHash source repository : https://github.com/Cyan4973/xxHash

    */

#pragma warning disable CA1815
#pragma warning disable CA1066
#pragma warning disable CA2231
#pragma warning disable 0809
#pragma warning disable CA1065

    /// <summary>
    /// Calculates a HashCode of a sequence of bytes.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct ByteHashCode
    {
        private static readonly uint s_seed = (uint)typeof(HashCode).GetField(nameof(s_seed), BindingFlags.NonPublic | BindingFlags.Static).GetValue(null);

        public static readonly int Empty = Combine(Array.Empty<byte>());

        private const uint Prime1 = 2654435761U;
        private const uint Prime2 = 2246822519U;
        private const uint Prime3 = 3266489917U;
        private const uint Prime4 = 668265263U;
        private const uint Prime5 = 374761393U;

        private uint _acc0, _acc1, _acc2, _acc3;
        private uint _queue0, _queue1, _queue2, _queue3;
        private ulong _length;

        public static int Combine(ReadOnlySpan<byte> values)
        {
            uint acc;
            var length = (uint)values.Length;
            ReadOnlySpan<uint> ints = MemoryMarshal.Cast<byte, uint>(values);

            if (values.Length < 16)
            {
                acc = s_seed + Prime5;
            }
            else
            {
                var acc0 = s_seed + Prime1 + Prime2;
                var acc1 = s_seed + Prime2;
                var acc2 = s_seed;
                var acc3 = s_seed - Prime1;

                while (ints.Length > 3)
                {
                    acc0 = Round(acc0, ints[0]);
                    acc1 = Round(acc1, ints[1]);
                    acc2 = Round(acc2, ints[2]);
                    acc3 = Round(acc3, ints[3]);
                    ints = ints.Slice(4);
                    values = values.Slice(4 * sizeof(uint));
                }

                acc = Rol(acc0, 1) + Rol(acc1, 7) + Rol(acc2, 12) + Rol(acc3, 18);
            }

            acc += length;

            while (ints.Length > 0)
            {
                acc = RemainderRound(acc, ints[0]);
                ints = ints.Slice(1);
                values = values.Slice(sizeof(uint));
            }

            while (values.Length > 0)
            {
                acc = RemainderRound(acc, values[0]);
                values = values.Slice(1);
            }

            acc = MixFinal(acc);
            return (int)acc;
        }

        /// <summary>
        /// Adds the specified byte to the HashCode.
        /// </summary>
        /// <param name="value">The byte sequence to add</param>
        public void Add(byte value)
        {
            var index = _length++;
            var position = (byte)(index % 16);

            Span<byte> octets = MemoryMarshal.Cast<uint, byte>(MemoryMarshal.CreateSpan(ref _queue0, 4));
            octets[position] = value;

            if (position == 15)
            {
                if (index == 15)
                {
                    _acc0 = s_seed + Prime1 + Prime2;
                    _acc1 = s_seed + Prime2;
                    _acc2 = s_seed;
                    _acc3 = s_seed - Prime1;
                }

                _acc0 = Round(_acc0, _queue0);
                _acc1 = Round(_acc1, _queue1);
                _acc2 = Round(_acc2, _queue2);
                _acc3 = Round(_acc3, _queue3);
                _queue0 = _queue1 = _queue2 = _queue3 = 0;
            }
        }

        public int ToHashCode()
        {
            var length = _length;
            var acc = length < 16
                ? s_seed + Prime5
                : Rol(_acc0, 1) + Rol(_acc1, 7) + Rol(_acc2, 12) + Rol(_acc3, 18);

            acc += (uint)length;

            Span<byte> remainder = MemoryMarshal.Cast<uint, byte>(MemoryMarshal.CreateSpan(ref _queue0, 4))
                .Slice(0, (int)(length % 16));
            Span<uint> ints = MemoryMarshal.Cast<byte, uint>(remainder);

            while (ints.Length > 0)
            {
                acc = RemainderRound(acc, ints[0]);
                ints = ints.Slice(1);
                remainder = remainder.Slice(sizeof(uint));
            }

            while (remainder.Length > 0)
            {
                acc = RemainderRound(acc, remainder[0]);
                remainder = remainder.Slice(1);
            }

            acc = MixFinal(acc);
            return (int)acc;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static uint Rol(uint value, int count) => (value << count) | (value >> (32 - count));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static uint Round(uint acc, uint lane)
        {
            acc += lane * Prime2;
            acc = Rol(acc, 13);
            acc *= Prime1;
            return acc;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static uint RemainderRound(uint acc, uint lane)
        {
            acc += lane * Prime3;
            acc = Rol(acc, 17) * Prime4;
            return acc;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static uint RemainderRound(uint acc, byte lane)
        {
            acc += lane * Prime5;
            acc = Rol(acc, 11) * Prime1;
            return acc;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static uint MixFinal(uint hash)
        {
            hash ^= hash >> 15;
            hash *= Prime2;
            hash ^= hash >> 13;
            hash *= Prime3;
            hash ^= hash >> 16;
            return hash;
        }

        [Obsolete("HashCode is a mutable struct and should not be compared with other HashCodes. Use ToHashCode to retrieve the computed hash code.", error: true)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override int GetHashCode() => throw new NotSupportedException("HashCode is a mutable struct and should not be compared with other HashCodes. Use ToHashCode to retrieve the computed hash code.");

        [Obsolete("HashCode is a mutable struct and should not be compared with other HashCodes.", error: true)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override bool Equals(object obj) => throw new NotSupportedException("HashCode is a mutable struct and should not be compared with other HashCodes.");
    }

#pragma warning restore CA1065
#pragma warning restore 0809
#pragma warning restore CA2231
#pragma warning restore CA1066
#pragma warning restore CA1815 // Override equals and operator equals on value types
}
