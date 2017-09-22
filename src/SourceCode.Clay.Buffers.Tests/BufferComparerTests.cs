using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace SourceCode.Clay.Buffers.Tests
{
    public static class BufferComparerTests
    {
        #region Helpers

        private static ArraySegment<byte> GenerateSegment(ushort offset, ushort length, int delta = 0)
        {
            var result = new byte[length + offset * 2]; // Add extra space at start and end
            for (var i = 1 + offset; i < length + offset; i++)
                result[i] = (byte)((result[i - 1] + i - offset + delta) & 0xFF);

            return new ArraySegment<byte>(result, offset, length);
        }

        #endregion

        #region GetHashCode

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(BufferComparer_GetHashCode_SpanShort))]
        public static void BufferComparer_GetHashCode_SpanShort()
        {
            var bytes = GenerateSegment(0, 16).AsSpan();
            var hash = BufferComparer.Default.GetHashCode(bytes);
            Assert.Equal(-779918115, hash);

            bytes = GenerateSegment(10, 16).AsSpan();
            hash = BufferComparer.Default.GetHashCode(bytes);
            Assert.Equal(-779918115, hash);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(BufferComparer_GetHashCode_SpanMedium))]
        public static void BufferComparer_GetHashCode_SpanMedium()
        {
            // NB: Note limited Fidelity

            var bytes = GenerateSegment(0, 712).AsSpan();
            var hash = BufferComparer.Limited.GetHashCode(bytes);
            Assert.Equal(1507092677, hash);

            bytes = GenerateSegment(10, 712).AsSpan();
            hash = BufferComparer.Limited.GetHashCode(bytes);
            Assert.Equal(1507092677, hash);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(BufferComparer_GetHashCode_SpanLong))]
        public static void BufferComparer_GetHashCode_SpanLong()
        {
            // NB: Note limited Fidelity

            var bytes = GenerateSegment(0, 1024).AsSpan();
            var hash = BufferComparer.Limited.GetHashCode(bytes);
            Assert.Equal(1507092677, hash);

            bytes = GenerateSegment(10, 1024).AsSpan();
            hash = BufferComparer.Limited.GetHashCode(bytes);
            Assert.Equal(1507092677, hash);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(BufferComparer_GetHashCode_ReadOnlySpanShort))]
        public static void BufferComparer_GetHashCode_ReadOnlySpanShort()
        {
            var bytes = GenerateSegment(0, 16).AsReadOnlySpan();
            var hash = BufferComparer.Default.GetHashCode(bytes);
            Assert.Equal(-779918115, hash);

            bytes = GenerateSegment(10, 16).AsReadOnlySpan();
            hash = BufferComparer.Default.GetHashCode(bytes);
            Assert.Equal(-779918115, hash);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(BufferComparer_GetHashCode_ReadOnlySpanMedium))]
        public static void BufferComparer_GetHashCode_ReadOnlySpanMedium()
        {
            // NB: Note limited Fidelity

            var bytes = GenerateSegment(0, 712).AsReadOnlySpan();
            var hash = BufferComparer.Limited.GetHashCode(bytes);
            Assert.Equal(1507092677, hash);

            bytes = GenerateSegment(10, 712).AsReadOnlySpan();
            hash = BufferComparer.Limited.GetHashCode(bytes);
            Assert.Equal(1507092677, hash);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(BufferComparer_GetHashCode_ReadOnlySpanLong))]
        public static void BufferComparer_GetHashCode_ReadOnlySpanLong()
        {
            // NB: Note limited Fidelity

            var bytes = GenerateSegment(0, 1024).AsReadOnlySpan();
            var hash = BufferComparer.Limited.GetHashCode(bytes);
            Assert.Equal(1507092677, hash);

            bytes = GenerateSegment(10, 1024).AsReadOnlySpan();
            hash = BufferComparer.Limited.GetHashCode(bytes);
            Assert.Equal(1507092677, hash);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(BufferComparer_GetHashCode_ArrayShort))]
        public static void BufferComparer_GetHashCode_ArrayShort()
        {
            var bytes = GenerateSegment(0, 16).Array;
            var hash = BufferComparer.Default.GetHashCode(bytes);
            Assert.Equal(-779918115, hash);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(BufferComparer_GetHashCode_ArrayMedium))]
        public static void BufferComparer_GetHashCode_ArrayMedium()
        {
            // NB: Note limited Fidelity

            var bytes = GenerateSegment(0, 712).Array;
            var hash = BufferComparer.Limited.GetHashCode(bytes);
            Assert.Equal(1507092677, hash);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(BufferComparer_GetHashCode_ArrayLong))]
        public static void BufferComparer_GetHashCode_ArrayLong()
        {
            // NB: Note limited Fidelity

            var bytes = GenerateSegment(0, 1024).Array;
            var hash = BufferComparer.Limited.GetHashCode(bytes);
            Assert.Equal(1507092677, hash);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(BufferComparer_GetHashCode_ArraySegmentShort))]
        public static void BufferComparer_GetHashCode_ArraySegmentShort()
        {
            var bytes = GenerateSegment(0, 16);
            var hash = BufferComparer.Default.GetHashCode(bytes);
            Assert.Equal(-779918115, hash);

            bytes = GenerateSegment(10, 16);
            hash = BufferComparer.Default.GetHashCode(bytes);
            Assert.Equal(-779918115, hash);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(BufferComparer_GetHashCode_ArraySegmentMedium))]
        public static void BufferComparer_GetHashCode_ArraySegmentMedium()
        {
            // NB: Note limited Fidelity

            var bytes = GenerateSegment(0, 712);
            var hash = BufferComparer.Limited.GetHashCode(bytes);
            Assert.Equal(1507092677, hash);

            bytes = GenerateSegment(10, 712);
            hash = BufferComparer.Limited.GetHashCode(bytes);
            Assert.Equal(1507092677, hash);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(BufferComparer_GetHashCode_ArraySegmentLong))]
        public static void BufferComparer_GetHashCode_ArraySegmentLong()
        {
            // NB: Note limited Fidelity

            var bytes = GenerateSegment(0, 1024);
            var hash = BufferComparer.Limited.GetHashCode(bytes);
            Assert.Equal(1507092677, hash);

            bytes = GenerateSegment(10, 1024);
            hash = BufferComparer.Limited.GetHashCode(bytes);
            Assert.Equal(1507092677, hash);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(BufferComparer_GetHashCode_ListShort))]
        public static void BufferComparer_GetHashCode_ListShort()
        {
            var bytes = GenerateSegment(0, 16).ToList();
            var hash = BufferComparer.DefaultList.GetHashCode(bytes);
            Assert.Equal(-779918115, hash);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(BufferComparer_GetHashCode_ListMedium))]
        public static void BufferComparer_GetHashCode_ListMedium()
        {
            // NB: Note limited Fidelity

            var bytes = GenerateSegment(0, 712).ToList();
            var hash = BufferComparer.LimitedList.GetHashCode(bytes);
            Assert.Equal(1507092677, hash);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(BufferComparer_GetHashCode_ListLong))]
        public static void BufferComparer_GetHashCode_ListLong()
        {
            // NB: Note limited Fidelity

            var bytes = GenerateSegment(0, 1024).ToList();
            var hash = BufferComparer.LimitedList.GetHashCode(bytes);
            Assert.Equal(1507092677, hash);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(BufferComparer_GetHashCode_ReadOnlyListShort))]
        public static void BufferComparer_GetHashCode_ReadOnlyListShort()
        {
            var bytes = GenerateSegment(0, 16).ToList();
            var hash = BufferComparer.DefaultReadOnlyList.GetHashCode(bytes);
            Assert.Equal(-779918115, hash);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(BufferComparer_GetHashCode_ReadOnlyListMedium))]
        public static void BufferComparer_GetHashCode_ReadOnlyListMedium()
        {
            // NB: Note limited Fidelity

            var bytes = GenerateSegment(0, 712).ToList();
            var hash = BufferComparer.LimitedReadOnlyList.GetHashCode(bytes);
            Assert.Equal(1507092677, hash);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(BufferComparer_GetHashCode_ReadOnlyListLong))]
        public static void BufferComparer_GetHashCode_ReadOnlyListLong()
        {
            // NB: Note limited Fidelity

            var bytes = GenerateSegment(0, 1024).ToList();
            var hash = BufferComparer.LimitedReadOnlyList.GetHashCode(bytes);
            Assert.Equal(1507092677, hash);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(BufferComparer_GetHashCode_EnumerableShort))]
        public static void BufferComparer_GetHashCode_EnumerableShort()
        {
            var bytes = GenerateSegment(0, 16).Take(16);
            var hash = BufferComparer.DefaultEnumerable.GetHashCode(bytes);
            Assert.Equal(-779918115, hash);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(BufferComparer_GetHashCode_EnumerableMedium))]
        public static void BufferComparer_GetHashCode_EnumerableMedium()
        {
            // NB: Note limited Fidelity

            var bytes = GenerateSegment(0, 712).Take(712);
            var hash = BufferComparer.LimitedEnumerable.GetHashCode(bytes);
            Assert.Equal(1507092677, hash);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(BufferComparer_GetHashCode_EnumerableLong))]
        public static void BufferComparer_GetHashCode_EnumerableLong()
        {
            // NB: Note limited Fidelity

            var bytes = GenerateSegment(0, 1024).Take(1024);
            var hash = BufferComparer.LimitedEnumerable.GetHashCode(bytes);
            Assert.Equal(1507092677, hash);
        }

        #endregion

        #region IEqualityComparer

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(BufferComparer_Equals_Span))]
        public static void BufferComparer_Equals_Span()
        {
            var a = GenerateSegment(0, 16).AsSpan();
            var a1 = GenerateSegment(10, 16).AsSpan(); // Same
            var a2 = GenerateSegment(0, 16).AsSpan(); // Same
            var c = GenerateSegment(0, 16, 1).AsSpan();
            var d = GenerateSegment(0, 15).AsSpan();

            Assert.Equal(a, a1, BufferComparer.Default);
            Assert.Equal(a, a2, BufferComparer.Default);
            Assert.NotEqual(a, c, BufferComparer.Default);
            Assert.NotEqual(a, d, BufferComparer.Default);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(BufferComparer_Equals_ReadOnlySpan))]
        public static void BufferComparer_Equals_ReadOnlySpan()
        {
            var a = GenerateSegment(0, 16).AsReadOnlySpan();
            var a1 = GenerateSegment(10, 16).AsReadOnlySpan(); // Same
            var a2 = GenerateSegment(0, 16).AsReadOnlySpan(); // Same
            var c = GenerateSegment(0, 16, 1).AsReadOnlySpan();
            var d = GenerateSegment(0, 15).AsReadOnlySpan();

            Assert.Equal(a, a1, BufferComparer.Default);
            Assert.Equal(a, a2, BufferComparer.Default);
            Assert.NotEqual(a, c, BufferComparer.Default);
            Assert.NotEqual(a, d, BufferComparer.Default);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(BufferComparer_Equals_Array))]
        public static void BufferComparer_Equals_Array()
        {
            var a = GenerateSegment(0, 16).Array;
            var a1 = GenerateSegment(0, 16).Array; // Same
            var c = GenerateSegment(0, 16, 1).Array;
            var d = GenerateSegment(0, 15).Array;

            Assert.Equal(a, a1, BufferComparer.Default);
            Assert.NotEqual(a, c, BufferComparer.Default);
            Assert.NotEqual(a, d, BufferComparer.Default);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(BufferComparer_Equals_ArraySegment))]
        public static void BufferComparer_Equals_ArraySegment()
        {
            var a = GenerateSegment(0, 16);
            var a1 = GenerateSegment(10, 16); // Same
            var a2 = GenerateSegment(0, 16); // Same
            var c = GenerateSegment(0, 16, 1);
            var d = GenerateSegment(0, 15);

            Assert.Equal(a, a1, BufferComparer.Default);
            Assert.Equal(a, a2, BufferComparer.Default);
            Assert.NotEqual(a, c, BufferComparer.Default);
            Assert.NotEqual(a, d, BufferComparer.Default);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(BufferComparer_Equals_List))]
        public static void BufferComparer_Equals_List()
        {
            var a = GenerateSegment(0, 16).ToList();
            var a1 = GenerateSegment(0, 16).ToList(); // Same
            var c = GenerateSegment(0, 16, 1).ToList();
            var d = GenerateSegment(0, 15).ToList();

            Assert.Equal(a, a1, BufferComparer.DefaultList);
            Assert.NotEqual(a, c, BufferComparer.DefaultList);
            Assert.NotEqual(a, d, BufferComparer.DefaultList);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(BufferComparer_Equals_ReadOnlyList))]
        public static void BufferComparer_Equals_ReadOnlyList()
        {
            var a = new ReadOnlyListWrapper<byte>(GenerateSegment(0, 16));
            var a1 = new ReadOnlyListWrapper<byte>(GenerateSegment(0, 16)); // Same
            var c = new ReadOnlyListWrapper<byte>(GenerateSegment(0, 16, 1));
            var d = new ReadOnlyListWrapper<byte>(GenerateSegment(0, 15));

            Assert.Equal(a, a1, BufferComparer.DefaultReadOnlyList);
            Assert.NotEqual(a, c, BufferComparer.DefaultReadOnlyList);
            Assert.NotEqual(a, d, BufferComparer.DefaultReadOnlyList);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(BufferComparer_Equals_Enumerable))]
        public static void BufferComparer_Equals_Enumerable()
        {
            var a = GenerateSegment(0, 16).Take(16);
            var a1 = GenerateSegment(0, 16).Take(16); // Same
            var c = GenerateSegment(0, 16, 1).Take(16);
            var d = GenerateSegment(0, 16).Take(15);

            Assert.Equal(a, a1, BufferComparer.DefaultEnumerable);
            Assert.NotEqual(a, c, BufferComparer.DefaultEnumerable);
            Assert.NotEqual(a, d, BufferComparer.DefaultEnumerable);
        }

        #endregion

        #region IComparer

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(BufferComparer_Compare_Array_Length_One))]
        public static void BufferComparer_Compare_Array_Length_One()
        {
            var a = new byte[1] { 1 };
            var a1 = new byte[1] { 1 }; // Same
            var a2 = new byte[4] { 3, 2, 1, 2 }; // Same-ish
            var b = new byte[1] { 2 };
            var c = new byte[1] { 0 };

            // ReadOnlySpan
            Assert.True(BufferComparer.Default.Compare((ReadOnlySpan<byte>)a, a1) == 0);
            Assert.True(BufferComparer.Default.Compare((ReadOnlySpan<byte>)a, b) < 0);
            Assert.True(BufferComparer.Default.Compare((ReadOnlySpan<byte>)a, c) > 0);

            // Span (implicit conversion from Span to ReadOnlySpan)
            Assert.True(BufferComparer.Default.Compare((Span<byte>)a, a1) == 0);
            Assert.True(BufferComparer.Default.Compare((Span<byte>)a, b) < 0);
            Assert.True(BufferComparer.Default.Compare((Span<byte>)a, c) > 0);

            // Array (implicit conversion from byte[] to ReadOnlySpan)
            Assert.True(BufferComparer.Default.Compare(a, a1) == 0);
            Assert.True(BufferComparer.Default.Compare(a, b) < 0);
            Assert.True(BufferComparer.Default.Compare(a, c) > 0);

            // ArraySegment (implicit conversion from ArraySegment to ReadOnlySpan)
            Assert.True(BufferComparer.Default.Compare(new ArraySegment<byte>(a), new ArraySegment<byte>(a1)) == 0);
            Assert.True(BufferComparer.Default.Compare(new ArraySegment<byte>(a), new ArraySegment<byte>(a2, 2, 1)) == 0);
            Assert.True(BufferComparer.Default.Compare(new ArraySegment<byte>(a), new ArraySegment<byte>(b)) < 0);
            Assert.True(BufferComparer.Default.Compare(new ArraySegment<byte>(a), new ArraySegment<byte>(c)) > 0);

            // IList (implicit conversion from byte[] to IList
            Assert.True(BufferComparer.DefaultList.Compare(a.ToList(), a1.ToList()) == 0);
            Assert.True(BufferComparer.DefaultList.Compare(a.ToList(), b.ToList()) < 0);
            Assert.True(BufferComparer.DefaultList.Compare(a.ToList(), c.ToList()) > 0);

            // IList (implicit conversion from byte[] to IList, with internal optimization back to byte[])
            Assert.True(BufferComparer.DefaultList.Compare(a, a1) == 0);
            Assert.True(BufferComparer.DefaultList.Compare(a, b) < 0);
            Assert.True(BufferComparer.DefaultList.Compare(a, c) > 0);

            // IList (implicit conversion from ArraySegment to IList, with internal optimization back to ArraySegment)
            Assert.True(BufferComparer.DefaultList.Compare(new ArraySegment<byte>(a), new ArraySegment<byte>(a1)) == 0);
            Assert.True(BufferComparer.DefaultList.Compare(new ArraySegment<byte>(a), new ArraySegment<byte>(a2, 2, 1)) == 0);
            Assert.True(BufferComparer.DefaultList.Compare(new ArraySegment<byte>(a), new ArraySegment<byte>(b)) < 0);
            Assert.True(BufferComparer.DefaultList.Compare(new ArraySegment<byte>(a), new ArraySegment<byte>(c)) > 0);

            // IReadOnlyList (implicit conversion from byte[] to IReadOnlyList)
            Assert.True(BufferComparer.DefaultReadOnlyList.Compare(a.ToList(), a1.ToList()) == 0);
            Assert.True(BufferComparer.DefaultReadOnlyList.Compare(a.ToList(), b.ToList()) < 0);
            Assert.True(BufferComparer.DefaultReadOnlyList.Compare(a.ToList(), c.ToList()) > 0);

            // IReadOnlyList (implicit conversion from byte[] to IReadOnlyList, with internal optimization back to byte[])
            Assert.True(BufferComparer.DefaultReadOnlyList.Compare(a, a1) == 0);
            Assert.True(BufferComparer.DefaultReadOnlyList.Compare(a, b) < 0);
            Assert.True(BufferComparer.DefaultReadOnlyList.Compare(a, c) > 0);

            // IReadOnlyList (implicit conversion from ArraySegment to IReadOnlyList, with internal optimization back to ArraySegment)
            Assert.True(BufferComparer.DefaultReadOnlyList.Compare(new ArraySegment<byte>(a), new ArraySegment<byte>(a1)) == 0);
            Assert.True(BufferComparer.DefaultReadOnlyList.Compare(new ArraySegment<byte>(a), new ArraySegment<byte>(a2, 2, 1)) == 0);
            Assert.True(BufferComparer.DefaultReadOnlyList.Compare(new ArraySegment<byte>(a), new ArraySegment<byte>(b)) < 0);
            Assert.True(BufferComparer.DefaultReadOnlyList.Compare(new ArraySegment<byte>(a), new ArraySegment<byte>(c)) > 0);

            // IEnumerable (implicit conversion from byte[] to IEnumerable)
            Assert.True(BufferComparer.DefaultEnumerable.Compare(a.ToList(), a1.ToList()) == 0);
            Assert.True(BufferComparer.DefaultEnumerable.Compare(a.ToList(), b.ToList()) < 0);
            Assert.True(BufferComparer.DefaultEnumerable.Compare(a.ToList(), c.ToList()) > 0);

            // IEnumerable (implicit conversion from byte[] to IEnumerable, with internal optimization back to byte[])
            Assert.True(BufferComparer.DefaultEnumerable.Compare(a, a1) == 0);
            Assert.True(BufferComparer.DefaultEnumerable.Compare(a, b) < 0);
            Assert.True(BufferComparer.DefaultEnumerable.Compare(a, c) > 0);

            // IEnumerable (implicit conversion from ArraySegment to IEnumerable, with internal optimization back to ArraySegment)
            Assert.True(BufferComparer.DefaultEnumerable.Compare(new ArraySegment<byte>(a), new ArraySegment<byte>(a1)) == 0);
            Assert.True(BufferComparer.DefaultEnumerable.Compare(new ArraySegment<byte>(a), new ArraySegment<byte>(a2, 2, 1)) == 0);
            Assert.True(BufferComparer.DefaultEnumerable.Compare(new ArraySegment<byte>(a), new ArraySegment<byte>(b)) < 0);
            Assert.True(BufferComparer.DefaultEnumerable.Compare(new ArraySegment<byte>(a), new ArraySegment<byte>(c)) > 0);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(BufferComparer_Compare_Array_Length_N))]
        public static void BufferComparer_Compare_Array_Length_N()
        {
            var a = GenerateSegment(0, 16);
            var a1 = GenerateSegment(0, 16); // Same
            var a2 = GenerateSegment(10, 16); // Same
            var c = GenerateSegment(0, 16, 1);
            var d = GenerateSegment(0, 15);

            // ReadOnlySpan
            Assert.True(BufferComparer.Default.Compare((ReadOnlySpan<byte>)a, a1) == 0);
            Assert.True(BufferComparer.Default.Compare((ReadOnlySpan<byte>)a, a2) == 0);
            Assert.True(BufferComparer.Default.Compare((ReadOnlySpan<byte>)a, c) < 0);
            Assert.True(BufferComparer.Default.Compare((ReadOnlySpan<byte>)c, a) > 0);
            Assert.True(BufferComparer.Default.Compare((ReadOnlySpan<byte>)d, a) < 0);
            Assert.True(BufferComparer.Default.Compare((ReadOnlySpan<byte>)a, d) > 0);

            // Span (implicit conversion from Span to ReadOnlySpan)
            Assert.True(BufferComparer.Default.Compare((Span<byte>)a, a1) == 0);
            Assert.True(BufferComparer.Default.Compare((Span<byte>)a, a2) == 0);
            Assert.True(BufferComparer.Default.Compare((Span<byte>)a, c) < 0);
            Assert.True(BufferComparer.Default.Compare((Span<byte>)c, a) > 0);
            Assert.True(BufferComparer.Default.Compare((Span<byte>)d, a) < 0);
            Assert.True(BufferComparer.Default.Compare((Span<byte>)a, d) > 0);

            // Array (implicit conversion from byte[] to ReadOnlySpan)
            Assert.True(BufferComparer.Default.Compare(a.Array, a1.Array) == 0);
            Assert.True(BufferComparer.Default.Compare(a.Array, c.Array) < 0);
            Assert.True(BufferComparer.Default.Compare(c.Array, a.Array) > 0);
            Assert.True(BufferComparer.Default.Compare(d.Array, a.Array) < 0);
            Assert.True(BufferComparer.Default.Compare(a.Array, d.Array) > 0);

            // ArraySegment (implicit conversion from ArraySegment to ReadOnlySpan)
            Assert.True(BufferComparer.Default.Compare(a, a1) == 0);
            Assert.True(BufferComparer.Default.Compare(a, a2) == 0);
            Assert.True(BufferComparer.Default.Compare(a, c) < 0);
            Assert.True(BufferComparer.Default.Compare(c, a) > 0);
            Assert.True(BufferComparer.Default.Compare(d, a) < 0);
            Assert.True(BufferComparer.Default.Compare(a, d) > 0);

            // IList (implicit conversion from byte[] to IList)
            Assert.True(BufferComparer.DefaultList.Compare(a.Array, a1.Array) == 0);
            Assert.True(BufferComparer.DefaultList.Compare(a.Array, c.Array) < 0);
            Assert.True(BufferComparer.DefaultList.Compare(c.Array, a.Array) > 0);
            Assert.True(BufferComparer.DefaultList.Compare(d.Array, a.Array) < 0);
            Assert.True(BufferComparer.DefaultList.Compare(a.Array, d.Array) > 0);

            // IList (implicit conversion from ArraySegment to IList, with internal optimization back to ArraySegment)
            Assert.True(BufferComparer.DefaultList.Compare(a, a1) == 0);
            Assert.True(BufferComparer.DefaultList.Compare(a, a2) == 0);
            Assert.True(BufferComparer.DefaultList.Compare(a, c) < 0);
            Assert.True(BufferComparer.DefaultList.Compare(c, a) > 0);
            Assert.True(BufferComparer.DefaultList.Compare(d, a) < 0);
            Assert.True(BufferComparer.DefaultList.Compare(a, d) > 0);

            // IReadOnlyList (implicit conversion from byte[] to IReadOnlyList)
            Assert.True(BufferComparer.DefaultReadOnlyList.Compare(a.Array, a1.Array) == 0);
            Assert.True(BufferComparer.DefaultReadOnlyList.Compare(a.Array, c.Array) < 0);
            Assert.True(BufferComparer.DefaultReadOnlyList.Compare(c.Array, a.Array) > 0);
            Assert.True(BufferComparer.DefaultReadOnlyList.Compare(d.Array, a.Array) < 0);
            Assert.True(BufferComparer.DefaultReadOnlyList.Compare(a.Array, d.Array) > 0);

            // IReadOnlyList (implicit conversion from ArraySegment to IReadOnlyList, with internal optimization back to ArraySegment)
            Assert.True(BufferComparer.DefaultReadOnlyList.Compare(a, a1) == 0);
            Assert.True(BufferComparer.DefaultReadOnlyList.Compare(a, a2) == 0);
            Assert.True(BufferComparer.DefaultReadOnlyList.Compare(a, c) < 0);
            Assert.True(BufferComparer.DefaultReadOnlyList.Compare(c, a) > 0);
            Assert.True(BufferComparer.DefaultReadOnlyList.Compare(d, a) < 0);
            Assert.True(BufferComparer.DefaultReadOnlyList.Compare(a, d) > 0);

            // IEnumerable (implicit conversion from byte[] to IEnumerable)
            Assert.True(BufferComparer.DefaultEnumerable.Compare(a.Array, a1.Array) == 0);
            Assert.True(BufferComparer.DefaultEnumerable.Compare(a.Array, c.Array) < 0);
            Assert.True(BufferComparer.DefaultEnumerable.Compare(c.Array, a.Array) > 0);
            Assert.True(BufferComparer.DefaultEnumerable.Compare(d.Array, a.Array) < 0);
            Assert.True(BufferComparer.DefaultEnumerable.Compare(a.Array, d.Array) > 0);

            // IEnumerable (implicit conversion from ArraySegment to IEnumerable, with internal optimization back to ArraySegment)
            Assert.True(BufferComparer.DefaultEnumerable.Compare(a, a1) == 0);
            Assert.True(BufferComparer.DefaultEnumerable.Compare(a, a2) == 0);
            Assert.True(BufferComparer.DefaultEnumerable.Compare(a, c) < 0);
            Assert.True(BufferComparer.DefaultEnumerable.Compare(c, a) > 0);
            Assert.True(BufferComparer.DefaultEnumerable.Compare(d, a) < 0);
            Assert.True(BufferComparer.DefaultEnumerable.Compare(a, d) > 0);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(BufferComparer_Compare_ArraySegment_Length_One))]
        public static void BufferComparer_Compare_ArraySegment_Length_One()
        {
            var a = new ArraySegment<byte>(new byte[] { 1 });
            var a1 = new ArraySegment<byte>(new byte[5] { 5, 4, 1, 2, 3 }, 2, 1); // Same
            var b = new ArraySegment<byte>(new byte[] { 0, 0, 0, 2 }, 3, 1);
            var c = new ArraySegment<byte>(new byte[] { 3, 3, 3, 3, 0 }, 4, 1);

            // ArraySegment (implicit conversion)
            Assert.True(BufferComparer.Default.Compare(a, a1) == 0);
            Assert.True(BufferComparer.Default.Compare(a, b) < 0);
            Assert.True(BufferComparer.Default.Compare(a, c) > 0);

            // IList (implicit conversion from ArraySegment to IList, with internal optimization back to ArraySegment)
            Assert.True(BufferComparer.DefaultList.Compare(a, a1) == 0);
            Assert.True(BufferComparer.DefaultList.Compare(a, b) < 0);
            Assert.True(BufferComparer.DefaultList.Compare(a, c) > 0);

            // IReadOnlyList (implicit conversion from ArraySegment to IReadOnlyList, with internal optimization back to ArraySegment)
            Assert.True(BufferComparer.DefaultReadOnlyList.Compare(a, a1) == 0);
            Assert.True(BufferComparer.DefaultReadOnlyList.Compare(a, b) < 0);
            Assert.True(BufferComparer.DefaultReadOnlyList.Compare(a, c) > 0);

            // IEnumerable (implicit conversion from ArraySegment to IEnumerable, with internal optimization back to ArraySegment)
            Assert.True(BufferComparer.DefaultEnumerable.Compare(a, a1) == 0);
            Assert.True(BufferComparer.DefaultEnumerable.Compare(a, b) < 0);
            Assert.True(BufferComparer.DefaultEnumerable.Compare(a, c) > 0);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(BufferComparer_Compare_ArraySegment_Length_N))]
        public static void BufferComparer_Compare_ArraySegment_Length_N()
        {
            var a = GenerateSegment(0, 16);
            var a1 = GenerateSegment(0, 16); // Same
            var a2 = GenerateSegment(10, 16); // Same
            var c = GenerateSegment(0, 16, 1);
            var d = GenerateSegment(0, 15);

            // ArraySegment (implicit conversion)
            Assert.True(BufferComparer.Default.Compare(a, a1) == 0);
            Assert.True(BufferComparer.Default.Compare(a, a2) == 0);
            Assert.True(BufferComparer.Default.Compare(a, c) < 0);
            Assert.True(BufferComparer.Default.Compare(c, a) > 0);
            Assert.True(BufferComparer.Default.Compare(d, a) < 0);
            Assert.True(BufferComparer.Default.Compare(a, d) > 0);

            // IList (implicit conversion from ArraySegment to IList, with internal optimization back to ArraySegment)
            Assert.True(BufferComparer.DefaultList.Compare(a, a1) == 0);
            Assert.True(BufferComparer.DefaultList.Compare(a, a2) == 0);
            Assert.True(BufferComparer.DefaultList.Compare(a, c) < 0);
            Assert.True(BufferComparer.DefaultList.Compare(c, a) > 0);
            Assert.True(BufferComparer.DefaultList.Compare(d, a) < 0);
            Assert.True(BufferComparer.DefaultList.Compare(a, d) > 0);

            // IReadOnlyList (implicit conversion from ArraySegment to IReadOnlyList, with internal optimization back to ArraySegment)
            Assert.True(BufferComparer.DefaultReadOnlyList.Compare(a, a1) == 0);
            Assert.True(BufferComparer.DefaultReadOnlyList.Compare(a, a2) == 0);
            Assert.True(BufferComparer.DefaultReadOnlyList.Compare(a, c) < 0);
            Assert.True(BufferComparer.DefaultReadOnlyList.Compare(c, a) > 0);
            Assert.True(BufferComparer.DefaultReadOnlyList.Compare(d, a) < 0);
            Assert.True(BufferComparer.DefaultReadOnlyList.Compare(a, d) > 0);

            // IEnumerable (implicit conversion from ArraySegment to IEnumerable, with internal optimization back to ArraySegment)
            Assert.True(BufferComparer.DefaultEnumerable.Compare(a, a1) == 0);
            Assert.True(BufferComparer.DefaultEnumerable.Compare(a, a2) == 0);
            Assert.True(BufferComparer.DefaultEnumerable.Compare(a, c) < 0);
            Assert.True(BufferComparer.DefaultEnumerable.Compare(c, a) > 0);
            Assert.True(BufferComparer.DefaultEnumerable.Compare(d, a) < 0);
            Assert.True(BufferComparer.DefaultEnumerable.Compare(a, d) > 0);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(BufferComparer_Compare_List_Length_One))]
        public static void BufferComparer_Compare_List_Length_One()
        {
            IList<byte> a = new byte[1] { 1 }.ToList();
            IList<byte> a1 = new byte[1] { 1 }.ToList(); // Same
            IList<byte> b = new byte[1] { 2 }.ToList();
            IList<byte> c = new byte[1] { 0 }.ToList();

            // IList
            Assert.True(BufferComparer.DefaultList.Compare(a, a1) == 0);
            Assert.True(BufferComparer.DefaultList.Compare(a, b) < 0);
            Assert.True(BufferComparer.DefaultList.Compare(a, c) > 0);

            // IList (internal optimization back to byte[])
            Assert.True(BufferComparer.DefaultList.Compare(a.ToArray(), a1.ToArray()) == 0);
            Assert.True(BufferComparer.DefaultList.Compare(a.ToArray(), b.ToArray()) < 0);
            Assert.True(BufferComparer.DefaultList.Compare(a.ToArray(), c.ToArray()) > 0);

            // IReadOnlyList
            Assert.True(BufferComparer.DefaultReadOnlyList.Compare((IReadOnlyList<byte>)a, a1) == 0);
            Assert.True(BufferComparer.DefaultReadOnlyList.Compare((IReadOnlyList<byte>)a, b) < 0);
            Assert.True(BufferComparer.DefaultReadOnlyList.Compare((IReadOnlyList<byte>)a, c) > 0);

            // IReadOnlyList (internal optimization back to byte[])
            Assert.True(BufferComparer.DefaultReadOnlyList.Compare(a.ToArray(), a1.ToArray()) == 0);
            Assert.True(BufferComparer.DefaultReadOnlyList.Compare(a.ToArray(), b.ToArray()) < 0);
            Assert.True(BufferComparer.DefaultReadOnlyList.Compare(a.ToArray(), c.ToArray()) > 0);

            // IEnumerable
            Assert.True(BufferComparer.DefaultEnumerable.Compare(a, a1) == 0);
            Assert.True(BufferComparer.DefaultEnumerable.Compare(a, b) < 0);
            Assert.True(BufferComparer.DefaultEnumerable.Compare(a, c) > 0);

            // IEnumerable (internal optimization back to byte[])
            Assert.True(BufferComparer.DefaultEnumerable.Compare(a.ToArray(), a1.ToArray()) == 0);
            Assert.True(BufferComparer.DefaultEnumerable.Compare(a.ToArray(), b.ToArray()) < 0);
            Assert.True(BufferComparer.DefaultEnumerable.Compare(a.ToArray(), c.ToArray()) > 0);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = "BufferComparer Compare List")]
        public static void BufferComparer_Compare_List_Length_N()
        {
            var a = GenerateSegment(0, 16).ToList();
            var a1 = GenerateSegment(0, 16).ToList(); // Same
            var c = GenerateSegment(0, 16, 1).ToList();
            var d = GenerateSegment(0, 15).ToList();

            // IList
            Assert.True(BufferComparer.DefaultList.Compare(a, a1) == 0);
            Assert.True(BufferComparer.DefaultList.Compare(a, c) < 0);
            Assert.True(BufferComparer.DefaultList.Compare(c, a) > 0);
            Assert.True(BufferComparer.DefaultList.Compare(d, a) < 0);
            Assert.True(BufferComparer.DefaultList.Compare(a, d) > 0);

            // IList (internal optimization back to ArraySegment)
            Assert.True(BufferComparer.DefaultList.Compare(a.ToArray(), a1.ToArray()) == 0);
            Assert.True(BufferComparer.DefaultList.Compare(a.ToArray(), c.ToArray()) < 0);
            Assert.True(BufferComparer.DefaultList.Compare(c.ToArray(), a.ToArray()) > 0);
            Assert.True(BufferComparer.DefaultList.Compare(d.ToArray(), a.ToArray()) < 0);
            Assert.True(BufferComparer.DefaultList.Compare(a.ToArray(), d.ToArray()) > 0);

            // IReadOnlyList
            Assert.True(BufferComparer.DefaultReadOnlyList.Compare(a, a1) == 0);
            Assert.True(BufferComparer.DefaultReadOnlyList.Compare(a, c) < 0);
            Assert.True(BufferComparer.DefaultReadOnlyList.Compare(c, a) > 0);
            Assert.True(BufferComparer.DefaultReadOnlyList.Compare(d, a) < 0);
            Assert.True(BufferComparer.DefaultReadOnlyList.Compare(a, d) > 0);

            // IReadOnlyList (internal optimization back to ArraySegment)
            Assert.True(BufferComparer.DefaultReadOnlyList.Compare(a.ToArray(), a1.ToArray()) == 0);
            Assert.True(BufferComparer.DefaultReadOnlyList.Compare(a.ToArray(), c.ToArray()) < 0);
            Assert.True(BufferComparer.DefaultReadOnlyList.Compare(c.ToArray(), a.ToArray()) > 0);
            Assert.True(BufferComparer.DefaultReadOnlyList.Compare(d.ToArray(), a.ToArray()) < 0);
            Assert.True(BufferComparer.DefaultReadOnlyList.Compare(a.ToArray(), d.ToArray()) > 0);

            // IEnumerable
            Assert.True(BufferComparer.DefaultEnumerable.Compare(a, a1) == 0);
            Assert.True(BufferComparer.DefaultEnumerable.Compare(a, c) < 0);
            Assert.True(BufferComparer.DefaultEnumerable.Compare(c, a) > 0);
            Assert.True(BufferComparer.DefaultEnumerable.Compare(d, a) < 0);
            Assert.True(BufferComparer.DefaultEnumerable.Compare(a, d) > 0);

            // IEnumerable (internal optimization back to ArraySegment)
            Assert.True(BufferComparer.DefaultEnumerable.Compare(a.ToArray(), a1.ToArray()) == 0);
            Assert.True(BufferComparer.DefaultEnumerable.Compare(a.ToArray(), c.ToArray()) < 0);
            Assert.True(BufferComparer.DefaultEnumerable.Compare(c.ToArray(), a.ToArray()) > 0);
            Assert.True(BufferComparer.DefaultEnumerable.Compare(d.ToArray(), a.ToArray()) < 0);
            Assert.True(BufferComparer.DefaultEnumerable.Compare(a.ToArray(), d.ToArray()) > 0);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(BufferComparer_Compare_ReadOnlyList_Length_One))]
        public static void BufferComparer_Compare_ReadOnlyList_Length_One()
        {
            IReadOnlyList<byte> a = new byte[1] { 1 }.ToList();
            IReadOnlyList<byte> a1 = new byte[1] { 1 }.ToList(); // Same
            IReadOnlyList<byte> b = new byte[1] { 2 }.ToList();
            IReadOnlyList<byte> c = new byte[1] { 0 }.ToList();

            // IReadOnlyList
            Assert.True(BufferComparer.DefaultReadOnlyList.Compare(a, a1) == 0);
            Assert.True(BufferComparer.DefaultReadOnlyList.Compare(a, b) < 0);
            Assert.True(BufferComparer.DefaultReadOnlyList.Compare(a, c) > 0);

            // IReadOnlyList (internal optimization back to byte[])
            Assert.True(BufferComparer.DefaultReadOnlyList.Compare(a.ToArray(), a1.ToArray()) == 0);
            Assert.True(BufferComparer.DefaultReadOnlyList.Compare(a.ToArray(), b.ToArray()) < 0);
            Assert.True(BufferComparer.DefaultReadOnlyList.Compare(a.ToArray(), c.ToArray()) > 0);

            // IEnumerable
            Assert.True(BufferComparer.DefaultEnumerable.Compare(a, a1) == 0);
            Assert.True(BufferComparer.DefaultEnumerable.Compare(a, b) < 0);
            Assert.True(BufferComparer.DefaultEnumerable.Compare(a, c) > 0);

            // IEnumerable (internal optimization back to byte[])
            Assert.True(BufferComparer.DefaultEnumerable.Compare(a.ToArray(), a1.ToArray()) == 0);
            Assert.True(BufferComparer.DefaultEnumerable.Compare(a.ToArray(), b.ToArray()) < 0);
            Assert.True(BufferComparer.DefaultEnumerable.Compare(a.ToArray(), c.ToArray()) > 0);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(BufferComparer_Compare_ReadOnlyList_Length_N))]
        public static void BufferComparer_Compare_ReadOnlyList_Length_N()
        {
            var a = new ReadOnlyListWrapper<byte>(GenerateSegment(0, 16));
            var a1 = new ReadOnlyListWrapper<byte>(GenerateSegment(0, 16)); // Same
            var c = new ReadOnlyListWrapper<byte>(GenerateSegment(0, 16, 1));
            var d = new ReadOnlyListWrapper<byte>(GenerateSegment(0, 15));

            // IReadOnlyList
            Assert.True(BufferComparer.DefaultReadOnlyList.Compare(a, a1) == 0);
            Assert.True(BufferComparer.DefaultReadOnlyList.Compare(a, c) < 0);
            Assert.True(BufferComparer.DefaultReadOnlyList.Compare(c, a) > 0);
            Assert.True(BufferComparer.DefaultReadOnlyList.Compare(d, a) < 0);
            Assert.True(BufferComparer.DefaultReadOnlyList.Compare(a, d) > 0);

            // IEnumerable
            Assert.True(BufferComparer.DefaultEnumerable.Compare(a, a1) == 0);
            Assert.True(BufferComparer.DefaultEnumerable.Compare(a, c) < 0);
            Assert.True(BufferComparer.DefaultEnumerable.Compare(c, a) > 0);
            Assert.True(BufferComparer.DefaultEnumerable.Compare(d, a) < 0);
            Assert.True(BufferComparer.DefaultEnumerable.Compare(a, d) > 0);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(BufferComparer_Compare_Enumerable_Length_One))]
        public static void BufferComparer_Compare_Enumerable_Length_One()
        {
            IEnumerable<byte> a = new byte[1] { 1 };
            IEnumerable<byte> a1 = new byte[1] { 1 }; // Same
            IEnumerable<byte> b = new byte[1] { 2 };
            IEnumerable<byte> c = new byte[1] { 0 };

            // IEnumerable
            Assert.True(BufferComparer.DefaultEnumerable.Compare(a, a1) == 0);
            Assert.True(BufferComparer.DefaultEnumerable.Compare(a, b) < 0);
            Assert.True(BufferComparer.DefaultEnumerable.Compare(a, c) > 0);

            // IEnumerable (internal optimization back to byte[])
            Assert.True(BufferComparer.DefaultEnumerable.Compare(a.ToArray(), a1.ToArray()) == 0);
            Assert.True(BufferComparer.DefaultEnumerable.Compare(a.ToArray(), b.ToArray()) < 0);
            Assert.True(BufferComparer.DefaultEnumerable.Compare(a.ToArray(), c.ToArray()) > 0);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(BufferComparer_Compare_Enumerable_Length_N))]
        public static void BufferComparer_Compare_Enumerable_Length_N()
        {
            var a = GenerateSegment(0, 16).Take(16);
            var a1 = GenerateSegment(0, 16).Take(16); // Same
            var c = GenerateSegment(0, 16, 1).Take(16);
            var d = GenerateSegment(0, 16).Take(15);

            // IEnumerable
            Assert.True(BufferComparer.DefaultEnumerable.Compare(a, a1) == 0);
            Assert.True(BufferComparer.DefaultEnumerable.Compare(a, c) < 0);
            Assert.True(BufferComparer.DefaultEnumerable.Compare(c, a) > 0);
            Assert.True(BufferComparer.DefaultEnumerable.Compare(d, a) < 0);
            Assert.True(BufferComparer.DefaultEnumerable.Compare(a, d) > 0);

            // IEnumerable (internal optimization back to byte[])
            Assert.True(BufferComparer.DefaultEnumerable.Compare(a.ToArray(), a1.ToArray()) == 0);
            Assert.True(BufferComparer.DefaultEnumerable.Compare(a.ToArray(), c.ToArray()) < 0);
            Assert.True(BufferComparer.DefaultEnumerable.Compare(c.ToArray(), a.ToArray()) > 0);
            Assert.True(BufferComparer.DefaultEnumerable.Compare(d.ToArray(), a.ToArray()) < 0);
            Assert.True(BufferComparer.DefaultEnumerable.Compare(a.ToArray(), d.ToArray()) > 0);
        }

        #endregion
    }
}
