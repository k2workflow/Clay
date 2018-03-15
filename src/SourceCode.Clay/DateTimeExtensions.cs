#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using System;

namespace SourceCode.Clay
{
    /// <summary>
    /// Represents <see cref="DateTime"/> extensions.
    /// </summary>
    public static class DateTimeExtensions
    {
        #region Constants

        // TODO: Remove these once this ships: https://github.com/dotnet/corefx/issues/24449

        /// <summary>
        /// Gets the Unix epoch as a <see cref="DateTime"/>.
        /// </summary>
        public static DateTime UnixEpoch { get; } = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        /// <summary>
        /// Gets the difference between the Unix epoch and the Windows epoch.
        /// </summary>
        public static long UnixEpochDifference { get; } = UnixEpoch.ToFileTimeUtc();

        #endregion

        #region Posix

        /// <summary>
        /// Converts the specified <see cref="DateTime"/> to a Posix timestamp.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>The Posix timestamp.</returns>
        public static long ToPosixFileTime(this DateTime value) => value.ToFileTime() - UnixEpochDifference;

        /// <summary>
        /// Converts the specified <see cref="DateTime"/> to a UTC Posix timestamp.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>The UTC Posix timestamp.</returns>
        public static long ToPosixFileTimeUtc(this DateTime value) => value.ToFileTimeUtc() - UnixEpochDifference;

        /// <summary>
        /// Converts the specified Posix timestamp to a <see cref="DateTime"/>.
        /// </summary>
        /// <param name="posix">The Posix timestamp to convert.</param>
        /// <returns>The <see cref="DateTime"/>.</returns>
        public static DateTime FromPosixFileTime(long posix) => DateTime.FromFileTime(posix + UnixEpochDifference);

        /// <summary>
        /// Converts the specified Posix timestamp to a <see cref="DateTime"/>.
        /// </summary>
        /// <param name="posix">The Posix timestamp to convert.</param>
        /// <returns>The <see cref="DateTime"/>.</returns>
        public static DateTime FromPosixFileTimeUtc(long posix) => DateTime.FromFileTimeUtc(posix + UnixEpochDifference);

        #endregion

        #region Round

        /// <summary>
        /// Rounds down the specified <see cref="DateTime"/> to the current minute.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>The rounded-down value.</returns>
        public static DateTime RoundDownMinute(this DateTime value)
            => new DateTime(value.Year, value.Month, value.Day, value.Hour, value.Minute, 0, value.Kind);

        /// <summary>
        /// Rounds down the specified <see cref="DateTime"/> to the current hour.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>The rounded-down value.</returns>
        public static DateTime RoundDownHour(this DateTime value)
            => new DateTime(value.Year, value.Month, value.Day, value.Hour, 0, 0, value.Kind);

        /// <summary>
        /// Rounds down the specified <see cref="DateTime"/> to the current day.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>The rounded-down value.</returns>
        public static DateTime RoundDownDay(this DateTime value)
            => new DateTime(value.Year, value.Month, value.Day, 0, 0, 0, value.Kind);

        #endregion
    }
}
