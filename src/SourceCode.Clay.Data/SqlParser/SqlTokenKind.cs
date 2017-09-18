namespace SourceCode.Clay.Data.SqlParser
{
    public enum SqlTokenKind : byte
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
