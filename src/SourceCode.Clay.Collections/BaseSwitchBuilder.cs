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

        private readonly IReadOnlyList<TValue> _values;
        private readonly Func<TKey, int> _indexer;

        #endregion

        #region Constructors

        protected BaseSwitchBuilder(IReadOnlyDictionary<TKey, TValue> cases)
        {
            (_values, _indexer) = BuildSwitchExpression(cases);
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
        protected virtual TKey NormalizeKey(TKey key) => key;

        /// <summary>
        /// A persistent pointer to the <see cref="NormalizeKey(TKey)"/> method.
        /// </summary>
        private static readonly MethodInfo _normalize = typeof(BaseSwitchBuilder<TKey, TValue>).GetMethod(nameof(NormalizeKey), BindingFlags.Instance | BindingFlags.NonPublic);

        /// <summary>
        /// Builds the underlying <see cref="Expression"/> based switch.
        /// </summary>
        /// <param name="cases">The cases to transform into a dynamic switch.</param>
        /// <returns>A lambda that returns an index for a specified key value.</returns>
        private (TValue[] ar, Func<TKey, int> func) BuildSwitchExpression(IReadOnlyDictionary<TKey, TValue> cases)
        {
            TValue[] values;
            Expression<Func<TKey, int>> expr;

            // Return -1 if key is not found (per standard convention for IndexOf())
            var notFound = Expression.Constant(-1);

            // Fast path if no cases
            if (cases == null || cases.Count == 0)
            {
                values = Array.Empty<TValue>();
                expr = Expression.Lambda<Func<TKey, int>>(notFound);
            }
            else
            {
                values = new TValue[cases.Count];
                var normalizedCases = new Dictionary<TKey, int>(cases.Count);

                // Extract valuea and ensure normalized keys are unique
                var i = 0;
                foreach (var @case in cases)
                {
                    values[i] = @case.Value;

                    // Expression MUST match #1 below
                    var normalizedKey = NormalizeKey(@case.Key);
                    normalizedCases[normalizedKey] = i;

                    i++;
                }

                // Define formal parameter
                var formalParam = Expression.Parameter(typeof(TKey), "key");

                // Expression MUST match #1 above
                var @this = Expression.Constant(this);
                var switchValue = Expression.Call(@this, _normalize, formalParam);

                // Create <Key, SwitchCase>[] list
                i = 0;
                var switchCases = new SwitchCase[cases.Count];
                foreach (var @case in normalizedCases)
                {
                    // Get normalized Key
                    var key = Expression.Constant(@case.Key);

                    // Create Case Expression
                    var value = Expression.Constant(@case.Value);
                    switchCases[i] = Expression.SwitchCase(value, key);

                    i++;
                }

                // Create Switch Expression
                var switchExpr = Expression.Switch(switchValue, notFound, switchCases);

                // Create final Expression
                expr = Expression.Lambda<Func<TKey, int>>(switchExpr, formalParam);
            }

            // Compile Expression
            var func = expr.Compile();
            return (values, func);
        }

        #endregion
    }
}
