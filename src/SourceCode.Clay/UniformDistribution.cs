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

        /// <summary>
        /// Creates a new instance of the class.
        /// </summary>
        /// <param name="min">The minimum of the population.</param>
        /// <param name="max">The maximum of the population.</param>
        /// <param name="random">The Random instance to use as a source.
        /// If not specified, a shared thread-safe (thread-static) instance will be used.</param>
        public UniformDistribution(double min, double max, Random random = null)
            : base(min, max, random)
        {
            _range = max - min;
        }

        /// <summary>
        /// Creates a new instance of the class.
        /// </summary>
        /// <param name="random">The Random instance to use as a source.
        /// If not specified, a shared thread-safe (thread-static) instance will be used.</param>
        public UniformDistribution(Random random = null)
            : this(0, 1, random)
        { }

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
    }
}
