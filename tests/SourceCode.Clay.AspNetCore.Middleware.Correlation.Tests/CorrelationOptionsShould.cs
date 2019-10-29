using System;
using System.Collections.Generic;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Xunit;

namespace SourceCode.Clay.AspNetCore.Middleware.Correlation.Tests
{
    public class CorrelationOptionsShould
    {
        [Fact]
        public void HaveCorrectDefaults()
        {
            // Act
            var options = new CorrelationOptions();

            // Assert
            options.Headers.Should().BeEquivalentTo(new[] { new CorrelationHeader(Constants.XCorrelationID) }, "the default correlation id header should be used.");

            string generated = options.Headers[0].CorrelationIdGenerator(new DefaultHttpContext());
            generated.Should().NotBeNullOrWhiteSpace();
            Guid.TryParse(generated, out _).Should().Be(true, "the default generator should create guid strings");
        }

        [Fact]
        public void GenerateDifferentCorrelationIdsForEachRequest()
        {
            // Act
            var options = new CorrelationOptions();
            var previous = new HashSet<string>(StringComparer.OrdinalIgnoreCase); // Guids are case-insensitive
            var context = new DefaultHttpContext();

            // Assert
            while (previous.Count < 10000)
            {
                string generated = options.Headers[0].CorrelationIdGenerator(context);

                // .Add returns false if the element is already present.
                previous.Add(generated).Should().BeTrue("each generated value should be unique");
            }
        }
    }
}
