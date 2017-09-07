using System;
using Xunit;

namespace SourceCode.Clay.Tests
{
    public static class ExceptionExtensionsTests
    {
        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(When_suppress_exception))]
        public static void When_suppress_exception()
        {
#pragma warning disable S112 // General exceptions should never be thrown
            var expected = new Exception();
#pragma warning restore S112 // General exceptions should never be thrown

            try
            {
                throw expected;
            }
            catch (Exception e)
            {
                var actual = e.Suppress();

                Assert.Equal(expected, actual);
            }
        }
    }
}
