using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace SourceCode.Clay.Javascript.Ast
{
    public class JSObjectExpression : JSExpression, IEnumerable
    {
        public override JSNodeType Type => JSNodeType.ObjectExpression;

        public IList<JSProperty> Properties { get; }

        public JSExpression this[Discriminated<JSLiteral, JSIdentifier> key]
        {
            set => Properties.Add(new JSProperty(key, value));
        }

        public JSObjectExpression()
        {
            Properties = new List<JSProperty>();
        }

        public JSObjectExpression Add(JSProperty property)
        {
            Properties.Add(property);
            return this;
        }

        public JSObjectExpression Add(IEnumerable<JSProperty> properties)
        {
            foreach (var property in properties)
                Properties.Add(property);
            return this;
        }

        public JSObjectExpression Add(params JSProperty[] property)
            => Add((IEnumerable<JSProperty>)property);

        public JSObjectExpression Add(Discriminated<JSLiteral, JSIdentifier> key, JSExpression value)
            => Add(new JSProperty(key, value));

        public JSObjectExpression Add(string key, JSExpression value)
            => Add(new JSIdentifier(key), value);

        public JSObjectExpression Add(int index, JSExpression value)
            => Add(new JSLiteral(index), value);

        IEnumerator IEnumerable.GetEnumerator() => Enumerable.Empty<object>().GetEnumerator();
    }
}
