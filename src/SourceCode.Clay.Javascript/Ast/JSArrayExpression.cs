using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace SourceCode.Clay.Javascript.Ast
{
    public class JSArrayExpression : JSExpression, IEnumerable
    {
        public override JSNodeType Type => JSNodeType.ArrayExpression;

        public List<JSExpression> Elements { get; }

        public JSArrayExpression()
        {
            Elements = new List<JSExpression>();
        }

        public JSArrayExpression(int capacity)
        {
            Elements = new List<JSExpression>(capacity);
        }

        public JSArrayExpression(JSExpression element)
        {
            Elements = new List<JSExpression>() { element };
        }

        public JSArrayExpression(IEnumerable<JSExpression> elements)
        {
            Elements = new List<JSExpression>(elements);
        }

        public JSArrayExpression(params JSExpression[] elements)
            : this((IEnumerable<JSExpression>)elements)
        { }

        public JSArrayExpression Add(JSExpression element)
        {
            Elements.Add(element);
            return this;
        }

        public JSArrayExpression Add(IEnumerable<JSExpression> elements)
        {
            foreach (JSExpression element in elements)
                Elements.Add(element);
            return this;
        }

        public JSArrayExpression Add(params JSExpression[] element)
            => Add((IEnumerable<JSExpression>)element);

        IEnumerator IEnumerable.GetEnumerator() => Enumerable.Empty<object>().GetEnumerator();
    }
}
