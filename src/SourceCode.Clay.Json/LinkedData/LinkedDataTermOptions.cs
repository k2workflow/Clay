#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using System;

namespace SourceCode.Clay.Json.LinkedData
{
    [Flags]
    public enum LinkedDataTermOptions
    {
        None = 0b0000,
        SimpleTerm = 0b0001,
        ReverseProperty = 0b0010,
        Prefix = 0b0100
    }
}
