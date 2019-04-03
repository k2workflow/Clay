#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace SourceCode.Clay.Algorithms.Random
{
    /// <summary>
    /// A random number generator with a Uniform distribution that is thread-safe.
    /// Can be instantiated with a custom seed to make it behave in a deterministic manner.
    /// </summary>
    public sealed class Uniform : IRandom
    {
        /// <summary>
        /// A shared instance of <see cref="Uniform"/>, in the range [0, 1), that is safe to use concurrently.
        /// </summary>
        public static Uniform Shared { get; } = new Uniform();

        private static readonly System.Random s_random = new System.Random();
        private readonly System.Random _random; // MUST be accessed within a lock

        private readonly Func<double> _nextDouble;

        /// <summary>
        /// Creates an instance of the class that generates numbers in the range [0, 1).
        /// </summary>
        /// <param name="seed">The seed to initialize the random number generator with.</param>
        public Uniform(int seed)
        {
            _random = new System.Random(seed);
            _nextDouble = NextDoubleLocked; // Curry [0, 1)
        }

        /// <summary>
        /// Creates an instance of the class that generates numbers in the range [0, 1).
        /// </summary>
        public Uniform()
        {
            _random = s_random;
            _nextDouble = NextDoubleLocked; // Curry [0, 1)
        }

        // Used by factory methods only
        private Uniform(double min, double range, int? seed)
        {
            Debug.Assert(!double.IsInfinity(min + range));

            _random = seed == null
                ? s_random // Do not use 'new Random()' here; concurrent instantiations may get the same seed
                : new System.Random(seed.Value);

            _nextDouble = () => min + range * NextDoubleLocked(); // Curry [min, max)
        }

        /// <summary>
        /// Creates a new instance of the class that generates numbers in the specified range.
        /// </summary>
        /// <param name="min">The minimum of the population.</param>
        /// <param name="range">The range of values in the population.</param>
        /// <param name="seed">The seed to initialize the random number generator with.
        /// If not specified, a randomly-seeded instance will be used.</param>
        public static Uniform FromRange(double min, double range, int? seed = null)
        {
            if (double.IsInfinity(min + range)) throw new ArgumentOutOfRangeException(nameof(range));

            return new Uniform(min, range, seed);
        }

        /// <summary>
        /// Creates a new instance of the class that generates numbers in the specified range.
        /// </summary>
        /// <param name="min">The minimum of the population.</param>
        /// <param name="max">The maximum of the population.</param>
        /// <param name="seed">The seed to initialize the random number generator with.
        /// If not specified, a randomly-seeded instance will be used.</param>
        public static Uniform Between(double min, double max, int? seed = null)
            => FromRange(min, max - min, seed);

        /// <summary>
        /// Creates a new instance of the class that generates numbers in the specified range.
        /// </summary>
        /// <param name="μ">Mu. The mean of the population.</param>
        /// <param name="σ">Sigma. The standard deviation of the population.</param>
        /// <param name="seed">The seed to initialize the random number generator with.
        /// If not specified, a randomly-seeded instance will be used.</param>
        public static Uniform FromMuSigma(double μ, double σ, int? seed = null)
        {
            (double min, double range) = DeriveMinRange(μ, σ);

            return new Uniform(min, range, seed);
        }

        /// <summary>
        /// Returns the next random number within the specified range.
        /// </summary>
        public double NextDouble()
            => _nextDouble(); // Curried

        /// <summary>
        /// Returns a random floating-point number that is greater than or equal to 0.0,
        /// and less than 1.0.
        /// This method uses locks in order to avoid issues with concurrent access on <see cref="Random"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private double NextDoubleLocked()
        {
            // https://stackoverflow.com/questions/25448070/getting-random-numbers-in-a-thread-safe-way/25448166#25448166
            // https://docs.microsoft.com/en-us/dotnet/api/system.random?view=netframework-4.7.2#the-systemrandom-class-and-thread-safety
            // It is safe to lock on _random since it is not exposed to outside use, so cannot be contended.
            lock (_random)
            {
                return _random.NextDouble();
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
                for (int i = 0; i < cnt; i++)
                {
                    yield return NextDouble();
                }
            }
        }

        /// <summary>
        /// Derives the min and max, given the mean and standard deviation.
        /// </summary>
        /// <param name="μ">Mu. The mean of the population.</param>
        /// <param name="σ">Sigma. The standard deviation of the population.</param>
        private static (double min, double range) DeriveMinRange(double μ, double σ)
        {
            // https://www.quora.com/What-is-the-standard-deviation-of-a-uniform-distribution-How-is-this-formula-determined
            //
            // From the article:
            // max+min = 2μ
            // max-min = σ√12 (range)
            //
            // Subtracting: max+min - max-min = 2min == 2μ - σ√12. Thus min = μ - σ√12/2
            // Adding:      max+min + max-min = 2max == 2μ + σ√12. Thus max = μ + σ√12/2

            const double sqrt12 = 3.4641016151377544; // √12
            double σ12 = σ * sqrt12; // σ√12
            double min = μ - σ12 / 2; // μ - σ√12/2

            return (min, σ12);
        }
    }
}
