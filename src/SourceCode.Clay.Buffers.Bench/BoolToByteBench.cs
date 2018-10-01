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
    ----------- |---------:|----------:|----------:|-------:|---------:|
         Actual | 2.432 ns | 0.0150 ns | 0.0140 ns |   0.70 |     0.01 | x
     UnsafeCode | 3.502 ns | 0.0504 ns | 0.0471 ns |   1.01 |     0.02 |
       UnsafeAs | 2.453 ns | 0.0173 ns | 0.0153 ns |   0.71 |     0.01 | x
          Union | 2.925 ns | 0.0577 ns | 0.0881 ns |   0.84 |     0.03 | ~
         Branch | 3.466 ns | 0.0564 ns | 0.0528 ns |   1.00 |     0.00 |
     */

    //[MemoryDiagnoser]
    public class BoolToByteBench
    {
        private const uint _iterations = 1000;
        private const uint N = ushort.MaxValue;

        // Prevent optimization by leaving non-readonly
#pragma warning disable IDE0044 // Add readonly modifier
        private static bool @true = true;
        private static bool @false = false;
#pragma warning restore IDE0044 // Add readonly modifier

        [Benchmark(Baseline = false, OperationsPerInvoke = (int)(_iterations * N))]
        public static ulong Actual()
        {
            var sum = 0ul;

            for (var i = 0; i < _iterations; i++)
            {
                for (var n = 0; n <= N; n++)
                {
                    sum += BitOps.Evaluate(@true, 1ul);
                    sum++;
                    sum -= BitOps.Evaluate(@false, 1ul);
                    sum--;

                    sum += BitOps.Evaluate(@true, 4ul);
                    sum -= BitOps.Evaluate(@false, 3ul);
                    sum += BitOps.Evaluate(@true, 3ul, 2ul);
                    sum -= 7;
                }
            }

            return sum;
        }

        [Benchmark(Baseline = false, OperationsPerInvoke = (int)(_iterations * N))]
        public static ulong UnsafeCode()
        {
            var sum = 0ul;

            for (var i = 0; i < _iterations; i++)
            {
                for (var n = 0; n <= N; n++)
                {
                    sum += ToByteUnsafe(@true, 1);
                    sum++;
                    sum -= ToByteUnsafe(@false, 0);
                    sum--;

                    sum += ToByteUnsafe(@true, 4);
                    sum -= ToByteUnsafe(@false, 3);
                    sum += ToByteUnsafe(@true, 3, 2);
                    sum -= 7;
                }
            }

            return sum;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static byte ToByteUnsafe(bool condition, byte trueValue, byte falseValue = 0)
        {
            uint val;
            unsafe
            {
                val = *(byte*)&condition;
            }

            val = (val * trueValue)
                + ((1 - val) * falseValue);

            return (byte)val;
        }

        [Benchmark(Baseline = false, OperationsPerInvoke = (int)(_iterations * N))]
        public static ulong UnsafeAs()
        {
            var sum = 0ul;

            for (var i = 0; i < _iterations; i++)
            {
                for (var n = 0; n <= N; n++)
                {
                    sum += ToByteUnsafeAs(@true, 1);
                    sum++;
                    sum -= ToByteUnsafeAs(@false, 1);
                    sum--;

                    sum += ToByteUnsafeAs(@true, 4);
                    sum -= ToByteUnsafeAs(@false, 3);
                    sum += ToByteUnsafeAs(@true, 3, 2);
                    sum -= 7;
                }
            }

            return sum;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static byte ToByteUnsafeAs(bool condition, byte trueValue, byte falseValue = 0)
        {
            uint val = Unsafe.As<bool, byte>(ref condition);

            val = (val * trueValue)
                + ((1 - val) * falseValue);

            return (byte)val;
        }

        [Benchmark(Baseline = false, OperationsPerInvoke = (int)(_iterations * N))]
        public static ulong Union()
        {
            var sum = 0ul;

            for (var i = 0; i < _iterations; i++)
            {
                for (var n = 0; n <= N; n++)
                {
                    sum += BoolToByte.Evaluate(@true, 1);
                    sum++;
                    sum -= BoolToByte.Evaluate(@false, 1);
                    sum--;

                    sum += BoolToByte.Evaluate(@true, 4);
                    sum -= BoolToByte.Evaluate(@false, 3);
                    sum += BoolToByte.Evaluate(@true, 3, 2);
                    sum -= 7;
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
                    sum += ToByteBranch(@true, 1);
                    sum++;
                    sum -= ToByteBranch(@false, 1);
                    sum--;

                    sum += ToByteBranch(@true, 4);
                    sum -= ToByteBranch(@false, 3);
                    sum += ToByteBranch(@true, 3, 2);
                    sum -= 7;
                }
            }

            return sum;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static byte ToByteBranch(bool condition, byte trueValue, byte falseValue = 0)
        {
            var val = condition ? 1u : 0u;

            val = (val * trueValue)
                + ((1 - val) * falseValue);

            return (byte)val;
        }

        [StructLayout(LayoutKind.Explicit, Size = 1)] // Runtime can choose Pack
        private struct BoolToByte
        {
            [FieldOffset(0)]
            public bool Bool;

            [FieldOffset(0)]
            public readonly byte Byte;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            private static uint Evaluate(bool condition)
                => new BoolToByte { Bool = condition }.Byte;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static uint Evaluate(bool condition, uint trueValue, uint falseValue)
            {
                var val = Evaluate(condition);
                val = (val * trueValue)
                    + ((1 - val) * falseValue);

                return (byte)val;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static uint Evaluate(bool condition, uint trueValue) 
                => Evaluate(condition) * trueValue;
        }
    }
}
