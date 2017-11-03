#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using Newtonsoft.Json.Linq;
using SourceCode.Clay.Json.Bench.Properties;

//using System.Json;

namespace SourceCode.Clay.Json.Bench
{
    [MemoryDiagnoser]
    public class CloneBench
    {
        #region Fields

        private const int InvokeCount = 1;

        //private readonly JsonObject _json;
        private readonly JObject _newton;

        #endregion

        #region Constructors

        public CloneBench()
        {
            var str = Resources.AdventureWorks;

            //_json = (JsonObject)JsonValue.Parse(str);
            _newton = JObject.Parse(str);
        }

        #endregion

        #region Methods

        [Benchmark(Baseline = true, OperationsPerInvoke = InvokeCount)]
        public long ToStringClone()
        {
            var total = 0L;
            for (var j = 0; j < InvokeCount; j++)
            {
                //var str = _json.ToString();
                //var clone = JsonValue.Parse(str);

                var str = _newton.ToString();
                var clone = (JObject)JToken.Parse(str);

                total += clone.Count;
            }

            return total;
        }

        [Benchmark(Baseline = false, OperationsPerInvoke = InvokeCount)]
        public long NewtonDeepClone()
        {
            var total = 0L;
            for (var j = 0; j < InvokeCount; j++)
            {
                var clone = _newton.DeepClone();

                total += System.Linq.Enumerable.Count(clone.Children());
            }

            return total;
        }

        //[Benchmark(Baseline = false, OperationsPerInvoke = InvokeCount)]
        //public long SmartClone()
        //{
        //    var total = 0L;
        //    for (var j = 0; j < InvokeCount; j++)
        //    {
        //        var clone = _json.Clone();

        //        total += clone.Count;
        //    }

        //    return total;
        //}

        #endregion
    }
}
