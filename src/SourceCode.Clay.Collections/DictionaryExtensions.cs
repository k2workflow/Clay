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
        internal abstract class BaseSwitchImpl<K, V> : IDynamicSwitch<K, V>
        {
            private readonly Func<K, int> _indexer;
            private readonly IReadOnlyList<V> _values;

            protected BaseSwitchImpl(IReadOnlyDictionary<K, V> cases, Func<K, K> normalize)
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

                _indexer = Build(dict, normalize);
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

            private static Func<K, int> Build(IReadOnlyDictionary<K, int> cases, Func<K, K> normalize)
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
                if (normalize != null)
                {
                    switchValue = (Expression<Func<K, K>>)(k => normalize(k));
                    //switchValue = Expression.Call(formalParam, nameof(string.ToLowerInvariant), null);
                }

                // Create <Key, SwitchCase>[] list
                var i = 0;
                var switchCases = new SwitchCase[count];
                foreach (var @case in cases)
                {
                    // Get Key
                    var key = @case.Key;

                    // Format MUST match #1 above
                    if (normalize != null)
                    {
                        key = normalize(key);
                    }

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
    }
}
