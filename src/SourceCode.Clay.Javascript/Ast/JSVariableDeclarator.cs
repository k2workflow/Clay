namespace SourceCode.Clay.Javascript.Ast
{
    public class JSVariableDeclarator : IJSNode
    {
        public virtual JSNodeType Type => JSNodeType.VariableDeclarator;

        public IJSPattern Identifier { get; set; }

        public JSExpression Initializer { get; set; }

        public JSVariableDeclarator()
        {

        }

        public JSVariableDeclarator(IJSPattern identifier, JSExpression initializer)
        {
            Identifier = identifier;
            Initializer = initializer;
        }

        public JSVariableDeclarator(IJSPattern identifier)
            : this(identifier, null)
        {

        }

        public static implicit operator JSVariableDeclaration(JSVariableDeclarator declarator)
            => new JSVariableDeclaration() { declarator };

        public static implicit operator JSStatement(JSVariableDeclarator declarator)
            => new JSVariableDeclaration() { declarator };
    }
}