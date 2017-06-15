using System.Collections.Generic;

namespace SourceCode.Clay.Collections.Generic
{
    /// <summary>
    /// A dynamic switch for dictionaries with <see cref="System.String"/> keys.
    /// Uses an case-insensitive (invariant) comparison of key values.
    /// </summary>
    /// <typeparam name="TValue">The type of values.</typeparam>
    internal sealed class CaseInsensitiveSwitchBuilder<TValue> : BaseSwitchBuilder<string, TValue>
    {
        public CaseInsensitiveSwitchBuilder(IReadOnlyDictionary<string, TValue> cases)
            : base(cases)
        { }

        protected override string NormalizeKey(string key) => key.ToUpperInvariant(); // Apparently UC (vs LC) is optimized in CLR
    }
}
