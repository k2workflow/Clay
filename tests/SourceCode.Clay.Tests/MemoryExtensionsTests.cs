// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Xunit;

namespace SourceCode.Clay.Tests
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public static partial class MemoryTests
    {
        [Theory]
        [InlineData(new int[0], 1, new int[0])]
        [InlineData(new int[] { 1 }, 1, new int[0])]
        [InlineData(new int[] { 2 }, 1, new int[] { 2 })]
        [InlineData(new int[] { 1, 2, 1 }, 1, new int[] { 2, 1 })]
        [InlineData(new int[] { 1, 1, 2, 1 }, 1, new int[] { 2, 1 })]
        [InlineData(new int[] { 1, 1, 2, 1 }, 2, new int[] { 1, 1, 2, 1 })]
        [InlineData(new int[] { 1, 1, 2, 1 }, 3, new int[] { 1, 1, 2, 1 })]
        [InlineData(new int[] { 1, 1, 1, 2 }, 1, new int[] { 2 })]
        [InlineData(new int[] { 1, 1, 1, 1 }, 1, new int[0])]
        public static void MemoryExtensions_TrimStart_Single(int[] values, int trim, int[] expected)
        {
            Memory<int> memory = new Memory<int>(values).TrimStart(trim);
            Assert.True(System.Linq.Enumerable.SequenceEqual(expected, memory.ToArray()));

            ReadOnlyMemory<int> rom = new ReadOnlyMemory<int>(values).TrimStart(trim);
            Assert.True(System.Linq.Enumerable.SequenceEqual(expected, rom.ToArray()));

            Span<int> span = new Span<int>(values).TrimStart(trim);
            Assert.True(System.Linq.Enumerable.SequenceEqual(expected, span.ToArray()));

            ReadOnlySpan<int> ros = new ReadOnlySpan<int>(values).TrimStart(trim);
            Assert.True(System.Linq.Enumerable.SequenceEqual(expected, ros.ToArray()));
        }

        [Theory]
        [InlineData(new int[0], 1, new int[0])]
        [InlineData(new int[] { 1 }, 1, new int[0])]
        [InlineData(new int[] { 2 }, 1, new int[] { 2 })]
        [InlineData(new int[] { 1, 2, 1 }, 1, new int[] { 1, 2 })]
        [InlineData(new int[] { 1, 2, 1, 1 }, 1, new int[] { 1, 2 })]
        [InlineData(new int[] { 1, 2, 1, 1 }, 2, new int[] { 1, 2, 1, 1 })]
        [InlineData(new int[] { 1, 2, 1, 1 }, 3, new int[] { 1, 2, 1, 1 })]
        [InlineData(new int[] { 2, 1, 1, 1 }, 1, new int[] { 2 })]
        [InlineData(new int[] { 1, 1, 1, 1 }, 1, new int[0])]
        public static void MemoryExtensions_TrimEnd_Single(int[] values, int trim, int[] expected)
        {
            Memory<int> memory = new Memory<int>(values).TrimEnd(trim);
            Assert.True(System.Linq.Enumerable.SequenceEqual(expected, memory.ToArray()));

            ReadOnlyMemory<int> rom = new ReadOnlyMemory<int>(values).TrimEnd(trim);
            Assert.True(System.Linq.Enumerable.SequenceEqual(expected, rom.ToArray()));

            Span<int> span = new Span<int>(values).TrimEnd(trim);
            Assert.True(System.Linq.Enumerable.SequenceEqual(expected, span.ToArray()));

            ReadOnlySpan<int> ros = new ReadOnlySpan<int>(values).TrimEnd(trim);
            Assert.True(System.Linq.Enumerable.SequenceEqual(expected, ros.ToArray()));
        }

        [Theory]
        [InlineData(new int[0], 1, new int[0])]
        [InlineData(new int[] { 1 }, 1, new int[0])]
        [InlineData(new int[] { 2 }, 1, new int[] { 2 })]
        [InlineData(new int[] { 1, 2, 1 }, 1, new int[] { 2 })]
        [InlineData(new int[] { 1, 2, 1, 1 }, 1, new int[] { 2 })]
        [InlineData(new int[] { 1, 2, 1, 1 }, 2, new int[] { 1, 2, 1, 1 })]
        [InlineData(new int[] { 1, 2, 1, 1 }, 3, new int[] { 1, 2, 1, 1 })]
        [InlineData(new int[] { 2, 1, 1, 1 }, 1, new int[] { 2 })]
        [InlineData(new int[] { 1, 1, 1, 2 }, 1, new int[] { 2 })]
        [InlineData(new int[] { 1, 1, 1, 1 }, 1, new int[0])]
        public static void MemoryExtensions_Trim_Single(int[] values, int trim, int[] expected)
        {
            Memory<int> memory = new Memory<int>(values).Trim(trim);
            Assert.True(System.Linq.Enumerable.SequenceEqual(expected, memory.ToArray()));

            ReadOnlyMemory<int> rom = new ReadOnlyMemory<int>(values).Trim(trim);
            Assert.True(System.Linq.Enumerable.SequenceEqual(expected, rom.ToArray()));

            Span<int> span = new Span<int>(values).Trim(trim);
            Assert.True(System.Linq.Enumerable.SequenceEqual(expected, span.ToArray()));

            ReadOnlySpan<int> ros = new ReadOnlySpan<int>(values).Trim(trim);
            Assert.True(System.Linq.Enumerable.SequenceEqual(expected, ros.ToArray()));
        }

        [Theory]
        [InlineData(new int[0], new int[0], new int[0])]
        [InlineData(new int[0], new int[] { 1 }, new int[0])]
        [InlineData(new int[] { 1 }, new int[0], new int[] { 1 })]
        [InlineData(new int[] { 1 }, new int[] { 1 }, new int[0])]
        [InlineData(new int[] { 2 }, new int[] { 1 }, new int[] { 2 })]
        [InlineData(new int[] { 1, 2, 1 }, new int[] { 1 }, new int[] { 2, 1 })]
        [InlineData(new int[] { 1, 1, 2, 1 }, new int[] { 1 }, new int[] { 2, 1 })]
        [InlineData(new int[] { 1, 1, 2, 1 }, new int[] { 2 }, new int[] { 1, 1, 2, 1 })]
        [InlineData(new int[] { 1, 1, 2, 1 }, new int[] { 3 }, new int[] { 1, 1, 2, 1 })]
        [InlineData(new int[] { 1, 1, 2, 1 }, new int[] { 1, 2 }, new int[0])]
        [InlineData(new int[] { 1, 1, 2, 3 }, new int[] { 1, 2 }, new int[] { 3 })]
        [InlineData(new int[] { 1, 1, 2, 3 }, new int[] { 1, 2, 4 }, new int[] { 3 })]
        [InlineData(new int[] { 1, 1, 1, 2 }, new int[] { 1 }, new int[] { 2 })]
        [InlineData(new int[] { 1, 1, 1, 1 }, new int[] { 1 }, new int[0])]
        public static void MemoryExtensions_TrimStart_Multi(int[] values, int[] trims, int[] expected)
        {
            Memory<int> memory = new Memory<int>(values).TrimStart(trims);
            Assert.True(System.Linq.Enumerable.SequenceEqual(expected, memory.ToArray()));

            ReadOnlyMemory<int> rom = new ReadOnlyMemory<int>(values).TrimStart(trims);
            Assert.True(System.Linq.Enumerable.SequenceEqual(expected, rom.ToArray()));

            Span<int> span = new Span<int>(values).TrimStart(trims);
            Assert.True(System.Linq.Enumerable.SequenceEqual(expected, span.ToArray()));

            ReadOnlySpan<int> ros = new ReadOnlySpan<int>(values).TrimStart(trims);
            Assert.True(System.Linq.Enumerable.SequenceEqual(expected, ros.ToArray()));
        }

        [Theory]
        [InlineData(new int[0], new int[0], new int[0])]
        [InlineData(new int[0], new int[] { 1 }, new int[0])]
        [InlineData(new int[] { 1 }, new int[0], new int[] { 1 })]
        [InlineData(new int[] { 1 }, new int[] { 1 }, new int[0])]
        [InlineData(new int[] { 2 }, new int[] { 1 }, new int[] { 2 })]
        [InlineData(new int[] { 1, 2, 1 }, new int[] { 1 }, new int[] { 1, 2 })]
        [InlineData(new int[] { 1, 2, 1, 1 }, new int[] { 1 }, new int[] { 1, 2 })]
        [InlineData(new int[] { 1, 2, 1, 1 }, new int[] { 2 }, new int[] { 1, 2, 1, 1 })]
        [InlineData(new int[] { 1, 2, 1, 1 }, new int[] { 3 }, new int[] { 1, 2, 1, 1 })]
        [InlineData(new int[] { 1, 2, 1, 1 }, new int[] { 1, 2 }, new int[0])]
        [InlineData(new int[] { 3, 2, 1, 1 }, new int[] { 1, 2 }, new int[] { 3 })]
        [InlineData(new int[] { 3, 2, 1, 1 }, new int[] { 1, 2, 4 }, new int[] { 3 })]
        [InlineData(new int[] { 2, 1, 1, 1 }, new int[] { 1 }, new int[] { 2 })]
        [InlineData(new int[] { 1, 1, 1, 1 }, new int[] { 1 }, new int[0])]
        public static void MemoryExtensions_TrimEnd_Multi(int[] values, int[] trims, int[] expected)
        {
            Memory<int> memory = new Memory<int>(values).TrimEnd(trims);
            Assert.True(System.Linq.Enumerable.SequenceEqual(expected, memory.ToArray()));

            ReadOnlyMemory<int> rom = new ReadOnlyMemory<int>(values).TrimEnd(trims);
            Assert.True(System.Linq.Enumerable.SequenceEqual(expected, rom.ToArray()));

            Span<int> span = new Span<int>(values).TrimEnd(trims);
            Assert.True(System.Linq.Enumerable.SequenceEqual(expected, span.ToArray()));

            ReadOnlySpan<int> ros = new ReadOnlySpan<int>(values).TrimEnd(trims);
            Assert.True(System.Linq.Enumerable.SequenceEqual(expected, ros.ToArray()));
        }

        [Theory]
        [InlineData(new int[0], new int[0], new int[0])]
        [InlineData(new int[0], new int[] { 1 }, new int[0])]
        [InlineData(new int[] { 1 }, new int[0], new int[] { 1 })]
        [InlineData(new int[] { 1 }, new int[] { 1 }, new int[0])]
        [InlineData(new int[] { 2 }, new int[] { 1 }, new int[] { 2 })]
        [InlineData(new int[] { 1, 2, 1 }, new int[] { 1 }, new int[] { 2 })]
        [InlineData(new int[] { 1, 2, 1, 1 }, new int[] { 1 }, new int[] { 2 })]
        [InlineData(new int[] { 1, 2, 1, 1 }, new int[] { 2 }, new int[] { 1, 2, 1, 1 })]
        [InlineData(new int[] { 1, 2, 1, 1 }, new int[] { 3 }, new int[] { 1, 2, 1, 1 })]
        [InlineData(new int[] { 1, 2, 1, 1 }, new int[] { 1, 2 }, new int[0])]
        [InlineData(new int[] { 2, 1, 3, 2, 1, 1 }, new int[] { 1, 2 }, new int[] { 3 })]
        [InlineData(new int[] { 2, 1, 3, 2, 1, 1 }, new int[] { 1, 2, 4 }, new int[] { 3 })]
        [InlineData(new int[] { 1, 2, 1, 1, 1 }, new int[] { 1 }, new int[] { 2 })]
        [InlineData(new int[] { 1, 1, 1, 1 }, new int[] { 1 }, new int[0])]
        public static void MemoryExtensions_Trim_Multi(int[] values, int[] trims, int[] expected)
        {
            Memory<int> memory = new Memory<int>(values).Trim(trims);
            Assert.True(System.Linq.Enumerable.SequenceEqual(expected, memory.ToArray()));

            ReadOnlyMemory<int> rom = new ReadOnlyMemory<int>(values).Trim(trims);
            Assert.True(System.Linq.Enumerable.SequenceEqual(expected, rom.ToArray()));

            Span<int> span = new Span<int>(values).Trim(trims);
            Assert.True(System.Linq.Enumerable.SequenceEqual(expected, span.ToArray()));

            ReadOnlySpan<int> ros = new ReadOnlySpan<int>(values).Trim(trims);
            Assert.True(System.Linq.Enumerable.SequenceEqual(expected, ros.ToArray()));
        }

        public sealed class Foo : IEquatable<Foo>
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

        public static IEnumerable<object[]> IdempotentValues => new object[][]
        {
            new object[1] { new Foo[] { } },
            new object[1] { new Foo[] { null, 1, 2, 3, null, 2, 1, null } }
        };

        [Theory]
        [MemberData(nameof(IdempotentValues))]
        public static void MemoryExtensions_TrimStart_Idempotent(Foo[] values)
        {
            Foo[] expected = values;

            Foo[] trim = null;

            Memory<Foo> memory = new Memory<Foo>(values).TrimStart(trim);
            Assert.True(System.Linq.Enumerable.SequenceEqual(expected, memory.ToArray()));

            ReadOnlyMemory<Foo> rom = new ReadOnlyMemory<Foo>(values).TrimStart(trim);
            Assert.True(System.Linq.Enumerable.SequenceEqual(expected, rom.ToArray()));

            Span<Foo> span = new Span<Foo>(values).TrimStart(trim);
            Assert.True(System.Linq.Enumerable.SequenceEqual(expected, span.ToArray()));

            ReadOnlySpan<Foo> ros = new ReadOnlySpan<Foo>(values).TrimStart(trim);
            Assert.True(System.Linq.Enumerable.SequenceEqual(expected, ros.ToArray()));

            trim = new Foo[] { };

            memory = new Memory<Foo>(values).TrimStart(trim);
            Assert.True(System.Linq.Enumerable.SequenceEqual(expected, memory.ToArray()));

            rom = new ReadOnlyMemory<Foo>(values).TrimStart(trim);
            Assert.True(System.Linq.Enumerable.SequenceEqual(expected, rom.ToArray()));

            span = new Span<Foo>(values).TrimStart(trim);
            Assert.True(System.Linq.Enumerable.SequenceEqual(expected, span.ToArray()));

            ros = new ReadOnlySpan<Foo>(values).TrimStart(trim);
            Assert.True(System.Linq.Enumerable.SequenceEqual(expected, ros.ToArray()));
        }

        [Theory]
        [MemberData(nameof(IdempotentValues))]
        public static void MemoryExtensions_TrimEnd_Idempotent(Foo[] values)
        {
            Foo[] expected = values;

            Foo[] trim = null;

            Memory<Foo> memory = new Memory<Foo>(values).TrimEnd(trim);
            Assert.True(System.Linq.Enumerable.SequenceEqual(expected, memory.ToArray()));

            ReadOnlyMemory<Foo> rom = new ReadOnlyMemory<Foo>(values).TrimEnd(trim);
            Assert.True(System.Linq.Enumerable.SequenceEqual(expected, rom.ToArray()));

            Span<Foo> span = new Span<Foo>(values).TrimEnd(trim);
            Assert.True(System.Linq.Enumerable.SequenceEqual(expected, span.ToArray()));

            ReadOnlySpan<Foo> ros = new ReadOnlySpan<Foo>(values).TrimEnd(trim);
            Assert.True(System.Linq.Enumerable.SequenceEqual(expected, ros.ToArray()));

            trim = new Foo[] { };

            memory = new Memory<Foo>(values).TrimEnd(trim);
            Assert.True(System.Linq.Enumerable.SequenceEqual(expected, memory.ToArray()));

            rom = new ReadOnlyMemory<Foo>(values).TrimEnd(trim);
            Assert.True(System.Linq.Enumerable.SequenceEqual(expected, rom.ToArray()));

            span = new Span<Foo>(values).TrimEnd(trim);
            Assert.True(System.Linq.Enumerable.SequenceEqual(expected, span.ToArray()));

            ros = new ReadOnlySpan<Foo>(values).TrimEnd(trim);
            Assert.True(System.Linq.Enumerable.SequenceEqual(expected, ros.ToArray()));
        }

        [Theory]
        [MemberData(nameof(IdempotentValues))]
        public static void MemoryExtensions_Trim_Idempotent(Foo[] values)
        {
            Foo[] expected = values;

            Foo[] trim = null;

            Memory<Foo> memory = new Memory<Foo>(values).Trim(trim);
            Assert.True(System.Linq.Enumerable.SequenceEqual(expected, memory.ToArray()));

            ReadOnlyMemory<Foo> rom = new ReadOnlyMemory<Foo>(values).Trim(trim);
            Assert.True(System.Linq.Enumerable.SequenceEqual(expected, rom.ToArray()));

            Span<Foo> span = new Span<Foo>(values).Trim(trim);
            Assert.True(System.Linq.Enumerable.SequenceEqual(expected, span.ToArray()));

            ReadOnlySpan<Foo> ros = new ReadOnlySpan<Foo>(values).Trim(trim);
            Assert.True(System.Linq.Enumerable.SequenceEqual(expected, ros.ToArray()));

            trim = new Foo[] { };

            memory = new Memory<Foo>(values).Trim(trim);
            Assert.True(System.Linq.Enumerable.SequenceEqual(expected, memory.ToArray()));

            rom = new ReadOnlyMemory<Foo>(values).Trim(trim);
            Assert.True(System.Linq.Enumerable.SequenceEqual(expected, rom.ToArray()));

            span = new Span<Foo>(values).Trim(trim);
            Assert.True(System.Linq.Enumerable.SequenceEqual(expected, span.ToArray()));

            ros = new ReadOnlySpan<Foo>(values).Trim(trim);
            Assert.True(System.Linq.Enumerable.SequenceEqual(expected, ros.ToArray()));
        }

        [Fact]
        public static void MemoryExtensions_TrimStart_Single_Null()
        {
            var values = new Foo[] { null, null, 1, 2, null, null };
            Foo trim = null;
            var expected = new Foo[] { 1, 2, null, null };

            Memory<Foo> memory = new Memory<Foo>(values).TrimStart(trim);
            Assert.True(System.Linq.Enumerable.SequenceEqual(expected, memory.ToArray()));

            ReadOnlyMemory<Foo> rom = new ReadOnlyMemory<Foo>(values).TrimStart(trim);
            Assert.True(System.Linq.Enumerable.SequenceEqual(expected, rom.ToArray()));

            Span<Foo> span = new Span<Foo>(values).TrimStart(trim);
            Assert.True(System.Linq.Enumerable.SequenceEqual(expected, span.ToArray()));

            ReadOnlySpan<Foo> ros = new ReadOnlySpan<Foo>(values).TrimStart(trim);
            Assert.True(System.Linq.Enumerable.SequenceEqual(expected, ros.ToArray()));
        }

        [Fact]
        public static void MemoryExtensions_TrimStart_Multi_Null()
        {
            var values = new Foo[] { null, 1, 2, 3, null, 2, 1, null };
            var trim = new Foo[] { null, 1, 2 };
            var expected = new Foo[] { 3, null, 2, 1, null };

            Memory<Foo> memory = new Memory<Foo>(values).TrimStart(trim);
            Assert.True(System.Linq.Enumerable.SequenceEqual(expected, memory.ToArray()));

            ReadOnlyMemory<Foo> rom = new ReadOnlyMemory<Foo>(values).TrimStart(trim);
            Assert.True(System.Linq.Enumerable.SequenceEqual(expected, rom.ToArray()));

            Span<Foo> span = new Span<Foo>(values).TrimStart(trim);
            Assert.True(System.Linq.Enumerable.SequenceEqual(expected, span.ToArray()));

            ReadOnlySpan<Foo> ros = new ReadOnlySpan<Foo>(values).TrimStart(trim);
            Assert.True(System.Linq.Enumerable.SequenceEqual(expected, ros.ToArray()));
        }

        [Fact]
        public static void MemoryExtensions_TrimEnd_Single_Null()
        {
            var values = new Foo[] { null, null, 1, 2, null, null };
            Foo trim = null;
            var expected = new Foo[] { null, null, 1, 2 };

            Memory<Foo> memory = new Memory<Foo>(values).TrimEnd(trim);
            Assert.True(System.Linq.Enumerable.SequenceEqual(expected, memory.ToArray()));

            ReadOnlyMemory<Foo> rom = new ReadOnlyMemory<Foo>(values).TrimEnd(trim);
            Assert.True(System.Linq.Enumerable.SequenceEqual(expected, rom.ToArray()));

            Span<Foo> span = new Span<Foo>(values).TrimEnd(trim);
            Assert.True(System.Linq.Enumerable.SequenceEqual(expected, span.ToArray()));

            ReadOnlySpan<Foo> ros = new ReadOnlySpan<Foo>(values).TrimEnd(trim);
            Assert.True(System.Linq.Enumerable.SequenceEqual(expected, ros.ToArray()));
        }

        [Fact]
        public static void MemoryExtensions_TrimEnd_Multi_Null()
        {
            var values = new Foo[] { null, 1, 2, 3, null, 2, 1, null };
            var trim = new Foo[] { null, 1, 2 };
            var expected = new Foo[] { null, 1, 2, 3 };

            Memory<Foo> memory = new Memory<Foo>(values).TrimEnd(trim);
            Assert.True(System.Linq.Enumerable.SequenceEqual(expected, memory.ToArray()));

            ReadOnlyMemory<Foo> rom = new ReadOnlyMemory<Foo>(values).TrimEnd(trim);
            Assert.True(System.Linq.Enumerable.SequenceEqual(expected, rom.ToArray()));

            Span<Foo> span = new Span<Foo>(values).TrimEnd(trim);
            Assert.True(System.Linq.Enumerable.SequenceEqual(expected, span.ToArray()));

            ReadOnlySpan<Foo> ros = new ReadOnlySpan<Foo>(values).TrimEnd(trim);
            Assert.True(System.Linq.Enumerable.SequenceEqual(expected, ros.ToArray()));
        }

        [Fact]
        public static void MemoryExtensions_Trim_Single_Null()
        {
            var values = new Foo[] { null, null, 1, 2, null, null };
            Foo trim = null;
            var expected = new Foo[] { 1, 2 };

            Memory<Foo> memory = new Memory<Foo>(values).Trim(trim);
            Assert.True(System.Linq.Enumerable.SequenceEqual(expected, memory.ToArray()));

            ReadOnlyMemory<Foo> rom = new ReadOnlyMemory<Foo>(values).Trim(trim);
            Assert.True(System.Linq.Enumerable.SequenceEqual(expected, rom.ToArray()));

            Span<Foo> span = new Span<Foo>(values).Trim(trim);
            Assert.True(System.Linq.Enumerable.SequenceEqual(expected, span.ToArray()));

            ReadOnlySpan<Foo> ros = new ReadOnlySpan<Foo>(values).Trim(trim);
            Assert.True(System.Linq.Enumerable.SequenceEqual(expected, ros.ToArray()));
        }

        [Fact]
        public static void MemoryExtensions_Trim_Multi_Null()
        {
            var values = new Foo[] { null, 1, 2, 3, null, 2, 1, null };
            var trim = new Foo[] { null, 1, 2 };
            var expected = new Foo[] { 3 };

            Memory<Foo> memory = new Memory<Foo>(values).Trim(trim);
            Assert.True(System.Linq.Enumerable.SequenceEqual(expected, memory.ToArray()));

            ReadOnlyMemory<Foo> rom = new ReadOnlyMemory<Foo>(values).Trim(trim);
            Assert.True(System.Linq.Enumerable.SequenceEqual(expected, rom.ToArray()));

            Span<Foo> span = new Span<Foo>(values).Trim(trim);
            Assert.True(System.Linq.Enumerable.SequenceEqual(expected, span.ToArray()));

            ReadOnlySpan<Foo> ros = new ReadOnlySpan<Foo>(values).Trim(trim);
            Assert.True(System.Linq.Enumerable.SequenceEqual(expected, ros.ToArray()));
        }
    }

    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public static partial class StringTests
    {
        [Theory]
        [InlineData("  Hello  ", "Hello")]
        [InlineData("      \t      ", "")]
        [InlineData("", "")]
        [InlineData("      ", "")]
        public static void Trim_Memory(string s, string expected)
        {
            Assert.Equal(expected, s.AsSpan().Trim().ToString()); // ReadOnlySpan
            Assert.Equal(expected, new Span<char>(s.ToCharArray()).Trim().ToString());
            Assert.Equal(expected, new Memory<char>(s.ToCharArray()).Trim().ToString());
            Assert.Equal(expected, s.AsMemory().Trim().ToString()); // ReadOnlyMemory
        }

        [Theory]
        [InlineData("  Hello  ", "  Hello")]
        [InlineData("      \t      ", "")]
        [InlineData("", "")]
        [InlineData("      ", "")]
        public static void TrimEnd_Memory(string s, string expected)
        {
            Assert.Equal(expected, s.AsSpan().TrimEnd().ToString()); // ReadOnlySpan
            Assert.Equal(expected, new Span<char>(s.ToCharArray()).TrimEnd().ToString());
            Assert.Equal(expected, new Memory<char>(s.ToCharArray()).TrimEnd().ToString());
            Assert.Equal(expected, s.AsMemory().TrimEnd().ToString()); // ReadOnlyMemory
        }

        [Theory]
        [InlineData("  Hello  ", "Hello  ")]
        [InlineData("      \t      ", "")]
        [InlineData("", "")]
        [InlineData("      ", "")]
        public static void TrimStart_Memory(string s, string expected)
        {
            Assert.Equal(expected, s.AsSpan().TrimStart().ToString()); // ReadOnlySpan
            Assert.Equal(expected, new Span<char>(s.ToCharArray()).TrimStart().ToString());
            Assert.Equal(expected, new Memory<char>(s.ToCharArray()).TrimStart().ToString());
            Assert.Equal(expected, s.AsMemory().TrimStart().ToString()); // ReadOnlyMemory
        }

        [Fact]
        public static void ZeroLengthTrim_Memory()
        {
            string s1 = string.Empty;

            ReadOnlySpan<char> ros = s1.AsSpan();
            Assert.True(ros.SequenceEqual(ros.Trim()));
            Assert.True(ros.SequenceEqual(ros.TrimStart()));
            Assert.True(ros.SequenceEqual(ros.TrimEnd()));

            Span<char> span = new Span<char>(s1.ToCharArray());
            Assert.True(span.SequenceEqual(span.Trim()));
            Assert.True(span.SequenceEqual(span.TrimStart()));
            Assert.True(span.SequenceEqual(span.TrimEnd()));

            Memory<char> mem = new Memory<char>(s1.ToCharArray());
            Assert.True(mem.Span.SequenceEqual(mem.Trim().Span));
            Assert.True(mem.Span.SequenceEqual(mem.TrimStart().Span));
            Assert.True(mem.Span.SequenceEqual(mem.TrimEnd().Span));

            ReadOnlyMemory<char> rom = new ReadOnlyMemory<char>(s1.ToCharArray());
            Assert.True(rom.Span.SequenceEqual(rom.Trim().Span));
            Assert.True(rom.Span.SequenceEqual(rom.TrimStart().Span));
            Assert.True(rom.Span.SequenceEqual(rom.TrimEnd().Span));
        }

        [Fact]
        public static void NoWhiteSpaceTrim_Memory()
        {
            for (int length = 0; length < 32; length++)
            {
                char[] a = new char[length];
                for (int i = 0; i < length; i++)
                {
                    a[i] = 'a';
                }
                string s1 = new string(a);

                ReadOnlySpan<char> ros = s1.AsSpan();
                Assert.True(ros.SequenceEqual(ros.Trim()));
                Assert.True(ros.SequenceEqual(ros.TrimStart()));
                Assert.True(ros.SequenceEqual(ros.TrimEnd()));

                Span<char> span = new Span<char>(a);
                Assert.True(span.SequenceEqual(span.Trim()));
                Assert.True(span.SequenceEqual(span.TrimStart()));
                Assert.True(span.SequenceEqual(span.TrimEnd()));

                Memory<char> mem = new Memory<char>(a);
                Assert.True(mem.Span.SequenceEqual(mem.Trim().Span));
                Assert.True(mem.Span.SequenceEqual(mem.TrimStart().Span));
                Assert.True(mem.Span.SequenceEqual(mem.TrimEnd().Span));

                ReadOnlyMemory<char> rom = new ReadOnlyMemory<char>(a);
                Assert.True(rom.Span.SequenceEqual(rom.Trim().Span));
                Assert.True(rom.Span.SequenceEqual(rom.TrimStart().Span));
                Assert.True(rom.Span.SequenceEqual(rom.TrimEnd().Span));
            }
        }

        [Fact]
        public static void OnlyWhiteSpaceTrim_Memory()
        {
            for (int length = 0; length < 32; length++)
            {
                char[] a = new char[length];
                for (int i = 0; i < length; i++)
                {
                    a[i] = ' ';
                }
                string s1 = new string(a);

                ReadOnlySpan<char> ros = new ReadOnlySpan<char>(a);
                Assert.True(ReadOnlySpan<char>.Empty.SequenceEqual(ros.Trim()));
                Assert.True(ReadOnlySpan<char>.Empty.SequenceEqual(ros.TrimStart()));
                Assert.True(ReadOnlySpan<char>.Empty.SequenceEqual(ros.TrimEnd()));

                Span<char> span = new Span<char>(a);
                Assert.True(Span<char>.Empty.SequenceEqual(span.Trim()));
                Assert.True(Span<char>.Empty.SequenceEqual(span.TrimStart()));
                Assert.True(Span<char>.Empty.SequenceEqual(span.TrimEnd()));

                Memory<char> mem = new Memory<char>(a);
                Assert.True(Memory<char>.Empty.Span.SequenceEqual(mem.Trim().Span));
                Assert.True(Memory<char>.Empty.Span.SequenceEqual(mem.TrimStart().Span));
                Assert.True(Memory<char>.Empty.Span.SequenceEqual(mem.TrimEnd().Span));

                ReadOnlyMemory<char> rom = new ReadOnlyMemory<char>(a);
                Assert.True(ReadOnlyMemory<char>.Empty.Span.SequenceEqual(rom.Trim().Span));
                Assert.True(ReadOnlyMemory<char>.Empty.Span.SequenceEqual(rom.TrimStart().Span));
                Assert.True(ReadOnlyMemory<char>.Empty.Span.SequenceEqual(rom.TrimEnd().Span));
            }
        }

        [Fact]
        public static void WhiteSpaceAtStartTrim_Memory()
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

                ReadOnlySpan<char> ros = s1.AsSpan();
                Assert.True(ros.Slice(1).SequenceEqual(ros.Trim()));
                Assert.True(ros.Slice(1).SequenceEqual(ros.TrimStart()));
                Assert.True(ros.SequenceEqual(ros.TrimEnd()));

                Span<char> span = new Span<char>(a);
                Assert.True(span.Slice(1).SequenceEqual(span.Trim()));
                Assert.True(span.Slice(1).SequenceEqual(span.TrimStart()));
                Assert.True(span.SequenceEqual(span.TrimEnd()));

                Memory<char> mem = new Memory<char>(a);
                Assert.True(mem.Slice(1).Span.SequenceEqual(mem.Trim().Span));
                Assert.True(mem.Slice(1).Span.SequenceEqual(mem.TrimStart().Span));
                Assert.True(mem.Span.SequenceEqual(mem.TrimEnd().Span));

                ReadOnlyMemory<char> rom = new ReadOnlyMemory<char>(a);
                Assert.True(rom.Slice(1).Span.SequenceEqual(rom.Trim().Span));
                Assert.True(rom.Slice(1).Span.SequenceEqual(rom.TrimStart().Span));
                Assert.True(rom.Span.SequenceEqual(rom.TrimEnd().Span));
            }
        }

        [Fact]
        public static void WhiteSpaceAtEndTrim_Memory()
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

                ReadOnlySpan<char> ros = s1.AsSpan();
                Assert.True(ros.Slice(0, length - 1).SequenceEqual(ros.Trim()));
                Assert.True(ros.SequenceEqual(ros.TrimStart()));
                Assert.True(ros.Slice(0, length - 1).SequenceEqual(ros.TrimEnd()));

                Span<char> span = new Span<char>(a);
                Assert.True(span.Slice(0, length - 1).SequenceEqual(span.Trim()));
                Assert.True(span.SequenceEqual(span.TrimStart()));
                Assert.True(span.Slice(0, length - 1).SequenceEqual(span.TrimEnd()));

                Memory<char> mem = new Memory<char>(a);
                Assert.True(mem.Slice(0, length - 1).Span.SequenceEqual(mem.Trim().Span));
                Assert.True(mem.Span.SequenceEqual(mem.TrimStart().Span));
                Assert.True(mem.Slice(0, length - 1).Span.SequenceEqual(mem.TrimEnd().Span));

                ReadOnlyMemory<char> rom = new ReadOnlyMemory<char>(a);
                Assert.True(rom.Slice(0, length - 1).Span.SequenceEqual(rom.Trim().Span));
                Assert.True(rom.Span.SequenceEqual(rom.TrimStart().Span));
                Assert.True(rom.Slice(0, length - 1).Span.SequenceEqual(rom.TrimEnd().Span));
            }
        }

        [Fact]
        public static void WhiteSpaceAtStartAndEndTrim_Memory()
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

                ReadOnlySpan<char> ros = s1.AsSpan();
                Assert.True(ros.Slice(1, length - 2).SequenceEqual(ros.Trim()));
                Assert.True(ros.Slice(1).SequenceEqual(ros.TrimStart()));
                Assert.True(ros.Slice(0, length - 1).SequenceEqual(ros.TrimEnd()));

                Span<char> span = new Span<char>(a);
                Assert.True(span.Slice(1, length - 2).SequenceEqual(span.Trim()));
                Assert.True(span.Slice(1).SequenceEqual(span.TrimStart()));
                Assert.True(span.Slice(0, length - 1).SequenceEqual(span.TrimEnd()));

                Memory<char> mem = new Memory<char>(a);
                Assert.True(mem.Slice(1, length - 2).Span.SequenceEqual(mem.Trim().Span));
                Assert.True(mem.Slice(1).Span.SequenceEqual(mem.TrimStart().Span));
                Assert.True(mem.Slice(0, length - 1).Span.SequenceEqual(mem.TrimEnd().Span));

                ReadOnlyMemory<char> rom = new ReadOnlyMemory<char>(a);
                Assert.True(rom.Slice(1, length - 2).Span.SequenceEqual(rom.Trim().Span));
                Assert.True(rom.Slice(1).Span.SequenceEqual(rom.TrimStart().Span));
                Assert.True(rom.Slice(0, length - 1).Span.SequenceEqual(rom.TrimEnd().Span));
            }
        }

        [Fact]
        public static void WhiteSpaceInMiddleTrim_Memory()
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

                ReadOnlySpan<char> ros = s1.AsSpan();
                Assert.True(ros.SequenceEqual(ros.Trim()));
                Assert.True(ros.SequenceEqual(ros.TrimStart()));
                Assert.True(ros.SequenceEqual(ros.TrimEnd()));

                Span<char> span = new Span<char>(a);
                Assert.True(span.SequenceEqual(span.Trim()));
                Assert.True(span.SequenceEqual(span.TrimStart()));
                Assert.True(span.SequenceEqual(span.TrimEnd()));

                Memory<char> mem = new Memory<char>(a);
                Assert.True(mem.Span.SequenceEqual(mem.Trim().Span));
                Assert.True(mem.Span.SequenceEqual(mem.TrimStart().Span));
                Assert.True(mem.Span.SequenceEqual(mem.TrimEnd().Span));

                ReadOnlyMemory<char> rom = new ReadOnlyMemory<char>(a);
                Assert.True(rom.Span.SequenceEqual(rom.Trim().Span));
                Assert.True(rom.Span.SequenceEqual(rom.TrimStart().Span));
                Assert.True(rom.Span.SequenceEqual(rom.TrimEnd().Span));
            }
        }

        [Fact]
        public static void TrimWhiteSpaceMultipleTimes_Memory()
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

                // ReadOnlySpan
                {
                    ReadOnlySpan<char> ros = s1.AsSpan();
                    ReadOnlySpan<char> trimResult = ros.Trim();
                    ReadOnlySpan<char> trimStartResult = ros.TrimStart();
                    ReadOnlySpan<char> trimEndResult = ros.TrimEnd();
                    Assert.True(ros.Slice(1, length - 2).SequenceEqual(trimResult));
                    Assert.True(ros.Slice(1).SequenceEqual(trimStartResult));
                    Assert.True(ros.Slice(0, length - 1).SequenceEqual(trimEndResult));

                    // 2nd attempt should do nothing
                    Assert.True(trimResult.SequenceEqual(trimResult.Trim()));
                    Assert.True(trimStartResult.SequenceEqual(trimStartResult.TrimStart()));
                    Assert.True(trimEndResult.SequenceEqual(trimEndResult.TrimEnd()));
                }

                // Span
                {
                    Span<char> span = new Span<char>(a);
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
                    Memory<char> mem = new Memory<char>(a);
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
                    ReadOnlyMemory<char> rom = new ReadOnlyMemory<char>(a);
                    ReadOnlyMemory<char> trimResult = rom.Trim();
                    ReadOnlyMemory<char> trimStartResult = rom.TrimStart();
                    ReadOnlyMemory<char> trimEndResult = rom.TrimEnd();
                    Assert.True(rom.Slice(1, length - 2).Span.SequenceEqual(trimResult.Span));
                    Assert.True(rom.Slice(1).Span.SequenceEqual(trimStartResult.Span));
                    Assert.True(rom.Slice(0, length - 1).Span.SequenceEqual(trimEndResult.Span));

                    // 2nd attempt should do nothing
                    Assert.True(trimResult.Span.SequenceEqual(trimResult.Trim().Span));
                    Assert.True(trimStartResult.Span.SequenceEqual(trimStartResult.TrimStart().Span));
                    Assert.True(trimEndResult.Span.SequenceEqual(trimEndResult.TrimEnd().Span));
                }
            }
        }

        [Fact]
        public static void MakeSureNoTrimChecksGoOutOfRange_Memory()
        {
            for (int length = 3; length < 64; length++)
            {
                char[] first = new char[length];
                first[0] = ' ';
                first[length - 1] = ' ';
                string s1 = new string(first, 1, length - 2);

                ReadOnlySpan<char> ros = s1.AsSpan();
                Assert.True(ros.SequenceEqual(ros.Trim()));
                Assert.True(ros.SequenceEqual(ros.TrimStart()));
                Assert.True(ros.SequenceEqual(ros.TrimEnd()));

                Span<char> span = new Span<char>(s1.ToCharArray());
                Assert.True(span.SequenceEqual(span.Trim()));
                Assert.True(span.SequenceEqual(span.TrimStart()));
                Assert.True(span.SequenceEqual(span.TrimEnd()));

                Memory<char> mem = new Memory<char>(s1.ToCharArray());
                Assert.True(mem.Span.SequenceEqual(mem.Trim().Span));
                Assert.True(mem.Span.SequenceEqual(mem.TrimStart().Span));
                Assert.True(mem.Span.SequenceEqual(mem.TrimEnd().Span));

                ReadOnlyMemory<char> rom = new ReadOnlyMemory<char>(s1.ToCharArray());
                Assert.True(rom.Span.SequenceEqual(rom.Trim().Span));
                Assert.True(rom.Span.SequenceEqual(rom.TrimStart().Span));
                Assert.True(rom.Span.SequenceEqual(rom.TrimEnd().Span));
            }
        }
    }

    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public static partial class MemoryExtensions
    {
        /// <summary>
        /// Removes all leading and trailing occurrences of a specified element from the memory.
        /// </summary>
        /// <param name="memory">The source memory from which the element is removed.</param>
        /// <param name="trimElement">The specified element to look for and remove.</param>
        public static Memory<T> Trim<T>(this Memory<T> memory, T trimElement)
            where T : IEquatable<T>
        {
            ReadOnlySpan<T> span = memory.Span;
            int start = ClampStart(span, trimElement);
            int length = ClampEnd(span, start, trimElement);
            return memory.Slice(start, length);
        }

        /// <summary>
        /// Removes all leading occurrences of a specified element from the memory.
        /// </summary>
        /// <param name="memory">The source memory from which the element is removed.</param>
        /// <param name="trimElement">The specified element to look for and remove.</param>
        public static Memory<T> TrimStart<T>(this Memory<T> memory, T trimElement)
            where T : IEquatable<T>
            => memory.Slice(ClampStart(memory.Span, trimElement));

        /// <summary>
        /// Removes all trailing occurrences of a specified element from the memory.
        /// </summary>
        /// <param name="memory">The source memory from which the element is removed.</param>
        /// <param name="trimElement">The specified element to look for and remove.</param>
        public static Memory<T> TrimEnd<T>(this Memory<T> memory, T trimElement)
            where T : IEquatable<T>
            => memory.Slice(0, ClampEnd(memory.Span, 0, trimElement));

        /// <summary>
        /// Removes all leading and trailing occurrences of a specified element from the memory.
        /// </summary>
        /// <param name="memory">The source memory from which the element is removed.</param>
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
        /// Removes all leading occurrences of a specified element from the memory.
        /// </summary>
        /// <param name="memory">The source memory from which the element is removed.</param>
        /// <param name="trimElement">The specified element to look for and remove.</param>
        public static ReadOnlyMemory<T> TrimStart<T>(this ReadOnlyMemory<T> memory, T trimElement)
            where T : IEquatable<T>
            => memory.Slice(ClampStart(memory.Span, trimElement));

        /// <summary>
        /// Removes all trailing occurrences of a specified element from the memory.
        /// </summary>
        /// <param name="memory">The source memory from which the element is removed.</param>
        /// <param name="trimElement">The specified element to look for and remove.</param>
        public static ReadOnlyMemory<T> TrimEnd<T>(this ReadOnlyMemory<T> memory, T trimElement)
            where T : IEquatable<T>
            => memory.Slice(0, ClampEnd(memory.Span, 0, trimElement));

        /// <summary>
        /// Removes all leading and trailing occurrences of a specified element from the span.
        /// </summary>
        /// <param name="span">The source span from which the element is removed.</param>
        /// <param name="trimElement">The specified element to look for and remove.</param>
        public static Span<T> Trim<T>(this Span<T> span, T trimElement)
            where T : IEquatable<T>
        {
            int start = ClampStart(span, trimElement);
            int length = ClampEnd(span, start, trimElement);
            return span.Slice(start, length);
        }

        /// <summary>
        /// Removes all leading occurrences of a specified element from the span.
        /// </summary>
        /// <param name="span">The source span from which the element is removed.</param>
        /// <param name="trimElement">The specified element to look for and remove.</param>
        public static Span<T> TrimStart<T>(this Span<T> span, T trimElement)
            where T : IEquatable<T>
            => span.Slice(ClampStart(span, trimElement));

        /// <summary>
        /// Removes all trailing occurrences of a specified element from the span.
        /// </summary>
        /// <param name="span">The source span from which the element is removed.</param>
        /// <param name="trimElement">The specified element to look for and remove.</param>
        public static Span<T> TrimEnd<T>(this Span<T> span, T trimElement)
            where T : IEquatable<T>
            => span.Slice(0, ClampEnd(span, 0, trimElement));

        /// <summary>
        /// Removes all leading and trailing occurrences of a specified element from the span.
        /// </summary>
        /// <param name="span">The source span from which the element is removed.</param>
        /// <param name="trimElement">The specified element to look for and remove.</param>
        public static ReadOnlySpan<T> Trim<T>(this ReadOnlySpan<T> span, T trimElement)
            where T : IEquatable<T>
        {
            int start = ClampStart(span, trimElement);
            int length = ClampEnd(span, start, trimElement);
            return span.Slice(start, length);
        }

        /// <summary>
        /// Removes all leading occurrences of a specified element from the span.
        /// </summary>
        /// <param name="span">The source span from which the element is removed.</param>
        /// <param name="trimElement">The specified element to look for and remove.</param>
        public static ReadOnlySpan<T> TrimStart<T>(this ReadOnlySpan<T> span, T trimElement)
            where T : IEquatable<T>
            => span.Slice(ClampStart(span, trimElement));

        /// <summary>
        /// Removes all trailing occurrences of a specified element from the span.
        /// </summary>
        /// <param name="span">The source span from which the element is removed.</param>
        /// <param name="trimElement">The specified element to look for and remove.</param>
        public static ReadOnlySpan<T> TrimEnd<T>(this ReadOnlySpan<T> span, T trimElement)
            where T : IEquatable<T>
            => span.Slice(0, ClampEnd(span, 0, trimElement));

        /// <summary>
        /// Delimits all leading occurrences of a specified element from the span.
        /// </summary>
        /// <param name="span">The source span from which the element is removed.</param>
        /// <param name="trimElement">The specified element to look for and remove.</param>
        private static int ClampStart<T>(ReadOnlySpan<T> span, T trimElement)
            where T : IEquatable<T>
        {
            int start = 0;

            if (trimElement != null)
            {
                for (; start < span.Length; start++)
                {
                    if (!trimElement.Equals(span[start]))
                    {
                        break;
                    }
                }
            }
            else
            {
                for (; start < span.Length; start++)
                {
                    if (span[start] != null)
                    {
                        break;
                    }
                }
            }

            return start;
        }

        /// <summary>
        /// Delimits all trailing occurrences of a specified element from the span.
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

            if (trimElement != null)
            {
                for (; end >= start; end--)
                {
                    if (!trimElement.Equals(span[end]))
                    {
                        break;
                    }
                }
            }
            else
            {
                for (; end >= start; end--)
                {
                    if (span[end] != null)
                    {
                        break;
                    }
                }
            }

            return end - start + 1;
        }

        /// <summary>
        /// Removes all leading and trailing occurrences of a set of elements specified
        /// in a readonly span from the memory.
        /// </summary>
        /// <param name="memory">The source memory from which the elements are removed.</param>
        /// <param name="trimElements">The span which contains the set of elements to remove.</param>
        /// <remarks>If <paramref name="trimElements"/> is empty, the memory is returned unaltered.</remarks>
        public static Memory<T> Trim<T>(this Memory<T> memory, ReadOnlySpan<T> trimElements)
            where T : IEquatable<T>
        {
            if (trimElements.Length > 1)
            {
                ReadOnlySpan<T> span = memory.Span;
                int start = ClampStart(span, trimElements);
                int length = ClampEnd(span, start, trimElements);
                return memory.Slice(start, length);
            }

            if (trimElements.Length == 1)
            {
                return Trim(memory, trimElements[0]);
            }

            return memory;
        }

        /// <summary>
        /// Removes all leading occurrences of a set of elements specified
        /// in a readonly span from the memory.
        /// </summary>
        /// <param name="memory">The source memory from which the elements are removed.</param>
        /// <param name="trimElements">The span which contains the set of elements to remove.</param>
        /// <remarks>If <paramref name="trimElements"/> is empty, the memory is returned unaltered.</remarks>
        public static Memory<T> TrimStart<T>(this Memory<T> memory, ReadOnlySpan<T> trimElements)
            where T : IEquatable<T>
        {
            if (trimElements.Length > 1)
            {
                return memory.Slice(ClampStart(memory.Span, trimElements));
            }

            if (trimElements.Length == 1)
            {
                return TrimStart(memory, trimElements[0]);
            }

            return memory;
        }

        /// <summary>
        /// Removes all trailing occurrences of a set of elements specified
        /// in a readonly span from the memory.
        /// </summary>
        /// <param name="memory">The source memory from which the elements are removed.</param>
        /// <param name="trimElements">The span which contains the set of elements to remove.</param>
        /// <remarks>If <paramref name="trimElements"/> is empty, the memory is returned unaltered.</remarks>
        public static Memory<T> TrimEnd<T>(this Memory<T> memory, ReadOnlySpan<T> trimElements)
            where T : IEquatable<T>
        {
            if (trimElements.Length > 1)
            {
                return memory.Slice(0, ClampEnd(memory.Span, 0, trimElements));
            }

            if (trimElements.Length == 1)
            {
                return TrimEnd(memory, trimElements[0]);
            }

            return memory;
        }

        /// <summary>
        /// Removes all leading and trailing occurrences of a set of elements specified
        /// in a readonly span from the memory.
        /// </summary>
        /// <param name="memory">The source memory from which the elements are removed.</param>
        /// <param name="trimElements">The span which contains the set of elements to remove.</param>
        /// <remarks>If <paramref name="trimElements"/> is empty, the memory is returned unaltered.</remarks>
        public static ReadOnlyMemory<T> Trim<T>(this ReadOnlyMemory<T> memory, ReadOnlySpan<T> trimElements)
            where T : IEquatable<T>
        {
            if (trimElements.Length > 1)
            {
                ReadOnlySpan<T> span = memory.Span;
                int start = ClampStart(span, trimElements);
                int length = ClampEnd(span, start, trimElements);
                return memory.Slice(start, length);
            }

            if (trimElements.Length == 1)
            {
                return Trim(memory, trimElements[0]);
            }

            return memory;
        }

        /// <summary>
        /// Removes all leading occurrences of a set of elements specified
        /// in a readonly span from the memory.
        /// </summary>
        /// <param name="memory">The source memory from which the elements are removed.</param>
        /// <param name="trimElements">The span which contains the set of elements to remove.</param>
        /// <remarks>If <paramref name="trimElements"/> is empty, the memory is returned unaltered.</remarks>
        public static ReadOnlyMemory<T> TrimStart<T>(this ReadOnlyMemory<T> memory, ReadOnlySpan<T> trimElements)
            where T : IEquatable<T>
        {
            if (trimElements.Length > 1)
            {
                return memory.Slice(ClampStart(memory.Span, trimElements));
            }

            if (trimElements.Length == 1)
            {
                return TrimStart(memory, trimElements[0]);
            }

            return memory;
        }

        /// <summary>
        /// Removes all trailing occurrences of a set of elements specified
        /// in a readonly span from the memory.
        /// </summary>
        /// <param name="memory">The source memory from which the elements are removed.</param>
        /// <param name="trimElements">The span which contains the set of elements to remove.</param>
        /// <remarks>If <paramref name="trimElements"/> is empty, the memory is returned unaltered.</remarks>
        public static ReadOnlyMemory<T> TrimEnd<T>(this ReadOnlyMemory<T> memory, ReadOnlySpan<T> trimElements)
            where T : IEquatable<T>
        {
            if (trimElements.Length > 1)
            {
                return memory.Slice(0, ClampEnd(memory.Span, 0, trimElements));
            }

            if (trimElements.Length == 1)
            {
                return TrimEnd(memory, trimElements[0]);
            }

            return memory;

        }

        /// <summary>
        /// Removes all leading and trailing occurrences of a set of elements specified
        /// in a readonly span from the span.
        /// </summary>
        /// <param name="span">The source span from which the elements are removed.</param>
        /// <param name="trimElements">The span which contains the set of elements to remove.</param>
        /// <remarks>If <paramref name="trimElements"/> is empty, the span is returned unaltered.</remarks>
        public static Span<T> Trim<T>(this Span<T> span, ReadOnlySpan<T> trimElements)
            where T : IEquatable<T>
        {
            if (trimElements.Length > 1)
            {
                int start = ClampStart(span, trimElements);
                int length = ClampEnd(span, start, trimElements);
                return span.Slice(start, length);
            }

            if (trimElements.Length == 1)
            {
                return Trim(span, trimElements[0]);
            }

            return span;
        }

        /// <summary>
        /// Removes all leading occurrences of a set of elements specified
        /// in a readonly span from the span.
        /// </summary>
        /// <param name="span">The source span from which the elements are removed.</param>
        /// <param name="trimElements">The span which contains the set of elements to remove.</param>
        /// <remarks>If <paramref name="trimElements"/> is empty, the span is returned unaltered.</remarks>
        public static Span<T> TrimStart<T>(this Span<T> span, ReadOnlySpan<T> trimElements)
            where T : IEquatable<T>
        {
            if (trimElements.Length > 1)
            {
                return span.Slice(ClampStart(span, trimElements));
            }

            if (trimElements.Length == 1)
            {
                return TrimStart(span, trimElements[0]);
            }

            return span;
        }

        /// <summary>
        /// Removes all trailing occurrences of a set of elements specified
        /// in a readonly span from the span.
        /// </summary>
        /// <param name="span">The source span from which the elements are removed.</param>
        /// <param name="trimElements">The span which contains the set of elements to remove.</param>
        /// <remarks>If <paramref name="trimElements"/> is empty, the span is returned unaltered.</remarks>
        public static Span<T> TrimEnd<T>(this Span<T> span, ReadOnlySpan<T> trimElements)
            where T : IEquatable<T>
        {
            if (trimElements.Length > 1)
            {
                return span.Slice(0, ClampEnd(span, 0, trimElements));
            }

            if (trimElements.Length == 1)
            {
                return TrimEnd(span, trimElements[0]);
            }

            return span;
        }

        /// <summary>
        /// Removes all leading and trailing occurrences of a set of elements specified
        /// in a readonly span from the span.
        /// </summary>
        /// <param name="span">The source span from which the elements are removed.</param>
        /// <param name="trimElements">The span which contains the set of elements to remove.</param>
        /// <remarks>If <paramref name="trimElements"/> is empty, the span is returned unaltered.</remarks>
        public static ReadOnlySpan<T> Trim<T>(this ReadOnlySpan<T> span, ReadOnlySpan<T> trimElements)
            where T : IEquatable<T>
        {
            if (trimElements.Length > 1)
            {
                int start = ClampStart(span, trimElements);
                int length = ClampEnd(span, start, trimElements);
                return span.Slice(start, length);
            }

            if (trimElements.Length == 1)
            {
                return Trim(span, trimElements[0]);
            }

            return span;
        }

        /// <summary>
        /// Removes all leading occurrences of a set of elements specified
        /// in a readonly span from the span.
        /// </summary>
        /// <param name="span">The source span from which the elements are removed.</param>
        /// <param name="trimElements">The span which contains the set of elements to remove.</param>
        /// <remarks>If <paramref name="trimElements"/> is empty, the span is returned unaltered.</remarks>
        public static ReadOnlySpan<T> TrimStart<T>(this ReadOnlySpan<T> span, ReadOnlySpan<T> trimElements)
            where T : IEquatable<T>
        {
            if (trimElements.Length > 1)
            {
                return span.Slice(ClampStart(span, trimElements));
            }

            if (trimElements.Length == 1)
            {
                return TrimStart(span, trimElements[0]);
            }

            return span;
        }

        /// <summary>
        /// Removes all trailing occurrences of a set of elements specified
        /// in a readonly span from the span.
        /// </summary>
        /// <param name="span">The source span from which the elements are removed.</param>
        /// <param name="trimElements">The span which contains the set of elements to remove.</param>
        /// <remarks>If <paramref name="trimElements"/> is empty, the span is returned unaltered.</remarks>
        public static ReadOnlySpan<T> TrimEnd<T>(this ReadOnlySpan<T> span, ReadOnlySpan<T> trimElements)
            where T : IEquatable<T>
        {
            if (trimElements.Length > 1)
            {
                return span.Slice(0, ClampEnd(span, 0, trimElements));
            }

            if (trimElements.Length == 1)
            {
                return TrimEnd(span, trimElements[0]);
            }

            return span;
        }

        /// <summary>
        /// Delimits all leading occurrences of a set of elements specified
        /// in a readonly span from the span.
        /// </summary>
        /// <param name="span">The source span from which the elements are removed.</param>
        /// <param name="trimElements">The span which contains the set of elements to remove.</param>
        private static int ClampStart<T>(ReadOnlySpan<T> span, ReadOnlySpan<T> trimElements)
            where T : IEquatable<T>
        {
            int start = 0;
            for (; start < span.Length; start++)
            {
                if (!trimElements.Contains(span[start]))
                {
                    break;
                }
            }

            return start;
        }

        /// <summary>
        /// Delimits all trailing occurrences of a set of elements specified
        /// in a readonly span from the span.
        /// </summary>
        /// <param name="span">The source span from which the elements are removed.</param>
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
                if (!trimElements.Contains(span[end]))
                {
                    break;
                }
            }

            return end - start + 1;
        }

        /// <summary>
        /// Removes all leading and trailing white-space characters from the memory.
        /// </summary>
        /// <param name="memory">The source memory from which the characters are removed.</param>
        public static Memory<char> Trim(this Memory<char> memory)
        {
            ReadOnlySpan<char> span = memory.Span;
            int start = ClampStart(span);
            int length = ClampEnd(span, start);
            return memory.Slice(start, length);
        }

        /// <summary>
        /// Removes all leading white-space characters from the memory.
        /// </summary>
        /// <param name="memory">The source memory from which the characters are removed.</param>
        public static Memory<char> TrimStart(this Memory<char> memory)
            => memory.Slice(ClampStart(memory.Span));

        /// <summary>
        /// Removes all trailing white-space characters from the memory.
        /// </summary>
        /// <param name="memory">The source memory from which the characters are removed.</param>
        public static Memory<char> TrimEnd(this Memory<char> memory)
            => memory.Slice(0, ClampEnd(memory.Span, 0));

        /// <summary>
        /// Removes all leading and trailing white-space characters from the memory.
        /// </summary>
        /// <param name="memory">The source memory from which the characters are removed.</param>
        public static ReadOnlyMemory<char> Trim(this ReadOnlyMemory<char> memory)
        {
            ReadOnlySpan<char> span = memory.Span;
            int start = ClampStart(span);
            int length = ClampEnd(span, start);
            return memory.Slice(start, length);
        }

        /// <summary>
        /// Removes all leading white-space characters from the memory.
        /// </summary>
        /// <param name="memory">The source memory from which the characters are removed.</param>
        public static ReadOnlyMemory<char> TrimStart(this ReadOnlyMemory<char> memory)
            => memory.Slice(ClampStart(memory.Span));

        /// <summary>
        /// Removes all trailing white-space characters from the memory.
        /// </summary>
        /// <param name="memory">The source memory from which the characters are removed.</param>
        public static ReadOnlyMemory<char> TrimEnd(this ReadOnlyMemory<char> memory)
            => memory.Slice(0, ClampEnd(memory.Span, 0));

        /// <summary>
        /// Removes all leading and trailing white-space characters from the span.
        /// </summary>
        /// <param name="span">The source span from which the characters are removed.</param>
        public static ReadOnlySpan<char> Trim(this ReadOnlySpan<char> span)
        {
            int start = 0;
            for (; start < span.Length; start++)
            {
                if (!char.IsWhiteSpace(span[start]))
                {
                    break;
                }
            }

            int end = span.Length - 1;
            for (; end > start; end--)
            {
                if (!char.IsWhiteSpace(span[end]))
                {
                    break;
                }
            }

            return span.Slice(start, end - start + 1);
        }

        /// <summary>
        /// Removes all leading white-space characters from the span.
        /// </summary>
        /// <param name="span">The source span from which the characters are removed.</param>
        public static ReadOnlySpan<char> TrimStart(this ReadOnlySpan<char> span)
        {
            int start = 0;
            for (; start < span.Length; start++)
            {
                if (!char.IsWhiteSpace(span[start]))
                {
                    break;
                }
            }

            return span.Slice(start);
        }

        /// <summary>
        /// Removes all trailing white-space characters from the span.
        /// </summary>
        /// <param name="span">The source span from which the characters are removed.</param>
        public static ReadOnlySpan<char> TrimEnd(this ReadOnlySpan<char> span)
        {
            int end = span.Length - 1;
            for (; end >= 0; end--)
            {
                if (!char.IsWhiteSpace(span[end]))
                {
                    break;
                }
            }

            return span.Slice(0, end + 1);
        }

        /// <summary>
        /// Removes all leading and trailing occurrences of a specified character from the span.
        /// </summary>
        /// <param name="span">The source span from which the character is removed.</param>
        /// <param name="trimChar">The specified character to look for and remove.</param>
        public static ReadOnlySpan<char> Trim(this ReadOnlySpan<char> span, char trimChar)
        {
            int start = 0;
            for (; start < span.Length; start++)
            {
                if (span[start] != trimChar)
                {
                    break;
                }
            }

            int end = span.Length - 1;
            for (; end > start; end--)
            {
                if (span[end] != trimChar)
                {
                    break;
                }
            }

            return span.Slice(start, end - start + 1);
        }

        /// <summary>
        /// Removes all leading occurrences of a specified character from the span.
        /// </summary>
        /// <param name="span">The source span from which the character is removed.</param>
        /// <param name="trimChar">The specified character to look for and remove.</param>
        public static ReadOnlySpan<char> TrimStart(this ReadOnlySpan<char> span, char trimChar)
        {
            int start = 0;
            for (; start < span.Length; start++)
            {
                if (span[start] != trimChar)
                {
                    break;
                }
            }

            return span.Slice(start);
        }

        /// <summary>
        /// Removes all trailing occurrences of a specified character from the span.
        /// </summary>
        /// <param name="span">The source span from which the character is removed.</param>
        /// <param name="trimChar">The specified character to look for and remove.</param>
        public static ReadOnlySpan<char> TrimEnd(this ReadOnlySpan<char> span, char trimChar)
        {
            int end = span.Length - 1;
            for (; end >= 0; end--)
            {
                if (span[end] != trimChar)
                {
                    break;
                }
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
            => span.TrimStart(trimChars).TrimEnd(trimChars);

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
                    {
                        goto Next;
                    }
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
                    {
                        goto Next;
                    }
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
        /// <param name="span">The source span from which the characters are removed.</param>
        public static Span<char> Trim(this Span<char> span)
        {
            int start = ClampStart(span);
            int length = ClampEnd(span, start);
            return span.Slice(start, length);
        }

        /// <summary>
        /// Removes all leading white-space characters from the span.
        /// </summary>
        /// <param name="span">The source span from which the characters are removed.</param>
        public static Span<char> TrimStart(this Span<char> span)
            => span.Slice(ClampStart(span));

        /// <summary>
        /// Removes all trailing white-space characters from the span.
        /// </summary>
        /// <param name="span">The source span from which the characters are removed.</param>
        public static Span<char> TrimEnd(this Span<char> span)
            => span.Slice(0, ClampEnd(span, 0));

        /// <summary>
        /// Delimits all leading occurrences of whitespace charecters from the span.
        /// </summary>
        /// <param name="span">The source span from which the characters are removed.</param>
        private static int ClampStart(ReadOnlySpan<char> span)
        {
            int start = 0;

            for (; start < span.Length; start++)
            {
                if (!char.IsWhiteSpace(span[start]))
                {
                    break;
                }
            }

            return start;
        }

        /// <summary>
        /// Delimits all trailing occurrences of whitespace charecters from the span.
        /// </summary>
        /// <param name="span">The source span from which the characters are removed.</param>
        /// <param name="start">The start index from which to being searching.</param>
        private static int ClampEnd(ReadOnlySpan<char> span, int start)
        {
            // Initially, start==len==0. If ClampStart trims all, start==len
            Debug.Assert((uint)start <= span.Length);

            int end = span.Length - 1;

            for (; end >= start; end--)
            {
                if (!char.IsWhiteSpace(span[end]))
                {
                    break;
                }
            }

            return end - start + 1;
        }
    }

    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    internal static class Shims
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Contains<T>(this ReadOnlySpan<T> span, T value)
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
    }
}
