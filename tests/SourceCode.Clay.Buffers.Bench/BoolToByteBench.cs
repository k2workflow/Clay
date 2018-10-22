#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using BenchmarkDotNet.Attributes;

namespace SourceCode.Clay.Buffers.Bench
{
    /*
      Method |     Mean |     Error |    StdDev |   Median | Scaled |
------------ |---------:|----------:|----------:|---------:|-------:|
      Branch | 4.449 ns | 0.0890 ns | 0.2297 ns | 4.367 ns |   0.98 | x
      Actual | 4.510 ns | 0.0197 ns | 0.0164 ns | 4.504 ns |   0.99 | x
  UnsafeCode | 5.645 ns | 0.0117 ns | 0.0091 ns | 5.645 ns |   1.24 |
    UnsafeAs | 4.552 ns | 0.0186 ns | 0.0156 ns | 4.552 ns |   1.00 | x
 UnsafeAsRef | 2.529 ns | 0.0157 ns | 0.0131 ns | 2.522 ns |   0.56 | xx
 UnionStruct | 5.646 ns | 0.0155 ns | 0.0137 ns | 5.644 ns |   1.24 |
    */

    //[MemoryDiagnoser]
    public class BoolToByteBench
    {
        private const int _iterations = 2000;
        private const int N = ushort.MaxValue;

        // Prevent folding by using volatile non-readonly non-constant
        private static volatile bool s_true = true;
        private static volatile bool s_false = false;

        #region Branch

