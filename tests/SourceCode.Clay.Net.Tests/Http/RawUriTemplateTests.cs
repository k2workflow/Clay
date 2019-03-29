using System;
using Xunit;

namespace SourceCode.Clay.Net.Http.Tests
{
    public static class RawUriTemplateTests
    {
        [Fact]
        public static void RawUriTemplate_Parse_Empty()
        {
            var template = RawUriTemplate.Parse("");
            Assert.Empty(template.Query);
            Assert.Empty(template.Path);
        }

        [Fact]
        public static void RawUriTemplate_Parse_Literal()
        {
            var template = RawUriTemplate.Parse("/test");
            Assert.Empty(template.Query);
            UriToken item = Assert.Single(template.Path);
            Assert.Null(item.Name);
            Assert.Null(item.Format);
            Assert.Equal(UriTokenType.Literal, item.Type);
            Assert.Equal("/test", item.Default);
        }

        [Fact]
        public static void RawUriTemplate_Parse_Query_Empty()
        {
            var template = RawUriTemplate.Parse("/test?");
            Assert.Empty(template.Query);
            UriToken item = Assert.Single(template.Path);
            Assert.Null(item.Name);
            Assert.Null(item.Format);
            Assert.Equal(UriTokenType.Literal, item.Type);
            Assert.Equal("/test", item.Default);
        }

        [Fact]
        public static void RawUriTemplate_Parse_Value()
        {
            var template = RawUriTemplate.Parse("{test?}");
            Assert.Empty(template.Query);
            UriToken item = Assert.Single(template.Path);
            Assert.Equal("test?", item.Name);
            Assert.Null(item.Format);
            Assert.Equal(UriTokenType.Value, item.Type);
            Assert.Null(item.Default);
        }

        [Fact]
        public static void RawUriTemplate_Parse_Value_InvalidLBrace1()
        {
            FormatException ex = Assert.Throws<FormatException>(() => RawUriTemplate.Parse("{test"));
            Assert.Equal("The URI template contains an invalid token.", ex.Message);
        }

        [Fact]
        public static void RawUriTemplate_Parse_Value_InvalidLBrace2()
        {
            FormatException ex = Assert.Throws<FormatException>(() => RawUriTemplate.Parse("test{"));
            Assert.Equal("The URI template contains an invalid token.", ex.Message);
        }

        [Fact]
        public static void RawUriTemplate_Parse_Value_InvalidRBrace1()
        {
            FormatException ex = Assert.Throws<FormatException>(() => RawUriTemplate.Parse("test}"));
            Assert.Equal("The URI template contains an invalid token.", ex.Message);
        }

        [Fact]
        public static void RawUriTemplate_Parse_Value_InvalidRBrace2()
        {
            FormatException ex = Assert.Throws<FormatException>(() => RawUriTemplate.Parse("}test"));
            Assert.Equal("The URI template contains an invalid token.", ex.Message);
        }

        [Fact]
        public static void RawUriTemplate_Parse_LiteralValue()
        {
            var template = RawUriTemplate.Parse("/test/{test}");
            Assert.Empty(template.Query);
            Assert.Collection(template.Path,
            item =>
            {
                Assert.Null(item.Name);
                Assert.Null(item.Format);
                Assert.Equal(UriTokenType.Literal, item.Type);
                Assert.Equal("/test/", item.Default);
            },
            item =>
            {
                Assert.Equal("test", item.Name);
                Assert.Null(item.Format);
                Assert.Equal(UriTokenType.Value, item.Type);
                Assert.Null(item.Default);
            });
        }

        [Fact]
        public static void RawUriTemplate_Parse_ValueLiteral()
        {
            var template = RawUriTemplate.Parse("{test}/test");
            Assert.Empty(template.Query);

            Assert.Collection(template.Path,
            item =>
            {
                Assert.Equal("test", item.Name);
                Assert.Null(item.Format);
                Assert.Equal(UriTokenType.Value, item.Type);
                Assert.Null(item.Default);
            },
            item =>
            {
                Assert.Null(item.Name);
                Assert.Null(item.Format);
                Assert.Equal(UriTokenType.Literal, item.Type);
                Assert.Equal("/test", item.Default);
            });
        }

