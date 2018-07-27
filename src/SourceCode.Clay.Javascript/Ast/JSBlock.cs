using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace SourceCode.Clay.Javascript.Ast
{
    public abstract class JSBlock : IJSNode, IJSBlock
    {
        public abstract JSNodeType Type { get; }

        public List<JSStatement> Body { get; }

        IList<JSStatement> IJSBlock.Body => Body;

        public JSBlock()
        {
            Body = new List<JSStatement>();
        }

        public JSBlock Add(JSStatement body)
        {
            Add(body);
            return this;
        }

        public JSBlock Add(params JSStatement[] body)
            => Add((IEnumerable<JSStatement>)body);

        public JSBlock Add(IEnumerable<JSStatement> body)
        {
            Body.AddRange(body);
            return this;
        }

        IJSBlock IJSBlock.Add(JSStatement body) => Add(body);

        IJSBlock IJSBlock.Add(params JSStatement[] body) => Add(body);

        IJSBlock IJSBlock.Add(IEnumerable<JSStatement> body) => Add(body);

        IEnumerator IEnumerable.GetEnumerator() => Enumerable.Empty<object>().GetEnumerator();
    }
}
