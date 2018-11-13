#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using System;
using System.Collections.Generic;

namespace SourceCode.Clay
{
    /// <summary>
    /// A random number generator with a Uniform distribution.
    /// </summary>
    public sealed class UniformDistribution : RandomDistribution
    {
        private readonly double _range;

        private UniformDistribution(double min, double max, Random random = null)
            : base(min, max, random)
        {
            _range = max - min;
        }

        /// <summary>
        /// Creates a new instance of the class.
        /// </summary>
        /// <param name="min">The minimum of the population.</param>
        /// <param name="max">The maximum of the population.</param>
        /// <param name="random">The Random instance to use as a source.
        /// If not specified, a shared thread-safe (thread-static) instance will be used.</param>
        public static UniformDistribution FromRange(double min, double max, Random random = null)
        {
            if (min > max) throw new ArgumentOutOfRangeException(nameof(max));

            return new UniformDistribution(min, max, random);
        }

        /// <summary>
        /// Creates a new instance of the class.
        /// </summary>
        /// <param name="μ">Mu. The mean of the population.</param>
        /// <param name="σ">Sigma. The standard deviation of the population.</param>
        /// <param name="random">The Random instance to use as a source.
        /// If not specified, a shared thread-safe (thread-static) instance will be used.</param>
        public static UniformDistribution FromMuSigma(double μ, double σ, Random random = null)
        {
            (double min, double max) = DeriveMinMax(μ, σ);

            return new UniformDistribution(min, max, random);
        }

        /// <summary>
        /// Returns the next random number.
        /// </summary>
        public override double NextDouble()
        {
            return Min + _range * SafeDouble();
        }

        /// <summary>
        /// Returns a sequence of random numbers within the specified range.
        /// </summary>
        /// <param name="count">The number of samples to generate.</param>
        public override IEnumerable<double> Sample(int count)
        {
            if (count < 0) throw new ArgumentOutOfRangeException(nameof(count));
            if (count == 0)
                yield break;

            for (int i = 0; i < count; i++)
            {
                yield return NextDouble();
            }
        }

        private static readonly double s_sqrt12 = Math.Sqrt(12.0);

        private static (double min, double max) DeriveMinMax(double μ, double σ)
        {
            // https://www.quora.com/What-is-the-standard-deviation-of-a-uniform-distribution-How-is-this-formula-determined

            double bpa = μ * 2.0; // b+a
            double bma = σ * s_sqrt12; // b-a

            double b = (bma + bpa) / 2.0; // (b+a + b-a) / 2 = 2b/2 = b
            double a = bpa - b; // b+a - b = a

            return (a, b);
        }
    }
}
