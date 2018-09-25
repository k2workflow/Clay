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
       If using AggressiveInlining:

         Method |     Mean |     Error |    StdDev | Scaled | ScaledSD |
        ------- |---------:|----------:|----------:|-------:|---------:|
         Unroll | 26.62 ns | 0.4968 ns | 0.4880 ns |   1.00 |     0.00 |
         Actual | 27.12 ns | 0.5385 ns | 1.1359 ns |   1.02 |     0.05 |
           Cast | 23.01 ns | 0.4557 ns | 1.0561 ns |   0.86 |     0.04 | <----

       Else:

         Method |     Mean |     Error |    StdDev | Scaled | ScaledSD |
        ------- |---------:|----------:|----------:|-------:|---------:|
         Unroll | 26.45 ns | 0.5115 ns | 0.6089 ns |   1.00 |     0.00 | <----
         Actual | 26.94 ns | 0.5356 ns | 1.1643 ns |   1.02 |     0.05 |
           Cast | 35.53 ns | 0.6935 ns | 1.2505 ns |   1.34 |     0.06 |

        Unroll is consistent (26ms), whether inlined or not.
        Cast is faster (23ms) when inlined, but slower (35ms) when not.
        Some callsites may not inline.
    */

    //[MemoryDiagnoser]
    public class SpanBench
    {
        private const uint _iterations = 100;
        private const uint N = ushort.MaxValue;

        [Benchmark(Baseline = true, OperationsPerInvoke = (int)(_iterations * N))]
        public static ulong Unroll()
        {
            var sum = 0ul;

            for (var i = 0; i < _iterations; i++)
            {
                for (var n = 0; n <= N; n++)
                {
                    long v = i * n * ushort.MaxValue + uint.MaxValue;
                    var b = BitConverter.GetBytes(v);
                    b = new byte[25] { 0, 0, 0, b[0], b[1], b[2], b[3], b[4], b[5], b[6], b[7], 0, 0, 1, 0, 0, 0, 1, 2, 0, 4, 5, 3, 0, 100 };

                    var u64 = ExtractUInt64_Unroll(b, n % b.Length);

                    sum = unchecked(sum + u64);
                }
            }

            return sum;
        }

        // Whatever is being used in the actual code
        [Benchmark(Baseline = false, OperationsPerInvoke = (int)(_iterations * N))]
        public static ulong Actual()
        {
            var sum = 0ul;

            for (var i = 0; i < _iterations; i++)
            {
                for (var n = 0; n <= N; n++)
                {
                    long v = i * n * ushort.MaxValue + uint.MaxValue;
                    var b = BitConverter.GetBytes(v);
                    b = new byte[25] { 0, 0, 0, b[0], b[1], b[2], b[3], b[4], b[5], b[6], b[7], 0, 0, 1, 0, 0, 0, 1, 2, 0, 4, 5, 3, 0, 100 };

                    var u64 = BitOps.ExtractUInt64(b, n % b.Length);

                    sum = unchecked(sum + u64);
                }
            }

            return sum;
        }

        [Benchmark(Baseline = false, OperationsPerInvoke = (int)(_iterations * N))]
        public static ulong Cast()
        {
            var sum = 0ul;

            for (var i = 0; i < _iterations; i++)
            {
                for (var n = 0; n <= N; n++)
                {
                    long v = i * n * ushort.MaxValue + uint.MaxValue;
                    var b = BitConverter.GetBytes(v);
                    b = new byte[25] { 0, 0, 0, b[0], b[1], b[2], b[3], b[4], b[5], b[6], b[7], 0, 0, 1, 0, 0, 0, 1, 2, 0, 4, 5, 3, 0, 100 };

                    var u64 = ExtractUInt64_Cast(b, n % b.Length);

                    sum = unchecked(sum + u64);
                }
            }

            return sum;
        }

        //[Benchmark(Baseline = false, OperationsPerInvoke = (int)(_iterations * N))]
        public static ulong Ptr()
        {
            var sum = 0ul;

            for (var i = 0; i < _iterations; i++)
            {
                for (var n = 0; n <= N; n++)
                {
                    long v = i * n * ushort.MaxValue + uint.MaxValue;
                    var b = BitConverter.GetBytes(v);
                    b = new byte[25] { 0, 0, 0, b[0], b[1], b[2], b[3], b[4], b[5], b[6], b[7], 0, 0, 1, 0, 0, 0, 1, 2, 0, 4, 5, 3, 0, 100 };

                    var u64 = ExtractUInt64_Ptr(b, n % b.Length);

                    sum = unchecked(sum + u64);
                }
            }

            return sum;
        }

        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static ulong ExtractUInt64_Unroll(ReadOnlySpan<byte> span, int bitOffset)
        {
            int ix = bitOffset >> 3; // div 8
            var len = span.Length - ix;
            int shft = bitOffset & 7; // mod 8

            ulong val = 0;
            switch (len)
            {
                // Need at least 8+1 bytes
                default:
                    if (len <= 0) throw new ArgumentOutOfRangeException(nameof(bitOffset));
                    goto case 9;

                case 9: val |= (ulong)span[ix + 8] << (8 * 8 - shft); goto case 8;
                case 8: val |= (ulong)span[ix + 7] << (7 * 8 - shft); goto case 7;
                case 7: val |= (ulong)span[ix + 6] << (6 * 8 - shft); goto case 6;
                case 6: val |= (ulong)span[ix + 5] << (5 * 8 - shft); goto case 5;
                case 5: val |= (ulong)span[ix + 4] << (4 * 8 - shft); goto case 4;
                case 4: val |= (ulong)span[ix + 3] << (3 * 8 - shft); goto case 3;
                case 3: val |= (ulong)span[ix + 2] << (2 * 8 - shft); goto case 2;
                case 2: val |= (ulong)span[ix + 1] << (1 * 8 - shft); goto case 1;
                case 1: val |= (ulong)span[ix + 0] >> shft; break;
            }

            return val;
        }

        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ulong ExtractUInt64_Cast(ReadOnlySpan<byte> span, int bitOffset)
        {
            int ix = bitOffset >> 3; // div 8
            if (ix >= span.Length) throw new ArgumentOutOfRangeException(nameof(bitOffset));

            // Need at least 8+1 bytes, so align on 16
            int len = Math.Min(span.Length - ix, 16);
            Span<byte> aligned = stackalloc byte[16];
            span.Slice(ix, len).CopyTo(aligned);

            ReadOnlySpan<ulong> cast = MemoryMarshal.Cast<byte, ulong>(aligned);

            int shft = bitOffset & 7; // mod 8
            ulong val = cast[0] >> shft;
            val |= cast[1] << (64 - shft);

            return val;
        }

        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ulong ExtractUInt64_Ptr(ReadOnlySpan<byte> span, int bitOffset)
        {
            int ix = bitOffset >> 3; // div 8
            if (ix >= span.Length) throw new ArgumentOutOfRangeException(nameof(bitOffset));

            // Need at least 8+1 bytes, so align on 16
            int len = Math.Min(span.Length - ix, 16);
            Span<byte> aligned = stackalloc byte[16];
            span.Slice(ix, len).CopyTo(aligned);

            unsafe
            {
                fixed (byte* ptr = &MemoryMarshal.GetReference(aligned))
                {
                    var cast = (ulong*)ptr;

                    int shft = bitOffset & 7; // mod 8
                    ulong val = cast[0] >> shft;
                    val |= cast[1] << (64 - shft);

                    return val;
                }
            }
        }
    }
}
