using System;

namespace SourceCode.Clay.Javascript.Ast
{
    [Flags]
    public enum JSRegexOptions
    {
        None = 0,
        Global = 1,
        IgnoreCase = 2,
        Multiline = 4,
        Unicode = 8,
        Sticky = 16
    }
}