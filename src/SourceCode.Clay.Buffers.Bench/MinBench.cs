#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using BenchmarkDotNet.Attributes;
using System;
using System.Runtime.CompilerServices;

namespace SourceCode.Clay.Buffers.Bench
{
    /*
         Method |      Mean |     Error |    StdDev | Scaled | ScaledSD |
       -------- |----------:|----------:|----------:|-------:|---------:|
        Logical | 1.2163 ns | 0.0239 ns | 0.0437 ns |   1.25 |     0.07 |
         Branch | 0.9743 ns | 0.0195 ns | 0.0407 ns |   1.00 |     0.00 |
    */

    //[MemoryDiagnoser]
    public class MinBench
    {
        private const uint _iterations = 1000;
        private const uint N = ushort.MaxValue;

        [Benchmark(Baseline = false, OperationsPerInvoke = (int)(_iterations * N))]
        public static ulong Logical()
        {
            var sum = 0ul;

            for (var i = 0; i < _iterations; i++)
            {
                for (var n = 0; n <= N; n++)
                {
                    sum += (uint)Min(n, 32_123);
                }
            }

            return sum;
        }

        [Benchmark(Baseline = true, OperationsPerInvoke = (int)(_iterations * N))]
        public static ulong Branch()
        {
            var sum = 0ul;

            for (var i = 0; i < _iterations; i++)
            {
                for (var n = 0; n <= N; n++)
                {
                    sum += (uint)Math.Min(n, 32_123);
                }
            }

            return sum;
        }

        /// <summary>
        /// Calculates the minimum of two numbers without branching.
        /// </summary>
        /// <param name="x">The first number.</param>
        /// <param name="y">The second number.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int Min(int x, int y)
        {
            // eg, y = 8
            // x       x^8     -(x<8)  &       8^
            // 0000    1000    1111    1000    0000
            // 0001    1001    1111    1001    0001
            // 0010    1010    1111    1010    0010
            // 0011    1011    1111    1011    0011
            // 0100    1100    1111    1100    0100
            // 0101    1101    1111    1101    0101
            // 0110    1110    1111    1110    0110
            // 0111    1111    1111    1111    0111
            // 1000    0000    0000    0000    1000
            // 1001    0001    0000    0000    1000

            return y ^ ((x ^ y) & -((x < y) ? 1 : 0));
        }
    }
}
