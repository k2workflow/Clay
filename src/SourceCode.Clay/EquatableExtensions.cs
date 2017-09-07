using System;
using System.Runtime.CompilerServices;

namespace SourceCode.Clay
{
    /// <summary>
    /// Represents <see cref="IEquatable{T}"/> extensions.
    /// </summary>
    public static class EquatableExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool NullableEquals<T>(this T x, T y)
            where T : IEquatable<T>
        {
            var xNull = ReferenceEquals(x, null);
            if (xNull ^ ReferenceEquals(y, null)) return false; // One is null but not the other
            if (xNull) return true; // Both are null

            // Both are not null; we can now test their values
            if (ReferenceEquals(x, y)) return true; // Same reference

            return x.Equals(y);
        }
    }
}
