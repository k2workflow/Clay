// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Xunit;

namespace SourceCode.Clay.Tests
{
    public static partial class SpanTests
    {
        [Theory]
        [InlineData("  Hello  ", new char[] { ' ' }, "Hello")]
        [InlineData(".  Hello  ..", new char[] { '.' }, "  Hello  ")]
        [InlineData(".  Hello  ..", new char[] { '.', ' ' }, "Hello")]
        [InlineData("123abcHello123abc", new char[] { '1', '2', '3', 'a', 'b', 'c' }, "Hello")]
        [InlineData("  Hello  ", null, "Hello")]
        [InlineData("  Hello  ", new char[0], "Hello")]
        [InlineData("      \t      ", null, "")]
        [InlineData("", null, "")]
        [InlineData("      ", new char[] { ' ' }, "")]
        [InlineData("aaaaa", new char[] { 'a' }, "")]
        [InlineData("abaabaa", new char[] { 'b', 'a' }, "")]
        public static void Trim(string s, char[] trimChars, string expected)
        {
            if (trimChars == null || trimChars.Length == 0 || (trimChars.Length == 1 && trimChars[0] == ' '))
            {
                Assert.Equal(expected, s.Trim());
                Assert.Equal(expected, s.AsSpan().Trim().ToString()); // ReadOnlySpan
                Assert.Equal(expected, new Span<char>(s.ToCharArray()).Trim().ToString());
                Assert.Equal(expected, new Memory<char>(s.ToCharArray()).Trim().ToString());
                Assert.Equal(expected, new ReadOnlyMemory<char>(s.ToCharArray()).Trim().ToString());
            }

            if (trimChars?.Length == 1)
            {
                Assert.Equal(expected, s.Trim(trimChars[0]));
                Assert.Equal(expected, s.AsSpan().Trim(trimChars[0]).ToString());
            }

            Assert.Equal(expected, s.Trim(trimChars));
            Assert.Equal(expected, s.AsSpan().Trim(trimChars).ToString());
        }

        [Theory]
        [InlineData("  Hello  ", new char[] { ' ' }, "  Hello")]
        [InlineData(".  Hello  ..", new char[] { '.' }, ".  Hello  ")]
        [InlineData(".  Hello  ..", new char[] { '.', ' ' }, ".  Hello")]
        [InlineData("123abcHello123abc", new char[] { '1', '2', '3', 'a', 'b', 'c' }, "123abcHello")]
        [InlineData("  Hello  ", null, "  Hello")]
        [InlineData("  Hello  ", new char[0], "  Hello")]
        [InlineData("      \t      ", null, "")]
        [InlineData("", null, "")]
        [InlineData("      ", new char[] { ' ' }, "")]
        [InlineData("aaaaa", new char[] { 'a' }, "")]
        [InlineData("abaabaa", new char[] { 'b', 'a' }, "")]
        public static void TrimEnd(string s, char[] trimChars, string expected)
        {
            if (trimChars == null || trimChars.Length == 0 || (trimChars.Length == 1 && trimChars[0] == ' '))
            {
                Assert.Equal(expected, s.TrimEnd());
                Assert.Equal(expected, s.AsSpan().TrimEnd().ToString()); // ReadOnlySpan
                Assert.Equal(expected, new Span<char>(s.ToCharArray()).TrimEnd().ToString());
                Assert.Equal(expected, new Memory<char>(s.ToCharArray()).TrimEnd().ToString());
                Assert.Equal(expected, new ReadOnlyMemory<char>(s.ToCharArray()).TrimEnd().ToString());
            }

            if (trimChars?.Length == 1)
            {
                Assert.Equal(expected, s.TrimEnd(trimChars[0]));
                Assert.Equal(expected, s.AsSpan().TrimEnd(trimChars[0]).ToString());
            }

            Assert.Equal(expected, s.TrimEnd(trimChars));
            Assert.Equal(expected, s.AsSpan().TrimEnd(trimChars).ToString());
        }

        [Theory]
        [InlineData("  Hello  ", new char[] { ' ' }, "Hello  ")]
        [InlineData(".  Hello  ..", new char[] { '.' }, "  Hello  ..")]
        [InlineData(".  Hello  ..", new char[] { '.', ' ' }, "Hello  ..")]
        [InlineData("123abcHello123abc", new char[] { '1', '2', '3', 'a', 'b', 'c' }, "Hello123abc")]
        [InlineData("  Hello  ", null, "Hello  ")]
        [InlineData("  Hello  ", new char[0], "Hello  ")]
        [InlineData("      \t      ", null, "")]
        [InlineData("", null, "")]
        [InlineData("      ", new char[] { ' ' }, "")]
        [InlineData("aaaaa", new char[] { 'a' }, "")]
        [InlineData("abaabaa", new char[] { 'b', 'a' }, "")]
        public static void TrimStart(string s, char[] trimChars, string expected)
        {
            if (trimChars == null || trimChars.Length == 0 || (trimChars.Length == 1 && trimChars[0] == ' '))
            {
                Assert.Equal(expected, s.TrimStart());
                Assert.Equal(expected, s.AsSpan().TrimStart().ToString()); // ReadOnlySpan
                Assert.Equal(expected, new Span<char>(s.ToCharArray()).TrimStart().ToString());
                Assert.Equal(expected, new Memory<char>(s.ToCharArray()).TrimStart().ToString());
                Assert.Equal(expected, new ReadOnlyMemory<char>(s.ToCharArray()).TrimStart().ToString());
            }

            if (trimChars?.Length == 1)
            {
                Assert.Equal(expected, s.TrimStart(trimChars[0]));
                Assert.Equal(expected, s.AsSpan().TrimStart(trimChars[0]).ToString());
            }

            Assert.Equal(expected, s.TrimStart(trimChars));
            Assert.Equal(expected, s.AsSpan().TrimStart(trimChars).ToString());
        }

        [Fact]
        public static void ZeroLengthTrim()
        {
            string s1 = string.Empty;
            Assert.True(s1.SequenceEqual(s1.Trim()));
            Assert.True(s1.SequenceEqual(s1.TrimStart()));
            Assert.True(s1.SequenceEqual(s1.TrimEnd()));

            ReadOnlySpan<char> span = s1.AsSpan();
            Assert.True(span.SequenceEqual(span.Trim()));
            Assert.True(span.SequenceEqual(span.TrimStart()));
            Assert.True(span.SequenceEqual(span.TrimEnd()));

            var span1 = new Span<char>(s1.ToCharArray());
            Assert.True(span1.SequenceEqual(span1.Trim()));
            Assert.True(span1.SequenceEqual(span1.TrimStart()));
            Assert.True(span1.SequenceEqual(span1.TrimEnd()));

            var mem = new Memory<char>(s1.ToCharArray());
            Assert.True(mem.Span.SequenceEqual(mem.Trim().Span));
            Assert.True(mem.Span.SequenceEqual(mem.TrimStart().Span));
            Assert.True(mem.Span.SequenceEqual(mem.TrimEnd().Span));

            var rom = new ReadOnlyMemory<char>(s1.ToCharArray());
            Assert.True(rom.Span.SequenceEqual(rom.Trim().Span));
            Assert.True(rom.Span.SequenceEqual(rom.TrimStart().Span));
            Assert.True(rom.Span.SequenceEqual(rom.TrimEnd().Span));
        }

        [Fact]
        public static void NoWhiteSpaceTrim()
        {
            for (int length = 0; length < 32; length++)
            {
                char[] a = new char[length];
                for (int i = 0; i < length; i++)
                {
                    a[i] = 'a';
                }

                string s1 = new string(a);
                Assert.True(s1.SequenceEqual(s1.Trim()));
                Assert.True(s1.SequenceEqual(s1.TrimStart()));
                Assert.True(s1.SequenceEqual(s1.TrimEnd()));

                ReadOnlySpan<char> span = s1.AsSpan();
                Assert.True(span.SequenceEqual(span.Trim()));
                Assert.True(span.SequenceEqual(span.TrimStart()));
                Assert.True(span.SequenceEqual(span.TrimEnd()));

                var span1 = new Span<char>(s1.ToCharArray());
                Assert.True(span1.SequenceEqual(span1.Trim()));
                Assert.True(span1.SequenceEqual(span1.TrimStart()));
                Assert.True(span1.SequenceEqual(span1.TrimEnd()));

                var mem = new Memory<char>(s1.ToCharArray());
                Assert.True(mem.Span.SequenceEqual(mem.Trim().Span));
                Assert.True(mem.Span.SequenceEqual(mem.TrimStart().Span));
                Assert.True(mem.Span.SequenceEqual(mem.TrimEnd().Span));

                var rom = new ReadOnlyMemory<char>(s1.ToCharArray());
                Assert.True(rom.Span.SequenceEqual(rom.Trim().Span));
                Assert.True(rom.Span.SequenceEqual(rom.TrimStart().Span));
                Assert.True(rom.Span.SequenceEqual(rom.TrimEnd().Span));
            }
        }

        [Fact]
        public static void OnlyWhiteSpaceTrim()
        {
            for (int length = 0; length < 32; length++)
            {
                char[] a = new char[length];
                for (int i = 0; i < length; i++)
                {
                    a[i] = ' ';
                }

                string s1 = new string(a);
                Assert.True(string.Empty.SequenceEqual(s1.Trim()));
                Assert.True(string.Empty.SequenceEqual(s1.TrimStart()));
                Assert.True(string.Empty.SequenceEqual(s1.TrimEnd()));

                var span = new ReadOnlySpan<char>(a);
                Assert.True(ReadOnlySpan<char>.Empty.SequenceEqual(span.Trim()));
                Assert.True(ReadOnlySpan<char>.Empty.SequenceEqual(span.TrimStart()));
                Assert.True(ReadOnlySpan<char>.Empty.SequenceEqual(span.TrimEnd()));

                var span1 = new Span<char>(s1.ToCharArray());
                Assert.True(Span<char>.Empty.SequenceEqual(span1.Trim()));
                Assert.True(Span<char>.Empty.SequenceEqual(span1.TrimStart()));
                Assert.True(Span<char>.Empty.SequenceEqual(span1.TrimEnd()));

                var mem = new Memory<char>(s1.ToCharArray());
                Assert.True(Memory<char>.Empty.Span.SequenceEqual(mem.Trim().Span));
                Assert.True(Memory<char>.Empty.Span.SequenceEqual(mem.TrimStart().Span));
                Assert.True(Memory<char>.Empty.Span.SequenceEqual(mem.TrimEnd().Span));

                var rom = new ReadOnlyMemory<char>(s1.ToCharArray());
                Assert.True(ReadOnlyMemory<char>.Empty.Span.SequenceEqual(rom.Trim().Span));
                Assert.True(ReadOnlyMemory<char>.Empty.Span.SequenceEqual(rom.TrimStart().Span));
                Assert.True(ReadOnlyMemory<char>.Empty.Span.SequenceEqual(rom.TrimEnd().Span));
            }
        }

        [Fact]
        public static void WhiteSpaceAtStartTrim()
        {
            for (int length = 2; length < 32; length++)
            {
                char[] a = new char[length];
                for (int i = 0; i < length; i++)
                {
                    a[i] = 'a';
                }
                a[0] = ' ';

                string s1 = new string(a);
                Assert.True(s1.Substring(1).SequenceEqual(s1.Trim()));
                Assert.True(s1.Substring(1).SequenceEqual(s1.TrimStart()));
                Assert.True(s1.SequenceEqual(s1.TrimEnd()));

                ReadOnlySpan<char> span = s1.AsSpan();
                Assert.True(span.Slice(1).SequenceEqual(span.Trim()));
                Assert.True(span.Slice(1).SequenceEqual(span.TrimStart()));
                Assert.True(span.SequenceEqual(span.TrimEnd()));

                var span1 = new Span<char>(s1.ToCharArray());
                Assert.True(span1.Slice(1).SequenceEqual(span1.Trim()));
                Assert.True(span1.Slice(1).SequenceEqual(span1.TrimStart()));
                Assert.True(span1.SequenceEqual(span1.TrimEnd()));

                var mem = new Memory<char>(s1.ToCharArray());
                Assert.True(mem.Slice(1).Span.SequenceEqual(mem.Trim().Span));
                Assert.True(mem.Slice(1).Span.SequenceEqual(mem.TrimStart().Span));
                Assert.True(mem.Span.SequenceEqual(mem.TrimEnd().Span));

                var rom = new ReadOnlyMemory<char>(s1.ToCharArray());
                Assert.True(rom.Slice(1).Span.SequenceEqual(rom.Trim().Span));
                Assert.True(rom.Slice(1).Span.SequenceEqual(rom.TrimStart().Span));
                Assert.True(rom.Span.SequenceEqual(rom.TrimEnd().Span));
            }
        }


        [Fact]
        public static void WhiteSpaceAtEndTrim()
        {
            for (int length = 2; length < 32; length++)
            {
                char[] a = new char[length];
                for (int i = 0; i < length; i++)
                {
                    a[i] = 'a';
                }
                a[length - 1] = ' ';

                string s1 = new string(a);
                Assert.True(s1.Substring(0, length - 1).SequenceEqual(s1.Trim()));
                Assert.True(s1.SequenceEqual(s1.TrimStart()));
                Assert.True(s1.Substring(0, length - 1).SequenceEqual(s1.TrimEnd()));

                ReadOnlySpan<char> span = s1.AsSpan();
                Assert.True(span.Slice(0, length - 1).SequenceEqual(span.Trim()));
                Assert.True(span.SequenceEqual(span.TrimStart()));
                Assert.True(span.Slice(0, length - 1).SequenceEqual(span.TrimEnd()));

                var span1 = new Span<char>(s1.ToCharArray());
                Assert.True(span1.Slice(0, length - 1).SequenceEqual(span1.Trim()));
                Assert.True(span1.SequenceEqual(span1.TrimStart()));
                Assert.True(span1.Slice(0, length - 1).SequenceEqual(span1.TrimEnd()));

                var mem = new Memory<char>(s1.ToCharArray());
                Assert.True(mem.Slice(0, length - 1).Span.SequenceEqual(mem.Trim().Span));
                Assert.True(mem.Span.SequenceEqual(mem.TrimStart().Span));
                Assert.True(mem.Slice(0, length - 1).Span.SequenceEqual(mem.TrimEnd().Span));

                var rom = new ReadOnlyMemory<char>(s1.ToCharArray());
                Assert.True(rom.Slice(0, length - 1).Span.SequenceEqual(rom.Trim().Span));
                Assert.True(rom.Span.SequenceEqual(rom.TrimStart().Span));
                Assert.True(rom.Slice(0, length - 1).Span.SequenceEqual(rom.TrimEnd().Span));
            }
        }