        [Benchmark(Baseline = false, OperationsPerInvoke = _iterations * N)]
        public static ulong Branch()
        {
            ulong sum = 0;

            for (int i = 0; i < _iterations; i++)
            {
                for (int n = 0; n <= N; n++)
                {
                    sum += AsByteBranch(s_true);
                    sum++;
                    sum -= AsByteBranch(s_false);
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
        private static byte AsByteBranch(bool condition)
            => (byte)(condition ? 1 : 0);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static uint ToByteBranch(bool condition, uint trueValue)
        {
            uint val = AsByteBranch(condition);
            return val * trueValue;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static uint ToByteBranch(bool condition, uint trueValue, uint falseValue)
        {
            uint val = AsByteBranch(condition);
            return (val * trueValue) + (1 - val) * falseValue;
        }

        #endregion

        #region Actual

        [Benchmark(Baseline = false, OperationsPerInvoke = _iterations * N)]
        public static ulong Actual()
        {
            ulong sum = 0;

            for (int i = 0; i < _iterations; i++)
            {
                for (int n = 0; n <= N; n++)
                {
                    sum += BitOps.AsByte(s_true);
                    sum++;
                    sum -= BitOps.AsByte(s_false);
                    sum--;

                    sum += BitOps.If(s_true, 4u);
                    sum -= BitOps.If(s_false, 3u);
                    sum += BitOps.If(s_true, 3u, 2u);
                    sum -= 7;
                }
            }

            return sum;
        }

        #endregion

        #region UnsafeCode

        [Benchmark(Baseline = false, OperationsPerInvoke = _iterations * N)]
        public static ulong UnsafeCode()
        {
            ulong sum = 0;

            for (int i = 0; i < _iterations; i++)
            {
                for (int n = 0; n <= N; n++)
                {
                    sum += AsByteUnsafe(s_true);
                    sum++;
                    sum -= AsByteUnsafe(s_false);
                    sum--;

                    sum += IfUnsafe(s_true, 4);
                    sum -= IfUnsafe(s_false, 3);
                    sum += IfUnsafe(s_true, 3, 2);
                    sum -= 7;
                }
            }

            return sum;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static byte AsByteUnsafe(bool condition)
        {
            int val;
            unsafe
            {
                val = *(byte*)&condition;
            }

            // Normalize bool's underlying value to 0|1
            // https://github.com/dotnet/roslyn/issues/24652
            {
                val = -val; // Negation will set sign-bit iff non-zero
                val >>= 31; // Send sign-bit to lsb
                val &= 1; // Zero all other bits
            }

            return (byte)val;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static uint IfUnsafe(bool condition, uint trueValue)
        {
            uint val = AsByteUnsafe(condition);
            return val * trueValue;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static uint IfUnsafe(bool condition, uint trueValue, uint falseValue)
        {
            uint val = AsByteUnsafe(condition);
            return (val * trueValue) + (1 - val) * falseValue;
        }

        #endregion

        #region UnsafeAs

        [Benchmark(Baseline = true, OperationsPerInvoke = _iterations * N)]
        public static ulong UnsafeAs()
        {
            ulong sum = 0;

            for (int i = 0; i < _iterations; i++)
            {
                for (int n = 0; n <= N; n++)
                {
                    sum += AsByteUnsafeAs(s_true);
                    sum++;
                    sum -= AsByteUnsafeAs(s_false);
                    sum--;

                    sum += IfUnsafeAs(s_true, 4);
                    sum -= IfUnsafeAs(s_false, 3);
                    sum += IfUnsafeAs(s_true, 3, 2);
                    sum -= 7;
                }
            }

            return sum;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static byte AsByteUnsafeAs(bool condition)
        {
            int val = Unsafe.As<bool, byte>(ref condition);

            // Normalize bool's underlying value to 0|1
            // https://github.com/dotnet/roslyn/issues/24652
            {
                val = -val; // Negation will set sign-bit iff non-zero
                val >>= 31; // Send sign-bit to lsb
                val &= 1; // Zero all other bits
            }

            return (byte)val;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static uint IfUnsafeAs(bool condition, uint trueValue)
        {
            uint val = AsByteUnsafeAs(condition);
            return val * trueValue;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static uint IfUnsafeAs(bool condition, uint trueValue, uint falseValue)
        {
            uint val = AsByteUnsafeAs(condition);
            return (val * trueValue) + (1 - val) * falseValue;
        }

        #endregion

        #region UnsafeAsRef

        [Benchmark(Baseline = false, OperationsPerInvoke = _iterations * N)]
        public static ulong UnsafeAsRef()
        {
            ulong sum = 0;

            for (int i = 0; i < _iterations; i++)
            {
                for (int n = 0; n <= N; n++)
                {
#pragma warning disable CS0420 // A reference to a volatile field will not be treated as volatile
                    sum += AsByteUnsafeAsRef(ref s_true);
                    sum++;
                    sum -= AsByteUnsafeAsRef(ref s_false);
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
        private static byte AsByteUnsafeAsRef(ref bool condition)
        {
            int val = Unsafe.As<bool, byte>(ref condition);

            // Normalize bool's underlying value to 0|1
            // https://github.com/dotnet/roslyn/issues/24652
            {
                val = -val; // Negation will set sign-bit iff non-zero
                val >>= 31; // Send sign-bit to lsb
                val &= 1; // Zero all other bits
            }

            return (byte)val;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static uint ToByteUnsafeAsRef(ref bool condition, uint trueValue)
        {
            uint val = AsByteUnsafeAsRef(ref condition);
            return val * trueValue;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static uint ToByteUnsafeAsRef(ref bool condition, uint trueValue, uint falseValue)
        {
            uint val = AsByteUnsafeAsRef(ref condition);
            return (val * trueValue) + (1 - val) * falseValue;
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
                    sum += BoolToByte.AsByte(s_true);
                    sum++;
                    sum -= BoolToByte.AsByte(s_false);
                    sum--;

                    sum += BoolToByte.If(s_true, 4);
                    sum -= BoolToByte.If(s_false, 3);
                    sum += BoolToByte.If(s_true, 3, 2);
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
            public static uint AsByte(bool condition)
            {
                int val = new BoolToByte { Bool = condition }.Byte;

                // Normalize bool's underlying value to 0|1
                // https://github.com/dotnet/roslyn/issues/24652
                {
                    val = -val; // Negation will set sign-bit iff non-zero
                    val >>= 31; // Send sign-bit to lsb
                    val &= 1; // Zero all other bits
                }

                return (byte)val;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static uint If(bool condition, uint trueValue)
            {
                uint val = AsByte(condition);
                return val * trueValue;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static uint If(bool condition, uint trueValue, uint falseValue)
            {
                uint val = AsByte(condition);
                return (val * trueValue) + (1 - val) * falseValue;
            }
        }

        #endregion
    }
}
