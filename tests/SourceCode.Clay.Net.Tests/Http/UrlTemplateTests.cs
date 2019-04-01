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

        [Fact]
        public static void UriTemplate_Sample()
        {
            UrlTemplate template = "/api/v1/{EnvironmentID:N}/{LabelName}/{Domain}/{ID:x8}";
            var uri = template.ToString(new
            {
                Domain = "K2WORKFLOW",
                EnvironmentId = new Guid("77f171c3-a345-4590-a8df-09ff4170db40"),
                ID = 1,
                LabelName = "K2/"
            });

            Assert.Equal("/api/v1/77f171c3a3454590a8df09ff4170db40/K2%2F/K2WORKFLOW/00000001", uri);
        }
    }
}
