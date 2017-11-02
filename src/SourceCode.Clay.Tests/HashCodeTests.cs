#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using System;
using System.Text;
using Xunit;

namespace SourceCode.Clay.Tests
{
    public static class HashCodeTests
    {
        #region Methods

        [Theory(DisplayName = nameof(HashCode_Add_TestVector))]
        [InlineData("", 0, 0x02cc5d05U)]
        [InlineData("", 42, 0xd5be6eb8U)]
        [InlineData("abcd", 0, 0xa3643705U)]
        [InlineData("abcd", 42, 0xe37d5ac5U)]
        [InlineData("abcd0123", 0, 0x4603e94cU)]
        [InlineData("abcd0123", 42, 0x0a1e5fd9U)]
        [InlineData("abcd0123efgh", 0, 0xd8a1e80fU)]
        [InlineData("abcd0123efgh", 42, 0x50272acfU)]
        [InlineData("abcd0123efgh4567", 0, 0x4b62a7cfU)]
        [InlineData("abcd0123efgh4567", 42, 0xc60be75cU)]
        [InlineData("abcd0123efgh4567ijkl", 0, 0xc33a7641U)]
        [InlineData("abcd0123efgh4567ijkl", 42, 0xc890e852U)]
        [InlineData("abcd0123efgh4567ijkl8901", 0, 0x1a794705U)]
        [InlineData("abcd0123efgh4567ijkl8901", 42, 0x9cf7242dU)]
        [InlineData("abcd0123efgh4567ijkl8901mnop", 0, 0x4d79177dU)]
        [InlineData("abcd0123efgh4567ijkl8901mnop", 42, 0x48bdc35aU)]
        [InlineData("abcd0123efgh4567ijkl8901mnop2345", 0, 0x59d79205U)]
        [InlineData("abcd0123efgh4567ijkl8901mnop2345", 42, 0x4a6b3fc4U)]
        [InlineData("abcd0123efgh4567ijkl8901mnop2345qrst", 0, 0x49585aaeU)]
        [InlineData("abcd0123efgh4567ijkl8901mnop2345qrst", 42, 0xa7c825a9U)]
        [InlineData("abcd0123efgh4567ijkl8901mnop2345qrst6789", 0, 0x2f005ff1U)]
        [InlineData("abcd0123efgh4567ijkl8901mnop2345qrst6789", 42, 0xc1395e63U)]
        [InlineData("abcd0123efgh4567ijkl8901mnop2345qrst6789uvwx", 0, 0x0ce339bdU)]
        [InlineData("abcd0123efgh4567ijkl8901mnop2345qrst6789uvwx", 42, 0x14cbd465U)]
        [InlineData("abcd0123efgh4567ijkl8901mnop2345qrst6789uvwx0123", 0, 0xb31bd2ffU)]
        [InlineData("abcd0123efgh4567ijkl8901mnop2345qrst6789uvwx0123", 42, 0x2305d75dU)]
        [InlineData("abcd0123efgh4567ijkl8901mnop2345qrst6789uvwx0123yzab", 0, 0xa821efa3U)]
        [InlineData("abcd0123efgh4567ijkl8901mnop2345qrst6789uvwx0123yzab", 42, 0xfb9d3ad2)]
        public static void HashCode_Add_TestVector(string vector, int seed, uint expected)
        {
            // Useful: https://asecuritysite.com/encryption/xxHash

            var buffer = Encoding.ASCII.GetBytes(vector);

            var hc = new HashCode(seed);
            for (var i = 0; i < buffer.Length; i += 4)
            {
                var j = BitConverter.ToInt32(buffer, i);
                hc.Add(j);
            }

            unchecked
            {
                Assert.Equal(expected, (uint)hc.ToHashCode());
            }
        }

        [Fact(DisplayName = nameof(HashCode_Add_OneValue))]
        public static void HashCode_Add_OneValue()
        {
            var sut = new HashCode();
            sut.Add(1);
            Assert.Equal(-205818221, sut.ToHashCode());
        }

