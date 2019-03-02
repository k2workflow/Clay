using System.Collections.Generic;

namespace SourceCode.Clay.Collections.Generic
{
    /// <summary>
    /// Represents <see cref="IReadOnlyDictionary{TKey, TValue}"/> extensions.
    /// </summary>
    public static class IReadOnlyDictionaryExtensions
    {
        public static IReadOnlyDictionary<TKey, TValue> OrEmptyIfNull<TKey, TValue>(this IReadOnlyDictionary<TKey, TValue> source) =>
           source ?? EmptyMap.Empty<TKey, TValue>();
    }
}
