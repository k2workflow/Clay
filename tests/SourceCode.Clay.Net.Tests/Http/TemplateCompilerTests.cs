using System;
using Xunit;
using static System.FormattableString;

namespace SourceCode.Clay.Net.Http.Tests
{
    public static class TemplateCompilerTests
    {
        private class Formattable : IFormattable
        {
            public string ToString(string format, IFormatProvider formatProvider) => "f" + format;
        }

        private struct NotFormattable
        {
            public override string ToString() => "Foo!";
        }

        private enum NormalEnum
        {
            First = 0,
            Second = 1
        }

        [Flags]
        private enum FlagsEnum
        {
            None = 0,
            First = 1,
            Second = 2
        }

        private struct Values
        {
            public int Field;
            public int? NullableField;
            public NotFormattable? NotFormattable;
            public Formattable Formattable;
            public string[] Collection;
            public NormalEnum Normal;
            public FlagsEnum Flags;
            public NormalEnum? NormalNullable;
        }

        [Fact]
        public static void TemplateCompiler_Compile_Constant()
        {
            var template = RawUriTemplate.Parse("/test/url?some=value&foo=bar");
            Func<object, string> compiled = TemplateCompiler.Compile<object>(template);
            string result = compiled(new object());
            Assert.Equal("/test/url?some=value&foo=bar", result);
        }

        [Fact]
        public static void TemplateCompiler_Compile_Path()
        {
            var values = new Values()
            {
                Field = 100
            };

            var template = RawUriTemplate.Parse("/test/{Field}");
            Func<Values, string> compiled = TemplateCompiler.Compile<Values>(template);
            string result = compiled(values);
            Assert.Equal("/test/100", result);
        }

        [Fact]
        public static void TemplateCompiler_Compile_NullablePath()
        {
            var values = new Values()
            {
                NullableField = 100
            };

            var template = RawUriTemplate.Parse("/test/{NullableField:x}");
            Func<Values, string> compiled = TemplateCompiler.Compile<Values>(template);
            string result = compiled(values);
            Assert.Equal("/test/64", result);
        }

        [Fact]
        public static void TemplateCompiler_Compile_DefaultPath()
        {
            var values = new Values();

            var template = RawUriTemplate.Parse("/test/{NullableField=Default}");
            Func<Values, string> compiled = TemplateCompiler.Compile<Values>(template);
            string result = compiled(values);
            Assert.Equal("/test/Default", result);
        }

        [Fact]
        public static void TemplateCompiler_Compile_Query()
        {
            var values = new Values()
            {
                Field = 100
            };

            var template = RawUriTemplate.Parse("?f={Field}");
            Func<Values, string> compiled = TemplateCompiler.Compile<Values>(template);
            string result = compiled(values);
            Assert.Equal("?f=100", result);
        }

        [Fact]
        public static void TemplateCompiler_Compile_NullableQuery()
        {
            var values = new Values()
            {
                NullableField = 100
            };

            var template = RawUriTemplate.Parse("?f={NullableField:x}");
            Func<Values, string> compiled = TemplateCompiler.Compile<Values>(template);
            string result = compiled(values);
            Assert.Equal("?f=64", result);
        }

        [Fact]
        public static void TemplateCompiler_Compile_NullableQuery_Null()
        {
            var values = new Values();

            var template = RawUriTemplate.Parse("?f={NullableField:x}");
            Func<Values, string> compiled = TemplateCompiler.Compile<Values>(template);
            string result = compiled(values);
            Assert.Equal("", result);
        }

        [Fact]
        public static void TemplateCompiler_Compile_DefaultQuery()
        {
            var values = new Values();

            var template = RawUriTemplate.Parse("?f={NullableField=Default}");
            Func<Values, string> compiled = TemplateCompiler.Compile<Values>(template);
            string result = compiled(values);
            Assert.Equal("?f=Default", result);
        }

        [Fact]
        public static void TemplateCompiler_Compile_CollectionQuery_Empty()
        {
            var values = new Values();

            var template = RawUriTemplate.Parse("?f={Collection[]}");
            Func<Values, string> compiled = TemplateCompiler.Compile<Values>(template);
            string result = compiled(values);
            Assert.Equal("", result);
        }

        [Fact]
        public static void TemplateCompiler_Compile_CollectionQuery()
        {
            var values = new Values()
            {
                Collection = new[]
                {
                    "Test",
                    null,
                    "Foo"
                }
            };

            var template = RawUriTemplate.Parse("?f={Collection[]}");
            Func<Values, string> compiled = TemplateCompiler.Compile<Values>(template);
            string result = compiled(values);
            Assert.Equal("?f=Test&f&f=Foo", result);
        }

        [Fact]
        public static void TemplateCompiler_Compile_CollectionQuery_NotCollection()
        {
            var template = RawUriTemplate.Parse("?f={Field[]}");
            InvalidOperationException ex = Assert.Throws<InvalidOperationException>(() => TemplateCompiler.Compile<Values>(template));
            Assert.Equal("An enumerable was expected for Field.", ex.Message);
        }

        [Fact]
        public static void TemplateCompiler_Compile_EnumQuery()
        {
            var values = new Values()
            {
                Normal = NormalEnum.Second
            };

            var template = RawUriTemplate.Parse("?f={Normal.First}&s={Normal.Second}&t={Normal.Second}");
            Func<Values, string> compiled = TemplateCompiler.Compile<Values>(template);
            string result = compiled(values);
            Assert.Equal("?s&t", result);
        }

