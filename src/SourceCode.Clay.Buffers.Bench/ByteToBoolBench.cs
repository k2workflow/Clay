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
     */

    //[MemoryDiagnoser]
    public class ByteToBoolBench
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
                    BitOps.Evaluate(n);
                }
            }

            return sum;
        }

        public static ulong UnsafeCode()
        {
            var sum = 0ul;

            for (var i = 0; i < _iterations; i++)
            {
                for (var n = 0; n <= N; n++)
                {
                    sum += ToByteUnsafe(@true, 1);
                    sum++;
                }
            }

            return sum;
        }

        private static bool ToByteUnsafe(int condition)
        {
            bool val;
            unsafe
            {
                val = *(bool*)&condition;
            }

            return val;
        }

        [Benchmark(Baseline = false, OperationsPerInvoke = (int)(_iterations * N))]
        public static ulong UnsafeAs()
        {
            var sum = 0ul;

            for (var i = 0; i < _iterations; i++)
            {
                for (var n = 0; n <= N; n++)
                {
                    sum += ToBoolUnsafeAs(@true, 1);
                    sum++;
                }
            }

            return sum;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool ToBoolUnsafeAs(byte condition)
        {
            uint val = Unsafe.As<byte, bool>(ref condition);

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
        private static bool ToByteBranch(byte condition)
        {
            var val = condition == 0 ? false : true;
            return val;
        }

        [StructLayout(LayoutKind.Explicit, Size = 1)] // Runtime can choose Pack
        private struct BoolToByte
        {
            [FieldOffset(0)]
            public bool Bool;

            [FieldOffset(0)]
            public byte Byte;

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
