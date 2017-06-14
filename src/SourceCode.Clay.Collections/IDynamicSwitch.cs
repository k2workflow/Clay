namespace SourceCode.Clay.Collections.Generic
{
    /// <summary>
    /// Interface used for exposing dynamic switch statements.
    /// The members are very similar to those exposed by <see cref="System.Collections.IDictionary"/>.
    /// </summary>
    /// <typeparam name="TKey">The type of key used in the switch.</typeparam>
    /// <typeparam name="TValue">The type of value used in the switch.</typeparam>
    public interface IDynamicSwitch<in TKey, TValue>
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        TValue this[TKey key] { get; }

        /// <summary>
        ///
        /// </summary>
        int Count { get; }

        /// <summary>
        ///
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        bool ContainsKey(TKey key);

        /// <summary>
        ///
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        bool TryGetValue(TKey key, out TValue value);
    }
}