        [Fact(DisplayName = nameof(HashCode_Tally_OneValue))]
        public static void HashCode_Tally_OneValue()
        {
            var expected = new HashCode();
            expected.Add(1);

            var sut = new HashCode().Tally(1);

            Assert.Equal(expected.ToHashCode(), sut.ToHashCode());
        }

        [Fact(DisplayName = nameof(HashCode_Add_TwoValues))]
        public static void HashCode_Add_TwoValues()
        {
            var sut = new HashCode();
            sut.Add(1);
            sut.Add(1);
            Assert.Equal(-1844029331, sut.ToHashCode());
        }

        [Fact(DisplayName = nameof(HashCode_Tally_TwoValues))]
        public static void HashCode_Tally_TwoValues()
        {
            var expected = new HashCode();
            expected.Add(1);
            expected.Add(1);

            var sut = new HashCode().Tally(1).Tally(1);

            Assert.Equal(expected.ToHashCode(), sut.ToHashCode());
        }

        [Fact(DisplayName = nameof(HashCode_Add_Null))]
        public static void HashCode_Add_Null()
        {
            var sut = new HashCode();
            sut.Add(1);
            sut.Add((string)null);
            Assert.Equal(149775153, sut.ToHashCode());
        }

        [Fact(DisplayName = nameof(HashCode_Tally_Null))]
        public static void HashCode_Tally_Null()
        {
            var expected = new HashCode();
            expected.Add(1);
            expected.Add<string>(null);

            var sut = new HashCode().Tally(1).Tally((string)null);

            Assert.Equal(expected.ToHashCode(), sut.ToHashCode());
        }

        [Fact(DisplayName = nameof(HashCode_Tally_Null_Coalesce))]
        public static void HashCode_Tally_Null_Coalesce()
        {
            int[] list = null;

            var expected = new HashCode();
            expected.Add(-42);
            expected.Add("empty sentinel");
            expected.Add(-1);

            var sut = new HashCode().Tally(null, -42).Tally(null, "empty sentinel").TallyCount(list, -1);

            Assert.Equal(expected.ToHashCode(), sut.ToHashCode());
        }

        [Fact(DisplayName = nameof(HashCode_Add_256Values))]
        public static void HashCode_Add_256Values()
        {
            var sut = new HashCode();
            for (var i = 0; i < 256; i++)
                sut.Add(i);

            Assert.Equal(1358681886, sut.ToHashCode());
        }

        [Fact(DisplayName = nameof(HashCode_Tally_256Values))]
        public static void HashCode_Tally_256Values()
        {
            var expected = new HashCode();
            var sut = new HashCode();
            for (var i = 0; i < 256; i++)
            {
                expected.Add(i);
                sut = sut.Tally(i);
            }

            Assert.Equal(expected.ToHashCode(), sut.ToHashCode());
        }

        [Fact(DisplayName = nameof(HashCode_Add_Generic))]
        public static void HashCode_Add_Generic()
        {
            var sut = new HashCode();
            sut.Add(1);
            sut.Add(100.2);

            Assert.Equal(-273013950, sut.ToHashCode());
        }

        [Fact(DisplayName = nameof(HashCode_Tally_Generic))]
        public static void HashCode_Tally_Generic()
        {
            var expected = new HashCode();
            expected.Add(1);
            expected.Add(100.2);

            var sut = new HashCode().Tally(1).Tally(100.2);

            Assert.Equal(expected.ToHashCode(), sut.ToHashCode());
        }

        [Fact(DisplayName = nameof(HashCode_Add_GenericEqualityComparer))]
        public static void HashCode_Add_GenericEqualityComparer()
        {
            var a = new HashCode();
            a.Add(1);
            a.Add("Hello", StringComparer.Ordinal);

            var b = new HashCode();
            b.Add(1);
            b.Add("Hello", StringComparer.OrdinalIgnoreCase);

            Assert.NotEqual(a.ToHashCode(), b.ToHashCode());
        }

