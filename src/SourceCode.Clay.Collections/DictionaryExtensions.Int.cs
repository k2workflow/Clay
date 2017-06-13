using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace SourceCode.Clay.Collections.Generic
{
    /// <summary>
    /// Represents extensions for lists.
    /// </summary>
    public static partial class DictionaryExtensions
    {
        /// <summary>
        /// Builds a dynamic switch with <see cref="System.Byte"/> keys.
        /// </summary>
        /// <typeparam name="T">The type of the elements.</typeparam>
        /// <param name="cases">The items to convert into a dynamic switch statement.</param>
        /// <returns>The compiled switch statement.</returns>
        public static IDynamicSwitch<byte, T> ToDynamicSwitch<T>(IReadOnlyDictionary<byte, T> cases)
        {
            var proxy = new StructSwitchImpl<byte, T>(cases);
            return proxy;
        }

        /// <summary>
        /// Builds a dynamic switch with <see cref="System.SByte"/> keys.
        /// </summary>
        /// <typeparam name="T">The type of the elements.</typeparam>
        /// <param name="cases">The items to convert into a dynamic switch statement.</param>
        /// <returns>The compiled switch statement.</returns>
        public static IDynamicSwitch<sbyte, T> ToDynamicSwitch<T>(IReadOnlyDictionary<sbyte, T> cases)
        {
            var proxy = new StructSwitchImpl<sbyte, T>(cases);
            return proxy;
        }

        /// <summary>
        /// Builds a dynamic switch with <see cref="System.Int16"/> keys.
        /// </summary>
        /// <typeparam name="T">The type of the elements.</typeparam>
        /// <param name="cases">The items to convert into a dynamic switch statement.</param>
        /// <returns>The compiled switch statement.</returns>
        public static IDynamicSwitch<short, T> ToDynamicSwitch<T>(IReadOnlyDictionary<short, T> cases)
        {
            var proxy = new StructSwitchImpl<short, T>(cases);
            return proxy;
        }

        /// <summary>
        /// Builds a dynamic switch with <see cref="System.UInt16"/> keys.
        /// </summary>
        /// <typeparam name="T">The type of the elements.</typeparam>
        /// <param name="cases">The items to convert into a dynamic switch statement.</param>
        /// <returns>The compiled switch statement.</returns>
        public static IDynamicSwitch<ushort, T> ToDynamicSwitch<T>(IReadOnlyDictionary<ushort, T> cases)
        {
            var proxy = new StructSwitchImpl<ushort, T>(cases);
            return proxy;
        }

        /// <summary>
        /// Builds a dynamic switch with <see cref="System.Int32"/> keys.
        /// </summary>
        /// <typeparam name="T">The type of the elements.</typeparam>
        /// <param name="cases">The items to convert into a dynamic switch statement.</param>
        /// <returns>The compiled switch statement.</returns>
        public static IDynamicSwitch<int, T> ToDynamicSwitch<T>(IReadOnlyDictionary<int, T> cases)
        {
            var proxy = new StructSwitchImpl<int, T>(cases);
            return proxy;
        }

        /// <summary>
        /// Builds a dynamic switch with <see cref="System.UInt32"/> keys.
        /// </summary>
        /// <typeparam name="T">The type of the elements.</typeparam>
        /// <param name="cases">The items to convert into a dynamic switch statement.</param>
        /// <returns>The compiled switch statement.</returns>
        public static IDynamicSwitch<uint, T> ToDynamicSwitch<T>(IReadOnlyDictionary<uint, T> cases)
        {
            var proxy = new StructSwitchImpl<uint, T>(cases);
            return proxy;
        }

        #region Helpers

        internal sealed class StructSwitchImpl<K, V> : IDynamicSwitch<K, V>
            where K : struct, IEquatable<K>
        {
            private readonly Func<K, int> _indexer;
            private readonly IReadOnlyList<V> _values;

            public StructSwitchImpl(IReadOnlyDictionary<K, V> cases)
            {
                var count = cases?.Count ?? 0;

                var list = Array.Empty<V>();
                var dict = new Dictionary<K, int>(count);

                if (count > 0)
                {
                    list = new V[count];

                    var i = 0;
                    foreach (var @case in cases)
                    {
                        list[i] = @case.Value;
                        dict[@case.Key] = i;
                        i++;
                    }
                }

                _indexer = Build(dict);
                _values = list;
            }

            public V this[K key] => _values[_indexer(key)];

            public int Count => _values.Count;

            public bool ContainsKey(K key) => _indexer(key) >= 0;

            public bool TryGetValue(K key, out V value)
            {
                value = default(V);
                var ix = _indexer(key);

                if (ix < 0) return false;

                value = _values[ix];
                return true;
            }

            private static Func<K, int> Build(IReadOnlyDictionary<K, int> cases)
            {
                // Return -1 if item is not found (per standard convention for IndexOf())
                var notFound = Expression.Constant(-1);

                // Exit early if no items
                var count = cases?.Count ?? 0;
                if (count == 0)
                {
                    var noItems = Expression.Lambda<Func<K, int>>(notFound);
                    return noItems.Compile();
                }

                // Define formal parameter
                var formalParam = Expression.Parameter(typeof(K), "key");

                // Format MUST match #1 below
                Expression switchValue = formalParam;

                // Create <Key, SwitchCase>[] list
                var i = 0;
                var switchCases = new SwitchCase[count];
                foreach (var @case in cases)
                {
                    // Get Key
                    var key = @case.Key;

                    // Create Case Expression
                    var body = Expression.Constant(@case.Value, typeof(V));
                    switchCases[i++] = Expression.SwitchCase(body, Expression.Constant(key));
                }

                // Create Switch Expression
                var switchExpr = Expression.Switch(switchValue, notFound, switchCases);

                // Compile Lambda
                var lambda = Expression.Lambda<Func<K, int>>(switchExpr, formalParam);
                return lambda.Compile();
            }
        }

        #endregion
    }
}
