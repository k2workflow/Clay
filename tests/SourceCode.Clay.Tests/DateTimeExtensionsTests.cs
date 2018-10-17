#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using System;
using Xunit;

namespace SourceCode.Clay.Tests
{
    public static class DateTimeExtensionsTests
    {
        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(DateTimeExtensions_ToPosixFileTime))]
        public static void DateTimeExtensions_ToPosixFileTime()
        {
            var posix = new DateTime(1987, 01, 19, 02, 30, 33, 123, DateTimeKind.Utc).ToPosixFileTime();
            Assert.Equal(5380218331230000, posix);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(DateTimeExtensions_ToPosixFileTimeUtc))]
        public static void DateTimeExtensions_ToPosixFileTimeUtc()
        {
            var posix = new DateTime(1987, 01, 19, 02, 30, 33, 123, DateTimeKind.Utc).ToPosixFileTimeUtc();
            Assert.Equal(5380218331230000, posix);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(DateTimeExtensions_FromPosixFileTime))]
        public static void DateTimeExtensions_FromPosixFileTime()
        {
            DateTime dt = DateTimeExtensions.FromPosixFileTime(5380218331230000).ToUniversalTime();
            Assert.Equal(new DateTime(1987, 01, 19, 02, 30, 33, 123, DateTimeKind.Utc), dt);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(DateTimeExtensions_FromPosixFileTimeUtc))]
        public static void DateTimeExtensions_FromPosixFileTimeUtc()
        {
            DateTime dt = DateTimeExtensions.FromPosixFileTimeUtc(5380218331230000);
            Assert.Equal(new DateTime(1987, 01, 19, 02, 30, 33, 123, DateTimeKind.Utc), dt);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(DateTimeExtensions_RoundMinute))]
        public static void DateTimeExtensions_RoundMinute()
        {
            DateTime tt = new DateTime(1987, 01, 19, 02, 0, 0, 0, DateTimeKind.Utc).RoundDownMinute();
            Assert.Equal(new DateTime(1987, 01, 19, 02, 0, 0, 0, DateTimeKind.Utc), tt);

            tt = new DateTime(1987, 01, 19, 02, 29, 0, 0, DateTimeKind.Utc).RoundDownMinute();
            Assert.Equal(new DateTime(1987, 01, 19, 02, 29, 0, 0, DateTimeKind.Utc), tt);

            tt = new DateTime(1987, 01, 19, 02, 29, 33, 123, DateTimeKind.Utc).RoundDownMinute();
            Assert.Equal(new DateTime(1987, 01, 19, 02, 29, 0, 0, DateTimeKind.Utc), tt);

            tt = new DateTime(1987, 01, 19, 02, 59, 59, 999, DateTimeKind.Utc).RoundDownMinute();
            Assert.Equal(new DateTime(1987, 01, 19, 02, 59, 0, 0, DateTimeKind.Utc), tt);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(DateTimeExtensions_RoundHour))]
        public static void DateTimeExtensions_RoundHour()
        {
            DateTime tt = new DateTime(1987, 01, 19, 00, 0, 0, 0, DateTimeKind.Utc).RoundDownHour();
            Assert.Equal(new DateTime(1987, 01, 19, 00, 0, 0, 0, DateTimeKind.Utc), tt);

            tt = new DateTime(1987, 01, 19, 02, 0, 0, 0, DateTimeKind.Utc).RoundDownHour();
            Assert.Equal(new DateTime(1987, 01, 19, 02, 0, 0, 0, DateTimeKind.Utc), tt);

            tt = new DateTime(1987, 01, 19, 02, 29, 0, 0, DateTimeKind.Utc).RoundDownHour();
            Assert.Equal(new DateTime(1987, 01, 19, 02, 0, 0, 0, DateTimeKind.Utc), tt);

            tt = new DateTime(1987, 01, 19, 23, 59, 59, 999, DateTimeKind.Utc).RoundDownHour();
            Assert.Equal(new DateTime(1987, 01, 19, 23, 0, 0, 0, DateTimeKind.Utc), tt);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(DateTimeExtensions_RoundDay))]
        public static void DateTimeExtensions_RoundDay()
        {
            DateTime tt = new DateTime(1987, 01, 19, 00, 0, 0, 0, DateTimeKind.Utc).RoundDownDay();
            Assert.Equal(new DateTime(1987, 01, 19, 00, 0, 0, 0, DateTimeKind.Utc), tt);

            tt = new DateTime(1987, 01, 19, 02, 0, 0, 0, DateTimeKind.Utc).RoundDownDay();
            Assert.Equal(new DateTime(1987, 01, 19, 0, 0, 0, 0, DateTimeKind.Utc), tt);

            tt = new DateTime(1987, 01, 19, 02, 29, 0, 0, DateTimeKind.Utc).RoundDownDay();
            Assert.Equal(new DateTime(1987, 01, 19, 0, 0, 0, 0, DateTimeKind.Utc), tt);

            tt = new DateTime(1987, 01, 31, 23, 59, 59, 999, DateTimeKind.Utc).RoundDownDay();
            Assert.Equal(new DateTime(1987, 01, 31, 0, 0, 0, 0, DateTimeKind.Utc), tt);
        }
    }
}
