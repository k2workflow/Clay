#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace SourceCode.Clay
{
    /// <summary>
    /// Represents <see cref="int"/> and <see cref="double"> extensions.
    /// </summary>
    public static class NumericExtensions
    {
        /// <summary>
        /// Clamp the floor and ceiling of a values using the specified <paramref name="min"/> and <paramref name="max"/>.
        /// </summary>
        /// <param name="value">The value to clamp.</param>
        /// <param name="min">The minimum boundary.</param>
        /// <param name="max">The maximum boundary.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte Clamp(this byte value, byte min, byte max)
        {
            Debug.Assert(min < max);

            byte v = value;
            v = Math.Max(min, v); // Floor
            v = Math.Min(max, v); // Ceiling

            return v;
        }

        /// <summary>
        /// Clamp the floor and ceiling of a values using the specified <paramref name="min"/> and <paramref name="max"/>.
        /// </summary>
        /// <param name="value">The value to clamp.</param>
        /// <param name="min">The minimum boundary.</param>
        /// <param name="max">The maximum boundary.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static short Clamp(this short value, short min, short max)
        {
            Debug.Assert(min < max);

            short v = value;
            v = Math.Max(min, v); // Floor
            v = Math.Min(max, v); // Ceiling

            return v;
        }

        /// <summary>
        /// Clamp the floor and ceiling of a values using the specified <paramref name="min"/> and <paramref name="max"/>.
        /// </summary>
        /// <param name="value">The value to clamp.</param>
        /// <param name="min">The minimum boundary.</param>
        /// <param name="max">The maximum boundary.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Clamp(this int value, int min, int max)
        {
            Debug.Assert(min < max);

            int v = value;
            v = Math.Max(min, v); // Floor
            v = Math.Min(max, v); // Ceiling

            return v;
        }

        /// <summary>
        /// Clamp the floor and ceiling of a values using the specified <paramref name="min"/> and <paramref name="max"/>.
        /// </summary>
        /// <param name="value">The value to clamp.</param>
        /// <param name="min">The minimum boundary.</param>
        /// <param name="max">The maximum boundary.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long Clamp(this long value, long min, long max)
        {
            Debug.Assert(min < max);

            long v = value;
            v = Math.Max(min, v); // Floor
            v = Math.Min(max, v); // Ceiling

            return v;
        }

        /// <summary>
        /// Clamp the floor and ceiling of a values using the specified <paramref name="min"/> and <paramref name="max"/>.
        /// </summary>
        /// <param name="value">The value to clamp.</param>
        /// <param name="min">The minimum boundary.</param>
        /// <param name="max">The maximum boundary.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Clamp(this float value, float min, float max)
        {
            Debug.Assert(min < max);

            float v = value;
            v = Math.Max(min, v); // Floor
            v = Math.Min(max, v); // Ceiling

            return v;
        }

        /// <summary>
        /// Clamp the floor and ceiling of a values using the specified <paramref name="min"/> and <paramref name="max"/>.
        /// </summary>
        /// <param name="value">The value to clamp.</param>
        /// <param name="min">The minimum boundary.</param>
        /// <param name="max">The maximum boundary.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double Clamp(this double value, double min, double max)
        {
            Debug.Assert(min < max);

            double v = value;
            v = Math.Max(min, v); // Floor
            v = Math.Min(max, v); // Ceiling

            return v;
        }

        /// <summary>
        /// Clamp the floor and ceiling of a values using the specified <paramref name="min"/> and <paramref name="max"/>.
        /// </summary>
        /// <param name="value">The value to clamp.</param>
        /// <param name="min">The minimum boundary.</param>
        /// <param name="max">The maximum boundary.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static decimal Clamp(this decimal value, decimal min, decimal max)
        {
            Debug.Assert(min < max);

            decimal v = value;
            v = Math.Max(min, v); // Floor
            v = Math.Min(max, v); // Ceiling

            return v;
        }
    }
}
