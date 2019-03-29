using System;
using Xunit;
using static System.FormattableString;

namespace SourceCode.Clay.Net.Http.Tests
{
    public static class UrlTemplateTests
    {
        [Fact]
        public static void UriTemplate_Implicit()
        {
            UrlTemplate template = "implicit";
            Assert.Equal("implicit", template.Template);
        }

        [Fact]
        public static void UriTemplate_ToString_0()
        {
            UrlTemplate template = "implicit";
            Assert.Equal("implicit", template.ToString());
        }

        [Fact]
        public static void UriTemplate_ToString_1()
        {
            DateTime dt = DateTime.Now;
            UrlTemplate template = "{Year}";
            Assert.Equal(Invariant($"{dt.Year}"), template.ToString(dt));
        }

        [Fact]
        public static void UriTemplate_Ctor_Null()
        {
            Assert.Throws<ArgumentNullException>(() => new UrlTemplate(null));
        }
    }
}
