using SourceCode.Clay.Json.Pointers;
using SourceCode.Clay.OpenApi.Expressions;
using Xunit;

namespace SourceCode.Clay.OpenApi.Tests.Expressions
{
    public static class FieldExpressionTests
    {
        #region Methods

        [Fact(DisplayName = nameof(FieldExpression_Parse_Method))]
        public static void FieldExpression_Parse_Method()
        {
            var sut = FieldExpression.Parse("$method");
            Assert.Equal(FieldExpressionType.Method, sut.ExpressionType);
            Assert.Equal("$method", sut.ToString());
        }

        [Fact(DisplayName = nameof(FieldExpression_Parse_RequestBody))]
        public static void FieldExpression_Parse_RequestBody()
        {
            var sut = FieldExpression.Parse("$request.body#/foo/bar");
            Assert.Equal(FieldExpressionType.Request, sut.ExpressionType);
            Assert.Equal(FieldExpressionSource.Body, sut.ExpressionSource);
            Assert.Equal(JsonPointer.Parse("/foo/bar"), sut.Pointer);
            Assert.Equal("$request.body#/foo/bar", sut.ToString());
        }

        [Fact(DisplayName = nameof(FieldExpression_Parse_RequestHeader))]
        public static void FieldExpression_Parse_RequestHeader()
        {
            var sut = FieldExpression.Parse("$request.header.foo");
            Assert.Equal(FieldExpressionType.Request, sut.ExpressionType);
            Assert.Equal(FieldExpressionSource.Header, sut.ExpressionSource);
            Assert.Equal("foo", sut.Name);
            Assert.Equal("$request.header.foo", sut.ToString());
        }

        [Fact(DisplayName = nameof(FieldExpression_Parse_RequestPath))]
        public static void FieldExpression_Parse_RequestPath()
        {
            var sut = FieldExpression.Parse("$request.path.foo");
            Assert.Equal(FieldExpressionType.Request, sut.ExpressionType);
            Assert.Equal(FieldExpressionSource.Path, sut.ExpressionSource);
            Assert.Equal("foo", sut.Name);
            Assert.Equal("$request.path.foo", sut.ToString());
        }

        [Fact(DisplayName = nameof(FieldExpression_Parse_RequestQuery))]
        public static void FieldExpression_Parse_RequestQuery()
        {
            var sut = FieldExpression.Parse("$request.query.foo");
            Assert.Equal(FieldExpressionType.Request, sut.ExpressionType);
            Assert.Equal(FieldExpressionSource.Query, sut.ExpressionSource);
            Assert.Equal("foo", sut.Name);
            Assert.Equal("$request.query.foo", sut.ToString());
        }

        [Fact(DisplayName = nameof(FieldExpression_Parse_ResponseBody))]
        public static void FieldExpression_Parse_ResponseBody()
        {
            var sut = FieldExpression.Parse("$response.body#/foo/bar");
            Assert.Equal(FieldExpressionType.Response, sut.ExpressionType);
            Assert.Equal(FieldExpressionSource.Body, sut.ExpressionSource);
            Assert.Equal(JsonPointer.Parse("/foo/bar"), sut.Pointer);
            Assert.Equal("$response.body#/foo/bar", sut.ToString());
        }

        [Fact(DisplayName = nameof(FieldExpression_Parse_ResponseHeader))]
        public static void FieldExpression_Parse_ResponseHeader()
        {
            var sut = FieldExpression.Parse("$response.header.foo");
            Assert.Equal(FieldExpressionType.Response, sut.ExpressionType);
            Assert.Equal(FieldExpressionSource.Header, sut.ExpressionSource);
            Assert.Equal("foo", sut.Name);
            Assert.Equal("$response.header.foo", sut.ToString());
        }

        [Fact(DisplayName = nameof(FieldExpression_Parse_ResponsePath))]
        public static void FieldExpression_Parse_ResponsePath()
        {
            var sut = FieldExpression.Parse("$response.path.foo");
            Assert.Equal(FieldExpressionType.Response, sut.ExpressionType);
            Assert.Equal(FieldExpressionSource.Path, sut.ExpressionSource);
            Assert.Equal("foo", sut.Name);
            Assert.Equal("$response.path.foo", sut.ToString());
        }

        [Fact(DisplayName = nameof(FieldExpression_Parse_ResponseQuery))]
        public static void FieldExpression_Parse_ResponseQuery()
        {
            var sut = FieldExpression.Parse("$response.query.foo");
            Assert.Equal(FieldExpressionType.Response, sut.ExpressionType);
            Assert.Equal(FieldExpressionSource.Query, sut.ExpressionSource);
            Assert.Equal("foo", sut.Name);
            Assert.Equal("$response.query.foo", sut.ToString());
        }

        [Fact(DisplayName = nameof(FieldExpression_Parse_StatusCode))]
        public static void FieldExpression_Parse_StatusCode()
        {
            var sut = FieldExpression.Parse("$statusCode");
            Assert.Equal(FieldExpressionType.StatusCode, sut.ExpressionType);
            Assert.Equal("$statusCode", sut.ToString());
        }

        [Fact(DisplayName = nameof(FieldExpression_Parse_Url))]
        public static void FieldExpression_Parse_Url()
        {
            var sut = FieldExpression.Parse("$url");
            Assert.Equal(FieldExpressionType.Url, sut.ExpressionType);
            Assert.Equal("$url", sut.ToString());
        }

        #endregion Methods
    }
}
