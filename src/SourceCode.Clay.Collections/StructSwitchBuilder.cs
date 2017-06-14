using System;
using System.Collections.Generic;

namespace SourceCode.Clay.Collections.Generic
{
    /// <summary>
    /// A dynamic switch for dictionaries with <see cref="System.String"/> keys.
    /// Uses a case-sensitive (invariant) comparison of key values.
    /// </summary>
    /// <typeparam name="TKey">The type of keys.</typeparam>
    /// <typeparam name="TValue">The type of values.</typeparam>
    internal sealed class StructSwitchBuilder<TKey, TValue> : BaseSwitchBuilder<TKey, TValue>
        where TKey : struct, IEquatable<TKey>
    {
        public StructSwitchBuilder(IReadOnlyDictionary<TKey, TValue> cases)
            : base(cases)
        { }
    }
}
