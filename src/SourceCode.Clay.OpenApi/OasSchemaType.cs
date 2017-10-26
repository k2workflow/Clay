#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

namespace SourceCode.Clay.OpenApi
{
    /// <summary>
    /// Represents the different JSON schema types.
    /// </summary>
#pragma warning disable CA1028 // Enum Storage should be Int32

    public enum OasSchemaType : byte
#pragma warning restore CA1028 // Enum Storage should be Int32
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
}
