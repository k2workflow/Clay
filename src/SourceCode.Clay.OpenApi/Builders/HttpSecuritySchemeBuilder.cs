#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using System;

namespace SourceCode.Clay.OpenApi
{
    /// <summary>
    /// Defines an HTTP security scheme that can be used by the operations.
    /// </summary>
    public class HttpSecuritySchemeBuilder : SecuritySchemeBuilder, IOpenApiBuilder<HttpSecurityScheme>
    {
        #region Properties

        /// <summary>
        /// Gets or sets the name of the HTTP Authorization scheme to be used in the Authorization header.
        /// </summary>
        public string Scheme { get; set; }

        /// <summary>
        /// Gets or sets the hint to the client to identify how the bearer token is formatted.
        /// </summary>
        public string BearerFormat { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of the <see cref="HttpSecuritySchemeBuilder"/> class.
        /// </summary>
        public HttpSecuritySchemeBuilder()
        {
        }

        /// <summary>
        /// Creates a new instance of the <see cref="HttpSecuritySchemeBuilder"/> class.
        /// </summary>
        /// <param name="value">The <see cref="HttpSecurityScheme"/> to copy values from.</param>
        public HttpSecuritySchemeBuilder(HttpSecurityScheme value)
            : base(value)
        {
            if (ReferenceEquals(value, null)) throw new ArgumentNullException(nameof(value));
            Scheme = value.Scheme;
            BearerFormat = value.BearerFormat;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Performs an implicit conversion from <see cref="HttpSecuritySchemeBuilder"/> to <see cref="HttpSecurityScheme"/>.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator HttpSecurityScheme(HttpSecuritySchemeBuilder builder) => builder?.Build();

        /// <summary>
        /// Performs an implicit conversion from <see cref="HttpSecuritySchemeBuilder"/> to <see cref="Referable{HttpSecurityScheme}"/>.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator Referable<HttpSecurityScheme>(HttpSecuritySchemeBuilder builder) => builder?.Build();

        /// <summary>
        /// Performs an implicit conversion from <see cref="HttpSecuritySchemeBuilder"/> to <see cref="Referable{SecurityScheme}"/>.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator Referable<SecurityScheme>(HttpSecuritySchemeBuilder builder) => builder?.Build();

        /// <summary>
        /// Performs an implicit conversion from <see cref="HttpSecurityScheme"/> to <see cref="HttpSecuritySchemeBuilder"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator HttpSecuritySchemeBuilder(HttpSecurityScheme value) => ReferenceEquals(value, null) ? null : new HttpSecuritySchemeBuilder(value);

        /// <summary>
        /// Creates the <see cref="HttpSecurityScheme"/> from this builder.
        /// </summary>
        /// <returns>The <see cref="HttpSecurityScheme"/>.</returns>
        public HttpSecurityScheme Build() => new HttpSecurityScheme(
            description: Description,
            scheme: Scheme,
            bearerFormat: BearerFormat);

        #endregion
    }
}
