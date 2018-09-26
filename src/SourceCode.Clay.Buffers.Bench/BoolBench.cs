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
         Method |      Mean |     Error |    StdDev | Scaled |
    ----------- |----------:|----------:|----------:|-------:|
     UnsafeCode | 0.9050 ns | 0.0038 ns | 0.0035 ns |   0.50 | x
       UnsafeAs | 0.9074 ns | 0.0085 ns | 0.0076 ns |   0.50 | x
          Union | 1.0628 ns | 0.0062 ns | 0.0058 ns |   0.59 |
         Branch | 1.8081 ns | 0.0076 ns | 0.0067 ns |   1.00 |
    */

    //[MemoryDiagnoser]
    public class BoolBench
    {
        private const uint _iterations = 1000;
        private const uint N = ushort.MaxValue;

        // Prevent optimization by leaving non-readonly
#pragma warning disable IDE0044 // Add readonly modifier
        private static bool @true = true;
        private static bool @false = false;
#pragma warning restore IDE0044 // Add readonly modifier

        [Benchmark(Baseline = false, OperationsPerInvoke = (int)(_iterations * N))]
        public static ulong UnsafeCode()
        {
            var sum = 0ul;

            for (var i = 0; i < _iterations; i++)
            {
                for (var n = 0; n <= N; n++)
                {
                    sum += BitOps.ToByte(@true);
                    sum++;
                    sum -= BitOps.ToByte(@false);
                }
            }

            return sum;
        }

        [Benchmark(Baseline = false, OperationsPerInvoke = (int)(_iterations * N))]
        public static ulong UnsafeAs()
        {
            var sum = 0ul;

            for (var i = 0; i < _iterations; i++)
            {
                for (var n = 0; n <= N; n++)
                {
                    sum += Unsafe.As<bool, byte>(ref @true);
                    sum++;
                    sum -= Unsafe.As<bool, byte>(ref @false);
                }
            }

            return sum;
        }

        [Benchmark(Baseline = false, OperationsPerInvoke = (int)(_iterations * N))]
        public static ulong Union()
        {
            var sum = 0ul;

            for (var i = 0; i < _iterations; i++)
            {
                for (var n = 0; n <= N; n++)
                {
                    sum += BoolToByte.ToByte(@true);
                    sum++;
                    sum -= BoolToByte.ToByte(@false);
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
                    sum += (byte)(@true ? 1 : 0);
                    sum++;
                    sum -= (byte)(@false ? 1 : 0);
                }
            }

            return sum;
        }

        [StructLayout(LayoutKind.Explicit, Size = 1)] // Runtime can choose Pack
        private struct BoolToByte
        {
            [FieldOffset(0)]
            public bool Bool;

            [FieldOffset(0)]
            public readonly byte Byte;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static byte ToByte(bool on)
                => (new BoolToByte { Bool = on }).Byte;
        }
    }
}
