using System;
using System.Collections.Generic;
using FluentAssertions;
using Microsoft.Extensions.Primitives;
using Xunit;

namespace SourceCode.Clay.AspNetCore.Middleware.Correlation.Tests
{
    public class CorrelationContextShould
    {
        [Fact]
        public void NotThrowWhenCalledWithANullHeaderName()
        {
            Func<CorrelationContext> factory = () => new CorrelationContext("abc", null);
            factory.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void ThrowWhenCalledWithANullCorrelationId()
        {
            Func<CorrelationContext> factory = () => new CorrelationContext(null, new Dictionary<CorrelationHeader, StringValues>());
            factory.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void ThrowWhenCalledWithAnEmptyNullCorrelationId()
        {
            Func<CorrelationContext> factory = () => new CorrelationContext(string.Empty, new Dictionary<CorrelationHeader, StringValues>());
            factory.Should().Throw<ArgumentException>();
        }
    }
}
