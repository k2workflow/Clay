using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace SourceCode.Clay.Javascript.Ast
{
    public class JSSequenceExpression : JSExpression, IEnumerable
    {
        public override JSNodeType Type => JSNodeType.SequenceExpression;

        public List<JSExpression> Expressions { get; }

        public JSSequenceExpression()
        {
            Expressions = new List<JSExpression>();
        }

        public JSSequenceExpression Add(JSExpression expression)
        {
            Expressions.Add(expression);
            return this;
        }

        public JSSequenceExpression Add(IEnumerable<JSExpression> expression)
        {
            Expressions.AddRange(expression);
            return this;
        }

        public JSSequenceExpression Add(params JSExpression[] expression)
            => Add((IEnumerable<JSExpression>)expression);

        IEnumerator IEnumerable.GetEnumerator() => Enumerable.Empty<object>().GetEnumerator();
    }
}