        [Fact]
        public static void WhiteSpaceAtStartAndEndTrim()
        {
            for (int length = 3; length < 32; length++)
            {
                char[] a = new char[length];
                for (int i = 0; i < length; i++)
                {
                    a[i] = 'a';
                }
                a[0] = ' ';
                a[length - 1] = ' ';

                string s1 = new string(a);
                Assert.True(s1.Substring(1, length - 2).SequenceEqual(s1.Trim()));
                Assert.True(s1.Substring(1).SequenceEqual(s1.TrimStart()));
                Assert.True(s1.Substring(0, length - 1).SequenceEqual(s1.TrimEnd()));

                ReadOnlySpan<char> span = s1.AsSpan();
                Assert.True(span.Slice(1, length - 2).SequenceEqual(span.Trim()));
                Assert.True(span.Slice(1).SequenceEqual(span.TrimStart()));
                Assert.True(span.Slice(0, length - 1).SequenceEqual(span.TrimEnd()));

                var span1 = new Span<char>(s1.ToCharArray());
                Assert.True(span1.Slice(1, length - 2).SequenceEqual(span1.Trim()));
                Assert.True(span1.Slice(1).SequenceEqual(span1.TrimStart()));
                Assert.True(span1.Slice(0, length - 1).SequenceEqual(span1.TrimEnd()));

                var mem = new Memory<char>(s1.ToCharArray());
                Assert.True(mem.Slice(1, length - 2).Span.SequenceEqual(mem.Trim().Span));
                Assert.True(mem.Slice(1).Span.SequenceEqual(mem.TrimStart().Span));
                Assert.True(mem.Slice(0, length - 1).Span.SequenceEqual(mem.TrimEnd().Span));

                var rom = new ReadOnlyMemory<char>(s1.ToCharArray());
                Assert.True(rom.Slice(1, length - 2).Span.SequenceEqual(rom.Trim().Span));
                Assert.True(rom.Slice(1).Span.SequenceEqual(rom.TrimStart().Span));
                Assert.True(rom.Slice(0, length - 1).Span.SequenceEqual(rom.TrimEnd().Span));
            }
        }

        [Fact]
        public static void WhiteSpaceInMiddleTrim()
        {
            for (int length = 3; length < 32; length++)
            {
                char[] a = new char[length];
                for (int i = 0; i < length; i++)
                {
                    a[i] = 'a';
                }
                a[1] = ' ';

                string s1 = new string(a);
                Assert.True(s1.SequenceEqual(s1.Trim()));
                Assert.True(s1.SequenceEqual(s1.TrimStart()));
                Assert.True(s1.SequenceEqual(s1.TrimEnd()));

                ReadOnlySpan<char> span = s1.AsSpan();
                Assert.True(span.SequenceEqual(span.Trim()));
                Assert.True(span.SequenceEqual(span.TrimStart()));
                Assert.True(span.SequenceEqual(span.TrimEnd()));

                var span1 = new Span<char>(s1.ToCharArray());
                Assert.True(span1.SequenceEqual(span1.Trim()));
                Assert.True(span1.SequenceEqual(span1.TrimStart()));
                Assert.True(span1.SequenceEqual(span1.TrimEnd()));

                var mem = new Memory<char>(s1.ToCharArray());
                Assert.True(mem.Span.SequenceEqual(mem.Trim().Span));
                Assert.True(mem.Span.SequenceEqual(mem.TrimStart().Span));
                Assert.True(mem.Span.SequenceEqual(mem.TrimEnd().Span));

                var rom = new ReadOnlyMemory<char>(s1.ToCharArray());
                Assert.True(rom.Span.SequenceEqual(rom.Trim().Span));
                Assert.True(rom.Span.SequenceEqual(rom.TrimStart().Span));
                Assert.True(rom.Span.SequenceEqual(rom.TrimEnd().Span));
            }
        }

        [Fact]
        public static void TrimWhiteSpaceMultipleTimes()
        {
            for (int length = 3; length < 32; length++)
            {
                char[] a = new char[length];
                for (int i = 0; i < length; i++)
                {
                    a[i] = 'a';
                }
                a[0] = ' ';
                a[length - 1] = ' ';

                string s1 = new string(a);
                string trimResultString = s1.Trim();
                string trimStartResultString = s1.TrimStart();
                string trimEndResultString = s1.TrimEnd();
                Assert.True(s1.Substring(1, length - 2).SequenceEqual(trimResultString));
                Assert.True(s1.Substring(1).SequenceEqual(trimStartResultString));
                Assert.True(s1.Substring(0, length - 1).SequenceEqual(trimEndResultString));

                // 2nd attempt should do nothing
                Assert.True(trimResultString.SequenceEqual(trimResultString.Trim()));
                Assert.True(trimStartResultString.SequenceEqual(trimStartResultString.TrimStart()));
                Assert.True(trimEndResultString.SequenceEqual(trimEndResultString.TrimEnd()));

                // ReadOnlySpan
                {
                    ReadOnlySpan<char> span = s1.AsSpan();
                    ReadOnlySpan<char> trimResult = span.Trim();
                    ReadOnlySpan<char> trimStartResult = span.TrimStart();
                    ReadOnlySpan<char> trimEndResult = span.TrimEnd();
                    Assert.True(span.Slice(1, length - 2).SequenceEqual(trimResult));
                    Assert.True(span.Slice(1).SequenceEqual(trimStartResult));
                    Assert.True(span.Slice(0, length - 1).SequenceEqual(trimEndResult));

                    // 2nd attempt should do nothing
                    Assert.True(trimResult.SequenceEqual(trimResult.Trim()));
                    Assert.True(trimStartResult.SequenceEqual(trimStartResult.TrimStart()));
                    Assert.True(trimEndResult.SequenceEqual(trimEndResult.TrimEnd()));
                }

                // Span
                {
                    var span = new Span<char>(s1.ToCharArray());
                    Span<char> trimResult = span.Trim();
                    Span<char> trimStartResult = span.TrimStart();
                    Span<char> trimEndResult = span.TrimEnd();
                    Assert.True(span.Slice(1, length - 2).SequenceEqual(trimResult));
                    Assert.True(span.Slice(1).SequenceEqual(trimStartResult));
                    Assert.True(span.Slice(0, length - 1).SequenceEqual(trimEndResult));

                    // 2nd attempt should do nothing
                    Assert.True(trimResult.SequenceEqual(trimResult.Trim()));
                    Assert.True(trimStartResult.SequenceEqual(trimStartResult.TrimStart()));
                    Assert.True(trimEndResult.SequenceEqual(trimEndResult.TrimEnd()));
                }

                // Memory
                {
                    var mem = new Memory<char>(s1.ToCharArray());
                    Memory<char> trimResult = mem.Trim();
                    Memory<char> trimStartResult = mem.TrimStart();
                    Memory<char> trimEndResult = mem.TrimEnd();
                    Assert.True(mem.Slice(1, length - 2).Span.SequenceEqual(trimResult.Span));
                    Assert.True(mem.Slice(1).Span.SequenceEqual(trimStartResult.Span));
                    Assert.True(mem.Slice(0, length - 1).Span.SequenceEqual(trimEndResult.Span));

                    // 2nd attempt should do nothing
                    Assert.True(trimResult.Span.SequenceEqual(trimResult.Trim().Span));
                    Assert.True(trimStartResult.Span.SequenceEqual(trimStartResult.TrimStart().Span));
                    Assert.True(trimEndResult.Span.SequenceEqual(trimEndResult.TrimEnd().Span));
                }

                // ReadOnlyMemory
                {
                    var mem = new ReadOnlyMemory<char>(s1.ToCharArray());
                    ReadOnlyMemory<char> trimResult = mem.Trim();
                    ReadOnlyMemory<char> trimStartResult = mem.TrimStart();
                    ReadOnlyMemory<char> trimEndResult = mem.TrimEnd();
                    Assert.True(mem.Slice(1, length - 2).Span.SequenceEqual(trimResult.Span));
                    Assert.True(mem.Slice(1).Span.SequenceEqual(trimStartResult.Span));
                    Assert.True(mem.Slice(0, length - 1).Span.SequenceEqual(trimEndResult.Span));

                    // 2nd attempt should do nothing
                    Assert.True(trimResult.Span.SequenceEqual(trimResult.Trim().Span));
                    Assert.True(trimStartResult.Span.SequenceEqual(trimStartResult.TrimStart().Span));
                    Assert.True(trimEndResult.Span.SequenceEqual(trimEndResult.TrimEnd().Span));
                }
            }
        }

        [Fact]
        public static void MakeSureNoTrimChecksGoOutOfRange()
        {
            for (int length = 3; length < 64; length++)
            {
                char[] first = new char[length];
                first[0] = ' ';
                first[length - 1] = ' ';

                string s1 = new string(first, 1, length - 2);
                Assert.True(s1.SequenceEqual(s1.Trim()));
                Assert.True(s1.SequenceEqual(s1.TrimStart()));
                Assert.True(s1.SequenceEqual(s1.TrimEnd()));

                ReadOnlySpan<char> span = s1.AsSpan();
                Assert.True(span.SequenceEqual(span.Trim()));
                Assert.True(span.SequenceEqual(span.TrimStart()));
                Assert.True(span.SequenceEqual(span.TrimEnd()));

                var span1 = new Span<char>(s1.ToCharArray());
                Assert.True(span1.SequenceEqual(span1.Trim()));
                Assert.True(span1.SequenceEqual(span1.TrimStart()));
                Assert.True(span1.SequenceEqual(span1.TrimEnd()));

                var mem = new Memory<char>(s1.ToCharArray());
                Assert.True(mem.Span.SequenceEqual(mem.Trim().Span));
                Assert.True(mem.Span.SequenceEqual(mem.TrimStart().Span));
                Assert.True(mem.Span.SequenceEqual(mem.TrimEnd().Span));

                var rom = new ReadOnlyMemory<char>(s1.ToCharArray());
                Assert.True(rom.Span.SequenceEqual(rom.Trim().Span));
                Assert.True(rom.Span.SequenceEqual(rom.TrimStart().Span));
                Assert.True(rom.Span.SequenceEqual(rom.TrimEnd().Span));
            }
        }

        [Theory]
        [InlineData(new int[] { 1 }, 1, new int[] { })]
        [InlineData(new int[] { 2 }, 1, new int[] { 2 })]
        [InlineData(new int[] { 1, 2, 1 }, 1, new int[] { 2, 1 })]
        [InlineData(new int[] { 1, 1, 2, 1 }, 1, new int[] { 2, 1 })]
        [InlineData(new int[] { 1, 1, 2, 1 }, 2, new int[] { 1, 1, 2, 1 })]
        [InlineData(new int[] { 1, 1, 2, 1 }, 3, new int[] { 1, 1, 2, 1 })]
        [InlineData(new int[] { 1, 1, 1, 2 }, 1, new int[] { 2 })]
        [InlineData(new int[] { 1, 1, 1, 1 }, 1, new int[] { })]
        public static void MemoryExtensions_TrimStart_Single(int[] values, int trim, int[] expected)
        {
            Span<int> span = new Span<int>(values).TrimStart(trim);
            Assert.True(System.Linq.Enumerable.SequenceEqual(expected, span.ToArray()));
        }

        [Theory]
        [InlineData(new int[] { 1 }, 1, new int[] { })]
        [InlineData(new int[] { 2 }, 1, new int[] { 2 })]
        [InlineData(new int[] { 1, 2, 1 }, 1, new int[] { 1, 2 })]
        [InlineData(new int[] { 1, 2, 1, 1 }, 1, new int[] { 1, 2 })]
        [InlineData(new int[] { 1, 2, 1, 1 }, 2, new int[] { 1, 2, 1, 1 })]
        [InlineData(new int[] { 1, 2, 1, 1 }, 3, new int[] { 1, 2, 1, 1 })]
        [InlineData(new int[] { 2, 1, 1, 1 }, 1, new int[] { 2 })]
        [InlineData(new int[] { 1, 1, 1, 1 }, 1, new int[] { })]
        public static void MemoryExtensions_TrimEnd_Single(int[] values, int trim, int[] expected)
        {
            Span<int> span = new Span<int>(values).TrimEnd(trim);
            Assert.True(System.Linq.Enumerable.SequenceEqual(expected, span.ToArray()));
        }

        [Theory]
        [InlineData(new int[] { 1 }, 1, new int[] { })]
        [InlineData(new int[] { 2 }, 1, new int[] { 2 })]
        [InlineData(new int[] { 1, 2, 1 }, 1, new int[] { 2 })]
        [InlineData(new int[] { 1, 2, 1, 1 }, 1, new int[] { 2 })]
        [InlineData(new int[] { 1, 2, 1, 1 }, 2, new int[] { 1, 2, 1, 1 })]
        [InlineData(new int[] { 1, 2, 1, 1 }, 3, new int[] { 1, 2, 1, 1 })]
        [InlineData(new int[] { 2, 1, 1, 1 }, 1, new int[] { 2 })]
        [InlineData(new int[] { 1, 1, 1, 2 }, 1, new int[] { 2 })]
        [InlineData(new int[] { 1, 1, 1, 1 }, 1, new int[] { })]
        public static void MemoryExtensions_Trim_Single(int[] values, int trim, int[] expected)
        {
            Span<int> span = new Span<int>(values).Trim(trim);
            Assert.True(System.Linq.Enumerable.SequenceEqual(expected, span.ToArray()));
        }

        [Theory]
        [InlineData(new int[] { 1 }, new int[] { 1 }, new int[] { })]
        [InlineData(new int[] { 2 }, new int[] { 1 }, new int[] { 2 })]
        [InlineData(new int[] { 1, 2, 1 }, new int[] { 1 }, new int[] { 2, 1 })]
        [InlineData(new int[] { 1, 1, 2, 1 }, new int[] { 1 }, new int[] { 2, 1 })]
        [InlineData(new int[] { 1, 1, 2, 1 }, new int[] { 2 }, new int[] { 1, 1, 2, 1 })]
        [InlineData(new int[] { 1, 1, 2, 1 }, new int[] { 3 }, new int[] { 1, 1, 2, 1 })]
        [InlineData(new int[] { 1, 1, 2, 1 }, new int[] { 1, 2 }, new int[] { })]
        [InlineData(new int[] { 1, 1, 2, 3 }, new int[] { 1, 2 }, new int[] { 3 })]
        [InlineData(new int[] { 1, 1, 2, 3 }, new int[] { 1, 2, 4 }, new int[] { 3 })]
        [InlineData(new int[] { 1, 1, 1, 2 }, new int[] { 1 }, new int[] { 2 })]
        [InlineData(new int[] { 1, 1, 1, 1 }, new int[] { 1 }, new int[] { })]
        public static void MemoryExtensions_TrimStart_Multi(int[] values, int[] trims, int[] expected)
        {
            Span<int> span = new Span<int>(values).TrimStart(trims);
            Assert.True(System.Linq.Enumerable.SequenceEqual(expected, span.ToArray()));
        }

