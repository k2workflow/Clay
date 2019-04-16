using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace SourceCode.Clay.Net.Tests
{
    public static class UrlTemplateWebRequestExtensionsTests
    {
        private readonly struct Data
        {
            public int A => 1;
            public int B => 2;
            public int C => 3;
        }

        private const string Expected = "http://test/1/2/3";
        private static readonly UrlTemplate s_boxedTemplate = "http://test/{A}/{B}/{C}";
        private static readonly UrlTemplate<Data> s_genericTemplate = "http://test/{A}/{B}/{C}";
        private static readonly object s_boxed = new { A = 1, B = 2, C = 3 };

        [Fact]
        public static void UrlTemplateWebRequestExtensions_CreateWebRequest_Boxed()
        {
            System.Net.WebRequest request = s_boxedTemplate.CreateWebRequest(s_boxed);
            Assert.Equal(Expected, request.RequestUri.ToString());
        }

        [Fact]
        public static void UrlTemplateWebRequestExtensions_CreateWebRequest_Generic()
        {
            System.Net.WebRequest request = s_genericTemplate.CreateWebRequest(default);
            Assert.Equal(Expected, request.RequestUri.ToString());
        }

        [Fact]
        public static void UrlTemplateWebRequestExtensions_CreateWebRequest_Boxed_Args()
        {
            Assert.Throws<ArgumentNullException>(() => UrlTemplateWebRequestExtensions
                .CreateWebRequest(default(UrlTemplate), null));
        }

        [Fact]
        public static void UrlTemplateWebRequestExtensions_CreateWebRequest_Generic_Args()
        {
            Assert.Throws<ArgumentNullException>(() => UrlTemplateWebRequestExtensions
                .CreateWebRequest(default(UrlTemplate<Data>), default));
        }

        [Fact]
        public static void UrlTemplateWebRequestExtensions_CreateHttpWebRequest_Boxed()
        {
            System.Net.WebRequest request = s_boxedTemplate.CreateHttpWebRequest(s_boxed);
            Assert.Equal(Expected, request.RequestUri.ToString());
        }

        [Fact]
        public static void UrlTemplateWebRequestExtensions_CreateHttpWebRequest_Generic()
        {
            System.Net.WebRequest request = s_genericTemplate.CreateHttpWebRequest(default);
            Assert.Equal(Expected, request.RequestUri.ToString());
        }

        [Fact]
        public static void UrlTemplateWebRequestExtensions_CreateHttpWebRequest_Boxed_Args()
        {
            Assert.Throws<ArgumentNullException>(() => UrlTemplateWebRequestExtensions
                .CreateHttpWebRequest(default(UrlTemplate), null));
        }

        [Fact]
        public static void UrlTemplateWebRequestExtensions_CreateHttpWebRequest_Generic_Args()
        {
            Assert.Throws<ArgumentNullException>(() => UrlTemplateWebRequestExtensions
                .CreateHttpWebRequest(default(UrlTemplate<Data>), default));
        }
    }
}
