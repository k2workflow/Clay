#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using SourceCode.Clay.OpenApi.Expressions;
using Xunit;

namespace SourceCode.Clay.OpenApi.Tests.Expressions
{
    public static class CompoundExpressionTests
    {
        #region Methods

        [Fact(DisplayName = nameof(CompoundExpression_Parse))]
        public static void CompoundExpression_Parse()
        {
            var sut = OasExpression.Parse("http://example.com/api/foo?bar={$request.body#/bar}&baz={$url}&foo={$method}{$statusCode}");

            Assert.Equal(7, sut.Count);
            Assert.Equal(OasExpressionComponentType.Literal, sut[0].ComponentType);
            Assert.Equal(OasExpressionComponentType.Field, sut[1].ComponentType);
            Assert.Equal(OasExpressionComponentType.Literal, sut[2].ComponentType);
            Assert.Equal(OasExpressionComponentType.Field, sut[3].ComponentType);
            Assert.Equal(OasExpressionComponentType.Literal, sut[4].ComponentType);
            Assert.Equal(OasExpressionComponentType.Field, sut[5].ComponentType);
            Assert.Equal(OasExpressionComponentType.Field, sut[6].ComponentType);

            Assert.Equal("http://example.com/api/foo?bar=", sut[0].ToString());
            Assert.Equal("$request.body#/bar", sut[1].ToString());
            Assert.Equal("&baz=", sut[2].ToString());
            Assert.Equal("$url", sut[3].ToString());
            Assert.Equal("&foo=", sut[4].ToString());
            Assert.Equal("$method", sut[5].ToString());
            Assert.Equal("$statusCode", sut[6].ToString());
        }

        #endregion
    }
}
