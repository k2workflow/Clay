using System;
using System.Linq.Expressions;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Moq.Language.Flow;
using Moq.Protected;

namespace SourceCode.Clay.Net.Tests
{
    internal static class HttpMessageHandlerMocks
    {
        public static HttpClient HttpClient(this Mock<HttpMessageHandler> mock)
            => new HttpClient(mock.Object, false);

        public static ISetup<HttpMessageHandler, Task<HttpResponseMessage>> SetupHttp(this Mock<HttpMessageHandler> mock, Expression requestMessage = null, Expression cancellationToken = null)
            => mock
                .Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", requestMessage ?? ItExpr.IsAny<HttpRequestMessage>(), cancellationToken ?? ItExpr.IsAny<CancellationToken>());

        public static ISetup<HttpMessageHandler, Task<HttpResponseMessage>> SetupHttp(this Mock<HttpMessageHandler> mock, HttpMethod method, string requestUri, Expression cancellationToken = null)
            => SetupHttp(mock, ItExpr.Is<HttpRequestMessage>(x => x.Method.Method == method.Method && x.RequestUri.ToString() == requestUri), cancellationToken);

        public static IReturnsThrows<HttpMessageHandler, Task<HttpResponseMessage>> Callback(this ISetup<HttpMessageHandler, Task<HttpResponseMessage>> setup, Action<HttpRequestMessage, CancellationToken> action)
            => setup.Callback(action);

        public static IReturnsThrows<HttpMessageHandler, Task<HttpResponseMessage>> Callback(this ISetup<HttpMessageHandler, Task<HttpResponseMessage>> setup, Action<HttpRequestMessage> action)
            => Callback(setup, (r, c) => action(r));

        public static IReturnsResult<HttpMessageHandler> Returns(this ISetup<HttpMessageHandler, Task<HttpResponseMessage>> setup, Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> func)
            => setup.Returns(func);

        public static IReturnsResult<HttpMessageHandler> Returns(this ISetup<HttpMessageHandler, Task<HttpResponseMessage>> setup, Func<HttpRequestMessage, Task<HttpResponseMessage>> func)
            => Returns(setup, (r, c) => func(r));

        public static IReturnsResult<HttpMessageHandler> ReturnsAsync(this ISetup<HttpMessageHandler, Task<HttpResponseMessage>> setup, Func<HttpRequestMessage, CancellationToken, HttpResponseMessage> func)
            => setup.ReturnsAsync<HttpRequestMessage, CancellationToken, HttpMessageHandler, HttpResponseMessage>(func);

        public static IReturnsResult<HttpMessageHandler> ReturnsAsync(this ISetup<HttpMessageHandler, Task<HttpResponseMessage>> setup, Func<HttpRequestMessage, HttpResponseMessage> func)
            => ReturnsAsync(setup, (r, c) => func(r));
    }
}
