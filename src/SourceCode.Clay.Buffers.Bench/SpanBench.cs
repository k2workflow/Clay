#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using BenchmarkDotNet.Attributes;
using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace SourceCode.Clay.Buffers.Bench
{
    /*
         Method |     Mean |     Error |    StdDev | Scaled | ScaledSD |
        ------- |---------:|----------:|----------:|-------:|---------:|
         Unsafe | 1.574 ns | 0.0118 ns | 0.0092 ns |   1.10 |     0.05 |
           Safe | 1.575 ns | 0.0310 ns | 0.0380 ns |   1.10 |     0.05 |
         Branch | 1.435 ns | 0.0285 ns | 0.0632 ns |   1.00 |     0.00 |
    */

    //[MemoryDiagnoser]
    public class SpanBench
    {
        private const uint _iterations = 100;
        private const uint N = ushort.MaxValue;
        private const long f = _iterations / long.MaxValue;

        [Benchmark(Baseline = false, OperationsPerInvoke = (int)(_iterations * N))]
        public static ulong SpanCast()
        {
            var sum = 0ul;

            for (var i = 0; i < _iterations; i++)
            {
                var v = (ulong)(i * f);
                Span<ulong> span = stackalloc ulong[4] { 0, v, 0, 0 };
                var bytes = MemoryMarshal.Cast<ulong, byte>(span);

                for (var n = 0; n <= N; n++)
                {
                    sum = unchecked(sum + BitOps.ExtractUInt64(bytes, 64 * 2));
                }
            }

            return sum;
        }

        [Benchmark(Baseline = true, OperationsPerInvoke = (int)(_iterations * N))]
        public static ulong Legacy()
        {
            var sum = 0ul;

            for (var i = 0; i < _iterations; i++)
            {
                var v = (ulong)(i * f);
                Span<ulong> span = stackalloc ulong[4] { 0, v, 0, 0 };
                var bytes = MemoryMarshal.Cast<ulong, byte>(span);

                for (var n = 0; n <= N; n++)
                {
                    sum = unchecked(sum + ExtractUInt64(bytes, 64 * 2));
                }
            }

            return sum;
        }

        private static ulong ExtractUInt64(ReadOnlySpan<byte> span, int bitOffset)
        {
            int ix = bitOffset >> 3; // div 8
            var len = span.Length - ix;
            int shft = bitOffset & (8 - 1); // mod 8

            ulong val = 0;
            switch (len)
            {
                // [0,1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,..]
                default:
                    if (len <= 0) goto Error;
                    goto case 16;

                // [0,1,2,3,4,5,6,7,8,9,10,11,12,13,14,15]
                case 16:
                    ReadOnlySpan<ulong> cast16 = MemoryMarshal.Cast<byte, ulong>(span);
                    val = cast16[ix >> 4] >> shft;
                    val |= cast16[ix >> 4 + 1] >> (15 * 8 - shft);
                    break;

                // [0,1,2,3,4,5,6,7,8,9,10,11,12,13,14]
                case 15:
                    val = (ulong)span[ix + 14] << (14 * 8 - shft);
                    goto case 14; // Fallthru

                // [0,1,2,3,4,5,6,7,8,9,10,11,12,13]
                case 14:
                    val |= (ulong)span[ix + 13] << (13 * 8 - shft);
                    goto case 13; // Fallthru

                // [0,1,2,3,4,5,6,7,8,9,10,11,12]
                case 13:
                    val |= (ulong)span[ix + 12] << (12 * 8 - shft);
                    goto case 12; // Fallthru

                // [0,1,2,3,4,5,6,7,8,9,10,11]
                case 12:
                    val |= (ulong)span[ix + 11] << (11 * 8 - shft);
                    goto case 11; // Fallthru

                // [0,1,2,3,4,5,6,7,8,9,10]
                case 11:
                    val |= (ulong)span[ix + 10] << (10 * 8 - shft);
                    goto case 10; // Fallthru

                // [0,1,2,3,4,5,6,7,8,9]
                case 10:
                    val |= (ulong)span[ix + 9] << (9 * 8 - shft);
                    goto case 9; // Fallthru

                // [0,1,2,3,4,5,6,7,8]
                case 9:
                    val |= (ulong)span[ix + 8] << (8 * 8 - shft);
                    goto case 8; // Fallthru

                // [0,1,2,3,4,5,6,7]
                case 8:
                    ReadOnlySpan<ulong> cast8 = MemoryMarshal.Cast<byte, ulong>(span);
                    val |= cast8[ix >> 3] >> shft;
                    break;

                // [0,1,2,3,4,5,6]
                case 7:
                    val = (ulong)span[ix + 6] << (6 * 8 - shft);
                    goto case 6; // Fallthru

                // [0,1,2,3,4,5]
                case 6:
                    val |= (ulong)span[ix + 5] << (5 * 8 - shft);
                    goto case 5; // Fallthru

                // [0,1,2,3,4]
                case 5:
                    val |= (ulong)span[ix + 4] << (4 * 8 - shft);
                    goto case 4; // Fallthru

                // [0,1,2,3]
                case 4:
                    ReadOnlySpan<uint> cast4 = MemoryMarshal.Cast<byte, uint>(span);
                    val |= (ulong)cast4[ix >> 2] >> shft;
                    break;

                // [0,1,2]
                case 3:
                    val = (ulong)span[ix + 2] << (2 * 8 - shft);
                    goto case 2; // Fallthru

                // [0,1]
                case 2:
                    ReadOnlySpan<ushort> cast2 = MemoryMarshal.Cast<byte, ushort>(span);
                    val |= (ulong)cast2[ix >> 1] >> shft;
                    break;

                // [0]
                case 1:
                    val = (ulong)span[ix] >> shft;
                    break;
            }

            return val;

            Error:
            throw new ArgumentOutOfRangeException(nameof(bitOffset));
        }
    }
}
