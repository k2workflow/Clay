#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using System;
using System.Security.Cryptography;
using System.Threading;

namespace SourceCode.Security.Clay
{
    /// <summary>
    /// Represents a thread-safe <see cref="Random"/> implementation using a crypto seed.
    /// </summary>
    public sealed class SafeRandom : Random
    {
        private static readonly RandomNumberGenerator s_seedSource = RandomNumberGenerator.Create();

        private static readonly ThreadLocal<Random> t_local = new ThreadLocal<Random>(() =>
        {
            var buffer = new byte[4];
            s_seedSource.GetBytes(buffer);
            var seed = BitConverter.ToInt32(buffer, 0);

            var rand = new Random(seed);
            return rand;
        });

        /// <summary>
        /// Returns a non-negative random integer.
        /// </summary>
        /// <returns>
        /// A 32-bit signed integer that is greater than or equal to 0 and less than <see cref="System.Int32.MaxValue" />.
        /// </returns>
        public override int Next() => t_local.Value.Next();

        /// <summary>
        /// Returns a non-negative random integer that is less than the specified maximum.
        /// </summary>
        /// <param name="maxValue">The exclusive upper bound of the random number to be generated. <paramref name="maxValue" /> must be greater than or equal to 0.</param>
        /// <returns>
        /// A 32-bit signed integer that is greater than or equal to 0, and less than <paramref name="maxValue" />; that is, the range of return values ordinarily includes 0 but not <paramref name="maxValue" />. However, if <paramref name="maxValue" /> equals 0, <paramref name="maxValue" /> is returned.
        /// </returns>
        public override int Next(int maxValue) => t_local.Value.Next(maxValue);

        /// <summary>
        /// Returns a random integer that is within a specified range.
        /// </summary>
        /// <param name="minValue">The inclusive lower bound of the random number returned.</param>
        /// <param name="maxValue">The exclusive upper bound of the random number returned. <paramref name="maxValue" /> must be greater than or equal to <paramref name="minValue" />.</param>
        /// <returns>
        /// A 32-bit signed integer greater than or equal to <paramref name="minValue" /> and less than <paramref name="maxValue" />; that is, the range of return values includes <paramref name="minValue" /> but not <paramref name="maxValue" />. If <paramref name="minValue" /> equals <paramref name="maxValue" />, <paramref name="minValue" /> is returned.
        /// </returns>
        public override int Next(int minValue, int maxValue) => t_local.Value.Next(minValue, maxValue);

        /// <summary>
        /// Fills the elements of a specified array of bytes with random numbers.
        /// </summary>
        /// <param name="buffer">An array of bytes to contain random numbers.</param>
        public override void NextBytes(byte[] buffer) => t_local.Value.NextBytes(buffer);

        /// <summary>
        /// Returns a random floating-point number that is greater than or equal to 0.0, and less than 1.0.
        /// </summary>
        /// <returns>
        /// A double-precision floating point number that is greater than or equal to 0.0, and less than 1.0.
        /// </returns>
        public override double NextDouble() => t_local.Value.NextDouble();
    }
}
