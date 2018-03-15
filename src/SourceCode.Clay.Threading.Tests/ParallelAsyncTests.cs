#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SourceCode.Clay.Threading.Tests
{
    public static class ParallelAsyncTests
    {
        #region Methods

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
            var actual = 0;
            Assert.ThrowsAsync<ArgumentNullException>(() => ParallelAsync.ForEachAsync(data, options, action));

            // Null source
            actual = 0;
            ParallelAsync.ForEachAsync((int[])null, options, action).Wait();
            Assert.Equal(0, actual);

            // Empty source
            actual = 0;
            ParallelAsync.ForEachAsync(Array.Empty<int>(), options, action).Wait();
            Assert.Equal(0, actual);

            // Null options
            actual = 0;
            ParallelAsync.ForEachAsync(data, options, action).Wait();
            Assert.Equal(data.Length, actual);

            // Local functions
            Task action(int n)
            {
                Interlocked.Increment(ref actual);
                return Task.CompletedTask;
            };
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

            ParallelAsync.ForEachAsync(data, options, action).Wait();

            Assert.Collection(data, n => Assert.Equal(0, n), n => Assert.Equal(2, n), n => Assert.Equal(4, n));

            // Local functions
            Task action(int n)
            {
                data[n] = n * 2;
                return Task.CompletedTask;
            };
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
            var actual = 0;
            Assert.ThrowsAsync<ArgumentNullException>(() => ParallelAsync.ForAsync(0, data.Length, options, action));

            // Bad range
            actual = 0;
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

            // Local functions
            Task action(int n)
            {
                Interlocked.Increment(ref actual);
                return Task.CompletedTask;
            };
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

            ParallelAsync.ForAsync(0, data.Length, options, action).Wait();

            Assert.Collection(data, n => Assert.Equal(0, n), n => Assert.Equal(2, n), n => Assert.Equal(4, n));

            // Local functions
            Task action(int i)
            {
                data[i] = data[i] * 2;
                return Task.CompletedTask;
            };
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
            var actual = 0;
            Assert.ThrowsAsync<ArgumentNullException>(async () => await ParallelAsync.ForEachAsync(data, options, func));

            // Null source
            actual = 0;
            var result = ParallelAsync.ForEachAsync((int[])null, options, func).Result;
            Assert.Equal(0, actual);
            Assert.Empty(result);

            // Empty source
            actual = 0;
            result = ParallelAsync.ForEachAsync(Array.Empty<int>(), options, func).Result;
            Assert.Equal(0, actual);
            Assert.Empty(result);

            // Local functions
            Task<KeyValuePair<int, int>> func(int n)
            {
                Interlocked.Increment(ref actual);
                return Task.FromResult(new KeyValuePair<int, int>(n, n));
            };
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

            var actual = ParallelAsync.ForEachAsync(data, options, func);

            Assert.Collection(actual.Result, n => Assert.Equal(0, n.Value), n => Assert.Equal(2, n.Value), n => Assert.Equal(4, n.Value));

            // Local functions
            Task<KeyValuePair<int, int>> func(int n)
            {
                return Task.FromResult(new KeyValuePair<int, int>(n, n * 2));
            };
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
            var actual = 0;
            Assert.ThrowsAsync<ArgumentNullException>(() => ParallelAsync.ForAsync(0, data.Length, options, func));

            // Bad range
            actual = 0;
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

            // Local functions
            Task<KeyValuePair<int, int>> func(int n)
            {
                Interlocked.Increment(ref actual);
                return Task.FromResult(new KeyValuePair<int, int>(n, n * 2));
            };
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

            var actual = ParallelAsync.ForAsync(0, data.Length, options, func);

            Assert.Collection(actual.Result, n => Assert.Equal(0, n.Value), n => Assert.Equal(2, n.Value), n => Assert.Equal(4, n.Value));

            // Local functions
            Task<int> func(int i)
            {
                return Task.FromResult(data[i] * 2);
            };
        }

        #endregion

        #region Timing

        private const int delay = 50;
        private const int loops = 100;

        [Trait("Type", "Unit")]
        [Theory(DisplayName = nameof(ParallelAsync_For_Action_Delay))]
        [InlineData(null)]
        [InlineData(-1)]
        [InlineData(4)]
        public static void ParallelAsync_For_Action_Delay(int? maxDop)
        {
            var options = maxDop.HasValue ? new ParallelOptions { MaxDegreeOfParallelism = maxDop.Value } : null;

            var sw = new Stopwatch();
            sw.Start();

            var actual = 0;

            ParallelAsync.ForAsync(0, loops, options, func).Wait();

            sw.Stop();

            Assert.Equal(loops, actual);
            Assert.True(sw.ElapsedMilliseconds < delay * loops); // Environmental factors mean we can't assert a lower boundary

            // Local functions
            async Task func(int i)
            {
                Interlocked.Increment(ref actual);
                await Task.Delay(delay).ConfigureAwait(false);
            };
        }

        [Trait("Type", "Unit")]
        [Theory(DisplayName = nameof(ParallelAsync_For_Func_Delay))]
        [InlineData(null)]
        [InlineData(-1)]
        [InlineData(4)]
        public static void ParallelAsync_For_Func_Delay(int? maxDop)
        {
            var options = maxDop.HasValue ? new ParallelOptions { MaxDegreeOfParallelism = maxDop.Value } : null;

            var sw = new Stopwatch();
            sw.Start();

            var actual = 0;
            var result = ParallelAsync.ForAsync(0, loops, options, func).Result;

            sw.Stop();

            Assert.Equal(loops, actual);
            Assert.Equal(loops, result.Count);
            Assert.True(sw.ElapsedMilliseconds < delay * loops); // Environmental factors mean we can't assert a lower boundary

            // Local functions
            async Task<KeyValuePair<int, int>> func(int n)
            {
                Interlocked.Increment(ref actual);
                await Task.Delay(delay).ConfigureAwait(false);
                return new KeyValuePair<int, int>(n, n * 2);
            };
        }

        [Trait("Type", "Unit")]
        [Theory(DisplayName = nameof(ParallelAsync_ForEach_Action_Delay))]
        [InlineData(null)]
        [InlineData(-1)]
        [InlineData(4)]
        public static void ParallelAsync_ForEach_Action_Delay(int? maxDop)
        {
            var options = maxDop.HasValue ? new ParallelOptions { MaxDegreeOfParallelism = maxDop.Value } : null;

            var sw = new Stopwatch();
            sw.Start();

            var actual = 0;
            ParallelAsync.ForEachAsync(Enumerable.Range(0, loops), options, func).Wait();

            sw.Stop();

            Assert.Equal(loops, actual);
            Assert.True(sw.ElapsedMilliseconds < delay * loops); // Environmental factors mean we can't assert a lower boundary

            // Local functions
            async Task func(int i)
            {
                Interlocked.Increment(ref actual);
                await Task.Delay(delay).ConfigureAwait(false);
            };
        }

        [Trait("Type", "Unit")]
        [Theory(DisplayName = nameof(ParallelAsync_ForEach_Func_Delay))]
        [InlineData(null)]
        [InlineData(-1)]
        [InlineData(4)]
        public static void ParallelAsync_ForEach_Func_Delay(int? maxDop)
        {
            var options = maxDop.HasValue ? new ParallelOptions { MaxDegreeOfParallelism = maxDop.Value } : null;

            var sw = new Stopwatch();
            sw.Start();

            var actual = 0;
            var result = ParallelAsync.ForEachAsync(Enumerable.Range(0, loops), options, func).Result;

            sw.Stop();

            Assert.Equal(loops, actual);
            Assert.True(sw.ElapsedMilliseconds < delay * loops); // Environmental factors mean we can't assert a lower boundary

            // Local functions
            async Task<KeyValuePair<int, int>> func(int n)
            {
                Interlocked.Increment(ref actual);
                await Task.Delay(delay).ConfigureAwait(false);
                return new KeyValuePair<int, int>(n, n * 2);
            };
        }

        #endregion
    }
}
