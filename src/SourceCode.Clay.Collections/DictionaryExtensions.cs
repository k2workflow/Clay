using System.Collections.Generic;

namespace SourceCode.Clay.Collections.Generic
{
    public static partial class DictionaryExtensions // .ToDynamicSwitch
    {
        /// <summary>
        /// Builds a dynamic switch with <see cref="System.String"/> keys.
        /// Uses ordinal string comparison semantics.
        /// </summary>
        /// <typeparam name="T">The type of the elements.</typeparam>
        /// <param name="cases">The items to convert into a dynamic switch statement.</param>
        /// <param name="ignoreCase">Invariant lowercase (ordinal) comparisons should be used.</param>
        /// <returns>The compiled switch statement.</returns>
        public static IDynamicSwitch<string, T> ToDynamicSwitch<T>(this IReadOnlyDictionary<string, T> cases, bool ignoreCase)
        {
            if (ignoreCase)
                return new StringOrdinalIgnoreSwitchBuilder<T>(cases);

            return new StringOrdinalSwitchBuilder<T>(cases);
        }

        /// <summary>
        /// Builds a dynamic switch with <see cref="System.Boolean"/> keys.
        /// </summary>
        /// <typeparam name="T">The type of the elements.</typeparam>
        /// <param name="cases">The items to convert into a dynamic switch statement.</param>
        /// <returns>The compiled switch statement.</returns>
        public static IDynamicSwitch<bool, T> ToDynamicSwitch<T>(this IReadOnlyDictionary<bool, T> cases)
        {
            var impl = new SwitchBuilder<bool, T>(cases);
            return impl;
        }

        /// <summary>
        /// Builds a dynamic switch with <see cref="System.Byte"/> keys.
        /// </summary>
        /// <typeparam name="T">The type of the elements.</typeparam>
        /// <param name="cases">The items to convert into a dynamic switch statement.</param>
        /// <returns>The compiled switch statement.</returns>
        public static IDynamicSwitch<byte, T> ToDynamicSwitch<T>(this IReadOnlyDictionary<byte, T> cases)
        {
            var impl = new SwitchBuilder<byte, T>(cases);
            return impl;
        }

        /// <summary>
        /// Builds a dynamic switch with <see cref="System.SByte"/> keys.
        /// </summary>
        /// <typeparam name="T">The type of the elements.</typeparam>
        /// <param name="cases">The items to convert into a dynamic switch statement.</param>
        /// <returns>The compiled switch statement.</returns>
        public static IDynamicSwitch<sbyte, T> ToDynamicSwitch<T>(this IReadOnlyDictionary<sbyte, T> cases)
        {
            var impl = new SwitchBuilder<sbyte, T>(cases);
            return impl;
        }

        /// <summary>
        /// Builds a dynamic switch with <see cref="System.Int16"/> keys.
        /// </summary>
        /// <typeparam name="T">The type of the elements.</typeparam>
        /// <param name="cases">The items to convert into a dynamic switch statement.</param>
        /// <returns>The compiled switch statement.</returns>
        public static IDynamicSwitch<short, T> ToDynamicSwitch<T>(this IReadOnlyDictionary<short, T> cases)
        {
            var impl = new SwitchBuilder<short, T>(cases);
            return impl;
        }

        /// <summary>
        /// Builds a dynamic switch with <see cref="System.UInt16"/> keys.
        /// </summary>
        /// <typeparam name="T">The type of the elements.</typeparam>
        /// <param name="cases">The items to convert into a dynamic switch statement.</param>
        /// <returns>The compiled switch statement.</returns>
        public static IDynamicSwitch<ushort, T> ToDynamicSwitch<T>(this IReadOnlyDictionary<ushort, T> cases)
        {
            var impl = new SwitchBuilder<ushort, T>(cases);
            return impl;
        }

        /// <summary>
        /// Builds a dynamic switch with <see cref="System.Int32"/> keys.
        /// </summary>
        /// <typeparam name="T">The type of the elements.</typeparam>
        /// <param name="cases">The items to convert into a dynamic switch statement.</param>
        /// <returns>The compiled switch statement.</returns>
        public static IDynamicSwitch<int, T> ToDynamicSwitch<T>(this IReadOnlyDictionary<int, T> cases)
        {
            var impl = new SwitchBuilder<int, T>(cases);
            return impl;
        }

        /// <summary>
        /// Builds a dynamic switch with <see cref="System.UInt32"/> keys.
        /// </summary>
        /// <typeparam name="T">The type of the elements.</typeparam>
        /// <param name="cases">The items to convert into a dynamic switch statement.</param>
        /// <returns>The compiled switch statement.</returns>
        public static IDynamicSwitch<uint, T> ToDynamicSwitch<T>(this IReadOnlyDictionary<uint, T> cases)
        {
            var impl = new SwitchBuilder<uint, T>(cases);
            return impl;
        }
    }
}
