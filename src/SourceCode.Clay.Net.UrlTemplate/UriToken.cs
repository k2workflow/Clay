using System;
using System.Linq.Expressions;

namespace SourceCode.Clay.Net
{
    internal readonly struct UriToken
    {
        public string Name { get; }

        public string SubName { get; }

        public string Default { get; }

        public ConstantExpression DefaultConstant { get; }

        public string Format { get; }

        public ConstantExpression FormatConstant { get; }

        public UriTokenType Type { get; }

        public UriToken(UriTokenType type, string name, string @default, string format)
        {
            SubName = null;
            if (type == UriTokenType.Value)
            {
                var idx = name.IndexOf('.');
                if (name.EndsWith("[]", StringComparison.Ordinal))
                {
                    name = name.Substring(0, name.Length - 2);
                    type = UriTokenType.Collection;
                }
                else if (idx > 0)
                {
                    SubName = name.Substring(idx + 1);
                    name = name.Substring(0, idx);
                }
            }

            Name = name;
            Default = @default;
            Format = format;
            Type = type;

            DefaultConstant = Expression.Constant(Default, typeof(string));
            FormatConstant = Expression.Constant(Format, typeof(string));
        }
    }
}
