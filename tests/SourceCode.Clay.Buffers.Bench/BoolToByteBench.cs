#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using BenchmarkDotNet.Attributes;

namespace SourceCode.Clay.Buffers.Bench
{
    /*
          Method |     Mean |     Error |    StdDev | Scaled |
    ------------ |---------:|----------:|----------:|-------:|
          Branch | 3.787 ns | 0.0393 ns | 0.0328 ns |   1.48 | !
          Actual | 3.126 ns | 0.0319 ns | 0.0299 ns |   1.22 |
      UnsafeCode | 3.441 ns | 0.0428 ns | 0.0379 ns |   1.34 |
        UnsafeAs | 2.565 ns | 0.0329 ns | 0.0292 ns |   1.00 | x
     UnsafeAsRef | 2.328 ns | 0.0384 ns | 0.0340 ns |   0.91 | x
     UnionStruct | 3.409 ns | 0.0482 ns | 0.0427 ns |   1.33 |
    */

    //[MemoryDiagnoser]
    public class BoolToByteBench
    {
        private const int _iterations = 2000;
        private const int N = ushort.MaxValue;

#pragma warning disable IDE0044 // Add readonly modifier
        // Prevent folding by using non-readonly non-constant
        private static volatile bool s_true = true;
        private static volatile bool s_false = false;
#pragma warning restore IDE0044 // Add readonly modifier

        #region Branch

