using System.Collections.Generic;

namespace SourceCode.Clay.Javascript.Ast
{
    public class JSFunctionExpression : JSBlockExpression, IJSFunction
    {
        public override JSNodeType Type => JSNodeType.FunctionExpression;

        public JSIdentifier Identifier { get; set; }

        public IList<IJSPattern> Parameters { get; }

        IList<IJSPattern> IJSFunction.Parameters => Parameters;

        public JSFunctionExpression()
        {
            Parameters = new List<IJSPattern>();
        }

        public JSFunctionExpression(int capacity)
        {
            Parameters = new List<IJSPattern>(capacity);
        }

        public JSFunctionExpression(JSIdentifier identifier)
        {
            Parameters = new List<IJSPattern>();
            Identifier = identifier;
        }

        public JSFunctionExpression(JSIdentifier identifier, int capacity)
        {
            Parameters = new List<IJSPattern>(capacity);
            Identifier = identifier;
        }

        public JSFunctionExpression(JSIdentifier identifier, IEnumerable<IJSPattern> parameters)
        {
            Parameters = new List<IJSPattern>(parameters);
            Identifier = identifier;
        }

        public JSFunctionExpression(JSIdentifier identifier, params IJSPattern[] parameters)
            : this(identifier, (IEnumerable<IJSPattern>)parameters)
        {
        }

        public new JSFunctionExpression Add(JSStatement body) => (JSFunctionExpression)base.Add(body);
        public new JSFunctionExpression Add(params JSStatement[] body) => (JSFunctionExpression)base.Add(body);
        public new JSFunctionExpression Add(IEnumerable<JSStatement> body) => (JSFunctionExpression)base.Add(body);

        public JSFunctionExpression Add(IJSPattern parameter)
        {
            Parameters.Add(parameter);
            return this;
        }

        public JSFunctionExpression Add(IEnumerable<IJSPattern> parameters)
        {
            foreach (var parameter in parameters)
                Parameters.Add(parameter);
            return this;
        }

        public JSFunctionExpression Add(params IJSPattern[] parameters)
            => Add((IEnumerable<IJSPattern>)parameters);

        IJSFunction IJSFunction.Add(IJSPattern parameter) => Add(parameter);

        IJSFunction IJSFunction.Add(IEnumerable<IJSPattern> parameters) => Add(parameters);

        IJSFunction IJSFunction.Add(params IJSPattern[] parameters) => Add(parameters);
    }
}
