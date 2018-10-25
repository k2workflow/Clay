namespace SourceCode.Clay.Javascript.Ast
{
    public class JSProperty : IJSNode
    {
        public virtual JSNodeType Type => JSNodeType.Property;

        public IJSIndexer Key { get; set; }

        public JSExpression Value { get; set; }

        public JSPropertyKind Kind { get; set; }

        public JSProperty()
        {
        }

        public JSProperty(JSPropertyKind kind, IJSIndexer key, JSExpression value)
        {
            Kind = kind;
            Key = key;
            Value = value;
        }

        public JSProperty(IJSIndexer key, JSExpression value)
            : this(JSPropertyKind.Initializer, key, value)
        {
        }
    }
}
