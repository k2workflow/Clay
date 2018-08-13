namespace SourceCode.Clay.Javascript.Ast
{
    public class JSUnaryExpression : JSExpression
    {
        public override JSNodeType Type => JSNodeType.UnaryExpression;

        public JSUnaryOperator Operator { get; set; }

        public JSExpression Expression { get; set; }

        public JSUnaryExpression()
        {
        }

        public JSUnaryExpression(JSUnaryOperator @operator, JSExpression argument)
        {
            Operator = @operator;
            Expression = argument;
        }
    }
}
