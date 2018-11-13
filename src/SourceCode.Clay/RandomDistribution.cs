#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using System;
using System.Collections.Generic;

namespace SourceCode.Clay
{
    public abstract class RandomDistribution
    {
        [ThreadStatic]
        private static readonly Random s_uniform = new Random();

        /// <summary>
        /// A default shared instance to use for Uniform distributions, in the range [0, 1).
        /// </summary>
        public static UniformDistribution Uniform { get; } = new UniformDistribution(0, 1);

        /// <summary>
        /// A default shared instance to use for Normal (Guass) distributions, in the range [0, 1).
        /// </summary>
        public static NormalDistribution Normal { get; } = NormalDistribution.FromRange(0, 1);

        private readonly Random _random;
        protected double Min { get; }
        protected double Max { get; }

        protected RandomDistribution(double min, double max, Random random)
        {
            if (min > max) throw new ArgumentOutOfRangeException(nameof(max));

            _random = random ?? s_uniform;
            Min = min;
            Max = max;
        }

        protected double SafeDouble()
        {
            // https://stackoverflow.com/questions/25448070/getting-random-numbers-in-a-thread-safe-way/25448166#25448166
            // https://docs.microsoft.com/en-us/dotnet/api/system.random?view=netframework-4.7.2#the-systemrandom-class-and-thread-safety
            lock (_random)
            {
                return _random.NextDouble();
            }
        }

        /// <summary>
        /// Returns the next random number.
        /// </summary>
        public abstract double NextDouble();

        /// <summary>
        /// Returns a sequence of random numbers within the specified range.
        /// </summary>
        /// <param name="count">The number of samples to generate.</param>
        public abstract IEnumerable<double> Sample(int count);
    }
}
