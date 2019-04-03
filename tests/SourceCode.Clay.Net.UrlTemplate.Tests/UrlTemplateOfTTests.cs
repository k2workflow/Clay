using System;
using Xunit;
using static System.FormattableString;

namespace SourceCode.Clay.Net.Tests
{
    public static class UrlTemplateOfTTests
    {
        private struct GetUserUrl
        {
            private static readonly UrlTemplate<GetUserUrl> s_template = "/api/v1/{EnvironmentID:N}/{LabelName}/{Domain}/{ID:x8}";
            public Guid EnvironmentId { get; set; }
            public string LabelName { get; set; }
            public string Domain { get; set; }
            public int ID { get; set; }
            public override string ToString() => s_template.ToString(this);
        }

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

        [Fact]
        public static void UriTemplateOfT_Sample()
        {
            var uri = new GetUserUrl()
            {
                Domain = "K2WORKFLOW",
                EnvironmentId = new Guid("77f171c3-a345-4590-a8df-09ff4170db40"),
                ID = 1,
                LabelName = "K2/"
            }.ToString();

            Assert.Equal("/api/v1/77f171c3a3454590a8df09ff4170db40/K2%2F/K2WORKFLOW/00000001", uri);
        }
    }
}
