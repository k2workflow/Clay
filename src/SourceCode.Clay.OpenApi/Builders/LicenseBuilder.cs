#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using System;

namespace SourceCode.Clay.OpenApi
{
    /// <summary>
    /// License information for the exposed API.
    /// </summary>
    public class LicenseBuilder : IOpenApiBuilder<License>
    {
        #region Properties

        /// <summary>
        /// Gets or sets the license name used for the API.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the URL to the license used for the API
        /// </summary>
        public Uri Url { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of the <see cref="LicenseBuilder"/> class.
        /// </summary>
        public LicenseBuilder()
        {
        }

        /// <summary>
        /// Creates a new instance of the <see cref="LicenseBuilder"/> class.
        /// </summary>
        /// <param name="value">The <see cref="License"/> to copy values from.</param>
        public LicenseBuilder(License value)
        {
            if (value is null) throw new ArgumentNullException(nameof(value));
            Name = value.Name;
            Url = value.Url;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Performs an implicit conversion from <see cref="LicenseBuilder"/> to <see cref="License"/>.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator License(LicenseBuilder builder) => builder?.Build();

        /// <summary>
        /// Performs an implicit conversion from <see cref="License"/> to <see cref="LicenseBuilder"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator LicenseBuilder(License value) => value is null ? null : new LicenseBuilder(value);

        /// <summary>
        /// Creates the <see cref="License"/> from this builder.
        /// </summary>
        /// <returns>The <see cref="License"/>.</returns>
        public License Build() => new License(
            name: Name,
            url: Url);

        #endregion
    }
}
