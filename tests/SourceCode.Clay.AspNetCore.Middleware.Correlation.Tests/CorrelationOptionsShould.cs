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
            options.Headers.Should().Equal(new[] { new CorrelationHeader(Constants.XCorrelationID, true) }, "the default correlation id header should be used.");
            options.UseTraceIdentifier.Should().Be(false, "the default trace identifier is machine and connection specific - it is not a very good cross machine correlation id by default.");
            options.UpdateTraceIdentifier.Should().Be(true, "the internal logging of ASP.NET uses the trace identifier thus it is easier if it is updated to match any incoming correlation id.");
            options.CorrelationIdGenerator.Should().NotBeNull("the options should have a non-null correlation id generator by default");

            string generated = options.CorrelationIdGenerator(new DefaultHttpContext());
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
                string generated = options.CorrelationIdGenerator(context);

                // .Add returns false if the element is already present.
                previous.Add(generated).Should().BeTrue("each generated value should be unique");
            }
        }
    }
}
