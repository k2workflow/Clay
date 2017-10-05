#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;

namespace SourceCode.Clay.OpenApi
{
    /// <summary>
    /// Represents an HTTP response key.
    /// </summary>
    public struct ResponseKey : IEquatable<ResponseKey>
    {
        #region Well-known

        /// <summary>
        /// Gets the default response key.
        /// </summary>
        public static ResponseKey Default { get; }

        /// <summary>Gets the response key for <see cref="HttpStatusCode.Continue"/>.</summary>
        public static ResponseKey Continue { get; } = new ResponseKey(100);

        /// <summary>Gets the response key for <see cref="HttpStatusCode.SwitchingProtocols"/>.</summary>
        public static ResponseKey SwitchingProtocols { get; } = new ResponseKey(101);

        /// <summary>Gets the response key for <see cref="HttpStatusCode.OK"/>.</summary>
        public static ResponseKey OK { get; } = new ResponseKey(200);

        /// <summary>Gets the response key for <see cref="HttpStatusCode.Created"/>.</summary>
        public static ResponseKey Created { get; } = new ResponseKey(201);

        /// <summary>Gets the response key for <see cref="HttpStatusCode.Accepted"/>.</summary>
        public static ResponseKey Accepted { get; } = new ResponseKey(202);

        /// <summary>Gets the response key for <see cref="HttpStatusCode.NonAuthoritativeInformation"/>.</summary>
        public static ResponseKey NonAuthoritativeInformation { get; } = new ResponseKey(203);

        /// <summary>Gets the response key for <see cref="HttpStatusCode.NoContent"/>.</summary>
        public static ResponseKey NoContent { get; } = new ResponseKey(204);

        /// <summary>Gets the response key for <see cref="HttpStatusCode.ResetContent"/>.</summary>
        public static ResponseKey ResetContent { get; } = new ResponseKey(205);

        /// <summary>Gets the response key for <see cref="HttpStatusCode.PartialContent"/>.</summary>
        public static ResponseKey PartialContent { get; } = new ResponseKey(206);

        /// <summary>Gets the response key for <see cref="HttpStatusCode.Ambiguous"/>.</summary>
        public static ResponseKey Ambiguous { get; } = new ResponseKey(300);

        /// <summary>Gets the response key for <see cref="HttpStatusCode.MultipleChoices"/>.</summary>
        public static ResponseKey MultipleChoices { get; } = new ResponseKey(300);

        /// <summary>Gets the response key for <see cref="HttpStatusCode.Moved"/>.</summary>
        public static ResponseKey Moved { get; } = new ResponseKey(301);

        /// <summary>Gets the response key for <see cref="HttpStatusCode.MovedPermanently"/>.</summary>
        public static ResponseKey MovedPermanently { get; } = new ResponseKey(301);

        /// <summary>Gets the response key for <see cref="HttpStatusCode.Found"/>.</summary>
        public static ResponseKey Found { get; } = new ResponseKey(302);

        /// <summary>Gets the response key for <see cref="HttpStatusCode.Redirect"/>.</summary>
        public static ResponseKey Redirect { get; } = new ResponseKey(302);

        /// <summary>Gets the response key for <see cref="HttpStatusCode.RedirectMethod"/>.</summary>
        public static ResponseKey RedirectMethod { get; } = new ResponseKey(303);

        /// <summary>Gets the response key for <see cref="HttpStatusCode.SeeOther"/>.</summary>
        public static ResponseKey SeeOther { get; } = new ResponseKey(303);

        /// <summary>Gets the response key for <see cref="HttpStatusCode.NotModified"/>.</summary>
        public static ResponseKey NotModified { get; } = new ResponseKey(304);

        /// <summary>Gets the response key for <see cref="HttpStatusCode.UseProxy"/>.</summary>
        public static ResponseKey UseProxy { get; } = new ResponseKey(305);

        /// <summary>Gets the response key for <see cref="HttpStatusCode.Unused"/>.</summary>
        public static ResponseKey Unused { get; } = new ResponseKey(306);

        /// <summary>Gets the response key for <see cref="HttpStatusCode.RedirectKeepVerb"/>.</summary>
        public static ResponseKey RedirectKeepVerb { get; } = new ResponseKey(307);

        /// <summary>Gets the response key for <see cref="HttpStatusCode.TemporaryRedirect"/>.</summary>
        public static ResponseKey TemporaryRedirect { get; } = new ResponseKey(307);

