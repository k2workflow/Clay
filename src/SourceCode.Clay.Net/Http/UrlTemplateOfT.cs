using System;

namespace SourceCode.Clay.Net.Http
{
    public sealed class UrlTemplate<T>
    {
        public string Template { get; }
        private readonly Func<T, string> _compiled;

        public UrlTemplate(string template)
        {
            Template = template ?? throw new ArgumentNullException(nameof(template));
            _compiled = TemplateCompiler.Compile<T>(RawUriTemplate.Parse(template));
        }

        public override string ToString() => Template;
        public string ToString(T value) => _compiled(value);

        public static implicit operator UrlTemplate<T>(string template) => new UrlTemplate<T>(template);
    }
}
