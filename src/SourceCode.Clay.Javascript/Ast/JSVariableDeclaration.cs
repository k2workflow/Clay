using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace SourceCode.Clay.Javascript.Ast
{
    public class JSVariableDeclaration : JSStatement, IEnumerable, IJSPatternDeclaration, IJSInitializer
    {
        public override JSNodeType Type => JSNodeType.VariableDeclaration;

        public List<JSVariableDeclarator> Declarations { get; }

        public JSVariableDeclarationKind Kind { get; set; }

        public JSVariableDeclaration()
        {
            Declarations = new List<JSVariableDeclarator>();
        }

        public JSVariableDeclaration(int capacity)
        {
            Declarations = new List<JSVariableDeclarator>(capacity);
        }

        public JSVariableDeclaration(JSVariableDeclarator declaration)
        {
            Declarations = new List<JSVariableDeclarator>() { declaration };
        }

        public JSVariableDeclaration(IEnumerable<JSVariableDeclarator> declarations)
        {
            Declarations = new List<JSVariableDeclarator>(declarations);
        }

        public JSVariableDeclaration(params JSVariableDeclarator[] declarations)
            : this((IEnumerable<JSVariableDeclarator>)declarations)
        { }

        public JSVariableDeclaration Add(JSVariableDeclarator declaration)
        {
            Declarations.Add(declaration);
            return this;
        }

        public JSVariableDeclaration Add(IEnumerable<JSVariableDeclarator> declarations)
        {
            foreach (JSVariableDeclarator declaration in declarations)
                Declarations.Add(declaration);
            return this;
        }

        public JSVariableDeclaration Add(params JSVariableDeclarator[] declaration)
            => Add((IEnumerable<JSVariableDeclarator>)declaration);

        public JSVariableDeclaration Add(IJSPattern identifier, JSExpression initializer)
            => Add(new JSVariableDeclarator(identifier, initializer));

        public JSVariableDeclaration Add(string identifier, JSExpression initializer)
            => Add(new JSIdentifier(identifier), initializer);

        public JSVariableDeclaration Add(IJSPattern identifier)
            => Add(identifier, null);

        public JSVariableDeclaration Add(string identifier)
            => Add(identifier, null);

        IEnumerator IEnumerable.GetEnumerator() => Enumerable.Empty<object>().GetEnumerator();

        public static implicit operator JSVariableDeclaration(JSVariableDeclarator declarator)
            => declarator is null
            ? null
            : new JSVariableDeclaration() { declarator };
    }
}
