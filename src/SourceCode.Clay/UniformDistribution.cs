#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace SourceCode.Clay
{
    /// <summary>
    /// A random number generator with a Uniform distribution.
    /// </summary>
    public sealed class UniformDistribution : RandomDistribution
    {
        private UniformDistribution(ClampInfo clamp, Random random)
            : base(clamp, random)
        {
            Debug.Assert(clamp != null);
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
            if (double.IsInfinity(max - min)) throw new ArgumentOutOfRangeException(nameof(max));

            return new UniformDistribution(new ClampInfo(min, max), random);
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
            if (double.IsInfinity(max - min)) throw new ArgumentOutOfRangeException(nameof(max));

            return new UniformDistribution(new ClampInfo(min, max), random);
        }

        /// <summary>
        /// Returns the next random number.
        /// </summary>
        public override double NextDouble()
        {
            return Clamp.Min + Clamp.Range * SafeDouble();
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

        /// <summary>
        /// Derives the min and max, given the mean and standard deviation.
        /// </summary>
        /// <param name="μ">Mu. The mean of the population.</param>
        /// <param name="σ">Sigma. The standard deviation of the population.</param>
        private static (double min, double max) DeriveMinMax(double μ, double σ)
        {
            // https://www.quora.com/What-is-the-standard-deviation-of-a-uniform-distribution-How-is-this-formula-determined
            //
            // From the article:
            // b+a = 2μ;
            // b-a = σ√12;
            //
            // Subtracting: b+a - b-a = 2a == 2μ - σ√12. Thus a = μ - σ√12/2
            // Adding:      b+a + b-a = 2b == 2μ + σ√12. Thus b = μ + σ√12/2

            const double sqrt12_2 = 3.4641016151377544 / 2d; // √12/2
            double σ12_2 = σ * sqrt12_2; // σ√12/2
            double a = μ - σ12_2; // μ - σ√12/2
            double b = μ + σ12_2; // μ + σ√12/2

            return (a, b);
        }
    }
}
