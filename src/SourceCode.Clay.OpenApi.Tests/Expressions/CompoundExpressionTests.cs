using SourceCode.Clay.OpenApi.Expressions;
using Xunit;

namespace SourceCode.Clay.OpenApi.Tests.Expressions
{
    public static class CompoundExpressionTests
    {
        [Fact(DisplayName = nameof(CompoundExpression_Parse))]
        public static void CompoundExpression_Parse()
        {
            var sut = CompoundExpression.Parse("http://example.com/api/foo?bar={$request.body#/bar}&baz={$url}&foo={$method}{$statusCode}");

            Assert.Equal(7, sut.Count);
            Assert.Equal(ExpressionComponentType.Literal, sut[0].ComponentType);
            Assert.Equal(ExpressionComponentType.Field, sut[1].ComponentType);
            Assert.Equal(ExpressionComponentType.Literal, sut[2].ComponentType);
            Assert.Equal(ExpressionComponentType.Field, sut[3].ComponentType);
            Assert.Equal(ExpressionComponentType.Literal, sut[4].ComponentType);
            Assert.Equal(ExpressionComponentType.Field, sut[5].ComponentType);
            Assert.Equal(ExpressionComponentType.Field, sut[6].ComponentType);

            Assert.Equal("http://example.com/api/foo?bar=", sut[0].ToString());
            Assert.Equal("$request.body#/bar", sut[1].ToString());
            Assert.Equal("&baz=", sut[2].ToString());
            Assert.Equal("$url", sut[3].ToString());
            Assert.Equal("&foo=", sut[4].ToString());
            Assert.Equal("$method", sut[5].ToString());
            Assert.Equal("$statusCode", sut[6].ToString());
        }
    }
}
