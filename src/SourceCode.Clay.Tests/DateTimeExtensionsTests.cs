using System;
using Xunit;

namespace SourceCode.Clay.Tests
{
    public class DateTimeExtensionsTests
    {
        [Trait("Type", "Unit")]
        [Fact(DisplayName = "DateTimeExtensions ToPosixFileTime")]
        public static void DateTimeExtensions_ToPosixFileTime()
        {
            var posix = new DateTime(1987, 01, 19, 02, 30, 33, 123, DateTimeKind.Utc).ToPosixFileTime();
            Assert.Equal(5380218331230000, posix);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = "DateTimeExtensions ToPosixFileTimeUtc")]
        public static void DateTimeExtensions_ToPosixFileTimeUtc()
        {
            var posix = new DateTime(1987, 01, 19, 02, 30, 33, 123, DateTimeKind.Utc).ToPosixFileTimeUtc();
            Assert.Equal(5380218331230000, posix);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = "DateTimeExtensions FromPosixFileTime")]
        public static void DateTimeExtensions_FromPosixFileTime()
        {
            var dt = DateTimeExtensions.FromPosixFileTime(5380218331230000).ToUniversalTime();
            Assert.Equal(new DateTime(1987, 01, 19, 02, 30, 33, 123, DateTimeKind.Utc), dt);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = "DateTimeExtensions FromPosixFileTimeUtc")]
        public static void DateTimeExtensions_FromPosixFileTimeUtc()
        {
            var dt = DateTimeExtensions.FromPosixFileTimeUtc(5380218331230000);
            Assert.Equal(new DateTime(1987, 01, 19, 02, 30, 33, 123, DateTimeKind.Utc), dt);
        }
    }
}
