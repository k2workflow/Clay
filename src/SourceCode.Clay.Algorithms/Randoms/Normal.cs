#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace SourceCode.Clay.Algorithms.Randoms
{
    /// <summary>
    /// A random number generator with a Normal (Gaussian) distribution that is thread-safe.
    /// Can be instantiated with a custom seed to make it behave in a deterministic manner.
    /// Uses the Box-Muller transform to generate random numbers from a Uniform distribution.
    /// </summary>
    public sealed class Normal : IRandom
    {
        /// <summary>
        /// A shared instance of <see cref="Normal"/>, in the range [0, 1), that is safe to use concurrently.
        /// </summary>
        public static Normal Shared { get; } = new Normal();

        private static readonly Random s_random = new Random();
        private readonly Random _random; // MUST be accessed within a lock

        private readonly double _μ;
        private readonly double _σ;
        private readonly Func<double, double> _clamp;

        private readonly object _lock = new object();
        private double _chamber;
        private bool _chambered;

        /// <summary>
        /// Creates an instance of the class that generates numbers in the range [0, 1).
        /// </summary>
        /// <param name="seed">The seed to initialize the random number generator with.</param>
        public Normal(int seed)
        {
            _random = new Random(seed);
            _clamp = d => Clamp(0, 1, d); // Curry [0, 1)

            (_μ, _σ) = DeriveMuSigma(0, 1);
        }

        /// <summary>
        /// Creates an instance of the class that generates numbers in the range [0, 1).
        /// </summary>
        public Normal()
        {
            _random = s_random; // Do not use 'new Random()' here; concurrent instantiations may get the same seed
            _clamp = d => Clamp(0, 1, d); // Curry [0, 1)

            (_μ, _σ) = DeriveMuSigma(0, 1);
        }

        // Used by factory methods only
        private Normal(double μ, double σ, (double min, double range)? clamp, int? seed)
        {
            _random = seed == null
                ? s_random // Do not use 'new Random()' here; concurrent instantiations may get the same seed
                : new Random(seed.Value);

            _clamp = d => d; // Curry (-∞, ∞)
            if (clamp != null)
            {
                (double min, double range) = clamp.Value;

                _clamp = d => min; // Curry [min, min]
                if (range != 0)
                {
                    double max = min + range;
                    Debug.Assert(!double.IsInfinity(max));

                    _clamp = d => Clamp(min, max, d); // Curry [min, max)
                }
            }

            _μ = μ;
            _σ = σ;
        }

        /// <summary>
        /// Creates a new instance of the class that generates numbers in the specified range.
        /// </summary>
        /// <param name="min">The minimum of the population.</param>
        /// <param name="range">The range of values in the population.</param>
        /// <param name="seed">The seed to initialize the random number generator with.
        /// If not specified, a randomly-seeded instance will be used.</param>
        public static Normal FromRange(double min, double range, int? seed = null)
        {
            if (double.IsInfinity(min + range)) throw new ArgumentOutOfRangeException(nameof(range));

            (double μ, double σ) = DeriveMuSigma(min, range);

            return new Normal(μ, σ, (min, range), seed);
        }

        /// <summary>
        /// Creates a new instance of the class that generates numbers in the specified range.
        /// </summary>
        /// <param name="min">The minimum of the population.</param>
        /// <param name="max">The maximum of the population.</param>
        /// <param name="seed">The seed to initialize the random number generator with.
        /// If not specified, a randomly-seeded instance will be used.</param>
        public static Normal Between(double min, double max, int? seed = null)
            => FromRange(min, max - min, seed);

        /// <summary>
        /// Creates a new instance of the class that generates numbers in the specified range.
        /// </summary>
        /// <param name="μ">Mu. The mean of the population.</param>
        /// <param name="σ">Sigma. The standard deviation of the population.</param>
        /// <param name="seed">The seed to initialize the random number generator with.
        /// If not specified, a randomly-seeded instance will be used.</param>
        public static Normal FromMuSigma(double μ, double σ, int? seed = null)
            => new Normal(μ, σ, null, seed);

        /// <summary>
        /// Returns the next random number within the specified range.
        /// </summary>
        public double NextDouble()
        {
            // Do NOT lock on _random since this lock is on a different level
            lock (_lock)
            {
                if (_chambered)
                {
                    _chambered = false;
                    return _chamber; // Would have needlessly locked on _random
                }

                double value;
                (value, _chamber) = NextNormalPair(); // Internally locks on _random

                _chambered = true;
                return value;
            }
        }

        /// <summary>
        /// Returns a sequence of random numbers within the specified range.
        /// </summary>
        /// <param name="count">The number of samples to generate.</param>
        public IEnumerable<double> Sample(int count)
        {
            if (count < 0) throw new ArgumentOutOfRangeException(nameof(count));

            if (count == 0)
                return Array.Empty<double>();

            return Enumerate(count);

            IEnumerable<double> Enumerate(int cnt)
            {
                for (int i = 0; i < cnt; i += 2)
                {
                    (double r1, double r2) = NextNormalPair();
                    yield return r1;

                    if (i + 1 < cnt)
                        yield return r2;
                }
            }
        }

        /// <summary>
        /// Returns a pair of random numbers with a Normal distribution.
        /// Uses the Box-Muller transform to generate random numbers from a Uniform distribution.
        /// </summary>
        /// <param name="random">The <see cref="_random"/> instance.</param>
        /// <param name="μ">Mu. The mean of the population.</param>
        /// <param name="σ">Sigma. The standard deviation of the population.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private (double, double) NextNormalPair()
        {
            double r1, r2, sq;

            do
            {
                (r1, r2) = NextDoublePairLocked(); // Uniform value 1 & 2

                r1 = 2.0 * r1 - 1.0;
                r2 = 2.0 * r2 - 1.0;
                sq = r1 * r1 + r2 * r2;
            } while (sq == 0 || sq >= 1.0);

            sq = Math.Sqrt(-2.0 * Math.Log(sq) / sq);

            r1 = r1 * sq; // Gaussian value 1
            r1 = _μ + r1 * _σ; // Stretch and move origin

            r2 = r2 * sq; // Gaussian value 2
            r2 = _μ + r2 * _σ; // Stretch and move origin

            // Curried
            r1 = _clamp(r1);
            r2 = _clamp(r2);

            return (r1, r2);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static double Clamp(double min, double max, double value)
        {
            Debug.Assert(min <= max);

            value = Math.Max(min, value); // Floor
            value = Math.Min(max, value); // Ceiling

            return value;
        }

        /// <summary>
        /// Returns a pair of random floating-point numbers that are greater than or equal to 0.0,
        /// and less than 1.0.
        /// This method uses locks in order to avoid issues with concurrent access on <see cref="Random"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private (double, double) NextDoublePairLocked()
        {
            // https://stackoverflow.com/questions/25448070/getting-random-numbers-in-a-thread-safe-way/25448166#25448166
            // https://docs.microsoft.com/en-us/dotnet/api/system.random?view=netframework-4.7.2#the-systemrandom-class-and-thread-safety
            // It is safe to lock on _random since it is not exposed to outside use, so cannot be contended.
            lock (_random)
            {
                double r1 = _random.NextDouble();
                double r2 = _random.NextDouble();

                return (r1, r2);
            }
        }

        /// <summary>
        /// Derives the mean and standard deviation, given the min and max.
        /// </summary>
        /// <param name="clamp">The minimum and maximum of the population.</param>
        /// </param>
        private static (double μ, double σ) DeriveMuSigma(double min, double range)
        {
            Debug.Assert(!double.IsInfinity(min + range));

            // Note that ~99.7% of population is within +/- 3 standard deviations
            const double sd = 3;

            double h2 = range / 2;
            double μ = min + h2; // Mu = min + half-range
            double σ = h2 / sd; // Sigma = half-range / 3 (same as range / 6)

            return (μ, σ);
        }
    }
}
