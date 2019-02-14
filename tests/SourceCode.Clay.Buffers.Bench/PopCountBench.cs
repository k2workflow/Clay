#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using System.Runtime.CompilerServices;
using BenchmarkDotNet.Attributes;

namespace SourceCode.Clay.Buffers.Bench
{
    /*
 Method |     Mean |     Error |    StdDev | Ratio | RatioSD |
------- |---------:|----------:|----------:|------:|--------:|
    Mul | 3.608 ns | 0.0558 ns | 0.0522 ns |  1.00 |    0.00 |
    Par | 4.474 ns | 0.0385 ns | 0.0360 ns |  1.24 |    0.02 |
    */

    //[MemoryDiagnoser]
    public class PopCountBench
    {
        private const int _iterations = 5000;
        private const int N = ushort.MaxValue;

        [Benchmark(Baseline = true, OperationsPerInvoke = _iterations * N)]
        public static long Mul()
        {
            long sum = 0;

            for (int i = 0; i < _iterations; i++)
            {
                for (int n = 0; n <= N; n++)
                {
                    uint value = (uint)(uint.MaxValue / (_iterations + 1.0) / (n + 1.0));
                    sum += PopCountMul(value);
                }
            }

            return sum;
        }

        [Benchmark(Baseline = false, OperationsPerInvoke = _iterations * N)]
        public static long Par()
        {
            long sum = 0;

            for (int i = 0; i < _iterations; i++)
            {
                for (int n = 0; n <= N; n++)
                {
                    uint value = (uint)(uint.MaxValue / (_iterations + 1.0) / (n + 1.0));
                    sum += PopCountPar(value);
                }
            }

            return sum;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int PopCountMul(uint value)
        {
            //if (Popcnt.IsSupported)
            //{
            //    return (int)Popcnt.PopCount(value);
            //}

            return SoftwareFallback(value);

            int SoftwareFallback(uint v)
            {
                const uint c1 = 0x_55555555u;
                const uint c2 = 0x_33333333u;
                const uint c3 = 0x_0F0F0F0Fu;
                const uint c4 = 0x_01010101u;

                v = v - ((v >> 1) & c1);
                v = (v & c2) + ((v >> 2) & c2);
                v = (((v + (v >> 4)) & c3) * c4) >> 24;

                return (int)v;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int PopCountPar(uint value)
        {
            //if (Popcnt.IsSupported)
            //{
            //    return (int)Popcnt.PopCount(value);
            //}

            return SoftwareFallback(value);

            int SoftwareFallback(uint v)
            {
                v = v - ((v >> 1) & 0x55555555u);
                v = ((v >> 2) & 0x33333333u) + (v & 0x33333333u);
                v = ((v >> 4) + v) & 0x0F0F0F0Fu;
                v = ((v >> 8) + v) & 0x00FF00FFu;
                v = ((v >> 16) + v) & 0x0000FFFFu;

                return (int)v;
            }
        }
    }
}
