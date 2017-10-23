#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using System;

namespace SourceCode.Clay.OpenApi
{
    /// <summary>
    /// Represents options for a JSON schema.
    /// </summary>
    [Flags]
#pragma warning disable CA1028 // Enum Storage should be Int32
    public enum SchemaOptions : byte
#pragma warning restore CA1028 // Enum Storage should be Int32
    {
        /// <summary>
        /// No options are set.
        /// </summary>
        None = 0,

        /// <summary>
        /// The contained items must be unique.
        /// </summary>
        UniqueItems = 1,

        /// <summary>
        /// This item is required.
        /// </summary>
        Required = 2,

        /// <summary>
        /// This item is nullable.
        /// </summary>
        Nullable = 4,

        /// <summary>
        /// This item is deprecated.
        /// </summary>
        Deprecated = 8
    }
}
