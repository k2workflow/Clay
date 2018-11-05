#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using System;
using System.Linq;
using Xunit;

namespace SourceCode.Clay.Tests
{
    public static class DecorrelatedJitterTests
    {
        private static readonly Random s_random = new Random(123456789); // Specific seed for determinism

        [Fact(DisplayName = nameof(Jitter_check_default_range_small))]
        public static void Jitter_check_default_range_small()
        {
            var minDelay = TimeSpan.FromMilliseconds(10);
            var maxDelay = TimeSpan.FromMilliseconds(1500);

            var jitter = new DecorrelatedJitter(100_000, minDelay, maxDelay, s_random);
            TimeSpan[] delays = jitter.Default().ToArray();

            Assert.All(delays, n => Assert.True(n >= minDelay && n <= maxDelay));

            int groupCount = delays
                .Select(n => n.TotalMilliseconds)
                .GroupBy(n => n)
                .Count();

            Assert.True(groupCount >= 1000); // 1,491
        }

        [Fact(DisplayName = nameof(Jitter_check_normal_range_small))]
        public static void Jitter_check_normal_range_small()
        {
            var minDelay = TimeSpan.FromMilliseconds(10);
            var maxDelay = TimeSpan.FromMilliseconds(1500);

            var jitter = new DecorrelatedJitter(100_000, minDelay, maxDelay, s_random);
            TimeSpan[] delays = jitter.Normal().ToArray();

            Assert.All(delays, n => Assert.True(n >= minDelay && n <= maxDelay));

            int groupCount = delays
                .Select(n => n.TotalMilliseconds)
                .GroupBy(n => n)
                .Count();

            Assert.True(groupCount >= 1000); // 1,479
        }

        [Fact(DisplayName = nameof(Jitter_check_default_range_large))]
        public static void Jitter_check_default_range_large()
        {
            var minDelay = TimeSpan.FromMilliseconds(10);
            var maxDelay = TimeSpan.FromMilliseconds(150_000);

            var jitter = new DecorrelatedJitter(100_000, minDelay, maxDelay, s_random);
            TimeSpan[] delays = jitter.Default().ToArray();

            Assert.All(delays, n => Assert.True(n >= minDelay && n <= maxDelay));

            int groupCount = delays
                .Select(n => n.TotalMilliseconds)
                .GroupBy(n => n)
                .Count();

            Assert.True(groupCount >= 30_000); // 38,572
        }

        [Fact(DisplayName = nameof(Jitter_check_normal_range_large))]
        public static void Jitter_check_normal_range_large()
        {
            var minDelay = TimeSpan.FromMilliseconds(10);
            var maxDelay = TimeSpan.FromMilliseconds(150_000);

            var jitter = new DecorrelatedJitter(100_000, minDelay, maxDelay, s_random);
            TimeSpan[] delays = jitter.Normal().ToArray();

            Assert.All(delays, n => Assert.True(n >= minDelay && n <= maxDelay));

            int groupCount = delays
                .Select(n => n.TotalMilliseconds)
                .GroupBy(n => n)
                .Count();

            Assert.True(groupCount >= 50_000); // 61,403
        }

        [Fact(DisplayName = nameof(Jitter_check_zero))]
        public static void Jitter_check_zero()
        {
            var minDelay = TimeSpan.FromMilliseconds(0);
            var maxDelay = TimeSpan.FromMilliseconds(0);

            var jitter = new DecorrelatedJitter(100_000, minDelay, maxDelay, s_random);

            // Default
            TimeSpan[] delays = jitter.Default().ToArray();
            Assert.All(delays, n => Assert.True(n == minDelay));

            // Normal
            delays = jitter.Normal().ToArray();
            Assert.All(delays, n => Assert.True(n == minDelay));
        }

        [Fact(DisplayName = nameof(Jitter_check_empty))]
        public static void Jitter_check_empty()
        {
            var minDelay = TimeSpan.FromMilliseconds(10);
            var maxDelay = TimeSpan.FromMilliseconds(1500);

            var jitter = new DecorrelatedJitter(0, minDelay, maxDelay, s_random);

            // Default
            TimeSpan[] delays = jitter.Default().ToArray();
            Assert.Empty(delays);

            // Normal
            delays = jitter.Normal().ToArray();
            Assert.Empty(delays);
        }
    }
}
