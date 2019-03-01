#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using BenchmarkDotNet.Attributes;

namespace SourceCode.Clay.Numerics.Bench
{
    /*
    */

    //[MemoryDiagnoser]
    public class DigitBench
    {
        private const int _iterations = 10;
        private const int N = 1000_000;

        //[Benchmark(Baseline = true, OperationsPerInvoke = _iterations * N)]
        public long ViaSpan()
        {
            long sum = 0;

            for (uint n = 0; n < N; n++)
            {
                sum += ViaSpan(n);
            }

            return sum;
        }

        [Benchmark(Baseline = false, OperationsPerInvoke = _iterations * N)]
        public long ViaUnsafe()
        {
            long sum = 0;

            for (uint n = 0; n < N; n++)
            {
                sum += ViaUnsafe(n);
            }

            return sum;
        }

        private static ReadOnlySpan<byte> s_Log10Ceiling32 => new byte[33]
        {
            11, 10, 10, 09, 09, 09, 08, 08,
            08, 07, 07, 07, 07, 06, 06, 06,
            05, 05, 05, 04, 04, 04, 04, 03,
            03, 03, 02, 02, 02, 01, 01, 01,
            02
        };

        private static readonly uint[] s_Pow10Ceiling32 = new uint[12]
        {
            0, 1, 10, 100, 1000, 10000,
            100000, 1000000, 10000000, 100000000, 1000000000, 0
        };

        private unsafe struct Foo
        {
            public fixed uint Bar[12];

            public Foo(int i)
            {
                Bar[00] = 0u;
                Bar[01] = 1u;
                Bar[02] = 10u;
                Bar[03] = 100u;
                Bar[04] = 1000u;
                Bar[05] = 10000u;
                Bar[06] = 100000u;
                Bar[07] = 1000000u;
                Bar[08] = 10000000u;
                Bar[09] = 100000000u;
                Bar[10] = 1000000000u;
                Bar[11] = 0;
            }
        }

        private static readonly Foo _foo = new Foo(1);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int ViaSpan(uint value)
        {
            int log10 = Unsafe.AddByteOffset(
                ref MemoryMarshal.GetReference(s_Log10Ceiling32),
                (IntPtr)BitOps.LeadingZeroCount(value));

            int diff = (int)((value - s_Pow10Ceiling32[log10]) >> 31);
            return (int)(log10 - diff);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int ViaUnsafe(uint value)
        {
            int log10 = Unsafe.AddByteOffset(
                ref MemoryMarshal.GetReference(s_Log10Ceiling32),
                (IntPtr)BitOps.LeadingZeroCount(value));

            //var u = Unsafe.AsRef<uint[]>(_foo);// s_Pow10Ceiling32);
            var u = new ReadOnlySpan<uint>(s_Pow10Ceiling32);

            //ReadOnlySpan<uint> u = stackalloc uint[12]
            //{
            //    0,
            //    1,
            //    10,
            //    100,
            //    1000,
            //    10000,
            //    100000,
            //    1000000,
            //    10000000,
            //    100000000,
            //    1000000000,
            //    0
            //};

            //fixed (uint* u = Unsafe.AsRef(_foo))
            {
                int diff = (int)((value - u[log10]) >> 31);
                //int diff = unchecked((int)((value - Unsafe.Add(ref s_Pow10Ceiling32.GetRawSzArrayData(), y)) >> 31));
                return (int)(log10 - diff);
            }
        }
    }
}