        [Fact]
        public static void TemplateCompiler_Compile_EnumQuery_NotEnum()
        {
            var template = RawUriTemplate.Parse("?f={Field.First}");
            InvalidOperationException ex = Assert.Throws<InvalidOperationException>(() => TemplateCompiler.Compile<Values>(template));
            Assert.Equal("An enum was expected for Field.", ex.Message);
        }

        [Fact]
        public static void TemplateCompiler_Compile_EnumQuery_Named()
        {
            var values = new Values()
            {
                Normal = NormalEnum.Second
            };

            var template = RawUriTemplate.Parse("?f={Normal.First}&s={Normal.Second=second}&t={Normal.Second}");
            Func<Values, string> compiled = TemplateCompiler.Compile<Values>(template);
            string result = compiled(values);
            Assert.Equal("?s=second&t", result);
        }

        [Fact]
        public static void TemplateCompiler_Compile_EnumQuery_Nullable()
        {
            var values = new Values()
            {
                NormalNullable = NormalEnum.First
            };

            var template = RawUriTemplate.Parse("?f={NormalNullable.First}&s={NormalNullable.Second}");
            Func<Values, string> compiled = TemplateCompiler.Compile<Values>(template);
            string result = compiled(values);
            Assert.Equal("?f", result);
        }

        [Fact]
        public static void TemplateCompiler_Compile_EnumQuery_Nullable_Null()
        {
            var values = new Values();
            var template = RawUriTemplate.Parse("?f={NormalNullable.First}&s={NormalNullable.Second}");
            Func<Values, string> compiled = TemplateCompiler.Compile<Values>(template);
            string result = compiled(values);
            Assert.Equal("", result);
        }

        [Fact]
        public static void TemplateCompiler_Compile_EnumQuery_Nullable_Named()
        {
            var values = new Values()
            {
                NormalNullable = NormalEnum.First
            };

            var template = RawUriTemplate.Parse("?f={NormalNullable.First=foo}&s={NormalNullable.Second=bar}");
            Func<Values, string> compiled = TemplateCompiler.Compile<Values>(template);
            string result = compiled(values);
            Assert.Equal("?f=foo", result);
        }

        [Fact]
        public static void TemplateCompiler_Compile_EnumQuery_Nullable_Named_Null()
        {
            var values = new Values();
            var template = RawUriTemplate.Parse("?f={NormalNullable.First=foo}&s={NormalNullable.Second=bar}");
            Func<Values, string> compiled = TemplateCompiler.Compile<Values>(template);
            string result = compiled(values);
            Assert.Equal("", result);
        }

        [Fact]
        public static void TemplateCompiler_Compile_EnumQuery_Flags()
        {
            var values = new Values()
            {
                Flags = FlagsEnum.First | FlagsEnum.Second
            };

            var template = RawUriTemplate.Parse("?f={Flags.First}&s={Flags.Second}");
            Func<Values, string> compiled = TemplateCompiler.Compile<Values>(template);
            string result = compiled(values);
            Assert.Equal("?f&s", result);
        }

        [Fact]
        public static void TemplateCompiler_Compile_EnumQuery_Flags_Named()
        {
            var values = new Values()
            {
                Flags = FlagsEnum.First | FlagsEnum.Second
            };

            var template = RawUriTemplate.Parse("?f={Flags.First=first}&s={Flags.Second=baz}");
            Func<Values, string> compiled = TemplateCompiler.Compile<Values>(template);
            string result = compiled(values);
            Assert.Equal("?f=first&s=baz", result);
        }

        [Fact]
        public static void TemplateCompiler_Compile_NotFormattable()
        {
            var values = new Values()
            {
                NotFormattable = new NotFormattable()
            };

            var template = RawUriTemplate.Parse("?f={NotFormattable}");
            Func<Values, string> compiled = TemplateCompiler.Compile<Values>(template);
            string result = compiled(values);
            Assert.Equal("?f=Foo%21", result);
        }

        [Fact]
        public static void TemplateCompiler_Compile_NotFormattable_Null()
        {
            var values = new Values();

            var template = RawUriTemplate.Parse("?f={NotFormattable}");
            Func<Values, string> compiled = TemplateCompiler.Compile<Values>(template);
            string result = compiled(values);
            Assert.Equal("", result);
        }

        [Fact]
        public static void TemplateCompiler_Compile_NotFormattable_Default()
        {
            var values = new Values();

            var template = RawUriTemplate.Parse("?f={NotFormattable=Value}");
            Func<Values, string> compiled = TemplateCompiler.Compile<Values>(template);
            string result = compiled(values);
            Assert.Equal("?f=Value", result);
        }

        [Fact]
        public static void TemplateCompiler_Compile_Formattable()
        {
            var values = new Values()
            {
                Formattable = new Formattable()
            };

            var template = RawUriTemplate.Parse("?f={Formattable:x}");
            Func<Values, string> compiled = TemplateCompiler.Compile<Values>(template);
            string result = compiled(values);
            Assert.Equal("?f=fx", result);
        }

        [Fact]
        public static void TemplateCompiler_Compile_Formattable_Null()
        {
            var values = new Values();

            var template = RawUriTemplate.Parse("?f={Formattable:x}");
            Func<Values, string> compiled = TemplateCompiler.Compile<Values>(template);
            string result = compiled(values);
            Assert.Equal("", result);
        }

        [Fact]
        public static void TemplateCompiler_Compile_Formattable_Default()
        {
            var values = new Values();

            var template = RawUriTemplate.Parse("?f={Formattable=Value}");
            Func<Values, string> compiled = TemplateCompiler.Compile<Values>(template);
            string result = compiled(values);
            Assert.Equal("?f=Value", result);
        }
    }
}
