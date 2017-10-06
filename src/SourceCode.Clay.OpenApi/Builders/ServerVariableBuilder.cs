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
    public class ServerVariableBuilder
    {
        #region Properties

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

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of the <see cref="ServerVariableBuilder"/> class.
        /// </summary>
        public ServerVariableBuilder()
        {
        }

        /// <summary>
        /// Creates a new instance of the <see cref="ServerVariableBuilder"/> class.
        /// </summary>
        /// <param name="value">The <see cref="ServerVariable"/> to copy values from.</param>
        public ServerVariableBuilder(ServerVariable value)
        {
            if (ReferenceEquals(value, null)) throw new ArgumentNullException(nameof(value));
            Enum = new List<string>(value.Enum);
            Default = value.Default;
            Description = value.Description;
        }

        #endregion

        #region Methods

        public static implicit operator ServerVariable(ServerVariableBuilder builder) => builder?.Build();

        public static implicit operator ServerVariableBuilder(ServerVariable value) => ReferenceEquals(value, null) ? null : new ServerVariableBuilder(value);

        /// <summary>
        /// Creates the <see cref="ServerVariable"/> from this builder.
        /// </summary>
        /// <returns>The <see cref="ServerVariable"/>.</returns>
        public ServerVariable Build() => new ServerVariable(
            @enum: new ReadOnlyCollection<String>(Enum),
            @default: Default,
            description: Description);

        #endregion
    }
}
