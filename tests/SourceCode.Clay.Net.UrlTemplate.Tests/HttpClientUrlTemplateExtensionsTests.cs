using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Moq;
using Xunit;

namespace SourceCode.Clay.Net.Tests
{
    public static class HttpClientUrlTemplateExtensionsTests
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
        public static void HttpClientUrlTemplateExtensions_CreateRequestMessage_Boxed()
        {
            using (var cli = new HttpClient())
            {
                HttpRequestMessage message = cli.CreateRequestMessage(HttpMethod.Get, s_boxedTemplate, s_boxed);
                Assert.Equal(HttpMethod.Get.Method, message.Method.Method);
                Assert.Equal(Expected, message.RequestUri.ToString());
            }
        }

        [Fact]
        public static void HttpClientUrlTemplateExtensions_CreateRequestMessage_Generic()
        {
            using (var cli = new HttpClient())
            {
                HttpRequestMessage message = cli.CreateRequestMessage(HttpMethod.Get, s_genericTemplate, default);
                Assert.Equal(HttpMethod.Get.Method, message.Method.Method);
                Assert.Equal(Expected, message.RequestUri.ToString());
            }
        }

        [Fact]
        public static void HttpClientUrlTemplateExtensions_CreateRequestMessage_Boxed_Args()
        {
            Assert.Throws<ArgumentNullException>(() => HttpClientUrlTemplateExtensions
                .CreateRequestMessage(null, HttpMethod.Delete, s_boxedTemplate, null));
            Assert.Throws<ArgumentNullException>(() => HttpClientUrlTemplateExtensions
                .CreateRequestMessage(new HttpClient(), null, s_boxedTemplate, null));
            Assert.Throws<ArgumentNullException>(() => HttpClientUrlTemplateExtensions
                .CreateRequestMessage(new HttpClient(), HttpMethod.Delete, default(UrlTemplate), null));
        }

        [Fact]
        public static void HttpClientUrlTemplateExtensions_CreateRequestMessage_Generic_Args()
        {
            Assert.Throws<ArgumentNullException>(() => HttpClientUrlTemplateExtensions
                .CreateRequestMessage(null, HttpMethod.Delete, s_genericTemplate, default));
            Assert.Throws<ArgumentNullException>(() => HttpClientUrlTemplateExtensions
                .CreateRequestMessage(new HttpClient(), null, s_genericTemplate, default));
            Assert.Throws<ArgumentNullException>(() => HttpClientUrlTemplateExtensions
                .CreateRequestMessage(new HttpClient(), HttpMethod.Delete, default(UrlTemplate<Data>), default));
        }

        [Fact]
        public static async Task HttpClientUrlTemplateExtensions_DeleteAsync_Boxed()
        {
            var mock = new Mock<HttpMessageHandler>(MockBehavior.Strict);

            mock.SetupHttp(HttpMethod.Delete, Expected)
                .ReturnsAsync(req => new HttpResponseMessage(HttpStatusCode.OK))
                .Verifiable();

            using (HttpClient cli = mock.HttpClient())
            {
                await cli.DeleteAsync(s_boxedTemplate, s_boxed);
            }

            mock.VerifyAll();
        }

        [Fact]
        public static async Task HttpClientUrlTemplateExtensions_DeleteAsync_Generic()
        {
            var mock = new Mock<HttpMessageHandler>(MockBehavior.Strict);

            mock.SetupHttp(HttpMethod.Delete, Expected)
                .ReturnsAsync(req => new HttpResponseMessage(HttpStatusCode.OK))
                .Verifiable();

            using (HttpClient cli = mock.HttpClient())
            {
                await cli.DeleteAsync(s_genericTemplate, default);
            }

            mock.VerifyAll();
        }

