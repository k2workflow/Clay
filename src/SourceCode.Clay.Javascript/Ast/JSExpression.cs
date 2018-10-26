namespace SourceCode.Clay.Javascript.Ast
{
    public abstract class JSExpression : IJSNode, IJSInitializer
    {
        public abstract JSNodeType Type { get; }

        protected JSExpression()
        { }

        public static implicit operator JSStatement(JSExpression expression)
            => expression is null ? null : new JSExpressionStatement(expression);
    }
}
