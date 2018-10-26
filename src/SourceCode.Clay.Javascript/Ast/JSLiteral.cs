namespace SourceCode.Clay.Javascript.Ast
{
    public class JSLiteral : JSExpression, IJSIndexer
    {
        public override JSNodeType Type => JSNodeType.Literal;

        public object Value { get; set; }

        public JSLiteral()
        {
        }

        public JSLiteral(object value)
        {
            Value = value;
        }
    }
}
