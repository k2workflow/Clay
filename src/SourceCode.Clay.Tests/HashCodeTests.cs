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
            var sut = new HashCode().Add(1);
            Assert.Equal(1364076727, sut.ToHashCode());
        }

        [Fact(DisplayName = nameof(HashCode_Add_TwoValues))]
        public static void HashCode_Add_TwoValues()
        {
            var sut = new HashCode().Add(1).Add(1);
            Assert.Equal(861615803, sut.ToHashCode());
        }

        [Fact(DisplayName = nameof(HashCode_Add_256Values))]
        public static void HashCode_Add_256Values()
        {
            var sut = new HashCode();
            for (var i = 0; i < 256; i++)
                sut = sut.Add(i);

            Assert.Equal(974456161, sut.ToHashCode());
        }

        [Fact(DisplayName = nameof(HashCode_Add_Combine))]
        public static void HashCode_Add_Combine()
        {
            var a = new HashCode().Add(2);
            var sut = new HashCode().Add(1).Add(a);

            Assert.Equal(-218950259, sut.ToHashCode());
        }

        [Fact(DisplayName = nameof(HashCode_Add_Generic))]
        public static void HashCode_Add_Generic()
        {
            var sut = new HashCode().Add(1).Add(100.2);

            Assert.Equal(-1261165540, sut.ToHashCode());
        }

        [Fact(DisplayName = nameof(HashCode_Add_GenericEqualityComparer))]
        public static void HashCode_Add_GenericEqualityComparer()
        {
            var a = new HashCode().Add(1).Add("Hello", StringComparer.Ordinal);
            var b = new HashCode().Add(1).Add("Hello", StringComparer.OrdinalIgnoreCase);

            Assert.NotEqual(a.ToHashCode(), b.ToHashCode());
        }

        [Fact(DisplayName = nameof(HashCode_EqualsOperator))]
        public static void HashCode_EqualsOperator()
        {
            var sut = new HashCode();

#           pragma warning disable CS1718 // Comparison made to same variable
            Assert.True(sut == sut);
#           pragma warning restore CS1718 // Comparison made to same variable
        }

        [Fact(DisplayName = nameof(HashCode_NotEqualsOperator))]
        public static void HashCode_NotEqualsOperator()
        {
            var sut = new HashCode();

#           pragma warning disable CS1718 // Comparison made to same variable
            Assert.False(sut != sut);
#           pragma warning restore CS1718 // Comparison made to same variable
        }

        #endregion
    }
}
