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

            var test2 = new StringSwitchVsDictionaryBench();
            test2.Lookup();
            test2.Switch();

            //var summary1 = BenchmarkRunner.Run<Int32SwitchVsDictionaryBench>();
            var summary2 = BenchmarkRunner.Run<StringSwitchVsDictionaryBench>();
        }
    }
}
