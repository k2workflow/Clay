#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;

namespace SourceCode.Clay.Collections.Bench
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var test1 = new Int32SwitchVsDictionaryBench();
            test1.Lookup();
            test1.Switch();

            Summary summary1 = BenchmarkRunner.Run<Int32SwitchVsDictionaryBench>();
        }
    }
}
