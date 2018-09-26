#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using BenchmarkDotNet.Running;

namespace SourceCode.Clay.Buffers.Bench
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            //var bench = new SwitchBench();
            //var summary = BenchmarkRunner.Run<SwitchBench>();
            //var summary = BenchmarkRunner.Run<BoolBench>();
            //var summary = BenchmarkRunner.Run<MinBench>();
            var summary = BenchmarkRunner.Run<SpanBench>();

            //SpanBench.Legacy();
            //SpanBench.Cast();
            //SpanBench.Ptr();
        }
    }
}
