using System.Collections.Generic;

namespace SourceCode.Clay.Javascript.Ast
{
    public class JSIfStatement : JSBodyStatement
    {
        public override JSNodeType Type => JSNodeType.IfStatement;

        public JSExpression Test { get; set; }

        public JSStatement Alternate { get; set; }

        public JSIfStatement()
        {
        }

        public JSIfStatement(JSExpression test, JSStatement consequent, JSStatement alternate)
            : base(consequent)
        {
            Test = test;
            Alternate = alternate;
        }

        public JSIfStatement(JSExpression test, JSStatement consequent)
            : this(test, consequent, null)
        {
        }

        public JSIfStatement(JSExpression test)
        {
            Test = test;
        }

        public JSIfStatement Else(JSStatement body)
        {
            if (Alternate is null) Alternate = body;
            else if (Alternate is JSBlockStatement block) block.Body.Add(body);
            else Alternate = new JSBlockStatement()
            {
                Body =
                {
                    Alternate,
                    body
                }
            };

            return this;
        }

        public JSIfStatement Else(IEnumerable<JSStatement> body)
        {
            foreach (var item in body)
                Else(item);
            return this;
        }

        public JSIfStatement Else(params JSStatement[] body)
            => Else((IEnumerable<JSStatement>)body);

        public new JSIfStatement Add(JSStatement body) => (JSIfStatement)base.Add(body);
        public new JSIfStatement Add(params JSStatement[] body) => (JSIfStatement)base.Add(body);
        public new JSIfStatement Add(IEnumerable<JSStatement> body) => (JSIfStatement)base.Add(body);
    }
}
