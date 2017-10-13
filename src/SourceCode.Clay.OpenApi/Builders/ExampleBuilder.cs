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
    public class ExampleBuilder : IOpenApiBuilder<Example>
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
        /// Creates a new instance of the <see cref="ExampleBuilder"/> class.
        /// </summary>
        public ExampleBuilder()
        {
        }

        /// <summary>
        /// Creates a new instance of the <see cref="ExampleBuilder"/> class.
        /// </summary>
        /// <param name="value">The <see cref="Example"/> to copy values from.</param>
        public ExampleBuilder(Example value)
        {
            if (ReferenceEquals(value, null)) throw new ArgumentNullException(nameof(value));
            Summary = value.Summary;
            Description = value.Description;
            ExternalValue = value.ExternalValue;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Performs an implicit conversion from <see cref="ExampleBuilder"/> to <see cref="Example"/>.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator Example(ExampleBuilder builder) => builder?.Build();

        /// <summary>
        /// Performs an implicit conversion from <see cref="ExampleBuilder"/> to <see cref="Referable{Example}"/>.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator Referable<Example>(ExampleBuilder builder) => builder?.Build();

        /// <summary>
        /// Performs an implicit conversion from <see cref="Example"/> to <see cref="ExampleBuilder"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator ExampleBuilder(Example value) => ReferenceEquals(value, null) ? null : new ExampleBuilder(value);

        /// <summary>
        /// Creates the <see cref="Example"/> from this builder.
        /// </summary>
        /// <returns>The <see cref="Example"/>.</returns>
        public Example Build() => new Example(
            summary: Summary,
            description: Description,
            externalValue: ExternalValue);

        #endregion
    }
}