        [Fact]
        public static void RawUriTemplate_Parse_LiteralValueLiteral()
        {
            var template = RawUriTemplate.Parse("/test/{test?}/test");
            Assert.Empty(template.Query);
            Assert.Collection(template.Path,
            item =>
            {
                Assert.Null(item.Name);
                Assert.Null(item.Format);
                Assert.Equal(UriTokenType.Literal, item.Type);
                Assert.Equal("/test/", item.Default);
            },
            item =>
            {
                Assert.Equal("test?", item.Name);
                Assert.Null(item.Format);
                Assert.Equal(UriTokenType.Value, item.Type);
                Assert.Null(item.Default);
            },
            item =>
            {
                Assert.Null(item.Name);
                Assert.Null(item.Format);
                Assert.Equal(UriTokenType.Literal, item.Type);
                Assert.Equal("/test", item.Default);
            });
        }

        [Fact]
        public static void RawUriTemplate_Parse_LBraceLiteral()
        {
            var template = RawUriTemplate.Parse("/test{{foo");
            Assert.Empty(template.Query);
            UriToken item = Assert.Single(template.Path);
            Assert.Null(item.Name);
            Assert.Null(item.Format);
            Assert.Equal(UriTokenType.Literal, item.Type);
            Assert.Equal("/test{foo", item.Default);
        }

        [Fact]
        public static void RawUriTemplate_Parse_RBraceLiteral()
        {
            var template = RawUriTemplate.Parse("/test}}foo");
            Assert.Empty(template.Query);
            UriToken item = Assert.Single(template.Path);
            Assert.Null(item.Name);
            Assert.Null(item.Format);
            Assert.Equal(UriTokenType.Literal, item.Type);
            Assert.Equal("/test}foo", item.Default);
        }

        [Fact]
        public static void RawUriTemplate_Parse_Value_Format()
        {
            var template = RawUriTemplate.Parse("{test:N?}");
            Assert.Empty(template.Query);
            UriToken item = Assert.Single(template.Path);
            Assert.Equal("test", item.Name);
            Assert.Equal("N?", item.Format);
            Assert.Equal(UriTokenType.Value, item.Type);
            Assert.Null(item.Default);
        }

        [Fact]
        public static void RawUriTemplate_Parse_Value_Format_Default()
        {
            var template = RawUriTemplate.Parse("{test:N?=ba?z}");
            Assert.Empty(template.Query);
            UriToken item = Assert.Single(template.Path);
            Assert.Equal("test", item.Name);
            Assert.Equal("N?", item.Format);
            Assert.Equal(UriTokenType.Value, item.Type);
            Assert.Equal("ba?z", item.Default);
        }

        [Fact]
        public static void RawUriTemplate_Parse_Value_Default()
        {
            var template = RawUriTemplate.Parse("{te?st=foo?}");
            Assert.Empty(template.Query);
            UriToken item = Assert.Single(template.Path);
            Assert.Equal("te?st", item.Name);
            Assert.Null(item.Format);
            Assert.Equal(UriTokenType.Value, item.Type);
            Assert.Equal("foo?", item.Default);
        }

        [Fact]
        public static void RawUriTemplate_Parse_Value_Format_Escape()
        {
            var template = RawUriTemplate.Parse("{te::st:N}");
            Assert.Empty(template.Query);
            UriToken item = Assert.Single(template.Path);
            Assert.Equal("te:st", item.Name);
            Assert.Equal("N", item.Format);
            Assert.Equal(UriTokenType.Value, item.Type);
            Assert.Null(item.Default);
        }

        [Fact]
        public static void RawUriTemplate_Parse_Value_Default_Escape()
        {
            var template = RawUriTemplate.Parse("{te==st=foo}");
            Assert.Empty(template.Query);
            UriToken item = Assert.Single(template.Path);
            Assert.Equal("te=st", item.Name);
            Assert.Null(item.Format);
            Assert.Equal(UriTokenType.Value, item.Type);
            Assert.Equal("foo", item.Default);
        }

        [Fact]
        public static void RawUriTemplate_Parse_Value_Format_Escape_RBrace()
        {
            var template = RawUriTemplate.Parse("{test:{{N}}}");
            Assert.Empty(template.Query);
            UriToken item = Assert.Single(template.Path);
            Assert.Equal("test", item.Name);
            Assert.Equal("{N}", item.Format);
            Assert.Equal(UriTokenType.Value, item.Type);
            Assert.Null(item.Default);
        }

