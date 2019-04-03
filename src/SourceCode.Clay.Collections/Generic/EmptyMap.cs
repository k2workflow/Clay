#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using System.Collections;
using System.Collections.Generic;

namespace SourceCode.Clay.Collections.Generic
{
    /// <summary>
    /// Represents an empty <see cref="IReadOnlyDictionary{TKey, TValue}"/> instance.
    /// </summary>
    public sealed class EmptyMap
    {
        public static IReadOnlyDictionary<TKey, TValue> Empty<TKey, TValue>() => new EmptyMapImpl<TKey, TValue>();

        private sealed class EmptyMapImpl<TKey, TValue> : IReadOnlyDictionary<TKey, TValue>
        {
            public TValue this[TKey key] => throw new KeyNotFoundException();

            public IEnumerable<TKey> Keys => System.Linq.Enumerable.Empty<TKey>();

            public IEnumerable<TValue> Values => System.Linq.Enumerable.Empty<TValue>();

            public int Count => 0;

            public EmptyMapImpl()
            { }

            public bool ContainsKey(TKey key)
                => false;

            public bool TryGetValue(TKey key, out TValue value)
            {
                value = default;
                return false;
            }

            public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
            {
                yield break;
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                yield break;
            }
        }
    }
}
