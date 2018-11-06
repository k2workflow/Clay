#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace SourceCode.Clay
{
    /// <summary>
    /// Represents <see cref="Random"/> extensions.
    /// </summary>
    public static class RandomExtensions
    {
        /// <summary>
        /// Returns a sequence of random numbers.
        /// Uses the Box-Muller transform to generate random numbers from a Normal (Guassian) distribution.
        /// </summary>
        /// <param name="random">The <see cref="Random"/> instance.</param>
        /// <param name="μ">Mu. The mean of the population.</param>
        /// <param name="σ">Sigma. The standard deviation of the population.</param>
        public static IEnumerable<double> GetNormalSample(this Random random, int count, double μ, double σ)
        {
            if (random == null) throw new ArgumentNullException(nameof(random));
            if (count < 0) throw new ArgumentOutOfRangeException(nameof(count));

            if (count == 0) yield break;

            for (int i = 0; i < count; i += 2)
            {
                (double r1, double r2) = NextNormalPair(random, μ, σ);

                yield return r1;

                if (i + 1 < count)
                    yield return r2;
            }
        }

        /// <summary>
        /// Fills the elements of a specified array of doubles with random numbers.
        /// Uses the Box-Muller transform to generate random numbers from a Normal (Guassian) distribution.
        /// The floor and ceiling of the resulting values are clamped using the specified <paramref name="min"/> and <paramref name="max"/>.
        /// </summary>
        /// <param name="random">The <see cref="Random"/> instance.</param>
        /// <param name="min">The minimum of the population.</param>
        /// <param name="max">The maximum of the population.</param>
        public static IEnumerable<double> ClampedNormalSample(this Random random, int count, double min, double max)
        {
            if (random == null) throw new ArgumentNullException(nameof(random));
            if (min > max) throw new ArgumentOutOfRangeException(nameof(max));
            if (count < 0) throw new ArgumentOutOfRangeException(nameof(count));

            if (count == 0) yield break;

            (double μ, double σ) = DeriveMuSigma(min, max);

            for (int i = 0; i < count; i += 2)
            {
                (double r1, double r2) = NextNormalPair(random, μ, σ);

                r1 = r1.Clamp(min, max);
                yield return r1;

                if (i + 1 < count)
                {
                    r2 = r2.Clamp(min, max);
                    yield return r2;
                }
            }
        }

        /// <summary>
        /// Returns a pair of random numbers.
        /// Uses the Box-Muller transform to generate random numbers from a Normal (Guassian) distribution.
        /// </summary>
        /// <param name="random">The <see cref="Random"/> instance.</param>
        /// <param name="μ">Mu. The mean of the population.</param>
        /// <param name="σ">Sigma. The standard deviation of the population.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static (double, double) NextNormalPair(this Random random, double μ, double σ)
        {
            if (random == null) throw new ArgumentNullException(nameof(random));

            double r1, r2, sq;
            do
            {
                r1 = 2.0 * random.NextDouble() - 1.0;
                r2 = 2.0 * random.NextDouble() - 1.0;
                sq = r1 * r1 + r2 * r2;
            } while (sq == 0 || sq >= 1.0);

            sq = Math.Sqrt(-2.0 * Math.Log(sq) / sq);

            r1 = r1 * sq; // Guassian value 1
            r1 = μ + r1 * σ; // Stretch and move origin

            r2 = r2 * sq; // Guassian value 2
            r2 = μ + r2 * σ; // Stretch and move origin

            return (r1, r2);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static (double μ, double σ) DeriveMuSigma(double min, double max)
        {
            Debug.Assert(min <= max);

            double range = max - min;
            double half = range / 2.0;

            double μ = min + half; // Mu = min + half-range

            // ~99.7% of population is within +/- 3 standard deviations
            const double sd = 3.0;
            double σ = half / sd; // Sigma = half-range / 3 == range / 6

            return (μ, σ);
        }
    }
}
