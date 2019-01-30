#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using BenchmarkDotNet.Attributes;
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace SourceCode.Clay.Buffers.Bench
{
    //[MemoryDiagnoser]
    public class SwitchBench
    {
        private const uint _iterations = 100;
        private const uint N = byte.MaxValue;

        [Benchmark(Baseline = false, OperationsPerInvoke = (int)(_iterations * N))]
        public static ulong Switch()
        {
            var sum = 0ul;

            for (var i = 0; i < _iterations; i++)
            {
                for (var n = 0; n <= N; n++)
                {
                    sum += (ulong)TrailingZerosSwitch((byte)n);
                }
            }

            return sum;
        }

        [Benchmark(Baseline = true, OperationsPerInvoke = (int)(_iterations * N))]
        public static ulong Lookup()
        {
            var sum = 0ul;

            for (var i = 0; i < _iterations; i++)
            {
                for (var n = 0; n <= N; n++)
                {
                    sum += (ulong)BitOps.TrailingZeroCount((byte)n);
                }
            }

            return sum;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int TrailingZerosSwitch(byte value)
        {
            var val = value;

            // The expression (n & -n) returns lsb(n).
            // Only possible values are therefore [0,1,2,4,...,128]
            var lsb = val & -val; // eg 44==0010 1100 -> (44 & -44) -> 4. 4==0100, which is the lsb of 44.

            // We want to map [0...128] to the smallest contiguous range, ideally [0..9] since 9 is the range cardinality.
            // Mod-11 is a simple perfect-hashing scheme over this range, where 11 is chosen as the closest prime greater than 9.
            lsb = lsb % 11; // mod 11

            // Build this table by taking n = 0,1,2,4,...,512
            // [2^n % 11] = tz(n) manually counted
            byte cnt;
            switch (lsb)
            {
                //    n                            2^n  % 11     b=bin(n)   z=tz(b)
                case 00: cnt = 8; break;        //   0  [ 0]     0000_0000  8
                case 01: cnt = 0; break;        //   1  [ 1]     0000_0001  0 
                case 02: cnt = 1; break;        //   2  [ 2]     0000_0010  1
                case 03: cnt = 8; goto default; // 256  [ 3]  01_0000_0000  8 (n/a) 1u << 8

                case 04: cnt = 2; break;        //   4  [ 4]     0000_0100  2
                case 05: cnt = 4; break;        //  16  [ 5]     0001_0000  4
                case 06: cnt = 9; goto default; // 512  [ 6]  10_0000_0000  9 (n/a) 1u << 9
                case 07: cnt = 7; break;        // 128  [ 7]     1000_0000  7

                case 08: cnt = 3; break;        //   8  [ 8]     0000_1000  3
                case 09: cnt = 6; break;        //  64  [ 9]     0100_0000  6
                case 10: cnt = 5; break;        //  32  [10]     0010_0000  5

                default:
                    cnt = 10;
                    Debug.Fail($"{value} resulted in unexpected {typeof(byte)} hash {lsb}, with count {cnt}");
                    break;
            }

            return cnt;
        }
    }
}
