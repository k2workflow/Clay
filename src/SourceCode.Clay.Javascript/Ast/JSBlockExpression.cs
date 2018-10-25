using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace SourceCode.Clay.Javascript.Ast
{
    public abstract class JSBlockExpression : JSExpression, IJSBlock
    {
        public List<JSStatement> Body { get; }

        IList<JSStatement> IJSBlock.Body => Body;

        protected JSBlockExpression()
        {
            Body = new List<JSStatement>();
        }

        protected JSBlockExpression(int capacity)
        {
            Body = new List<JSStatement>(capacity);
        }

        protected JSBlockExpression(JSStatement body)
        {
            Body = new List<JSStatement>() { body };
        }

        protected JSBlockExpression(IEnumerable<JSStatement> bodies)
        {
            Body = new List<JSStatement>(bodies);
        }

        public JSBlockExpression Add(JSStatement body)
        {
            Body.Add(body);
            return this;
        }

        public JSBlockExpression Add(params JSStatement[] body)
            => Add((IEnumerable<JSStatement>)body);

        public JSBlockExpression Add(IEnumerable<JSStatement> bodies)
        {
            foreach (JSStatement body in bodies)
                Body.Add(body);
            return this;
        }

        IJSBlock IJSBlock.Add(JSStatement body) => Add(body);

        IJSBlock IJSBlock.Add(params JSStatement[] body) => Add(body);

        IJSBlock IJSBlock.Add(IEnumerable<JSStatement> body) => Add(body);

        IEnumerator IEnumerable.GetEnumerator() => Enumerable.Empty<object>().GetEnumerator();
    }
}