        [Theory]
        [InlineData(new int[] { 1 }, new int[] { 1 }, new int[] { })]
        [InlineData(new int[] { 2 }, new int[] { 1 }, new int[] { 2 })]
        [InlineData(new int[] { 1, 2, 1 }, new int[] { 1 }, new int[] { 1, 2 })]
        [InlineData(new int[] { 1, 2, 1, 1 }, new int[] { 1 }, new int[] { 1, 2 })]
        [InlineData(new int[] { 1, 2, 1, 1 }, new int[] { 2 }, new int[] { 1, 2, 1, 1 })]
        [InlineData(new int[] { 1, 2, 1, 1 }, new int[] { 3 }, new int[] { 1, 2, 1, 1 })]
        [InlineData(new int[] { 1, 2, 1, 1 }, new int[] { 1, 2 }, new int[] { })]
        [InlineData(new int[] { 3, 2, 1, 1 }, new int[] { 1, 2 }, new int[] { 3 })]
        [InlineData(new int[] { 3, 2, 1, 1 }, new int[] { 1, 2, 4 }, new int[] { 3 })]
        [InlineData(new int[] { 2, 1, 1, 1 }, new int[] { 1 }, new int[] { 2 })]
        [InlineData(new int[] { 1, 1, 1, 1 }, new int[] { 1 }, new int[] { })]
        public static void MemoryExtensions_TrimEnd_Multi(int[] values, int[] trims, int[] expected)
        {
            Span<int> span = new Span<int>(values).TrimEnd(trims);
            Assert.True(System.Linq.Enumerable.SequenceEqual(expected, span.ToArray()));
        }

        [Theory]
        [InlineData(new int[] { 1 }, new int[] { 1 }, new int[] { })]
        [InlineData(new int[] { 2 }, new int[] { 1 }, new int[] { 2 })]
        [InlineData(new int[] { 1, 2, 1 }, new int[] { 1 }, new int[] { 2 })]
        [InlineData(new int[] { 1, 2, 1, 1 }, new int[] { 1 }, new int[] { 2 })]
        [InlineData(new int[] { 1, 2, 1, 1 }, new int[] { 2 }, new int[] { 1, 2, 1, 1 })]
        [InlineData(new int[] { 1, 2, 1, 1 }, new int[] { 3 }, new int[] { 1, 2, 1, 1 })]
        [InlineData(new int[] { 1, 2, 1, 1 }, new int[] { 1, 2 }, new int[] { })]
        [InlineData(new int[] { 2, 1, 3, 2, 1, 1 }, new int[] { 1, 2 }, new int[] { 3 })]
        [InlineData(new int[] { 2, 1, 3, 2, 1, 1 }, new int[] { 1, 2, 4 }, new int[] { 3 })]
        [InlineData(new int[] { 1, 2, 1, 1, 1 }, new int[] { 1 }, new int[] { 2 })]
        [InlineData(new int[] { 1, 1, 1, 1 }, new int[] { 1 }, new int[] { })]
        public static void MemoryExtensions_Trim_Multi(int[] values, int[] trims, int[] expected)
        {
            Span<int> span = new Span<int>(values).Trim(trims);
            Assert.True(System.Linq.Enumerable.SequenceEqual(expected, span.ToArray()));
        }

        private sealed class Foo : IEquatable<Foo>
        {
            public int Value { get; set; }

            public bool Equals(Foo other)
            {
                if (this == null && other == null)
                    return true;
                if (other == null)
                    return false;
                return Value == other.Value;
            }

            public static implicit operator Foo(int value) => new Foo { Value = value };
            public static implicit operator int? (Foo foo) => foo?.Value;
        }

        [Fact]
        public static void MemoryExtensions_TrimStart_Single_Null()
        {
            var values = new Foo[] { null, null, 1, 2, null, null };
            var trim = (Foo)null;

            var expected = new Foo[] { 1, 2, null, null };

            Span<Foo> span = new Span<Foo>(values).TrimStart(trim);
            Assert.True(System.Linq.Enumerable.SequenceEqual(expected, span.ToArray()));

        }

        [Fact]
        public static void MemoryExtensions_TrimStart_Multi_Null()
        {
            var values = new Foo[] { null, 1, 2, 3, null, 2, 1, null };
            var trim = new Foo[] { null, 1, 2 };

            var expected = new Foo[] { 3, null, 2, 1, null };

            Span<Foo> span = new Span<Foo>(values).TrimStart(trim);
            Assert.True(System.Linq.Enumerable.SequenceEqual(expected, span.ToArray()));
        }

        [Fact]
        public static void MemoryExtensions_TrimEnd_Single_Null()
        {
            var values = new Foo[] { null, null, 1, 2, null, null };
            var trim = (Foo)null;

            var expected = new Foo[] { null, null, 1, 2 };

            Span<Foo> span = new Span<Foo>(values).TrimEnd(trim);
            Assert.True(System.Linq.Enumerable.SequenceEqual(expected, span.ToArray()));
        }

        [Fact]
        public static void MemoryExtensions_TrimEnd_Multi_Null()
        {
            var values = new Foo[] { null, 1, 2, 3, null, 2, 1, null };
            var trim = new Foo[] { null, 1, 2 };

            var expected = new Foo[] { null, 1, 2, 3 };

            Span<Foo> span = new Span<Foo>(values).TrimEnd(trim);
            Assert.True(System.Linq.Enumerable.SequenceEqual(expected, span.ToArray()));
        }

        [Fact]
        public static void MemoryExtensions_Trim_Single_Null()
        {
            var values = new Foo[] { null, null, 1, 2, null, null };
            var trim = (Foo)null;

            var expected = new Foo[] { 1, 2 };

            Span<Foo> span = new Span<Foo>(values).Trim(trim);
            Assert.True(System.Linq.Enumerable.SequenceEqual(expected, span.ToArray()));
        }

        [Fact]
        public static void MemoryExtensions_Trim_Multi_Null()
        {
            var values = new Foo[] { null, 1, 2, 3, null, 2, 1, null };
            var trim = new Foo[] { null, 1, 2 };

            var expected = new Foo[] { 3 };

            Span<Foo> span = new Span<Foo>(values).Trim(trim);
            Assert.True(System.Linq.Enumerable.SequenceEqual(expected, span.ToArray()));
        }
    }

    public static partial class ReadOnlySpanTests
    {
        [Theory]
        [InlineData(new int[] { 1 }, 1, new int[] { })]
        [InlineData(new int[] { 2 }, 1, new int[] { 2 })]
        [InlineData(new int[] { 1, 2, 1 }, 1, new int[] { 2, 1 })]
        [InlineData(new int[] { 1, 1, 2, 1 }, 1, new int[] { 2, 1 })]
        [InlineData(new int[] { 1, 1, 2, 1 }, 2, new int[] { 1, 1, 2, 1 })]
        [InlineData(new int[] { 1, 1, 2, 1 }, 3, new int[] { 1, 1, 2, 1 })]
        [InlineData(new int[] { 1, 1, 1, 2 }, 1, new int[] { 2 })]
        [InlineData(new int[] { 1, 1, 1, 1 }, 1, new int[] { })]
        public static void MemoryExtensions_TrimStart_Single(int[] values, int trim, int[] expected)
        {
            ReadOnlySpan<int> ros = new ReadOnlySpan<int>(values).TrimStart(trim);
            Assert.True(System.Linq.Enumerable.SequenceEqual(expected, ros.ToArray()));
        }

        [Theory]
        [InlineData(new int[] { 1 }, 1, new int[] { })]
        [InlineData(new int[] { 2 }, 1, new int[] { 2 })]
        [InlineData(new int[] { 1, 2, 1 }, 1, new int[] { 1, 2 })]
        [InlineData(new int[] { 1, 2, 1, 1 }, 1, new int[] { 1, 2 })]
        [InlineData(new int[] { 1, 2, 1, 1 }, 2, new int[] { 1, 2, 1, 1 })]
        [InlineData(new int[] { 1, 2, 1, 1 }, 3, new int[] { 1, 2, 1, 1 })]
        [InlineData(new int[] { 2, 1, 1, 1 }, 1, new int[] { 2 })]
        [InlineData(new int[] { 1, 1, 1, 1 }, 1, new int[] { })]
        public static void MemoryExtensions_TrimEnd_Single(int[] values, int trim, int[] expected)
        {
            ReadOnlySpan<int> ros = new ReadOnlySpan<int>(values).TrimEnd(trim);
            Assert.True(System.Linq.Enumerable.SequenceEqual(expected, ros.ToArray()));
        }

        [Theory]
        [InlineData(new int[] { 1 }, 1, new int[] { })]
        [InlineData(new int[] { 2 }, 1, new int[] { 2 })]
        [InlineData(new int[] { 1, 2, 1 }, 1, new int[] { 2 })]
        [InlineData(new int[] { 1, 2, 1, 1 }, 1, new int[] { 2 })]
        [InlineData(new int[] { 1, 2, 1, 1 }, 2, new int[] { 1, 2, 1, 1 })]
        [InlineData(new int[] { 1, 2, 1, 1 }, 3, new int[] { 1, 2, 1, 1 })]
        [InlineData(new int[] { 2, 1, 1, 1 }, 1, new int[] { 2 })]
        [InlineData(new int[] { 1, 1, 1, 2 }, 1, new int[] { 2 })]
        [InlineData(new int[] { 1, 1, 1, 1 }, 1, new int[] { })]
        public static void MemoryExtensions_Trim_Single(int[] values, int trim, int[] expected)
        {
            ReadOnlySpan<int> ros = new ReadOnlySpan<int>(values).Trim(trim);
            Assert.True(System.Linq.Enumerable.SequenceEqual(expected, ros.ToArray()));
        }

        [Theory]
        [InlineData(new int[] { 1 }, new int[] { 1 }, new int[] { })]
        [InlineData(new int[] { 2 }, new int[] { 1 }, new int[] { 2 })]
        [InlineData(new int[] { 1, 2, 1 }, new int[] { 1 }, new int[] { 2, 1 })]
        [InlineData(new int[] { 1, 1, 2, 1 }, new int[] { 1 }, new int[] { 2, 1 })]
        [InlineData(new int[] { 1, 1, 2, 1 }, new int[] { 2 }, new int[] { 1, 1, 2, 1 })]
        [InlineData(new int[] { 1, 1, 2, 1 }, new int[] { 3 }, new int[] { 1, 1, 2, 1 })]
        [InlineData(new int[] { 1, 1, 2, 1 }, new int[] { 1, 2 }, new int[] { })]
        [InlineData(new int[] { 1, 1, 2, 3 }, new int[] { 1, 2 }, new int[] { 3 })]
        [InlineData(new int[] { 1, 1, 2, 3 }, new int[] { 1, 2, 4 }, new int[] { 3 })]
        [InlineData(new int[] { 1, 1, 1, 2 }, new int[] { 1 }, new int[] { 2 })]
        [InlineData(new int[] { 1, 1, 1, 1 }, new int[] { 1 }, new int[] { })]
        public static void MemoryExtensions_TrimStart_Multi(int[] values, int[] trims, int[] expected)
        {
            ReadOnlySpan<int> ros = new ReadOnlySpan<int>(values).TrimStart(trims);
            Assert.True(System.Linq.Enumerable.SequenceEqual(expected, ros.ToArray()));
        }

        [Theory]
        [InlineData(new int[] { 1 }, new int[] { 1 }, new int[] { })]
        [InlineData(new int[] { 2 }, new int[] { 1 }, new int[] { 2 })]
        [InlineData(new int[] { 1, 2, 1 }, new int[] { 1 }, new int[] { 1, 2 })]
        [InlineData(new int[] { 1, 2, 1, 1 }, new int[] { 1 }, new int[] { 1, 2 })]
        [InlineData(new int[] { 1, 2, 1, 1 }, new int[] { 2 }, new int[] { 1, 2, 1, 1 })]
        [InlineData(new int[] { 1, 2, 1, 1 }, new int[] { 3 }, new int[] { 1, 2, 1, 1 })]
        [InlineData(new int[] { 1, 2, 1, 1 }, new int[] { 1, 2 }, new int[] { })]
        [InlineData(new int[] { 3, 2, 1, 1 }, new int[] { 1, 2 }, new int[] { 3 })]
        [InlineData(new int[] { 3, 2, 1, 1 }, new int[] { 1, 2, 4 }, new int[] { 3 })]
        [InlineData(new int[] { 2, 1, 1, 1 }, new int[] { 1 }, new int[] { 2 })]
        [InlineData(new int[] { 1, 1, 1, 1 }, new int[] { 1 }, new int[] { })]
        public static void MemoryExtensions_TrimEnd_Multi(int[] values, int[] trims, int[] expected)
        {
            ReadOnlySpan<int> ros = new ReadOnlySpan<int>(values).TrimEnd(trims);
            Assert.True(System.Linq.Enumerable.SequenceEqual(expected, ros.ToArray()));
        }

        [Theory]
        [InlineData(new int[] { 1 }, new int[] { 1 }, new int[] { })]
        [InlineData(new int[] { 2 }, new int[] { 1 }, new int[] { 2 })]
        [InlineData(new int[] { 1, 2, 1 }, new int[] { 1 }, new int[] { 2 })]
        [InlineData(new int[] { 1, 2, 1, 1 }, new int[] { 1 }, new int[] { 2 })]
        [InlineData(new int[] { 1, 2, 1, 1 }, new int[] { 2 }, new int[] { 1, 2, 1, 1 })]
        [InlineData(new int[] { 1, 2, 1, 1 }, new int[] { 3 }, new int[] { 1, 2, 1, 1 })]
        [InlineData(new int[] { 1, 2, 1, 1 }, new int[] { 1, 2 }, new int[] { })]
        [InlineData(new int[] { 2, 1, 3, 2, 1, 1 }, new int[] { 1, 2 }, new int[] { 3 })]
        [InlineData(new int[] { 2, 1, 3, 2, 1, 1 }, new int[] { 1, 2, 4 }, new int[] { 3 })]
        [InlineData(new int[] { 1, 2, 1, 1, 1 }, new int[] { 1 }, new int[] { 2 })]
        [InlineData(new int[] { 1, 1, 1, 1 }, new int[] { 1 }, new int[] { })]
        public static void MemoryExtensions_Trim_Multi(int[] values, int[] trims, int[] expected)
        {
            ReadOnlySpan<int> ros = new ReadOnlySpan<int>(values).Trim(trims);
            Assert.True(System.Linq.Enumerable.SequenceEqual(expected, ros.ToArray()));
        }

        private sealed class Foo : IEquatable<Foo>
        {
            public int Value { get; set; }

            public bool Equals(Foo other)
            {
                if (this == null && other == null)
                    return true;
                if (other == null)
                    return false;
                return Value == other.Value;
            }

