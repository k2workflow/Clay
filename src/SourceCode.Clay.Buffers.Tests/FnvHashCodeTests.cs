#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using System;
using System.Text;
using Xunit;

namespace SourceCode.Clay.Buffers.Tests
{
    public static class FnvHashCodeTests
    {
        #region Constants

        private static readonly int[] Expected = new[] { int.MinValue, -42, -1, 0, 1, 42, int.MaxValue };

        #endregion

        #region Methods

        [Theory(DisplayName = nameof(FnvHashCode_TestVectors))]
        [InlineData(0x811C9DC5ul, "")]
        [InlineData(0xe40c292cul, "a")]
        [InlineData(0xe70c2de5ul, "b")]
        [InlineData(0xe60c2c52ul, "c")]
        [InlineData(0xe10c2473ul, "d")]
        [InlineData(0xe00c22e0ul, "e")]
        [InlineData(0xe30c2799ul, "f")]
        [InlineData(0x6222e842ul, "fo")]
        [InlineData(0xa9f37ed7ul, "foo")]
        [InlineData(0x3f5076eful, "foob")]
        [InlineData(0x39aaa18aul, "fooba")]
        [InlineData(0xbf9cf968ul, "foobar")]
        [InlineData(0xd19701c3ul, @"http://www.isthe.com/chongo/tech/math/prime/mersenne.html#largest")]
        public static void FnvHashCode_TestVectors(ulong expected, string value)
        {
            // Using 'FNV-1a 32' test vectors from http://www.isthe.com/chongo/tech/comp/fnv

            var bytes = Encoding.UTF8.GetBytes(value);
            var actual = FnvHashCode.Combine(bytes);

            Assert.Equal(expected, (uint)actual);
        }

        [Theory(DisplayName = nameof(FnvHashCode_Check_2))]
        [InlineData(-1679724187, 0, 0)]
        [InlineData(1270166100, 0, 1)]
        [InlineData(1048580676, 1, 0)]
        [InlineData(-1269751295, 0, -1)]
        [InlineData(-2067818047, -1, 0)]
        [InlineData(-885018111, int.MinValue, int.MaxValue)]
        [InlineData(1482966209, int.MaxValue, int.MinValue)]
        public static void FnvHashCode_Check_2(int expected, int a, int b)
        {
            var actual = FnvHashCode.Combine(a, b);
            Assert.Equal(expected, actual);

            if (a == b)
                Assert.Equal(FnvHashCode.Combine(a, b), FnvHashCode.Combine(b, a));
            else
                Assert.NotEqual(FnvHashCode.Combine(a, b), FnvHashCode.Combine(b, a));
        }

        [Fact(DisplayName = nameof(FnvHashCode_Combine_2))]
        public static void FnvHashCode_Combine_2()
        {
            foreach (var a in Expected)
            {
                foreach (var b in Expected)
                {
                    if (a == b)
                        Assert.Equal(FnvHashCode.Combine(a, b), FnvHashCode.Combine(b, a));
                    else
                        Assert.NotEqual(FnvHashCode.Combine(a, b), FnvHashCode.Combine(b, a));
                }
            }
        }

        [Fact(DisplayName = nameof(FnvHashCode_Combine_3))]
        public static void FnvHashCode_Combine_3()
        {
            foreach (var a in Expected)
            {
                foreach (var b in Expected)
                {
                    foreach (var c in Expected)
                    {
                        var aSpan = new[] { a, b, c }.AsSpan().NonPortableCast<int, byte>();
                        var bSpan = new[] { c, b, a }.AsSpan().NonPortableCast<int, byte>();

                        if (System.Linq.Enumerable.SequenceEqual(new[] { a, b, c }, new[] { c, b, a }))
                        {
                            Assert.Equal(FnvHashCode.Combine(a, b, c), FnvHashCode.Combine(c, b, a));
                            Assert.Equal(FnvHashCode.Combine(aSpan), FnvHashCode.Combine(bSpan));
                        }
                        else
                        {
                            Assert.NotEqual(FnvHashCode.Combine(a, b, c), FnvHashCode.Combine(c, b, a));
                            Assert.NotEqual(FnvHashCode.Combine(bSpan), FnvHashCode.Combine(aSpan));
                        }
                    }
                }
            }
        }

        [Fact(DisplayName = nameof(FnvHashCode_Combine_4))]
        public static void FnvHashCode_Combine_4()
        {
            foreach (var a in Expected)
            {
                foreach (var b in Expected)
                {
                    foreach (var c in Expected)
                    {
                        foreach (var d in Expected)
                        {
                            var aSpan = new[] { a, b, c, d }.AsSpan().NonPortableCast<int, byte>();
                            var bSpan = new[] { d, c, b, a }.AsSpan().NonPortableCast<int, byte>();

                            if (System.Linq.Enumerable.SequenceEqual(new[] { a, b, c, d }, new[] { d, c, b, a }))
                            {
                                Assert.Equal(FnvHashCode.Combine(a, b, c, d), FnvHashCode.Combine(d, c, b, a));
                                Assert.Equal(FnvHashCode.Combine(aSpan), FnvHashCode.Combine(bSpan));
                            }
                            else
                            {
                                Assert.NotEqual(FnvHashCode.Combine(a, b, c, d), FnvHashCode.Combine(d, c, b, a));
                                Assert.NotEqual(FnvHashCode.Combine(aSpan), FnvHashCode.Combine(bSpan));
                            }
                        }
                    }
                }
            }
        }

        [Fact(DisplayName = nameof(FnvHashCode_Combine_5))]
        public static void FnvHashCode_Combine_5()
        {
            foreach (var a in Expected)
            {
                foreach (var b in Expected)
                {
                    foreach (var c in Expected)
                    {
                        foreach (var d in Expected)
                        {
                            foreach (var e in Expected)
                            {
                                var aSpan = new[] { a, b, c, d, e }.AsSpan().NonPortableCast<int, byte>();
                                var bSpan = new[] { e, d, c, b, a }.AsSpan().NonPortableCast<int, byte>();

                                if (System.Linq.Enumerable.SequenceEqual(new[] { a, b, c, d, e }, new[] { e, d, c, b, a }))
                                {
                                    Assert.Equal(FnvHashCode.Combine(a, b, c, d, e), FnvHashCode.Combine(e, d, c, b, a));
                                    Assert.Equal(FnvHashCode.Combine(aSpan), FnvHashCode.Combine(bSpan));
                                }
                                else
                                {
                                    Assert.NotEqual(FnvHashCode.Combine(a, b, c, d, e), FnvHashCode.Combine(e, d, c, b, a));
                                    Assert.NotEqual(FnvHashCode.Combine(aSpan), FnvHashCode.Combine(bSpan));
                                }
                            }
                        }
                    }
                }
            }
        }

        #endregion
    }
}
