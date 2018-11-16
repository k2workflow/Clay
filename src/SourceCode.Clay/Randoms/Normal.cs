#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace SourceCode.Clay.Randoms
{
    /// <summary>
    /// A random number generator with a Normal (Gaussian) distribution that is thread-safe.
    /// Can be instantiated with a custom <see cref="Uniform"/> instance, for example to make
    /// it behave in a deterministic manner.
    /// Uses the Box-Muller transform to generate random numbers from a Uniform distribution.
    /// </summary>
    public sealed class Normal : IRandom
    {
        /// <summary>
        /// A shared instance of <see cref="Normal"/>, in the range [0, 1), that is safe to use concurrently.
        /// </summary>
        public static Normal Shared { get; } = new Normal();

        private readonly Uniform _uniform;
        private readonly Clamp _clamp;
        private readonly double _μ;
        private readonly double _σ;

        private readonly object _lock = new object();
        private double _chamber;
        private bool _chambered;

        private Normal(double μ, double σ, Clamp clamp, Uniform uniform)
        {
            _uniform = uniform ?? Uniform.Shared;
            _clamp = clamp ?? Clamp.Default;

            _μ = μ;
            _σ = σ;
        }

        private Normal(Uniform uniform, Clamp clamp)
        {
            _uniform = uniform ?? Uniform.Shared;
            _clamp = clamp ?? Clamp.Default;

            (_μ, _σ) = DeriveMuSigma(0, 1);
        }

        /// <summary>
        /// Creates an instance of the class that generates numbers in the range [0, 1).
        /// </summary>
        /// <param name="uniform">The <see cref="Uniform"/> instance to use as the Box-Muller source.
        /// If not specified, a shared thread-safe, randomly-seeded instance will be used.</param>
        public Normal(Uniform uniform = null)
            : this(uniform, null)
        { }

        /// <summary>
        /// Creates an instance of the class that generates numbers in the range [0, 1).
        /// </summary>
        /// <param name="seed">The seed to initialize the random number generator with.</param>
        public Normal(int seed)
            : this(new Uniform(seed), null)
        { }

        /// <summary>
        /// Creates a new instance of the class that generates numbers in the specified range.
        /// </summary>
        /// <param name="min">The minimum of the population.</param>
        /// <param name="max">The maximum of the population.</param>
        /// <param name="uniform">The Random instance to use as the Box-Muller source.
        /// If not specified, a shared thread-safe randomly-seeded instance will be used.</param>
        public static Normal FromRange(double min, double max, Uniform uniform = null)
        {
            if (min > max) throw new ArgumentOutOfRangeException(nameof(max));
            if (double.IsInfinity(max - min)) throw new ArgumentOutOfRangeException(nameof(max));

            (double μ, double σ) = DeriveMuSigma(min, max);

            return new Normal(μ, σ, new Clamp(min, max), uniform);
        }

        /// <summary>
        /// Creates a new instance of the class that generates numbers in the specified range.
        /// </summary>
        /// <param name="μ">Mu. The mean of the population.</param>
        /// <param name="σ">Sigma. The standard deviation of the population.</param>
        /// <param name="uniform">The Random instance to use as the Box-Muller source.
        /// If not specified, a shared thread-safe randomly-seeded instance will be used.</param>
        public static Normal FromMuSigma(double μ, double σ, Uniform uniform = null)
        {
            return new Normal(μ, σ, null, uniform);
        }

        /// <summary>
        /// Returns the next random number within the specified range.
        /// </summary>
        public double NextDouble()
        {
            // https://stackoverflow.com/questions/25448070/getting-random-numbers-in-a-thread-safe-way/25448166#25448166
            // https://docs.microsoft.com/en-us/dotnet/api/system.random?view=netframework-4.7.2#the-systemrandom-class-and-thread-safety
            // No need to lock on _uniform since it is already thread-safe, and doing so
            // might cause undue contention should it be mapped to the shared instance.
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
        public IEnumerable<double> Sample(int count)
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
        /// <param name="random">The <see cref="_uniform"/> instance.</param>
        /// <param name="μ">Mu. The mean of the population.</param>
        /// <param name="σ">Sigma. The standard deviation of the population.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private (double, double) NextNormalPair()
        {
            double r1, r2, sq;

            do
            {
                r1 = 2.0 * _uniform.NextDouble() - 1.0;
                r2 = 2.0 * _uniform.NextDouble() - 1.0;
                sq = r1 * r1 + r2 * r2;
            } while (sq == 0 || sq >= 1.0);

            sq = Math.Sqrt(-2.0 * Math.Log(sq) / sq);

            r1 = r1 * sq; // Gaussian value 1
            r1 = _μ + r1 * _σ; // Stretch and move origin

            r2 = r2 * sq; // Gaussian value 2
            r2 = _μ + r2 * _σ; // Stretch and move origin

            // Clamp
            if (_clamp != null)
            {
                r1 = _clamp.Constrain(r1);
                r2 = _clamp.Constrain(r2);
            }

            return (r1, r2);
        }

        /// <summary>
        /// Derives the mean and standard deviation, given the min and max.
        /// </summary>
        /// <param name="min">The minimum of the population.</param>
        /// <param name="max">The maximum of the population.</param>
        private static (double μ, double σ) DeriveMuSigma(double min, double max)
        {
            Debug.Assert(min <= max);

            double range = max - min;
            double half = range / 2d;

            double μ = min + half; // Mu = min + half-range

            // ~99.7% of population is within +/- 3 standard deviations
            const double sd = 3d;
            double σ = half / sd; // Sigma = half-range / 3 == range / 6

            return (μ, σ);
        }
    }
}
