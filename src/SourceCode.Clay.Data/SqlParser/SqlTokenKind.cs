#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

namespace SourceCode.Clay.Data.SqlParser
{
#pragma warning disable CA1028 // Enum Storage should be Int32

    public enum SqlTokenKind : byte
#pragma warning restore CA1028 // Enum Storage should be Int32
    {
        // Literals

        Symbol,
        Literal,

        // Strings

        SquareString,
        QuotedString,

        // Sundry

        Whitespace,
        LineComment,
        BlockComment
    }
}
