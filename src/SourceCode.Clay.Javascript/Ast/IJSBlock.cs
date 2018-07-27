using System.Collections;
using System.Collections.Generic;

namespace SourceCode.Clay.Javascript.Ast
{
    public interface IJSBlock : IJSNode, IEnumerable
    {
        IList<JSStatement> Body { get; }

        IJSBlock Add(JSStatement body);
        IJSBlock Add(params JSStatement[] body);
        IJSBlock Add(IEnumerable<JSStatement> body);
    }
}
