#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using Newtonsoft.Json.Linq;
using SourceCode.Clay.Json.Bench.Properties;
using System.Json;

namespace SourceCode.Clay.Json.Bench
{
    [MemoryDiagnoser]
    public class ParseBench
    {
        #region Fields

        private const int InvokeCount = 1;

        private readonly string _str;

        #endregion

        #region Constructors

        public ParseBench()
        {
            _str = Resources.AdventureWorks;
        }

        #endregion

        #region Methods

        [Benchmark(Baseline = true, OperationsPerInvoke = InvokeCount)]
        public long NewtonParse()
        {
            var total = 0L;
            for (var j = 0; j < InvokeCount; j++)
            {
                var json = JObject.Parse(_str);

                total += System.Linq.Enumerable.Count(json.Children());
            }

            return total;
        }

        [Benchmark(Baseline = false, OperationsPerInvoke = InvokeCount)]
        public long JsonValueParse()
        {
            var total = 0L;
            for (var j = 0; j < InvokeCount; j++)
            {
                var json = JsonValue.Parse(_str);

                total += json.Count;
            }

            return total;
        }

        #endregion
    }
}