            public static implicit operator Foo(int value) => new Foo { Value = value };
            public static implicit operator int? (Foo foo) => foo?.Value;
        }

        [Fact]
        public static void MemoryExtensions_TrimStart_Single_Null()
        {
            var values = new Foo[] { null, null, 1, 2, null, null };
            var trim = (Foo)null;

            var expected = new Foo[] { 1, 2, null, null };

            ReadOnlySpan<Foo> ros = new ReadOnlySpan<Foo>(values).TrimStart(trim);
            Assert.True(System.Linq.Enumerable.SequenceEqual(expected, ros.ToArray()));
        }

        [Fact]
        public static void MemoryExtensions_TrimStart_Multi_Null()
        {
            var values = new Foo[] { null, 1, 2, 3, null, 2, 1, null };
            var trim = new Foo[] { null, 1, 2 };

            var expected = new Foo[] { 3, null, 2, 1, null };

            ReadOnlySpan<Foo> ros = new ReadOnlySpan<Foo>(values).TrimStart(trim);
            Assert.True(System.Linq.Enumerable.SequenceEqual(expected, ros.ToArray()));
        }

        [Fact]
        public static void MemoryExtensions_TrimEnd_Single_Null()
        {
            var values = new Foo[] { null, null, 1, 2, null, null };
            var trim = (Foo)null;

            var expected = new Foo[] { null, null, 1, 2 };

            ReadOnlySpan<Foo> ros = new ReadOnlySpan<Foo>(values).TrimEnd(trim);
            Assert.True(System.Linq.Enumerable.SequenceEqual(expected, ros.ToArray()));
        }

        [Fact]
        public static void MemoryExtensions_TrimEnd_Multi_Null()
        {
            var values = new Foo[] { null, 1, 2, 3, null, 2, 1, null };
            var trim = new Foo[] { null, 1, 2 };

            var expected = new Foo[] { null, 1, 2, 3 };

            ReadOnlySpan<Foo> ros = new ReadOnlySpan<Foo>(values).TrimEnd(trim);
            Assert.True(System.Linq.Enumerable.SequenceEqual(expected, ros.ToArray()));
        }

        [Fact]
        public static void MemoryExtensions_Trim_Single_Null()
        {
            var values = new Foo[] { null, null, 1, 2, null, null };
            var trim = (Foo)null;

            var expected = new Foo[] { 1, 2 };

            ReadOnlySpan<Foo> ros = new ReadOnlySpan<Foo>(values).Trim(trim);
            Assert.True(System.Linq.Enumerable.SequenceEqual(expected, ros.ToArray()));
        }

        [Fact]
        public static void MemoryExtensions_Trim_Multi_Null()
        {
            var values = new Foo[] { null, 1, 2, 3, null, 2, 1, null };
            var trim = new Foo[] { null, 1, 2 };

            var expected = new Foo[] { 3 };

            ReadOnlySpan<Foo> ros = new ReadOnlySpan<Foo>(values).Trim(trim);
            Assert.True(System.Linq.Enumerable.SequenceEqual(expected, ros.ToArray()));
        }
    }

    public static partial class MemoryTests
    {
        [Theory]
        [InlineData(new int[] { 1 }, 1, new int[] { })]
        [InlineData(new int[] { 2 }, 1, new int[] { 2 })]
        [InlineData(new int[] { 1, 2, 1 }, 1, new int[] { 2, 1 })]
        [InlineData(new int[] { 1, 1, 2, 1 }, 1, new int[] { 2, 1 })]
        [InlineData(new int[] { 1, 1, 2, 1 }, 2, new int[] { 1, 1, 2, 1 })]
        [InlineData(new int[] { 1, 1, 2, 1 }, 3, new int[] { 1, 1, 2, 1 })]
        [InlineData(new int[] { 1, 1, 1, 2 }, 1, new int[] { 2 })]
        [InlineData(new int[] { 1, 1, 1, 1 }, 1, new int[] { })]
        public static void MemoryExtensions_TrimStart_Single(int[] values, int trim, int[] expected)
        {
            Memory<int> memory = new Memory<int>(values).TrimStart(trim);
            Assert.True(System.Linq.Enumerable.SequenceEqual(expected, memory.ToArray()));
        }

        [Theory]
        [InlineData(new int[] { 1 }, 1, new int[] { })]
        [InlineData(new int[] { 2 }, 1, new int[] { 2 })]
        [InlineData(new int[] { 1, 2, 1 }, 1, new int[] { 1, 2 })]
        [InlineData(new int[] { 1, 2, 1, 1 }, 1, new int[] { 1, 2 })]
        [InlineData(new int[] { 1, 2, 1, 1 }, 2, new int[] { 1, 2, 1, 1 })]
        [InlineData(new int[] { 1, 2, 1, 1 }, 3, new int[] { 1, 2, 1, 1 })]
        [InlineData(new int[] { 2, 1, 1, 1 }, 1, new int[] { 2 })]
        [InlineData(new int[] { 1, 1, 1, 1 }, 1, new int[] { })]
        public static void MemoryExtensions_TrimEnd_Single(int[] values, int trim, int[] expected)
        {
            Memory<int> memory = new Memory<int>(values).TrimEnd(trim);
            Assert.True(System.Linq.Enumerable.SequenceEqual(expected, memory.ToArray()));
        }

        [Theory]
        [InlineData(new int[] { 1 }, 1, new int[] { })]
        [InlineData(new int[] { 2 }, 1, new int[] { 2 })]
        [InlineData(new int[] { 1, 2, 1 }, 1, new int[] { 2 })]
        [InlineData(new int[] { 1, 2, 1, 1 }, 1, new int[] { 2 })]
        [InlineData(new int[] { 1, 2, 1, 1 }, 2, new int[] { 1, 2, 1, 1 })]
        [InlineData(new int[] { 1, 2, 1, 1 }, 3, new int[] { 1, 2, 1, 1 })]
        [InlineData(new int[] { 2, 1, 1, 1 }, 1, new int[] { 2 })]
        [InlineData(new int[] { 1, 1, 1, 2 }, 1, new int[] { 2 })]
        [InlineData(new int[] { 1, 1, 1, 1 }, 1, new int[] { })]
        public static void MemoryExtensions_Trim_Single(int[] values, int trim, int[] expected)
        {
            Memory<int> memory = new Memory<int>(values).Trim(trim);
            Assert.True(System.Linq.Enumerable.SequenceEqual(expected, memory.ToArray()));
        }

        [Theory]
        [InlineData(new int[] { 1 }, new int[] { 1 }, new int[] { })]
        [InlineData(new int[] { 2 }, new int[] { 1 }, new int[] { 2 })]
        [InlineData(new int[] { 1, 2, 1 }, new int[] { 1 }, new int[] { 2, 1 })]
        [InlineData(new int[] { 1, 1, 2, 1 }, new int[] { 1 }, new int[] { 2, 1 })]
        [InlineData(new int[] { 1, 1, 2, 1 }, new int[] { 2 }, new int[] { 1, 1, 2, 1 })]
        [InlineData(new int[] { 1, 1, 2, 1 }, new int[] { 3 }, new int[] { 1, 1, 2, 1 })]
        [InlineData(new int[] { 1, 1, 2, 1 }, new int[] { 1, 2 }, new int[] { })]
        [InlineData(new int[] { 1, 1, 2, 3 }, new int[] { 1, 2 }, new int[] { 3 })]
        [InlineData(new int[] { 1, 1, 2, 3 }, new int[] { 1, 2, 4 }, new int[] { 3 })]
        [InlineData(new int[] { 1, 1, 1, 2 }, new int[] { 1 }, new int[] { 2 })]
        [InlineData(new int[] { 1, 1, 1, 1 }, new int[] { 1 }, new int[] { })]
        public static void MemoryExtensions_TrimStart_Multi(int[] values, int[] trims, int[] expected)
        {
            Memory<int> memory = new Memory<int>(values).TrimStart(trims);
            Assert.True(System.Linq.Enumerable.SequenceEqual(expected, memory.ToArray()));
        }

        [Theory]
        [InlineData(new int[] { 1 }, new int[] { 1 }, new int[] { })]
        [InlineData(new int[] { 2 }, new int[] { 1 }, new int[] { 2 })]
        [InlineData(new int[] { 1, 2, 1 }, new int[] { 1 }, new int[] { 1, 2 })]
        [InlineData(new int[] { 1, 2, 1, 1 }, new int[] { 1 }, new int[] { 1, 2 })]
        [InlineData(new int[] { 1, 2, 1, 1 }, new int[] { 2 }, new int[] { 1, 2, 1, 1 })]
        [InlineData(new int[] { 1, 2, 1, 1 }, new int[] { 3 }, new int[] { 1, 2, 1, 1 })]
        [InlineData(new int[] { 1, 2, 1, 1 }, new int[] { 1, 2 }, new int[] { })]
        [InlineData(new int[] { 3, 2, 1, 1 }, new int[] { 1, 2 }, new int[] { 3 })]
        [InlineData(new int[] { 3, 2, 1, 1 }, new int[] { 1, 2, 4 }, new int[] { 3 })]
        [InlineData(new int[] { 2, 1, 1, 1 }, new int[] { 1 }, new int[] { 2 })]
        [InlineData(new int[] { 1, 1, 1, 1 }, new int[] { 1 }, new int[] { })]
        public static void MemoryExtensions_TrimEnd_Multi(int[] values, int[] trims, int[] expected)
        {
            Memory<int> memory = new Memory<int>(values).TrimEnd(trims);
            Assert.True(System.Linq.Enumerable.SequenceEqual(expected, memory.ToArray()));
        }

        [Theory]
        [InlineData(new int[] { 1 }, new int[] { 1 }, new int[] { })]
        [InlineData(new int[] { 2 }, new int[] { 1 }, new int[] { 2 })]
        [InlineData(new int[] { 1, 2, 1 }, new int[] { 1 }, new int[] { 2 })]
        [InlineData(new int[] { 1, 2, 1, 1 }, new int[] { 1 }, new int[] { 2 })]
        [InlineData(new int[] { 1, 2, 1, 1 }, new int[] { 2 }, new int[] { 1, 2, 1, 1 })]
        [InlineData(new int[] { 1, 2, 1, 1 }, new int[] { 3 }, new int[] { 1, 2, 1, 1 })]
        [InlineData(new int[] { 1, 2, 1, 1 }, new int[] { 1, 2 }, new int[] { })]
        [InlineData(new int[] { 2, 1, 3, 2, 1, 1 }, new int[] { 1, 2 }, new int[] { 3 })]
        [InlineData(new int[] { 2, 1, 3, 2, 1, 1 }, new int[] { 1, 2, 4 }, new int[] { 3 })]
        [InlineData(new int[] { 1, 2, 1, 1, 1 }, new int[] { 1 }, new int[] { 2 })]
        [InlineData(new int[] { 1, 1, 1, 1 }, new int[] { 1 }, new int[] { })]
        public static void MemoryExtensions_Trim_Multi(int[] values, int[] trims, int[] expected)
        {
            Memory<int> memory = new Memory<int>(values).Trim(trims);
            Assert.True(System.Linq.Enumerable.SequenceEqual(expected, memory.ToArray()));
        }

        private sealed class Foo : IEquatable<Foo>
        {
            public int Value { get; set; }

            public bool Equals(Foo other)
            {
                if (this == null && other == null)
                    return true;
                if (other == null)
                    return false;
                return Value == other.Value;
            }

            public static implicit operator Foo(int value) => new Foo { Value = value };
            public static implicit operator int? (Foo foo) => foo?.Value;
        }

        [Fact]
        public static void MemoryExtensions_TrimStart_Single_Null()
        {
            var values = new Foo[] { null, null, 1, 2, null, null };
            var trim = (Foo)null;

            var expected = new Foo[] { 1, 2, null, null };

            Memory<Foo> memory = new Memory<Foo>(values).TrimStart(trim);
            Assert.True(System.Linq.Enumerable.SequenceEqual(expected, memory.ToArray()));
        }

        [Fact]
        public static void MemoryExtensions_TrimStart_Multi_Null()
        {
            var values = new Foo[] { null, 1, 2, 3, null, 2, 1, null };
            var trim = new Foo[] { null, 1, 2 };

            var expected = new Foo[] { 3, null, 2, 1, null };

            Memory<Foo> memory = new Memory<Foo>(values).TrimStart(trim);
            Assert.True(System.Linq.Enumerable.SequenceEqual(expected, memory.ToArray()));
        }

        [Fact]
        public static void MemoryExtensions_TrimEnd_Single_Null()
        {
            var values = new Foo[] { null, null, 1, 2, null, null };
            var trim = (Foo)null;

            var expected = new Foo[] { null, null, 1, 2 };

            Memory<Foo> memory = new Memory<Foo>(values).TrimEnd(trim);
            Assert.True(System.Linq.Enumerable.SequenceEqual(expected, memory.ToArray()));
        }

        [Fact]
        public static void MemoryExtensions_TrimEnd_Multi_Null()
        {
            var values = new Foo[] { null, 1, 2, 3, null, 2, 1, null };
            var trim = new Foo[] { null, 1, 2 };

            var expected = new Foo[] { null, 1, 2, 3 };

            Memory<Foo> memory = new Memory<Foo>(values).TrimEnd(trim);
            Assert.True(System.Linq.Enumerable.SequenceEqual(expected, memory.ToArray()));
        }

        [Fact]
        public static void MemoryExtensions_Trim_Single_Null()
        {
            var values = new Foo[] { null, null, 1, 2, null, null };
            var trim = (Foo)null;

            var expected = new Foo[] { 1, 2 };

            Memory<Foo> memory = new Memory<Foo>(values).Trim(trim);
            Assert.True(System.Linq.Enumerable.SequenceEqual(expected, memory.ToArray()));
        }

        [Fact]
        public static void MemoryExtensions_Trim_Multi_Null()
        {
            var values = new Foo[] { null, 1, 2, 3, null, 2, 1, null };
            var trim = new Foo[] { null, 1, 2 };

            var expected = new Foo[] { 3 };

            Memory<Foo> memory = new Memory<Foo>(values).Trim(trim);
            Assert.True(System.Linq.Enumerable.SequenceEqual(expected, memory.ToArray()));
        }
    }

    public static partial class ReadOnlyMemoryTests
    {
        [Theory]
        [InlineData(new int[] { 1 }, 1, new int[] { })]
        [InlineData(new int[] { 2 }, 1, new int[] { 2 })]
        [InlineData(new int[] { 1, 2, 1 }, 1, new int[] { 2, 1 })]
        [InlineData(new int[] { 1, 1, 2, 1 }, 1, new int[] { 2, 1 })]
        [InlineData(new int[] { 1, 1, 2, 1 }, 2, new int[] { 1, 1, 2, 1 })]
        [InlineData(new int[] { 1, 1, 2, 1 }, 3, new int[] { 1, 1, 2, 1 })]
        [InlineData(new int[] { 1, 1, 1, 2 }, 1, new int[] { 2 })]
        [InlineData(new int[] { 1, 1, 1, 1 }, 1, new int[] { })]
        public static void MemoryExtensions_TrimStart_Single(int[] values, int trim, int[] expected)
        {
            ReadOnlyMemory<int> rom = new ReadOnlyMemory<int>(values).TrimStart(trim);
            Assert.True(System.Linq.Enumerable.SequenceEqual(expected, rom.ToArray()));
        }

