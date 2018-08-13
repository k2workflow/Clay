namespace SourceCode.Clay.Javascript.Ast
{
    public class JSThrowStatement : JSExpressionStatement
    {
        public override JSNodeType Type => JSNodeType.ThrowStatement;

        public JSThrowStatement()
        {
        }

        public JSThrowStatement(JSExpression argument)
            : base(argument)
        {
        }
    }
}