        [Fact]
        public static void RawUriTemplate_Parse_Value_Default_Escape_RBrace()
        {
            var template = RawUriTemplate.Parse("{test={{fo?o}}}");
            Assert.Empty(template.Query);
            UriToken item = Assert.Single(template.Path);
            Assert.Equal("test", item.Name);
            Assert.Null(item.Format);
            Assert.Equal(UriTokenType.Value, item.Type);
            Assert.Equal("{fo?o}", item.Default);
        }

        [Fact]
        public static void RawUriTemplate_Parse_Query_Name()
        {
            var template = RawUriTemplate.Parse("?test");
            Assert.Empty(template.Path);
            UriQuery query = Assert.Single(template.Query);
            Assert.Empty(query.Value);
            UriToken name = Assert.Single(query.Name);

            Assert.Null(name.Name);
            Assert.Null(name.Format);
            Assert.Equal(UriTokenType.Literal, name.Type);
            Assert.Equal("test", name.Default);
        }

        [Fact]
        public static void RawUriTemplate_Parse_Query_Name_Value()
        {
            var template = RawUriTemplate.Parse("?test=bar");
            Assert.Empty(template.Path);
            UriQuery query = Assert.Single(template.Query);
            UriToken name = Assert.Single(query.Name);

            Assert.Null(name.Name);
            Assert.Null(name.Format);
            Assert.Equal(UriTokenType.Literal, name.Type);
            Assert.Equal("test", name.Default);

            UriToken value = Assert.Single(query.Value);
            Assert.Null(value.Name);
            Assert.Null(value.Format);
            Assert.Equal(UriTokenType.Literal, value.Type);
            Assert.Equal("bar", value.Default);
        }

        [Fact]
        public static void RawUriTemplate_Parse_Query_NameToken_ValueToken()
        {
            var template = RawUriTemplate.Parse("?test{foo}={baz}bar");
            Assert.Empty(template.Path);
            UriQuery query = Assert.Single(template.Query);

            Assert.Collection(query.Name,
            name =>
            {
                Assert.Null(name.Name);
                Assert.Null(name.Format);
                Assert.Equal(UriTokenType.Literal, name.Type);
                Assert.Equal("test", name.Default);
            },
            name =>
            {
                Assert.Equal("foo", name.Name);
                Assert.Null(name.Format);
                Assert.Equal(UriTokenType.Value, name.Type);
                Assert.Null(name.Default);
            });

            Assert.Collection(query.Value,
            value =>
            {
                Assert.Equal("baz", value.Name);
                Assert.Null(value.Format);
                Assert.Equal(UriTokenType.Value, value.Type);
                Assert.Null(value.Default);
            },
            value =>
            {
                Assert.Null(value.Name);
                Assert.Null(value.Format);
                Assert.Equal(UriTokenType.Literal, value.Type);
                Assert.Equal("bar", value.Default);
            });
        }

        [Fact]
        public static void RawUriTemplate_Parse_Query_Name_Value_Name_Value()
        {
            var template = RawUriTemplate.Parse("?test=bar&foo=baz");
            Assert.Empty(template.Path);

            Assert.Collection(template.Query,
            query =>
            {
                UriToken name = Assert.Single(query.Name);
                Assert.Null(name.Name);
                Assert.Null(name.Format);
                Assert.Equal(UriTokenType.Literal, name.Type);
                Assert.Equal("test", name.Default);

                UriToken value = Assert.Single(query.Value);
                Assert.Null(value.Name);
                Assert.Null(value.Format);
                Assert.Equal(UriTokenType.Literal, value.Type);
                Assert.Equal("bar", value.Default);
            },
            query =>
            {
                UriToken name = Assert.Single(query.Name);
                Assert.Null(name.Name);
                Assert.Null(name.Format);
                Assert.Equal(UriTokenType.Literal, name.Type);
                Assert.Equal("foo", name.Default);

                UriToken value = Assert.Single(query.Value);
                Assert.Null(value.Name);
                Assert.Null(value.Format);
                Assert.Equal(UriTokenType.Literal, value.Type);
                Assert.Equal("baz", value.Default);
            });
        }
    }
}
