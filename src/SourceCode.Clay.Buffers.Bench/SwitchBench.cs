#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using BenchmarkDotNet.Attributes;
using System;

namespace SourceCode.Clay.Buffers.Bench
{
    [MemoryDiagnoser]
    public class SwitchBench
    {
        private const int _iterations = 1000;

        [Benchmark(Baseline = false, OperationsPerInvoke = _iterations)]
        public static ulong Now()
        {
            var sum = 0ul;

            for (var i = 0; i < _iterations; i++)
            {
                sum++;
            }

            return sum;
        }

        [Benchmark(Baseline = true, OperationsPerInvoke = _iterations)]
        public static ulong Was()
        {
            var sum = 0ul;

            for (var i = 0; i < _iterations; i++)
            {
                sum++;
            }

            return sum;
        }
    }
}
