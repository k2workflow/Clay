using System.Collections.Generic;

namespace SourceCode.Clay.Collections.Generic
{
    /// <summary>
    /// Useful properties and methods for <see cref="IDictionary{TKey, TValue}"/>.
    /// </summary>
    public static class Dictionary
    {
        #region Constants

        /// <summary>
        /// Returns an empty dictionary that is immutable.
        /// </summary>
        /// <typeparam name="TKey">The type of keys in the dictionary.</typeparam>
        /// <typeparam name="TValue">The type of values in the dictionary.</typeparam>
        /// <returns>Returns an empty <see cref="IDictionary{TKey, TValue}".</returns>
        public static IDictionary<TKey, TValue> Empty<TKey, TValue>() => EmptyDictionaryImpl<TKey, TValue>.Value;

        #endregion
    }
}
