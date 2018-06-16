#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using System;

namespace SourceCode.Clay.Json.LinkedData
{
    [Flags]
    internal enum LinkedDataTermOptions
    {
        None = 0b0000_0000,
        SimpleTerm = 0b0000_0001,
        ReverseProperty = 0b0000_0010,
        Prefix = 0b0000_0100,
        ClearLanguage = 0b0000_1000,
        ClearTerm = 0b0001_0000,
    }
}