        [Theory]
        [InlineData(new int[] { 1 }, 1, new int[] { })]
        [InlineData(new int[] { 2 }, 1, new int[] { 2 })]
        [InlineData(new int[] { 1, 2, 1 }, 1, new int[] { 1, 2 })]
        [InlineData(new int[] { 1, 2, 1, 1 }, 1, new int[] { 1, 2 })]
        [InlineData(new int[] { 1, 2, 1, 1 }, 2, new int[] { 1, 2, 1, 1 })]
        [InlineData(new int[] { 1, 2, 1, 1 }, 3, new int[] { 1, 2, 1, 1 })]
        [InlineData(new int[] { 2, 1, 1, 1 }, 1, new int[] { 2 })]
        [InlineData(new int[] { 1, 1, 1, 1 }, 1, new int[] { })]
        public static void MemoryExtensions_TrimEnd_Single(int[] values, int trim, int[] expected)
        {
            ReadOnlyMemory<int> rom = new ReadOnlyMemory<int>(values).TrimEnd(trim);
            Assert.True(System.Linq.Enumerable.SequenceEqual(expected, rom.ToArray()));
        }

        [Theory]
        [InlineData(new int[] { 1 }, 1, new int[] { })]
        [InlineData(new int[] { 2 }, 1, new int[] { 2 })]
        [InlineData(new int[] { 1, 2, 1 }, 1, new int[] { 2 })]
        [InlineData(new int[] { 1, 2, 1, 1 }, 1, new int[] { 2 })]
        [InlineData(new int[] { 1, 2, 1, 1 }, 2, new int[] { 1, 2, 1, 1 })]
        [InlineData(new int[] { 1, 2, 1, 1 }, 3, new int[] { 1, 2, 1, 1 })]
        [InlineData(new int[] { 2, 1, 1, 1 }, 1, new int[] { 2 })]
        [InlineData(new int[] { 1, 1, 1, 2 }, 1, new int[] { 2 })]
        [InlineData(new int[] { 1, 1, 1, 1 }, 1, new int[] { })]
        public static void MemoryExtensions_Trim_Single(int[] values, int trim, int[] expected)
        {
            ReadOnlyMemory<int> rom = new ReadOnlyMemory<int>(values).Trim(trim);
            Assert.True(System.Linq.Enumerable.SequenceEqual(expected, rom.ToArray()));
        }

        [Theory]
        [InlineData(new int[] { 1 }, new int[] { 1 }, new int[] { })]
        [InlineData(new int[] { 2 }, new int[] { 1 }, new int[] { 2 })]
        [InlineData(new int[] { 1, 2, 1 }, new int[] { 1 }, new int[] { 2, 1 })]
        [InlineData(new int[] { 1, 1, 2, 1 }, new int[] { 1 }, new int[] { 2, 1 })]
        [InlineData(new int[] { 1, 1, 2, 1 }, new int[] { 2 }, new int[] { 1, 1, 2, 1 })]
        [InlineData(new int[] { 1, 1, 2, 1 }, new int[] { 3 }, new int[] { 1, 1, 2, 1 })]
        [InlineData(new int[] { 1, 1, 2, 1 }, new int[] { 1, 2 }, new int[] { })]
        [InlineData(new int[] { 1, 1, 2, 3 }, new int[] { 1, 2 }, new int[] { 3 })]
        [InlineData(new int[] { 1, 1, 2, 3 }, new int[] { 1, 2, 4 }, new int[] { 3 })]
        [InlineData(new int[] { 1, 1, 1, 2 }, new int[] { 1 }, new int[] { 2 })]
        [InlineData(new int[] { 1, 1, 1, 1 }, new int[] { 1 }, new int[] { })]
        public static void MemoryExtensions_TrimStart_Multi(int[] values, int[] trims, int[] expected)
        {
            ReadOnlyMemory<int> rom = new ReadOnlyMemory<int>(values).TrimStart(trims);
            Assert.True(System.Linq.Enumerable.SequenceEqual(expected, rom.ToArray()));
        }

        [Theory]
        [InlineData(new int[] { 1 }, new int[] { 1 }, new int[] { })]
        [InlineData(new int[] { 2 }, new int[] { 1 }, new int[] { 2 })]
        [InlineData(new int[] { 1, 2, 1 }, new int[] { 1 }, new int[] { 1, 2 })]
        [InlineData(new int[] { 1, 2, 1, 1 }, new int[] { 1 }, new int[] { 1, 2 })]
        [InlineData(new int[] { 1, 2, 1, 1 }, new int[] { 2 }, new int[] { 1, 2, 1, 1 })]
        [InlineData(new int[] { 1, 2, 1, 1 }, new int[] { 3 }, new int[] { 1, 2, 1, 1 })]
        [InlineData(new int[] { 1, 2, 1, 1 }, new int[] { 1, 2 }, new int[] { })]
        [InlineData(new int[] { 3, 2, 1, 1 }, new int[] { 1, 2 }, new int[] { 3 })]
        [InlineData(new int[] { 3, 2, 1, 1 }, new int[] { 1, 2, 4 }, new int[] { 3 })]
        [InlineData(new int[] { 2, 1, 1, 1 }, new int[] { 1 }, new int[] { 2 })]
        [InlineData(new int[] { 1, 1, 1, 1 }, new int[] { 1 }, new int[] { })]
        public static void MemoryExtensions_TrimEnd_Multi(int[] values, int[] trims, int[] expected)
        {
            ReadOnlyMemory<int> rom = new ReadOnlyMemory<int>(values).TrimEnd(trims);
            Assert.True(System.Linq.Enumerable.SequenceEqual(expected, rom.ToArray()));
        }

        [Theory]
        [InlineData(new int[] { 1 }, new int[] { 1 }, new int[] { })]
        [InlineData(new int[] { 2 }, new int[] { 1 }, new int[] { 2 })]
        [InlineData(new int[] { 1, 2, 1 }, new int[] { 1 }, new int[] { 2 })]
        [InlineData(new int[] { 1, 2, 1, 1 }, new int[] { 1 }, new int[] { 2 })]
        [InlineData(new int[] { 1, 2, 1, 1 }, new int[] { 2 }, new int[] { 1, 2, 1, 1 })]
        [InlineData(new int[] { 1, 2, 1, 1 }, new int[] { 3 }, new int[] { 1, 2, 1, 1 })]
        [InlineData(new int[] { 1, 2, 1, 1 }, new int[] { 1, 2 }, new int[] { })]
        [InlineData(new int[] { 2, 1, 3, 2, 1, 1 }, new int[] { 1, 2 }, new int[] { 3 })]
        [InlineData(new int[] { 2, 1, 3, 2, 1, 1 }, new int[] { 1, 2, 4 }, new int[] { 3 })]
        [InlineData(new int[] { 1, 2, 1, 1, 1 }, new int[] { 1 }, new int[] { 2 })]
        [InlineData(new int[] { 1, 1, 1, 1 }, new int[] { 1 }, new int[] { })]
        public static void MemoryExtensions_Trim_Multi(int[] values, int[] trims, int[] expected)
        {
            ReadOnlyMemory<int> rom = new ReadOnlyMemory<int>(values).Trim(trims);
            Assert.True(System.Linq.Enumerable.SequenceEqual(expected, rom.ToArray()));
        }

        private sealed class Foo : IEquatable<Foo>
        {
            public int Value { get; set; }

            public bool Equals(Foo other)
            {
                if (this == null && other == null)
                    return true;
                if (other == null)
                    return false;
                return Value == other.Value;
            }

            public static implicit operator Foo(int value) => new Foo { Value = value };
            public static implicit operator int? (Foo foo) => foo?.Value;
        }

        [Fact]
        public static void MemoryExtensions_TrimStart_Single_Null()
        {
            var values = new Foo[] { null, null, 1, 2, null, null };
            var trim = (Foo)null;

            var expected = new Foo[] { 1, 2, null, null };

            ReadOnlyMemory<Foo> rom = new ReadOnlyMemory<Foo>(values).TrimStart(trim);
            Assert.True(System.Linq.Enumerable.SequenceEqual(expected, rom.ToArray()));
        }

        [Fact]
        public static void MemoryExtensions_TrimStart_Multi_Null()
        {
            var values = new Foo[] { null, 1, 2, 3, null, 2, 1, null };
            var trim = new Foo[] { null, 1, 2 };

            var expected = new Foo[] { 3, null, 2, 1, null };

            ReadOnlyMemory<Foo> rom = new ReadOnlyMemory<Foo>(values).TrimStart(trim);
            Assert.True(System.Linq.Enumerable.SequenceEqual(expected, rom.ToArray()));
        }

        [Fact]
        public static void MemoryExtensions_TrimEnd_Single_Null()
        {
            var values = new Foo[] { null, null, 1, 2, null, null };
            var trim = (Foo)null;

            var expected = new Foo[] { null, null, 1, 2 };

            ReadOnlyMemory<Foo> rom = new ReadOnlyMemory<Foo>(values).TrimEnd(trim);
            Assert.True(System.Linq.Enumerable.SequenceEqual(expected, rom.ToArray()));
        }

        [Fact]
        public static void MemoryExtensions_TrimEnd_Multi_Null()
        {
            var values = new Foo[] { null, 1, 2, 3, null, 2, 1, null };
            var trim = new Foo[] { null, 1, 2 };

            var expected = new Foo[] { null, 1, 2, 3 };

            ReadOnlyMemory<Foo> rom = new ReadOnlyMemory<Foo>(values).TrimEnd(trim);
            Assert.True(System.Linq.Enumerable.SequenceEqual(expected, rom.ToArray()));
        }

        [Fact]
        public static void MemoryExtensions_Trim_Single_Null()
        {
            var values = new Foo[] { null, null, 1, 2, null, null };
            var trim = (Foo)null;

            var expected = new Foo[] { 1, 2 };

            ReadOnlyMemory<Foo> rom = new ReadOnlyMemory<Foo>(values).Trim(trim);
            Assert.True(System.Linq.Enumerable.SequenceEqual(expected, rom.ToArray()));
        }

        [Fact]
        public static void MemoryExtensions_Trim_Multi_Null()
        {
            var values = new Foo[] { null, 1, 2, 3, null, 2, 1, null };
            var trim = new Foo[] { null, 1, 2 };

            var expected = new Foo[] { 3 };

            ReadOnlyMemory<Foo> rom = new ReadOnlyMemory<Foo>(values).Trim(trim);
            Assert.True(System.Linq.Enumerable.SequenceEqual(expected, rom.ToArray()));
        }
    }

    public static class MemoryExtensionsTests
    {
        [Theory]
        [InlineData(new int[] { 1 }, 1, new int[] { })]
        [InlineData(new int[] { 2 }, 1, new int[] { 2 })]
        [InlineData(new int[] { 1, 2, 1 }, 1, new int[] { 2, 1 })]
        [InlineData(new int[] { 1, 1, 2, 1 }, 1, new int[] { 2, 1 })]
        [InlineData(new int[] { 1, 1, 2, 1 }, 2, new int[] { 1, 1, 2, 1 })]
        [InlineData(new int[] { 1, 1, 2, 1 }, 3, new int[] { 1, 1, 2, 1 })]
        [InlineData(new int[] { 1, 1, 1, 2 }, 1, new int[] { 2 })]
        [InlineData(new int[] { 1, 1, 1, 1 }, 1, new int[] { })]
        public static void MemoryExtensions_TrimStart_Single(int[] values, int trim, int[] expected)
        {
            // Memory
            Memory<int> memory = MemoryExtensions.TrimStart(new Memory<int>(values), trim);
            Assert.True(System.Linq.Enumerable.SequenceEqual(expected, memory.ToArray()));

            // RoM
            ReadOnlyMemory<int> rom = MemoryExtensions.TrimStart(new ReadOnlyMemory<int>(values), trim);
            Assert.True(System.Linq.Enumerable.SequenceEqual(expected, rom.ToArray()));

            // Span
            Span<int> span = MemoryExtensions.TrimStart(new Span<int>(values), trim);
            Assert.True(System.Linq.Enumerable.SequenceEqual(expected, span.ToArray()));

            // RoS
            ReadOnlySpan<int> ros = MemoryExtensions.TrimStart(new ReadOnlySpan<int>(values), trim);
            Assert.True(System.Linq.Enumerable.SequenceEqual(expected, ros.ToArray()));
        }

        [Theory]
        [InlineData(new int[] { 1 }, 1, new int[] { })]
        [InlineData(new int[] { 2 }, 1, new int[] { 2 })]
        [InlineData(new int[] { 1, 2, 1 }, 1, new int[] { 1, 2 })]
        [InlineData(new int[] { 1, 2, 1, 1 }, 1, new int[] { 1, 2 })]
        [InlineData(new int[] { 1, 2, 1, 1 }, 2, new int[] { 1, 2, 1, 1 })]
        [InlineData(new int[] { 1, 2, 1, 1 }, 3, new int[] { 1, 2, 1, 1 })]
        [InlineData(new int[] { 2, 1, 1, 1 }, 1, new int[] { 2 })]
        [InlineData(new int[] { 1, 1, 1, 1 }, 1, new int[] { })]
        public static void MemoryExtensions_TrimEnd_Single(int[] values, int trim, int[] expected)
        {
            // Memory
            Memory<int> memory = MemoryExtensions.TrimEnd(new Memory<int>(values), trim);
            Assert.True(System.Linq.Enumerable.SequenceEqual(expected, memory.ToArray()));

            // RoM
            ReadOnlyMemory<int> rom = MemoryExtensions.TrimEnd(new ReadOnlyMemory<int>(values), trim);
            Assert.True(System.Linq.Enumerable.SequenceEqual(expected, rom.ToArray()));

            // Span
            Span<int> span = MemoryExtensions.TrimEnd(new Span<int>(values), trim);
            Assert.True(System.Linq.Enumerable.SequenceEqual(expected, span.ToArray()));

            // RoS
            ReadOnlySpan<int> ros = MemoryExtensions.TrimEnd(new ReadOnlySpan<int>(values), trim);
            Assert.True(System.Linq.Enumerable.SequenceEqual(expected, ros.ToArray()));
        }

