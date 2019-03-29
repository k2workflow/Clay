using System;
using Xunit;
using static System.FormattableString;

namespace SourceCode.Clay.Net.Http.Tests
{
    public static class UrlTemplateOfTTests
    {
        [Fact]
        public static void UriTemplateOfT_Implicit()
        {
            UrlTemplate<object> template = "implicit";
            Assert.Equal("implicit", template.Template);
        }

        [Fact]
        public static void UriTemplateOfT_ToString_0()
        {
            UrlTemplate<object> template = "implicit";
            Assert.Equal("implicit", template.ToString());
        }

        [Fact]
        public static void UriTemplateOfT_ToString_1()
        {
            DateTime dt = DateTime.Now;
            UrlTemplate<DateTime> template = "{Year}";
            Assert.Equal(Invariant($"{dt.Year}"), template.ToString(dt));
        }

        [Fact]
        public static void UriTemplateOfT_Ctor_Null()
        {
            Assert.Throws<ArgumentNullException>(() => new UrlTemplate<object>(null));
        }
    }
}
