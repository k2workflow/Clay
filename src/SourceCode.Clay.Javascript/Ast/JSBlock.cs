using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace SourceCode.Clay.Javascript.Ast
{
    public abstract class JSBlock : IJSNode, IJSBlock
    {
        public abstract JSNodeType Type { get; }

        public IList<JSStatement> Body { get; }

        protected JSBlock()
        {
            Body = new List<JSStatement>();
        }

        protected JSBlock(int capacity)
        {
            Body = new List<JSStatement>(capacity);
        }

        protected JSBlock(JSStatement body)
        {
            Body = new List<JSStatement>() { body };
        }

        protected JSBlock(IEnumerable<JSStatement> bodies)
        {
            Body = new List<JSStatement>(bodies);
        }

        public JSBlock Add(JSStatement body)
        {
            Add(body);
            return this;
        }

        public JSBlock Add(params JSStatement[] body)
            => Add((IEnumerable<JSStatement>)body);

        public JSBlock Add(IEnumerable<JSStatement> bodies)
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
