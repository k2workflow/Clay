using System.Collections.Generic;

namespace SourceCode.Clay.Collections.Generic
{
    internal sealed class StringOrdinalSwitchBuilder<T> : SwitchBuilder<string, T>
    {
        public StringOrdinalSwitchBuilder(IReadOnlyDictionary<string, T> cases)
            : base(cases)
        { }
    }
}
