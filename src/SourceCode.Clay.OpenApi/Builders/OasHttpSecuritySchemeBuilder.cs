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
    public class OasHttpSecuritySchemeBuilder : OasSecuritySchemeBuilder, IOasBuilder<OasHttpSecurityScheme>
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

        /// <summary>Gets the security scheme type.</summary>
        public override OasSecuritySchemeType SchemeType => OasSecuritySchemeType.Http;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of the <see cref="OasHttpSecuritySchemeBuilder"/> class.
        /// </summary>
        public OasHttpSecuritySchemeBuilder()
        {
        }

        /// <summary>
        /// Creates a new instance of the <see cref="OasHttpSecuritySchemeBuilder"/> class.
        /// </summary>
        /// <param name="value">The <see cref="OasHttpSecurityScheme"/> to copy values from.</param>
        public OasHttpSecuritySchemeBuilder(OasHttpSecurityScheme value)
            : base(value)
        {
            if (value is null) throw new ArgumentNullException(nameof(value));
            Scheme = value.Scheme;
            BearerFormat = value.BearerFormat;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Performs an implicit conversion from <see cref="OasHttpSecuritySchemeBuilder"/> to <see cref="OasHttpSecurityScheme"/>.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator OasHttpSecurityScheme(OasHttpSecuritySchemeBuilder builder) => (OasHttpSecurityScheme)builder?.Build();

        /// <summary>
        /// Performs an implicit conversion from <see cref="OasHttpSecuritySchemeBuilder"/> to <see cref="OasReferable{HttpSecurityScheme}"/>.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator OasReferable<OasHttpSecurityScheme>(OasHttpSecuritySchemeBuilder builder) => (OasHttpSecurityScheme)builder?.Build();

        /// <summary>
        /// Performs an implicit conversion from <see cref="OasHttpSecurityScheme"/> to <see cref="OasHttpSecuritySchemeBuilder"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator OasHttpSecuritySchemeBuilder(OasHttpSecurityScheme value) => value is null ? null : new OasHttpSecuritySchemeBuilder(value);

        /// <summary>
        /// Creates the <see cref="OasHttpSecurityScheme"/> from this builder.
        /// </summary>
        /// <returns>The <see cref="OasHttpSecurityScheme"/>.</returns>
        public override OasSecurityScheme Build() => new OasHttpSecurityScheme(
            description: Description,
            scheme: Scheme,
            bearerFormat: BearerFormat);

        /// <summary>
        /// Creates the <see cref="OasHttpSecurityScheme"/> from this builder.
        /// </summary>
        /// <returns>The <see cref="OasHttpSecurityScheme"/>.</returns>
        OasHttpSecurityScheme IOasBuilder<OasHttpSecurityScheme>.Build() => (OasHttpSecurityScheme)Build();

        #endregion
    }
}
