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
    public class ApiKeySecuritySchemeBuilder : SecuritySchemeBuilder, IOpenApiBuilder<ApiKeySecurityScheme>
    {
        #region Properties

        /// <summary>
        /// Gets or sets the name of the header, query or cookie parameter to be used.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the location of the API key.
        /// </summary>
        public ParameterLocation Location { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of the <see cref="ApiKeySecuritySchemeBuilder"/> class.
        /// </summary>
        public ApiKeySecuritySchemeBuilder()
        {
        }

        /// <summary>
        /// Creates a new instance of the <see cref="ApiKeySecuritySchemeBuilder"/> class.
        /// </summary>
        /// <param name="value">The <see cref="ApiKeySecurityScheme"/> to copy values from.</param>
        public ApiKeySecuritySchemeBuilder(ApiKeySecurityScheme value)
            : base(value)
        {
            if (ReferenceEquals(value, null)) throw new ArgumentNullException(nameof(value));
            Name = value.Name;
            Location = value.Location;
        }

        #endregion

        #region Methods

        public static implicit operator ApiKeySecurityScheme(ApiKeySecuritySchemeBuilder builder) => builder?.Build();

        public static implicit operator ApiKeySecuritySchemeBuilder(ApiKeySecurityScheme value) => ReferenceEquals(value, null) ? null : new ApiKeySecuritySchemeBuilder(value);

        /// <summary>
        /// Creates the <see cref="ApiKeySecurityScheme"/> from this builder.
        /// </summary>
        /// <returns>The <see cref="ApiKeySecurityScheme"/>.</returns>
        public ApiKeySecurityScheme Build() => new ApiKeySecurityScheme(
            description: Description,
            name: Name,
            location: Location);

        #endregion
    }
}
