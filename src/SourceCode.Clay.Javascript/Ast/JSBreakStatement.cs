namespace SourceCode.Clay.Javascript.Ast
{
    public class JSBreakStatement : JSStatement
    {
        public override JSNodeType Type => JSNodeType.BreakStatement;

        public JSIdentifier Label { get; set; }

        public JSBreakStatement()
        {
        }

        public JSBreakStatement(JSIdentifier identifier)
        {
            Label = identifier;
        }
    }
}
