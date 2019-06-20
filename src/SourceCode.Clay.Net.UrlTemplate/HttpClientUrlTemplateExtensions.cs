using System;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace SourceCode.Clay.Net
{
    /// <summary>
    /// Represents extension methods for interacting with <see cref="HttpClient"/> and <see cref="UrlTemplate"/>
    /// or <see cref="UrlTemplate{T}"/>.
    /// </summary>
    public static class HttpClientUrlTemplateExtensions
    {
        #region Boxed
        /// <summary>
        /// Send a DELETE request to the specified Uri with a cancellation token as an asynchronous
        /// operation.
        /// </summary>
        /// <param name="httpClient">The HTTP client to perform the request on.</param>
        /// <param name="urlTemplate">The <see cref="UrlTemplate"/>.</param>
        /// <param name="parameters">The format parameters for the <see cref="UrlTemplate"/>.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public static Task<HttpResponseMessage> DeleteAsync(this HttpClient httpClient, UrlTemplate urlTemplate, object parameters, CancellationToken cancellationToken = default)
        {
            if (httpClient == null) throw new ArgumentNullException(nameof(httpClient));
            if (urlTemplate == null) throw new ArgumentNullException(nameof(urlTemplate));
            var uri = urlTemplate.ToString(parameters);
            return httpClient.DeleteAsync(new Uri(uri), cancellationToken);
        }

        /// <summary>
        /// Send a GET request to the specified Uri with a cancellation token as an asynchronous
        /// operation.
        /// </summary>
        /// <param name="httpClient">The HTTP client to perform the request on.</param>
        /// <param name="urlTemplate">The <see cref="UrlTemplate"/>.</param>
        /// <param name="parameters">The format parameters for the <see cref="UrlTemplate"/>.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public static Task<HttpResponseMessage> GetAsync(this HttpClient httpClient, UrlTemplate urlTemplate, object parameters, CancellationToken cancellationToken = default)
        {
            if (httpClient == null) throw new ArgumentNullException(nameof(httpClient));
            if (urlTemplate == null) throw new ArgumentNullException(nameof(urlTemplate));
            var uri = urlTemplate.ToString(parameters);
            return httpClient.GetAsync(new Uri(uri), cancellationToken);
        }

        /// <summary>
        /// Send a GET request to the specified Uri with a cancellation token as an asynchronous
        /// operation.
        /// </summary>
        /// <param name="httpClient">The HTTP client to perform the request on.</param>
        /// <param name="urlTemplate">The <see cref="UrlTemplate"/>.</param>
        /// <param name="parameters">The format parameters for the <see cref="UrlTemplate"/>.</param>
        /// <param name="completionOption">An HTTP completion option value that indicates when the operation should be considered completed.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public static Task<HttpResponseMessage> GetAsync(this HttpClient httpClient, UrlTemplate urlTemplate, object parameters, HttpCompletionOption completionOption, CancellationToken cancellationToken = default)
        {
            if (httpClient == null) throw new ArgumentNullException(nameof(httpClient));
            if (urlTemplate == null) throw new ArgumentNullException(nameof(urlTemplate));
            var uri = urlTemplate.ToString(parameters);
            return httpClient.GetAsync(new Uri(uri), completionOption, cancellationToken);
        }

        /// <summary>
        /// Send a GET request to the specified Uri with a cancellation token as an asynchronous
        /// operation, and returns the resulting response as a byte array.
        /// </summary>
        /// <param name="httpClient">The HTTP client to perform the request on.</param>
        /// <param name="urlTemplate">The <see cref="UrlTemplate"/>.</param>
        /// <param name="parameters">The format parameters for the <see cref="UrlTemplate"/>.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public static Task<byte[]> GetByteArrayAsync(this HttpClient httpClient, UrlTemplate urlTemplate, object parameters)
        {
            if (httpClient == null) throw new ArgumentNullException(nameof(httpClient));
            if (urlTemplate == null) throw new ArgumentNullException(nameof(urlTemplate));
            var uri = urlTemplate.ToString(parameters);
            return httpClient.GetByteArrayAsync(new Uri(uri));
        }

        /// <summary>
        /// Send a GET request to the specified Uri with a cancellation token as an asynchronous
        /// operation, and returns the resulting response as a <see cref="Stream"/>.
        /// </summary>
        /// <param name="httpClient">The HTTP client to perform the request on.</param>
        /// <param name="urlTemplate">The <see cref="UrlTemplate"/>.</param>
        /// <param name="parameters">The format parameters for the <see cref="UrlTemplate"/>.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public static Task<Stream> GetStreamAsync(this HttpClient httpClient, UrlTemplate urlTemplate, object parameters)
        {
            if (httpClient == null) throw new ArgumentNullException(nameof(httpClient));
            if (urlTemplate == null) throw new ArgumentNullException(nameof(urlTemplate));
            var uri = urlTemplate.ToString(parameters);
            return httpClient.GetStreamAsync(new Uri(uri));
        }

        /// <summary>
        /// Send a GET request to the specified Uri with a cancellation token as an asynchronous
        /// operation, and returns the resulting response as a <see cref="string"/>.
        /// </summary>
        /// <param name="httpClient">The HTTP client to perform the request on.</param>
        /// <param name="urlTemplate">The <see cref="UrlTemplate"/>.</param>
        /// <param name="parameters">The format parameters for the <see cref="UrlTemplate"/>.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public static Task<string> GetStringAsync(this HttpClient httpClient, UrlTemplate urlTemplate, object parameters)
        {
            if (httpClient == null) throw new ArgumentNullException(nameof(httpClient));
            if (urlTemplate == null) throw new ArgumentNullException(nameof(urlTemplate));
            var uri = urlTemplate.ToString(parameters);
            return httpClient.GetStringAsync(new Uri(uri));
        }

        /// <summary>
        /// Send a POST request to the specified Uri with a cancellation token as an asynchronous
        /// operation.
        /// </summary>
        /// <param name="httpClient">The HTTP client to perform the request on.</param>
        /// <param name="urlTemplate">The <see cref="UrlTemplate"/>.</param>
        /// <param name="parameters">The format parameters for the <see cref="UrlTemplate"/>.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public static Task<HttpResponseMessage> PostAsync(this HttpClient httpClient, UrlTemplate urlTemplate, object parameters, HttpContent content, CancellationToken cancellationToken = default)
        {
            if (httpClient == null) throw new ArgumentNullException(nameof(httpClient));
            if (urlTemplate == null) throw new ArgumentNullException(nameof(urlTemplate));
            var uri = urlTemplate.ToString(parameters);
            return httpClient.PostAsync(new Uri(uri), content, cancellationToken);
        }

        /// <summary>
        /// Send a PUT request to the specified Uri with a cancellation token as an asynchronous
        /// operation.
        /// </summary>
        /// <param name="httpClient">The HTTP client to perform the request on.</param>
        /// <param name="urlTemplate">The <see cref="UrlTemplate"/>.</param>
        /// <param name="parameters">The format parameters for the <see cref="UrlTemplate"/>.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public static Task<HttpResponseMessage> PutAsync(this HttpClient httpClient, UrlTemplate urlTemplate, object parameters, HttpContent content, CancellationToken cancellationToken = default)
        {
            if (httpClient == null) throw new ArgumentNullException(nameof(httpClient));
            if (urlTemplate == null) throw new ArgumentNullException(nameof(urlTemplate));
            var uri = urlTemplate.ToString(parameters);
            return httpClient.PutAsync(new Uri(uri), content, cancellationToken);
        }

        /// <summary>
        /// Creates a <see cref="HttpRequestMessage"/> for the specified <see cref="HttpClient"/>.
        /// </summary>
        /// <param name="httpClient">The HTTP client to perform the request on.</param>
        /// <param name="method">The HTTP method.</param>
        /// <param name="urlTemplate">The <see cref="UrlTemplate"/>.</param>
        /// <param name="parameters">The format parameters for the <see cref="UrlTemplate"/>.</param>
        /// <returns>The <see cref="HttpRequestMessage"/>.</returns>
        public static HttpRequestMessage CreateRequestMessage(this HttpClient httpClient, HttpMethod method, UrlTemplate urlTemplate, object parameters)
        {
            if (httpClient == null) throw new ArgumentNullException(nameof(httpClient));
            if (method == null) throw new ArgumentNullException(nameof(method));
            if (urlTemplate == null) throw new ArgumentNullException(nameof(urlTemplate));
            var uri = urlTemplate.ToString(parameters);
            return new HttpRequestMessage(method, new Uri(uri));
        }
        #endregion

        #region Generic
        /// <summary>
        /// Send a DELETE request to the specified Uri with a cancellation token as an asynchronous
        /// operation.
        /// </summary>
        /// <typeparam name="T">The type of the format parameters for <paramref name="urlTemplate"/>.</typeparam>
        /// <param name="httpClient">The HTTP client to perform the request on.</param>
        /// <param name="urlTemplate">The <see cref="UrlTemplate{T}"/>.</param>
        /// <param name="parameters">The format parameters for the <see cref="UrlTemplate{T}"/>.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public static Task<HttpResponseMessage> DeleteAsync<T>(this HttpClient httpClient, UrlTemplate<T> urlTemplate, T parameters, CancellationToken cancellationToken = default)
        {
            if (httpClient == null) throw new ArgumentNullException(nameof(httpClient));
            if (urlTemplate == null) throw new ArgumentNullException(nameof(urlTemplate));
            var uri = urlTemplate.ToString(parameters);
            return httpClient.DeleteAsync(new Uri(uri), cancellationToken);
        }

        /// <summary>
        /// Send a GET request to the specified Uri with a cancellation token as an asynchronous
        /// operation.
        /// </summary>
        /// <typeparam name="T">The type of the format parameters for <paramref name="urlTemplate"/>.</typeparam>
        /// <param name="httpClient">The HTTP client to perform the request on.</param>
        /// <param name="urlTemplate">The <see cref="UrlTemplate{T}"/>.</param>
        /// <param name="parameters">The format parameters for the <see cref="UrlTemplate{T}"/>.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public static Task<HttpResponseMessage> GetAsync<T>(this HttpClient httpClient, UrlTemplate<T> urlTemplate, T parameters, CancellationToken cancellationToken = default)
        {
            if (httpClient == null) throw new ArgumentNullException(nameof(httpClient));
            if (urlTemplate == null) throw new ArgumentNullException(nameof(urlTemplate));
            var uri = urlTemplate.ToString(parameters);
            return httpClient.GetAsync(new Uri(uri), cancellationToken);
        }

        /// <summary>
        /// Send a GET request to the specified Uri with a cancellation token as an asynchronous
        /// operation.
        /// </summary>
        /// <typeparam name="T">The type of the format parameters for <paramref name="urlTemplate"/>.</typeparam>
        /// <param name="httpClient">The HTTP client to perform the request on.</param>
        /// <param name="urlTemplate">The <see cref="UrlTemplate{T}"/>.</param>
        /// <param name="parameters">The format parameters for the <see cref="UrlTemplate{T}"/>.</param>
        /// <param name="completionOption">An HTTP completion option value that indicates when the operation should be considered completed.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public static Task<HttpResponseMessage> GetAsync<T>(this HttpClient httpClient, UrlTemplate<T> urlTemplate, T parameters, HttpCompletionOption completionOption, CancellationToken cancellationToken = default)
        {
            if (httpClient == null) throw new ArgumentNullException(nameof(httpClient));
            if (urlTemplate == null) throw new ArgumentNullException(nameof(urlTemplate));
            var uri = urlTemplate.ToString(parameters);
            return httpClient.GetAsync(new Uri(uri), completionOption, cancellationToken);
        }

        /// <summary>
        /// Send a GET request to the specified Uri with a cancellation token as an asynchronous
        /// operation, and returns the resulting response as a byte array.
        /// </summary>
        /// <typeparam name="T">The type of the format parameters for <paramref name="urlTemplate"/>.</typeparam>
        /// <param name="httpClient">The HTTP client to perform the request on.</param>
        /// <param name="urlTemplate">The <see cref="UrlTemplate{T}"/>.</param>
        /// <param name="parameters">The format parameters for the <see cref="UrlTemplate{T}"/>.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public static Task<byte[]> GetByteArrayAsync<T>(this HttpClient httpClient, UrlTemplate<T> urlTemplate, T parameters)
        {
            if (httpClient == null) throw new ArgumentNullException(nameof(httpClient));
            if (urlTemplate == null) throw new ArgumentNullException(nameof(urlTemplate));
            var uri = urlTemplate.ToString(parameters);
            return httpClient.GetByteArrayAsync(new Uri(uri));
        }

        /// <summary>
        /// Send a GET request to the specified Uri with a cancellation token as an asynchronous
        /// operation, and returns the resulting response as a <see cref="Stream"/>.
        /// </summary>
        /// <typeparam name="T">The type of the format parameters for <paramref name="urlTemplate"/>.</typeparam>
        /// <param name="httpClient">The HTTP client to perform the request on.</param>
        /// <param name="urlTemplate">The <see cref="UrlTemplate{T}"/>.</param>
        /// <param name="parameters">The format parameters for the <see cref="UrlTemplate{T}"/>.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public static Task<Stream> GetStreamAsync<T>(this HttpClient httpClient, UrlTemplate<T> urlTemplate, T parameters)
        {
            if (httpClient == null) throw new ArgumentNullException(nameof(httpClient));
            if (urlTemplate == null) throw new ArgumentNullException(nameof(urlTemplate));
            var uri = urlTemplate.ToString(parameters);
            return httpClient.GetStreamAsync(new Uri(uri));
        }

        /// <summary>
        /// Send a GET request to the specified Uri with a cancellation token as an asynchronous
        /// operation, and returns the resulting response as a <see cref="string"/>.
        /// </summary>
        /// <typeparam name="T">The type of the format parameters for <paramref name="urlTemplate"/>.</typeparam>
        /// <param name="httpClient">The HTTP client to perform the request on.</param>
        /// <param name="urlTemplate">The <see cref="UrlTemplate{T}"/>.</param>
        /// <param name="parameters">The format parameters for the <see cref="UrlTemplate{T}"/>.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public static Task<string> GetStringAsync<T>(this HttpClient httpClient, UrlTemplate<T> urlTemplate, T parameters)
        {
            if (httpClient == null) throw new ArgumentNullException(nameof(httpClient));
            if (urlTemplate == null) throw new ArgumentNullException(nameof(urlTemplate));
            var uri = urlTemplate.ToString(parameters);
            return httpClient.GetStringAsync(new Uri(uri));
        }

        /// <summary>
        /// Send a POST request to the specified Uri with a cancellation token as an asynchronous
        /// operation.
        /// </summary>
        /// <typeparam name="T">The type of the format parameters for <paramref name="urlTemplate"/>.</typeparam>
        /// <param name="httpClient">The HTTP client to perform the request on.</param>
        /// <param name="urlTemplate">The <see cref="UrlTemplate{T}"/>.</param>
        /// <param name="parameters">The format parameters for the <see cref="UrlTemplate{T}"/>.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public static Task<HttpResponseMessage> PostAsync<T>(this HttpClient httpClient, UrlTemplate<T> urlTemplate, T parameters, HttpContent content, CancellationToken cancellationToken = default)
        {
            if (httpClient == null) throw new ArgumentNullException(nameof(httpClient));
            if (urlTemplate == null) throw new ArgumentNullException(nameof(urlTemplate));
            var uri = urlTemplate.ToString(parameters);
            return httpClient.PostAsync(new Uri(uri), content, cancellationToken);
        }

        /// <summary>
        /// Send a PUT request to the specified Uri with a cancellation token as an asynchronous
        /// operation.
        /// </summary>
        /// <typeparam name="T">The type of the format parameters for <paramref name="urlTemplate"/>.</typeparam>
        /// <param name="httpClient">The HTTP client to perform the request on.</param>
        /// <param name="urlTemplate">The <see cref="UrlTemplate{T}"/>.</param>
        /// <param name="parameters">The format parameters for the <see cref="UrlTemplate{T}"/>.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public static Task<HttpResponseMessage> PutAsync<T>(this HttpClient httpClient, UrlTemplate<T> urlTemplate, T parameters, HttpContent content, CancellationToken cancellationToken = default)
        {
            if (httpClient == null) throw new ArgumentNullException(nameof(httpClient));
            if (urlTemplate == null) throw new ArgumentNullException(nameof(urlTemplate));
            var uri = urlTemplate.ToString(parameters);
            return httpClient.PutAsync(new Uri(uri), content, cancellationToken);
        }

        /// <summary>
        /// Creates a <see cref="HttpRequestMessage"/> for the specified <see cref="HttpClient"/>.
        /// </summary>
        /// <typeparam name="T">The type of the format parameters for <paramref name="urlTemplate"/>.</typeparam>
        /// <param name="httpClient">The HTTP client to perform the request on.</param>
        /// <param name="method">The HTTP method.</param>
        /// <param name="urlTemplate">The <see cref="UrlTemplate{T}"/>.</param>
        /// <param name="parameters">The format parameters for the <see cref="UrlTemplate{T}"/>.</param>
        /// <returns>The <see cref="HttpRequestMessage"/>.</returns>
        public static HttpRequestMessage CreateRequestMessage<T>(this HttpClient httpClient, HttpMethod method, UrlTemplate<T> urlTemplate, T parameters)
        {
            if (httpClient == null) throw new ArgumentNullException(nameof(httpClient));
            if (method == null) throw new ArgumentNullException(nameof(method));
            if (urlTemplate == null) throw new ArgumentNullException(nameof(urlTemplate));
            var uri = urlTemplate.ToString(parameters);
            return new HttpRequestMessage(method, new Uri(uri));
        }
        #endregion
    }
}
