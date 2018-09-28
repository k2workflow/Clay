using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace SourceCode.Clay.Javascript.Ast
{
    public class JSSequenceExpression : JSExpression, IEnumerable
    {
        public override JSNodeType Type => JSNodeType.SequenceExpression;

        public IList<JSExpression> Expressions { get; }

        public JSSequenceExpression()
        {
            Expressions = new List<JSExpression>();
        }

        public JSSequenceExpression(int capacity)
        {
            Expressions = new List<JSExpression>(capacity);
        }

        public JSSequenceExpression(JSExpression expression)
        {
            Expressions = new List<JSExpression>() { expression };
        }

        public JSSequenceExpression(IEnumerable<JSExpression> expressions)
        {
            Expressions = new List<JSExpression>(expressions);
        }

        public JSSequenceExpression(params JSExpression[] expressions)
            : this((IEnumerable<JSExpression>)expressions)
        { }

        public JSSequenceExpression Add(JSExpression expression)
        {
            Expressions.Add(expression);
            return this;
        }

        public JSSequenceExpression Add(IEnumerable<JSExpression> expressions)
        {
            foreach (JSExpression expression in expressions)
                Expressions.Add(expression);
            return this;
        }

        public JSSequenceExpression Add(params JSExpression[] expression)
            => Add((IEnumerable<JSExpression>)expression);

        IEnumerator IEnumerable.GetEnumerator() => Enumerable.Empty<object>().GetEnumerator();
    }
}
