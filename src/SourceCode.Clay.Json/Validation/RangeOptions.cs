#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using System;

namespace SourceCode.Clay.Json.Validation
{
    /// <summary>
    /// Indicates options for validating a range.
    /// </summary>
    [Flags]
    public enum RangeOptions
    {
        /// <summary>
        /// The range is exclusive for both minimum and maximum.
        /// </summary>
        Exclusive = 0,

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
        Inclusive = MinimumInclusive | MaximumInclusive // 3
    }
}
