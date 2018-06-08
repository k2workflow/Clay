#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using System;
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
        /// <typeparam name="TSource">The type of the elements.</typeparam>
        /// <param name="cases">The items to convert into a dynamic switch statement.</param>
        /// <returns>The compiled switch statement.</returns>
        public static IDynamicSwitch<bool, TSource> ToDynamicSwitch<TSource>(this IReadOnlyDictionary<bool, TSource> cases)
        {
            var impl = new SwitchBuilderImpl<bool, TSource>(cases);
            return impl;
        }

        /// <summary>
        /// Builds a dynamic switch with <see cref="System.Byte"/> keys.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements.</typeparam>
        /// <param name="cases">The items to convert into a dynamic switch statement.</param>
        /// <returns>The compiled switch statement.</returns>
        public static IDynamicSwitch<byte, TSource> ToDynamicSwitch<TSource>(this IReadOnlyDictionary<byte, TSource> cases)
        {
            var impl = new SwitchBuilderImpl<byte, TSource>(cases);
            return impl;
        }

        /// <summary>
        /// Builds a dynamic switch with <see cref="System.SByte"/> keys.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements.</typeparam>
        /// <param name="cases">The items to convert into a dynamic switch statement.</param>
        /// <returns>The compiled switch statement.</returns>
        public static IDynamicSwitch<sbyte, TSource> ToDynamicSwitch<TSource>(this IReadOnlyDictionary<sbyte, TSource> cases)
        {
            var impl = new SwitchBuilderImpl<sbyte, TSource>(cases);
            return impl;
        }

        /// <summary>
        /// Builds a dynamic switch with <see cref="System.Int16"/> keys.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements.</typeparam>
        /// <param name="cases">The items to convert into a dynamic switch statement.</param>
        /// <returns>The compiled switch statement.</returns>
        public static IDynamicSwitch<short, TSource> ToDynamicSwitch<TSource>(this IReadOnlyDictionary<short, TSource> cases)
        {
            var impl = new SwitchBuilderImpl<short, TSource>(cases);
            return impl;
        }

        /// <summary>
        /// Builds a dynamic switch with <see cref="System.UInt16"/> keys.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements.</typeparam>
        /// <param name="cases">The items to convert into a dynamic switch statement.</param>
        /// <returns>The compiled switch statement.</returns>
        public static IDynamicSwitch<ushort, TSource> ToDynamicSwitch<TSource>(this IReadOnlyDictionary<ushort, TSource> cases)
        {
            var impl = new SwitchBuilderImpl<ushort, TSource>(cases);
            return impl;
        }

        /// <summary>
        /// Builds a dynamic switch with <see cref="System.Int32"/> keys.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements.</typeparam>
        /// <param name="cases">The items to convert into a dynamic switch statement.</param>
        /// <returns>The compiled switch statement.</returns>
        public static IDynamicSwitch<int, TSource> ToDynamicSwitch<TSource>(this IReadOnlyDictionary<int, TSource> cases)
        {
            var impl = new SwitchBuilderImpl<int, TSource>(cases);
            return impl;
        }

        /// <summary>
        /// Builds a dynamic switch with <see cref="System.UInt32"/> keys.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements.</typeparam>
        /// <param name="cases">The items to convert into a dynamic switch statement.</param>
        /// <returns>The compiled switch statement.</returns>
        public static IDynamicSwitch<uint, TSource> ToDynamicSwitch<TSource>(this IReadOnlyDictionary<uint, TSource> cases)
        {
            var impl = new SwitchBuilderImpl<uint, TSource>(cases);
            return impl;
        }

        /// <summary>
        /// Builds a dynamic switch with <see cref="System.Int32"/> keys.
        /// </summary>
        /// <typeparam name="TItem">The type of the elements.</typeparam>
        /// <param name="cases">The items.</param>
        /// <param name="keyExtractor">The key extractor.</param>
        /// <param name="valueExtractor">The value extractor.</param>
        /// <returns>The compiled switch statement.</returns>
        public static IDynamicSwitch<int, TValue> ToDynamicSwitch<TItem, TValue>(this IReadOnlyList<TItem> cases, Func<TItem, int> keyExtractor, Func<TItem, TValue> valueExtractor)
        {
            if (cases == null) throw new ArgumentNullException(nameof(cases));
            if (keyExtractor == null) throw new ArgumentNullException(nameof(keyExtractor));
            if (valueExtractor == null) throw new ArgumentNullException(nameof(valueExtractor));

            var unique = new Dictionary<int, TValue>(cases.Count);
            foreach (var @case in cases)
            {
                var key = keyExtractor(@case);
                var value = valueExtractor(@case);

                // Rely on this throwing if there are any duplicates
                unique.Add(key, value);
            }

            var impl = unique.ToDynamicSwitch();
            return impl;
        }

        /// <summary>
        /// Builds a dynamic switch with <see cref="System.Int32"/> keys.
        /// </summary>
        /// <typeparam name="TItem">The type of the elements.</typeparam>
        /// <param name="cases">The items.</param>
        /// <param name="keyExtractor">The key extractor.</param>
        /// <returns>The compiled switch statement.</returns>
        public static IDynamicSwitch<int, TItem> ToDynamicSwitch<TItem>(this IReadOnlyList<TItem> cases, Func<TItem, int> keyExtractor)
        {
            if (cases == null) throw new ArgumentNullException(nameof(cases));
            if (keyExtractor == null) throw new ArgumentNullException(nameof(keyExtractor));

            TItem valueExtractor(TItem item) => item; // Local function
            var impl = cases.ToDynamicSwitch(keyExtractor, valueExtractor);
            return impl;
        }
    }
}
