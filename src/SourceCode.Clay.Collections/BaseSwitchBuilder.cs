using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace SourceCode.Clay.Collections.Generic
{
    /// <summary>
    /// Base class for dynamic switches.
    /// </summary>
    /// <typeparam name="TKey">The type of keys.</typeparam>
    /// <typeparam name="TValue">The type of values.</typeparam>
    internal abstract class BaseSwitchBuilder<TKey, TValue> : IDynamicSwitch<TKey, TValue>
    {
        private readonly Func<TKey, int> _indexer;
        private readonly IReadOnlyList<TValue> _values;

        protected BaseSwitchBuilder(IReadOnlyDictionary<TKey, TValue> cases)
        {
            var count = cases?.Count ?? 0;

            var list = Array.Empty<TValue>();
            var dict = new Dictionary<TKey, int>(count);

            if (count > 0)
            {
                list = new TValue[count];

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

        public TValue this[TKey key] => _values[_indexer(key)];

        public int Count => _values.Count;

        public bool ContainsKey(TKey key) => _indexer(key) >= 0;

        public bool TryGetValue(TKey key, out TValue value)
        {
            value = default(TValue);
            var ix = _indexer(key);

            if (ix < 0) return false;

            value = _values[ix];
            return true;
        }

        /// <summary>
        /// Override this method if the keys need to be transformed in some manner before
        /// being compared. For example, changing string keys to their lowercase equivalent.
        /// </summary>
        /// <param name="key">The key value to be transformed.</param>
        /// <returns>The transformed key value.</returns>
        protected virtual TKey Normalize(TKey key) => key;

        /// <summary>
        /// Builds the underlying <see cref="Expression"/> based switch.
        /// </summary>
        /// <param name="cases">The cases to transform into a dynamic switch.</param>
        /// <returns>A lambda that returns an index for a specified key value.</returns>
        private Func<TKey, int> Build(IReadOnlyDictionary<TKey, int> cases)
        {
            // Return -1 if item is not found (per standard convention for IndexOf())
            var notFound = Expression.Constant(-1);

            // Exit early if no items
            var count = cases?.Count ?? 0;
            if (count == 0)
            {
                var noItems = Expression.Lambda<Func<TKey, int>>(notFound);
                return noItems.Compile();
            }

            // Define formal parameter
            var formalParam = Expression.Parameter(typeof(TKey), "key");

            // Expression MUST match #1 below
            var mi = this.GetType().GetMethod(nameof(Normalize), BindingFlags.Instance | BindingFlags.NonPublic);
            var @this = Expression.Constant(this);
            var switchValue = Expression.Call(@this, mi, formalParam);

            // Create <Key, SwitchCase>[] list
            var i = 0;
            var switchCases = new SwitchCase[count];
            foreach (var @case in cases)
            {
                // Get Key
                var key = @case.Key;

                // Expression MUST match #1 above
                key = Normalize(key);

                // Create Case Expression
                var body = Expression.Constant(@case.Value);
                switchCases[i++] = Expression.SwitchCase(body, Expression.Constant(key));
            }

            // Create Switch Expression
            var switchExpr = Expression.Switch(switchValue, notFound, switchCases);

            // Compile Lambda
            var lambda = Expression.Lambda<Func<TKey, int>>(switchExpr, formalParam);
            return lambda.Compile();
        }
    }
}
