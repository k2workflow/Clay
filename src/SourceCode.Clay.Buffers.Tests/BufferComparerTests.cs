using System;
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
        [Fact(DisplayName = nameof(BufferComparer_GetHashCode_Null_Array))]
        public static void BufferComparer_GetHashCode_Null_Array()
        {
            Assert.Equal(HashCode.FnvNull, BufferComparer.Array.GetHashCode(default));
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(BufferComparer_GetHashCode_Empty_Array))]
        public static void BufferComparer_GetHashCode_Empty_Array()
        {
            Assert.Equal(HashCode.FnvEmpty, BufferComparer.Array.GetHashCode(Array.Empty<byte>()));
            Assert.Equal(HashCode.FnvEmpty, BufferComparer.Span.GetHashCode(Array.Empty<byte>()));
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(BufferComparer_GetHashCode_Empty_Span))]
        public static void BufferComparer_GetHashCode_Empty_Span()
        {
            Assert.Equal(HashCode.FnvEmpty, BufferComparer.Span.GetHashCode(Span<byte>.Empty));
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(BufferComparer_GetHashCode_SpanShort))]
        public static void BufferComparer_GetHashCode_SpanShort()
        {
            var bytes = GenerateSegment(0, 16).AsSpan();
            var hash = BufferComparer.Span.GetHashCode(bytes);
            Assert.Equal(-779918115, hash);

            bytes = GenerateSegment(10, 16).AsSpan();
            hash = BufferComparer.Span.GetHashCode(bytes);
            Assert.Equal(-779918115, hash);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(BufferComparer_GetHashCode_SpanMedium))]
        public static void BufferComparer_GetHashCode_SpanMedium()
        {
            var bytes = GenerateSegment(0, 712).AsSpan();
            var hash = BufferComparer.Span.GetHashCode(bytes);
            Assert.Equal(1507092677, hash);

            bytes = GenerateSegment(10, 712).AsSpan();
            hash = BufferComparer.Span.GetHashCode(bytes);
            Assert.Equal(1507092677, hash);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(BufferComparer_GetHashCode_SpanLong))]
        public static void BufferComparer_GetHashCode_SpanLong()
        {
            var bytes = GenerateSegment(0, 1024).AsSpan();
            var hash = BufferComparer.Span.GetHashCode(bytes);
            Assert.Equal(1507092677, hash);

            bytes = GenerateSegment(10, 1024).AsSpan();
            hash = BufferComparer.Span.GetHashCode(bytes);
            Assert.Equal(1507092677, hash);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(BufferComparer_GetHashCode_ReadOnlySpanShort))]
        public static void BufferComparer_GetHashCode_ReadOnlySpanShort()
        {
            var bytes = GenerateSegment(0, 16).AsReadOnlySpan();
            var hash = BufferComparer.Span.GetHashCode(bytes);
            Assert.Equal(-779918115, hash);

            bytes = GenerateSegment(10, 16).AsReadOnlySpan();
            hash = BufferComparer.Span.GetHashCode(bytes);
            Assert.Equal(-779918115, hash);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(BufferComparer_GetHashCode_ReadOnlySpanMedium))]
        public static void BufferComparer_GetHashCode_ReadOnlySpanMedium()
        {
            var bytes = GenerateSegment(0, 712).AsReadOnlySpan();
            var hash = BufferComparer.Span.GetHashCode(bytes);
            Assert.Equal(1507092677, hash);

            bytes = GenerateSegment(10, 712).AsReadOnlySpan();
            hash = BufferComparer.Span.GetHashCode(bytes);
            Assert.Equal(1507092677, hash);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(BufferComparer_GetHashCode_ReadOnlySpanLong))]
        public static void BufferComparer_GetHashCode_ReadOnlySpanLong()
        {
            var bytes = GenerateSegment(0, 1024).AsReadOnlySpan();
            var hash = BufferComparer.Span.GetHashCode(bytes);
            Assert.Equal(1507092677, hash);

            bytes = GenerateSegment(10, 1024).AsReadOnlySpan();
            hash = BufferComparer.Span.GetHashCode(bytes);
            Assert.Equal(1507092677, hash);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(BufferComparer_GetHashCode_ArrayShort))]
        public static void BufferComparer_GetHashCode_ArrayShort()
        {
            var bytes = GenerateSegment(0, 16).Array;
            var hash = BufferComparer.Span.GetHashCode(bytes);
            Assert.Equal(-779918115, hash);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(BufferComparer_GetHashCode_ArrayMedium))]
        public static void BufferComparer_GetHashCode_ArrayMedium()
        {
            var bytes = GenerateSegment(0, BufferComparer.DefaultHashCodeFidelity).Array;
            var hash = BufferComparer.Span.GetHashCode(bytes);
            Assert.Equal(1507092677, hash);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(BufferComparer_GetHashCode_ArrayLong))]
        public static void BufferComparer_GetHashCode_ArrayLong()
        {
            var bytes = GenerateSegment(0, 1024).Array;
            var hash = BufferComparer.Span.GetHashCode(bytes);
            Assert.Equal(1507092677, hash);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(BufferComparer_GetHashCode_ArraySegmentShort))]
        public static void BufferComparer_GetHashCode_ArraySegmentShort()
        {
            var bytes = GenerateSegment(0, 16);
            var hash = BufferComparer.Span.GetHashCode(bytes);
            Assert.Equal(-779918115, hash);

            bytes = GenerateSegment(10, 16);
            hash = BufferComparer.Span.GetHashCode(bytes);
            Assert.Equal(-779918115, hash);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(BufferComparer_GetHashCode_ArraySegmentMedium))]
        public static void BufferComparer_GetHashCode_ArraySegmentMedium()
        {
            var bytes = GenerateSegment(0, 712);
            var hash = BufferComparer.Span.GetHashCode(bytes);
            Assert.Equal(1507092677, hash);

            bytes = GenerateSegment(10, 712);
            hash = BufferComparer.Span.GetHashCode(bytes);
            Assert.Equal(1507092677, hash);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(BufferComparer_GetHashCode_ArraySegmentLong))]
        public static void BufferComparer_GetHashCode_ArraySegmentLong()
        {
            var bytes = GenerateSegment(0, 1024);
            var hash = BufferComparer.Span.GetHashCode(bytes);
            Assert.Equal(1507092677, hash);

            bytes = GenerateSegment(10, 1024);
            hash = BufferComparer.Span.GetHashCode(bytes);
            Assert.Equal(1507092677, hash);
        }

        #endregion

        #region IEqualityComparer

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(BufferComparer_Equals_Null_Array))]
        public static void BufferComparer_Equals_Null_Array()
        {
            byte[] a = null;
            byte[] b = null;

            Assert.Equal(a, b, BufferComparer.Array);
            Assert.Throws<ArgumentNullException>(() => BufferComparer.Span.Equals(a, b));
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(BufferComparer_Equals_Empty_Array))]
        public static void BufferComparer_Equals_Empty_Array()
        {
            var a = Array.Empty<byte>();
            var b = Array.Empty<byte>();

            Assert.Equal(a, b, BufferComparer.Array);
            Assert.Equal(a, b, BufferComparer.Span);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(BufferComparer_Equals_Span))]
        public static void BufferComparer_Equals_Span()
        {
            var a = GenerateSegment(0, 16).AsSpan();
            var a1 = GenerateSegment(10, 16).AsSpan(); // Same
            var a2 = GenerateSegment(0, 16).AsSpan(); // Same
            var c = GenerateSegment(0, 16, 1).AsSpan();
            var d = GenerateSegment(0, 15).AsSpan();

            Assert.Equal(a, a1, BufferComparer.Span);
            Assert.Equal(a, a2, BufferComparer.Span);
            Assert.NotEqual(a, c, BufferComparer.Span);
            Assert.NotEqual(a, d, BufferComparer.Span);
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

            Assert.Equal(a, a1, BufferComparer.Span);
            Assert.Equal(a, a2, BufferComparer.Span);
            Assert.NotEqual(a, c, BufferComparer.Span);
            Assert.NotEqual(a, d, BufferComparer.Span);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(BufferComparer_Equals_Array))]
        public static void BufferComparer_Equals_Array()
        {
            var a = GenerateSegment(0, 16).Array;
            var a1 = GenerateSegment(0, 16).Array; // Same
            var c = GenerateSegment(0, 16, 1).Array;
            var d = GenerateSegment(0, 15).Array;

            Assert.Equal(a, a1, BufferComparer.Span);
            Assert.NotEqual(a, c, BufferComparer.Span);
            Assert.NotEqual(a, d, BufferComparer.Span);
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

            Assert.Equal(a, a1, BufferComparer.Span);
            Assert.Equal(a, a2, BufferComparer.Span);
            Assert.NotEqual(a, c, BufferComparer.Span);
            Assert.NotEqual(a, d, BufferComparer.Span);
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
            Assert.True(BufferComparer.Span.Compare((ReadOnlySpan<byte>)a, a1) == 0);
            Assert.True(BufferComparer.Span.Compare((ReadOnlySpan<byte>)a, b) < 0);
            Assert.True(BufferComparer.Span.Compare((ReadOnlySpan<byte>)a, c) > 0);

            // Span (implicit conversion from Span to ReadOnlySpan)
            Assert.True(BufferComparer.Span.Compare((Span<byte>)a, a1) == 0);
            Assert.True(BufferComparer.Span.Compare((Span<byte>)a, b) < 0);
            Assert.True(BufferComparer.Span.Compare((Span<byte>)a, c) > 0);

            // Array (implicit conversion from byte[] to ReadOnlySpan)
            Assert.True(BufferComparer.Span.Compare(a, a1) == 0);
            Assert.True(BufferComparer.Span.Compare(a, b) < 0);
            Assert.True(BufferComparer.Span.Compare(a, c) > 0);

            // ArraySegment (implicit conversion from ArraySegment to ReadOnlySpan)
            Assert.True(BufferComparer.Span.Compare(new ArraySegment<byte>(a), new ArraySegment<byte>(a1)) == 0);
            Assert.True(BufferComparer.Span.Compare(new ArraySegment<byte>(a), new ArraySegment<byte>(a2, 2, 1)) == 0);
            Assert.True(BufferComparer.Span.Compare(new ArraySegment<byte>(a), new ArraySegment<byte>(b)) < 0);
            Assert.True(BufferComparer.Span.Compare(new ArraySegment<byte>(a), new ArraySegment<byte>(c)) > 0);
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
            Assert.True(BufferComparer.Span.Compare((ReadOnlySpan<byte>)a, a1) == 0);
            Assert.True(BufferComparer.Span.Compare((ReadOnlySpan<byte>)a, a2) == 0);
            Assert.True(BufferComparer.Span.Compare((ReadOnlySpan<byte>)a, c) < 0);
            Assert.True(BufferComparer.Span.Compare((ReadOnlySpan<byte>)c, a) > 0);
            Assert.True(BufferComparer.Span.Compare((ReadOnlySpan<byte>)d, a) < 0);
            Assert.True(BufferComparer.Span.Compare((ReadOnlySpan<byte>)a, d) > 0);

            // Span (implicit conversion from Span to ReadOnlySpan)
            Assert.True(BufferComparer.Span.Compare((Span<byte>)a, a1) == 0);
            Assert.True(BufferComparer.Span.Compare((Span<byte>)a, a2) == 0);
            Assert.True(BufferComparer.Span.Compare((Span<byte>)a, c) < 0);
            Assert.True(BufferComparer.Span.Compare((Span<byte>)c, a) > 0);
            Assert.True(BufferComparer.Span.Compare((Span<byte>)d, a) < 0);
            Assert.True(BufferComparer.Span.Compare((Span<byte>)a, d) > 0);

            // Array (implicit conversion from byte[] to ReadOnlySpan)
            Assert.True(BufferComparer.Span.Compare(a.Array, a1.Array) == 0);
            Assert.True(BufferComparer.Span.Compare(a.Array, c.Array) < 0);
            Assert.True(BufferComparer.Span.Compare(c.Array, a.Array) > 0);
            Assert.True(BufferComparer.Span.Compare(d.Array, a.Array) < 0);
            Assert.True(BufferComparer.Span.Compare(a.Array, d.Array) > 0);

            // ArraySegment (implicit conversion from ArraySegment to ReadOnlySpan)
            Assert.True(BufferComparer.Span.Compare(a, a1) == 0);
            Assert.True(BufferComparer.Span.Compare(a, a2) == 0);
            Assert.True(BufferComparer.Span.Compare(a, c) < 0);
            Assert.True(BufferComparer.Span.Compare(c, a) > 0);
            Assert.True(BufferComparer.Span.Compare(d, a) < 0);
            Assert.True(BufferComparer.Span.Compare(a, d) > 0);
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
            Assert.True(BufferComparer.Span.Compare(a, a1) == 0);
            Assert.True(BufferComparer.Span.Compare(a, b) < 0);
            Assert.True(BufferComparer.Span.Compare(a, c) > 0);
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
            Assert.True(BufferComparer.Span.Compare(a, a1) == 0);
            Assert.True(BufferComparer.Span.Compare(a, a2) == 0);
            Assert.True(BufferComparer.Span.Compare(a, c) < 0);
            Assert.True(BufferComparer.Span.Compare(c, a) > 0);
            Assert.True(BufferComparer.Span.Compare(d, a) < 0);
            Assert.True(BufferComparer.Span.Compare(a, d) > 0);
        }

        #endregion
    }
}