        [Fact(DisplayName = nameof(HashCode_Tally_GenericEqualityComparer))]
        public static void HashCode_Tally_GenericEqualityComparer()
        {
            var expectedA = new HashCode();
            expectedA.Add(1);
            expectedA.Add("Hello", StringComparer.Ordinal);

            var expectedB = new HashCode();
            expectedB.Add(1);
            expectedB.Add("Hello", StringComparer.OrdinalIgnoreCase);

            var sutA = new HashCode().Tally(1).Tally("Hello", StringComparer.Ordinal);
            var sutB = new HashCode().Tally(1).Tally("Hello", StringComparer.OrdinalIgnoreCase);

            Assert.Equal(expectedA.ToHashCode(), sutA.ToHashCode());
            Assert.Equal(expectedB.ToHashCode(), sutB.ToHashCode());
        }

        [Fact(DisplayName = nameof(HashCode_TallyCount_Array))]
        public static void HashCode_TallyCount_Array()
        {
            var bytes = new byte[1000];
            for (var i = 0; i < bytes.Length; i++)
                bytes[i] = unchecked((byte)i);

            var expected = new HashCode();
            expected.Add(bytes.Length);

            var sut = new HashCode().TallyCount(bytes);

            Assert.Equal(expected.ToHashCode(), sut.ToHashCode());
        }

        [Fact(DisplayName = nameof(HashCode_TallyCount_Collection))]
        public static void HashCode_TallyCount_Collection()
        {
            var bytes = new byte[1000];
            for (var i = 0; i < bytes.Length; i++)
                bytes[i] = unchecked((byte)i);

            var expected = new HashCode();
            expected.Add(bytes.Length);

            var sut = new HashCode().TallyCount((System.Collections.ICollection)bytes);

            Assert.Equal(expected.ToHashCode(), sut.ToHashCode());
        }

        [Fact(DisplayName = nameof(HashCode_TallyCount_GenericCollection))]
        public static void HashCode_TallyCount_GenericCollection()
        {
            var bytes = new byte[1000];
            for (var i = 0; i < bytes.Length; i++)
                bytes[i] = unchecked((byte)i);

            var expected = new HashCode();
            expected.Add(bytes.Length);

            var sut = new HashCode().TallyCount((System.Collections.Generic.ICollection<byte>)bytes);

            Assert.Equal(expected.ToHashCode(), sut.ToHashCode());
        }

        [Fact(DisplayName = nameof(HashCode_TallyCount_ReadOnlyGenericCollection))]
        public static void HashCode_TallyCount_ReadOnlyGenericCollection()
        {
            var bytes = new byte[1000];
            for (var i = 0; i < bytes.Length; i++)
                bytes[i] = unchecked((byte)i);

            var expected = new HashCode();
            expected.Add(bytes.Length);

            var sut = new HashCode().TallyCount((System.Collections.Generic.IReadOnlyCollection<byte>)bytes);

            Assert.Equal(expected.ToHashCode(), sut.ToHashCode());
        }

        [Fact(DisplayName = nameof(HashCode_Combine))]
        public static void HashCode_Combine()
        {
            // Check that both:
            // * All parameters are included
            // * No parameters are duplicated
            var hcs = new[]
            {
                HashCode.Combine(1),
                HashCode.Combine(1, 2),
                HashCode.Combine(1, 2, 3),
                HashCode.Combine(1, 2, 3, 4),
                HashCode.Combine(1, 2, 3, 4, 5),
                HashCode.Combine(1, 2, 3, 4, 5, 6),
                HashCode.Combine(1, 2, 3, 4, 5, 6, 7),
                HashCode.Combine(1, 2, 3, 4, 5, 6, 7, 8),

                HashCode.Combine(2),
                HashCode.Combine(2, 3),
                HashCode.Combine(2, 3, 4),
                HashCode.Combine(2, 3, 4, 5),
                HashCode.Combine(2, 3, 4, 5, 6),
                HashCode.Combine(2, 3, 4, 5, 6, 7),
                HashCode.Combine(2, 3, 4, 5, 6, 7, 8),
                HashCode.Combine(2, 3, 4, 5, 6, 7, 8, 9),
            };

            for (var i = 0; i < hcs.Length; i++)
            {
                for (var j = 0; j < hcs.Length; j++)
                {
                    if (i == j) continue;
                    Assert.NotEqual(hcs[i], hcs[j]);
                }
            }
        }

        #endregion
    }
}
