using System.Collections.Generic;

namespace SourceCode.Clay.Javascript.Ast
{
    public class JSWhileStatement : JSBodyStatement
    {
        public override JSNodeType Type => JSNodeType.WhileStatement;

        public JSExpression Test { get; set; }

        public JSWhileStatement()
        { }

        public JSWhileStatement(JSStatement body)
            : base(body)
        { }

        public JSWhileStatement(JSExpression test)
        {
            Test = test;
        }

        public JSWhileStatement(JSExpression test, JSStatement body)
            : base(body)
        {
            Test = test;
        }

        public new JSWhileStatement Add(JSStatement body) => (JSWhileStatement)base.Add(body);
        public new JSWhileStatement Add(params JSStatement[] body) => (JSWhileStatement)base.Add(body);
        public new JSWhileStatement Add(IEnumerable<JSStatement> body) => (JSWhileStatement)base.Add(body);
    }
}
