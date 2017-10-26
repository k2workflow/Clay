#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

namespace SourceCode.Clay.OpenApi
{
    // Reasoning: Refers to system types.
#pragma warning disable CA1720 // Identifier contains type name

    /// <summary>
    /// Represents the different JSON schema types.
    /// </summary>
    public enum OasSchemaType
    {
        /// <summary>
        /// The type is a string.
        /// </summary>
        String = 1,

        /// <summary>
        /// The type is a number.
        /// </summary>
        Number = 2,

        /// <summary>
        /// The type is an object.
        /// </summary>
        Object = 3,

        /// <summary>
        /// The type is an array.
        /// </summary>
        Array = 4,

        /// <summary>
        /// The type is a boolean.
        /// </summary>
        Boolean = 5,

        /// <summary>
        /// The type is an integer.
        /// </summary>
        Integer = 6
    }

#pragma warning restore CA1720 // Identifier contains type name
}
