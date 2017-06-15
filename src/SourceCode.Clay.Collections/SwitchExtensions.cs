﻿using System;
using System.Collections.Generic;

namespace SourceCode.Clay.Collections.Generic
{
    /// <summary>
    /// Represents extensions that generate dynamic switches from dictionaries and lists.
    /// </summary>
    /// <seealso cref="Expression.Switch"/>
    public static class SwitchExtensions
    {
        /// <summary>
        /// Builds a dynamic switch with <see cref="System.Boolean"/> keys.
        /// </summary>
        /// <typeparam name="T">The type of the elements.</typeparam>
        /// <param name="cases">The items to convert into a dynamic switch statement.</param>
        /// <returns>The compiled switch statement.</returns>
        public static IDynamicSwitch<bool, T> ToDynamicSwitch<T>(this IReadOnlyDictionary<bool, T> cases)
        {
            var impl = new StructSwitchBuilder<bool, T>(cases);
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
            var impl = new StructSwitchBuilder<byte, T>(cases);
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
            var impl = new StructSwitchBuilder<sbyte, T>(cases);
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
            var impl = new StructSwitchBuilder<short, T>(cases);
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
            var impl = new StructSwitchBuilder<ushort, T>(cases);
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
            var impl = new StructSwitchBuilder<int, T>(cases);
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
            var impl = new StructSwitchBuilder<uint, T>(cases);
            return impl;
        }

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
                return new CaseInsensitiveSwitchBuilder<T>(cases);

            return new CaseSensitiveSwitchBuilder<T>(cases);
        }

        /// <summary>
        /// Builds a dynamic switch with <see cref="System.String"/> keys.
        /// </summary>
        /// <typeparam name="TItem">The type of the elements.</typeparam>
        /// <param name="cases">The items.</param>
        /// <param name="keyExtractor">The key extractor.</param>
        /// <param name="valueExtractor">The value extractor.</param>
        /// <param name="ignoreCase">if set to <c>true</c> case will be ignored.</param>
        /// <returns>The compiled switch statement.</returns>
        public static IDynamicSwitch<string, TValue> ToDynamicSwitch<TItem, TValue>(this IReadOnlyList<TItem> cases, Func<TItem, string> keyExtractor, Func<TItem, TValue> valueExtractor, bool ignoreCase)
        {
            if (cases == null) throw new ArgumentNullException(nameof(cases));
            if (keyExtractor == null) throw new ArgumentNullException(nameof(keyExtractor));
            if (valueExtractor == null) throw new ArgumentNullException(nameof(valueExtractor));

            var dict = new Dictionary<string, TValue>(cases.Count);
            foreach (var @case in cases)
            {
                var key = keyExtractor(@case);
                var value = valueExtractor(@case);

                dict.Add(key, value); // Rely on this throwing if there are any duplicates
            }

            var impl = dict.ToDynamicSwitch(ignoreCase);
            return impl;
        }

        /// <summary>
        /// Builds a dynamic switch with <see cref="System.String"/> keys.
        /// </summary>
        /// <typeparam name="TItem">The type of the elements.</typeparam>
        /// <param name="cases">The items.</param>
        /// <param name="keyExtractor">The key extractor.</param>
        /// <param name="ignoreCase">if set to <c>true</c> case will be ignored.</param>
        /// <returns>The compiled switch statement.</returns>
        public static IDynamicSwitch<string, TItem> ToDynamicSwitch<TItem>(this IReadOnlyList<TItem> cases, Func<TItem, string> keyExtractor, bool ignoreCase)
        {
            if (cases == null) throw new ArgumentNullException(nameof(cases));
            if (keyExtractor == null) throw new ArgumentNullException(nameof(keyExtractor));

            var impl = cases.ToDynamicSwitch(keyExtractor, v => v, ignoreCase);
            return impl;
        }
    }
}
