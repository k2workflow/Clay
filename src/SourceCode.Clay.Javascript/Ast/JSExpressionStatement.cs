namespace SourceCode.Clay.Javascript.Ast
{
    public class JSExpressionStatement : JSStatement
    {
        public override JSNodeType Type => JSNodeType.ExpressionStatement;

        public virtual JSExpression Expression { get; set; }

        public JSExpressionStatement()
        {

        }

        public JSExpressionStatement(JSExpression expression)
        {
            Expression = expression;
        }
    }
}
