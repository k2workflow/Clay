using System.Collections.Generic;

namespace SourceCode.Clay.Javascript.Ast
{
    public class JSCatchClause : JSBlock
    {
        public override JSNodeType Type => JSNodeType.CatchClause;

        public IJSPattern Parameter { get; set; }

        public JSCatchClause()
        {

        }

        public JSCatchClause(IJSPattern parameter)
        {
            Parameter = parameter;
        }

        public new JSCatchClause Add(JSStatement body) => (JSCatchClause)base.Add(body);
        public new JSCatchClause Add(params JSStatement[] body) => (JSCatchClause)base.Add(body);
        public new JSCatchClause Add(IEnumerable<JSStatement> body) => (JSCatchClause)base.Add(body);
    }
}