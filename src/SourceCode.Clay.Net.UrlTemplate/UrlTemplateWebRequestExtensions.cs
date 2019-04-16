using System;
using System.Net;

namespace SourceCode.Clay.Net
{
    public static class UrlTemplateWebRequestExtensions
    {
        #region Boxed
        /// <summary>
        /// Creates a new <see cref="WebRequest"/> with a <see cref="WebRequest.RequestUri"/> specified
        /// by the given <paramref name="urlTemplate"/> and <paramref name="parameters"/>.
        /// </summary>
        /// <param name="urlTemplate">The <see cref="UrlTemplate"/>.</param>
        /// <param name="parameters">The format parameters for the <see cref="UrlTemplate"/>.</param>
        /// <returns>The new <see cref="WebRequest"/>.</returns>
        public static WebRequest CreateWebRequest(this UrlTemplate urlTemplate, object parameters)
        {
            if (urlTemplate == null) throw new ArgumentNullException(nameof(urlTemplate));
            var uri = urlTemplate.ToString(parameters);
            return WebRequest.Create(uri);
        }

        /// <summary>
        /// Creates a new <see cref="HttpWebRequest"/> with a <see cref="HttpWebRequest.RequestUri"/> specified
        /// by the given <paramref name="urlTemplate"/> and <paramref name="parameters"/>.
        /// </summary>
        /// <param name="urlTemplate">The <see cref="UrlTemplate"/>.</param>
        /// <param name="parameters">The format parameters for the <see cref="UrlTemplate"/>.</param>
        /// <returns>The new <see cref="HttpWebRequest"/>.</returns>
        public static WebRequest CreateHttpWebRequest(this UrlTemplate urlTemplate, object parameters)
        {
            if (urlTemplate == null) throw new ArgumentNullException(nameof(urlTemplate));
            var uri = urlTemplate.ToString(parameters);
            return WebRequest.CreateHttp(uri);
        }
        #endregion

        #region Generic
        /// <summary>
        /// Creates a new <see cref="WebRequest"/> with a <see cref="WebRequest.RequestUri"/> specified
        /// by the given <paramref name="urlTemplate"/> and <paramref name="parameters"/>.
        /// </summary>
        /// <typeparam name="T">The type of the format parameters for <paramref name="urlTemplate"/>.</typeparam>
        /// <param name="urlTemplate">The <see cref="UrlTemplate{T}"/>.</param>
        /// <param name="parameters">The format parameters for the <see cref="UrlTemplate{T}"/>.</param>
        /// <returns>The new <see cref="WebRequest"/>.</returns>
        public static WebRequest CreateWebRequest<T>(this UrlTemplate<T> urlTemplate, T parameters)
        {
            if (urlTemplate == null) throw new ArgumentNullException(nameof(urlTemplate));
            var uri = urlTemplate.ToString(parameters);
            return WebRequest.Create(uri);
        }

        /// <summary>
        /// Creates a new <see cref="HttpWebRequest"/> with a <see cref="HttpWebRequest.RequestUri"/> specified
        /// by the given <paramref name="urlTemplate"/> and <paramref name="parameters"/>.
        /// </summary>
        /// <typeparam name="T">The type of the format parameters for <paramref name="urlTemplate"/>.</typeparam>
        /// <param name="urlTemplate">The <see cref="UrlTemplate{T}"/>.</param>
        /// <param name="parameters">The format parameters for the <see cref="UrlTemplate{T}"/>.</param>
        /// <returns>The new <see cref="HttpWebRequest"/>.</returns>
        public static WebRequest CreateHttpWebRequest<T>(this UrlTemplate<T> urlTemplate, T parameters)
        {
            if (urlTemplate == null) throw new ArgumentNullException(nameof(urlTemplate));
            var uri = urlTemplate.ToString(parameters);
            return WebRequest.CreateHttp(uri);
        }
        #endregion
    }
}
