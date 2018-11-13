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
    /// A random number generator with a Normal (Gaussian) distribution.
    /// Uses the Box-Muller transform to generate random numbers from a Uniform distribution.
    /// </summary>
    public sealed class NormalDistribution : RandomDistribution
    {
        private readonly double _μ;
        private readonly double _σ;
        private readonly bool _clamped;

        private readonly object _lock = new object();
        private double _chamber;
        private bool _chambered;

        private NormalDistribution(double μ, double σ, double min, double max, Random random)
            : base(min, max, random)
        {
            _μ = μ;
            _σ = σ;
            _clamped = true;
        }

        private NormalDistribution(double μ, double σ, Random random)
            : base(0, 0, random)
        {
            _μ = μ;
            _σ = σ;
            _clamped = false;
        }

        /// <summary>
        /// Creates a new instance of the class.
        /// </summary>
        /// <param name="min">The minimum of the population.</param>
        /// <param name="max">The maximum of the population.</param>
        /// <param name="random">The Random instance to use as a source.
        /// If not specified, a shared thread-safe (thread-static) instance will be used.</param>
        public static NormalDistribution FromRange(double min, double max, Random random = null)
        {
            if (min > max) throw new ArgumentOutOfRangeException(nameof(max));

            (double μ, double σ) = DeriveMuSigma(min, max);

            return new NormalDistribution(μ, σ, min, max, random);
        }

        /// <summary>
        /// Creates a new instance of the class.
        /// </summary>
        /// <param name="μ">Mu. The mean of the population.</param>
        /// <param name="σ">Sigma. The standard deviation of the population.</param>
        /// <param name="random">The Random instance to use as a source.
        /// If not specified, a shared thread-safe (thread-static) instance will be used.</param>
        public static NormalDistribution FromMuSigma(double μ, double σ, Random random = null)
        {
            return new NormalDistribution(μ, σ, random);
        }

        /// <summary>
        /// Returns the next random number.
        /// </summary>
        public override double NextDouble()
        {
            // https://stackoverflow.com/questions/25448070/getting-random-numbers-in-a-thread-safe-way/25448166#25448166
            // https://docs.microsoft.com/en-us/dotnet/api/system.random?view=netframework-4.7.2#the-systemrandom-class-and-thread-safety
            lock (_lock)
            {
                if (_chambered)
                {
                    _chambered = false;
                    return _chamber;
                }

                double value;
                (value, _chamber) = NextNormalPair();

                _chambered = true;
                return value;
            }
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

            for (int i = 0; i < count; i += 2)
            {
                (double r1, double r2) = NextNormalPair();
                yield return r1;

                if (i + 1 < count)
                    yield return r2;
            }
        }

        /// <summary>
        /// Returns a pair of random numbers.
        /// Uses the Box-Muller transform to generate random numbers from a Normal (Gaussian) distribution.
        /// </summary>
        /// <param name="random">The <see cref="Random"/> instance.</param>
        /// <param name="μ">Mu. The mean of the population.</param>
        /// <param name="σ">Sigma. The standard deviation of the population.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private (double, double) NextNormalPair()
        {
            double r1, r2, sq;

            do
            {
                r1 = 2.0 * SafeDouble() - 1.0;
                r2 = 2.0 * SafeDouble() - 1.0;
                sq = r1 * r1 + r2 * r2;
            } while (sq == 0 || sq >= 1.0);

            sq = Math.Sqrt(-2.0 * Math.Log(sq) / sq);

            r1 = r1 * sq; // Gaussian value 1
            r1 = _μ + r1 * _σ; // Stretch and move origin

            r2 = r2 * sq; // Gaussian value 2
            r2 = _μ + r2 * _σ; // Stretch and move origin

            // Clamp
            if (_clamped)
            {
                r1 = Clamp(r1);
                r2 = Clamp(r2);
            }

            return (r1, r2);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private double Clamp(double value)
        {
            value = Math.Max(Min, value); // Floor
            value = Math.Min(Max, value); // Ceiling

            return value;
        }

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
