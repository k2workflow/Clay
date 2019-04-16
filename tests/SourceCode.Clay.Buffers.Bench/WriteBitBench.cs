#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using System.Diagnostics;
using System.Runtime.CompilerServices;
using BenchmarkDotNet.Attributes;

namespace SourceCode.Clay.Buffers.Bench
{
    /*
|      Method |     Mean |     Error |    StdDev | Ratio | RatioSD |
|------------ |---------:|----------:|----------:|------:|--------:|
| BranchBench | 2.899 ns | 0.0484 ns | 0.0453 ns |  1.00 |    0.00 | *
| UnsafeBench | 3.299 ns | 0.0761 ns | 0.0747 ns |  1.14 |    0.03 |
    */

    //[MemoryDiagnoser]
    public class WriteBitBench
    {
        private const int _iterations = 5000;
        private const int N = ushort.MaxValue;

        // Try prevent folding by using volatile non-readonly non-constant
        private static volatile bool s_true = true;
        private static volatile bool s_false = false;

        [Benchmark(Baseline = true, OperationsPerInvoke = _iterations * N)]
        public ulong BranchBench()
        {
            ulong sum = 0;

            for (int i = 0; i < _iterations; i++)
            {
                for (int n = 0; n <= N; n++)
                {
                    bool on = n % 2 == 0 ? s_true : s_false;
                    sum += WriteBitBranch((byte)n, n, on);
                    sum++;
                }
            }

            return sum;
        }

        [Benchmark(Baseline = false, OperationsPerInvoke = _iterations * N)]
        public ulong UnsafeBench()
        {
            ulong sum = 0;

            for (int i = 0; i < _iterations; i++)
            {
                for (int n = 0; n <= N; n++)
                {
                    bool on = n % 2 == 0 ? s_true : s_false;
                    sum += WriteBitUnsafe((byte)n, n, on);
                    sum++;
                }
            }

            return sum;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static byte WriteBitBranch(byte value, int bitOffset, bool on)
        {
            uint mask = 1u << (bitOffset & 7);

            if (on)
                return (byte)(value | mask);

            return (byte)(value & ~mask);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static byte WriteBitUnsafe(byte value, int bitOffset, bool on)
        {
            uint mask = 1u << (bitOffset & 7);

            uint sel = (uint)-Unsafe.As<bool, byte>(ref on); // T=FFFFFFFF, F=00000000
            Debug.Assert(sel == 0xFFFF_FFFF || sel == 0); // CLR permits other values for bool

            uint tv = (value | mask) & sel;
            uint fv = /*Bmi1.IsSupported ? Bmi1.AndNot(falseValue, sel) :*/ (value & ~mask) & ~sel;
            return (byte)(tv | fv);
        }
    }
}
