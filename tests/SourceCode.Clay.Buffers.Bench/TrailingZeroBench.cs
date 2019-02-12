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
     Method |     Mean |     Error |    StdDev | Ratio | RatioSD |
----------- |---------:|----------:|----------:|------:|--------:|
      Guard | 2.297 ns | 0.0338 ns | 0.0316 ns |  1.00 |    0.00 |
     Branch | 2.844 ns | 0.0494 ns | 0.0462 ns |  1.24 |    0.03 |
 UnsafeImpl | 3.279 ns | 0.0444 ns | 0.0347 ns |  1.43 |    0.02 |
    */

    //[MemoryDiagnoser]
    public class TrailingZeroBench
    {
        private const int _iterations = 5000;
        private const int N = ushort.MaxValue;

        [Benchmark(Baseline = true, OperationsPerInvoke = _iterations * N)]
        public static ulong Guard()
        {
            ulong sum = 0;

            for (int i = 0; i < _iterations; i++)
            {
                for (int n = 0; n <= N; n++)
                {
                    uint value = (uint)(uint.MaxValue / (_iterations + 1.0) / (n + 1.0));
                    sum += TrailingZeroCountGuard(value);
                }
            }

            return sum;
        }

        [Benchmark(Baseline = false, OperationsPerInvoke = _iterations * N)]
        public static ulong Branch()
        {
            ulong sum = 0;

            for (int i = 0; i < _iterations; i++)
            {
                for (int n = 0; n <= N; n++)
                {
                    uint value = (uint)(uint.MaxValue / (_iterations + 1.0) / (n + 1.0));
                    sum += TrailingZeroCountBranch(value);
                }
            }

            return sum;
        }

        [Benchmark(Baseline = false, OperationsPerInvoke = _iterations * N)]
        public static ulong UnsafeImpl()
        {
            ulong sum = 0;

            for (int i = 0; i < _iterations; i++)
            {
                for (int n = 0; n <= N; n++)
                {
                    uint value = (uint)(uint.MaxValue / (_iterations + 1.0) / (n + 1.0));
                    sum += TrailingZeroCountUnsafe(value);
                }
            }

            return sum;
        }

        private static ReadOnlySpan<byte> s_TrailingZeroCountDeBruijn => new byte[32]
        {
            00, 01, 28, 02, 29, 14, 24, 03,
            30, 22, 20, 15, 25, 17, 04, 08,
            31, 27, 13, 23, 21, 19, 16, 07,
            26, 12, 18, 06, 11, 05, 10, 09
        };

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static uint TrailingZeroCountGuard(uint value)
        {
            if (value == 0u)
                return 32u;

            // Using deBruijn sequence, k=2, n=5 (2^5=32) : 0000_0111_0111_1100_1011_0101_0011_0001
            uint ix = (uint)((value & -value) * 0x077CB531u) >> 27;

            // uint.MaxValue >> 27 is always in range [0 - 31] so we use Unsafe.AddByteOffset to avoid bounds check
            uint count = Unsafe.AddByteOffset(ref MemoryMarshal.GetReference(s_TrailingZeroCountDeBruijn), (IntPtr)ix);
            return count;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static uint TrailingZeroCountBranch(uint value)
        {
            // Using deBruijn sequence, k=2, n=5 (2^5=32) : 0000_0111_0111_1100_1011_0101_0011_0001
            uint ix = (uint)((value & -value) * 0x077CB531u) >> 27;

            // uint.MaxValue >> 27 is always in range [0 - 31] so we use Unsafe.AddByteOffset to avoid bounds check
            uint count = Unsafe.AddByteOffset(ref MemoryMarshal.GetReference(s_TrailingZeroCountDeBruijn), (IntPtr)ix);

            // Above code has behavior 0->0, so special-case in order to match intrinsic path

            // Branchless equivalent of: c32 = value == 0 ? 32 : 0
            uint c32 = value == 0u ? 32u : 0u;
            return c32 + count;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static uint TrailingZeroCountUnsafe(uint value)
        {
            // Using deBruijn sequence, k=2, n=5 (2^5=32) : 0000_0111_0111_1100_1011_0101_0011_0001
            uint ix = (uint)((value & -value) * 0x077CB531u) >> 27;

            // uint.MaxValue >> 27 is always in range [0 - 31] so we use Unsafe.AddByteOffset to avoid bounds check
            uint count = Unsafe.AddByteOffset(ref MemoryMarshal.GetReference(s_TrailingZeroCountDeBruijn), (IntPtr)ix);

            // Above code has behavior 0->0, so special-case in order to match intrinsic path

            // Branchless equivalent of: c32 = value == 0 ? 32 : 0
            bool is0 = value == 0u;
            uint c32 = Unsafe.As<bool, byte>(ref is0) * 32u; // 0|1 x 32

            return c32 + count;
        }
    }
}
