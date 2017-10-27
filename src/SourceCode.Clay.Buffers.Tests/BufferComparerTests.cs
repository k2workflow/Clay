#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using System;
using System.Collections.Generic;
using Xunit;

namespace SourceCode.Clay.Buffers.Tests
{
    public static class BufferComparerTests
    {
        #region Helpers

        public static ArraySegment<byte> GenerateSegment(ushort offset, ushort length, int delta = 0)
        {
            var result = new byte[length + offset * 2]; // Add extra space at start and end
            for (var i = 1 + offset; i < length + offset; i++)
                result[i] = (byte)((result[i - 1] + i - offset + delta) & 0xFF);

            return new ArraySegment<byte>(result, offset, length);
        }

        public static ReadOnlyMemory<byte> AsReadOnlyMemory(this ArraySegment<byte> seg)
            => (ReadOnlyMemory<byte>)seg;

        public static ReadOnlyMemory<byte> AsReadOnlyMemory(this byte[] array)
            => (ReadOnlyMemory<byte>)array;

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
            Assert.Equal(HashCode.FnvEmpty, BufferComparer.Memory.GetHashCode(Array.Empty<byte>()));
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(BufferComparer_GetHashCode_Empty_Memory))]
        public static void BufferComparer_GetHashCode_Empty_Memory()
        {
            Assert.Equal(HashCode.FnvEmpty, BufferComparer.Memory.GetHashCode(Memory<byte>.Empty));
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(BufferComparer_GetHashCode_MemoryShort))]
        public static void BufferComparer_GetHashCode_MemoryShort()
        {
            var bytes = GenerateSegment(0, 16);
            var hash = BufferComparer.Memory.GetHashCode(bytes);
            Assert.Equal(-779918115, hash);

            bytes = GenerateSegment(10, 16);
            hash = BufferComparer.Memory.GetHashCode(bytes);
            Assert.Equal(-779918115, hash);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(BufferComparer_GetHashCode_MemoryMedium))]
        public static void BufferComparer_GetHashCode_MemoryMedium()
        {
            var bytes = GenerateSegment(0, 712);
            var hash = BufferComparer.Memory.GetHashCode(bytes);
            Assert.Equal(1507092677, hash);

            bytes = GenerateSegment(10, 712);
            hash = BufferComparer.Memory.GetHashCode(bytes);
            Assert.Equal(1507092677, hash);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(BufferComparer_GetHashCode_MemoryLong))]
        public static void BufferComparer_GetHashCode_MemoryLong()
        {
            var bytes = GenerateSegment(0, 1024);
            var hash = BufferComparer.Memory.GetHashCode(bytes);
            Assert.Equal(1507092677, hash);

            bytes = GenerateSegment(10, 1024);
            hash = BufferComparer.Memory.GetHashCode(bytes);
            Assert.Equal(1507092677, hash);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(BufferComparer_GetHashCode_ReadOnlyMemoryShort))]
        public static void BufferComparer_GetHashCode_ReadOnlyMemoryShort()
        {
            var bytes = GenerateSegment(0, 16).AsReadOnlyMemory();
            var hash = BufferComparer.Memory.GetHashCode(bytes);
            Assert.Equal(-779918115, hash);

            bytes = GenerateSegment(10, 16).AsReadOnlyMemory();
            hash = BufferComparer.Memory.GetHashCode(bytes);
            Assert.Equal(-779918115, hash);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(BufferComparer_GetHashCode_ReadOnlyMemoryMedium))]
        public static void BufferComparer_GetHashCode_ReadOnlyMemoryMedium()
        {
            var bytes = GenerateSegment(0, 712).AsReadOnlyMemory();
            var hash = BufferComparer.Memory.GetHashCode(bytes);
            Assert.Equal(1507092677, hash);

            bytes = GenerateSegment(10, 712).AsReadOnlyMemory();
            hash = BufferComparer.Memory.GetHashCode(bytes);
            Assert.Equal(1507092677, hash);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(BufferComparer_GetHashCode_ReadOnlyMemoryLong))]
        public static void BufferComparer_GetHashCode_ReadOnlyMemoryLong()
        {
            var bytes = GenerateSegment(0, 1024).AsReadOnlyMemory();
            var hash = BufferComparer.Memory.GetHashCode(bytes);
            Assert.Equal(1507092677, hash);

            bytes = GenerateSegment(10, 1024).AsReadOnlyMemory();
            hash = BufferComparer.Memory.GetHashCode(bytes);
            Assert.Equal(1507092677, hash);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(BufferComparer_GetHashCode_ArrayShort))]
        public static void BufferComparer_GetHashCode_ArrayShort()
        {
            var bytes = GenerateSegment(0, 16).Array;
            var hash = BufferComparer.Memory.GetHashCode(bytes);
            Assert.Equal(-779918115, hash);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(BufferComparer_GetHashCode_ArrayMedium))]
        public static void BufferComparer_GetHashCode_ArrayMedium()
        {
            var bytes = GenerateSegment(0, BufferComparer.DefaultHashCodeFidelity).Array;
            var hash = BufferComparer.Memory.GetHashCode(bytes);
            Assert.Equal(1507092677, hash);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(BufferComparer_GetHashCode_ArrayLong))]
        public static void BufferComparer_GetHashCode_ArrayLong()
        {
            var bytes = GenerateSegment(0, 1024).Array;
            var hash = BufferComparer.Memory.GetHashCode(bytes);
            Assert.Equal(1507092677, hash);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(BufferComparer_GetHashCode_ArraySegmentShort))]
        public static void BufferComparer_GetHashCode_ArraySegmentShort()
        {
            var bytes = GenerateSegment(0, 16);
            var hash = BufferComparer.Memory.GetHashCode(bytes);
            Assert.Equal(-779918115, hash);

            bytes = GenerateSegment(10, 16);
            hash = BufferComparer.Memory.GetHashCode(bytes);
            Assert.Equal(-779918115, hash);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(BufferComparer_GetHashCode_ArraySegmentMedium))]
        public static void BufferComparer_GetHashCode_ArraySegmentMedium()
        {
            var bytes = GenerateSegment(0, 712);
            var hash = BufferComparer.Memory.GetHashCode(bytes);
            Assert.Equal(1507092677, hash);

            bytes = GenerateSegment(10, 712);
            hash = BufferComparer.Memory.GetHashCode(bytes);
            Assert.Equal(1507092677, hash);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(BufferComparer_GetHashCode_ArraySegmentLong))]
        public static void BufferComparer_GetHashCode_ArraySegmentLong()
        {
            var bytes = GenerateSegment(0, 1024);
            var hash = BufferComparer.Memory.GetHashCode(bytes);
            Assert.Equal(1507092677, hash);

            bytes = GenerateSegment(10, 1024);
            hash = BufferComparer.Memory.GetHashCode(bytes);
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
            Assert.Throws<ArgumentNullException>(() => BufferComparer.Memory.Equals(a, b));
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(BufferComparer_Equals_Empty_Array))]
        public static void BufferComparer_Equals_Empty_Array()
        {
            var a = Array.Empty<byte>();
            var b = Array.Empty<byte>();

            Assert.Equal(a, b, BufferComparer.Array);
            Assert.Equal(a, b, BufferComparer.Memory);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(BufferComparer_Equals_Memory))]
        public static void BufferComparer_Equals_Memory()
        {
            var a = GenerateSegment(0, 16);
            var a1 = GenerateSegment(10, 16); // Same
            var a2 = GenerateSegment(0, 16); // Same
            var c = GenerateSegment(0, 16, 1);
            var d = GenerateSegment(0, 15);

            Assert.Equal(a, a1, BufferComparer.Memory);
            Assert.Equal(a, a2, BufferComparer.Memory);
            Assert.NotEqual(a, c, BufferComparer.Memory);
            Assert.NotEqual(a, d, BufferComparer.Memory);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(BufferComparer_Equals_ReadOnlyMemory))]
        public static void BufferComparer_Equals_ReadOnlyMemory()
        {
            var a = GenerateSegment(0, 16).AsReadOnlyMemory();
            var a1 = GenerateSegment(10, 16).AsReadOnlyMemory(); // Same
            var a2 = GenerateSegment(0, 16).AsReadOnlyMemory(); // Same
            var c = GenerateSegment(0, 16, 1).AsReadOnlyMemory();
            var d = GenerateSegment(0, 15).AsReadOnlyMemory();

            Assert.Equal(a, a1, BufferComparer.Memory);
            Assert.Equal(a, a2, BufferComparer.Memory);
            Assert.NotEqual(a, c, BufferComparer.Memory);
            Assert.NotEqual(a, d, BufferComparer.Memory);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(BufferComparer_Equals_Array))]
        public static void BufferComparer_Equals_Array()
        {
            var a = GenerateSegment(0, 16).Array;
            var a1 = GenerateSegment(0, 16).Array; // Same
            var c = GenerateSegment(0, 16, 1).Array;
            var d = GenerateSegment(0, 15).Array;

            // Array
            Assert.Equal(a, a1, BufferComparer.Array);
            Assert.NotEqual(a, c, BufferComparer.Array);
            Assert.NotEqual(a, d, BufferComparer.Array);

            // Span
            Assert.Equal(a, a1, BufferComparer.Memory);
            Assert.NotEqual(a, c, BufferComparer.Memory);
            Assert.NotEqual(a, d, BufferComparer.Memory);
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

            Assert.Equal(a, a1, BufferComparer.Memory);
            Assert.Equal(a, a2, BufferComparer.Memory);
            Assert.NotEqual(a, c, BufferComparer.Memory);
            Assert.NotEqual(a, d, BufferComparer.Memory);
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

            // Array
            Assert.True(BufferComparer.Array.Compare(a, a1) == 0);
            Assert.True(BufferComparer.Array.Compare(a, b) < 0);
            Assert.True(BufferComparer.Array.Compare(a, c) > 0);

            // ReadOnlyMemory
            Assert.True(BufferComparer.Memory.Compare((ReadOnlyMemory<byte>)a, a1) == 0);
            Assert.True(BufferComparer.Memory.Compare((ReadOnlyMemory<byte>)a, b) < 0);
            Assert.True(BufferComparer.Memory.Compare((ReadOnlyMemory<byte>)a, c) > 0);

            // Span (implicit conversion from Span to ReadOnlyMemory)
            Assert.True(BufferComparer.Memory.Compare((Memory<byte>)a, a1) == 0);
            Assert.True(BufferComparer.Memory.Compare((Memory<byte>)a, b) < 0);
            Assert.True(BufferComparer.Memory.Compare((Memory<byte>)a, c) > 0);

            // Array (implicit conversion from byte[] to ReadOnlyMemory)
            Assert.True(BufferComparer.Memory.Compare(a, a1) == 0);
            Assert.True(BufferComparer.Memory.Compare(a, b) < 0);
            Assert.True(BufferComparer.Memory.Compare(a, c) > 0);

            // ArraySegment (implicit conversion from ArraySegment to ReadOnlyMemory)
            Assert.True(BufferComparer.Memory.Compare(new ArraySegment<byte>(a), new ArraySegment<byte>(a1)) == 0);
            Assert.True(BufferComparer.Memory.Compare(new ArraySegment<byte>(a), new ArraySegment<byte>(a2, 2, 1)) == 0);
            Assert.True(BufferComparer.Memory.Compare(new ArraySegment<byte>(a), new ArraySegment<byte>(b)) < 0);
            Assert.True(BufferComparer.Memory.Compare(new ArraySegment<byte>(a), new ArraySegment<byte>(c)) > 0);
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

            // Array
            Assert.True(BufferComparer.Array.Compare(a.Array, a1.Array) == 0);
            Assert.True(BufferComparer.Array.Compare(a.Array, c.Array) < 0);
            Assert.True(BufferComparer.Array.Compare(c.Array, a.Array) > 0);
            Assert.True(BufferComparer.Array.Compare(d.Array, a.Array) < 0);
            Assert.True(BufferComparer.Array.Compare(a.Array, d.Array) > 0);

            // ReadOnlyMemory
            Assert.True(BufferComparer.Memory.Compare((ReadOnlyMemory<byte>)a, a1) == 0);
            Assert.True(BufferComparer.Memory.Compare((ReadOnlyMemory<byte>)a, a2) == 0);
            Assert.True(BufferComparer.Memory.Compare((ReadOnlyMemory<byte>)a, c) < 0);
            Assert.True(BufferComparer.Memory.Compare((ReadOnlyMemory<byte>)c, a) > 0);
            Assert.True(BufferComparer.Memory.Compare((ReadOnlyMemory<byte>)d, a) < 0);
            Assert.True(BufferComparer.Memory.Compare((ReadOnlyMemory<byte>)a, d) > 0);

            // Span (implicit conversion from Span to ReadOnlyMemory)
            Assert.True(BufferComparer.Memory.Compare((Memory<byte>)a, a1) == 0);
            Assert.True(BufferComparer.Memory.Compare((Memory<byte>)a, a2) == 0);
            Assert.True(BufferComparer.Memory.Compare((Memory<byte>)a, c) < 0);
            Assert.True(BufferComparer.Memory.Compare((Memory<byte>)c, a) > 0);
            Assert.True(BufferComparer.Memory.Compare((Memory<byte>)d, a) < 0);
            Assert.True(BufferComparer.Memory.Compare((Memory<byte>)a, d) > 0);

            // Array (implicit conversion from byte[] to ReadOnlyMemory)
            Assert.True(BufferComparer.Memory.Compare(a.Array, a1.Array) == 0);
            Assert.True(BufferComparer.Memory.Compare(a.Array, c.Array) < 0);
            Assert.True(BufferComparer.Memory.Compare(c.Array, a.Array) > 0);
            Assert.True(BufferComparer.Memory.Compare(d.Array, a.Array) < 0);
            Assert.True(BufferComparer.Memory.Compare(a.Array, d.Array) > 0);

            // ArraySegment (implicit conversion from ArraySegment to ReadOnlyMemory)
            Assert.True(BufferComparer.Memory.Compare(a, a1) == 0);
            Assert.True(BufferComparer.Memory.Compare(a, a2) == 0);
            Assert.True(BufferComparer.Memory.Compare(a, c) < 0);
            Assert.True(BufferComparer.Memory.Compare(c, a) > 0);
            Assert.True(BufferComparer.Memory.Compare(d, a) < 0);
            Assert.True(BufferComparer.Memory.Compare(a, d) > 0);
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
            Assert.True(BufferComparer.Memory.Compare(a, a1) == 0);
            Assert.True(BufferComparer.Memory.Compare(a, b) < 0);
            Assert.True(BufferComparer.Memory.Compare(a, c) > 0);
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
            Assert.True(BufferComparer.Memory.Compare(a, a1) == 0);
            Assert.True(BufferComparer.Memory.Compare(a, a2) == 0);
            Assert.True(BufferComparer.Memory.Compare(a, c) < 0);
            Assert.True(BufferComparer.Memory.Compare(c, a) > 0);
            Assert.True(BufferComparer.Memory.Compare(d, a) < 0);
            Assert.True(BufferComparer.Memory.Compare(a, d) > 0);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(BufferComparer_Compare_Array_With_Comparison))]
        public static void BufferComparer_Compare_Array_With_Comparison()
        {
            var expected = new byte[4] { 1, 2, 3, 4 };

            var a1 = new byte[4] { 1, 2, 3, 6 };
            var a2 = new byte[4] { 1, 2, 3, 7 };
            var a3 = new byte[4] { 1, 2, 3, 5 };

            var list = new List<byte[]>(new[] { a1, a2, expected, a3 });
            list.Sort(BufferComparer.Array.Compare);

            Assert.Equal(expected, list[0], BufferComparer.Array);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(BufferComparer_Compare_Memory_With_Comparison))]
        public static void BufferComparer_Compare_Memory_With_Comparison()
        {
            var expected = new byte[4] { 1, 2, 3, 4 }.AsReadOnlyMemory();

            var a1 = new byte[4] { 1, 2, 3, 6 }.AsReadOnlyMemory();
            var a2 = new byte[4] { 1, 2, 3, 7 }.AsReadOnlyMemory();
            var a3 = new byte[4] { 1, 2, 3, 5 }.AsReadOnlyMemory();

            var list = new List<ReadOnlyMemory<byte>>(new[] { a1, a2, expected, a3 });
            list.Sort(BufferComparer.Memory.Compare);

            Assert.Equal(expected, list[0], BufferComparer.Memory);
        }

        #endregion
    }
}
