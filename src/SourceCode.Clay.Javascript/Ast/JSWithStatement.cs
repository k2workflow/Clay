using System.Collections.Generic;

namespace SourceCode.Clay.Javascript.Ast
{
    public class JSWithStatement : JSBodyStatement
    {
        public override JSNodeType Type => JSNodeType.WithStatement;

        public JSExpression Object { get; set; }

        public JSWithStatement()
        { }

        public JSWithStatement(JSExpression @object)
        {
            Object = @object;
        }

        public JSWithStatement(JSExpression @object, JSStatement statement)
            : base(statement)
        {
            Object = @object;
        }

        public new JSWithStatement Add(JSStatement body) => (JSWithStatement)base.Add(body);
        public new JSWithStatement Add(params JSStatement[] body) => (JSWithStatement)base.Add(body);
        public new JSWithStatement Add(IEnumerable<JSStatement> body) => (JSWithStatement)base.Add(body);
    }
}
