using System;

namespace SourceCode.Clay.Net
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

#pragma warning disable CA2225 // Operator overloads have named alternates
        // Provided by ctor.
        public static implicit operator UrlTemplate<T>(string template) => new UrlTemplate<T>(template);
#pragma warning restore CA2225 // Operator overloads have named alternates
    }
}
