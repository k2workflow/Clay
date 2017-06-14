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
        #region Fields

        private readonly Func<TKey, int> _indexer;
        private readonly IReadOnlyList<TValue> _values;

        #endregion

        #region Constructors

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

                    // Expression MUST match #1 elsewhere in this class
                    var normalizedKey = Normalize(@case.Key);

                    dict[normalizedKey] = i;
                    i++;
                }
            }

            _indexer = BuildSwitchExpression(dict);
            _values = list;
        }

        #endregion

        #region IDynamicSwitch

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

        #endregion

        #region Helpers

        /// <summary>
        /// Override this method if the keys need to be transformed in some manner before
        /// being compared. For example, changing string keys to their lowercase equivalent.
        /// </summary>
        /// <param name="key">The key value to be transformed.</param>
        /// <returns>The transformed key value.</returns>
        protected virtual TKey Normalize(TKey key) => key;

        /// <summary>
        /// A persistent pointer to the <see cref="Normalize(TKey)"/> method.
        /// </summary>
        private static readonly MethodInfo _normalize = typeof(BaseSwitchBuilder<TKey, TValue>).GetMethod(nameof(Normalize), BindingFlags.Instance | BindingFlags.NonPublic);

        /// <summary>
        /// Builds the underlying <see cref="Expression"/> based switch.
        /// </summary>
        /// <param name="cases">The cases to transform into a dynamic switch.</param>
        /// <returns>A lambda that returns an index for a specified key value.</returns>
        private Func<TKey, int> BuildSwitchExpression(IReadOnlyDictionary<TKey, int> cases)
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

            // Expression MUST match #1 elsewhere in this class
            var @this = Expression.Constant(this);
            var switchValue = Expression.Call(@this, _normalize, formalParam);

            // Create <Key, SwitchCase>[] list
            var i = 0;
            var switchCases = new SwitchCase[count];
            foreach (var @case in cases)
            {
                // Get (already normalized) Key
                var key = @case.Key;

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

        #endregion
    }
}
