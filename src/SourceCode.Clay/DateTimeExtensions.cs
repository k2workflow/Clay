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

        #region Methods

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
        public static DateTime FromPosixFileTime(this long posix) => DateTime.FromFileTime(posix + UnixEpochDifference);

        /// <summary>
        /// Converts the specified Posix timestamp to a <see cref="DateTime"/>.
        /// </summary>
        /// <param name="posix">The Posix timestamp to convert.</param>
        /// <returns>The <see cref="DateTime"/>.</returns>
        public static DateTime FromPosixFileTimeUtc(this long posix) => DateTime.FromFileTimeUtc(posix + UnixEpochDifference);

        #endregion
    }
}
