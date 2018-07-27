namespace SourceCode.Clay.Javascript.Ast
{
    public class JSConditionalExpression : JSExpression
    {
        public override JSNodeType Type => JSNodeType.ConditionalExpression;

        public JSExpression Test { get; set; }

        public JSExpression Consequent { get; set; }

        public JSExpression Alternate { get; set; }

        public JSConditionalExpression()
        {

        }

        public JSConditionalExpression(JSExpression test, JSExpression consequent, JSExpression alternate)
        {
            Test = test;
            Consequent = consequent;
            Alternate = alternate;
        }
    }
}
