#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using BenchmarkDotNet.Attributes;
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace SourceCode.Clay.Buffers.Bench
{
    /*
         Method |     Mean |     Error |    StdDev | Scaled |
    ----------- |---------:|----------:|----------:|-------:|
         Branch | 1.801 ns | 0.0234 ns | 0.0219 ns |   1.00 |
         Actual | 1.108 ns | 0.0173 ns | 0.0161 ns |   0.62 | x
     UnsafeCode | 3.419 ns | 0.0339 ns | 0.0265 ns |   1.90 |
       UnsafeAs | 1.079 ns | 0.0213 ns | 0.0269 ns |   0.60 | x
          Union | 1.148 ns | 0.0063 ns | 0.0056 ns |   0.64 | ~
    */

    //[MemoryDiagnoser]
    public class ByteToBoolBench
    {
        private const uint _iterations = 1000;
        private const uint N = ushort.MaxValue - short.MinValue;

#pragma warning disable IDE0044 // Add readonly modifier
        // Prevent folding by using non-readonly non-constant
        private static volatile bool s_true = true;
        private static volatile bool s_false = false;
#pragma warning restore IDE0044 // Add readonly modifier

        [Benchmark(Baseline = true, OperationsPerInvoke = (int)(_iterations * N))]
        public static ulong Branch()
        {
            ulong sum = 0ul;

            for (int i = 0; i < _iterations; i++)
            {
                for (int n = short.MinValue; n <= short.MaxValue; n++)
                {
                    if (ToBoolBranch(n) == s_true)
                        sum++;
                }
            }

            return sum;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool ToBoolBranch(int value)
        {
            bool val = value == 0 ? false : true;
            return val;
        }

        [Benchmark(Baseline = false, OperationsPerInvoke = (int)(_iterations * N))]
        public static ulong Logical()
        {
            ulong sum = 0ul;

            for (int i = 0; i < _iterations; i++)
            {
                for (int n = short.MinValue; n <= short.MaxValue; n++)
                {
                    if (ToBoolLogical(n) == s_true)
                        sum++;
                }
            }

            return sum;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool ToBoolLogical(int value)
            => value != 0;

        [Benchmark(Baseline = false, OperationsPerInvoke = (int)(_iterations * N))]
        public static ulong Actual()
        {
            ulong sum = 0ul;

            for (int i = 0; i < _iterations; i++)
            {
                for (int n = short.MinValue; n <= short.MaxValue; n++)
                {
                    if (BitOps.NotEqualToZero(n) == s_true)
                        sum++;
                }
            }

            return sum;
        }

        [Benchmark(Baseline = false, OperationsPerInvoke = (int)(_iterations * N))]
        public static ulong UnsafeCode()
        {
            ulong sum = 0ul;

            for (int i = 0; i < _iterations; i++)
            {
                for (int n = short.MinValue; n <= short.MaxValue; n++)
                {
                    if (ToBoolUnsafe(n) == s_true)
                        sum++;
                }
            }

            return sum;
        }

        [Benchmark(Baseline = false, OperationsPerInvoke = (int)(_iterations * N))]
        private static bool ToBoolUnsafe(int value)
        {
            byte val = (byte)BitOps.FillTrailingOnes(unchecked((uint)value));
            val &= 1;
            Debug.Assert(val == 0 || val == 1);

            bool tf;
            unsafe
            {
                tf = *(bool*)&val;
            }

            return tf;
        }

        [Benchmark(Baseline = false, OperationsPerInvoke = (int)(_iterations * N))]
        public static ulong UnsafeAs()
        {
            ulong sum = 0ul;

            for (int i = 0; i < _iterations; i++)
            {
                for (int n = short.MinValue; n <= short.MaxValue; n++)
                {
                    if (ToBoolUnsafeAs(n) == s_true)
                        sum++;
                }
            }

            return sum;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool ToBoolUnsafeAs(int value)
        {
            byte val = (byte)BitOps.FillTrailingOnes(unchecked((uint)value));
            val &= 1;
            Debug.Assert(val == 0 || val == 1);

            bool tf = Unsafe.As<byte, bool>(ref val);
            return tf;
        }

        [Benchmark(Baseline = false, OperationsPerInvoke = (int)(_iterations * N))]
        public static ulong Union()
        {
            ulong sum = 0ul;

            for (int i = 0; i < _iterations; i++)
            {
                for (int n = short.MinValue; n <= short.MaxValue; n++)
                {
                    if (BoolToByteBench.BoolToByte.Evaluate(n) == s_true)
                        sum++;
                }
            }

            return sum;
        }
    }
}
