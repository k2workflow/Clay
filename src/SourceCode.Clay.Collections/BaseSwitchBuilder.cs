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

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseSwitchBuilder{TKey, TValue}"/> class.
        /// </summary>
        /// <param name="cases">The cases.</param>
        protected BaseSwitchBuilder(IReadOnlyDictionary<TKey, TValue> cases)
        {
            var (values, indexer) = BuildSwitchExpression(cases);

            _values = values;
            _indexer = indexer;
        }

        #endregion

        #region IDynamicSwitch

        /// <summary>
        /// Gets the value with the specified key.
        /// </summary>
        /// <value>
        /// The <see cref="TValue"/>.
        /// </value>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public TValue this[TKey key] => _values[_indexer(key)];

        /// <summary>
        /// The number of items in the switch.
        /// </summary>
        public int Count => _values.Count;

        /// <summary>
        /// Checks whether the specified key is present in the switch.
        /// </summary>
        /// <param name="key">The key value.</param>
        /// <returns></returns>
        public bool ContainsKey(TKey key) => _indexer(key) >= 0;

        /// <summary>
        /// Attempts to get the value corresponding to the specified key.
        /// </summary>
        /// <param name="key">The key value.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public bool TryGetValue(TKey key, out TValue value)
        {
            value = default;
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

                // Extract values and ensure normalized keys are unique
                var i = 0;
                foreach (var @case in cases)
                {
                    values[i] = @case.Value;

                    // Expression MUST match #1 below
                    var normalizedKey = NormalizeKey(@case.Key);
                    normalizedCases.Add(normalizedKey, i); // Rely on this throwing if there are any duplicates

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