        [Fact]
        public static void HttpClientUrlTemplateExtensions_DeleteAsync_Boxed_Args()
        {
            Assert.ThrowsAsync<ArgumentNullException>(() => HttpClientUrlTemplateExtensions
                .DeleteAsync(null, s_boxedTemplate, null));
            Assert.ThrowsAsync<ArgumentNullException>(() => HttpClientUrlTemplateExtensions
                .DeleteAsync(new HttpClient(), default(UrlTemplate), null));
        }

        [Fact]
        public static void HttpClientUrlTemplateExtensions_DeleteAsync_Generic_Args()
        {
            Assert.ThrowsAsync<ArgumentNullException>(() => HttpClientUrlTemplateExtensions
                .DeleteAsync(null, s_genericTemplate, default));
            Assert.ThrowsAsync<ArgumentNullException>(() => HttpClientUrlTemplateExtensions
                .DeleteAsync(new HttpClient(), default(UrlTemplate<Data>), default));
        }

        [Fact]
        public static async Task HttpClientUrlTemplateExtensions_GetAsync_Boxed()
        {
            var mock = new Mock<HttpMessageHandler>(MockBehavior.Strict);

            mock.SetupHttp(HttpMethod.Get, Expected)
                .ReturnsAsync(req => new HttpResponseMessage(HttpStatusCode.OK))
                .Verifiable();

            using (HttpClient cli = mock.HttpClient())
            {
                await cli.GetAsync(s_boxedTemplate, s_boxed);
            }

            mock.VerifyAll();
        }

        [Fact]
        public static async Task HttpClientUrlTemplateExtensions_GetAsync_Generic()
        {
            var mock = new Mock<HttpMessageHandler>(MockBehavior.Strict);

            mock.SetupHttp(HttpMethod.Get, Expected)
                .ReturnsAsync(req => new HttpResponseMessage(HttpStatusCode.OK))
                .Verifiable();

            using (HttpClient cli = mock.HttpClient())
            {
                await cli.GetAsync(s_genericTemplate, default);
            }

            mock.VerifyAll();
        }

        [Fact]
        public static void HttpClientUrlTemplateExtensions_GetAsync_Boxed_Args()
        {
            Assert.ThrowsAsync<ArgumentNullException>(() => HttpClientUrlTemplateExtensions
                .GetAsync(null, s_boxedTemplate, null));
            Assert.ThrowsAsync<ArgumentNullException>(() => HttpClientUrlTemplateExtensions
                .GetAsync(new HttpClient(), default(UrlTemplate), null));
        }

        [Fact]
        public static void HttpClientUrlTemplateExtensions_GetAsync_Generic_Args()
        {
            Assert.ThrowsAsync<ArgumentNullException>(() => HttpClientUrlTemplateExtensions
                .GetAsync(null, s_genericTemplate, default));
            Assert.ThrowsAsync<ArgumentNullException>(() => HttpClientUrlTemplateExtensions
                .GetAsync(new HttpClient(), default(UrlTemplate<Data>), default));
        }

        [Fact]
        public static async Task HttpClientUrlTemplateExtensions_GetAsync_Boxed_CompletionOption()
        {
            var mock = new Mock<HttpMessageHandler>(MockBehavior.Strict);

            mock.SetupHttp(HttpMethod.Get, Expected)
                .ReturnsAsync(req => new HttpResponseMessage(HttpStatusCode.OK))
                .Verifiable();

            using (HttpClient cli = mock.HttpClient())
            {
                await cli.GetAsync(s_boxedTemplate, s_boxed, HttpCompletionOption.ResponseHeadersRead);
            }

            mock.VerifyAll();
        }

        [Fact]
        public static async Task HttpClientUrlTemplateExtensions_GetAsync_Generic_CompletionOption()
        {
            var mock = new Mock<HttpMessageHandler>(MockBehavior.Strict);

            mock.SetupHttp(HttpMethod.Get, Expected)
                .ReturnsAsync(req => new HttpResponseMessage(HttpStatusCode.OK))
                .Verifiable();

            using (HttpClient cli = mock.HttpClient())
            {
                await cli.GetAsync(s_genericTemplate, default, HttpCompletionOption.ResponseHeadersRead);
            }

            mock.VerifyAll();
        }