        [Theory]
        [InlineData(new int[] { 1 }, 1, new int[] { })]
        [InlineData(new int[] { 2 }, 1, new int[] { 2 })]
        [InlineData(new int[] { 1, 2, 1 }, 1, new int[] { 2 })]
        [InlineData(new int[] { 1, 2, 1, 1 }, 1, new int[] { 2 })]
        [InlineData(new int[] { 1, 2, 1, 1 }, 2, new int[] { 1, 2, 1, 1 })]
        [InlineData(new int[] { 1, 2, 1, 1 }, 3, new int[] { 1, 2, 1, 1 })]
        [InlineData(new int[] { 2, 1, 1, 1 }, 1, new int[] { 2 })]
        [InlineData(new int[] { 1, 1, 1, 2 }, 1, new int[] { 2 })]
        [InlineData(new int[] { 1, 1, 1, 1 }, 1, new int[] { })]
        public static void MemoryExtensions_Trim_Single(int[] values, int trim, int[] expected)
        {
            // Memory
            Memory<int> memory = MemoryExtensions.Trim(new Memory<int>(values), trim);
            Assert.True(System.Linq.Enumerable.SequenceEqual(expected, memory.ToArray()));

            // RoM
            ReadOnlyMemory<int> rom = MemoryExtensions.Trim(new ReadOnlyMemory<int>(values), trim);
            Assert.True(System.Linq.Enumerable.SequenceEqual(expected, rom.ToArray()));

            // Span
            Span<int> span = MemoryExtensions.Trim(new Span<int>(values), trim);
            Assert.True(System.Linq.Enumerable.SequenceEqual(expected, span.ToArray()));

            // RoS
            ReadOnlySpan<int> ros = MemoryExtensions.Trim(new ReadOnlySpan<int>(values), trim);
            Assert.True(System.Linq.Enumerable.SequenceEqual(expected, ros.ToArray()));
        }

        [Theory]
        [InlineData(new int[] { 1 }, new int[] { 1 }, new int[] { })]
        [InlineData(new int[] { 2 }, new int[] { 1 }, new int[] { 2 })]
        [InlineData(new int[] { 1, 2, 1 }, new int[] { 1 }, new int[] { 2, 1 })]
        [InlineData(new int[] { 1, 1, 2, 1 }, new int[] { 1 }, new int[] { 2, 1 })]
        [InlineData(new int[] { 1, 1, 2, 1 }, new int[] { 2 }, new int[] { 1, 1, 2, 1 })]
        [InlineData(new int[] { 1, 1, 2, 1 }, new int[] { 3 }, new int[] { 1, 1, 2, 1 })]
        [InlineData(new int[] { 1, 1, 2, 1 }, new int[] { 1, 2 }, new int[] { })]
        [InlineData(new int[] { 1, 1, 2, 3 }, new int[] { 1, 2 }, new int[] { 3 })]
        [InlineData(new int[] { 1, 1, 2, 3 }, new int[] { 1, 2, 4 }, new int[] { 3 })]
        [InlineData(new int[] { 1, 1, 1, 2 }, new int[] { 1 }, new int[] { 2 })]
        [InlineData(new int[] { 1, 1, 1, 1 }, new int[] { 1 }, new int[] { })]
        public static void MemoryExtensions_TrimStart_Multi(int[] values, int[] trims, int[] expected)
        {
            // Memory
            Memory<int> memory = MemoryExtensions.TrimStart(new Memory<int>(values), trims);
            Assert.True(System.Linq.Enumerable.SequenceEqual(expected, memory.ToArray()));

            // RoM
            ReadOnlyMemory<int> rom = MemoryExtensions.TrimStart(new ReadOnlyMemory<int>(values), trims);
            Assert.True(System.Linq.Enumerable.SequenceEqual(expected, rom.ToArray()));

            // Span
            Span<int> span = MemoryExtensions.TrimStart(new Span<int>(values), trims);
            Assert.True(System.Linq.Enumerable.SequenceEqual(expected, span.ToArray()));

            // RoS
            ReadOnlySpan<int> ros = MemoryExtensions.TrimStart(new ReadOnlySpan<int>(values), trims);
            Assert.True(System.Linq.Enumerable.SequenceEqual(expected, ros.ToArray()));
        }

        [Theory]
        [InlineData(new int[] { 1 }, new int[] { 1 }, new int[] { })]
        [InlineData(new int[] { 2 }, new int[] { 1 }, new int[] { 2 })]
        [InlineData(new int[] { 1, 2, 1 }, new int[] { 1 }, new int[] { 1, 2 })]
        [InlineData(new int[] { 1, 2, 1, 1 }, new int[] { 1 }, new int[] { 1, 2 })]
        [InlineData(new int[] { 1, 2, 1, 1 }, new int[] { 2 }, new int[] { 1, 2, 1, 1 })]
        [InlineData(new int[] { 1, 2, 1, 1 }, new int[] { 3 }, new int[] { 1, 2, 1, 1 })]
        [InlineData(new int[] { 1, 2, 1, 1 }, new int[] { 1, 2 }, new int[] { })]
        [InlineData(new int[] { 3, 2, 1, 1 }, new int[] { 1, 2 }, new int[] { 3 })]
        [InlineData(new int[] { 3, 2, 1, 1 }, new int[] { 1, 2, 4 }, new int[] { 3 })]
        [InlineData(new int[] { 2, 1, 1, 1 }, new int[] { 1 }, new int[] { 2 })]
        [InlineData(new int[] { 1, 1, 1, 1 }, new int[] { 1 }, new int[] { })]
        public static void MemoryExtensions_TrimEnd_Multi(int[] values, int[] trims, int[] expected)
        {
            // Memory
            Memory<int> memory = MemoryExtensions.TrimEnd(new Memory<int>(values), trims);
            Assert.True(System.Linq.Enumerable.SequenceEqual(expected, memory.ToArray()));

            // RoM
            ReadOnlyMemory<int> rom = MemoryExtensions.TrimEnd(new ReadOnlyMemory<int>(values), trims);
            Assert.True(System.Linq.Enumerable.SequenceEqual(expected, rom.ToArray()));

            // Span
            Span<int> span = MemoryExtensions.TrimEnd(new Span<int>(values), trims);
            Assert.True(System.Linq.Enumerable.SequenceEqual(expected, span.ToArray()));

            // RoS
            ReadOnlySpan<int> ros = MemoryExtensions.TrimEnd(new ReadOnlySpan<int>(values), trims);
            Assert.True(System.Linq.Enumerable.SequenceEqual(expected, ros.ToArray()));
        }

        [Theory]
        [InlineData(new int[] { 1 }, new int[] { 1 }, new int[] { })]
        [InlineData(new int[] { 2 }, new int[] { 1 }, new int[] { 2 })]
        [InlineData(new int[] { 1, 2, 1 }, new int[] { 1 }, new int[] { 2 })]
        [InlineData(new int[] { 1, 2, 1, 1 }, new int[] { 1 }, new int[] { 2 })]
        [InlineData(new int[] { 1, 2, 1, 1 }, new int[] { 2 }, new int[] { 1, 2, 1, 1 })]
        [InlineData(new int[] { 1, 2, 1, 1 }, new int[] { 3 }, new int[] { 1, 2, 1, 1 })]
        [InlineData(new int[] { 1, 2, 1, 1 }, new int[] { 1, 2 }, new int[] { })]
        [InlineData(new int[] { 2, 1, 3, 2, 1, 1 }, new int[] { 1, 2 }, new int[] { 3 })]
        [InlineData(new int[] { 2, 1, 3, 2, 1, 1 }, new int[] { 1, 2, 4 }, new int[] { 3 })]
        [InlineData(new int[] { 1, 2, 1, 1, 1 }, new int[] { 1 }, new int[] { 2 })]
        [InlineData(new int[] { 1, 1, 1, 1 }, new int[] { 1 }, new int[] { })]
        public static void MemoryExtensions_Trim_Multi(int[] values, int[] trims, int[] expected)
        {
            // Memory
            Memory<int> memory = MemoryExtensions.Trim(new Memory<int>(values), trims);
            Assert.True(System.Linq.Enumerable.SequenceEqual(expected, memory.ToArray()));

            // RoM
            ReadOnlyMemory<int> rom = MemoryExtensions.Trim(new ReadOnlyMemory<int>(values), trims);
            Assert.True(System.Linq.Enumerable.SequenceEqual(expected, rom.ToArray()));

            // Span
            Span<int> span = MemoryExtensions.Trim(new Span<int>(values), trims);
            Assert.True(System.Linq.Enumerable.SequenceEqual(expected, span.ToArray()));

            // RoS
            ReadOnlySpan<int> ros = MemoryExtensions.Trim(new ReadOnlySpan<int>(values), trims);
            Assert.True(System.Linq.Enumerable.SequenceEqual(expected, ros.ToArray()));
        }

        private sealed class Foo : IEquatable<Foo>
        {
            public int Value { get; set; }

            public bool Equals(Foo other)
            {
                if (this == null && other == null) return true;
                if (other == null) return false;
                return Value == other.Value;
            }

            public static implicit operator Foo(int value) => new Foo { Value = value };
            public static implicit operator int? (Foo foo) => foo?.Value;
        }

        [Fact]
        public static void MemoryExtensions_TrimStart_Single_Null()
        {
            var values = new Foo[] { null, null, 1, 2, null, null };
            var trim = (Foo)null;

            var expected = new Foo[] { 1, 2, null, null };

            // Memory
            Memory<Foo> memory = MemoryExtensions.TrimStart(new Memory<Foo>(values), trim);
            Assert.True(System.Linq.Enumerable.SequenceEqual(expected, memory.ToArray()));

            // RoM
            ReadOnlyMemory<Foo> rom = MemoryExtensions.TrimStart(new ReadOnlyMemory<Foo>(values), trim);
            Assert.True(System.Linq.Enumerable.SequenceEqual(expected, rom.ToArray()));

            // Span
            Span<Foo> span = MemoryExtensions.TrimStart(new Span<Foo>(values), trim);
            Assert.True(System.Linq.Enumerable.SequenceEqual(expected, span.ToArray()));

            // RoS
            ReadOnlySpan<Foo> ros = MemoryExtensions.TrimStart(new ReadOnlySpan<Foo>(values), trim);
            Assert.True(System.Linq.Enumerable.SequenceEqual(expected, ros.ToArray()));
        }

        [Fact]
        public static void MemoryExtensions_TrimStart_Multi_Null()
        {
            var values = new Foo[] { null, 1, 2, 3, null, 2, 1, null };
            var trim = new Foo[] { null, 1, 2 };

            var expected = new Foo[] { 3, null, 2, 1, null };

            // Memory
            Memory<Foo> memory = MemoryExtensions.TrimStart(new Memory<Foo>(values), trim);
            Assert.True(System.Linq.Enumerable.SequenceEqual(expected, memory.ToArray()));

            // RoM
            ReadOnlyMemory<Foo> rom = MemoryExtensions.TrimStart(new ReadOnlyMemory<Foo>(values), trim);
            Assert.True(System.Linq.Enumerable.SequenceEqual(expected, rom.ToArray()));

            // Span
            Span<Foo> span = MemoryExtensions.TrimStart(new Span<Foo>(values), trim);
            Assert.True(System.Linq.Enumerable.SequenceEqual(expected, span.ToArray()));

            // RoS
            ReadOnlySpan<Foo> ros = MemoryExtensions.TrimStart(new ReadOnlySpan<Foo>(values), trim);
            Assert.True(System.Linq.Enumerable.SequenceEqual(expected, ros.ToArray()));
        }

        [Fact]
        public static void MemoryExtensions_TrimEnd_Single_Null()
        {
            var values = new Foo[] { null, null, 1, 2, null, null };
            var trim = (Foo)null;
            var expected = new Foo[] { null, null, 1, 2 };

            // Memory
            Memory<Foo> memory = MemoryExtensions.TrimEnd(new Memory<Foo>(values), trim);
            Assert.True(System.Linq.Enumerable.SequenceEqual(expected, memory.ToArray()));

            // RoM
            ReadOnlyMemory<Foo> rom = MemoryExtensions.TrimEnd(new ReadOnlyMemory<Foo>(values), trim);
            Assert.True(System.Linq.Enumerable.SequenceEqual(expected, rom.ToArray()));

            // Span
            Span<Foo> span = MemoryExtensions.TrimEnd(new Span<Foo>(values), trim);
            Assert.True(System.Linq.Enumerable.SequenceEqual(expected, span.ToArray()));

            // RoS
            ReadOnlySpan<Foo> ros = MemoryExtensions.TrimEnd(new ReadOnlySpan<Foo>(values), trim);
            Assert.True(System.Linq.Enumerable.SequenceEqual(expected, ros.ToArray()));
        }

        [Fact]
        public static void MemoryExtensions_TrimEnd_Multi_Null()
        {
            var values = new Foo[] { null, 1, 2, 3, null, 2, 1, null };
            var trim = new Foo[] { null, 1, 2 };
            var expected = new Foo[] { null, 1, 2, 3 };

            // Memory
            Memory<Foo> memory = MemoryExtensions.TrimEnd(new Memory<Foo>(values), trim);
            Assert.True(System.Linq.Enumerable.SequenceEqual(expected, memory.ToArray()));

            // RoM
            ReadOnlyMemory<Foo> rom = MemoryExtensions.TrimEnd(new ReadOnlyMemory<Foo>(values), trim);
            Assert.True(System.Linq.Enumerable.SequenceEqual(expected, rom.ToArray()));

            // Span
            Span<Foo> span = MemoryExtensions.TrimEnd(new Span<Foo>(values), trim);
            Assert.True(System.Linq.Enumerable.SequenceEqual(expected, span.ToArray()));

            // RoS
            ReadOnlySpan<Foo> ros = MemoryExtensions.TrimEnd(new ReadOnlySpan<Foo>(values), trim);
            Assert.True(System.Linq.Enumerable.SequenceEqual(expected, ros.ToArray()));
        }

        [Fact]
        public static void MemoryExtensions_Trim_Single_Null()
        {
            var values = new Foo[] { null, null, 1, 2, null, null };
            var trim = (Foo)null;
            var expected = new Foo[] { 1, 2 };

            // Memory
            Memory<Foo> memory = MemoryExtensions.Trim(new Memory<Foo>(values), trim);
            Assert.True(System.Linq.Enumerable.SequenceEqual(expected, memory.ToArray()));

            // RoM
            ReadOnlyMemory<Foo> rom = MemoryExtensions.Trim(new ReadOnlyMemory<Foo>(values), trim);
            Assert.True(System.Linq.Enumerable.SequenceEqual(expected, rom.ToArray()));

            // Span
            Span<Foo> span = MemoryExtensions.Trim(new Span<Foo>(values), trim);
            Assert.True(System.Linq.Enumerable.SequenceEqual(expected, span.ToArray()));

            // RoS
            ReadOnlySpan<Foo> ros = MemoryExtensions.Trim(new ReadOnlySpan<Foo>(values), trim);
            Assert.True(System.Linq.Enumerable.SequenceEqual(expected, ros.ToArray()));
        }

