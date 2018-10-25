using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace SourceCode.Clay.Javascript.Ast
{
    public class JSMemberExpression : JSExpression, IEnumerable
    {
        public override JSNodeType Type => JSNodeType.MemberExpression;

        public JSExpression Object { get; set; }

        public List<JSExpression> Indices { get; }

        public bool IsComputed { get; set; }

        public JSMemberExpression()
        {
            Indices = new List<JSExpression>();
        }

        public JSMemberExpression(int capacity)
        {
            Indices = new List<JSExpression>(capacity);
        }

        public JSMemberExpression(JSExpression @object)
        {
            Indices = new List<JSExpression>();
            Object = @object;
        }

        public JSMemberExpression(JSExpression @object, int capacity)
        {
            Indices = new List<JSExpression>(capacity);
            Object = @object;
        }

        public JSMemberExpression(JSExpression @object, JSExpression property)
        {
            Object = @object;
            Indices = new List<JSExpression>() { property };
        }

        public JSMemberExpression(JSExpression @object, bool isComputed)
        {
            Indices = new List<JSExpression>();
            Object = @object;
            IsComputed = isComputed;
        }

        public JSMemberExpression(JSExpression @object, bool isComputed, int capacity)
        {
            Indices = new List<JSExpression>(capacity);
            Object = @object;
            IsComputed = isComputed;
        }

        public JSMemberExpression(JSExpression @object, JSExpression property, bool isComputed)
        {
            Object = @object;
            Indices = new List<JSExpression>() { property };
            IsComputed = isComputed;
        }

        public JSMemberExpression Add(JSExpression property)
        {
            Indices.Add(property);
            return this;
        }

        public JSMemberExpression Add(IEnumerable<JSExpression> properties)
        {
            foreach (JSExpression property in properties)
                Indices.Add(property);
            return this;
        }

        public JSMemberExpression Add(params JSExpression[] property)
            => Add((IEnumerable<JSExpression>)property);

        IEnumerator IEnumerable.GetEnumerator() => Enumerable.Empty<object>().GetEnumerator();
    }
}
