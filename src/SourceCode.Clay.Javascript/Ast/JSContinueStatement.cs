namespace SourceCode.Clay.Javascript.Ast
{
    public class JSContinueStatement : JSStatement
    {
        public override JSNodeType Type => JSNodeType.ContinueStatement;

        public JSIdentifier Label { get; set; }

        public JSContinueStatement()
        {
        }

        public JSContinueStatement(JSIdentifier identifier)
        {
            Label = identifier;
        }
    }
}
