using System;

namespace SourceCode.Clay
{
    /// <summary>
    /// Represents <see cref="DateTime"/> extensions.
    /// </summary>
    public static class DateTimeExtensions
    {
        /// <summary>
        /// Gets the Unix epoch as a <see cref="DateTime"/>.
        /// </summary>
        public static DateTime UnixEpoch { get; } = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        private static readonly long _difference = UnixEpoch.ToFileTimeUtc();

        /// <summary>
        /// Converts the specified <see cref="DateTime"/> to a Posix timestamp.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>The Posix timestamp.</returns>
        public static long ToPosixFileTime(this DateTime value) => value.ToFileTime() - _difference;

        /// <summary>
        /// Converts the specified <see cref="DateTime"/> to a UTC Posix timestamp.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>The UTC Posix timestamp.</returns>
        public static long ToPosixFileTimeUtc(this DateTime value) => value.ToFileTimeUtc() - _difference;

        /// <summary>
        /// Converts the specified Posix timestamp to a <see cref="DateTime"/>.
        /// </summary>
        /// <param name="posix">The Posix timestamp to convert.</param>
        /// <returns>The <see cref="DateTime"/>.</returns>
        public static DateTime FromPosixFileTime(long posix) => DateTime.FromFileTime(posix + _difference);

        /// <summary>
        /// Converts the specified Posix timestamp to a <see cref="DateTime"/>.
        /// </summary>
        /// <param name="posix">The Posix timestamp to convert.</param>
        /// <returns>The <see cref="DateTime"/>.</returns>
        public static DateTime FromPosixFileTimeUtc(long posix) => DateTime.FromFileTimeUtc(posix + _difference);
    }
}
