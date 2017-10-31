#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using System;

namespace SourceCode.Clay.OpenApi
{
    /// <summary>
    /// Defines an API key security scheme that can be used by the operations.
    /// </summary>
    public class OasApiKeySecuritySchemeBuilder : OasSecuritySchemeBuilder, IOasBuilder<OasApiKeySecurityScheme>
    {
        #region Properties

        /// <summary>
        /// Gets or sets the name of the header, query or cookie parameter to be used.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the location of the API key.
        /// </summary>
        public OasParameterLocation Location { get; set; }

        /// <summary>Gets the security scheme type.</summary>
        public override OasSecuritySchemeType SchemeType => OasSecuritySchemeType.ApiKey;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of the <see cref="OasApiKeySecuritySchemeBuilder"/> class.
        /// </summary>
        public OasApiKeySecuritySchemeBuilder()
        {
        }

        /// <summary>
        /// Creates a new instance of the <see cref="OasApiKeySecuritySchemeBuilder"/> class.
        /// </summary>
        /// <param name="value">The <see cref="OasApiKeySecurityScheme"/> to copy values from.</param>
        public OasApiKeySecuritySchemeBuilder(OasApiKeySecurityScheme value)
            : base(value)
        {
            if (value is null) throw new ArgumentNullException(nameof(value));
            Name = value.Name;
            Location = value.Location;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Performs an implicit conversion from <see cref="OasApiKeySecuritySchemeBuilder"/> to <see cref="OasApiKeySecurityScheme"/>.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator OasApiKeySecurityScheme(OasApiKeySecuritySchemeBuilder builder) => (OasApiKeySecurityScheme)builder?.Build();

        /// <summary>
        /// Performs an implicit conversion from <see cref="OasApiKeySecuritySchemeBuilder"/> to <see cref="OasReferable{ApiKeySecurityScheme}"/>.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator OasReferable<OasApiKeySecurityScheme>(OasApiKeySecuritySchemeBuilder builder) => (OasApiKeySecurityScheme)builder?.Build();

        /// <summary>
        /// Performs an implicit conversion from <see cref="OasApiKeySecurityScheme"/> to <see cref="OasApiKeySecuritySchemeBuilder"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator OasApiKeySecuritySchemeBuilder(OasApiKeySecurityScheme value) => value is null ? null : new OasApiKeySecuritySchemeBuilder(value);

        /// <summary>
        /// Creates the <see cref="OasApiKeySecurityScheme"/> from this builder.
        /// </summary>
        /// <returns>The <see cref="OasApiKeySecurityScheme"/>.</returns>
        public override OasSecurityScheme Build() => new OasApiKeySecurityScheme(
            description: Description,
            name: Name,
            location: Location);

        /// <summary>
        /// Creates the <see cref="OasApiKeySecurityScheme"/> from this builder.
        /// </summary>
        /// <returns>The <see cref="OasApiKeySecurityScheme"/>.</returns>
        OasApiKeySecurityScheme IOasBuilder<OasApiKeySecurityScheme>.Build() => (OasApiKeySecurityScheme)Build();

        #endregion
    }
}
