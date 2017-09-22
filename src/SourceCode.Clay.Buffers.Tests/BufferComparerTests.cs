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
        [Fact(DisplayName = nameof(BufferComparer_GetHashCode_SpanShort))]
        public static void BufferComparer_GetHashCode_SpanShort()
        {
            Span<byte> bytes = GenerateArray(16);
            var hash = BufferComparer.DefaultComparer.GetHashCode(bytes);
            Assert.Equal(-779918115, hash);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(BufferComparer_GetHashCode_SpanMedium))]
        public static void BufferComparer_GetHashCode_SpanMedium()
        {
            // NB: Fidelity

            Span<byte> bytes = GenerateArray(712);
            var hash = BufferComparer.DefaultComparer.GetHashCode(bytes);
            Assert.Equal(1507092677, hash);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(BufferComparer_GetHashCode_ReadOnlyLong))]
        public static void BufferComparer_GetHashCode_ReadOnlyLong()
        {
            Span<byte> bytes = GenerateArray(1024);
            var hash = BufferComparer.DefaultComparer.GetHashCode(bytes);
            Assert.Equal(1507092677, hash);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(BufferComparer_GetHashCode_ReadOnlySpanShort))]
        public static void BufferComparer_GetHashCode_ReadOnlySpanShort()
        {
            ReadOnlySpan<byte> bytes = GenerateArray(16);
            var hash = BufferComparer.DefaultComparer.GetHashCode(bytes);
            Assert.Equal(-779918115, hash);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(BufferComparer_GetHashCode_ReadOnlySpanMedium))]
        public static void BufferComparer_GetHashCode_ReadOnlySpanMedium()
        {
            // NB: Fidelity

            ReadOnlySpan<byte> bytes = GenerateArray(712);
            var hash = BufferComparer.DefaultComparer.GetHashCode(bytes);
            Assert.Equal(1507092677, hash);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(BufferComparer_GetHashCode_ReadOnlySpanLong))]
        public static void BufferComparer_GetHashCode_ReadOnlySpanLong()
        {
            ReadOnlySpan<byte> bytes = GenerateArray(1024);
            var hash = BufferComparer.DefaultComparer.GetHashCode(bytes);
            Assert.Equal(1507092677, hash);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(BufferComparer_GetHashCode_ArrayShort))]
        public static void BufferComparer_GetHashCode_ArrayShort()
        {
            var bytes = GenerateArray(16);
            var hash = BufferComparer.DefaultComparer.GetHashCode(bytes);
            Assert.Equal(-779918115, hash);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(BufferComparer_GetHashCode_ArrayMedium))]
        public static void BufferComparer_GetHashCode_ArrayMedium()
        {
            // NB: Fidelity

            var bytes = GenerateArray(712);
            var hash = BufferComparer.DefaultComparer.GetHashCode(bytes);
            Assert.Equal(1507092677, hash);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(BufferComparer_GetHashCode_ArrayLong))]
        public static void BufferComparer_GetHashCode_ArrayLong()
        {
            var bytes = GenerateArray(1024);
            var hash = BufferComparer.DefaultComparer.GetHashCode(bytes);
            Assert.Equal(1507092677, hash);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(BufferComparer_GetHashCode_ListShort))]
        public static void BufferComparer_GetHashCode_ListShort()
        {
            var bytes = GenerateArray(16).ToList();
            var hash = BufferComparer.DefaultListComparer.GetHashCode(bytes);
            Assert.Equal(-779918115, hash);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(BufferComparer_GetHashCode_ListMedium))]
        public static void BufferComparer_GetHashCode_ListMedium()
        {
            // NB: Fidelity

            var bytes = GenerateArray(712).ToList();
            var hash = BufferComparer.DefaultListComparer.GetHashCode(bytes);
            Assert.Equal(1507092677, hash);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(BufferComparer_GetHashCode_ListLong))]
        public static void BufferComparer_GetHashCode_ListLong()
        {
            var bytes = GenerateArray(1024).ToList();
            var hash = BufferComparer.DefaultListComparer.GetHashCode(bytes);
            Assert.Equal(1507092677, hash);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(BufferComparer_GetHashCode_ReadOnlyListShort))]
        public static void BufferComparer_GetHashCode_ReadOnlyListShort()
        {
            var bytes = GenerateArray(16).ToList();
            var hash = BufferComparer.DefaultReadOnlyListComparer.GetHashCode(bytes);
            Assert.Equal(-779918115, hash);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(BufferComparer_GetHashCode_ReadOnlyListMedium))]
        public static void BufferComparer_GetHashCode_ReadOnlyListMedium()
        {
            // NB: Fidelity

            var bytes = GenerateArray(712).ToList();
            var hash = BufferComparer.DefaultReadOnlyListComparer.GetHashCode(bytes);
            Assert.Equal(1507092677, hash);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(BufferComparer_GetHashCode_ReadOnlyListLong))]
        public static void BufferComparer_GetHashCode_ReadOnlyListLong()
        {
            var bytes = GenerateArray(1024).ToList();
            var hash = BufferComparer.DefaultReadOnlyListComparer.GetHashCode(bytes);
            Assert.Equal(1507092677, hash);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(BufferComparer_GetHashCode_EnumerableShort))]
        public static void BufferComparer_GetHashCode_EnumerableShort()
        {
            var bytes = GenerateArray(16).Take(16);
            var hash = BufferComparer.DefaultEnumerableComparer.GetHashCode(bytes);
            Assert.Equal(-779918115, hash);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(BufferComparer_GetHashCode_EnumerableMedium))]
        public static void BufferComparer_GetHashCode_EnumerableMedium()
        {
            // NB: Fidelity

            var bytes = GenerateArray(712).Take(712);
            var hash = BufferComparer.DefaultEnumerableComparer.GetHashCode(bytes);
            Assert.Equal(1507092677, hash);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(BufferComparer_GetHashCode_EnumerableLong))]
        public static void BufferComparer_GetHashCode_EnumerableLong()
        {
            var bytes = GenerateArray(1024).Take(1024);
            var hash = BufferComparer.DefaultEnumerableComparer.GetHashCode(bytes);
            Assert.Equal(1507092677, hash);
        }

        #endregion

        #region Equals

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(BufferComparer_Equals_Span))]
        public static void BufferComparer_Equals_Span()
        {
            Span<byte> a = GenerateArray(16);
            Span<byte> b = GenerateArray(16);
            Span<byte> c = GenerateArray(16, 1);
            Span<byte> d = GenerateArray(15);

            Assert.Equal(a, b, BufferComparer.DefaultComparer);
            Assert.NotEqual(a, c, BufferComparer.DefaultComparer);
            Assert.NotEqual(a, d, BufferComparer.DefaultComparer);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(BufferComparer_Equals_ReadOnlySpan))]
        public static void BufferComparer_Equals_ReadOnlySpan()
        {
            ReadOnlySpan<byte> a = GenerateArray(16);
            ReadOnlySpan<byte> b = GenerateArray(16);
            ReadOnlySpan<byte> c = GenerateArray(16, 1);
            ReadOnlySpan<byte> d = GenerateArray(15);

            Assert.Equal(a, b, BufferComparer.DefaultComparer);
            Assert.NotEqual(a, c, BufferComparer.DefaultComparer);
            Assert.NotEqual(a, d, BufferComparer.DefaultComparer);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(BufferComparer_Equals_Array))]
        public static void BufferComparer_Equals_Array()
        {
            var a = GenerateArray(16);
            var b = GenerateArray(16);
            var c = GenerateArray(16, 1);
            var d = GenerateArray(15);

            Assert.Equal(a, b, BufferComparer.DefaultComparer);
            Assert.NotEqual(a, c, BufferComparer.DefaultComparer);
            Assert.NotEqual(a, d, BufferComparer.DefaultComparer);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(BufferComparer_Equals_List))]
        public static void BufferComparer_Equals_List()
        {
            var a = GenerateArray(16).ToList();
            var b = GenerateArray(16).ToList();
            var c = GenerateArray(16, 1).ToList();
            var d = GenerateArray(15).ToList();

            Assert.Equal(a, b, BufferComparer.DefaultListComparer);
            Assert.NotEqual(a, c, BufferComparer.DefaultListComparer);
            Assert.NotEqual(a, d, BufferComparer.DefaultListComparer);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(BufferComparer_Equals_ReadOnlyList))]
        public static void BufferComparer_Equals_ReadOnlyList()
        {
            var a = new ReadOnlyListWrapper<byte>(GenerateArray(16));
            var b = new ReadOnlyListWrapper<byte>(GenerateArray(16));
            var c = new ReadOnlyListWrapper<byte>(GenerateArray(16, 1));
            var d = new ReadOnlyListWrapper<byte>(GenerateArray(15));

            Assert.Equal(a, b, BufferComparer.DefaultReadOnlyListComparer);
            Assert.NotEqual(a, c, BufferComparer.DefaultReadOnlyListComparer);
            Assert.NotEqual(a, d, BufferComparer.DefaultReadOnlyListComparer);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(BufferComparer_Equals_Enumerable))]
        public static void BufferComparer_Equals_Enumerable()
        {
            var a = GenerateArray(16).Take(16);
            var b = GenerateArray(16).Take(16);
            var c = GenerateArray(16, 1).Take(16);
            var d = GenerateArray(16).Take(15);

            Assert.Equal(a, b, BufferComparer.DefaultEnumerableComparer);
            Assert.NotEqual(a, c, BufferComparer.DefaultEnumerableComparer);
            Assert.NotEqual(a, d, BufferComparer.DefaultEnumerableComparer);
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

            // Span
            Assert.True(BufferComparer.DefaultComparer.Compare((Span<byte>)a, aa) == 0);
            Assert.True(BufferComparer.DefaultComparer.Compare((Span<byte>)a, b) < 0);
            Assert.True(BufferComparer.DefaultComparer.Compare((Span<byte>)a, c) > 0);

            // Span
            Assert.True(BufferComparer.DefaultComparer.Compare((ReadOnlySpan<byte>)a, aa) == 0);
            Assert.True(BufferComparer.DefaultComparer.Compare((ReadOnlySpan<byte>)a, b) < 0);
            Assert.True(BufferComparer.DefaultComparer.Compare((ReadOnlySpan<byte>)a, c) > 0);

            // Array
            Assert.True(BufferComparer.DefaultComparer.Compare(a, aa) == 0);
            Assert.True(BufferComparer.DefaultComparer.Compare(a, b) < 0);
            Assert.True(BufferComparer.DefaultComparer.Compare(a, c) > 0);

            // IList
            Assert.True(BufferComparer.DefaultListComparer.Compare(a, aa) == 0);
            Assert.True(BufferComparer.DefaultListComparer.Compare(a, b) < 0);
            Assert.True(BufferComparer.DefaultListComparer.Compare(a, c) > 0);

            // IReadOnlyList
            Assert.True(BufferComparer.DefaultReadOnlyListComparer.Compare(a, aa) == 0);
            Assert.True(BufferComparer.DefaultReadOnlyListComparer.Compare(a, b) < 0);
            Assert.True(BufferComparer.DefaultReadOnlyListComparer.Compare(a, c) > 0);

            // IEnumerable
            Assert.True(BufferComparer.DefaultEnumerableComparer.Compare(a, aa) == 0);
            Assert.True(BufferComparer.DefaultEnumerableComparer.Compare(a, b) < 0);
            Assert.True(BufferComparer.DefaultEnumerableComparer.Compare(a, c) > 0);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(BufferComparer_Compare_Array))]
        public static void BufferComparer_Compare_Array()
        {
            var a = GenerateArray(16);
            var aa = GenerateArray(16); // Same
            var c = GenerateArray(16, 1);
            var d = GenerateArray(15);

            // Span
            Assert.True(BufferComparer.DefaultComparer.Compare((Span<byte>)a, aa) == 0);
            Assert.True(BufferComparer.DefaultComparer.Compare((Span<byte>)a, c) < 0);
            Assert.True(BufferComparer.DefaultComparer.Compare((Span<byte>)c, a) > 0);
            Assert.True(BufferComparer.DefaultComparer.Compare((Span<byte>)d, a) < 0);
            Assert.True(BufferComparer.DefaultComparer.Compare((Span<byte>)a, d) > 0);

            // ReadOnlySpan
            Assert.True(BufferComparer.DefaultComparer.Compare((ReadOnlySpan<byte>)a, aa) == 0);
            Assert.True(BufferComparer.DefaultComparer.Compare((ReadOnlySpan<byte>)a, c) < 0);
            Assert.True(BufferComparer.DefaultComparer.Compare((ReadOnlySpan<byte>)c, a) > 0);
            Assert.True(BufferComparer.DefaultComparer.Compare((ReadOnlySpan<byte>)d, a) < 0);
            Assert.True(BufferComparer.DefaultComparer.Compare((ReadOnlySpan<byte>)a, d) > 0);

            // Array
            Assert.True(BufferComparer.DefaultComparer.Compare(a, aa) == 0);
            Assert.True(BufferComparer.DefaultComparer.Compare(a, c) < 0);
            Assert.True(BufferComparer.DefaultComparer.Compare(c, a) > 0);
            Assert.True(BufferComparer.DefaultComparer.Compare(d, a) < 0);
            Assert.True(BufferComparer.DefaultComparer.Compare(a, d) > 0);

            // IList
            Assert.True(BufferComparer.DefaultListComparer.Compare(a, aa) == 0);
            Assert.True(BufferComparer.DefaultListComparer.Compare(a, c) < 0);
            Assert.True(BufferComparer.DefaultListComparer.Compare(c, a) > 0);
            Assert.True(BufferComparer.DefaultListComparer.Compare(d, a) < 0);
            Assert.True(BufferComparer.DefaultListComparer.Compare(a, d) > 0);

            // IReadOnlyList
            Assert.True(BufferComparer.DefaultReadOnlyListComparer.Compare(a, aa) == 0);
            Assert.True(BufferComparer.DefaultReadOnlyListComparer.Compare(a, c) < 0);
            Assert.True(BufferComparer.DefaultReadOnlyListComparer.Compare(c, a) > 0);
            Assert.True(BufferComparer.DefaultReadOnlyListComparer.Compare(d, a) < 0);
            Assert.True(BufferComparer.DefaultReadOnlyListComparer.Compare(a, d) > 0);

            // IEnumerable
            Assert.True(BufferComparer.DefaultEnumerableComparer.Compare(a, aa) == 0);
            Assert.True(BufferComparer.DefaultEnumerableComparer.Compare(a, c) < 0);
            Assert.True(BufferComparer.DefaultEnumerableComparer.Compare(c, a) > 0);
            Assert.True(BufferComparer.DefaultEnumerableComparer.Compare(d, a) < 0);
            Assert.True(BufferComparer.DefaultEnumerableComparer.Compare(a, d) > 0);
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
            Assert.True(BufferComparer.DefaultComparer.Compare(a, aa) == 0);
            Assert.True(BufferComparer.DefaultComparer.Compare(a, b) < 0);
            Assert.True(BufferComparer.DefaultComparer.Compare(a, c) > 0);

            // IList
            Assert.True(BufferComparer.DefaultListComparer.Compare(a, aa) == 0);
            Assert.True(BufferComparer.DefaultListComparer.Compare(a, b) < 0);
            Assert.True(BufferComparer.DefaultListComparer.Compare(a, c) > 0);

            // IReadOnlyList
            Assert.True(BufferComparer.DefaultReadOnlyListComparer.Compare(a, aa) == 0);
            Assert.True(BufferComparer.DefaultReadOnlyListComparer.Compare(a, b) < 0);
            Assert.True(BufferComparer.DefaultReadOnlyListComparer.Compare(a, c) > 0);

            // IEnumerable
            Assert.True(BufferComparer.DefaultEnumerableComparer.Compare(a, aa) == 0);
            Assert.True(BufferComparer.DefaultEnumerableComparer.Compare(a, b) < 0);
            Assert.True(BufferComparer.DefaultEnumerableComparer.Compare(a, c) > 0);
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
            Assert.True(BufferComparer.DefaultComparer.Compare(a, aa) == 0);
            Assert.True(BufferComparer.DefaultComparer.Compare(a, c) < 0);
            Assert.True(BufferComparer.DefaultComparer.Compare(c, a) > 0);
            Assert.True(BufferComparer.DefaultComparer.Compare(d, a) < 0);
            Assert.True(BufferComparer.DefaultComparer.Compare(a, d) > 0);

            // IList
            Assert.True(BufferComparer.DefaultListComparer.Compare(a, aa) == 0);
            Assert.True(BufferComparer.DefaultListComparer.Compare(a, c) < 0);
            Assert.True(BufferComparer.DefaultListComparer.Compare(c, a) > 0);
            Assert.True(BufferComparer.DefaultListComparer.Compare(d, a) < 0);
            Assert.True(BufferComparer.DefaultListComparer.Compare(a, d) > 0);

            // IReadOnlyList
            Assert.True(BufferComparer.DefaultReadOnlyListComparer.Compare(a, aa) == 0);
            Assert.True(BufferComparer.DefaultReadOnlyListComparer.Compare(a, c) < 0);
            Assert.True(BufferComparer.DefaultReadOnlyListComparer.Compare(c, a) > 0);
            Assert.True(BufferComparer.DefaultReadOnlyListComparer.Compare(d, a) < 0);
            Assert.True(BufferComparer.DefaultReadOnlyListComparer.Compare(a, d) > 0);

            // IEnumerable
            Assert.True(BufferComparer.DefaultEnumerableComparer.Compare(a, aa) == 0);
            Assert.True(BufferComparer.DefaultEnumerableComparer.Compare(a, c) < 0);
            Assert.True(BufferComparer.DefaultEnumerableComparer.Compare(c, a) > 0);
            Assert.True(BufferComparer.DefaultEnumerableComparer.Compare(d, a) < 0);
            Assert.True(BufferComparer.DefaultEnumerableComparer.Compare(a, d) > 0);
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
            Assert.True(BufferComparer.DefaultListComparer.Compare(a, aa) == 0);
            Assert.True(BufferComparer.DefaultListComparer.Compare(a, b) < 0);
            Assert.True(BufferComparer.DefaultListComparer.Compare(a, c) > 0);

            // IReadOnlyList
            Assert.True(BufferComparer.DefaultReadOnlyListComparer.Compare((IReadOnlyList<byte>)a, aa) == 0);
            Assert.True(BufferComparer.DefaultReadOnlyListComparer.Compare((IReadOnlyList<byte>)a, b) < 0);
            Assert.True(BufferComparer.DefaultReadOnlyListComparer.Compare((IReadOnlyList<byte>)a, c) > 0);

            // IEnumerable
            Assert.True(BufferComparer.DefaultEnumerableComparer.Compare(a, aa) == 0);
            Assert.True(BufferComparer.DefaultEnumerableComparer.Compare(a, b) < 0);
            Assert.True(BufferComparer.DefaultEnumerableComparer.Compare(a, c) > 0);
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
            Assert.True(BufferComparer.DefaultListComparer.Compare(a, aa) == 0);
            Assert.True(BufferComparer.DefaultListComparer.Compare(a, c) < 0);
            Assert.True(BufferComparer.DefaultListComparer.Compare(c, a) > 0);
            Assert.True(BufferComparer.DefaultListComparer.Compare(d, a) < 0);
            Assert.True(BufferComparer.DefaultListComparer.Compare(a, d) > 0);

            // IReadOnlyList
            Assert.True(BufferComparer.DefaultReadOnlyListComparer.Compare(a, aa) == 0);
            Assert.True(BufferComparer.DefaultReadOnlyListComparer.Compare(a, c) < 0);
            Assert.True(BufferComparer.DefaultReadOnlyListComparer.Compare(c, a) > 0);
            Assert.True(BufferComparer.DefaultReadOnlyListComparer.Compare(d, a) < 0);
            Assert.True(BufferComparer.DefaultReadOnlyListComparer.Compare(a, d) > 0);

            // IEnumerable
            Assert.True(BufferComparer.DefaultEnumerableComparer.Compare(a, aa) == 0);
            Assert.True(BufferComparer.DefaultEnumerableComparer.Compare(a, c) < 0);
            Assert.True(BufferComparer.DefaultEnumerableComparer.Compare(c, a) > 0);
            Assert.True(BufferComparer.DefaultEnumerableComparer.Compare(d, a) < 0);
            Assert.True(BufferComparer.DefaultEnumerableComparer.Compare(a, d) > 0);
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
            Assert.True(BufferComparer.DefaultReadOnlyListComparer.Compare(a, aa) == 0);
            Assert.True(BufferComparer.DefaultReadOnlyListComparer.Compare(a, b) < 0);
            Assert.True(BufferComparer.DefaultReadOnlyListComparer.Compare(a, c) > 0);

            // IEnumerable
            Assert.True(BufferComparer.DefaultEnumerableComparer.Compare(a, aa) == 0);
            Assert.True(BufferComparer.DefaultEnumerableComparer.Compare(a, b) < 0);
            Assert.True(BufferComparer.DefaultEnumerableComparer.Compare(a, c) > 0);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(BufferComparer_Compare_ReadOnlyList))]
        public static void BufferComparer_Compare_ReadOnlyList()
        {
            var a = new ReadOnlyListWrapper<byte>(GenerateArray(16));
            var aa = new ReadOnlyListWrapper<byte>(GenerateArray(16)); // Same
            var c = new ReadOnlyListWrapper<byte>(GenerateArray(16, 1));
            var d = new ReadOnlyListWrapper<byte>(GenerateArray(15));

            // IReadOnlyList
            Assert.True(BufferComparer.DefaultReadOnlyListComparer.Compare(a, aa) == 0);
            Assert.True(BufferComparer.DefaultReadOnlyListComparer.Compare(a, c) < 0);
            Assert.True(BufferComparer.DefaultReadOnlyListComparer.Compare(c, a) > 0);
            Assert.True(BufferComparer.DefaultReadOnlyListComparer.Compare(d, a) < 0);
            Assert.True(BufferComparer.DefaultReadOnlyListComparer.Compare(a, d) > 0);

            // IEnumerable
            Assert.True(BufferComparer.DefaultEnumerableComparer.Compare(a, aa) == 0);
            Assert.True(BufferComparer.DefaultEnumerableComparer.Compare(a, c) < 0);
            Assert.True(BufferComparer.DefaultEnumerableComparer.Compare(c, a) > 0);
            Assert.True(BufferComparer.DefaultEnumerableComparer.Compare(d, a) < 0);
            Assert.True(BufferComparer.DefaultEnumerableComparer.Compare(a, d) > 0);
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
            Assert.True(BufferComparer.DefaultEnumerableComparer.Compare(a, aa) == 0);
            Assert.True(BufferComparer.DefaultEnumerableComparer.Compare(a, b) < 0);
            Assert.True(BufferComparer.DefaultEnumerableComparer.Compare(a, c) > 0);
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
            Assert.True(BufferComparer.DefaultEnumerableComparer.Compare(a, aa) == 0);
            Assert.True(BufferComparer.DefaultEnumerableComparer.Compare(a, c) < 0);
            Assert.True(BufferComparer.DefaultEnumerableComparer.Compare(c, a) > 0);
            Assert.True(BufferComparer.DefaultEnumerableComparer.Compare(d, a) < 0);
            Assert.True(BufferComparer.DefaultEnumerableComparer.Compare(a, d) > 0);
        }

        #endregion
    }
}
