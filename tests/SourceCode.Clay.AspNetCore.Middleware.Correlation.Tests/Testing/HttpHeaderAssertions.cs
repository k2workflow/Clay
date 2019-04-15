using System;
using System.Diagnostics;
using System.Linq;
using System.Net.Http.Headers;
using FluentAssertions;
using FluentAssertions.Execution;
using FluentAssertions.Primitives;

namespace SourceCode.Clay.AspNetCore.Middleware.Correlation.Tests
{
    [DebuggerNonUserCode]
    public sealed class HttpHeadersAssertions :
        ReferenceTypeAssertions<HttpHeaders, HttpHeadersAssertions>
    {
        public HttpHeadersAssertions(HttpHeaders instance)
        {
            Subject = instance;
        }

        protected override string Identifier => "headers";

        public AndConstraint<HttpHeadersAssertions> HaveCount(int count, string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .Given(() => Subject.AsEnumerable())
                .ForCondition(headers => headers.Count() == count)
                .FailWith("Expected {context:headers} to contain {0} entries(s){reason}, but found {1}.",
                    _ => count, _ => Subject.Count());

            Subject.AsEnumerable().Should().HaveCount(count, because, becauseArgs);

            return new AndConstraint<HttpHeadersAssertions>(this);
        }

        public AndConstraint<HttpHeadersAssertions> ContainHeader(
            string headername, string headervalue, string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .ForCondition(!string.IsNullOrEmpty(headername))
                .FailWith("You can't assert a header exist if you don't pass a proper name")
                .Then
                .Given(() => Subject.AsEnumerable())
                .ForCondition(headers => headers.Any(header => header.Key.Equals(headername, StringComparison.Ordinal) && header.Value.Any(value => string.Equals(value, headervalue, StringComparison.Ordinal))))
                .FailWith("Expected {context:header} to contain {0}{reason}, but found {1}.",
                    _ => headername, _ => Subject.GetValues(headername));

            return new AndConstraint<HttpHeadersAssertions>(this);
        }

        public AndConstraint<HttpHeadersAssertions> ContainHeader(
            string headername, string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .ForCondition(!string.IsNullOrEmpty(headername))
                .FailWith("You can't assert a header exist if you don't pass a proper name")
                .Then
                .Given(() => Subject.AsEnumerable())
                .ForCondition(headers => headers.Any(header => header.Key.Equals(headername, StringComparison.Ordinal)))
                .FailWith("Expected {context:header} to be present {reason}.",
                    _ => headername);

            return new AndConstraint<HttpHeadersAssertions>(this);
        }
    }
}
