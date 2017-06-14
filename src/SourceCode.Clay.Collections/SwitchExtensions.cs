using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq.Expressions;

namespace SourceCode.Clay.Collections.Generic
{
    /// <summary>
    /// Represents extensions that generate dynamic switches from dictionaries and lists.
    /// </summary>
    /// <seealso cref="Expression.Switch"/>
    public static class SwitchExtensions
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
                return new CaseInsensitiveSwitchBuilder<T>(cases);

            return new CaseSensitiveSwitchBuilder<T>(cases);
        }

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
        /// Builds an ordinal switch expression.
        /// </summary>
        /// <typeparam name="T">The type of the elements.</typeparam>
        /// <param name="items">The items.</param>
        /// <param name="keyExtractor">The key extractor.</param>
        /// <param name="ignoreCase">if set to <c>true</c> case will be ignored.</param>
        /// <returns>The compiled switch statement.</returns>
        public static Func<string, int> BuildOrdinalSwitchExpression<T>(this IReadOnlyList<T> items, Func<T, string> keyExtractor, bool ignoreCase)
        {
            if (items == null) throw new ArgumentNullException(nameof(items));
            if (keyExtractor == null) throw new ArgumentNullException(nameof(keyExtractor));

            // Return -1 if item is not found (per standard convention for IndexOf())
            var notFound = Expression.Constant(-1);

            // Exit early if no items
            if (items == null || items.Count == 0)
            {
                var noItems = Expression.Lambda<Func<string, int>>(notFound);
                return noItems.Compile();
            }

            // Define formal parameter & ensure value is UPPER
            var formalParam = Expression.Parameter(typeof(string), "key");

            // Format MUST match #1 below
            Expression switchValue = formalParam;
            if (ignoreCase)
            {
                switchValue = Expression.Call(formalParam, nameof(string.ToUpperInvariant), null);
            }

            // Create <Key, SwitchCase>[] list
            var switchCases = new SwitchCase[items.Count];
            for (var i = 0; i < switchCases.Length; i++)
            {
                // Extract Key from T
                var key = keyExtractor(items[i]);
                Debug.Assert(!string.IsNullOrWhiteSpace(key));

                // Format MUST match #1 above
                if (ignoreCase)
                {
                    key = key.ToUpperInvariant();
                }

                // Create Case Expression
                var body = Expression.Constant(i);
                switchCases[i] = Expression.SwitchCase(body, Expression.Constant(key));
            }

            // Create Switch Expression
            var switchExpr = Expression.Switch(switchValue, notFound, switchCases);

            // Compile Lambda
            var lambda = Expression.Lambda<Func<string, int>>(switchExpr, formalParam);
            return lambda.Compile();
        }

        /// <summary>
        /// Builds an ordinal switch expression.
        /// </summary>
        /// <typeparam name="TElement">The type of the elements.</typeparam>
        /// <typeparam name="TValue">The type of the return value.</typeparam>
        /// <param name="items">The items.</param>
        /// <param name="keyExtractor">The key extractor.</param>
        /// <param name="valueExtractor">The value extractor.</param>
        /// <returns>The compiled switch statement.</returns>

        public static Func<TKey, TValue> BuildSwitchExpression<TElement, TKey, TValue>(this IReadOnlyList<TElement> items, Func<TElement, TKey> keyExtractor, Func<TElement, TValue> valueExtractor)
        {
            if (items == null) throw new ArgumentNullException(nameof(items));
            if (keyExtractor == null) throw new ArgumentNullException(nameof(keyExtractor));
            if (valueExtractor == null) throw new ArgumentNullException(nameof(valueExtractor));

            var tValue = typeof(TValue);

            // Return default(TValue) if item is not found
            var notFound = Expression.Default(tValue);

            // Exit early if no items
            if (items == null || items.Count == 0)
            {
                var noItems = Expression.Lambda<Func<TKey, TValue>>(notFound);
                return noItems.Compile();
            }

            // Define formal parameter
            var formalParam = Expression.Parameter(typeof(TKey), "key");

            // Create <Key, SwitchCase>[] list
            var switchCases = new SwitchCase[items.Count];
            for (var i = 0; i < switchCases.Length; i++)
            {
                // Extract Key from T
                var key = keyExtractor(items[i]);
                var value = valueExtractor(items[i]);

                // Create Case Expression
                var body = Expression.Constant(value, tValue);
                switchCases[i] = Expression.SwitchCase(body, Expression.Constant(key));
            }

            // Create Switch Expression
            var switchExpr = Expression.Switch(formalParam, notFound, switchCases);

            // Compile Lambda
            var lambda = Expression.Lambda<Func<TKey, TValue>>(switchExpr, formalParam);
            return lambda.Compile();
        }
    }
}
