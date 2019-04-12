using System;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using SourceCode.Clay.AspNetCore.Middleware.Correlation.Tests.Mocks;
using Xunit;

namespace SourceCode.Clay.AspNetCore.Middleware.Correlation.Tests
{
    public class AddCorrelationIdShould
    {
        [Fact]
        public void AddRequireServicesToCollection()
        {
            // Arrange
            var services = new MockServiceCollection();

            // Act
            services.AddCorrelationId();

            // Assert
            services.Count.Should().Be(1);
            ServiceDescriptor serviceDescriptor = services[0];
            serviceDescriptor.ImplementationType.Should().Be(typeof(CorrelationContextAccessor));
            serviceDescriptor.ServiceType.Should().Be(typeof(ICorrelationContextAccessor));
            serviceDescriptor.Lifetime.Should().Be(ServiceLifetime.Singleton);
        }

        [Fact]
        public void ThrowIfCalledWithNullServiceCollection()
        {
            Action action = () => CollelationServicesCollectionExtensions.AddCorrelationId(null);
            action.Should().Throw<ArgumentNullException>();
        }

    }
}