        [Fact]
        public static void MemoryExtensions_Trim_Multi_Null()
        {
            var values = new Foo[] { null, 1, 2, 3, null, 2, 1, null };
            var trim = new Foo[] { null, 1, 2 };
            var expected = new Foo[] { 3 };

            // Memory
            Memory<Foo> memory = MemoryExtensions.Trim(new Memory<Foo>(values), trim);
            Assert.True(System.Linq.Enumerable.SequenceEqual(expected, memory.ToArray()));

            // RoM
            ReadOnlyMemory<Foo> rom = MemoryExtensions.Trim(new ReadOnlyMemory<Foo>(values), trim);
            Assert.True(System.Linq.Enumerable.SequenceEqual(expected, rom.ToArray()));

            // Span
            Span<Foo> span = MemoryExtensions.Trim(new Span<Foo>(values), trim);
            Assert.True(System.Linq.Enumerable.SequenceEqual(expected, span.ToArray()));

            // RoS
            ReadOnlySpan<Foo> ros = MemoryExtensions.Trim(new ReadOnlySpan<Foo>(values), trim);
            Assert.True(System.Linq.Enumerable.SequenceEqual(expected, ros.ToArray()));
        }
    }

    /// <summary>
    /// Extension methods for Span{T}, Memory{T}, and friends.
    /// </summary>
    public static partial class MemoryExtensions
    {
        /// <summary>
        /// Removes all leading and trailing occurrences of a specified element.
        /// </summary>
        /// <param name="trimElement">The specified element to look for and remove.</param>
        public static Memory<T> Trim<T>(this Memory<T> memory, T trimElement)
            where T : IEquatable<T>
        {
            Span<T> span = memory.Span;
            int start = ClampStart(span, trimElement);
            int length = ClampEnd(span, start, trimElement);
            return memory.Slice(start, length);
        }

        /// <summary>
        /// Removes all leading occurrences of a specified element.
        /// </summary>
        /// <param name="trimElement">The specified element to look for and remove.</param>
        public static Memory<T> TrimStart<T>(this Memory<T> memory, T trimElement)
            where T : IEquatable<T>
        {
            int start = ClampStart(memory.Span, trimElement);
            return memory.Slice(start);
        }

        /// <summary>
        /// Removes all trailing occurrences of a specified element.
        /// </summary>
        /// <param name="trimElement">The specified element to look for and remove.</param>
        public static Memory<T> TrimEnd<T>(this Memory<T> memory, T trimElement)
            where T : IEquatable<T>
        {
            int length = ClampEnd(memory.Span, 0, trimElement);
            return memory.Slice(0, length);
        }

        /// <summary>
        /// Removes all leading and trailing occurrences of a specified element.
        /// </summary>
        /// <param name="trimElement">The specified element to look for and remove.</param>
        public static ReadOnlyMemory<T> Trim<T>(this ReadOnlyMemory<T> memory, T trimElement)
            where T : IEquatable<T>
        {
            ReadOnlySpan<T> span = memory.Span;
            int start = ClampStart(span, trimElement);
            int length = ClampEnd(span, start, trimElement);
            return memory.Slice(start, length);
        }

        /// <summary>
        /// Removes all leading occurrences of a specified element.
        /// </summary>
        /// <param name="trimElement">The specified element to look for and remove.</param>
        public static ReadOnlyMemory<T> TrimStart<T>(this ReadOnlyMemory<T> memory, T trimElement)
            where T : IEquatable<T>
        {
            int start = ClampStart(memory.Span, trimElement);
            return memory.Slice(start);
        }

        /// <summary>
        /// Removes all trailing occurrences of a specified element.
        /// </summary>
        /// <param name="trimElement">The specified element to look for and remove.</param>
        public static ReadOnlyMemory<T> TrimEnd<T>(this ReadOnlyMemory<T> memory, T trimElement)
            where T : IEquatable<T>
        {
            int length = ClampEnd(memory.Span, 0, trimElement);
            return memory.Slice(0, length);
        }

        /// <summary>
        /// Removes all leading and trailing occurrences of a specified element.
        /// </summary>
        /// <param name="trimElement">The specified element to look for and remove.</param>
        public static Span<T> Trim<T>(this Span<T> span, T trimElement)
            where T : IEquatable<T>
        {
            int start = ClampStart(span, trimElement);
            int length = ClampEnd(span, start, trimElement);
            return span.Slice(start, length);
        }

        /// <summary>
        /// Removes all leading occurrences of a specified element.
        /// </summary>
        /// <param name="trimElement">The specified element to look for and remove.</param>
        public static Span<T> TrimStart<T>(this Span<T> span, T trimElement)
            where T : IEquatable<T>
        {
            int start = ClampStart(span, trimElement);
            return span.Slice(start);
        }

        /// <summary>
        /// Removes all trailing occurrences of a specified element.
        /// </summary>
        /// <param name="trimElement">The specified element to look for and remove.</param>
        public static Span<T> TrimEnd<T>(this Span<T> span, T trimElement)
            where T : IEquatable<T>
        {
            int length = ClampEnd(span, 0, trimElement);
            return span.Slice(0, length);
        }

        /// <summary>
        /// Removes all leading and trailing occurrences of a specified element.
        /// </summary>
        /// <param name="trimElement">The specified element to look for and remove.</param>
        public static ReadOnlySpan<T> Trim<T>(this ReadOnlySpan<T> span, T trimElement)
            where T : IEquatable<T>
        {
            int start = ClampStart(span, trimElement);
            int length = ClampEnd(span, start, trimElement);
            return span.Slice(start, length);
        }

        /// <summary>
        /// Removes all leading occurrences of a specified element.
        /// </summary>
        /// <param name="trimElement">The specified element to look for and remove.</param>
        public static ReadOnlySpan<T> TrimStart<T>(this ReadOnlySpan<T> span, T trimElement)
            where T : IEquatable<T>
        {
            int start = ClampStart(span, trimElement);
            return span.Slice(start);
        }

        /// <summary>
        /// Removes all trailing occurrences of a specified element.
        /// </summary>
        /// <param name="trimElement">The specified element to look for and remove.</param>
        public static ReadOnlySpan<T> TrimEnd<T>(this ReadOnlySpan<T> span, T trimElement)
            where T : IEquatable<T>
        {
            int length = ClampEnd(span, 0, trimElement);
            return span.Slice(0, length);
        }

        /// <summary>
        /// Delimits all leading occurrences of a specified element.
        /// </summary>
        /// <param name="span">The source span from which the element is removed.</param>
        /// <param name="trimElement">The specified element to look for and remove.</param>
        private static int ClampStart<T>(ReadOnlySpan<T> span, T trimElement)
            where T : IEquatable<T>
        {
            int start = 0;

            if (trimElement == null)
            {
                for (; start < span.Length; start++)
                {
                    if (span[start] != null)
                        break;
                }
            }
            else
            {
                for (; start < span.Length; start++)
                {
                    if (!trimElement.Equals(span[start]))
                        break;
                }
            }

            return start;
        }

        /// <summary>
        /// Delimits all trailing occurrences of a specified element.
        /// </summary>
        /// <param name="span">The source span from which the element is removed.</param>
        /// <param name="start">The start index from which to being searching.</param>
        /// <param name="trimElement">The specified element to look for and remove.</param>
        private static int ClampEnd<T>(ReadOnlySpan<T> span, int start, T trimElement)
            where T : IEquatable<T>
        {
            // Initially, start==len==0. If ClampStart trims all, start==len
            Debug.Assert((uint)start <= span.Length);

            int end = span.Length - 1;

            if (trimElement == null)
            {
                for (; end >= start; end--)
                {
                    if (span[end] != null)
                        break;
                }
            }
            else
            {
                for (; end >= start; end--)
                {
                    if (!trimElement.Equals(span[end]))
                        break;
                }
            }

            return end - start + 1;
        }

        /// <summary>
        /// Removes all leading and trailing occurrences of a set of elements specified
        /// in a readonly span from the memory.
        /// </summary>
        /// <param name="trimElements">The span which contains the set of elements to remove.</param>
        /// <remarks>If <paramref name="trimElements"/> is empty, the memory is returned unaltered.</remarks>
        public static Memory<T> Trim<T>(this Memory<T> memory, ReadOnlySpan<T> trimElements)
            where T : IEquatable<T>
        {
            if (trimElements.Length <= 1) // Optimize for N > 1
            {
                return trimElements.Length == 0 ? memory : Trim(memory, trimElements[0]);
            }

            Span<T> span = memory.Span;
            int start = ClampStart(span, trimElements);
            int length = ClampEnd(span, start, trimElements);
            return memory.Slice(start, length);
        }

        /// <summary>
        /// Removes all leading occurrences of a set of elements specified
        /// in a readonly span from the memory.
        /// </summary>
        /// <param name="trimElements">The span which contains the set of elements to remove.</param>
        /// <remarks>If <paramref name="trimElements"/> is empty, the memory is returned unaltered.</remarks>
        public static Memory<T> TrimStart<T>(this Memory<T> memory, ReadOnlySpan<T> trimElements)
            where T : IEquatable<T>
        {
            if (trimElements.Length <= 1) // Optimize for N > 1
            {
                return trimElements.Length == 0 ? memory : TrimStart(memory, trimElements[0]);
            }

            int start = ClampStart(memory.Span, trimElements);
            return memory.Slice(start);
        }

        /// <summary>
        /// Removes all trailing occurrences of a set of elements specified
        /// in a readonly span from the memory.
        /// </summary>
        /// <param name="trimElements">The span which contains the set of elements to remove.</param>
        /// <remarks>If <paramref name="trimElements"/> is empty, the memory is returned unaltered.</remarks>
        public static Memory<T> TrimEnd<T>(this Memory<T> memory, ReadOnlySpan<T> trimElements)
            where T : IEquatable<T>
        {
            if (trimElements.Length <= 1) // Optimize for N > 1
            {
                return trimElements.Length == 0 ? memory : TrimEnd(memory, trimElements[0]);
            }

            int length = ClampEnd(memory.Span, 0, trimElements);
            return memory.Slice(0, length);
        }

        /// <summary>
        /// Removes all leading and trailing occurrences of a set of elements specified
        /// in a readonly span from the memory.
        /// </summary>
        /// <param name="trimElements">The span which contains the set of elements to remove.</param>
        /// <remarks>If <paramref name="trimElements"/> is empty, the memory is returned unaltered.</remarks>
        public static ReadOnlyMemory<T> Trim<T>(this ReadOnlyMemory<T> memory, ReadOnlySpan<T> trimElements)
            where T : IEquatable<T>
        {
            if (trimElements.Length <= 1) // Optimize for N > 1
            {
                return trimElements.Length == 0 ? memory : Trim(memory, trimElements[0]);
            }

            ReadOnlySpan<T> span = memory.Span;
            int start = ClampStart(span, trimElements);
            int length = ClampEnd(span, start, trimElements);
            return memory.Slice(start, length);
        }

        /// <summary>
        /// Removes all leading occurrences of a set of elements specified
        /// in a readonly span from the memory.
        /// </summary>
        /// <param name="trimElements">The span which contains the set of elements to remove.</param>
        /// <remarks>If <paramref name="trimElements"/> is empty, the memory is returned unaltered.</remarks>
        public static ReadOnlyMemory<T> TrimStart<T>(this ReadOnlyMemory<T> memory, ReadOnlySpan<T> trimElements)
            where T : IEquatable<T>
        {
            if (trimElements.Length <= 1) // Optimize for N > 1
            {
                return trimElements.Length == 0 ? memory : TrimStart(memory, trimElements[0]);
            }

            int start = ClampStart(memory.Span, trimElements);
            return memory.Slice(start);
        }

        /// <summary>
        /// Removes all trailing occurrences of a set of elements specified
        /// in a readonly span from the memory.
        /// </summary>
        /// <param name="trimElements">The span which contains the set of elements to remove.</param>
        /// <remarks>If <paramref name="trimElements"/> is empty, the memory is returned unaltered.</remarks>
        public static ReadOnlyMemory<T> TrimEnd<T>(this ReadOnlyMemory<T> memory, ReadOnlySpan<T> trimElements)
            where T : IEquatable<T>
        {
            if (trimElements.Length <= 1) // Optimize for N > 1
            {
                return trimElements.Length == 0 ? memory : TrimEnd(memory, trimElements[0]);
            }

            int length = ClampEnd(memory.Span, 0, trimElements);
            return memory.Slice(0, length);
        }

        /// <summary>
        /// Removes all leading and trailing occurrences of a set of elements specified
        /// in a readonly span from the span.
        /// </summary>
        /// <param name="trimElements">The span which contains the set of elements to remove.</param>
        /// <remarks>If <paramref name="trimElements"/> is empty, the span is returned unaltered.</remarks>
        public static Span<T> Trim<T>(this Span<T> span, ReadOnlySpan<T> trimElements)
            where T : IEquatable<T>
        {
            if (trimElements.Length <= 1) // Optimize for N > 1
            {
                return trimElements.Length == 0 ? span : Trim(span, trimElements[0]);
            }

            int start = ClampStart(span, trimElements);
            int length = ClampEnd(span, start, trimElements);
            return span.Slice(start, length);
        }

        /// <summary>
        /// Removes all leading occurrences of a set of elements specified
        /// in a readonly span from the span.
        /// </summary>
        /// <param name="trimElements">The span which contains the set of elements to remove.</param>
        /// <remarks>If <paramref name="trimElements"/> is empty, the span is returned unaltered.</remarks>
        public static Span<T> TrimStart<T>(this Span<T> span, ReadOnlySpan<T> trimElements)
            where T : IEquatable<T>
        {
            if (trimElements.Length <= 1) // Optimize for N > 1
            {
                return trimElements.Length == 0 ? span : TrimStart(span, trimElements[0]);
            }

            int start = ClampStart(span, trimElements);
            return span.Slice(start);
        }

        /// <summary>
        /// Removes all trailing occurrences of a set of elements specified
        /// in a readonly span from the span.
        /// </summary>
        /// <param name="trimElements">The span which contains the set of elements to remove.</param>
        /// <remarks>If <paramref name="trimElements"/> is empty, the span is returned unaltered.</remarks>
        public static Span<T> TrimEnd<T>(this Span<T> span, ReadOnlySpan<T> trimElements)
            where T : IEquatable<T>
        {
            if (trimElements.Length <= 1) // Optimize for N > 1
            {
                return trimElements.Length == 0 ? span : TrimEnd(span, trimElements[0]);
            }

            int length = ClampEnd(span, 0, trimElements);
            return span.Slice(0, length);
        }

