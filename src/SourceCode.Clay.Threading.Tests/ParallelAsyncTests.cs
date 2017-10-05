using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SourceCode.Clay.Threading.Tests
{
    public static class ParallelAsyncTests
    {
        [Trait("Type", "Unit")]
        [Theory(DisplayName = nameof(ParallelAsync_ForEach_Action_Default_Arguments))]
        [InlineData(null)]
        [InlineData(-1)]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(16)]
        public static void ParallelAsync_ForEach_Action_Default_Arguments(int? maxDop)
        {
            var data = new int[] { 0, 1, 2 };
            var options = maxDop.HasValue ? new ParallelOptions { MaxDegreeOfParallelism = maxDop.Value } : null;

            // Null body
            Func<int, Task> action = null;
            Assert.ThrowsAsync<ArgumentNullException>(() => ParallelAsync.ForEachAsync(data, options, action));

            var actual = 0;
            action = n =>
            {
                Interlocked.Increment(ref actual);
                return Task.CompletedTask;
            };

            // Null source
            actual = 0;
            ParallelAsync.ForEachAsync(null, options, action).Wait();
            Assert.Equal(0, actual);

            // Empty source
            actual = 0;
            ParallelAsync.ForEachAsync(Array.Empty<int>(), options, action).Wait();
            Assert.Equal(0, actual);

            // Null options
            actual = 0;
            ParallelAsync.ForEachAsync(data, options, action).Wait();
            Assert.Equal(data.Length, actual);
        }

        [Trait("Type", "Unit")]
        [Theory(DisplayName = nameof(ParallelAsync_ForEach_Action))]
        [InlineData(null)]
        [InlineData(-1)]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(16)]
        public static void ParallelAsync_ForEach_Action(int? maxDop)
        {
            var data = new int[] { 0, 1, 2 };
            var options = maxDop.HasValue ? new ParallelOptions { MaxDegreeOfParallelism = maxDop.Value } : null;

            Func<int, Task> action = n =>
            {
                data[n] = n * 2;
                return Task.CompletedTask;
            };

            ParallelAsync.ForEachAsync(data, options, action).Wait();

            Assert.Collection(data, n => Assert.Equal(0, n), n => Assert.Equal(2, n), n => Assert.Equal(4, n));
        }

        [Trait("Type", "Unit")]
        [Theory(DisplayName = nameof(ParallelAsync_For_Action_Default_Arguments))]
        [InlineData(null)]
        [InlineData(-1)]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(16)]
        public static void ParallelAsync_For_Action_Default_Arguments(int? maxDop)
        {
            var data = new int[] { 0, 1, 2 };
            var options = maxDop.HasValue ? new ParallelOptions { MaxDegreeOfParallelism = maxDop.Value } : null;

            // Null body
            Func<int, Task> action = null;
            Assert.ThrowsAsync<ArgumentNullException>(() => ParallelAsync.ForAsync(0, data.Length, options, action));

            var actual = 0;
            action = n =>
            {
                Interlocked.Increment(ref actual);
                return Task.CompletedTask;
            };

            // Bad range
            Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => ParallelAsync.ForAsync(0, -1, options, action));

            // Empty source (-1)
            actual = 0;
            ParallelAsync.ForAsync(-1, -1, options, action).Wait();
            Assert.Equal(0, actual);

            // Empty source (0)
            actual = 0;
            ParallelAsync.ForAsync(0, 0, options, action).Wait();
            Assert.Equal(0, actual);

            // Empty source (100)
            actual = 0;
            ParallelAsync.ForAsync(100, 100, options, action).Wait();
            Assert.Equal(0, actual);
        }

        [Trait("Type", "Unit")]
        [Theory(DisplayName = nameof(ParallelAsync_For_Action))]
        [InlineData(null)]
        [InlineData(-1)]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(16)]
        public static void ParallelAsync_For_Action(int? maxDop)
        {
            var data = new int[] { 0, 1, 2 };
            var options = maxDop.HasValue ? new ParallelOptions { MaxDegreeOfParallelism = maxDop.Value } : null;

            Func<int, Task> action = i =>
            {
                data[i] = data[i] * 2;
                return Task.CompletedTask;
            };

            ParallelAsync.ForAsync(0, data.Length, options, action).Wait();

            Assert.Collection(data, n => Assert.Equal(0, n), n => Assert.Equal(2, n), n => Assert.Equal(4, n));
        }

        [Trait("Type", "Unit")]
        [Theory(DisplayName = nameof(ParallelAsync_ForEach_Func_Default_Arguments))]
        [InlineData(null)]
        [InlineData(-1)]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(16)]
        public static void ParallelAsync_ForEach_Func_Default_Arguments(int? maxDop)
        {
            var data = new int[] { 0, 1, 2 };
            var options = maxDop.HasValue ? new ParallelOptions { MaxDegreeOfParallelism = maxDop.Value } : null;

            // Null body
            Func<int, Task<KeyValuePair<int, int>>> func = null;
            Assert.ThrowsAsync<ArgumentNullException>(async () => await ParallelAsync.ForEachAsync(data, options, func));

            var actual = 0;
            func = n =>
            {
                Interlocked.Increment(ref actual);
                return Task.FromResult(new KeyValuePair<int, int>(n, n));
            };

            // Null source
            actual = 0;
            var result = ParallelAsync.ForEachAsync(null, options, func).Result;
            Assert.Equal(0, actual);
            Assert.Empty(result);

            // Empty source
            actual = 0;
            result = ParallelAsync.ForEachAsync(Array.Empty<int>(), options, func).Result;
            Assert.Equal(0, actual);
            Assert.Empty(result);
        }

        [Trait("Type", "Unit")]
        [Theory(DisplayName = nameof(ParallelAsync_ForEach_Func))]
        [InlineData(null)]
        [InlineData(-1)]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(16)]
        public static void ParallelAsync_ForEach_Func(int? maxDop)
        {
            var data = new int[] { 0, 1, 2 };
            var options = maxDop.HasValue ? new ParallelOptions { MaxDegreeOfParallelism = maxDop.Value } : null;

            Func<int, Task<KeyValuePair<int, int>>> func = n =>
            {
                return Task.FromResult(new KeyValuePair<int, int>(n, n * 2));
            };

            var actual = ParallelAsync.ForEachAsync(data, options, func);

            Assert.Collection(actual.Result, n => Assert.Equal(0, n.Value), n => Assert.Equal(2, n.Value), n => Assert.Equal(4, n.Value));
        }

        [Trait("Type", "Unit")]
        [Theory(DisplayName = nameof(ParallelAsync_For_Func_Default_Arguments))]
        [InlineData(null)]
        [InlineData(-1)]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(16)]
        public static void ParallelAsync_For_Func_Default_Arguments(int? maxDop)
        {
            var data = new int[] { 0, 1, 2 };
            var options = maxDop.HasValue ? new ParallelOptions { MaxDegreeOfParallelism = maxDop.Value } : null;

            // Null body
            Func<int, Task<KeyValuePair<int, int>>> func = null;
            Assert.ThrowsAsync<ArgumentNullException>(() => ParallelAsync.ForAsync(0, data.Length, options, func));

            var actual = 0;
            func = n =>
            {
                Interlocked.Increment(ref actual);
                return Task.FromResult(new KeyValuePair<int, int>(n, n * 2));
            };

            // Bad range
            Assert.ThrowsAsync<ArgumentNullException>(() => ParallelAsync.ForAsync(0, -1, options, func));

            // Empty source (-1)
            actual = 0;
            var result = ParallelAsync.ForAsync(-1, -1, options, func).Result;
            Assert.Equal(0, actual);
            Assert.Empty(result);

            // Empty source (0)
            actual = 0;
            result = ParallelAsync.ForAsync(0, 0, options, func).Result;
            Assert.Equal(0, actual);
            Assert.Empty(result);

            // Empty source (100)
            actual = 0;
            result = ParallelAsync.ForAsync(100, 100, options, func).Result;
            Assert.Equal(0, actual);
            Assert.Empty(result);
        }

        [Trait("Type", "Unit")]
        [Theory(DisplayName = nameof(ParallelAsync_For_Func))]
        [InlineData(null)]
        [InlineData(-1)]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(16)]
        public static void ParallelAsync_For_Func(int? maxDop)
        {
            var data = new int[] { 0, 1, 2 };
            var options = maxDop.HasValue ? new ParallelOptions { MaxDegreeOfParallelism = maxDop.Value } : null;

            Func<int, Task<int>> func = i =>
            {
                return Task.FromResult(data[i] * 2);
            };

            var actual = ParallelAsync.ForAsync(0, data.Length, options, func);

            Assert.Collection(actual.Result, n => Assert.Equal(0, n.Value), n => Assert.Equal(2, n.Value), n => Assert.Equal(4, n.Value));
        }
    }
}
