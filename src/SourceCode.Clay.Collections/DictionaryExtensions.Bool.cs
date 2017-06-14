using System.Collections.Generic;

namespace SourceCode.Clay.Collections.Generic
{
    public static partial class DictionaryExtensions
    {
        /// <summary>
        /// Builds a dynamic switch with <see cref="System.Boolean"/> keys.
        /// </summary>
        /// <typeparam name="T">The type of the elements.</typeparam>
        /// <param name="cases">The items to convert into a dynamic switch statement.</param>
        /// <returns>The compiled switch statement.</returns>
        public static IDynamicSwitch<bool, T> ToDynamicSwitch<T>(this IReadOnlyDictionary<bool, T> cases)
        {
            var impl = new StructSwitchImpl<bool, T>(cases);
            return impl;
        }
    }
}
