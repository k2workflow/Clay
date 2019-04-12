using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using SourceCode.Clay.AspNetCore.Middleware.Correlation.Tests.Mocks;
using Xunit;

namespace SourceCode.Clay.AspNetCore.Middleware.Correlation.Tests
{
    public class UseCorrelationIdShould
    {
        [Fact]
        public void AddMiddlewareWithDefaultOptionsIfNonIsProvided()
        {
            // Arrange
            var app = MockApplicationBuilder.MockUpWithRequiredServices(out _, out _, out _);

            RequestDelegate next = new RequestDelegate((HttpContext _) => Task.CompletedTask);
            var middlewareWasCalled = false;
            Func<RequestDelegate, RequestDelegate> middlewareSupplied = null;

            app.OnUseCalled = (Func<RequestDelegate, RequestDelegate> middleware) =>
            {
                middlewareSupplied = middleware;
                middlewareWasCalled = true;
            };

            // Act
            IApplicationBuilder returned = app.UseCorrelationId();

            // Assert
            returned.Should().Be(app, "the extension method should return the application builder to allow for chaining");
            middlewareWasCalled.Should().Be(true, "the correlation id middleware should have been added to the pipeline");

            // Arrange
            if (middlewareSupplied != null)
            {
                RequestDelegate result = middlewareSupplied.Invoke(next);
                result.Target.Should().BeAssignableTo(typeof(CorrelationMiddleware), "the build delegate target should be an instance of CorrelationIdMiddleware");
                CorrelationMiddleware middlewareInstance = result.Target as CorrelationMiddleware;
            }
        }

        [Fact]
        public void AddMiddlewareWithProvidedOptions()
        {
            // Arrange
            var app = MockApplicationBuilder.MockUpWithRequiredServices(out _, out _, out _);
            var options = new CorrelationOptions();

            RequestDelegate next = new RequestDelegate((HttpContext _) => Task.CompletedTask);

            var middlewareWasCalled = false;
            Func<RequestDelegate, RequestDelegate> middlewareSupplied = null;

            app.OnUseCalled = (Func<RequestDelegate, RequestDelegate> middleware) =>
            {
                middlewareSupplied = middleware;
                middlewareWasCalled = true;
            };

            // Act
            IApplicationBuilder returned = app.UseCorrelationId(options);

            // Assert
            returned.Should().Be(app, "the extension method should return the application builder to allow for chaining");
            middlewareWasCalled.Should().Be(true, "the correlation id middleware should have been added to the pipeline");

            // Arrange
            if (middlewareSupplied != null)
            {
                RequestDelegate result = middlewareSupplied.Invoke(next);
                result.Target.Should().BeAssignableTo(typeof(CorrelationMiddleware), "the build delegate target should be an instance of CorrelationIdMiddleware");
                CorrelationMiddleware middlewareInstance = result.Target as CorrelationMiddleware;
                middlewareInstance.Options.Should().Be(options, "the middleware should be initialized with the supplied CorrelationIdOptions");
            }
        }

        [Fact]
        public void AddMiddlewareWithConfigureOptions()
        {
            // Arrange
            var app = MockApplicationBuilder.MockUpWithRequiredServices(out _, out _, out _);

            RequestDelegate next = new RequestDelegate((HttpContext _) => Task.CompletedTask);

            var middlewareWasCalled = false;
            Func<RequestDelegate, RequestDelegate> middlewareSupplied = null;

            app.OnUseCalled = (Func<RequestDelegate, RequestDelegate> middleware) =>
            {
                middlewareSupplied = middleware;
                middlewareWasCalled = true;
            };

            var configureWasCalled = false;

            // Act
            IApplicationBuilder returned = app.UseCorrelationId((options) =>
            {
                configureWasCalled = true;
                options.Header = "SomeCustomHeader";
            });

            // Assert
            returned.Should().Be(app, "the extension method should return the application builder to allow for chaining");
            middlewareWasCalled.Should().Be(true, "the correlation id middleware should have been added to the pipeline");

            // Arrange
            if (middlewareSupplied != null)
            {
                RequestDelegate result = middlewareSupplied.Invoke(next);
                result.Target.Should().BeAssignableTo(typeof(CorrelationMiddleware), "the build delegate target should be an instance of CorrelationIdMiddleware");
                CorrelationMiddleware middlewareInstance = result.Target as CorrelationMiddleware;
                configureWasCalled.Should().Be(true, "the configure action should have been called to configure the options for the middleware");
                middlewareInstance.Options.Header.Should().Be("SomeCustomHeader", "the middleware should be initialized with the configured CorrelationIdOptions");
            }
        }

        [Fact]
        public void ThrowIfCalledWithNullApplicationBuilder()
        {
            Action action = () => CorrelationApplicationBuilderExtensions.UseCorrelationId(null);
            action.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void ThrowIfCalledWithNullApplicationBuilderAndValidOptions()
        {
            Action action = () => CorrelationApplicationBuilderExtensions.UseCorrelationId(null, new CorrelationOptions());
            action.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void ThrowIfCalledWithApplicationBuilderAndNullOptions()
        {
            Action action = () => CorrelationApplicationBuilderExtensions.UseCorrelationId(new MockApplicationBuilder(), (CorrelationOptions)null);
            action.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void ThrowIfCalledWithNullApplicationBuilderAndValidConfigureAction()
        {
            Action action = () => CorrelationApplicationBuilderExtensions.UseCorrelationId(null, (CorrelationOptions options) => { });
            action.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void ThrowIfCalledWithApplicationBuilderAndNullConfigureAction()
        {
            Action action = () => CorrelationApplicationBuilderExtensions.UseCorrelationId(new MockApplicationBuilder(), (Action<CorrelationOptions>)null);
            action.Should().Throw<ArgumentNullException>();
        }
    }
}
