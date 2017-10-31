#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using System;
using Xunit;

namespace SourceCode.Clay.Tests
{
    public static class HashCodeTests
    {
        #region Methods

        [Fact(DisplayName = nameof(HashCode_Add_OneValue))]
        public static void HashCode_Add_OneValue()
        {
            var sut = new HashCode();
            sut.Add(1);
            Assert.Equal(1364076727, sut.ToHashCode());
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
            Assert.Equal(861615803, sut.ToHashCode());
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

        [Fact(DisplayName = nameof(HashCode_Add_256Values))]
        public static void HashCode_Add_256Values()
        {
            var sut = new HashCode();
            for (var i = 0; i < 256; i++)
                sut.Add(i);

            Assert.Equal(974456161, sut.ToHashCode());
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

            Assert.Equal(-1261165540, sut.ToHashCode());
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

        [Fact(DisplayName = nameof(HashCode_AddRange))]
        public static void HashCode_AddRange()
        {
            var bytes = new byte[1000];
            for (var i = 0; i < bytes.Length; i++)
                bytes[i] = unchecked((byte)i);

            var sut = new HashCode();
            sut.AddRange(bytes);

            Assert.Equal(-1861433025, sut.ToHashCode());
        }

        [Fact(DisplayName = nameof(HashCode_TallyRange))]
        public static void HashCode_TallyRange()
        {
            var bytes = new byte[1000];
            for (var i = 0; i < bytes.Length; i++)
                bytes[i] = unchecked((byte)i);

            var expected = new HashCode();
            expected.AddRange(bytes);

            var sut = new HashCode().TallyRange(bytes);

            Assert.Equal(expected.ToHashCode(), sut.ToHashCode());
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
