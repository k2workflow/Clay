namespace SourceCode.Clay.Javascript.Ast
{
    public class JSIdentifier : JSExpression, IJSPattern
    {
        public override JSNodeType Type => JSNodeType.Identifier;

        public string Name { get; set; }

        public JSIdentifier()
        {

        }

        public JSIdentifier(string name)
        {
            Name = name;
        }
    }
}
