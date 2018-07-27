using System.Collections.Generic;

namespace SourceCode.Clay.Javascript.Ast
{
    public class JSFunctionDeclaration : JSBlockStatement, IJSFunction
    {
        public override JSNodeType Type => JSNodeType.FunctionDeclaration;

        public JSIdentifier Identifier { get; set; }

        public List<IJSPattern> Parameters { get; }

        IList<IJSPattern> IJSFunction.Parameters => Parameters;

        public JSFunctionDeclaration()
        {
            Parameters = new List<IJSPattern>();
        }

        public JSFunctionDeclaration(JSIdentifier identifier)
        {
            Parameters = new List<IJSPattern>();
            Identifier = identifier;
        }

        public JSFunctionDeclaration(JSIdentifier identifier, IEnumerable<IJSPattern> parameters)
        {
            Parameters = new List<IJSPattern>(parameters);
            Identifier = identifier;
        }

        public JSFunctionDeclaration(JSIdentifier identifier, params IJSPattern[] parameters)
            : this(identifier, (IEnumerable<IJSPattern>)parameters)
        {
            Parameters = new List<IJSPattern>(parameters);
            Identifier = identifier;
        }

        public new JSFunctionDeclaration Add(JSStatement body) => (JSFunctionDeclaration)base.Add(body);
        public new JSFunctionDeclaration Add(params JSStatement[] body) => (JSFunctionDeclaration)base.Add(body);
        public new JSFunctionDeclaration Add(IEnumerable<JSStatement> body) => (JSFunctionDeclaration)base.Add(body);

        public JSFunctionDeclaration Add(IJSPattern parameter)
        {
            Parameters.Add(parameter);
            return this;
        }

        public JSFunctionDeclaration Add(IEnumerable<IJSPattern> parameters)
        {
            Parameters.AddRange(parameters);
            return this;
        }

        public JSFunctionDeclaration Add(params IJSPattern[] parameters)
            => Add((IEnumerable<IJSPattern>)parameters);

        IJSFunction IJSFunction.Add(IJSPattern parameter) => Add(parameter);

        IJSFunction IJSFunction.Add(IEnumerable<IJSPattern> parameters) => Add(parameters);

        IJSFunction IJSFunction.Add(params IJSPattern[] parameters) => Add(parameters);
    }
}
