namespace SourceCode.Clay.Javascript.Ast
{
    public class JSNewExpression : JSCallExpression
    {
        public override JSNodeType Type => JSNodeType.NewExpression;
        
        public JSNewExpression()
        {
        }

        public JSNewExpression(JSExpression callee) 
            : base(callee)
        {
        }
    }
}
