#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using SourceCode.Clay.Json.Bench.Properties;
using System;
using System.Json;

namespace SourceCode.Clay.Json.Bench
{
    [MemoryDiagnoser]
    public class JsonEqualsBench
    {
        #region Fields

        private const int InvokeCount = 1;

        private readonly JsonObject _json1;
        private readonly JsonObject _json2;

        #endregion

        #region Constructors

        public JsonEqualsBench()
        {
            var str = Resources.AdventureWorks;
            _json1 = (JsonObject)JsonValue.Parse(str);
            _json2 = (JsonObject)JsonValue.Parse(str);
        }

        #endregion

        #region Methods

        [Benchmark(Baseline = true, OperationsPerInvoke = InvokeCount)]
        public long ToStringEquals()
        {
            var total = 0L;
            for (var j = 0; j < InvokeCount; j++)
            {
                var str1 = _json1.ToString();
                var str2 = _json2.ToString();

                var equal = StringComparer.Ordinal.Equals(str1, str2);

                total += (equal ? 1 : 0);
            }

            return total;
        }

        [Benchmark(Baseline = false, OperationsPerInvoke = InvokeCount)]
        public long SmartEquals()
        {
            var total = 0L;
            for (var j = 0; j < InvokeCount; j++)
            {
                var equal = JsonValueComparer.Default.Equals(_json1, _json2);

                total += (equal ? 1 : 0);
            }

            return total;
        }

        #endregion
    }
}
