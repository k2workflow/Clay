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
        [Fact]
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
        [Fact]
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
        [Fact]
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
