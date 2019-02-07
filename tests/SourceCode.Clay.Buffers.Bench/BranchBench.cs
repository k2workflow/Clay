#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using BenchmarkDotNet.Attributes;

namespace SourceCode.Clay.Buffers.Bench
{
    /*
 Method |     Mean |     Error |    StdDev | Ratio |
------- |---------:|----------:|----------:|------:|
 Branch | 2.729 ns | 0.0180 ns | 0.0160 ns |  1.00 |
 Unsafe | 2.355 ns | 0.0163 ns | 0.0152 ns |  0.86 |
    */

    //[MemoryDiagnoser]
    public class BranchBench
    {
        private const int _iterations = 5000;
        private const int N = ushort.MaxValue;

        // Try prevent folding by using volatile non-readonly non-constant
        private static volatile bool s_true = true;
        private static volatile bool s_false = false;

        [Benchmark(Baseline = true, OperationsPerInvoke = _iterations * N)]
        public static ulong Branch()
        {
            ulong sum = 0;

            for (int i = 0; i < _iterations; i++)
            {
                for (int n = 0; n <= N; n++)
                {
                    // Used various distributions for condition below, eg n % 3 <= 1, n % 5 >= 2
                    bool condition = n % 2 == 0 ? s_true : s_false;
                    sum += condition ? 32u : 0u;
                    sum++;
                }
            }

            return sum;
        }

        [Benchmark(Baseline = false, OperationsPerInvoke = _iterations * N)]
        public static ulong Unsafe()
        {
            ulong sum = 0;

            for (int i = 0; i < _iterations; i++)
            {
                for (int n = 0; n <= N; n++)
                {
                    // Used various distributions for condition below, eg n % 3 <= 1, n % 5 >= 2
                    bool condition = n % 2 == 0 ? s_true : s_false;
                    sum += System.Runtime.CompilerServices.Unsafe.As<bool, byte>(ref condition) * 32u;
                    sum++;
                }
            }

            return sum;
        }
    }
}
