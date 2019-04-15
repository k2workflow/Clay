using System;
using FluentAssertions;
using Xunit;

namespace SourceCode.Clay.AspNetCore.Middleware.Correlation.Tests
{
    public class CorrelationContextShould
    {
        [Fact]
        public void ThrowWhenCalledWithANullHeaderName()
        {
            Func<CorrelationContext> factory = () => new CorrelationContext("abc", null);
            factory.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void ThrowWhenCalledWithAnEmtpyHeaderName()
        {
            Func<CorrelationContext> factory = () => new CorrelationContext("abc", string.Empty);
            factory.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void ThrowWhenCalledWithANullCorrelationId()
        {
            Func<CorrelationContext> factory = () => new CorrelationContext(null, "abc");
            factory.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void ThrowWhenCalledWithAnEmptyNullCorrelationId()
        {
            Func<CorrelationContext> factory = () => new CorrelationContext(string.Empty, "abc");
            factory.Should().Throw<ArgumentException>();
        }
    }
}
