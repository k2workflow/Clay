using System.Collections.Generic;

namespace SourceCode.Clay.Collections.Generic
{
    public static partial class DictionaryExtensions
    {
        /// <summary>
        /// Builds a dynamic switch with <see cref="System.String"/> keys.
        /// Uses ordinal string comparison semantics.
        /// </summary>
        /// <typeparam name="T">The type of the elements.</typeparam>
        /// <param name="cases">The items to convert into a dynamic switch statement.</param>
        /// <param name="ignoreCase">Invariant lowercase (ordinal) comparisons should be used.</param>
        /// <returns>The compiled switch statement.</returns>
        public static IDynamicSwitch<string, T> ToOrdinalSwitch<T>(this IReadOnlyDictionary<string, T> cases, bool ignoreCase)
        {
            if (ignoreCase)
                return new OrdinalIgnoreStringSwitchImpl<T>(cases);
            else
                return new OrdinalStringSwitchImpl<T>(cases);
        }

        #region Helpers

        internal sealed class OrdinalStringSwitchImpl<T> : BaseSwitchImpl<string, T>
        {
            public OrdinalStringSwitchImpl(IReadOnlyDictionary<string, T> cases)
                : base(cases)
            { }
        }

        internal sealed class OrdinalIgnoreStringSwitchImpl<T> : BaseSwitchImpl<string, T>
        {
            public OrdinalIgnoreStringSwitchImpl(IReadOnlyDictionary<string, T> cases)
                : base(cases)
            { }

            protected override string Normalize(string key) => key.ToLowerInvariant();
        }

        #endregion
    }
}
