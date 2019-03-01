#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using System.Runtime.CompilerServices;
using BenchmarkDotNet.Attributes;

namespace SourceCode.Clay.Numerics.Bench
{
    /*
    */

    //[MemoryDiagnoser]
    public class LocateFirstBench
    {
        private const int _iterations = 5000;
        private const int N = ushort.MaxValue;

        [Benchmark(Baseline = false, OperationsPerInvoke = _iterations * N)]
        public static int ViaNew()
        {
            int sum = 0;

            for (int i = 0; i < _iterations; i++)
            {
                for (int n = 0; n <= N; n++)
                {
                    ulong value = (ulong)(uint.MaxValue / (_iterations + 1.0) / (n + 1.0));
                    sum += LocateFirstFoundByteNew(value);
                }
            }

            return sum;
        }

        [Benchmark(Baseline = true, OperationsPerInvoke = _iterations * N)]
        public static int ViaXor()
        {
            int sum = 0;

            for (int i = 0; i < _iterations; i++)
            {
                for (int n = 0; n <= N; n++)
                {
                    ulong value = (ulong)(uint.MaxValue / (_iterations + 1.0) / (n + 1.0));
                    sum += LocateFirstFoundByteXor(value);
                }
            }

            return sum;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int LocateFirstFoundByteNew(ulong value)
        {
            return BitOps.TrailingZeroCount(value) >> 3;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int LocateFirstFoundByteXor(ulong value)
        {
            //if (Bmi1.X64.IsSupported)
            //{
            //    return (int)(Bmi1.X64.TrailingZeroCount(value) >> 3);
            //}
            //else
            {
                // Flag least significant power of two bit
                ulong powerOfTwoFlag = value ^ (value - 1);
                // Shift all powers of two into the high byte and extract
                return (int)((powerOfTwoFlag * XorPowerOfTwoToHighByte) >> 57);
            }
        }

        private const ulong XorPowerOfTwoToHighByte = (0x07ul |
                                                       0x06ul << 8 |
                                                       0x05ul << 16 |
                                                       0x04ul << 24 |
                                                       0x03ul << 32 |
                                                       0x02ul << 40 |
                                                       0x01ul << 48) + 1;
    }
}
