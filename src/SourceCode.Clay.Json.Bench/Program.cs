#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using BenchmarkDotNet.Running;

namespace SourceCode.Clay.Json.Bench
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            //var test1 = new CloneBench();
            //var summary1 = BenchmarkRunner.Run<CloneBench>();

            //var test2 = new EqualsBench();
            //var summary2 = BenchmarkRunner.Run<EqualsBench>();

            //var test3 = new ParseBench();
            //var summary3 = BenchmarkRunner.Run<ParseBench>();

            //var test4 = new ToStringBench();
            //var summary4 = BenchmarkRunner.Run<ToStringBench>();

            var switcher = new BenchmarkSwitcher(new[]
            {
                typeof(CloneBench),
                typeof(EqualsBench),
                typeof(ParseBench),
                typeof(ToStringBench)
            });
            switcher.Run(args);
        }
    }
}