        /// <summary>Gets the response key for <see cref="HttpStatusCode.BadRequest"/>.</summary>
        public static ResponseKey BadRequest { get; } = new ResponseKey(400);

        /// <summary>Gets the response key for <see cref="HttpStatusCode.Unauthorized"/>.</summary>
        public static ResponseKey Unauthorized { get; } = new ResponseKey(401);

        /// <summary>Gets the response key for <see cref="HttpStatusCode.PaymentRequired"/>.</summary>
        public static ResponseKey PaymentRequired { get; } = new ResponseKey(402);

        /// <summary>Gets the response key for <see cref="HttpStatusCode.Forbidden"/>.</summary>
        public static ResponseKey Forbidden { get; } = new ResponseKey(403);

        /// <summary>Gets the response key for <see cref="HttpStatusCode.NotFound"/>.</summary>
        public static ResponseKey NotFound { get; } = new ResponseKey(404);

        /// <summary>Gets the response key for <see cref="HttpStatusCode.MethodNotAllowed"/>.</summary>
        public static ResponseKey MethodNotAllowed { get; } = new ResponseKey(405);

        /// <summary>Gets the response key for <see cref="HttpStatusCode.NotAcceptable"/>.</summary>
        public static ResponseKey NotAcceptable { get; } = new ResponseKey(406);

        /// <summary>Gets the response key for <see cref="HttpStatusCode.ProxyAuthenticationRequired"/>.</summary>
        public static ResponseKey ProxyAuthenticationRequired { get; } = new ResponseKey(407);

        /// <summary>Gets the response key for <see cref="HttpStatusCode.RequestTimeout"/>.</summary>
        public static ResponseKey RequestTimeout { get; } = new ResponseKey(408);

        /// <summary>Gets the response key for <see cref="HttpStatusCode.Conflict"/>.</summary>
        public static ResponseKey Conflict { get; } = new ResponseKey(409);

        /// <summary>Gets the response key for <see cref="HttpStatusCode.Gone"/>.</summary>
        public static ResponseKey Gone { get; } = new ResponseKey(410);

        /// <summary>Gets the response key for <see cref="HttpStatusCode.LengthRequired"/>.</summary>
        public static ResponseKey LengthRequired { get; } = new ResponseKey(411);

        /// <summary>Gets the response key for <see cref="HttpStatusCode.PreconditionFailed"/>.</summary>
        public static ResponseKey PreconditionFailed { get; } = new ResponseKey(412);

        /// <summary>Gets the response key for <see cref="HttpStatusCode.RequestEntityTooLarge"/>.</summary>
        public static ResponseKey RequestEntityTooLarge { get; } = new ResponseKey(413);

        /// <summary>Gets the response key for <see cref="HttpStatusCode.RequestUriTooLong"/>.</summary>
        public static ResponseKey RequestUriTooLong { get; } = new ResponseKey(414);

        /// <summary>Gets the response key for <see cref="HttpStatusCode.UnsupportedMediaType"/>.</summary>
        public static ResponseKey UnsupportedMediaType { get; } = new ResponseKey(415);

        /// <summary>Gets the response key for <see cref="HttpStatusCode.RequestedRangeNotSatisfiable"/>.</summary>
        public static ResponseKey RequestedRangeNotSatisfiable { get; } = new ResponseKey(416);

        /// <summary>Gets the response key for <see cref="HttpStatusCode.ExpectationFailed"/>.</summary>
        public static ResponseKey ExpectationFailed { get; } = new ResponseKey(417);

        /// <summary>Gets the response key for I'm a teapot.</summary>
        public static ResponseKey ImATeapot { get; } = new ResponseKey(418);

        /// <summary>Gets the response key for <see cref="HttpStatusCode.UpgradeRequired"/>.</summary>
        public static ResponseKey UpgradeRequired { get; } = new ResponseKey(426);

        /// <summary>Gets the response key for <see cref="HttpStatusCode.InternalServerError"/>.</summary>
        public static ResponseKey InternalServerError { get; } = new ResponseKey(500);

        /// <summary>Gets the response key for <see cref="HttpStatusCode.NotImplemented"/>.</summary>
        public static ResponseKey NotImplemented { get; } = new ResponseKey(501);

        /// <summary>Gets the response key for <see cref="HttpStatusCode.BadGateway"/>.</summary>
        public static ResponseKey BadGateway { get; } = new ResponseKey(502);