        [Fact]
        public static void HttpClientUrlTemplateExtensions_GetAsync_Boxed_CompletionOption_Args()
        {
            Assert.ThrowsAsync<ArgumentNullException>(() => HttpClientUrlTemplateExtensions
                .GetAsync(null, s_boxedTemplate, null, HttpCompletionOption.ResponseContentRead));
            Assert.ThrowsAsync<ArgumentNullException>(() => HttpClientUrlTemplateExtensions
                .GetAsync(new HttpClient(), default(UrlTemplate), null, HttpCompletionOption.ResponseContentRead));
        }

        [Fact]
        public static void HttpClientUrlTemplateExtensions_GetAsync_Generic_CompletionOption_Args()
        {
            Assert.ThrowsAsync<ArgumentNullException>(() => HttpClientUrlTemplateExtensions
                .GetAsync(null, s_genericTemplate, default, HttpCompletionOption.ResponseContentRead));
            Assert.ThrowsAsync<ArgumentNullException>(() => HttpClientUrlTemplateExtensions
                .GetAsync(new HttpClient(), default(UrlTemplate<Data>), default, HttpCompletionOption.ResponseContentRead));
        }

        [Fact]
        public static async Task HttpClientUrlTemplateExtensions_GetByteArrayAsync_Boxed()
        {
            var mock = new Mock<HttpMessageHandler>(MockBehavior.Strict);

            mock.SetupHttp(HttpMethod.Get, Expected)
                .ReturnsAsync(req => new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new ByteArrayContent(Array.Empty<byte>())
                })
                .Verifiable();

            using (HttpClient cli = mock.HttpClient())
            {
                await cli.GetByteArrayAsync(s_boxedTemplate, s_boxed);
            }

