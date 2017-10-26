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
    public struct OasResponseKey : IEquatable<OasResponseKey>
    {
        #region Well-known

        /// <summary>
        /// Gets the default response key.
        /// </summary>
        public static OasResponseKey Default { get; }

        /// <summary>Gets the response key for <see cref="HttpStatusCode.Continue"/>.</summary>
        public static OasResponseKey Continue { get; } = new OasResponseKey(100);

        /// <summary>Gets the response key for <see cref="HttpStatusCode.SwitchingProtocols"/>.</summary>
        public static OasResponseKey SwitchingProtocols { get; } = new OasResponseKey(101);

        /// <summary>Gets the response key for <see cref="HttpStatusCode.OK"/>.</summary>
        public static OasResponseKey OK { get; } = new OasResponseKey(200);

        /// <summary>Gets the response key for <see cref="HttpStatusCode.Created"/>.</summary>
        public static OasResponseKey Created { get; } = new OasResponseKey(201);

        /// <summary>Gets the response key for <see cref="HttpStatusCode.Accepted"/>.</summary>
        public static OasResponseKey Accepted { get; } = new OasResponseKey(202);

        /// <summary>Gets the response key for <see cref="HttpStatusCode.NonAuthoritativeInformation"/>.</summary>
        public static OasResponseKey NonAuthoritativeInformation { get; } = new OasResponseKey(203);

        /// <summary>Gets the response key for <see cref="HttpStatusCode.NoContent"/>.</summary>
        public static OasResponseKey NoContent { get; } = new OasResponseKey(204);

        /// <summary>Gets the response key for <see cref="HttpStatusCode.ResetContent"/>.</summary>
        public static OasResponseKey ResetContent { get; } = new OasResponseKey(205);

        /// <summary>Gets the response key for <see cref="HttpStatusCode.PartialContent"/>.</summary>
        public static OasResponseKey PartialContent { get; } = new OasResponseKey(206);

        /// <summary>Gets the response key for <see cref="HttpStatusCode.Ambiguous"/>.</summary>
        public static OasResponseKey Ambiguous { get; } = new OasResponseKey(300);

        /// <summary>Gets the response key for <see cref="HttpStatusCode.MultipleChoices"/>.</summary>
        public static OasResponseKey MultipleChoices { get; } = new OasResponseKey(300);

        /// <summary>Gets the response key for <see cref="HttpStatusCode.Moved"/>.</summary>
        public static OasResponseKey Moved { get; } = new OasResponseKey(301);

        /// <summary>Gets the response key for <see cref="HttpStatusCode.MovedPermanently"/>.</summary>
        public static OasResponseKey MovedPermanently { get; } = new OasResponseKey(301);

        /// <summary>Gets the response key for <see cref="HttpStatusCode.Found"/>.</summary>
        public static OasResponseKey Found { get; } = new OasResponseKey(302);

        /// <summary>Gets the response key for <see cref="HttpStatusCode.Redirect"/>.</summary>
        public static OasResponseKey Redirect { get; } = new OasResponseKey(302);

        /// <summary>Gets the response key for <see cref="HttpStatusCode.RedirectMethod"/>.</summary>
        public static OasResponseKey RedirectMethod { get; } = new OasResponseKey(303);

        /// <summary>Gets the response key for <see cref="HttpStatusCode.SeeOther"/>.</summary>
        public static OasResponseKey SeeOther { get; } = new OasResponseKey(303);

        /// <summary>Gets the response key for <see cref="HttpStatusCode.NotModified"/>.</summary>
        public static OasResponseKey NotModified { get; } = new OasResponseKey(304);

        /// <summary>Gets the response key for <see cref="HttpStatusCode.UseProxy"/>.</summary>
        public static OasResponseKey UseProxy { get; } = new OasResponseKey(305);

        /// <summary>Gets the response key for <see cref="HttpStatusCode.Unused"/>.</summary>
        public static OasResponseKey Unused { get; } = new OasResponseKey(306);

        /// <summary>Gets the response key for <see cref="HttpStatusCode.RedirectKeepVerb"/>.</summary>
        public static OasResponseKey RedirectKeepVerb { get; } = new OasResponseKey(307);

        /// <summary>Gets the response key for <see cref="HttpStatusCode.TemporaryRedirect"/>.</summary>
        public static OasResponseKey TemporaryRedirect { get; } = new OasResponseKey(307);

        /// <summary>Gets the response key for <see cref="HttpStatusCode.BadRequest"/>.</summary>
        public static OasResponseKey BadRequest { get; } = new OasResponseKey(400);

        /// <summary>Gets the response key for <see cref="HttpStatusCode.Unauthorized"/>.</summary>
        public static OasResponseKey Unauthorized { get; } = new OasResponseKey(401);

        /// <summary>Gets the response key for <see cref="HttpStatusCode.PaymentRequired"/>.</summary>
        public static OasResponseKey PaymentRequired { get; } = new OasResponseKey(402);

        /// <summary>Gets the response key for <see cref="HttpStatusCode.Forbidden"/>.</summary>
        public static OasResponseKey Forbidden { get; } = new OasResponseKey(403);

        /// <summary>Gets the response key for <see cref="HttpStatusCode.NotFound"/>.</summary>
        public static OasResponseKey NotFound { get; } = new OasResponseKey(404);

        /// <summary>Gets the response key for <see cref="HttpStatusCode.MethodNotAllowed"/>.</summary>
        public static OasResponseKey MethodNotAllowed { get; } = new OasResponseKey(405);

        /// <summary>Gets the response key for <see cref="HttpStatusCode.NotAcceptable"/>.</summary>
        public static OasResponseKey NotAcceptable { get; } = new OasResponseKey(406);

        /// <summary>Gets the response key for <see cref="HttpStatusCode.ProxyAuthenticationRequired"/>.</summary>
        public static OasResponseKey ProxyAuthenticationRequired { get; } = new OasResponseKey(407);

        /// <summary>Gets the response key for <see cref="HttpStatusCode.RequestTimeout"/>.</summary>
        public static OasResponseKey RequestTimeout { get; } = new OasResponseKey(408);

        /// <summary>Gets the response key for <see cref="HttpStatusCode.Conflict"/>.</summary>
        public static OasResponseKey Conflict { get; } = new OasResponseKey(409);

        /// <summary>Gets the response key for <see cref="HttpStatusCode.Gone"/>.</summary>
        public static OasResponseKey Gone { get; } = new OasResponseKey(410);

        /// <summary>Gets the response key for <see cref="HttpStatusCode.LengthRequired"/>.</summary>
        public static OasResponseKey LengthRequired { get; } = new OasResponseKey(411);

        /// <summary>Gets the response key for <see cref="HttpStatusCode.PreconditionFailed"/>.</summary>
        public static OasResponseKey PreconditionFailed { get; } = new OasResponseKey(412);

        /// <summary>Gets the response key for <see cref="HttpStatusCode.RequestEntityTooLarge"/>.</summary>
        public static OasResponseKey RequestEntityTooLarge { get; } = new OasResponseKey(413);

        /// <summary>Gets the response key for <see cref="HttpStatusCode.RequestUriTooLong"/>.</summary>
        public static OasResponseKey RequestUriTooLong { get; } = new OasResponseKey(414);

        /// <summary>Gets the response key for <see cref="HttpStatusCode.UnsupportedMediaType"/>.</summary>
        public static OasResponseKey UnsupportedMediaType { get; } = new OasResponseKey(415);

        /// <summary>Gets the response key for <see cref="HttpStatusCode.RequestedRangeNotSatisfiable"/>.</summary>
        public static OasResponseKey RequestedRangeNotSatisfiable { get; } = new OasResponseKey(416);

        /// <summary>Gets the response key for <see cref="HttpStatusCode.ExpectationFailed"/>.</summary>
        public static OasResponseKey ExpectationFailed { get; } = new OasResponseKey(417);

        /// <summary>Gets the response key for I'm a teapot.</summary>
        public static OasResponseKey ImATeapot { get; } = new OasResponseKey(418);

        /// <summary>Gets the response key for <see cref="HttpStatusCode.UpgradeRequired"/>.</summary>
        public static OasResponseKey UpgradeRequired { get; } = new OasResponseKey(426);

        /// <summary>Gets the response key for <see cref="HttpStatusCode.InternalServerError"/>.</summary>
        public static OasResponseKey InternalServerError { get; } = new OasResponseKey(500);

        /// <summary>Gets the response key for <see cref="HttpStatusCode.NotImplemented"/>.</summary>
        public static OasResponseKey NotImplemented { get; } = new OasResponseKey(501);

        /// <summary>Gets the response key for <see cref="HttpStatusCode.BadGateway"/>.</summary>
        public static OasResponseKey BadGateway { get; } = new OasResponseKey(502);

        /// <summary>Gets the response key for <see cref="HttpStatusCode.ServiceUnavailable"/>.</summary>
        public static OasResponseKey ServiceUnavailable { get; } = new OasResponseKey(503);

        /// <summary>Gets the response key for <see cref="HttpStatusCode.GatewayTimeout"/>.</summary>
        public static OasResponseKey GatewayTimeout { get; } = new OasResponseKey(504);

        /// <summary>Gets the response key for <see cref="HttpStatusCode.HttpVersionNotSupported"/>.</summary>
        public static OasResponseKey HttpVersionNotSupported { get; } = new OasResponseKey(505);

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
        /// Creates a new <see cref="OasResponseKey"/> value.
        /// </summary>
        /// <param name="httpStatusCode">The contained <see cref="System.Net.HttpStatusCode"/>.</param>
        public OasResponseKey(HttpStatusCode httpStatusCode)
        {
            _httpStatusCode = httpStatusCode;
        }

        /// <summary>
        /// Creates a new <see cref="OasResponseKey"/> value.
        /// </summary>
        /// <param name="httpStatusCode">The contained integral <see cref="System.Net.HttpStatusCode"/>.</param>
        public OasResponseKey(int httpStatusCode)
        {
            if (httpStatusCode < 100 || httpStatusCode > 999) throw new ArgumentOutOfRangeException(nameof(httpStatusCode));

            _httpStatusCode = (HttpStatusCode)httpStatusCode;
        }

        /// <summary>
        /// Creates a new <see cref="OasResponseKey"/> from the a string representation.
        /// </summary>
        /// <param name="responseKey">The response key.</param>
        public OasResponseKey(string responseKey)
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

        /// <summary>
        /// Implements the operator == operator.
        /// </summary>
        /// <param name="key1">The key1.</param>
        /// <param name="key2">The key2.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator ==(OasResponseKey key1, OasResponseKey key2) => key1.Equals(key2);

        /// <summary>
        /// Implements the operator != operator.
        /// </summary>
        /// <param name="key1">The key1.</param>
        /// <param name="key2">The key2.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator !=(OasResponseKey key1, OasResponseKey key2) => !(key1 == key2);

        /// <summary>
        /// Performs an implicit conversion from <see cref="System.Net.HttpStatusCode" /> to <see cref="SourceCode.Clay.OpenApi.OasResponseKey" />.
        /// </summary>
        /// <param name="httpStatusCode">The HTTP status code.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator OasResponseKey(HttpStatusCode httpStatusCode) => new OasResponseKey(httpStatusCode);

        /// <summary>Indicates whether this instance and a specified object are equal.</summary>
        /// <param name="obj">The object to compare with the current instance.</param>
        /// <returns>true if <paramref name="obj">obj</paramref> and this instance are the same type and represent the same value; otherwise, false.</returns>
        public override bool Equals(object obj) => obj is OasResponseKey o && Equals(o);

        /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>true if the current object is equal to the <paramref name="other">other</paramref> parameter; otherwise, false.</returns>
        public bool Equals(OasResponseKey other) => EqualityComparer<HttpStatusCode?>.Default.Equals(_httpStatusCode, other._httpStatusCode);

        /// <summary>Returns the hash code for this instance.</summary>
        /// <returns>A 32-bit signed integer that is the hash code for this instance.</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                var hc = 17L;

                hc = (hc * 23) + EqualityComparer<HttpStatusCode?>.Default.GetHashCode(_httpStatusCode);

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
