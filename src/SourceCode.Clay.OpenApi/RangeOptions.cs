#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using System;

namespace SourceCode.Clay.OpenApi
{
    /// <summary>
    /// Indicates options for validating a range.
    /// </summary>
    [Flags]
#pragma warning disable CA1028 // Enum Storage should be Int32
    public enum RangeOptions : byte
#pragma warning restore CA1028 // Enum Storage should be Int32
    {
#       pragma warning disable S2346 // Flags enumerations zero-value members should be named "None"
        // Reasoning: self-documenting.

        /// <summary>
        /// The range is exclusive for both minimum and maximum.
        /// </summary>
        Exclusive = 0,

#       pragma warning restore S2346 // Flags enumerations zero-value members should be named "None"

        /// <summary>
        /// The range is inclusive for minimum.
        /// </summary>
        MinimumInclusive = 1,

        /// <summary>
        /// The range is inclusive for maximum.
        /// </summary>
        MaximumInclusive = 2,

        /// <summary>
        /// The range is inclusive for both minimum and maximum.
        /// </summary>
        Inclusive = MinimumInclusive | MaximumInclusive
    }
}
