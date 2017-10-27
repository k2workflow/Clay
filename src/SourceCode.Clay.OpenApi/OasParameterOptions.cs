#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using System;

namespace SourceCode.Clay.OpenApi
{
    /// <summary>
    /// Represents the different options for <see cref="OasParameterBody"/>.
    /// </summary>
    [Flags]
    public enum OasParameterOptions
    {
        /// <summary>
        /// The default options.
        /// </summary>
        None = 0,

        /// <summary>
        /// Indicates that the parameter is not mandatory.
        /// </summary>
        Required = 1,

        /// <summary>
        /// Specifies that a parameter is deprecated.
        /// </summary>
        Deprecated = 2,

        /// <summary>
        /// Sets the ability to pass empty-valued parameters.
        /// </summary>
        AllowEmptyValue = 4,

        /// <summary>
        /// Indicates that parameter values of type array or object generate separate parameters for each value of the array or
        /// key-value pair of the map.
        /// </summary>
        Explode = 8,

#       pragma warning disable S4016 // Enumeration members should not be named "Reserved"
        // Reasoning: enumeration member is not actually called "Reserved"

        /// <summary>
        /// Indicates that the parameter value allows reserved characters.
        /// </summary>
        AllowReserved = 16

#       pragma warning restore S4016 // Enumeration members should not be named "Reserved"
    }
}
