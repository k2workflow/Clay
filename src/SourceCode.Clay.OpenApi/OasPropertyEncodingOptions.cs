#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using System;

namespace SourceCode.Clay.OpenApi
{
    /// <summary>
    /// Represents the options for <see cref="OasPropertyEncoding"/>.
    /// </summary>
    [Flags]
    public enum OasPropertyEncodingOptions
    {
        /// <summary>
        /// The default options.
        /// </summary>
        None = 0,

        /// <summary>
        /// Indicates that parameter values of type array or object generate separate parameters for each value of the array or
        /// key-value pair of the map.
        /// </summary>
        Explode = OasParameterOptions.Explode,

#       pragma warning disable S4016 // Enumeration members should not be named "Reserved"
        // Reasoning: enum member is not actually called "Reserved".

        /// <summary>
        /// Indicates that the parameter value allows reserved characters.
        /// </summary>
        AllowReserved = OasParameterOptions.AllowReserved

#       pragma warning restore S4016 // Enumeration members should not be named "Reserved"
    }
}
