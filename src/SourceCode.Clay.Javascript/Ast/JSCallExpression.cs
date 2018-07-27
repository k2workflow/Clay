using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace SourceCode.Clay.Javascript.Ast
{
    public class JSCallExpression : JSExpression, IEnumerable
    {
        public override JSNodeType Type => JSNodeType.CallExpression;

        public JSExpression Callee { get; set; }

        public List<JSExpression> Arguments { get; }

        public JSCallExpression()
        {
            Arguments = new List<JSExpression>();
        }

        public JSCallExpression(JSExpression callee)
        {
            Arguments = new List<JSExpression>();
            Callee = callee;
        }

        public JSCallExpression Add(JSExpression argument)
        {
            Arguments.Add(argument);
            return this;
        }

        public JSCallExpression Add(IEnumerable<JSExpression> argument)
        {
            Arguments.AddRange(argument);
            return this;
        }

        public JSCallExpression Add(params JSExpression[] argument)
            => Add((IEnumerable<JSExpression>)argument);

        IEnumerator IEnumerable.GetEnumerator() => Enumerable.Empty<object>().GetEnumerator();
    }
}
