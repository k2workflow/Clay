#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using SourceCode.Clay.Collections.Generic;
using System.Collections.Generic;

namespace SourceCode.Clay.Collections.Bench
{
    public class Int32SwitchVsDictionaryBench
    {
        #region Fields

        private const int ItemCount = 50;
        private const int InvokeCount = 1000;

        private readonly Dictionary<int, int> dict;
        private readonly IDynamicSwitch<int, int> @switch;

        #endregion

        #region Constructors

        public Int32SwitchVsDictionaryBench()
        {
            // Build dictionary
            dict = new Dictionary<int, int>(ItemCount);
            for (var i = 0; i < ItemCount; i++)
                dict[i] = i * 2;

            // Build switch
            @switch = dict.ToDynamicSwitch();
        }

        #endregion

        #region Methods

        [Benchmark(Baseline = true, OperationsPerInvoke = ItemCount * InvokeCount)]
        public long Lookup()
        {
            var total = 0L;
            for (var j = 0; j < InvokeCount; j++)
            {
                for (var i = dict.Count - 1; i >= 0; i--)
                {
                    unchecked
                    {
                        total += dict[i];
                    }
                }
            }

            return total;
        }

        [Benchmark(Baseline = false, OperationsPerInvoke = ItemCount * InvokeCount)]
        public long Switch()
        {
            var total = 0L;
            for (var j = 0; j < InvokeCount; j++)
            {
                for (var i = dict.Count - 1; i >= 0; i--)
                {
                    unchecked
                    {
                        total += @switch[i];
                    }
                }
            }

            return total;
        }

        #endregion
    }
}
