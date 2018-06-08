#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace SourceCode.Clay.OpenApi
{
    /// <summary>
    /// An object representing a Server Variable for server URL template substitution.
    /// </summary>
    public class OasServerVariableBuilder : IOasBuilder<OasServerVariable>
    {
        /// <summary>
        /// Gets the enumeration of string values to be used if the substitution options are from a limited set.
        /// </summary>
        public IList<string> Enum { get; }

        /// <summary>
        /// Gets or sets the default value to use for substitution, and to send, if an alternate value is not supplied.
        /// </summary>
        public string Default { get; set; }

        /// <summary>
        /// Gets or sets the description for the server variable.
        /// </summary>
        /// <remarks>
        /// CommonMark syntax MAY be used for rich text representation.
        /// </remarks>
        public string Description { get; set; }

        /// <summary>
        /// Creates a new instance of the <see cref="OasServerVariableBuilder"/> class.
        /// </summary>
        public OasServerVariableBuilder()
        {
            Enum = new List<string>();
        }

        /// <summary>
        /// Creates a new instance of the <see cref="OasServerVariableBuilder"/> class.
        /// </summary>
        /// <param name="value">The <see cref="OasServerVariable"/> to copy values from.</param>
        public OasServerVariableBuilder(OasServerVariable value)
        {
            if (value is null) throw new ArgumentNullException(nameof(value));
            Enum = new List<string>(value.Enum);
            Default = value.Default;
            Description = value.Description;
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="OasServerVariableBuilder"/> to <see cref="OasServerVariable"/>.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator OasServerVariable(OasServerVariableBuilder builder) => builder?.Build();

        /// <summary>
        /// Performs an implicit conversion from <see cref="OasServerVariable"/> to <see cref="OasServerVariableBuilder"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator OasServerVariableBuilder(OasServerVariable value) => value is null ? null : new OasServerVariableBuilder(value);

        /// <summary>
        /// Creates the <see cref="OasServerVariable"/> from this builder.
        /// </summary>
        /// <returns>The <see cref="OasServerVariable"/>.</returns>
        public virtual OasServerVariable Build() => new OasServerVariable(
            @enum: new ReadOnlyCollection<String>(Enum),
            @default: Default,
            description: Description);
    }
}
