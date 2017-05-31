using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace SourceCode.Clay.Tests
{
    public class BufferComparerTests
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
            Assert.Equal(a, b, BufferComparer.Default);
            Assert.NotEqual(a, c, BufferComparer.Default);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = "BufferComparer Equals List")]
        public static void BufferComparer_Equals_List()
        {
            var a = GenerateArray(16).ToList();
            var b = GenerateArray(16).ToList();
            var c = GenerateArray(16, 1).ToList();
            Assert.Equal(a, b, BufferComparer.Default);
            Assert.NotEqual(a, c, BufferComparer.Default);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = "BufferComparer Equals ReadOnlyList")]
        public static void BufferComparer_Equals_ReadOnlyList()
        {
            var a = new ReadOnlyListWrapper<byte>(GenerateArray(16));
            var b = new ReadOnlyListWrapper<byte>(GenerateArray(16));
            var c = new ReadOnlyListWrapper<byte>(GenerateArray(16, 1));
            Assert.Equal(a, b, BufferComparer.Default);
            Assert.NotEqual(a, c, BufferComparer.Default);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = "BufferComparer Equals Enumerable")]
        public static void BufferComparer_Equals_Enumerable()
        {
            var a = GenerateArray(16).Take(16);
            var b = GenerateArray(16).Take(16);
            var c = GenerateArray(16, 1).Take(16);
            Assert.Equal(a, b, BufferComparer.Default);
            Assert.NotEqual(a, c, BufferComparer.Default);
        } 

        #endregion
    }
}
