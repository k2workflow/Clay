#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using System;

namespace SourceCode.Clay.Json.LinkedData
{
    [Flags]
    public enum ContainerMappings
    {
        None = 0b0000_0000,
        Graph = 0b0000_0001,
        Id = 0b0000_0010,
        Index = 0b0000_0100,
        Language = 0b0000_1000,
        List = 0b0001_0000,
        Set = 0b0010_0000,
        Type = 0b0100_0000
    }
}
