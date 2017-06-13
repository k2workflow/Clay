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
        /// Builds a dynamic switch with <see cref="System.String"/> keys.
        /// Uses ordinal string comparison semantics.
        /// </summary>
        /// <typeparam name="T">The type of the elements.</typeparam>
        /// <param name="cases">The items.</param>
        /// <param name="ignoreCase">Invariant lowercase (ordinal) comparisons should be used.</param>
        /// <returns>The compiled switch statement.</returns>
        public static IDynamicSwitch<string, T> ToOrdinalSwitch<T>(IReadOnlyDictionary<string, T> cases, bool ignoreCase)
        {
            var impl = new OrdinalStringSwitchImpl<T>(cases, ignoreCase);
            return impl;
        }

        #region Helpers

        internal sealed class OrdinalStringSwitchImpl<T> : IDynamicSwitch<string, T>
        {
            private readonly Func<string, int> _indexer;
            private readonly IReadOnlyList<T> _values;

            public OrdinalStringSwitchImpl(IReadOnlyDictionary<string, T> cases, bool ignoreCase)
            {
                var count = cases?.Count ?? 0;

                var list = Array.Empty<T>();
                var dict = new Dictionary<string, int>(count);

                if (count > 0)
                {
                    list = new T[count];

                    var i = 0;
                    foreach (var @case in cases)
                    {
                        list[i] = @case.Value;
                        dict[@case.Key] = i;
                        i++;
                    }
                }

                _indexer = Build(dict, ignoreCase);
                _values = list;
            }

            public T this[string key] => _values[_indexer(key)];

            public int Count => _values.Count;

            public bool ContainsKey(string key) => _indexer(key) >= 0;

            public bool TryGetValue(string key, out T value)
            {
                value = default(T);
                var ix = _indexer(key);

                if (ix < 0) return false;

                value = _values[ix];
                return true;
            }

            private static Func<string, int> Build(IReadOnlyDictionary<string, int> cases, bool ignoreCase)
            {
                // Return -1 if item is not found (per standard convention for IndexOf())
                var notFound = Expression.Constant(-1);

                // Exit early if no items
                var count = cases?.Count ?? 0;
                if (count == 0)
                {
                    var noItems = Expression.Lambda<Func<string, int>>(notFound);
                    return noItems.Compile();
                }

                // Define formal parameter
                var formalParam = Expression.Parameter(typeof(string), "key");

                // Format MUST match #1 below
                Expression switchValue = formalParam;
                if (ignoreCase)
                {
                    switchValue = Expression.Call(formalParam, nameof(string.ToLowerInvariant), null);
                }

                // Create <Key, SwitchCase>[] list
                var i = 0;
                var switchCases = new SwitchCase[count];
                foreach (var @case in cases)
                {
                    // Get Key
                    var key = @case.Key;

                    // Format MUST match #1 above
                    if (ignoreCase)
                    {
                        key = key.ToLowerInvariant();
                    }

                    // Create Case Expression
                    var body = Expression.Constant(@case.Value, typeof(T));
                    switchCases[i++] = Expression.SwitchCase(body, Expression.Constant(key));
                }

                // Create Switch Expression
                var switchExpr = Expression.Switch(switchValue, notFound, switchCases);

                // Compile Lambda
                var lambda = Expression.Lambda<Func<string, int>>(switchExpr, formalParam);
                return lambda.Compile();
            }
        }

        #endregion
    }
}
