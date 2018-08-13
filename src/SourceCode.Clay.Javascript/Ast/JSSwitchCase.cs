using System.Collections.Generic;

namespace SourceCode.Clay.Javascript.Ast
{
    public class JSSwitchCase : JSBlock
    {
        public override JSNodeType Type => JSNodeType.SwitchCase;

        public JSExpression Test { get; set; }

        public JSSwitchCase()
        {
        }

        public JSSwitchCase(JSExpression test)
        {
            Test = test;
        }

        public new JSSwitchCase Add(JSStatement body) => (JSSwitchCase)base.Add(body);
        public new JSSwitchCase Add(params JSStatement[] body) => (JSSwitchCase)base.Add(body);
        public new JSSwitchCase Add(IEnumerable<JSStatement> body) => (JSSwitchCase)base.Add(body);
    }
}