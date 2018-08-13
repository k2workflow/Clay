namespace SourceCode.Clay.Javascript.Ast
{
    public abstract class JSStatement : IJSNode
    {
        public abstract JSNodeType Type { get; }

        protected JSStatement()
        {
        }
    }
}