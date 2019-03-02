using System;
using System.Collections.Generic;

namespace SourceCode.Clay.Collections.Generic
{
    /// <summary>
    /// Represents <see cref="IReadOnlyList{T}"/> extensions.
    /// </summary>
    public static class IReadOnlyListExtensions
    {
        public static IReadOnlyList<T> OrEmptyIfNull<T>(this IReadOnlyList<T> source) =>
           source ?? Array.Empty<T>();
    }
}
