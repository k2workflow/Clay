#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using System.Collections;
using System.Collections.Generic;

namespace SourceCode.Clay.Buffers.Tests
{
    internal sealed class ReadOnlyListWrapper<T> : IReadOnlyList<T>
    {
        private readonly IReadOnlyList<T> _list;

        public int Count => _list.Count;

        public T this[int index] => _list[index];

        public ReadOnlyListWrapper(IReadOnlyList<T> list)
        {
            _list = list;
        }

        public IEnumerator<T> GetEnumerator() => _list.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => _list.GetEnumerator();
    }
}
