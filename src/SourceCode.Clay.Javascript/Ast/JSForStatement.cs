using System.Collections.Generic;

namespace SourceCode.Clay.Javascript.Ast
{
    public class JSForStatement : JSBodyStatement
    {
        public override JSNodeType Type => JSNodeType.ForStatement;

        public Discriminated<JSVariableDeclaration, JSExpression> Initializer { get; set; }

        public JSExpression Test { get; set; }

        public JSExpression Update { get; set; }

        public JSForStatement()
        {

        }

        public JSForStatement(Discriminated<JSVariableDeclaration, JSExpression> initializer, JSExpression test, JSExpression update)
        {
            Initializer = initializer;
            Test = test;
            Update = update;
        }

        public JSForStatement(Discriminated<JSVariableDeclaration, JSExpression> initializer, JSExpression test, JSExpression update, JSStatement body)
            : base(body)
        {
            Initializer = initializer;
            Test = test;
            Update = update;
        }

        public new JSForStatement Add(JSStatement body) => (JSForStatement)base.Add(body);
        public new JSForStatement Add(params JSStatement[] body) => (JSForStatement)base.Add(body);
        public new JSForStatement Add(IEnumerable<JSStatement> body) => (JSForStatement)base.Add(body);
    }
}
