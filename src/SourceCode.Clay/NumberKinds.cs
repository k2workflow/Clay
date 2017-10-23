#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using System;

namespace SourceCode.Clay
{
#pragma warning disable S2342 // Enumeration types should comply with a naming convention
#pragma warning disable S2346 // Flags enumerations zero-value members should be named "None"
#pragma warning disable CA1720 // Identifier contains type name
#pragma warning disable CA1028 // Enum Storage should be Int32

    [Flags]
    public enum NumberKinds : byte
    {
        Signed = 1,

        Integer = 2,

        Real = 4,

        Decimal = 8
    }

#pragma warning restore CA1028 // Enum Storage should be Int32
#pragma warning restore CA1720 // Identifier contains type name
#pragma warning restore S2346 // Flags enumerations zero-value members should be named "None"
#pragma warning restore S2342 // Enumeration types should comply with a naming convention
}
