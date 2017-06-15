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
        /// <summary>
        /// Initializes a new instance of the <see cref="CaseInsensitiveSwitchBuilder{TValue}"/> class.
        /// </summary>
        /// <param name="cases">The cases.</param>
        public CaseInsensitiveSwitchBuilder(IReadOnlyDictionary<string, TValue> cases)
            : base(cases)
        { }

        /// <summary>
        /// Normalizes each switch key, so that comparisons are case-insensitive.
        /// </summary>
        /// <param name="key">The key value to be transformed.</param>
        /// <returns>
        /// The transformed key value.
        /// </returns>
        protected override string NormalizeKey(string key) => key.ToUpperInvariant(); // Apparently UC (vs LC) is optimized in CLR
    }
}
