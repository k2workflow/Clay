#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using BenchmarkDotNet.Attributes;
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace SourceCode.Clay.Buffers.Bench
{
    /*
         Method |     Mean |     Error |    StdDev | Scaled |
    ----------- |---------:|----------:|----------:|-------:|
         Branch | 3.847 ns | 0.0747 ns | 0.1071 ns |   1.00 |
         Actual | 2.491 ns | 0.0270 ns | 0.0239 ns |   0.65 | x
     UnsafeCode | 3.636 ns | 0.0436 ns | 0.0408 ns |   0.95 |
       UnsafeAs | 2.442 ns | 0.0223 ns | 0.0208 ns |   0.64 | x
          Union | 2.925 ns | 0.0641 ns | 0.0658 ns |   0.76 | ~
    */

    //[MemoryDiagnoser]
    public class BoolToByteBench
    {
        private const uint _iterations = 1000;
        private const uint N = ushort.MaxValue;

#pragma warning disable IDE0044 // Add readonly modifier
        // Prevent folding by using non-readonly non-constant
        private static bool @true = true;
        private static bool @false = false;
#pragma warning restore IDE0044 // Add readonly modifier

        [Benchmark(Baseline = true, OperationsPerInvoke = (int)(_iterations * N))]
        public static ulong Branch()
        {
            ulong sum = 0ul;

            for (int i = 0; i < _iterations; i++)
            {
                for (int n = 0; n <= N; n++)
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
            uint val = condition ? 1u : 0u;

            val = (val * trueValue)
                + ((1 - val) * falseValue);

            return (byte)val;
        }

        [Benchmark(Baseline = false, OperationsPerInvoke = (int)(_iterations * N))]
        public static ulong Actual()
        {
            ulong sum = 0ul;

            for (int i = 0; i < _iterations; i++)
            {
                for (int n = 0; n <= N; n++)
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
            ulong sum = 0ul;

            for (int i = 0; i < _iterations; i++)
            {
                for (int n = 0; n <= N; n++)
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
            ulong sum = 0ul;

            for (int i = 0; i < _iterations; i++)
            {
                for (int n = 0; n <= N; n++)
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
            ulong sum = 0ul;

            for (int i = 0; i < _iterations; i++)
            {
                for (int n = 0; n <= N; n++)
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

        [StructLayout(LayoutKind.Explicit, Size = 1)] // Runtime can choose Pack
        internal ref struct BoolToByte
        {
            [FieldOffset(0)]
            public bool Bool;

            [FieldOffset(0)]
            public byte Byte;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static uint Evaluate(bool condition, uint trueValue, uint falseValue)
            {
                uint val = EvaluateImpl(condition);
                val = (val * trueValue)
                    + ((1 - val) * falseValue);

                return (byte)val;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static uint Evaluate(bool condition, uint trueValue) 
                => EvaluateImpl(condition) * trueValue;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            private static uint EvaluateImpl(bool condition)
                => new BoolToByte { Bool = condition }.Byte;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static bool Evaluate(int value)
            {
                byte val = (byte)BitOps.FillTrailingOnes(unchecked((uint)value));
                val &= 1;

                Debug.Assert(val == 0 || val == 1);

                return new BoolToByte { Byte = val }.Bool;
            }
        }
    }
}
