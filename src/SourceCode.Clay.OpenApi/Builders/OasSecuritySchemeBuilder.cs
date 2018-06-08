#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using System;

namespace SourceCode.Clay.OpenApi
{
    /// <summary>
    /// Defines a security scheme that can be used by the operations.
    /// </summary>
    public abstract class OasSecuritySchemeBuilder : IOasBuilder<OasSecurityScheme>
    {
        /// <summary>
        /// Gets the security scheme type.
        /// </summary>
        public abstract OasSecuritySchemeType SchemeType { get; }

        /// <summary>
        /// Gets or sets the short description for security scheme.
        /// </summary>
        /// <remarks>
        /// CommonMark syntax MAY be used for rich text representation.
        /// </remarks>
        public string Description { get; set; }

        /// <summary>
        /// Creates a new instance of the <see cref="OasSecuritySchemeBuilder"/> class.
        /// </summary>
        protected OasSecuritySchemeBuilder()
        {
        }

        /// <summary>
        /// Creates a new instance of the <see cref="OasSecuritySchemeBuilder"/> class.
        /// </summary>
        /// <param name="value">The <see cref="OasSecurityScheme"/> to copy values from.</param>
        protected OasSecuritySchemeBuilder(OasSecurityScheme value)
        {
            if (value is null) throw new ArgumentNullException(nameof(value));
            Description = value.Description;
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="OasSecuritySchemeBuilder"/> to <see cref="OasSecurityScheme"/>.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator OasSecurityScheme(OasSecuritySchemeBuilder builder) => builder?.Build();

        /// <summary>
        /// Performs an implicit conversion from <see cref="OasSecuritySchemeBuilder"/> to <see cref="OasReferable{OasSecurityScheme}"/>.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator OasReferable<OasSecurityScheme>(OasSecuritySchemeBuilder builder) => builder?.Build();

        /// <summary>
        /// Creates the <see cref="OasSecurityScheme"/> from this builder.
        /// </summary>
        /// <returns>The <see cref="OasSecurityScheme"/>.</returns>
        public abstract OasSecurityScheme Build();
    }
}
