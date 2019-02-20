#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;

namespace SourceCode.Clay.Buffers.Bench
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            //Summary bench = new SwitchBench();
            //Summary summary = BenchmarkRunner.Run<SwitchBench>();
            //Summary summary = BenchmarkRunner.Run<BoolToByteBench>();
            //Summary summary = BenchmarkRunner.Run<BranchBench>();
            //Summary summary = BenchmarkRunner.Run<TrailingZeroBench>();
            //Summary summary = BenchmarkRunner.Run<LocateFirstBench>();
            //Summary summary = BenchmarkRunner.Run<PopCountBench>();
            Summary summary = BenchmarkRunner.Run<Log2Bench>();
            //Summary summary = BenchmarkRunner.Run<MinBench>();
            //Summary summary = BenchmarkRunner.Run<SpanBench>();

            //SpanBench.Legacy();
            //SpanBench.Cast();
            //SpanBench.Ptr();
        }
    }
}
