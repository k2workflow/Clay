using System;
using System.Collections.Concurrent;

namespace SourceCode.Clay.Net
{
    public sealed class UrlTemplate
    {
        public string Template { get; }
        private readonly RawUriTemplate _template;
        private readonly ConcurrentDictionary<RuntimeTypeHandle, Func<object, string>> _compiled;

        public UrlTemplate(string template)
        {
            Template = template ?? throw new ArgumentNullException(nameof(template));
            _template = RawUriTemplate.Parse(template);
            _compiled = new ConcurrentDictionary<RuntimeTypeHandle, Func<object, string>>();
        }

        private Func<object, string> GetCompiled(object specimen)
            => _compiled.GetOrAdd(Type.GetTypeHandle(specimen), rth => TemplateCompiler.Compile(_template, rth));

        public override string ToString() => Template;
        public string ToString(object value) => GetCompiled(value)(value);

        public static implicit operator UrlTemplate(string template) => new UrlTemplate(template);
    }
}
