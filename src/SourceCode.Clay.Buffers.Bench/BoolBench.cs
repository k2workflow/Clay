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
         Method |     Mean |     Error |    StdDev | Scaled |
    ----------- |---------:|----------:|----------:|-------:|
         Actual | 2.565 ns | 0.0504 ns | 0.0447 ns |   0.77 | x
     UnsafeCode | 3.387 ns | 0.0345 ns | 0.0323 ns |   1.02 |
       UnsafeAs | 2.383 ns | 0.0096 ns | 0.0090 ns |   0.71 | x
          Union | 3.051 ns | 0.0265 ns | 0.0248 ns |   0.92 | ~
         Branch | 3.334 ns | 0.0146 ns | 0.0137 ns |   1.00 |
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
                    sum += BoolToByte.ToByte(@true, 1);
                    sum++;
                    sum -= BoolToByte.ToByte(@false, 1);
                    sum--;

                    sum += BoolToByte.ToByte(@true, 4);
                    sum -= BoolToByte.ToByte(@false, 3);
                    sum += BoolToByte.ToByte(@true, 3, 2);
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
            private static byte ToByteImpl(bool condition)
                => new BoolToByte { Bool = condition }.Byte;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static byte ToByte(bool condition, byte trueValue, byte falseValue = 0)
            {
                uint val = ToByteImpl(condition);
                val = (val * trueValue)
                    + ((1 - val) * falseValue);

                return (byte)val;
            }
        }
    }
}
