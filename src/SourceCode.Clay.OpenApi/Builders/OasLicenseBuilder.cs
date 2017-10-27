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
    public class OasLicenseBuilder : IOasBuilder<OasLicense>
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
        /// Creates a new instance of the <see cref="OasLicenseBuilder"/> class.
        /// </summary>
        public OasLicenseBuilder()
        {
        }

        /// <summary>
        /// Creates a new instance of the <see cref="OasLicenseBuilder"/> class.
        /// </summary>
        /// <param name="value">The <see cref="OasLicense"/> to copy values from.</param>
        public OasLicenseBuilder(OasLicense value)
        {
            if (value is null) throw new ArgumentNullException(nameof(value));
            Name = value.Name;
            Url = value.Url;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Performs an implicit conversion from <see cref="OasLicenseBuilder"/> to <see cref="OasLicense"/>.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator OasLicense(OasLicenseBuilder builder) => builder?.Build();

        /// <summary>
        /// Performs an implicit conversion from <see cref="OasLicense"/> to <see cref="OasLicenseBuilder"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator OasLicenseBuilder(OasLicense value) => value is null ? null : new OasLicenseBuilder(value);

        /// <summary>
        /// Creates the <see cref="OasLicense"/> from this builder.
        /// </summary>
        /// <returns>The <see cref="OasLicense"/>.</returns>
        public OasLicense Build() => new OasLicense(
            name: Name,
            url: Url);

        #endregion
    }
}
