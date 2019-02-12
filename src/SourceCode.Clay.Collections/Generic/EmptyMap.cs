#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using System;
using System.Collections;
using System.Collections.Generic;

namespace SourceCode.Clay.Collections.Generic
{
    public sealed class EmptyMap<TKey, TValue> : IReadOnlyDictionary<TKey, TValue>
    {
        public static readonly EmptyMap<TKey, TValue> Empty = new EmptyMap<TKey, TValue>();

        public TValue this[TKey key] => throw new KeyNotFoundException();

        public IEnumerable<TKey> Keys => Array.Empty<TKey>();

        public IEnumerable<TValue> Values => Array.Empty<TValue>();

        public int Count => 0;

        private EmptyMap()
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
