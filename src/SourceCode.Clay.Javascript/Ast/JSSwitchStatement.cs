using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace SourceCode.Clay.Javascript.Ast
{
    public class JSSwitchStatement : JSStatement, IEnumerable
    {
        public override JSNodeType Type => JSNodeType.SwitchStatement;

        public JSExpression Discriminant { get; set; }

        public IList<JSSwitchCase> Cases { get; }

        public JSSwitchStatement()
        {
            Cases = new List<JSSwitchCase>();
        }

        public JSSwitchStatement(int capacity)
        {
            Cases = new List<JSSwitchCase>(capacity);
        }

        public JSSwitchStatement(JSExpression discriminant)
        {
            Cases = new List<JSSwitchCase>();
            Discriminant = discriminant;
        }

        public JSSwitchStatement(JSExpression discriminant, int capacity)
        {
            Cases = new List<JSSwitchCase>(capacity);
            Discriminant = discriminant;
        }

        public JSSwitchStatement Add(JSSwitchCase @case)
        {
            Cases.Add(@case);
            return this;
        }

        public JSSwitchStatement Add(IEnumerable<JSSwitchCase> cases)
        {
            foreach (var @case in cases)
                Cases.Add(@case);
            return this;
        }

        public JSSwitchStatement Add(params JSSwitchCase[] @case)
            => Add((IEnumerable<JSSwitchCase>)@case);

        public JSSwitchStatement Case(JSExpression test, IEnumerable<JSStatement> statements)
            => Add(new JSSwitchCase(test) { statements });

        public JSSwitchStatement Case(JSExpression test, params JSStatement[] statements)
            => Case(test, (IEnumerable<JSStatement>)statements);

        IEnumerator IEnumerable.GetEnumerator() => Enumerable.Empty<object>().GetEnumerator();
    }
}