        /// <summary>
        /// Removes all leading and trailing occurrences of a set of elements specified
        /// in a readonly span from the span.
        /// </summary>
        /// <param name="trimElements">The span which contains the set of elements to remove.</param>
        /// <remarks>If <paramref name="trimElements"/> is empty, the span is returned unaltered.</remarks>
        public static ReadOnlySpan<T> Trim<T>(this ReadOnlySpan<T> span, ReadOnlySpan<T> trimElements)
            where T : IEquatable<T>
        {
            if (trimElements.Length <= 1) // Optimize for N > 1
            {
                return trimElements.Length == 0 ? span : Trim(span, trimElements[0]);
            }

            int start = ClampStart(span, trimElements);
            int length = ClampEnd(span, start, trimElements);
            return span.Slice(start, length);
        }

        /// <summary>
        /// Removes all leading occurrences of a set of elements specified
        /// in a readonly span from the span.
        /// </summary>
        /// <param name="trimElements">The span which contains the set of elements to remove.</param>
        /// <remarks>If <paramref name="trimElements"/> is empty, the span is returned unaltered.</remarks>
        public static ReadOnlySpan<T> TrimStart<T>(this ReadOnlySpan<T> span, ReadOnlySpan<T> trimElements)
            where T : IEquatable<T>
        {
            if (trimElements.Length <= 1) // Optimize for N > 1
            {
                return trimElements.Length == 0 ? span : TrimStart(span, trimElements[0]);
            }

            int start = ClampStart(span, trimElements);
            return span.Slice(start);
        }

        /// <summary>
        /// Removes all trailing occurrences of a set of elements specified
        /// in a readonly span from the span.
        /// </summary>
        /// <param name="trimElements">The span which contains the set of elements to remove.</param>
        /// <remarks>If <paramref name="trimElements"/> is empty, the span is returned unaltered.</remarks>
        public static ReadOnlySpan<T> TrimEnd<T>(this ReadOnlySpan<T> span, ReadOnlySpan<T> trimElements)
            where T : IEquatable<T>
        {
            if (trimElements.Length <= 1) // Optimize for N > 1
            {
                return trimElements.Length == 0 ? span : TrimEnd(span, trimElements[0]);
            }

            int length = ClampEnd(span, 0, trimElements);
            return span.Slice(0, length);
        }

        /// <summary>
        /// Delimits all leading occurrences of a specified element.
        /// </summary>
        /// <param name="span">The source span from which the element is removed.</param>
        /// <param name="trimElements">The span which contains the set of elements to remove.</param>
        private static int ClampStart<T>(ReadOnlySpan<T> span, ReadOnlySpan<T> trimElements)
            where T : IEquatable<T>
        {
            int start = 0;

            for (; start < span.Length; start++)
            {
                if (!Contains(trimElements, span[start]))
                    break;
            }

            return start;
        }

        /// <summary>
        /// Delimits all trailing occurrences of a specified element.
        /// </summary>
        /// <param name="span">The source span from which the element is removed.</param>
        /// <param name="start">The start index from which to being searching.</param>
        /// <param name="trimElements">The span which contains the set of elements to remove.</param>
        private static int ClampEnd<T>(ReadOnlySpan<T> span, int start, ReadOnlySpan<T> trimElements)
            where T : IEquatable<T>
        {
            // Initially, start==len==0. If ClampStart trims all, start==len
            Debug.Assert((uint)start <= span.Length);

            int end = span.Length - 1;

            for (; end >= start; end--)
            {
                if (!Contains(trimElements, span[end]))
                    break;
            }

            return end - start + 1;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool Contains<T>(ReadOnlySpan<T> span, T value)
            where T : IEquatable<T>
        {
            return Contains(ref MemoryMarshal.GetReference(span), value, span.Length);
        }

        private unsafe static bool Contains<T>(ref T searchSpace, T value, int length)
            where T : IEquatable<T>
        {
            Debug.Assert(length >= 0);

            var index = (IntPtr)0; // Use IntPtr for arithmetic to avoid unnecessary 64->32->64 truncations

            if (default(T) != null || value != null)
            {
                while (length >= 8)
                {
                    length -= 8;

                    if (value.Equals(Unsafe.Add(ref searchSpace, index + 0)) ||
                        value.Equals(Unsafe.Add(ref searchSpace, index + 1)) ||
                        value.Equals(Unsafe.Add(ref searchSpace, index + 2)) ||
                        value.Equals(Unsafe.Add(ref searchSpace, index + 3)) ||
                        value.Equals(Unsafe.Add(ref searchSpace, index + 4)) ||
                        value.Equals(Unsafe.Add(ref searchSpace, index + 5)) ||
                        value.Equals(Unsafe.Add(ref searchSpace, index + 6)) ||
                        value.Equals(Unsafe.Add(ref searchSpace, index + 7)))
                    {
                        goto Found;
                    }

                    index += 8;
                }

                if (length >= 4)
                {
                    length -= 4;

                    if (value.Equals(Unsafe.Add(ref searchSpace, index + 0)) ||
                        value.Equals(Unsafe.Add(ref searchSpace, index + 1)) ||
                        value.Equals(Unsafe.Add(ref searchSpace, index + 2)) ||
                        value.Equals(Unsafe.Add(ref searchSpace, index + 3)))
                    {
                        goto Found;
                    }

                    index += 4;
                }

                while (length > 0)
                {
                    length -= 1;

                    if (value.Equals(Unsafe.Add(ref searchSpace, index)))
                        goto Found;

                    index += 1;
                }
            }
            else
            {
                byte* len = (byte*)length;
                for (index = (IntPtr)0; index.ToPointer() < len; index += 1)
                {
                    if (Unsafe.Add(ref searchSpace, index) == null)
                    {
                        goto Found;
                    }
                }
            }

            return false;

Found:
            return true;
        }

        /// <summary>
        /// Removes all leading and trailing white-space characters from the memory.
        /// </summary>
        public static Memory<char> Trim(this Memory<char> memory)
        {
            Span<char> span = memory.Span;

            int start = 0;
            for (; start < span.Length; start++)
            {
                if (!char.IsWhiteSpace(span[start]))
                    break;
            }

            int end = span.Length - 1;
            for (; end >= start; end--)
            {
                if (!char.IsWhiteSpace(span[end]))
                    break;
            }

            return memory.Slice(start, end - start + 1);
        }

        /// <summary>
        /// Removes all leading white-space characters from the memory.
        /// </summary>
        public static Memory<char> TrimStart(this Memory<char> memory)
        {
            Span<char> span = memory.Span;

            int start = 0;
            for (; start < span.Length; start++)
            {
                if (!char.IsWhiteSpace(span[start]))
                    break;
            }

            return memory.Slice(start);
        }

        /// <summary>
        /// Removes all trailing white-space characters from the memory.
        /// </summary>
        public static Memory<char> TrimEnd(this Memory<char> memory)
        {
            Span<char> span = memory.Span;

            int end = span.Length - 1;
            for (; end >= 0; end--)
            {
                if (!char.IsWhiteSpace(span[end]))
                    break;
            }

            return memory.Slice(0, end + 1);
        }

        /// <summary>
        /// Removes all leading and trailing white-space characters from the memory.
        /// </summary>
        public static ReadOnlyMemory<char> Trim(this ReadOnlyMemory<char> memory)
        {
            ReadOnlySpan<char> span = memory.Span;

            int start = 0;
            for (; start < span.Length; start++)
            {
                if (!char.IsWhiteSpace(span[start]))
                    break;
            }

            int end = span.Length - 1;
            for (; end >= start; end--)
            {
                if (!char.IsWhiteSpace(span[end]))
                    break;
            }

            return memory.Slice(start, end - start + 1);
        }

        /// <summary>
        /// Removes all leading white-space characters from the memory.
        /// </summary>
        public static ReadOnlyMemory<char> TrimStart(this ReadOnlyMemory<char> memory)
        {
            ReadOnlySpan<char> span = memory.Span;

            int start = 0;
            for (; start < span.Length; start++)
            {
                if (!char.IsWhiteSpace(span[start]))
                    break;
            }

            return memory.Slice(start);
        }

        /// <summary>
        /// Removes all trailing white-space characters from the memory.
        /// </summary>
        public static ReadOnlyMemory<char> TrimEnd(this ReadOnlyMemory<char> memory)
        {
            ReadOnlySpan<char> span = memory.Span;

            int end = span.Length - 1;
            for (; end >= 0; end--)
            {
                if (!char.IsWhiteSpace(span[end]))
                    break;
            }

            return memory.Slice(0, end + 1);
        }

        /// <summary>
        /// Removes all leading and trailing white-space characters from the span.
        /// </summary>
        public static ReadOnlySpan<char> Trim(this ReadOnlySpan<char> span)
        {
            int start = 0;
            for (; start < span.Length; start++)
            {
                if (!char.IsWhiteSpace(span[start]))
                    break;
            }
            int end = span.Length - 1;
            for (; end >= start; end--)
            {
                if (!char.IsWhiteSpace(span[end]))
                    break;
            }
            return span.Slice(start, end - start + 1);
        }

        /// <summary>
        /// Removes all leading white-space characters from the span.
        /// </summary>
        public static ReadOnlySpan<char> TrimStart(this ReadOnlySpan<char> span)
        {
            int start = 0;
            for (; start < span.Length; start++)
            {
                if (!char.IsWhiteSpace(span[start]))
                    break;
            }
            return span.Slice(start);
        }

        /// <summary>
        /// Removes all trailing white-space characters from the span.
        /// </summary>
        public static ReadOnlySpan<char> TrimEnd(this ReadOnlySpan<char> span)
        {
            int end = span.Length - 1;
            for (; end >= 0; end--)
            {
                if (!char.IsWhiteSpace(span[end]))
                    break;
            }
            return span.Slice(0, end + 1);
        }

        /// <summary>
        /// Removes all leading and trailing occurrences of a specified character.
        /// </summary>
        /// <param name="span">The source span from which the character is removed.</param>
        /// <param name="trimChar">The specified character to look for and remove.</param>
        public static ReadOnlySpan<char> Trim(this ReadOnlySpan<char> span, char trimChar)
        {
            int start = 0;
            for (; start < span.Length; start++)
            {
                if (span[start] != trimChar)
                    break;
            }
            int end = span.Length - 1;
            for (; end >= start; end--)
            {
                if (span[end] != trimChar)
                    break;
            }
            return span.Slice(start, end - start + 1);
        }

        /// <summary>
        /// Removes all leading occurrences of a specified character.
        /// </summary>
        /// <param name="span">The source span from which the character is removed.</param>
        /// <param name="trimChar">The specified character to look for and remove.</param>
        public static ReadOnlySpan<char> TrimStart(this ReadOnlySpan<char> span, char trimChar)
        {
            int start = 0;
            for (; start < span.Length; start++)
            {
                if (span[start] != trimChar)
                    break;
            }
            return span.Slice(start);
        }

        /// <summary>
        /// Removes all trailing occurrences of a specified character.
        /// </summary>
        /// <param name="span">The source span from which the character is removed.</param>
        /// <param name="trimChar">The specified character to look for and remove.</param>
        public static ReadOnlySpan<char> TrimEnd(this ReadOnlySpan<char> span, char trimChar)
        {
            int end = span.Length - 1;
            for (; end >= 0; end--)
            {
                if (span[end] != trimChar)
                    break;
            }
            return span.Slice(0, end + 1);
        }

        /// <summary>
        /// Removes all leading and trailing occurrences of a set of characters specified
        /// in a readonly span from the span.
        /// </summary>
        /// <param name="span">The source span from which the characters are removed.</param>
        /// <param name="trimChars">The span which contains the set of characters to remove.</param>
        /// <remarks>If <paramref name="trimChars"/> is empty, white-space characters are removed instead.</remarks>
        public static ReadOnlySpan<char> Trim(this ReadOnlySpan<char> span, ReadOnlySpan<char> trimChars)
        {
            return span.TrimStart(trimChars).TrimEnd(trimChars);
        }

        /// <summary>
        /// Removes all leading occurrences of a set of characters specified
        /// in a readonly span from the span.
        /// </summary>
        /// <param name="span">The source span from which the characters are removed.</param>
        /// <param name="trimChars">The span which contains the set of characters to remove.</param>
        /// <remarks>If <paramref name="trimChars"/> is empty, white-space characters are removed instead.</remarks>
        public static ReadOnlySpan<char> TrimStart(this ReadOnlySpan<char> span, ReadOnlySpan<char> trimChars)
        {
            if (trimChars.IsEmpty)
            {
                return span.TrimStart();
            }

            int start = 0;
            for (; start < span.Length; start++)
            {
                for (int i = 0; i < trimChars.Length; i++)
                {
                    if (span[start] == trimChars[i])
                        goto Next;
                }
                break;
Next:
                ;
            }
            return span.Slice(start);
        }

        /// <summary>
        /// Removes all trailing occurrences of a set of characters specified
        /// in a readonly span from the span.
        /// </summary>
        /// <param name="span">The source span from which the characters are removed.</param>
        /// <param name="trimChars">The span which contains the set of characters to remove.</param>
        /// <remarks>If <paramref name="trimChars"/> is empty, white-space characters are removed instead.</remarks>
        public static ReadOnlySpan<char> TrimEnd(this ReadOnlySpan<char> span, ReadOnlySpan<char> trimChars)
        {
            if (trimChars.IsEmpty)
            {
                return span.TrimEnd();
            }

            int end = span.Length - 1;
            for (; end >= 0; end--)
            {
                for (int i = 0; i < trimChars.Length; i++)
                {
                    if (span[end] == trimChars[i])
                        goto Next;
                }
                break;
Next:
                ;
            }
            return span.Slice(0, end + 1);
        }

        /// <summary>
        /// Removes all leading and trailing white-space characters from the span.
        /// </summary>
        public static Span<char> Trim(this Span<char> span)
        {
            int start = 0;
            for (; start < span.Length; start++)
            {
                if (!char.IsWhiteSpace(span[start]))
                    break;
            }

            int end = span.Length - 1;
            for (; end >= start; end--)
            {
                if (!char.IsWhiteSpace(span[end]))
                    break;
            }

            return span.Slice(start, end - start + 1);
        }

        /// <summary>
        /// Removes all leading white-space characters from the span.
        /// </summary>
        public static Span<char> TrimStart(this Span<char> span)
        {
            int start = 0;
            for (; start < span.Length; start++)
            {
                if (!char.IsWhiteSpace(span[start]))
                    break;
            }

            return span.Slice(start);
        }

        /// <summary>
        /// Removes all trailing white-space characters from the span.
        /// </summary>
        public static Span<char> TrimEnd(this Span<char> span)
        {
            int end = span.Length - 1;
            for (; end >= 0; end--)
            {
                if (!char.IsWhiteSpace(span[end]))
                    break;
            }

            return span.Slice(0, end + 1);
        }
    }
}
