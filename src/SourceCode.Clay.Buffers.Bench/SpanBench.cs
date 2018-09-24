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
         Actual | 14.79 ns | 0.2894 ns | 0.5576 ns |   1.01 |     0.05 |
           Cast | 14.26 ns | 0.2803 ns | 0.4606 ns |   0.97 |     0.04 |
            Ptr | 15.07 ns | 0.2962 ns | 0.2909 ns |   1.02 |     0.04 |
         Unroll | 14.72 ns | 0.2925 ns | 0.4378 ns |   1.00 |     0.00 |

       Else:

         Method |     Mean |     Error |    StdDev | Scaled | ScaledSD |
        ------- |---------:|----------:|----------:|-------:|---------:|
         Actual | 14.86 ns | 0.2965 ns | 0.5641 ns |   1.01 |     0.05 |
           Cast | 25.64 ns | 0.5088 ns | 0.9303 ns |   1.75 |     0.08 |
            Ptr | 24.64 ns | 0.4696 ns | 0.5025 ns |   1.68 |     0.06 |
         Unroll | 14.70 ns | 0.2908 ns | 0.4779 ns |   1.00 |     0.00 |

    */

    //[MemoryDiagnoser]
    public class SpanBench
    {
        private const uint _iterations = 100;
        private const uint N = ushort.MaxValue;

        // Whatever is being used in the actual code
        [Benchmark(Baseline = false, OperationsPerInvoke = (int)(_iterations * N))]
        public static ulong Actual()
        {
            var sum = 0ul;

            for (var i = 0; i < _iterations; i++)
            {
                for (var n = 0; n <= N; n++)
                {
                    var v = i * n * ushort.MaxValue + uint.MaxValue;
                    var bytes = BitConverter.GetBytes(v);

                    var u64 = BitOps.ExtractUInt64(bytes, n & 63);

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
                    var v = i * n * ushort.MaxValue + uint.MaxValue;
                    var bytes = BitConverter.GetBytes(v);

                    var u64 = ExtractUInt64_Cast(bytes, n & 63);

                    sum = unchecked(sum + u64);
                }
            }

            return sum;
        }

        [Benchmark(Baseline = false, OperationsPerInvoke = (int)(_iterations * N))]
        public static ulong Ptr()
        {
            var sum = 0ul;

            for (var i = 0; i < _iterations; i++)
            {
                for (var n = 0; n <= N; n++)
                {
                    var v = i * n * ushort.MaxValue + uint.MaxValue;
                    var bytes = BitConverter.GetBytes(v);

                    var u64 = ExtractUInt64_Ptr(bytes, n & 63);

                    sum = unchecked(sum + u64);
                }
            }

            return sum;
        }

        [Benchmark(Baseline = true, OperationsPerInvoke = (int)(_iterations * N))]
        public static ulong Unroll()
        {
            var sum = 0ul;

            for (var i = 0; i < _iterations; i++)
            {
                for (var n = 0; n <= N; n++)
                {
                    var v = i * n * ushort.MaxValue + uint.MaxValue;
                    var bytes = BitConverter.GetBytes(v);

                    var u64 = ExtractUInt64_Unroll(bytes, n & 63);

                    sum = unchecked(sum + u64);
                }
            }

            return sum;
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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static ulong ExtractUInt64_Unroll(ReadOnlySpan<byte> span, int bitOffset)
        {
            int ix = bitOffset >> 3; // div 8
            var len = span.Length - ix;
            int shft = bitOffset & 7; // mod 8

            ulong val = 0;
            switch (len)
            {
                default:
                    if (len <= 0) throw new ArgumentOutOfRangeException(nameof(bitOffset));
                    goto case 9;

                // Need at least 8+1 bytes
                case 9: val |= (ulong)span[ix + 8] << (8 * 8 - shft); goto case 8;

                case 8: val |= (ulong)span[ix + 7] << (7 * 8 - shft); goto case 7;
                case 7: val |= (ulong)span[ix + 6] << (6 * 8 - shft); goto case 6;
                case 6: val |= (ulong)span[ix + 5] << (5 * 8 - shft); goto case 5;
                case 5: val |= (ulong)span[ix + 4] << (4 * 8 - shft); goto case 4;

                case 4: val |= (ulong)span[ix + 3] << (3 * 8 - shft); goto case 3;
                case 3: val |= (ulong)span[ix + 2] << (2 * 8 - shft); goto case 2;
                case 2: val |= (ulong)span[ix + 1] << (1 * 8 - shft); goto case 1;
                case 1: val |= (ulong)span[ix + 0] >> shft;
                    return val;
            }
        }
    }
}
