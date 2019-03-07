using System.Collections.Generic;

namespace SourceCode.Clay.Collections.Generic
{
    /// <summary>
    /// Represents <see cref="IEnumerable{T}"/> extensions.
    /// </summary>
    public static class IEnumerableExtensions
    {
        public static IEnumerable<T> OrEmptyIfNull<T>(this IEnumerable<T> source)
            => source ?? System.Linq.Enumerable.Empty<T>();
    }
}
