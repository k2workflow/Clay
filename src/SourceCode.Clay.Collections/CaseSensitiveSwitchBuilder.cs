using System.Collections.Generic;

namespace SourceCode.Clay.Collections.Generic
{
    /// <summary>
    ///   A dynamic switch for dictionaries with <see cref="System.String"/> keys. Uses a
    ///   case-sensitive (invariant) comparison of key values.
    /// </summary>
    /// <typeparam name="TValue">The type of values.</typeparam>
    internal sealed class CaseSensitiveSwitchBuilder<TValue> : BaseSwitchBuilder<string, TValue>
    {
        #region Constructors

        /// <summary>
        ///   Initializes a new instance of the <see cref="CaseSensitiveSwitchBuilder{TValue}"/> class.
        /// </summary>
        /// <param name="cases">The cases.</param>
        public CaseSensitiveSwitchBuilder(IReadOnlyDictionary<string, TValue> cases)
            : base(cases)
        { }

        #endregion Constructors
    }
}
