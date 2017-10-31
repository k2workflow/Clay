#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using System;

namespace SourceCode.Clay.OpenApi
{
    /// <summary>
    /// Contains an example of how to use an entity in an Open API document.
    /// </summary>
    public class OasExampleBuilder : IOasBuilder<OasExample>
    {
        #region Properties

        /// <summary>
        /// Gets or sets the short description for the example.
        /// </summary>
        public string Summary { get; set; }

        /// <summary>
        /// Gets or sets the long description for the example.
        /// </summary>
        /// <remarks>CommonMark syntax MAY be used for rich text representation.</remarks>
        public string Description { get; set; }

        /// <summary>
        /// Gets the URL that points to the literal example.
        /// </summary>
        public Uri ExternalValue { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of the <see cref="OasExampleBuilder"/> class.
        /// </summary>
        public OasExampleBuilder()
        {
        }

        /// <summary>
        /// Creates a new instance of the <see cref="OasExampleBuilder"/> class.
        /// </summary>
        /// <param name="value">The <see cref="OasExample"/> to copy values from.</param>
        public OasExampleBuilder(OasExample value)
        {
            if (value is null) throw new ArgumentNullException(nameof(value));
            Summary = value.Summary;
            Description = value.Description;
            ExternalValue = value.ExternalValue;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Performs an implicit conversion from <see cref="OasExampleBuilder"/> to <see cref="OasExample"/>.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator OasExample(OasExampleBuilder builder) => builder?.Build();

        /// <summary>
        /// Performs an implicit conversion from <see cref="OasExampleBuilder"/> to <see cref="OasReferable{Example}"/>.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator OasReferable<OasExample>(OasExampleBuilder builder) => builder?.Build();

        /// <summary>
        /// Performs an implicit conversion from <see cref="OasExample"/> to <see cref="OasExampleBuilder"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator OasExampleBuilder(OasExample value) => value is null ? null : new OasExampleBuilder(value);

        /// <summary>
        /// Creates the <see cref="OasExample"/> from this builder.
        /// </summary>
        /// <returns>The <see cref="OasExample"/>.</returns>
        public virtual OasExample Build() => new OasExample(
            summary: Summary,
            description: Description,
            externalValue: ExternalValue);

        #endregion
    }
}