        [Benchmark(Baseline = false, OperationsPerInvoke = _iterations * N)]
        public static long Branch()
        {
            long sum = 0;

            for (int i = 0; i < _iterations; i++)
            {
                for (int n = 0; n <= N; n++)
                {
                    sum += ToByteBranch(s_true);
                    sum++;
                    sum -= ToByteBranch(s_false);
                    sum--;

                    sum += ToByteBranch(s_true, 4);
                    sum -= ToByteBranch(s_false, 3);
                    sum += ToByteBranch(s_true, 3, 2);
                    sum -= 7;
                }
            }

            return sum;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static byte ToByteBranch(bool condition)
        {
            int val = condition ? 1 : 0;
            return (byte)val;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static byte ToByteBranch(bool condition, int trueValue)
        {
            int val = condition ? 1 : 0;

            val *= trueValue;

            return (byte)val;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static byte ToByteBranch(bool condition, int trueValue, int falseValue)
        {
            int val = condition ? 1 : 0;

            val = (val * trueValue)
                + ((1 - val) * falseValue);

            return (byte)val;
        }

        #endregion

        #region Actual

        [Benchmark(Baseline = false, OperationsPerInvoke = _iterations * N)]
        public static long Actual()
        {
            long sum = 0;

            for (int i = 0; i < _iterations; i++)
            {
                for (int n = 0; n <= N; n++)
                {
                    sum += BitOps.AsByte(s_true);
                    sum++;
                    sum -= BitOps.AsByte(s_false);
                    sum--;

                    sum += BitOps.If(s_true, 4);
                    sum -= BitOps.If(s_false, 3);
                    sum += BitOps.If(s_true, 3, 2);
                    sum -= 7;
                }
            }

            return sum;
        }

        #endregion

        #region UnsafeCode

        [Benchmark(Baseline = false, OperationsPerInvoke = _iterations * N)]
        public static long UnsafeCode()
        {
            long sum = 0;

            for (int i = 0; i < _iterations; i++)
            {
                for (int n = 0; n <= N; n++)
                {
                    sum += ToByteUnsafe(s_true);
                    sum++;
                    sum -= ToByteUnsafe(s_false);
                    sum--;

                    sum += ToByteUnsafe(s_true, 4);
                    sum -= ToByteUnsafe(s_false, 3);
                    sum += ToByteUnsafe(s_true, 3, 2);
                    sum -= 7;
                }
            }

            return sum;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static byte ToByteUnsafe(bool condition)
        {
            int val;
            unsafe
            {
                val = *(byte*)&condition;
            }

            return (byte)val;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static byte ToByteUnsafe(bool condition, int trueValue)
        {
            int val;
            unsafe
            {
                val = *(byte*)&condition;
            }

            val *= trueValue;

            return (byte)val;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static byte ToByteUnsafe(bool condition, int trueValue, int falseValue)
        {
            int val;
            unsafe
            {
                val = *(byte*)&condition;
            }

            val = (val * trueValue)
                + ((1 - val) * falseValue);

            return (byte)val;
        }

        #endregion

        #region UnsafeAs

        [Benchmark(Baseline = true, OperationsPerInvoke = _iterations * N)]
        public static long UnsafeAs()
        {
            long sum = 0;

            for (int i = 0; i < _iterations; i++)
            {
                for (int n = 0; n <= N; n++)
                {
                    sum += ToByteUnsafeAs(s_true);
                    sum++;
                    sum -= ToByteUnsafeAs(s_false);
                    sum--;

                    sum += ToByteUnsafeAs(s_true, 4);
                    sum -= ToByteUnsafeAs(s_false, 3);
                    sum += ToByteUnsafeAs(s_true, 3, 2);
                    sum -= 7;
                }
            }

            return sum;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static byte ToByteUnsafeAs(bool condition)
            => Unsafe.As<bool, byte>(ref condition);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static byte ToByteUnsafeAs(bool condition, int trueValue)
        {
            int val = Unsafe.As<bool, byte>(ref condition) * trueValue;
            return (byte)val;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static byte ToByteUnsafeAs(bool condition, int trueValue, int falseValue)
        {
            int val = Unsafe.As<bool, byte>(ref condition);

            val = (val * trueValue)
                + ((1 - val) * falseValue);

            return (byte)val;
        }

        #endregion

        #region UnsafeAsRef

        [Benchmark(Baseline = false, OperationsPerInvoke = _iterations * N)]
        public static long UnsafeAsRef()
        {
            long sum = 0;

            for (int i = 0; i < _iterations; i++)
            {
                for (int n = 0; n <= N; n++)
                {
#pragma warning disable CS0420 // A reference to a volatile field will not be treated as volatile
                    sum += ToByteUnsafeAsRef(ref s_true);
                    sum++;
                    sum -= ToByteUnsafeAsRef(ref s_false);
                    sum--;

                    sum += ToByteUnsafeAsRef(ref s_true, 4);
                    sum -= ToByteUnsafeAsRef(ref s_false, 3);
                    sum += ToByteUnsafeAsRef(ref s_true, 3, 2);
                    sum -= 7;
#pragma warning restore CS0420 // A reference to a volatile field will not be treated as volatile
                }
            }

            return sum;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static byte ToByteUnsafeAsRef(ref bool condition)
            => Unsafe.As<bool, byte>(ref condition);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static byte ToByteUnsafeAsRef(ref bool condition, int trueValue)
        {
            int val = Unsafe.As<bool, byte>(ref condition) * trueValue;
            return (byte)val;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static byte ToByteUnsafeAsRef(ref bool condition, int trueValue, int falseValue)
        {
            int val = Unsafe.As<bool, byte>(ref condition);

            val = (val * trueValue)
                + ((1 - val) * falseValue);

            return (byte)val;
        }

        #endregion

        #region UnionStruct

        [Benchmark(Baseline = false, OperationsPerInvoke = _iterations * N)]
        public static long UnionStruct()
        {
            long sum = 0;

            for (int i = 0; i < _iterations; i++)
            {
                for (int n = 0; n <= N; n++)
                {
                    sum += BoolToByte.Evaluate(s_true);
                    sum++;
                    sum -= BoolToByte.Evaluate(s_false);
                    sum--;

                    sum += BoolToByte.Evaluate(s_true, 4);
                    sum -= BoolToByte.Evaluate(s_false, 3);
                    sum += BoolToByte.Evaluate(s_true, 3, 2);
                    sum -= 7;
                }
            }

            return sum;
        }

        [StructLayout(LayoutKind.Explicit, Size = 1, Pack = 1)]
        internal ref struct BoolToByte
        {
            [FieldOffset(0)]
            public bool Bool;

            [FieldOffset(0)]
            public byte Byte;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static int Evaluate(bool condition)
                => new BoolToByte { Bool = condition }.Byte;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static int Evaluate(bool condition, int trueValue)
                => Evaluate(condition) * trueValue;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static uint Evaluate(bool condition, int trueValue, int falseValue)
            {
                int val = Evaluate(condition);

                val = (val * trueValue)
                    + ((1 - val) * falseValue);

                return (byte)val;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static bool Evaluate(int value)
            {
                uint val = BitOps.FillTrailingOnes(unchecked((uint)value));
                val &= 1;

                Debug.Assert(val == 0 || val == 1);

                return new BoolToByte { Byte = (byte)val }.Bool;
            }
        }

        #endregion
    }
}
