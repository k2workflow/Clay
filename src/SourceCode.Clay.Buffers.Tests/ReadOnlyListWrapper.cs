using System.Collections;
using System.Collections.Generic;

namespace SourceCode.Clay.Buffers.Tests
{
    internal sealed class ReadOnlyListWrapper<T> : IReadOnlyList<T>
    {
        public T this[int index] => _list[index];

        public int Count => _list.Count;

        private readonly IReadOnlyList<T> _list;

        public ReadOnlyListWrapper(IReadOnlyList<T> list)
        {
            _list = list;
        }

        public IEnumerator<T> GetEnumerator() => _list.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => _list.GetEnumerator();
    }
}
