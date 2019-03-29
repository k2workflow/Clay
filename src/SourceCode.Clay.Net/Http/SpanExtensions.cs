using System;

namespace SourceCode.Clay.Net.Http
{
    internal static class SpanExtensions
    {
        internal static int IndexOf<T>(this ReadOnlySpan<T> span, T value, int startIndex)
            where T : IEquatable<T>
        {
            for (; startIndex < span.Length; startIndex++)
                if (span[startIndex].Equals(value))
                    return startIndex;
            return -1;
        }

        internal static (T, int) IndexOf<T>(this ReadOnlySpan<T> span, T value1, T value2, int startIndex)
            where T : IEquatable<T>
        {
            for (; startIndex < span.Length; startIndex++)
                if (span[startIndex].Equals(value1))
                    return (value1, startIndex);
                else if (span[startIndex].Equals(value2))
                    return (value2, startIndex);
            return (default, -1);
        }
    }
}
