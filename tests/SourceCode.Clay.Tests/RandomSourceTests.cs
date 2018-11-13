#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using System;
using System.Linq;
using Xunit;

namespace SourceCode.Clay.Tests
{
    public static class RandomSourceTests
    {
        private static readonly Random s_random = new Random(123456789); // Specific seed for determinism

        [Fact(DisplayName = nameof(Random_clamped_uniform))]
        public static void Random_clamped_uniform()
        {
            const int count = 100_000;
            const double min = 10;
            const double max = 1500;

            var normal = new RandomUniform(min, max, s_random);
            double[] values = normal.Sample(count).ToArray();

            Assert.All(values, n => Assert.True(n >= min && n <= max));

            int groupCount = values
                .GroupBy(n => n)
                .Count();

            Assert.True(groupCount >= 1000); // 1,479
        }

        [Fact(DisplayName = nameof(Random_clamped_normal))]
        public static void Random_clamped_normal()
        {
            const int count = 100_000;
            const double min = 10;
            const double max = 1500;

            var normal = RandomNormal.FromRange(min, max, s_random);
            double[] values = normal.Sample(count).ToArray();

            Assert.All(values, n => Assert.True(n >= min && n <= max));

            int groupCount = values
                .GroupBy(n => n)
                .Count();

            Assert.True(groupCount >= 1000); // 1,479
        }

        [Fact(DisplayName = nameof(Random_clamped_uniform_range_zero))]
        public static void Random_clamped_uniform_range_zero()
        {
            const int count = 150_000;
            const double min = 10;
            const double max = 10;

            var normal = new RandomUniform(min, max, s_random);
            double[] values = normal.Sample(count).ToArray();

            Assert.All(values, n => Assert.True(n == min));
        }

        [Fact(DisplayName = nameof(Random_clamped_normal_range_zero))]
        public static void Random_clamped_normal_range_zero()
        {
            const int count = 150_000;
            const double min = 10;
            const double max = 10;

            var normal = RandomNormal.FromRange(min, max, s_random);
            double[] values = normal.Sample(count).ToArray();

            Assert.All(values, n => Assert.True(n == min));
        }

        [Fact(DisplayName = nameof(Random_derive_normal))]
        public static void Random_derive_normal()
        {
            const int count = 100_000;
            const double μ = 100; // Mean
            const double σ = 10; // Sigma

            var normal = RandomNormal.FromMuSigma(μ, σ, s_random);
            double[] values = normal.Sample(count).ToArray();

            // ~99.7% of population is within +/- 3 standard deviations
            const double sd = 3 * σ;
            int cnt = values
                .Where(n => n >= μ - sd && n <= μ + sd)
                .Count();

            Assert.True(cnt >= 0.990 * count); // 99,716

            double min = values.Min();
            double avg = values.Average();
            double max = values.Max();

            Assert.True(min < avg);
            Assert.True(avg < max);
        }

        [Fact(DisplayName = nameof(Random_derive_normal_sigma_zero))]
        public static void Random_derive_normal_sigma_zero()
        {
            const int count = 150_000;
            const double μ = 100; // Mean
            const double σ = 0; // Sigma

            var normal = RandomNormal.FromMuSigma(μ, σ, s_random);
            double[] values = normal.Sample(count).ToArray();

            Assert.All(values, n => Assert.True(n == μ));
        }
    }
}
