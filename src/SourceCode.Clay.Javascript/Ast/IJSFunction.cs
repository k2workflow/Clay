using System.Collections;
using System.Collections.Generic;

namespace SourceCode.Clay.Javascript.Ast
{
    public interface IJSFunction : IJSBlock, IEnumerable
    {
        IList<IJSPattern> Parameters { get; }

        JSIdentifier Identifier { get; set; }

        IJSFunction Add(IJSPattern parameter);

        IJSFunction Add(IEnumerable<IJSPattern> parameters);

        IJSFunction Add(params IJSPattern[] parameters);
    }
}
