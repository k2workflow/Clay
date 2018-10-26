using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace SourceCode.Clay.Javascript.Ast
{
    public class JSObjectExpression : JSExpression, IEnumerable
    {
        public override JSNodeType Type => JSNodeType.ObjectExpression;

        public List<JSProperty> Properties { get; }

        public JSExpression this[IJSIndexer key]
        {
            set => Properties.Add(new JSProperty(key, value));
        }

        public JSObjectExpression()
        {
            Properties = new List<JSProperty>();
        }

        public JSObjectExpression(int capacity)
        {
            Properties = new List<JSProperty>(capacity);
        }

        public JSObjectExpression(JSProperty property)
        {
            Properties = new List<JSProperty>() { property };
        }

        public JSObjectExpression(IEnumerable<JSProperty> properties)
        {
            Properties = new List<JSProperty>(properties);
        }

        public JSObjectExpression(params JSProperty[] properties)
            : this((IEnumerable<JSProperty>)properties)
        { }

        public JSObjectExpression Add(JSProperty property)
        {
            Properties.Add(property);
            return this;
        }

        public JSObjectExpression Add(IEnumerable<JSProperty> properties)
        {
            foreach (JSProperty property in properties)
                Properties.Add(property);
            return this;
        }

        public JSObjectExpression Add(params JSProperty[] property)
            => Add((IEnumerable<JSProperty>)property);

        public JSObjectExpression Add(IJSIndexer key, JSExpression value)
            => Add(new JSProperty(key, value));

        public JSObjectExpression Add(string key, JSExpression value)
            => Add(new JSIdentifier(key), value);

        public JSObjectExpression Add(int index, JSExpression value)
            => Add(new JSLiteral(index), value);

        IEnumerator IEnumerable.GetEnumerator() => Enumerable.Empty<object>().GetEnumerator();
    }
}
