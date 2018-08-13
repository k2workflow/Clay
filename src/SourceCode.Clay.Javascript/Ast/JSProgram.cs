using System.Collections.Generic;

namespace SourceCode.Clay.Javascript.Ast
{
    public class JSProgram : JSBlock
    {
        public override JSNodeType Type => JSNodeType.Program;

        public JSProgram()
        {
        }

        public new JSProgram Add(JSStatement body) => (JSProgram)base.Add(body);
        public new JSProgram Add(params JSStatement[] body) => (JSProgram)base.Add(body);
        public new JSProgram Add(IEnumerable<JSStatement> body) => (JSProgram)base.Add(body);
    }
}
