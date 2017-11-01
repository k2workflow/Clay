#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using BenchmarkDotNet.Running;

namespace SourceCode.Clay.Json.Bench
{
    public static class Program
    {
        #region Methods

        public static void Main(string[] args)
        {
            //var test1 = new JsonCloneBench();
            //var summary1 = BenchmarkRunner.Run<JsonCloneBench>();

            //var test2 = new JsonEqualsBench();
            //var summary2 = BenchmarkRunner.Run<JsonEqualsBench>();

            var switcher = new BenchmarkSwitcher(new[]
            {
                typeof(JsonCloneBench),
                typeof(JsonEqualsBench),
            });
            switcher.Run(args);
        }

        #endregion
    }
}
