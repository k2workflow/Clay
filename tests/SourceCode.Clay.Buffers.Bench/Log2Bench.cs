#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using BenchmarkDotNet.Attributes;

namespace SourceCode.Clay.Buffers.Bench
{
    /*
|   Method |     Mean |     Error |    StdDev | Ratio |
|--------- |---------:|----------:|----------:|------:|
| Standard | 2.426 ns | 0.0104 ns | 0.0087 ns |  1.00 |
|     Pow2 | 2.732 ns | 0.0271 ns | 0.0226 ns |  1.13 |
    */

    //[MemoryDiagnoser]
    public class Log2Bench
    {
        [Benchmark(Baseline = true, OperationsPerInvoke = int.MaxValue)]
        public long Standard()
        {
            long sum = 0;

            for (int n = 0; n < int.MaxValue; n++)
            {
                sum += Log2SoftwareFallback((uint)n);
            }

            return sum;
        }

        [Benchmark(Baseline = false, OperationsPerInvoke = int.MaxValue)]
        public long Pow2()
        {
            long sum = 0;

            for (int n = 0; n < int.MaxValue; n++)
            {
                sum += Log2SoftwareFallbackPow2((uint)n);
            }

            return sum;
        }

        private static ReadOnlySpan<byte> s_Log2DeBruijn => new byte[32]
        {
            00, 09, 01, 10, 13, 21, 02, 29,
            11, 14, 16, 18, 22, 25, 03, 30,
            08, 12, 20, 28, 15, 17, 24, 07,
            19, 27, 23, 06, 26, 05, 04, 31
        };

        private static ReadOnlySpan<byte> s_Log2DeBruijnPow2 => new byte[32]
        {
            00, 01, 28, 02, 29, 14, 24, 03,
            30, 22, 20, 15, 25, 17, 04, 08,
            31, 27, 13, 23, 21, 19, 16, 07,
            26, 12, 18, 06, 11, 05, 10, 09
        };

        private static int Log2SoftwareFallback(uint value)
        {
            value |= value >> 01;
            value |= value >> 02;
            value |= value >> 04;
            value |= value >> 08;
            value |= value >> 16;

            // uint.MaxValue >> 27 is always in range [0 - 31] so we use Unsafe.AddByteOffset to avoid bounds check
            return Unsafe.AddByteOffset(
                // Using deBruijn sequence, k=2, n=5 (2^5=32) : 0b_0000_0111_1100_0100_1010_1100_1101_1101u
                ref MemoryMarshal.GetReference(s_Log2DeBruijn),
                // long -> IntPtr cast on 32-bit platforms is expensive - it does overflow checks not needed here
                (IntPtr)(int)((value * 0x07C4ACDDu) >> 27));
        }

        private static int Log2SoftwareFallbackPow2(uint value)
        {
            if ((value & (value - 1)) == 0) // Power of 2
            {
                // uint.MaxValue >> 27 is always in range [0 - 31] so we use Unsafe.AddByteOffset to avoid bounds check
                return Unsafe.AddByteOffset(
                    // Using deBruijn sequence, k=2, n=5 (2^5=32) : 0b_0000_0111_0111_1100_1011_0101_0011_0001u
                    ref MemoryMarshal.GetReference(s_Log2DeBruijnPow2),
                    // long -> IntPtr cast on 32-bit platforms is expensive - it does overflow checks not needed here
                    (IntPtr)(int)((value * 0x077CB531u) >> 27));
            }

            value |= value >> 01;
            value |= value >> 02;
            value |= value >> 04;
            value |= value >> 08;
            value |= value >> 16;

            // uint.MaxValue >> 27 is always in range [0 - 31] so we use Unsafe.AddByteOffset to avoid bounds check
            return Unsafe.AddByteOffset(
                // Using deBruijn sequence, k=2, n=5 (2^5=32) : 0b_0000_0111_1100_0100_1010_1100_1101_1101u
                ref MemoryMarshal.GetReference(s_Log2DeBruijn),
                // long -> IntPtr cast on 32-bit platforms is expensive - it does overflow checks not needed here
                (IntPtr)(int)((value * 0x07C4ACDDu) >> 27));
        }
    }
}
