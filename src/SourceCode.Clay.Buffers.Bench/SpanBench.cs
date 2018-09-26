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

          Method |     Mean |     Error |    StdDev | Scaled |
        -------- |---------:|----------:|----------:|-------:|
          Unroll | 26.76 ns | 0.2114 ns | 0.1978 ns |   1.00 |
            Blit | 25.97 ns | 0.1513 ns | 0.1416 ns |   0.97 |
         Hybrid1 | 21.96 ns | 0.1348 ns | 0.1195 ns |   0.82 |
         Hybrid2 | 25.41 ns | 0.1689 ns | 0.1580 ns |   0.95 |
          Actual | 20.66 ns | 0.1174 ns | 0.1041 ns |   0.77 | x
           Cast1 | 20.49 ns | 0.1084 ns | 0.0961 ns |   0.77 | x
           Cast2 | 22.48 ns | 0.0906 ns | 0.0803 ns |   0.84 |

       Else:

          Method |     Mean |     Error |    StdDev | Scaled |
        -------- |---------:|----------:|----------:|-------:|
          Unroll | 26.76 ns | 0.1428 ns | 0.1336 ns |   1.00 |
            Blit | 26.24 ns | 0.2536 ns | 0.2372 ns |   0.98 |
         Hybrid1 | 23.66 ns | 0.2209 ns | 0.2066 ns |   0.88 |
         Hybrid2 | 25.22 ns | 0.2099 ns | 0.1963 ns |   0.94 |
          Actual | 20.74 ns | 0.1182 ns | 0.1048 ns |   0.78 | x
           Cast1 | 20.64 ns | 0.1738 ns | 0.1540 ns |   0.77 | x
           Cast2 | 35.34 ns | 0.1551 ns | 0.1451 ns |   1.32 |

        Cast2 is fast when inlined, but much slower when not.
        All others are consistent, whether inlined or not.
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

        [Benchmark(Baseline = false, OperationsPerInvoke = (int)(_iterations * N))]
        public static ulong Hybrid1()
        {
            var sum = 0ul;

            for (var i = 0; i < _iterations; i++)
            {
                for (var n = 0; n <= N; n++)
                {
                    long v = i * n * ushort.MaxValue + uint.MaxValue;
                    var b = BitConverter.GetBytes(v);
                    b = new byte[25] { 0, 0, 0, b[0], b[1], b[2], b[3], b[4], b[5], b[6], b[7], 0, 0, 1, 0, 0, 0, 1, 2, 0, 4, 5, 3, 0, 100 };

                    var u64 = ExtractUInt64_Hybrid1(b, n % b.Length);

                    sum = unchecked(sum + u64);
                }
            }

            return sum;
        }

        [Benchmark(Baseline = false, OperationsPerInvoke = (int)(_iterations * N))]
        public static ulong Hybrid2()
        {
            var sum = 0ul;

            for (var i = 0; i < _iterations; i++)
            {
                for (var n = 0; n <= N; n++)
                {
                    long v = i * n * ushort.MaxValue + uint.MaxValue;
                    var b = BitConverter.GetBytes(v);
                    b = new byte[25] { 0, 0, 0, b[0], b[1], b[2], b[3], b[4], b[5], b[6], b[7], 0, 0, 1, 0, 0, 0, 1, 2, 0, 4, 5, 3, 0, 100 };

                    var u64 = ExtractUInt64_Hybrid2(b, n % b.Length);

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
        public static ulong Cast1()
        {
            var sum = 0ul;

            for (var i = 0; i < _iterations; i++)
            {
                for (var n = 0; n <= N; n++)
                {
                    long v = i * n * ushort.MaxValue + uint.MaxValue;
                    var b = BitConverter.GetBytes(v);
                    b = new byte[25] { 0, 0, 0, b[0], b[1], b[2], b[3], b[4], b[5], b[6], b[7], 0, 0, 1, 0, 0, 0, 1, 2, 0, 4, 5, 3, 0, 100 };

                    var u64 = ExtractUInt64_Cast1(b, n % b.Length);

                    sum = unchecked(sum + u64);
                }
            }

            return sum;
        }

        [Benchmark(Baseline = false, OperationsPerInvoke = (int)(_iterations * N))]
        public static ulong Cast2()
        {
            var sum = 0ul;

            for (var i = 0; i < _iterations; i++)
            {
                for (var n = 0; n <= N; n++)
                {
                    long v = i * n * ushort.MaxValue + uint.MaxValue;
                    var b = BitConverter.GetBytes(v);
                    b = new byte[25] { 0, 0, 0, b[0], b[1], b[2], b[3], b[4], b[5], b[6], b[7], 0, 0, 1, 0, 0, 0, 1, 2, 0, 4, 5, 3, 0, 100 };

                    var u64 = ExtractUInt64_Cast2(b, n % b.Length);

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

        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static ulong ExtractUInt64_Blit(ReadOnlySpan<byte> span, int bitOffset)
        {
            int ix = bitOffset >> 3; // div 8
            var len = Math.Max(0, span.Length - ix);
            int shft = bitOffset & 7; // mod 8

            var blit = new Blit64();
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

        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static ulong ExtractUInt64_Hybrid1(ReadOnlySpan<byte> span, int bitOffset)
        {
            int ix = bitOffset >> 3; // div 8
            var len = Math.Max(0, span.Length - ix);
            int shft = bitOffset & 7; // mod 8

            var blit = new Blit64();
            ulong val = 0;

            switch (len)
            {
                case 0: throw new ArgumentOutOfRangeException(nameof(bitOffset));

                // Need at least 8+1 bytes
                default:
                    val = (ulong)span[ix + 8] << (64 - shft);
                    goto case 8;

                case 8: blit.u64 = MemoryMarshal.Cast<byte, ulong>(span.Slice(ix, 8))[0]; break;

                case 7: blit.b6 = span[ix + 6]; goto case 6;
                case 6: blit.b5 = span[ix + 5]; goto case 5;
                case 5: blit.b4 = span[ix + 4]; goto case 4;
                case 4: blit.i0 = MemoryMarshal.Cast<byte, uint>(span.Slice(ix, 4))[0]; break;

                case 3: blit.b2 = span[ix + 2]; goto case 2;
                case 2: blit.s0 = MemoryMarshal.Cast<byte, ushort>(span.Slice(ix, 2))[0]; break;

                case 1: blit.b0 = span[ix + 0]; break;
            }

            val |= blit.u64 >> shft;
            return val;
        }

        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static ulong ExtractUInt64_Hybrid2(ReadOnlySpan<byte> span, int bitOffset)
        {
            int ix = bitOffset >> 3; // div 8
            var len = Math.Max(0, span.Length - ix);
            int shft = bitOffset & 7; // mod 8

            var blit = new Blit64();
            ulong val = 0;
            switch (len)
            {
                case 0: throw new ArgumentOutOfRangeException(nameof(bitOffset));

                // Need at least 8+1 bytes
                default:
                    val = (ulong)span[ix + 8] << (64 - shft);
                    goto case 8;

                case 8:
                    var r8 = MemoryMarshal.GetReference(span.Slice(ix, 8));
                    blit.u64 = Unsafe.ReadUnaligned<ulong>(ref r8);
                    break;

                case 7: blit.b6 = span[ix + 6]; goto case 6;
                case 6: blit.b5 = span[ix + 5]; goto case 5;
                case 5: blit.b4 = span[ix + 4]; goto case 4;
                case 4:
                    var r4 = MemoryMarshal.GetReference(span.Slice(ix, 4));
                    blit.i0 = Unsafe.ReadUnaligned<uint>(ref r4);
                    break;

                case 3: blit.b2 = span[ix + 2]; goto case 2;
                case 2:
                    var r2 = MemoryMarshal.GetReference(span.Slice(ix, 2));
                    blit.s0 = Unsafe.ReadUnaligned<ushort>(ref r2);
                    break;

                case 1: blit.b0 = span[ix + 0]; break;
            }

            val |= blit.u64 >> shft;
            return val;
        }

        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ulong ExtractUInt64_Cast1(ReadOnlySpan<byte> span, int bitOffset)
        {
            int ix = bitOffset >> 3; // div 8
            if (ix >= span.Length) throw new ArgumentOutOfRangeException(nameof(bitOffset));

            ulong cast = Unsafe.ReadUnaligned<ulong>(ref MemoryMarshal.GetReference(span.Slice(ix)));
            //ulong cast = Unsafe.As<byte, ulong>(ref Unsafe.AddByteOffset(ref MemoryMarshal.GetReference(span), (IntPtr)ix));

            int shft = bitOffset & 7; // mod 8
            ulong val = cast >> shft;

            if (ix + 8 < span.Length)
                val |= (ulong)span[ix + 8] << (64 - shft);

            return val;
        }

        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ulong ExtractUInt64_Cast2(ReadOnlySpan<byte> span, int bitOffset)
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

        [StructLayout(LayoutKind.Explicit, Size = 8)]
        private ref struct Blit64
        {
            // byte

            [FieldOffset(0)]
            public byte b0;

            [FieldOffset(1)]
            public byte b1;

            [FieldOffset(2)]
            public byte b2;

            [FieldOffset(3)]
            public byte b3;

            [FieldOffset(4)]
            public byte b4;

            [FieldOffset(5)]
            public byte b5;

            [FieldOffset(6)]
            public byte b6;

            [FieldOffset(7)]
            public byte b7;

            // ushort

            [FieldOffset(0)]
            public ushort s0;

            [FieldOffset(1)]
            public ushort s1;

            [FieldOffset(2)]
            public ushort s2;

            [FieldOffset(3)]
            public ushort s3;

            // uint

            [FieldOffset(0)]
            public uint i0;

            [FieldOffset(1)]
            public uint i1;

            // ulong

            [FieldOffset(0)]
            public ulong u64;
        }

        [StructLayout(LayoutKind.Explicit, Size = 4)]
        private ref struct Blit32
        {
            // byte

            [FieldOffset(0)]
            public byte b0;

            [FieldOffset(1)]
            public byte b1;

            [FieldOffset(2)]
            public byte b2;

            [FieldOffset(3)]
            public byte b3;

            // ushort

            [FieldOffset(0)]
            public ushort s0;

            [FieldOffset(1)]
            public ushort s1;

            // uint

            [FieldOffset(0)]
            public uint u32;
        }
    }
}