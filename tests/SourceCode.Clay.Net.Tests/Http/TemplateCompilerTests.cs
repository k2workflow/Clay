using System;
using Xunit;
using static System.FormattableString;

namespace SourceCode.Clay.Net.Http.Tests
{
    public static class TemplateCompilerTests
    {
        private class Formattable : IFormattable
        {
            public string ToString(string format, IFormatProvider formatProvider) => "Foo@";
        }

        private struct NotFormattable
        {
            public override string ToString() => "Foo!";
        }

        private struct Values
        {
            public int Field;
            public Guid Property { get; set; }
            public NotFormattable NotFormattable;
            public string Str;
            public Formattable Formattable;
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
        public static void TemplateCompiler_Compile_Placeholder()
        {
            var values = new Values()
            {
                Field = 100,
                Property = Guid.NewGuid()
            };

            var template = RawUriTemplate.Parse("/test/{Field}?some={Property}&nf={NotFormattable}&str={Str}&f={Formattable}");
            Func<Values, string> compiled = TemplateCompiler.Compile<Values>(template);
            string result = compiled(values);
            Assert.Equal(Invariant($"/test/{values.Field}?some={values.Property}&nf=Foo%21&str=&f="), result);
        }
    }
}
