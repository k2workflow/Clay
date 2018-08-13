using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace SourceCode.Clay.Javascript.Ast
{
    public abstract class JSBodyStatement : JSStatement, IEnumerable
    {
        public JSStatement Body { get; set; }

        protected JSBodyStatement()
        { }

        protected JSBodyStatement(JSStatement body)
        {
            Add(body);
        }

        protected JSBodyStatement(IEnumerable<JSStatement> body)
        {
            foreach (var item in body)
                Add(item);
        }

        protected JSBodyStatement(params JSStatement[] body)
            : this((IEnumerable<JSStatement>)body)
        { }

        public JSBodyStatement Add(JSStatement body)
        {
            if (Body is null) Body = body;
            else if (Body is JSBlockStatement block) block.Body.Add(body);
            else Body = new JSBlockStatement()
            {
                Body =
                {
                    Body,
                    body
                }
            };

            return this;
        }

        public JSBodyStatement Add(IEnumerable<JSStatement> body)
        {
            foreach (var item in body)
                Add(item);
            return this;
        }

        public JSBodyStatement Add(params JSStatement[] body)
            => Add((IEnumerable<JSStatement>)body);

        IEnumerator IEnumerable.GetEnumerator() => Enumerable.Empty<object>().GetEnumerator();
    }
}