        /// <summary>Gets the response key for <see cref="HttpStatusCode.ServiceUnavailable"/>.</summary>
        public static ResponseKey ServiceUnavailable { get; } = new ResponseKey(503);

        /// <summary>Gets the response key for <see cref="HttpStatusCode.GatewayTimeout"/>.</summary>
        public static ResponseKey GatewayTimeout { get; } = new ResponseKey(504);

        /// <summary>Gets the response key for <see cref="HttpStatusCode.HttpVersionNotSupported"/>.</summary>
        public static ResponseKey HttpVersionNotSupported { get; } = new ResponseKey(505);

        #endregion

        #region Fields

        private readonly HttpStatusCode? _httpStatusCode;

        #endregion

        #region Properties

        /// <summary>
        /// Gets a value indicating whether this response key is the default response.
        /// </summary>
        public bool IsDefault => !_httpStatusCode.HasValue;

        /// <summary>
        /// Gets the HTTP status code associated with this response.
        /// </summary>
        public HttpStatusCode HttpStatusCode => _httpStatusCode ?? throw new InvalidOperationException("Cannot retrieve the status code from a default ResponseKey.");

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new <see cref="ResponseKey"/> value.
        /// </summary>
        /// <param name="httpStatusCode">The contained <see cref="System.Net.HttpStatusCode"/>.</param>
        public ResponseKey(HttpStatusCode httpStatusCode)
        {
            _httpStatusCode = httpStatusCode;
        }

        /// <summary>
        /// Creates a new <see cref="ResponseKey"/> value.
        /// </summary>
        /// <param name="httpStatusCode">The contained integral <see cref="System.Net.HttpStatusCode"/>.</param>
        public ResponseKey(int httpStatusCode)
        {
            if (httpStatusCode < 100 || httpStatusCode > 999) throw new ArgumentOutOfRangeException(nameof(httpStatusCode));

            _httpStatusCode = (HttpStatusCode)httpStatusCode;
        }

        /// <summary>
        /// Creates a new <see cref="ResponseKey"/> from the a string representation.
        /// </summary>
        /// <param name="responseKey">The response key.</param>
        public ResponseKey(string responseKey)
        {
            if (string.IsNullOrEmpty(responseKey)) throw new ArgumentNullException(nameof(responseKey));

            if (StringComparer.Ordinal.Equals(responseKey, "default"))
                _httpStatusCode = default;
            else if (int.TryParse(responseKey, NumberStyles.None, CultureInfo.InvariantCulture, out var statusCode))
                _httpStatusCode = (HttpStatusCode)statusCode;
            else
                throw new FormatException("The response key is not valid.");
        }

        #endregion

        #region Methods

        public static bool operator ==(ResponseKey key1, ResponseKey key2) => key1.Equals(key2);

        public static bool operator !=(ResponseKey key1, ResponseKey key2) => !(key1 == key2);

        public static implicit operator ResponseKey(HttpStatusCode httpStatusCode) => new ResponseKey(httpStatusCode);

        /// <summary>Indicates whether this instance and a specified object are equal.</summary>
        /// <param name="obj">The object to compare with the current instance.</param>
        /// <returns>true if <paramref name="obj">obj</paramref> and this instance are the same type and represent the same value; otherwise, false.</returns>
        public override bool Equals(object obj) => obj is ResponseKey o && Equals(o);

        /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>true if the current object is equal to the <paramref name="other">other</paramref> parameter; otherwise, false.</returns>
        public bool Equals(ResponseKey other) => EqualityComparer<HttpStatusCode?>.Default.Equals(_httpStatusCode, other._httpStatusCode);

        /// <summary>Returns the hash code for this instance.</summary>
        /// <returns>A 32-bit signed integer that is the hash code for this instance.</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                var hc = 17L;
                hc = hc * 21 + EqualityComparer<HttpStatusCode?>.Default.GetHashCode(_httpStatusCode);
                return ((int)(hc >> 32)) ^ (int)hc;
            }
        }

        /// <summary>Returns the string representation of the status code.</summary>
        /// <returns>The string representation of the status code.</returns>
        public override string ToString()
            => _httpStatusCode.HasValue
            ? ((int)_httpStatusCode.Value).ToString(CultureInfo.InvariantCulture)
            : "default";

        #endregion
    }
}
