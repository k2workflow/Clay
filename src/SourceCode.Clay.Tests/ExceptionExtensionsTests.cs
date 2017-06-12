using System;
using Xunit;

namespace SourceCode.Clay.Tests
{
    public static class ExceptionExtensionsTests
    {
        [Trait("Type", "Unit")]
        [Fact(DisplayName = "ExceptionExtensions Suppress")]
        public static void When_suppress_exception()
        {
            var expected = new Exception();
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
