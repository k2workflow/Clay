#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using SourceCode.Clay.Json.Pointers;
using SourceCode.Clay.OpenApi.Expressions;
using Xunit;

namespace SourceCode.Clay.OpenApi.Tests.Expressions
{
    public static class FieldExpressionTests
    {
        [Fact(DisplayName = nameof(FieldExpression_Parse_Url))]
        public static void FieldExpression_Parse_Url()
        {
            var sut = OasFieldExpression.Parse("$url");
            Assert.Equal(OasFieldExpressionType.Url, sut.ExpressionType);
            Assert.Equal("$url", sut.ToString());
        }

        [Fact(DisplayName = nameof(FieldExpression_Parse_Method))]
        public static void FieldExpression_Parse_Method()
        {
            var sut = OasFieldExpression.Parse("$method");
            Assert.Equal(OasFieldExpressionType.Method, sut.ExpressionType);
            Assert.Equal("$method", sut.ToString());
        }

        [Fact(DisplayName = nameof(FieldExpression_Parse_StatusCode))]
        public static void FieldExpression_Parse_StatusCode()
        {
            var sut = OasFieldExpression.Parse("$statusCode");
            Assert.Equal(OasFieldExpressionType.StatusCode, sut.ExpressionType);
            Assert.Equal("$statusCode", sut.ToString());
        }

        [Fact(DisplayName = nameof(FieldExpression_Parse_RequestHeader))]
        public static void FieldExpression_Parse_RequestHeader()
        {
            var sut = OasFieldExpression.Parse("$request.header.foo");
            Assert.Equal(OasFieldExpressionType.Request, sut.ExpressionType);
            Assert.Equal(OasFieldExpressionSource.Header, sut.ExpressionSource);
            Assert.Equal("foo", sut.Name);
            Assert.Equal("$request.header.foo", sut.ToString());
        }

        [Fact(DisplayName = nameof(FieldExpression_Parse_RequestQuery))]
        public static void FieldExpression_Parse_RequestQuery()
        {
            var sut = OasFieldExpression.Parse("$request.query.foo");
            Assert.Equal(OasFieldExpressionType.Request, sut.ExpressionType);
            Assert.Equal(OasFieldExpressionSource.Query, sut.ExpressionSource);
            Assert.Equal("foo", sut.Name);
            Assert.Equal("$request.query.foo", sut.ToString());
        }

        [Fact(DisplayName = nameof(FieldExpression_Parse_RequestPath))]
        public static void FieldExpression_Parse_RequestPath()
        {
            var sut = OasFieldExpression.Parse("$request.path.foo");
            Assert.Equal(OasFieldExpressionType.Request, sut.ExpressionType);
            Assert.Equal(OasFieldExpressionSource.Path, sut.ExpressionSource);
            Assert.Equal("foo", sut.Name);
            Assert.Equal("$request.path.foo", sut.ToString());
        }

        [Fact(DisplayName = nameof(FieldExpression_Parse_RequestBody))]
        public static void FieldExpression_Parse_RequestBody()
        {
            var sut = OasFieldExpression.Parse("$request.body#/foo/bar");
            Assert.Equal(OasFieldExpressionType.Request, sut.ExpressionType);
            Assert.Equal(OasFieldExpressionSource.Body, sut.ExpressionSource);
            Assert.Equal(JsonPointer.Parse("/foo/bar"), sut.Pointer);
            Assert.Equal("$request.body#/foo/bar", sut.ToString());
        }

        [Fact(DisplayName = nameof(FieldExpression_Parse_ResponseHeader))]
        public static void FieldExpression_Parse_ResponseHeader()
        {
            var sut = OasFieldExpression.Parse("$response.header.foo");
            Assert.Equal(OasFieldExpressionType.Response, sut.ExpressionType);
            Assert.Equal(OasFieldExpressionSource.Header, sut.ExpressionSource);
            Assert.Equal("foo", sut.Name);
            Assert.Equal("$response.header.foo", sut.ToString());
        }

        [Fact(DisplayName = nameof(FieldExpression_Parse_ResponseQuery))]
        public static void FieldExpression_Parse_ResponseQuery()
        {
            var sut = OasFieldExpression.Parse("$response.query.foo");
            Assert.Equal(OasFieldExpressionType.Response, sut.ExpressionType);
            Assert.Equal(OasFieldExpressionSource.Query, sut.ExpressionSource);
            Assert.Equal("foo", sut.Name);
            Assert.Equal("$response.query.foo", sut.ToString());
        }

        [Fact(DisplayName = nameof(FieldExpression_Parse_ResponsePath))]
        public static void FieldExpression_Parse_ResponsePath()
        {
            var sut = OasFieldExpression.Parse("$response.path.foo");
            Assert.Equal(OasFieldExpressionType.Response, sut.ExpressionType);
            Assert.Equal(OasFieldExpressionSource.Path, sut.ExpressionSource);
            Assert.Equal("foo", sut.Name);
            Assert.Equal("$response.path.foo", sut.ToString());
        }

        [Fact(DisplayName = nameof(FieldExpression_Parse_ResponseBody))]
        public static void FieldExpression_Parse_ResponseBody()
        {
            var sut = OasFieldExpression.Parse("$response.body#/foo/bar");
            Assert.Equal(OasFieldExpressionType.Response, sut.ExpressionType);
            Assert.Equal(OasFieldExpressionSource.Body, sut.ExpressionSource);
            Assert.Equal(JsonPointer.Parse("/foo/bar"), sut.Pointer);
            Assert.Equal("$response.body#/foo/bar", sut.ToString());
        }
    }
}
