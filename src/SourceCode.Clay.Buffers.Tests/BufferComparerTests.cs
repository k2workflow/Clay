using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace SourceCode.Clay.Buffers.Tests
{
    public static class BufferComparerTests
    {
        private static byte[] GenerateArray(int length, int offset = 0)
        {
            var result = new byte[length];
            for (var i = 1; i < length; i++)
                result[i] = (byte)((result[i - 1] + i + offset) & 0xFF);
            return result;
        }

        #region GetHashCode

        [Trait("Type", "Unit")]
        [Fact(DisplayName = "BufferComparer GetHashCode ArrayShort")]
        public static void BufferComparer_GetHashCode_ArrayShort()
        {
            var bytes = GenerateArray(16);
            var hash = BufferComparer.Default.GetHashCode(bytes);
            Assert.Equal(-779918115, hash);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = "BufferComparer GetHashCode ArrayMedium")]
        public static void BufferComparer_GetHashCode_ArrayMedium()
        {
            // NB: Fidelity

            var bytes = GenerateArray(712);
            var hash = BufferComparer.Default.GetHashCode(bytes);
            Assert.Equal(1507092677, hash);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = "BufferComparer GetHashCode ArrayLong")]
        public static void BufferComparer_GetHashCode_ArrayLong()
        {
            var bytes = GenerateArray(1024);
            var hash = BufferComparer.Default.GetHashCode(bytes);
            Assert.Equal(1507092677, hash);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = "BufferComparer GetHashCode ListShort")]
        public static void BufferComparer_GetHashCode_ListShort()
        {
            var bytes = GenerateArray(16).ToList();
            var hash = BufferComparer.Default.GetHashCode((IList<byte>)bytes);
            Assert.Equal(-779918115, hash);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = "BufferComparer GetHashCode ListMedium")]
        public static void BufferComparer_GetHashCode_ListMedium()
        {
            // NB: Fidelity

            var bytes = GenerateArray(712).ToList();
            var hash = BufferComparer.Default.GetHashCode((IList<byte>)bytes);
            Assert.Equal(1507092677, hash);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = "BufferComparer GetHashCode ListLong")]
        public static void BufferComparer_GetHashCode_ListLong()
        {
            var bytes = GenerateArray(1024).ToList();
            var hash = BufferComparer.Default.GetHashCode((IList<byte>)bytes);
            Assert.Equal(1507092677, hash);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = "BufferComparer GetHashCode ReadOnlyListShort")]
        public static void BufferComparer_GetHashCode_ReadOnlyListShort()
        {
            var bytes = GenerateArray(16).ToList();
            var hash = BufferComparer.Default.GetHashCode((IReadOnlyList<byte>)bytes);
            Assert.Equal(-779918115, hash);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = "BufferComparer GetHashCode ReadOnlyListMedium")]
        public static void BufferComparer_GetHashCode_ReadOnlyListMedium()
        {
            // NB: Fidelity

            var bytes = GenerateArray(712).ToList();
            var hash = BufferComparer.Default.GetHashCode((IReadOnlyList<byte>)bytes);
            Assert.Equal(1507092677, hash);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = "BufferComparer GetHashCode ReadOnlyListLong")]
        public static void BufferComparer_GetHashCode_ReadOnlyListLong()
        {
            var bytes = GenerateArray(1024).ToList();
            var hash = BufferComparer.Default.GetHashCode((IReadOnlyList<byte>)bytes);
            Assert.Equal(1507092677, hash);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = "BufferComparer GetHashCode EnumerableShort")]
        public static void BufferComparer_GetHashCode_EnumerableShort()
        {
            var bytes = GenerateArray(16).Take(16);
            var hash = BufferComparer.Default.GetHashCode(bytes);
            Assert.Equal(-779918115, hash);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = "BufferComparer GetHashCode EnumerableMedium")]
        public static void BufferComparer_GetHashCode_EnumerableMedium()
        {
            // NB: Fidelity

            var bytes = GenerateArray(712).Take(712);
            var hash = BufferComparer.Default.GetHashCode(bytes);
            Assert.Equal(1507092677, hash);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = "BufferComparer GetHashCode EnumerableLong")]
        public static void BufferComparer_GetHashCode_EnumerableLong()
        {
            var bytes = GenerateArray(1024).Take(1024);
            var hash = BufferComparer.Default.GetHashCode(bytes);
            Assert.Equal(1507092677, hash);
        }

        #endregion

        #region Equals

        [Trait("Type", "Unit")]
        [Fact(DisplayName = "BufferComparer Equals Array")]
        public static void BufferComparer_Equals_Array()
        {
            var a = GenerateArray(16);
            var b = GenerateArray(16);
            var c = GenerateArray(16, 1);
            var d = GenerateArray(15);

            Assert.Equal(a, b, BufferComparer.Default);
            Assert.NotEqual(a, c, BufferComparer.Default);
            Assert.NotEqual(a, d, BufferComparer.Default);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = "BufferComparer Equals List")]
        public static void BufferComparer_Equals_List()
        {
            var a = GenerateArray(16).ToList();
            var b = GenerateArray(16).ToList();
            var c = GenerateArray(16, 1).ToList();
            var d = GenerateArray(15).ToList();

            Assert.Equal(a, b, BufferComparer.Default);
            Assert.NotEqual(a, c, BufferComparer.Default);
            Assert.NotEqual(a, d, BufferComparer.Default);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = "BufferComparer Equals ReadOnlyList")]
        public static void BufferComparer_Equals_ReadOnlyList()
        {
            var a = new ReadOnlyListWrapper<byte>(GenerateArray(16));
            var b = new ReadOnlyListWrapper<byte>(GenerateArray(16));
            var c = new ReadOnlyListWrapper<byte>(GenerateArray(16, 1));
            var d = new ReadOnlyListWrapper<byte>(GenerateArray(15));

            Assert.Equal(a, b, BufferComparer.Default);
            Assert.NotEqual(a, c, BufferComparer.Default);
            Assert.NotEqual(a, d, BufferComparer.Default);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = "BufferComparer Equals Enumerable")]
        public static void BufferComparer_Equals_Enumerable()
        {
            var a = GenerateArray(16).Take(16);
            var b = GenerateArray(16).Take(16);
            var c = GenerateArray(16, 1).Take(16);
            var d = GenerateArray(16).Take(15);

            Assert.Equal(a, b, BufferComparer.Default);
            Assert.NotEqual(a, c, BufferComparer.Default);
            Assert.NotEqual(a, d, BufferComparer.Default);
        }

        #endregion

        #region Compare

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(BufferComparer_Compare_Array_One))]
        public static void BufferComparer_Compare_Array_One()
        {
            var a = new byte[1] { 1 };
            var aa = new byte[1] { 1 }; // Same
            var b = new byte[1] { 2 };
            var c = new byte[1] { 0 };

            // Array
            Assert.True(BufferComparer.Default.Compare(a, aa) == 0);
            Assert.True(BufferComparer.Default.Compare(a, b) < 0);
            Assert.True(BufferComparer.Default.Compare(a, c) > 0);

            // IList
            Assert.True(BufferComparer.Default.Compare((IList<byte>)a, aa) == 0);
            Assert.True(BufferComparer.Default.Compare((IList<byte>)a, b) < 0);
            Assert.True(BufferComparer.Default.Compare((IList<byte>)a, c) > 0);

            // IReadOnlyList
            Assert.True(BufferComparer.Default.Compare((IReadOnlyList<byte>)a, aa) == 0);
            Assert.True(BufferComparer.Default.Compare((IReadOnlyList<byte>)a, b) < 0);
            Assert.True(BufferComparer.Default.Compare((IReadOnlyList<byte>)a, c) > 0);

            // IEnumerable
            Assert.True(BufferComparer.Default.Compare((IEnumerable<byte>)a, aa) == 0);
            Assert.True(BufferComparer.Default.Compare((IEnumerable<byte>)a, b) < 0);
            Assert.True(BufferComparer.Default.Compare((IEnumerable<byte>)a, c) > 0);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = "BufferComparer Compare Array")]
        public static void BufferComparer_Compare_Array()
        {
            var a = GenerateArray(16);
            var aa = GenerateArray(16); // Same
            var c = GenerateArray(16, 1);
            var d = GenerateArray(15);

            // Array
            Assert.True(BufferComparer.Default.Compare(a, aa) == 0);
            Assert.True(BufferComparer.Default.Compare(a, c) < 0);
            Assert.True(BufferComparer.Default.Compare(c, a) > 0);
            Assert.True(BufferComparer.Default.Compare(d, a) < 0);
            Assert.True(BufferComparer.Default.Compare(a, d) > 0);

            // IList
            Assert.True(BufferComparer.Default.Compare((IList<byte>)a, aa) == 0);
            Assert.True(BufferComparer.Default.Compare((IList<byte>)a, c) < 0);
            Assert.True(BufferComparer.Default.Compare((IList<byte>)c, a) > 0);
            Assert.True(BufferComparer.Default.Compare((IList<byte>)d, a) < 0);
            Assert.True(BufferComparer.Default.Compare((IList<byte>)a, d) > 0);

            // IReadOnlyList
            Assert.True(BufferComparer.Default.Compare((IReadOnlyList<byte>)a, aa) == 0);
            Assert.True(BufferComparer.Default.Compare((IReadOnlyList<byte>)a, c) < 0);
            Assert.True(BufferComparer.Default.Compare((IReadOnlyList<byte>)c, a) > 0);
            Assert.True(BufferComparer.Default.Compare((IReadOnlyList<byte>)d, a) < 0);
            Assert.True(BufferComparer.Default.Compare((IReadOnlyList<byte>)a, d) > 0);

            // IEnumerable
            Assert.True(BufferComparer.Default.Compare((IEnumerable<byte>)a, aa) == 0);
            Assert.True(BufferComparer.Default.Compare((IEnumerable<byte>)a, c) < 0);
            Assert.True(BufferComparer.Default.Compare((IEnumerable<byte>)c, a) > 0);
            Assert.True(BufferComparer.Default.Compare((IEnumerable<byte>)d, a) < 0);
            Assert.True(BufferComparer.Default.Compare((IEnumerable<byte>)a, d) > 0);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(BufferComparer_Compare_ArraySegment_One))]
        public static void BufferComparer_Compare_ArraySegment_One()
        {
            var a = new ArraySegment<byte>(new byte[] { 1 });
            var aa = new ArraySegment<byte>(new byte[] { 4, 1 }, 1, 1); // Same
            var b = new ArraySegment<byte>(new byte[] { 0, 0, 0, 2 }, 3, 1);
            var c = new ArraySegment<byte>(new byte[] { 3, 3, 3, 3, 0 }, 4, 1);

            // ArraySegment
            Assert.True(BufferComparer.Default.Compare(a, aa) == 0);
            Assert.True(BufferComparer.Default.Compare(a, b) < 0);
            Assert.True(BufferComparer.Default.Compare(a, c) > 0);

            // IList
            Assert.True(BufferComparer.Default.Compare((IList<byte>)a, aa) == 0);
            Assert.True(BufferComparer.Default.Compare((IList<byte>)a, b) < 0);
            Assert.True(BufferComparer.Default.Compare((IList<byte>)a, c) > 0);

            // IReadOnlyList
            Assert.True(BufferComparer.Default.Compare((IReadOnlyList<byte>)a, aa) == 0);
            Assert.True(BufferComparer.Default.Compare((IReadOnlyList<byte>)a, b) < 0);
            Assert.True(BufferComparer.Default.Compare((IReadOnlyList<byte>)a, c) > 0);

            // IEnumerable
            Assert.True(BufferComparer.Default.Compare((IEnumerable<byte>)a, aa) == 0);
            Assert.True(BufferComparer.Default.Compare((IEnumerable<byte>)a, b) < 0);
            Assert.True(BufferComparer.Default.Compare((IEnumerable<byte>)a, c) > 0);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(BufferComparer_Compare_ArraySegment))]
        public static void BufferComparer_Compare_ArraySegment()
        {
            var a = new ArraySegment<byte>(GenerateArray(16));
            var aa = new ArraySegment<byte>(GenerateArray(16)); // Same
            var c = new ArraySegment<byte>(GenerateArray(16, 1));
            var d = new ArraySegment<byte>(GenerateArray(15));

            // ArraySegment
            Assert.True(BufferComparer.Default.Compare(a, aa) == 0);
            Assert.True(BufferComparer.Default.Compare(a, c) < 0);
            Assert.True(BufferComparer.Default.Compare(c, a) > 0);
            Assert.True(BufferComparer.Default.Compare(d, a) < 0);
            Assert.True(BufferComparer.Default.Compare(a, d) > 0);

            // IList
            Assert.True(BufferComparer.Default.Compare((IList<byte>)a, aa) == 0);
            Assert.True(BufferComparer.Default.Compare((IList<byte>)a, c) < 0);
            Assert.True(BufferComparer.Default.Compare((IList<byte>)c, a) > 0);
            Assert.True(BufferComparer.Default.Compare((IList<byte>)d, a) < 0);
            Assert.True(BufferComparer.Default.Compare((IList<byte>)a, d) > 0);

            // IReadOnlyList
            Assert.True(BufferComparer.Default.Compare((IReadOnlyList<byte>)a, aa) == 0);
            Assert.True(BufferComparer.Default.Compare((IReadOnlyList<byte>)a, c) < 0);
            Assert.True(BufferComparer.Default.Compare((IReadOnlyList<byte>)c, a) > 0);
            Assert.True(BufferComparer.Default.Compare((IReadOnlyList<byte>)d, a) < 0);
            Assert.True(BufferComparer.Default.Compare((IReadOnlyList<byte>)a, d) > 0);

            // IEnumerable
            Assert.True(BufferComparer.Default.Compare((IEnumerable<byte>)a, aa) == 0);
            Assert.True(BufferComparer.Default.Compare((IEnumerable<byte>)a, c) < 0);
            Assert.True(BufferComparer.Default.Compare((IEnumerable<byte>)c, a) > 0);
            Assert.True(BufferComparer.Default.Compare((IEnumerable<byte>)d, a) < 0);
            Assert.True(BufferComparer.Default.Compare((IEnumerable<byte>)a, d) > 0);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(BufferComparer_Compare_List_One))]
        public static void BufferComparer_Compare_List_One()
        {
            IList<byte> a = new byte[1] { 1 }.ToList();
            IList<byte> aa = new byte[1] { 1 }.ToList(); // Same
            IList<byte> b = new byte[1] { 2 }.ToList();
            IList<byte> c = new byte[1] { 0 }.ToList();

            // IList
            Assert.True(BufferComparer.Default.Compare(a, aa) == 0);
            Assert.True(BufferComparer.Default.Compare(a, b) < 0);
            Assert.True(BufferComparer.Default.Compare(a, c) > 0);

            // IReadOnlyList
            Assert.True(BufferComparer.Default.Compare((IReadOnlyList<byte>)a, aa) == 0);
            Assert.True(BufferComparer.Default.Compare((IReadOnlyList<byte>)a, b) < 0);
            Assert.True(BufferComparer.Default.Compare((IReadOnlyList<byte>)a, c) > 0);

            // IEnumerable
            Assert.True(BufferComparer.Default.Compare((IEnumerable<byte>)a, aa) == 0);
            Assert.True(BufferComparer.Default.Compare((IEnumerable<byte>)a, b) < 0);
            Assert.True(BufferComparer.Default.Compare((IEnumerable<byte>)a, c) > 0);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = "BufferComparer Compare List")]
        public static void BufferComparer_Compare_List()
        {
            var a = GenerateArray(16).ToList();
            var aa = GenerateArray(16).ToList(); // Same
            var c = GenerateArray(16, 1).ToList();
            var d = GenerateArray(15).ToList();

            // IList
            Assert.True(BufferComparer.Default.Compare((IList<byte>)a, aa) == 0);
            Assert.True(BufferComparer.Default.Compare((IList<byte>)a, c) < 0);
            Assert.True(BufferComparer.Default.Compare((IList<byte>)c, a) > 0);
            Assert.True(BufferComparer.Default.Compare((IList<byte>)d, a) < 0);
            Assert.True(BufferComparer.Default.Compare((IList<byte>)a, d) > 0);

            // IReadOnlyList
            Assert.True(BufferComparer.Default.Compare((IReadOnlyList<byte>)a, aa) == 0);
            Assert.True(BufferComparer.Default.Compare((IReadOnlyList<byte>)a, c) < 0);
            Assert.True(BufferComparer.Default.Compare((IReadOnlyList<byte>)c, a) > 0);
            Assert.True(BufferComparer.Default.Compare((IReadOnlyList<byte>)d, a) < 0);
            Assert.True(BufferComparer.Default.Compare((IReadOnlyList<byte>)a, d) > 0);

            // IEnumerable
            Assert.True(BufferComparer.Default.Compare((IEnumerable<byte>)a, aa) == 0);
            Assert.True(BufferComparer.Default.Compare((IEnumerable<byte>)a, c) < 0);
            Assert.True(BufferComparer.Default.Compare((IEnumerable<byte>)c, a) > 0);
            Assert.True(BufferComparer.Default.Compare((IEnumerable<byte>)d, a) < 0);
            Assert.True(BufferComparer.Default.Compare((IEnumerable<byte>)a, d) > 0);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(BufferComparer_Compare_ReadOnlyList_One))]
        public static void BufferComparer_Compare_ReadOnlyList_One()
        {
            IReadOnlyList<byte> a = new byte[1] { 1 }.ToList();
            IReadOnlyList<byte> aa = new byte[1] { 1 }.ToList(); // Same
            IReadOnlyList<byte> b = new byte[1] { 2 }.ToList();
            IReadOnlyList<byte> c = new byte[1] { 0 }.ToList();

            // IReadOnlyList
            Assert.True(BufferComparer.Default.Compare(a, aa) == 0);
            Assert.True(BufferComparer.Default.Compare(a, b) < 0);
            Assert.True(BufferComparer.Default.Compare(a, c) > 0);

            // IEnumerable
            Assert.True(BufferComparer.Default.Compare((IEnumerable<byte>)a, aa) == 0);
            Assert.True(BufferComparer.Default.Compare((IEnumerable<byte>)a, b) < 0);
            Assert.True(BufferComparer.Default.Compare((IEnumerable<byte>)a, c) > 0);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = "BufferComparer Compare ReadOnlyList")]
        public static void BufferComparer_Compare_ReadOnlyList()
        {
            var a = new ReadOnlyListWrapper<byte>(GenerateArray(16));
            var aa = new ReadOnlyListWrapper<byte>(GenerateArray(16)); // Same
            var c = new ReadOnlyListWrapper<byte>(GenerateArray(16, 1));
            var d = new ReadOnlyListWrapper<byte>(GenerateArray(15));

            // IReadOnlyList
            Assert.True(BufferComparer.Default.Compare(a, aa) == 0);
            Assert.True(BufferComparer.Default.Compare(a, c) < 0);
            Assert.True(BufferComparer.Default.Compare(c, a) > 0);
            Assert.True(BufferComparer.Default.Compare(d, a) < 0);
            Assert.True(BufferComparer.Default.Compare(a, d) > 0);

            // IEnumerable
            Assert.True(BufferComparer.Default.Compare((IEnumerable<byte>)a, aa) == 0);
            Assert.True(BufferComparer.Default.Compare((IEnumerable<byte>)a, c) < 0);
            Assert.True(BufferComparer.Default.Compare((IEnumerable<byte>)c, a) > 0);
            Assert.True(BufferComparer.Default.Compare((IEnumerable<byte>)d, a) < 0);
            Assert.True(BufferComparer.Default.Compare((IEnumerable<byte>)a, d) > 0);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(BufferComparer_Compare_Enumerable_One))]
        public static void BufferComparer_Compare_Enumerable_One()
        {
            IEnumerable<byte> a = new byte[1] { 1 };
            IEnumerable<byte> aa = new byte[1] { 1 }; // Same
            IEnumerable<byte> b = new byte[1] { 2 };
            IEnumerable<byte> c = new byte[1] { 0 };

            // IEnumerable
            Assert.True(BufferComparer.Default.Compare(a, aa) == 0);
            Assert.True(BufferComparer.Default.Compare(a, b) < 0);
            Assert.True(BufferComparer.Default.Compare(a, c) > 0);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(BufferComparer_Compare_Enumerable))]
        public static void BufferComparer_Compare_Enumerable()
        {
            var a = GenerateArray(16).Take(16);
            var aa = GenerateArray(16).Take(16); // Same
            var c = GenerateArray(16, 1).Take(16);
            var d = GenerateArray(16).Take(15);

            // IEnumerable
            Assert.True(BufferComparer.Default.Compare(a, aa) == 0);
            Assert.True(BufferComparer.Default.Compare(a, c) < 0);
            Assert.True(BufferComparer.Default.Compare(c, a) > 0);
            Assert.True(BufferComparer.Default.Compare(d, a) < 0);
            Assert.True(BufferComparer.Default.Compare(a, d) > 0);
        }

        #endregion
    }
}
