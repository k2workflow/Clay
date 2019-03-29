using System.Collections.Generic;

namespace SourceCode.Clay.Net.Http
{
    internal readonly struct UriQuery
    {
        public IReadOnlyList<UriToken> Name { get; }
        public IReadOnlyList<UriToken> Value { get; }

        public UriQuery(IReadOnlyList<UriToken> name, IReadOnlyList<UriToken> value)
            => (Name, Value) = (name, value);
    }
}
