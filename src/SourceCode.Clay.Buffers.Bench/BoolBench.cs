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
         Method |     Mean |     Error |    StdDev | Scaled | ScaledSD |
        ------- |---------:|----------:|----------:|-------:|---------:|
         Unsafe | 1.574 ns | 0.0118 ns | 0.0092 ns |   1.10 |     0.05 |
           Safe | 1.575 ns | 0.0310 ns | 0.0380 ns |   1.10 |     0.05 |
         Branch | 1.435 ns | 0.0285 ns | 0.0632 ns |   1.00 |     0.00 |
    */

    //[MemoryDiagnoser]
    public class BoolBench
    {
        private const uint _iterations = 1000;
        private const uint N = ushort.MaxValue;

        [Benchmark(Baseline = false, OperationsPerInvoke = (int)(_iterations * N))]
        public static ulong Unsafe()
        {
            var sum = 0ul;

            for (var i = 0; i < _iterations; i++)
            {
                for (var n = 0; n <= N; n++)
                {
                    sum += BitOps.ToByte(n % 17 == 0);
                }
            }

            return sum;
        }

        [Benchmark(Baseline = false, OperationsPerInvoke = (int)(_iterations * N))]
        public static ulong Safe()
        {
            var sum = 0ul;

            for (var i = 0; i < _iterations; i++)
            {
                for (var n = 0; n <= N; n++)
                {
                    sum += BoolToByte.ToByte(n % 17 == 0);
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
                    sum += (byte)(n % 17 == 0 ? 1 : 0);
                }
            }

            return sum;
        }

        [StructLayout(LayoutKind.Explicit, Pack = 4, Size = 1)]
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
