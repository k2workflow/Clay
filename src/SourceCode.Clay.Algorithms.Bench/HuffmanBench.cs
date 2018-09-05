// Derived from:
// https://raw.githubusercontent.com/aspnet/KestrelHttpServer/64127e6c766b221cf147383c16079d3b7aad2ded/test/Kestrel.Core.Tests/HuffmanTests.cs

// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using BenchmarkDotNet.Attributes;

namespace SourceCode.Clay.Algorithms.Bench
{
    [MemoryDiagnoser]
    public class HuffmanBench
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
