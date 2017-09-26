using System.Runtime.CompilerServices;

namespace SourceCode.Clay
{
    /// <summary>
    /// Utility functions for <see cref="System.String"/>.
    /// </summary>
    public static class StringExtensions
    {
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
            return str.Substring(str.Length - len, len);
        }

        /// <summary>
        /// Truncates a string, inserting an ellipsis if necessary, returning a value with the specified total width.
        /// Tolerates <paramref name="totalWidth"/> values that are too large or too small (or negative).
        /// If the value is already smaller then <paramref name="totalWidth"/>, the original value is returned.
        /// Otherwise the value is truncated to the specified <paramref name="totalWidth"/> and the
        /// final 3 characters are replaced with "...".
        /// </summary>
        /// <param name="str">The string.</param>
        /// <param name="totalWidth">The total width.</param>
        /// <returns>The elided string.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string Elide(this string str, int totalWidth)
        {
            if (str == null || totalWidth <= 4 || str.Length <= totalWidth) return str;

            // str.Substring(0, width) + "..." incurs multiple allocations
            // new StringBuilder(str, width) allocates char[width] regardless
            var ca = str.ToCharArray(0, totalWidth);
            ca[totalWidth - 3] = '.'; // Actual ellipsis character = ALT+0133
            ca[totalWidth - 2] = '.';
            ca[totalWidth - 1] = '.';

            return new string(ca);
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
    }
}
