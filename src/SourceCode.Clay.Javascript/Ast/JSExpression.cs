namespace SourceCode.Clay.Javascript.Ast
{
    public abstract class JSExpression : IJSNode
    {
        public abstract JSNodeType Type { get; }

        public static implicit operator JSStatement(JSExpression expression)
            => expression == null ? null : new JSExpressionStatement(expression);
    }
}
