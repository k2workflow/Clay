using System.Collections.Generic;

namespace SourceCode.Clay.Collections.Generic
{
    internal sealed class StringOrdinalIgnoreSwitchBuilder<T> : SwitchBuilder<string, T>
    {
        public StringOrdinalIgnoreSwitchBuilder(IReadOnlyDictionary<string, T> cases)
            : base(cases)
        { }

        protected override string Normalize(string key) => key.ToLowerInvariant();
    }
}
