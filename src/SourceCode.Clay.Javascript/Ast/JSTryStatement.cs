using System.Collections.Generic;

namespace SourceCode.Clay.Javascript.Ast
{
    public class JSTryStatement : JSBlockStatement
    {
        public override JSNodeType Type => JSNodeType.TryStatement;

        public JSCatchClause Handler { get; set; }

        public JSBlockStatement Finalizer { get; }

        public JSTryStatement()
        {
            Finalizer = new JSBlockStatement();
        }

        public JSTryStatement(JSCatchClause handler)
        {
            Handler = handler;
            Finalizer = new JSBlockStatement();
        }

        public new JSTryStatement Add(JSStatement body) => (JSTryStatement)base.Add(body);
        public new JSTryStatement Add(params JSStatement[] body) => (JSTryStatement)base.Add(body);
        public new JSTryStatement Add(IEnumerable<JSStatement> body) => (JSTryStatement)base.Add(body);

        public JSTryStatement Catch(IJSPattern parameter, IEnumerable<JSStatement> statements)
        {
            Handler = new JSCatchClause(parameter) { statements };
            return this;
        }

        public JSTryStatement Catch(IJSPattern parameter, params JSStatement[] statements)
            => Catch(parameter, (IEnumerable<JSStatement>)statements);

        public JSTryStatement Finally(IEnumerable<JSStatement> statements)
        {
            Finalizer.Add(statements);
            return this;
        }

        public JSTryStatement Finally(params JSStatement[] statements)
            => Finally((IEnumerable<JSStatement>)statements);
    }
}
