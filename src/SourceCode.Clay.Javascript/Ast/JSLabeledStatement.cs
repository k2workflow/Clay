using System.Collections.Generic;

namespace SourceCode.Clay.Javascript.Ast
{
    public class JSLabeledStatement : JSBodyStatement
    {
        public override JSNodeType Type => JSNodeType.LabeledStatement;

        public JSIdentifier Label { get; set; }

        public JSLabeledStatement()
        { }

        public JSLabeledStatement(JSIdentifier label)
        {
            Label = label;
        }

        public JSLabeledStatement(JSIdentifier label, JSStatement body)
            : base(body)
        {
            Label = label;
        }

        public new JSLabeledStatement Add(JSStatement body) => (JSLabeledStatement)base.Add(body);
        public new JSLabeledStatement Add(params JSStatement[] body) => (JSLabeledStatement)base.Add(body);
        public new JSLabeledStatement Add(IEnumerable<JSStatement> body) => (JSLabeledStatement)base.Add(body);
    }
}
