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
    public abstract class SecuritySchemeBuilder
    {
        #region Properties

        /// <summary>
        /// Gets or sets the short description for security scheme.
        /// </summary>
        /// <remarks>
        /// CommonMark syntax MAY be used for rich text representation.
        /// </remarks>
        public string Description { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of the <see cref="SecuritySchemeBuilder"/> class.
        /// </summary>
        protected SecuritySchemeBuilder()
        {
        }

        /// <summary>
        /// Creates a new instance of the <see cref="SecuritySchemeBuilder"/> class.
        /// </summary>
        /// <param name="value">The <see cref="SecurityScheme"/> to copy values from.</param>
        protected SecuritySchemeBuilder(SecurityScheme value)
        {
            if (ReferenceEquals(value, null)) throw new ArgumentNullException(nameof(value));
            Description = value.Description;
        }

        #endregion
    }
}
