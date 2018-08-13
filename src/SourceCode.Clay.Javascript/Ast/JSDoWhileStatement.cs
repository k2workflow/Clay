using System.Collections.Generic;

namespace SourceCode.Clay.Javascript.Ast
{
    public class JSDoWhileStatement : JSWhileStatement
    {
        public override JSNodeType Type => JSNodeType.DoWhileStatement;

        public JSDoWhileStatement()
        {
        }

        public JSDoWhileStatement(JSExpression test)
            : base(test)
        {
        }

        public JSDoWhileStatement(JSExpression test, JSStatement body)
            : base(test, body)
        {
        }

        public new JSDoWhileStatement Add(JSStatement body) => (JSDoWhileStatement)base.Add(body);
        public new JSDoWhileStatement Add(params JSStatement[] body) => (JSDoWhileStatement)base.Add(body);
        public new JSDoWhileStatement Add(IEnumerable<JSStatement> body) => (JSDoWhileStatement)base.Add(body);
    }
}
