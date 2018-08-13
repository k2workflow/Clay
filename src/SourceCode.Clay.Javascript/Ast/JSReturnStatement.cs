namespace SourceCode.Clay.Javascript.Ast
{
    public class JSReturnStatement : JSExpressionStatement
    {
        public override JSNodeType Type => JSNodeType.ReturnStatement;

        public JSReturnStatement()
        {
        }

        public JSReturnStatement(JSExpression argument)
            : base(argument)
        {
        }
    }
}
