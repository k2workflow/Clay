using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace SourceCode.Clay.Javascript.Ast
{
    public class JSBlockStatement : JSStatement, IJSBlock
    {
        public override JSNodeType Type => JSNodeType.BlockStatement;

        public List<JSStatement> Body { get; }

        IList<JSStatement> IJSBlock.Body => Body;

        public JSBlockStatement()
        {
            Body = new List<JSStatement>();
        }

        public JSBlockStatement Add(JSStatement body)
        {
            Add(body);
            return this;
        }

        public JSBlockStatement Add(params JSStatement[] body)
            => Add((IEnumerable<JSStatement>)body);

        public JSBlockStatement Add(IEnumerable<JSStatement> body)
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
