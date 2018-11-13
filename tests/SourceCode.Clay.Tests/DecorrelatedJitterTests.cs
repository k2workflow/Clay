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
        private static readonly UniformDistribution s_uniform = new UniformDistribution(s_random);
        private static readonly NormalDistribution s_normal = NormalDistribution.FromRange(0, 1, s_random);

        [Fact(DisplayName = nameof(Jitter_seed_vs_non))]
        public static void Jitter_seed_vs_non()
        {
            var minDelay = TimeSpan.FromMilliseconds(10);
            var maxDelay = TimeSpan.FromMilliseconds(1500);

            var jitter1 = new DecorrelatedJitter(1, minDelay, maxDelay, null);
            var jitter2 = new DecorrelatedJitter(1, minDelay, maxDelay, s_uniform);

            Assert.NotEqual(jitter1.Generate().First(), jitter2.Generate().First());
        }

        [Fact(DisplayName = nameof(Jitter_uniform_vs_normal))]
        public static void Jitter_uniform_vs_normal()
        {
            var minDelay = TimeSpan.FromMilliseconds(10);
            var maxDelay = TimeSpan.FromMilliseconds(1500);

            var jitter1 = new DecorrelatedJitter(1, minDelay, maxDelay, new UniformDistribution(new Random(123456789)));
            var jitter2 = new DecorrelatedJitter(1, minDelay, maxDelay, NormalDistribution.FromRange(0, 1, new Random(123456789)));

            Assert.NotEqual(jitter1.Generate().First(), jitter2.Generate().First());
        }

        [Fact(DisplayName = nameof(Jitter_check_uniform_range_small))]
        public static void Jitter_check_uniform_range_small()
        {
            var minDelay = TimeSpan.FromMilliseconds(10);
            var maxDelay = TimeSpan.FromMilliseconds(1500);

            var jitter = new DecorrelatedJitter(100_000, minDelay, maxDelay, s_uniform);
            TimeSpan[] delays = jitter.Generate().ToArray();

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

            var jitter = new DecorrelatedJitter(100_000, minDelay, maxDelay, s_normal);
            TimeSpan[] delays = jitter.Generate().ToArray();

            Assert.All(delays, n => Assert.True(n >= minDelay && n <= maxDelay));

            int groupCount = delays
                .Select(n => n.TotalMilliseconds)
                .GroupBy(n => n)
                .Count();

            Assert.True(groupCount >= 1000); // 1,479
        }

        [Fact(DisplayName = nameof(Jitter_check_uniform_range_large))]
        public static void Jitter_check_uniform_range_large()
        {
            var minDelay = TimeSpan.FromMilliseconds(10);
            var maxDelay = TimeSpan.FromMilliseconds(150_000);

            var jitter = new DecorrelatedJitter(100_000, minDelay, maxDelay, s_uniform);
            TimeSpan[] delays = jitter.Generate().ToArray();

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

            var jitter = new DecorrelatedJitter(100_000, minDelay, maxDelay, s_normal);
            TimeSpan[] delays = jitter.Generate().ToArray();

            Assert.All(delays, n => Assert.True(n >= minDelay && n <= maxDelay));

            int groupCount = delays
                .Select(n => n.TotalMilliseconds)
                .GroupBy(n => n)
                .Count();

            Assert.True(groupCount >= 50_000); // 61,403
        }

        [Fact(DisplayName = nameof(Jitter_check_uniform_zero))]
        public static void Jitter_check_uniform_zero()
        {
            var minDelay = TimeSpan.FromMilliseconds(0);
            var maxDelay = TimeSpan.FromMilliseconds(0);

            var jitter = new DecorrelatedJitter(100_000, minDelay, maxDelay, s_uniform);

            TimeSpan[] delays = jitter.Generate().ToArray();
            Assert.All(delays, n => Assert.True(n == minDelay));
        }

        [Fact(DisplayName = nameof(Jitter_check_normal_zero))]
        public static void Jitter_check_normal_zero()
        {
            var minDelay = TimeSpan.FromMilliseconds(0);
            var maxDelay = TimeSpan.FromMilliseconds(0);

            var jitter = new DecorrelatedJitter(100_000, minDelay, maxDelay, s_normal);

            TimeSpan[] delays = jitter.Generate().ToArray();
            Assert.All(delays, n => Assert.True(n == minDelay));
        }

        [Fact(DisplayName = nameof(Jitter_check_uniform_empty))]
        public static void Jitter_check_uniform_empty()
        {
            var minDelay = TimeSpan.FromMilliseconds(10);
            var maxDelay = TimeSpan.FromMilliseconds(1500);

            var jitter = new DecorrelatedJitter(0, minDelay, maxDelay, s_uniform);

            TimeSpan[] delays = jitter.Generate().ToArray();
            Assert.Empty(delays);
        }

        [Fact(DisplayName = nameof(Jitter_check_normal_empty))]
        public static void Jitter_check_normal_empty()
        {
            var minDelay = TimeSpan.FromMilliseconds(10);
            var maxDelay = TimeSpan.FromMilliseconds(1500);

            var jitter = new DecorrelatedJitter(0, minDelay, maxDelay, s_normal);

            TimeSpan[] delays = jitter.Generate().ToArray();
            Assert.Empty(delays);
        }
    }
}
