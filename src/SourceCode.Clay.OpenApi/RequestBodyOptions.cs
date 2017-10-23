#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using System;

namespace SourceCode.Clay.OpenApi
{
    /// <summary>
    /// Represents options for <see cref="RequestBody"/>.
    /// </summary>
    [Flags]
#pragma warning disable CA1028 // Enum Storage should be Int32
    public enum RequestBodyOptions : byte
#pragma warning restore CA1028 // Enum Storage should be Int32
    {
        /// <summary>
        /// The default options.
        /// </summary>
        None = 0,

        /// <summary>
        /// The the request body is required in the request.
        /// </summary>
        Required = 1
    }
}
