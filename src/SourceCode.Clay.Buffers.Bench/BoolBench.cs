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
         Actual | 2.319 ns | 0.0184 ns | 0.0163 ns |   0.79 |     0.01 | x
     UnsafeCode | 3.370 ns | 0.0104 ns | 0.0087 ns |   1.15 |     0.01 |
       UnsafeAs | 2.435 ns | 0.0468 ns | 0.0539 ns |   0.83 |     0.02 | x
          Union | 2.808 ns | 0.0095 ns | 0.0084 ns |   0.96 |     0.01 |
         Branch | 2.937 ns | 0.0422 ns | 0.0374 ns |   1.00 |     0.00 |
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
                    sum += BitOps.Evaluate(@true, 1);
                    sum++;
                    sum -= BitOps.Evaluate(@false, 1);
                    sum--;

                    sum += BitOps.Evaluate(@true, 4);
                    sum -= BitOps.Evaluate(@false, 3);
                    sum += BitOps.Evaluate(@true, 3, 2);
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
                    sum += (byte)(@true ? 1 : 0);
                    sum++;
                    sum -= (byte)(@false ? 1 : 0);
                    sum--;

                    var val = (byte)(@true ? 4 : 0);
                    sum += val;

                    val = (byte)(@false ? 3 : 0);
                    sum -= val;

                    val = (byte)(@true ? 3 : 2);
                    sum += val;
                    sum -= 7;
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
