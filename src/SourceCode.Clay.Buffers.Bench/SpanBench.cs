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
         Unroll | 27.08 ns | 0.5333 ns | 0.9480 ns |   1.00 |     0.00 |
           Blit | 25.68 ns | 0.4996 ns | 0.6319 ns |   0.95 |     0.04 |
         Actual | 25.80 ns | 0.5153 ns | 0.6517 ns |   0.95 |     0.04 |
           Cast | 22.53 ns | 0.4461 ns | 0.7453 ns |   0.83 |     0.04 |

       Else:

         Method |     Mean |     Error |    StdDev | Scaled | ScaledSD |
        ------- |---------:|----------:|----------:|-------:|---------:|
         Unroll | 28.27 ns | 0.5615 ns | 1.5372 ns |   1.00 |     0.00 |
           Blit | 26.52 ns | 0.5273 ns | 0.8210 ns |   0.94 |     0.06 |
         Actual | 25.99 ns | 0.5493 ns | 0.6947 ns |   0.92 |     0.05 |
           Cast | 34.99 ns | 0.6713 ns | 0.6593 ns |   1.24 |     0.07 |

        Unroll/Blit are consistent (26ms), whether inlined or not.
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

        [Benchmark(Baseline = false, OperationsPerInvoke = (int)(_iterations * N))]
        public static ulong Blit()
        {
            var sum = 0ul;

            for (var i = 0; i < _iterations; i++)
            {
                for (var n = 0; n <= N; n++)
                {
                    long v = i * n * ushort.MaxValue + uint.MaxValue;
                    var b = BitConverter.GetBytes(v);
                    b = new byte[25] { 0, 0, 0, b[0], b[1], b[2], b[3], b[4], b[5], b[6], b[7], 0, 0, 1, 0, 0, 0, 1, 2, 0, 4, 5, 3, 0, 100 };

                    var u64 = ExtractUInt64_Blit(b, n % b.Length);

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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
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

                case 09: val = (ulong)span[ix + 8] << (8 * 8 - shft); goto case 8;
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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static ulong ExtractUInt64_Blit(ReadOnlySpan<byte> span, int bitOffset)
        {
            int ix = bitOffset >> 3; // div 8
            var len = Math.Max(0, span.Length - ix);
            int shft = bitOffset & 7; // mod 8

            var blit = new BitOps.Blit64();
            ulong val = 0;
            switch (len)
            {
                case 0: throw new ArgumentOutOfRangeException(nameof(bitOffset));

                // Need at least 8+1 bytes
                default:
                    val = (ulong)span[ix + 8] << (64 - shft);
                    goto case 8;

                case 8: blit.b7 = span[ix + 7]; goto case 7;
                case 7: blit.b6 = span[ix + 6]; goto case 6;
                case 6: blit.b5 = span[ix + 5]; goto case 5;
                case 5: blit.b4 = span[ix + 4]; goto case 4;
                case 4: blit.b3 = span[ix + 3]; goto case 3;
                case 3: blit.b2 = span[ix + 2]; goto case 2;
                case 2: blit.b1 = span[ix + 1]; goto case 1;
                case 1: blit.b0 = span[ix + 0]; break;
            }

            val |= blit.u64 >> shft;
            return val;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
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
