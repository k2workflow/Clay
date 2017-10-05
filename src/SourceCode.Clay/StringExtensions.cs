#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using System;
using System.Runtime.CompilerServices;

namespace SourceCode.Clay
{
    /// <summary>
    /// Utility functions for <see cref="System.String"/>.
    /// </summary>
    public static class StringExtensions
    {
        #region Methods

        /// <summary>
        /// Returns the specified number of characters from the left of a string.
        /// Tolerates <paramref name="length"/> values that are too large or too small (or negative).
        /// </summary>
        /// <param name="str">The string.</param>
        /// <param name="length">The length.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string Left(this string str, int length)
        {
            var len = length <= 0 ? 0 : length;

            if (str == null || str.Length <= len) return str;

            // Per Substring behavior, we don't respect surrogate pairs
            return str.Substring(0, len);
        }

        /// <summary>
        /// Returns the specified number of characters from the right of a string.
        /// Tolerates <paramref name="length"/> values that are too large or too small (or negative).
        /// </summary>
        /// <param name="str">The string.</param>
        /// <param name="length">The length.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string Right(this string str, int length)
        {
            var len = length <= 0 ? 0 : length;

            if (str == null || str.Length <= len) return str;

            // Per Substring behavior, we don't respect surrogate pairs
            return str.Substring(str.Length - len, len);
        }

        /// <summary>
        /// Truncates a string to a specified width, respecting surrogate pairs and inserting
        /// an ellipsis '…' in the final position.
        /// Tolerates width values that are too large or too small (or negative).
        /// If the value is already smaller than the specified width, the original value is returned.
        /// Note that if the target character is in a surrogate pair then the pair is treated atomically.
        /// In this case the result may be shorter than specified.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <param name="totalWidth">The total width.</param>
        /// <returns>The elided string.</returns>
        public static string Elide(this string str, int totalWidth)
        {
            if (str == null || totalWidth <= 2 || str.Length <= totalWidth) return str;

            // Since Elide is expected to be used primarily for display purposes, it needs to respect
            // surrogate pairs and not blindly split them in half.
            // https://stackoverflow.com/questions/2241348/what-is-unicode-utf-8-utf-16
            // https://stackoverflow.com/questions/14347799/how-do-i-create-a-string-with-a-surrogate-pair-inside-of-it

            var ca = str.ToCharArray();

            // Expect non-surrogates by default
            var n = 0;

            // High|Low (default on x86/x64)
            if (BitConverter.IsLittleEndian)
            {
                // If target is the LOW surrogate, replace both (returning a shorter-by-1-than-expected result)
                if (char.IsLowSurrogate(ca[totalWidth - 1]))
                    n = 1;

                // It it's the HIGH surrogate, we're replacing it regardless
            }
            // Low|High
            else
            {
                // If target is the HIGH surrogate, replace both (returning a shorter-by-1-than-expected result)
                if (char.IsHighSurrogate(ca[totalWidth - 1]))
                    n = 1;

                // It it's the LOW surrogate, we're replacing it regardless
            }

            ca[totalWidth - n - 1] = '…';
            return new string(ca, 0, totalWidth - n);
        }

        /// <summary>
        /// Compares two strings using ordinal rules, with checks for null and reference equality.
        /// A partner for the framework-provided <see cref="string.CompareOrdinal(string, string)"/> method.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool EqualsOrdinal(this string x, string y)
        {
            if (x == y) return true; // (null, null) or (s, s)
            if (x == null || y == null) return false; // (s, null) or (null, t)

            return x.Equals(y); // (s, t)
        }

        #endregion
    }
}
