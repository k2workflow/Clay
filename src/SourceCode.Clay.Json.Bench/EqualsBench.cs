#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using Newtonsoft.Json.Linq;
using SourceCode.Clay.Json.Bench.Properties;
using System;

//using System.Json;

namespace SourceCode.Clay.Json.Bench
{
    [MemoryDiagnoser]
    public class EqualsBench
    {
        #region Fields

        private const int InvokeCount = 1;

        //private readonly JsonObject _json1;
        //private readonly JsonObject _json2;

        private readonly JObject _newton1;
        private readonly JObject _newton2;

        #endregion

        #region Constructors

        public EqualsBench()
        {
            var str1 = Resources.AdventureWorks;
            var str2 = Resources.AdventureWorks + "\n"; // Mitigate interning

            //_json1 = (JsonObject)JsonValue.Parse(str1);
            //_json2 = (JsonObject)JsonValue.Parse(str2);

            _newton1 = JObject.Parse(str1);
            _newton2 = JObject.Parse(str2);
        }

        #endregion

        #region Methods

        [Benchmark(Baseline = true, OperationsPerInvoke = InvokeCount)]
        public long ToStringEquals()
        {
            var total = 0L;
            for (var j = 0; j < InvokeCount; j++)
            {
                //var str1 = _json1.ToString();
                //var str2 = _json2.ToString();

                var str2 = _newton1.ToString();
                var str1 = _newton2.ToString();

                var equal = StringComparer.Ordinal.Equals(str1, str2);

                total += (equal ? 1 : 0);
            }

            return total;
        }

        [Benchmark(Baseline = false, OperationsPerInvoke = InvokeCount)]
        public long NewtonDeepEquals()
        {
            var total = 0L;
            for (var j = 0; j < InvokeCount; j++)
            {
                var equal = JToken.EqualityComparer.Equals(_newton1, _newton2);

                total += (equal ? 1 : 0);
            }

            return total;
        }

        //[Benchmark(Baseline = false, OperationsPerInvoke = InvokeCount)]
        //public long SmartEquals()
        //{
        //    var total = 0L;
        //    for (var j = 0; j < InvokeCount; j++)
        //    {
        //        var equal = JsonValueComparer.Default.Equals(_json1, _json2);

        //        total += (equal ? 1 : 0);
        //    }

        //    return total;
        //}

        #endregion
    }
}