            mock.VerifyAll();
        }

        [Fact]
        public static async Task HttpClientUrlTemplateExtensions_GetByteArrayAsync_Generic()
        {
            var mock = new Mock<HttpMessageHandler>(MockBehavior.Strict);

            mock.SetupHttp(HttpMethod.Get, Expected)
                .ReturnsAsync(req => new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new ByteArrayContent(Array.Empty<byte>())
                })
                .Verifiable();

            using (HttpClient cli = mock.HttpClient())
            {
                await cli.GetByteArrayAsync(s_genericTemplate, default);
            }

            mock.VerifyAll();
        }

        [Fact]
        public static void HttpClientUrlTemplateExtensions_GetByteArrayAsync_Boxed_Args()
        {
            Assert.ThrowsAsync<ArgumentNullException>(() => HttpClientUrlTemplateExtensions
                .GetByteArrayAsync(null, s_boxedTemplate, null));
            Assert.ThrowsAsync<ArgumentNullException>(() => HttpClientUrlTemplateExtensions
                .GetByteArrayAsync(new HttpClient(), default(UrlTemplate), null));
        }

        [Fact]
        public static void HttpClientUrlTemplateExtensions_GetByteArrayAsync_Generic_Args()
        {
            Assert.ThrowsAsync<ArgumentNullException>(() => HttpClientUrlTemplateExtensions
                .GetByteArrayAsync(null, s_genericTemplate, default));
            Assert.ThrowsAsync<ArgumentNullException>(() => HttpClientUrlTemplateExtensions
                .GetByteArrayAsync(new HttpClient(), default(UrlTemplate<Data>), default));
        }

        [Fact]
        public static async Task HttpClientUrlTemplateExtensions_GetStreamAsync_Boxed()
        {
            var mock = new Mock<HttpMessageHandler>(MockBehavior.Strict);

            mock.SetupHttp(HttpMethod.Get, Expected)
                .ReturnsAsync(req => new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StreamContent(new MemoryStream())
                })
                .Verifiable();

            using (HttpClient cli = mock.HttpClient())
            {
                await cli.GetStreamAsync(s_boxedTemplate, s_boxed);
            }

            mock.VerifyAll();
        }

        [Fact]
        public static async Task HttpClientUrlTemplateExtensions_GetStreamAsync_Generic()
        {
            var mock = new Mock<HttpMessageHandler>(MockBehavior.Strict);

            mock.SetupHttp(HttpMethod.Get, Expected)
                .ReturnsAsync(req => new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StreamContent(new MemoryStream())
                })
                .Verifiable();

            using (HttpClient cli = mock.HttpClient())
            {
                await cli.GetStreamAsync(s_genericTemplate, default);
            }

            mock.VerifyAll();
        }

        [Fact]
        public static void HttpClientUrlTemplateExtensions_GetStreamAsync_Boxed_Args()
        {
            Assert.ThrowsAsync<ArgumentNullException>(() => HttpClientUrlTemplateExtensions
                .GetStreamAsync(null, s_boxedTemplate, null));
            Assert.ThrowsAsync<ArgumentNullException>(() => HttpClientUrlTemplateExtensions
                .GetStreamAsync(new HttpClient(), default(UrlTemplate), null));
        }

        [Fact]
        public static void HttpClientUrlTemplateExtensions_GetStreamAsync_Generic_Args()
        {
            Assert.ThrowsAsync<ArgumentNullException>(() => HttpClientUrlTemplateExtensions
                .GetStreamAsync(null, s_genericTemplate, default));
            Assert.ThrowsAsync<ArgumentNullException>(() => HttpClientUrlTemplateExtensions
                .GetStreamAsync(new HttpClient(), default(UrlTemplate<Data>), default));
        }

        [Fact]
        public static async Task HttpClientUrlTemplateExtensions_GetStringAsync_Boxed()
        {
            var mock = new Mock<HttpMessageHandler>(MockBehavior.Strict);

            mock.SetupHttp(HttpMethod.Get, Expected)
                .ReturnsAsync(req => new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent("Hello")
                })
                .Verifiable();

            using (HttpClient cli = mock.HttpClient())
            {
                await cli.GetStringAsync(s_boxedTemplate, s_boxed);
            }

            mock.VerifyAll();
        }

        [Fact]
        public static async Task HttpClientUrlTemplateExtensions_GetStringAsync_Generic()
        {
            var mock = new Mock<HttpMessageHandler>(MockBehavior.Strict);

            mock.SetupHttp(HttpMethod.Get, Expected)
                .ReturnsAsync(req => new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent("Hello")
                })
                .Verifiable();

            using (HttpClient cli = mock.HttpClient())
            {
                await cli.GetStringAsync(s_genericTemplate, default);
            }

            mock.VerifyAll();
        }

        [Fact]
        public static void HttpClientUrlTemplateExtensions_GetStringAsync_Boxed_Args()
        {
            Assert.ThrowsAsync<ArgumentNullException>(() => HttpClientUrlTemplateExtensions
                .GetStringAsync(null, s_boxedTemplate, null));
            Assert.ThrowsAsync<ArgumentNullException>(() => HttpClientUrlTemplateExtensions
                .GetStringAsync(new HttpClient(), default(UrlTemplate), null));
        }

        [Fact]
        public static void HttpClientUrlTemplateExtensions_GetStringAsync_Generic_Args()
        {
            Assert.ThrowsAsync<ArgumentNullException>(() => HttpClientUrlTemplateExtensions
                .GetStringAsync(null, s_genericTemplate, default));
            Assert.ThrowsAsync<ArgumentNullException>(() => HttpClientUrlTemplateExtensions
                .GetStringAsync(new HttpClient(), default(UrlTemplate<Data>), default));
        }

        [Fact]
        public static async Task HttpClientUrlTemplateExtensions_PostAsync_Boxed()
        {
            var mock = new Mock<HttpMessageHandler>(MockBehavior.Strict);

            mock.SetupHttp(HttpMethod.Post, Expected)
                .ReturnsAsync(req => new HttpResponseMessage(HttpStatusCode.OK))
                .Verifiable();

            using (HttpClient cli = mock.HttpClient())
            {
                await cli.PostAsync(s_boxedTemplate, s_boxed, new StringContent("Test"));
            }

            mock.VerifyAll();
        }

        [Fact]
        public static async Task HttpClientUrlTemplateExtensions_PostAsync_Generic()
        {
            var mock = new Mock<HttpMessageHandler>(MockBehavior.Strict);

            mock.SetupHttp(HttpMethod.Post, Expected)
                .ReturnsAsync(req => new HttpResponseMessage(HttpStatusCode.OK))
                .Verifiable();

            using (HttpClient cli = mock.HttpClient())
            {
                await cli.PostAsync(s_genericTemplate, default, new StringContent("Test"));
            }

            mock.VerifyAll();
        }

        [Fact]
        public static void HttpClientUrlTemplateExtensions_PostAsync_Boxed_Args()
        {
            Assert.ThrowsAsync<ArgumentNullException>(() => HttpClientUrlTemplateExtensions
                .PostAsync(null, s_boxedTemplate, null, null));
            Assert.ThrowsAsync<ArgumentNullException>(() => HttpClientUrlTemplateExtensions
                .PostAsync(new HttpClient(), default(UrlTemplate), null, null));
        }

        [Fact]
        public static void HttpClientUrlTemplateExtensions_PostAsync_Generic_Args()
        {
            Assert.ThrowsAsync<ArgumentNullException>(() => HttpClientUrlTemplateExtensions
                .PostAsync(null, s_genericTemplate, default, null));
            Assert.ThrowsAsync<ArgumentNullException>(() => HttpClientUrlTemplateExtensions
                .PostAsync(new HttpClient(), default(UrlTemplate<Data>), default, null));
        }

        [Fact]
        public static async Task HttpClientUrlTemplateExtensions_PutAsync_Boxed()
        {
            var mock = new Mock<HttpMessageHandler>(MockBehavior.Strict);

            mock.SetupHttp(HttpMethod.Put, Expected)
                .ReturnsAsync(req => new HttpResponseMessage(HttpStatusCode.OK))
                .Verifiable();

            using (HttpClient cli = mock.HttpClient())
            {
                await cli.PutAsync(s_boxedTemplate, s_boxed, new StringContent("Test"));
            }

            mock.VerifyAll();
        }

        [Fact]
        public static async Task HttpClientUrlTemplateExtensions_PutAsync_Generic()
        {
            var mock = new Mock<HttpMessageHandler>(MockBehavior.Strict);

            mock.SetupHttp(HttpMethod.Put, Expected)
                .ReturnsAsync(req => new HttpResponseMessage(HttpStatusCode.OK))
                .Verifiable();

            using (HttpClient cli = mock.HttpClient())
            {
                await cli.PutAsync(s_genericTemplate, default, new StringContent("Test"));
            }

            mock.VerifyAll();
        }

        [Fact]
        public static void HttpClientUrlTemplateExtensions_PutAsync_Boxed_Args()
        {
            Assert.ThrowsAsync<ArgumentNullException>(() => HttpClientUrlTemplateExtensions
                .PutAsync(null, s_boxedTemplate, null, null));
            Assert.ThrowsAsync<ArgumentNullException>(() => HttpClientUrlTemplateExtensions
                .PutAsync(new HttpClient(), default(UrlTemplate), null, null));
        }

        [Fact]
        public static void HttpClientUrlTemplateExtensions_PutAsync_Generic_Args()
        {
            Assert.ThrowsAsync<ArgumentNullException>(() => HttpClientUrlTemplateExtensions
                .PutAsync(null, s_genericTemplate, default, null));
            Assert.ThrowsAsync<ArgumentNullException>(() => HttpClientUrlTemplateExtensions
                .PutAsync(new HttpClient(), default(UrlTemplate<Data>), default, null));
        }
    }
}
