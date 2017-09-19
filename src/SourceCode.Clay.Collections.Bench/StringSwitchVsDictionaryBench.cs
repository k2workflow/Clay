using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using SourceCode.Clay.Collections.Generic;
using System;
using System.Collections.Generic;

namespace SourceCode.Clay.Collections.Bench
{
    public class StringSwitchVsDictionaryBench
    {
        private const int ItemCount = 50;
        private const int InvokeCount = 1000;

        private readonly Dictionary<string, int> dict;
        private readonly IDynamicSwitch<string, int> @switch;

        public StringSwitchVsDictionaryBench()
        {
            // Build dictionary
            dict = new Dictionary<string, int>(ItemCount);
            for (var i = 0; i < ItemCount; i++)
            {
                var key = new string((char)i, 10);
                dict[key] = i * 2;
            }

            // Build switch
            @switch = dict.ToDynamicSwitch(true);
        }

        [Benchmark(Baseline = true, OperationsPerInvoke = ItemCount * InvokeCount)]
        public int Lookup()
        {
            var total = 0;
            for (var j = 0; j < InvokeCount; j++)
            {
                for (var i = dict.Count - 1; i >= 0; i--)
                {
                    var key = new string((char)i, 10);

                    unchecked
                    {
                        total += dict[key];
                    }
                }
            }

            return total;
        }

        [Benchmark(Baseline = false, OperationsPerInvoke = ItemCount * InvokeCount)]
        public int Switch()
        {
            var total = 0;
            for (var j = 0; j < InvokeCount; j++)
            {
                for (var i = dict.Count - 1; i >= 0; i--)
                {
                    var key = new string((char)i, 10);

                    unchecked
                    {
                        total += @switch[key];
                    }
                }
            }

            return total;
        }
    }
}
