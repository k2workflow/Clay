using System;

namespace SourceCode.Clay.Net.Http
{
    internal readonly struct UriToken
    {
        public string Name { get; }

        public string Default { get; }

        public string Format { get; }

        public UriTokenType Type { get; }

        public UriToken(UriTokenType type, string name, string @default, string format)
            => (Name, Default, Format, Type) = (name, @default, format, type);
    }
} 
