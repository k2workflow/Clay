using System.Collections;
using System.Collections.Generic;

namespace SourceCode.Clay.Buffers.Tests
{
    internal sealed class ReadOnlyListWrapper<T> : IReadOnlyList<T>
    {
        #region Fields

        private readonly IReadOnlyList<T> _list;

        #endregion Fields

        #region Properties

        public int Count => _list.Count;

        #endregion Properties

        #region Indexers

        public T this[int index] => _list[index];

        #endregion Indexers

        #region Constructors

        public ReadOnlyListWrapper(IReadOnlyList<T> list)
        {
            _list = list;
        }

        #endregion Constructors

        #region Methods

        public IEnumerator<T> GetEnumerator() => _list.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => _list.GetEnumerator();

        #endregion Methods
    }
}
