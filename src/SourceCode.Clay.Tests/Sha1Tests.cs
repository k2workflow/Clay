#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xunit;

namespace SourceCode.Clay.Tests
{
    public static class Sha1Tests
    {
        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(When_check_default))]
        public static void When_check_default()
        {
            var expected = default(Sha1);

            var actual = Sha1.Parse(Sha1TestVectors.Zero);

            Assert.Equal(expected, actual);
            Assert.Equal(expected.GetHashCode(), actual.GetHashCode());
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(When_check_empty))]
        public static void When_check_empty()
        {
            var expected = Sha1.Parse(Sha1TestVectors.Empty);

            // Empty Array singleton
            var actual = Sha1.Hash(Array.Empty<byte>());
            Assert.Equal(expected, actual);
            Assert.Equal(expected.GetHashCode(), actual.GetHashCode());

            // Empty Array
            actual = Sha1.Hash(new byte[0]);
            Assert.Equal(expected, actual);
            Assert.Equal(expected.GetHashCode(), actual.GetHashCode());

            // Empty default ArraySegment
            actual = Sha1.Hash(default(ArraySegment<byte>));
            Assert.Equal(expected, actual);
            Assert.Equal(expected.GetHashCode(), actual.GetHashCode());

            // Empty new ArraySegment
            actual = Sha1.Hash(new ArraySegment<byte>(new byte[0], 0, 0));
            Assert.Equal(expected, actual);
            Assert.Equal(expected.GetHashCode(), actual.GetHashCode());

            // Empty default Span
            actual = Sha1.Hash(default(Span<byte>));
            Assert.Equal(expected, actual);
            Assert.Equal(expected.GetHashCode(), actual.GetHashCode());

            // Empty new Span
            actual = Sha1.Hash(new Span<byte>(new byte[0], 0, 0));
            Assert.Equal(expected, actual);
            Assert.Equal(expected.GetHashCode(), actual.GetHashCode());

            // Empty default ReadOnlySpan
            actual = Sha1.Hash(default(ReadOnlySpan<byte>));
            Assert.Equal(expected, actual);
            Assert.Equal(expected.GetHashCode(), actual.GetHashCode());

            // Empty new ReadOnlySpan
            actual = Sha1.Hash(new ReadOnlySpan<byte>(new byte[0], 0, 0));
            Assert.Equal(expected, actual);
            Assert.Equal(expected.GetHashCode(), actual.GetHashCode());

            // Empty String
            actual = Sha1.Hash(string.Empty);
            Assert.Equal(expected, actual);
            Assert.Equal(expected.GetHashCode(), actual.GetHashCode());
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(When_create_null_sha1))]
        public static void When_create_null_sha1()
        {
            var expected = default(Sha1);

            Assert.Throws<ArgumentOutOfRangeException>(() => new Sha1((byte[])null));
            Assert.Throws<ArgumentOutOfRangeException>(() => new Sha1(new Span<byte>()));
            Assert.Throws<ArgumentOutOfRangeException>(() => new Sha1(new Span<byte>(new byte[0])));
            Assert.Throws<ArgumentOutOfRangeException>(() => new Sha1(new Span<byte>(new byte[1] { 0 })));

            Assert.True(default == expected);
            Assert.False(default != expected);
            Assert.True(expected.Equals((object)expected));
            Assert.Equal(new KeyValuePair<string, string>("", Sha1TestVectors.Zero), expected.Split(0));
            Assert.Equal(new KeyValuePair<string, string>("00", Sha1TestVectors.Zero.Left(Sha1.HexLength - 2)), expected.Split(2));
            Assert.Equal(new KeyValuePair<string, string>(Sha1TestVectors.Zero, ""), expected.Split(Sha1.HexLength));

            // Null string
            Assert.Throws<ArgumentNullException>(() => Sha1.Hash((string)null));

            // Null bytes
            Assert.Throws<ArgumentNullException>(() => Sha1.Hash((byte[])null));

            Assert.Throws<ArgumentNullException>(() => Sha1.Hash(null, 0, 0));

            // Stream
            Assert.Throws<ArgumentNullException>(() => Sha1.Hash((Stream)null));

            // Roundtrip string
            var actual = Sha1.Parse(expected.ToString());
            Assert.Equal(expected, actual);
            Assert.Equal(expected.GetHashCode(), actual.GetHashCode());

            // Roundtrip formatted
            actual = Sha1.Parse(expected.ToString("D"));
            Assert.Equal(expected, actual);
            Assert.Equal(expected.GetHashCode(), actual.GetHashCode());

            actual = Sha1.Parse(expected.ToString("S"));
            Assert.Equal(expected, actual);
            Assert.Equal(expected.GetHashCode(), actual.GetHashCode());

            // With hex specifier
            actual = Sha1.Parse("0x" + expected.ToString("D"));
            Assert.Equal(expected, actual);
            Assert.Equal(expected.GetHashCode(), actual.GetHashCode());

            actual = Sha1.Parse("0x" + expected.ToString("S"));
            Assert.Equal(expected, actual);
            Assert.Equal(expected.GetHashCode(), actual.GetHashCode());
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(When_construct_sha1_from_Bytes))]
        public static void When_construct_sha1_from_Bytes()
        {
            var expected = Sha1.Hash("abc");
            var buffer = new byte[Sha1.ByteLength + 10];

            Assert.Throws<ArgumentOutOfRangeException>(() => new Sha1(new byte[Sha1.ByteLength - 1])); // Too short
            Assert.Throws<ArgumentOutOfRangeException>(() => new Sha1(new byte[Sha1.ByteLength].AsSpan().Slice(1))); // Bad offset

            // Construct Byte[]
            expected.CopyTo(buffer.AsSpan());
            var actual = new Sha1(buffer);
            Assert.Equal(expected, actual);
            Assert.Equal(expected.GetHashCode(), actual.GetHashCode());

            // Construct Byte[] with offset 0
            expected.CopyTo(buffer.AsSpan());
            actual = new Sha1(buffer);
            Assert.Equal(expected, actual);
            Assert.Equal(expected.GetHashCode(), actual.GetHashCode());

            // Construct Byte[] with offset N
            expected.CopyTo(buffer.AsSpan().Slice(5));
            actual = new Sha1(buffer.AsSpan().Slice(5));
            Assert.Equal(expected, actual);
            Assert.Equal(expected.GetHashCode(), actual.GetHashCode());
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(When_construct_sha1_from_ArraySegment))]
        public static void When_construct_sha1_from_ArraySegment()
        {
            var expected = Sha1.Hash("abc");
            var buffer = new byte[Sha1.ByteLength + 10];

            // Construct Byte[] with offset N
            expected.CopyTo(buffer.AsSpan().Slice(5));
            var actual = new Sha1(buffer.AsSpan().Slice(5));
            Assert.Equal(expected, actual);
            Assert.Equal(expected.GetHashCode(), actual.GetHashCode());

            // Construct Segment
            expected.CopyTo(buffer.AsSpan());
            var seg = new ArraySegment<byte>(buffer);
            actual = new Sha1(seg);
            Assert.Equal(expected, actual);
            Assert.Equal(expected.GetHashCode(), actual.GetHashCode());

            // Construct Segment with offset 0
            expected.CopyTo(buffer.AsSpan());
            seg = new ArraySegment<byte>(buffer, 0, Sha1.ByteLength);
            actual = new Sha1(seg);
            Assert.Equal(expected, actual);
            Assert.Equal(expected.GetHashCode(), actual.GetHashCode());

            // Construct Segment with offset N
            expected.CopyTo(buffer.AsSpan().Slice(5));
            seg = new ArraySegment<byte>(buffer, 5, Sha1.ByteLength);
            actual = new Sha1(seg);
            Assert.Equal(expected, actual);
            Assert.Equal(expected.GetHashCode(), actual.GetHashCode());
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(When_construct_sha1_from_Memory))]
        public static void When_construct_sha1_from_Memory()
        {
            var expected = Sha1.Hash("abc");
            var buffer = new byte[Sha1.ByteLength + 10];

            Assert.Throws<ArgumentOutOfRangeException>(() => new Sha1(Memory<byte>.Empty.Span)); // Empty
            Assert.Throws<ArgumentOutOfRangeException>(() => new Sha1(new Memory<byte>(new byte[Sha1.ByteLength - 1]).Span)); // Too short

            // Construct Memory
            expected.CopyTo(buffer.AsSpan());
            var mem = new Memory<byte>(buffer);
            var actual = new Sha1(mem.Span);
            Assert.Equal(expected, actual);
            Assert.Equal(expected.GetHashCode(), actual.GetHashCode());

            // Construct Memory with offset 0
            expected.CopyTo(buffer.AsSpan());
            mem = new Memory<byte>(buffer, 0, Sha1.ByteLength);
            actual = new Sha1(mem.Span);
            Assert.Equal(expected, actual);
            Assert.Equal(expected.GetHashCode(), actual.GetHashCode());

            // Construct Memory with offset N
            expected.CopyTo(buffer.AsSpan().Slice(5));
            mem = new Memory<byte>(buffer, 5, Sha1.ByteLength);
            actual = new Sha1(mem.Span);
            Assert.Equal(expected, actual);
            Assert.Equal(expected.GetHashCode(), actual.GetHashCode());
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(When_construct_sha1_from_ReadOnlyMemory))]
        public static void When_construct_sha1_from_ReadOnlyMemory()
        {
            var expected = Sha1.Hash("abc");
            var buffer = new byte[Sha1.ByteLength + 10];

            // Construct ReadOnlyMemory
            expected.CopyTo(buffer.AsSpan());
            var mem = new ReadOnlyMemory<byte>(buffer);
            var actual = new Sha1(mem.Span);
            Assert.Equal(expected, actual);
            Assert.Equal(expected.GetHashCode(), actual.GetHashCode());

            // Construct ReadOnlyMemory with offset 0
            expected.CopyTo(buffer.AsSpan());
            mem = new ReadOnlyMemory<byte>(buffer, 0, Sha1.ByteLength);
            actual = new Sha1(mem.Span);
            Assert.Equal(expected, actual);
            Assert.Equal(expected.GetHashCode(), actual.GetHashCode());

            // Construct ReadOnlyMemory with offset N
            expected.CopyTo(buffer.AsSpan().Slice(5));
            mem = new ReadOnlyMemory<byte>(buffer, 5, Sha1.ByteLength);
            actual = new Sha1(mem.Span);
            Assert.Equal(expected, actual);
            Assert.Equal(expected.GetHashCode(), actual.GetHashCode());
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(When_construct_sha1_from_Stream))]
        public static void When_construct_sha1_from_Stream()
        {
            var buffer = Encoding.UTF8.GetBytes(Guid.NewGuid().ToString());

            // Construct MemoryStream
            var mem = new MemoryStream(buffer);
            var expected = Sha1.Hash(buffer);
            var actual = Sha1.Hash(mem);
            Assert.Equal(expected, actual);
            Assert.Equal(expected.GetHashCode(), actual.GetHashCode());

            // Construct MemoryStream with offset 0
            mem = new MemoryStream(buffer, 0, buffer.Length);
            expected = Sha1.Hash(buffer, 0, buffer.Length);
            actual = Sha1.Hash(mem);
            Assert.Equal(expected, actual);
            Assert.Equal(expected.GetHashCode(), actual.GetHashCode());

            // Construct MemoryStream with offset N
            mem = new MemoryStream(buffer, 5, buffer.Length - 5);
            expected = Sha1.Hash(buffer, 5, buffer.Length - 5);
            actual = Sha1.Hash(mem);
            Assert.Equal(expected, actual);
            Assert.Equal(expected.GetHashCode(), actual.GetHashCode());
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(When_construct_sha1_from_Span))]
        public static void When_construct_sha1_from_Span()
        {
            var expected = Sha1.Hash("abc");
            var buffer = new byte[Sha1.ByteLength + 10];

            // Construct Span
            expected.CopyTo(buffer.AsSpan());
            var span = new Span<byte>(buffer);
            var actual = new Sha1(span);
            Assert.Equal(expected, actual);
            Assert.Equal(expected.GetHashCode(), actual.GetHashCode());

            // Construct Span with offset 0
            expected.CopyTo(buffer.AsSpan());
            span = new Span<byte>(buffer, 0, Sha1.ByteLength);
            actual = new Sha1(span);
            Assert.Equal(expected, actual);
            Assert.Equal(expected.GetHashCode(), actual.GetHashCode());

            // Construct Span with offset N
            expected.CopyTo(buffer.AsSpan().Slice(5));
            span = new Span<byte>(buffer, 5, Sha1.ByteLength);
            actual = new Sha1(span);
            Assert.Equal(expected, actual);
            Assert.Equal(expected.GetHashCode(), actual.GetHashCode());
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(When_copyto_with_null_buffer))]
        public static void When_copyto_with_null_buffer()
        {
            // Arrange
            var buffer = (byte[])null;
            var sha1 = new Sha1();

            // Action
            var actual = Assert.Throws<ArgumentException>(() => sha1.CopyTo(buffer.AsSpan()));
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(When_construct_sha1_from_ReadOnlySpan))]
        public static void When_construct_sha1_from_ReadOnlySpan()
        {
            var expected = Sha1.Hash("abc");
            var buffer = new byte[Sha1.ByteLength + 10];

            // Construct ReadOnlySpan
            expected.CopyTo(buffer.AsSpan());
            var span = new ReadOnlySpan<byte>(buffer);
            var actual = new Sha1(span);
            Assert.Equal(expected, actual);
            Assert.Equal(expected.GetHashCode(), actual.GetHashCode());

            // Construct ReadOnlySpan with offset 0
            expected.CopyTo(buffer.AsSpan());
            span = new ReadOnlySpan<byte>(buffer, 0, Sha1.ByteLength);
            actual = new Sha1(span);
            Assert.Equal(expected, actual);
            Assert.Equal(expected.GetHashCode(), actual.GetHashCode());

            // Construct ReadOnlySpan with offset N
            expected.CopyTo(buffer.AsSpan().Slice(5));
            span = new ReadOnlySpan<byte>(buffer, 5, Sha1.ByteLength);
            actual = new Sha1(span);
            Assert.Equal(expected, actual);
            Assert.Equal(expected.GetHashCode(), actual.GetHashCode());
        }

        [Trait("Type", "Unit")]
        [Theory(DisplayName = nameof(When_create_test_vectors))]
        [ClassData(typeof(Sha1TestVectors))] // http://www.di-mgt.com.au/sha_testvectors.html
        public static void When_create_test_vectors(string input, string expected)
        {
            var sha1 = Sha1.Parse(expected);
            {
                // String
                var actual = Sha1.Hash(input).ToString();
                Assert.Equal(expected, actual);
                Assert.Equal(expected.GetHashCode(), actual.GetHashCode());
                Assert.Equal(Sha1.HexLength, Encoding.UTF8.GetByteCount(actual));

                // Bytes
                var bytes = Encoding.UTF8.GetBytes(input);
                actual = Sha1.Hash(bytes).ToString();
                Assert.Equal(expected, actual);
                Assert.Equal(expected.GetHashCode(), actual.GetHashCode());

                actual = Sha1.Hash(bytes, 0, bytes.Length).ToString();
                Assert.Equal(expected, actual);
                Assert.Equal(expected.GetHashCode(), actual.GetHashCode());

                // Object
                Assert.True(expected.Equals((object)actual));

                // Roundtrip string
                actual = sha1.ToString();
                Assert.Equal(expected, actual);
                Assert.Equal(expected.GetHashCode(), actual.GetHashCode());

                // Roundtrip formatted
                actual = Sha1.Parse(sha1.ToString("D")).ToString();
                Assert.Equal(expected, actual);
                Assert.Equal(expected.GetHashCode(), actual.GetHashCode());

                actual = Sha1.Parse(sha1.ToString("S")).ToString();
                Assert.Equal(expected, actual);
                Assert.Equal(expected.GetHashCode(), actual.GetHashCode());

                // With hex specifier
                actual = Sha1.Parse("0x" + sha1.ToString("D")).ToString();
                Assert.Equal(expected, actual);
                Assert.Equal(expected.GetHashCode(), actual.GetHashCode());

                actual = Sha1.Parse("0x" + sha1.ToString("S")).ToString();
                Assert.Equal(expected, actual);
                Assert.Equal(expected.GetHashCode(), actual.GetHashCode());

                // Get Byte[]
                var buffer = new byte[Sha1.ByteLength];
                sha1.CopyTo(buffer.AsSpan());

                // Roundtrip Byte[]
                actual = new Sha1(buffer).ToString();
                Assert.Equal(expected, actual);
                Assert.Equal(expected.GetHashCode(), actual.GetHashCode());

                // Roundtrip Segment
                actual = new Sha1(new ArraySegment<byte>(buffer)).ToString();
                Assert.Equal(expected, actual);
                Assert.Equal(expected.GetHashCode(), actual.GetHashCode());
            }
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(When_create_sha1_from_empty_ArraySegment))]
        public static void When_create_sha1_from_empty_ArraySegment()
        {
            var expected = Sha1.Hash(Array.Empty<byte>());

            // Empty segment
            var actual = Sha1.Hash(default(ArraySegment<byte>));
            Assert.Equal(expected, actual);
            Assert.Equal(expected.GetHashCode(), actual.GetHashCode());
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(When_create_sha_from_bytes))]
        public static void When_create_sha_from_bytes()
        {
            var buffer = new byte[Sha1.ByteLength];
            var tinyBuffer = new byte[Sha1.ByteLength - 1];

            for (var i = 1; i < 200; i++)
            {
                var str = new string(char.MinValue, i);
                var byt = Encoding.UTF8.GetBytes(str);
                var sha = Sha1.Hash(byt);

                Assert.NotEqual(default, sha);
                Assert.Equal(Sha1.HexLength, Encoding.UTF8.GetByteCount(sha.ToString()));

                var copied = sha.TryCopyTo(buffer);
                Assert.True(copied);

                var shb = new Sha1(buffer);
                Assert.Equal(sha, shb);

                copied = sha.TryCopyTo(tinyBuffer);
                Assert.False(copied);
            }
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(When_create_sha_from_empty_string))]
        public static void When_create_sha_from_empty_string()
        {
            const string expected = Sha1TestVectors.Empty; // http://www.di-mgt.com.au/sha_testvectors.html
            var sha1 = Sha1.Parse(expected);
            {
                const string input = "";

                // String
                var actual = Sha1.Hash(input).ToString();
                Assert.Equal(expected, actual);
                Assert.Equal(Sha1.HexLength, Encoding.UTF8.GetByteCount(actual));

                // Empty array
                actual = Sha1.Hash(Array.Empty<byte>()).ToString();
                Assert.Equal(expected, actual);

                actual = Sha1.Hash(Array.Empty<byte>(), 0, 0).ToString();
                Assert.Equal(expected, actual);

                // Empty segment
                actual = Sha1.Hash(new ArraySegment<byte>(Array.Empty<byte>())).ToString();
                Assert.Equal(expected, actual);

                // Bytes
                var bytes = Encoding.UTF8.GetBytes(input);
                actual = Sha1.Hash(bytes).ToString();
                Assert.Equal(expected, actual);

                // Object
                Assert.True(expected.Equals((object)actual));

                // Roundtrip string
                actual = sha1.ToString();
                Assert.Equal(expected, actual);

                // Roundtrip formatted
                actual = Sha1.Parse(sha1.ToString("D")).ToString();
                Assert.Equal(expected, actual);

                actual = Sha1.Parse(sha1.ToString("S")).ToString();
                Assert.Equal(expected, actual);

                // With hex specifier
                actual = Sha1.Parse("0x" + sha1.ToString("D")).ToString();
                Assert.Equal(expected, actual);

                actual = Sha1.Parse("0x" + sha1.ToString("S")).ToString();
                Assert.Equal(expected, actual);

                // Get Byte[]
                var buffer = new byte[Sha1.ByteLength];
                sha1.CopyTo(buffer.AsSpan());

                // Roundtrip Byte[]
                actual = new Sha1(buffer).ToString();
                Assert.Equal(expected, actual);

                // Roundtrip Segment
                actual = new Sha1(new ArraySegment<byte>(buffer)).ToString();
                Assert.Equal(expected, actual);
            }
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(When_create_sha_from_narrow_string))]
        public static void When_create_sha_from_narrow_string()
        {
            for (var i = 1; i < 200; i++)
            {
                var str = new string(char.MinValue, i);
                var sha1 = Sha1.Hash(str);

                Assert.NotEqual(default, sha1);
                Assert.Equal(Sha1.HexLength, Encoding.UTF8.GetByteCount(sha1.ToString()));
            }
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(When_create_sha_from_wide_string_1))]
        public static void When_create_sha_from_wide_string_1()
        {
            for (var i = 1; i < 200; i++)
            {
                var str = new string(char.MaxValue, i);
                var sha1 = Sha1.Hash(str);

                Assert.NotEqual(default, sha1);
                Assert.Equal(Sha1.HexLength, Encoding.UTF8.GetByteCount(sha1.ToString()));
            }
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(When_create_sha_from_wide_string_2))]
        public static void When_create_sha_from_wide_string_2()
        {
            var str = string.Empty;
            for (var i = 1; i < 200; i++)
            {
                str += TestConstants.SurrogatePair;
                var sha1 = Sha1.Hash(str);

                Assert.NotEqual(default, sha1);
                Assert.Equal(Sha1.HexLength, Encoding.UTF8.GetByteCount(sha1.ToString()));
            }
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(When_format_sha1))]
        public static void When_format_sha1()
        {
            // http://www.di-mgt.com.au/sha_testvectors.html

            const string expected_N = "a9993e364706816aba3e25717850c26c9cd0d89d";
            var sha1 = Sha1.Parse(expected_N);

            // ("N")
            {
                var actual = sha1.ToString(); // Default format
                Assert.Equal(expected_N, actual);

                actual = sha1.ToString("N");
                Assert.Equal(expected_N, actual);

                actual = sha1.ToString("n");
                Assert.Equal(expected_N, actual);
            }

            // ("D")
            {
                const string expected_D = "a9993e36-4706816a-ba3e2571-7850c26c-9cd0d89d";

                var actual = sha1.ToString("D");
                Assert.Equal(expected_D, actual);

                actual = sha1.ToString("d");
                Assert.Equal(expected_D, actual);
            }

            // ("S")
            {
                const string expected_S = "a9993e36 4706816a ba3e2571 7850c26c 9cd0d89d";

                var actual = sha1.ToString("S");
                Assert.Equal(expected_S, actual);

                actual = sha1.ToString("s");
                Assert.Equal(expected_S, actual);
            }

            // (null)
            {
                Assert.Throws<FormatException>(() => sha1.ToString(null));
            }

            // ("")
            {
                Assert.Throws<FormatException>(() => sha1.ToString(""));
            }

            // (" ")
            {
                Assert.Throws<FormatException>(() => sha1.ToString(" "));
            }

            // ("x")
            {
                Assert.Throws<FormatException>(() => sha1.ToString("x"));
            }

            // ("ss")
            {
                Assert.Throws<FormatException>(() => sha1.ToString("nn"));
            }
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(When_parse_sha1))]
        public static void When_parse_sha1()
        {
            // http://www.di-mgt.com.au/sha_testvectors.html

            const string expected_N = "a9993e364706816aba3e25717850c26c9cd0d89d";
            var sha1 = Sha1.Parse(expected_N);

            Assert.True(Sha1.TryParse(expected_N, out _));
            Assert.True(Sha1.TryParse(expected_N.AsSpan(), out _));

            Assert.True(Sha1.TryParse("0x" + expected_N, out _));
            Assert.True(Sha1.TryParse("0X" + expected_N, out _));

            Assert.False(Sha1.TryParse(expected_N.Substring(10), out _));
            Assert.False(Sha1.TryParse("0x" + expected_N.Substring(10), out _));
            Assert.False(Sha1.TryParse("0X" + expected_N.Substring(10), out _));

            Assert.False(Sha1.TryParse("0x" + expected_N.Substring(Sha1.HexLength - 2), out _));
            Assert.False(Sha1.TryParse("0X" + expected_N.Substring(Sha1.HexLength - 2), out _));

            Assert.False(Sha1.TryParse(expected_N.Replace('8', 'G'), out _));
            Assert.False(Sha1.TryParse(expected_N.Replace('8', 'G').AsSpan(), out _));
            Assert.Throws<FormatException>(() => Sha1.Parse(expected_N.Replace('8', 'G').AsSpan()));

            Assert.False(Sha1.TryParse($"0x{new string('1', Sha1.HexLength - 2)}", out _));
            Assert.False(Sha1.TryParse($"0x{new string('1', Sha1.HexLength - 1)}", out _));
            Assert.True(Sha1.TryParse($"0x{new string('1', Sha1.HexLength)}", out _));

            // "N"
            {
                var actual = Sha1.Parse(sha1.ToString()); // Default format
                Assert.Equal(sha1, actual);

                actual = Sha1.Parse(sha1.ToString("N"));
                Assert.Equal(sha1, actual);

                actual = Sha1.Parse("0x" + sha1.ToString("N"));
                Assert.Equal(sha1, actual);

                Assert.Throws<FormatException>(() => Sha1.Parse(sha1.ToString("N") + "a"));
            }

            // "D"
            {
                var actual = Sha1.Parse(sha1.ToString("D"));
                Assert.Equal(sha1, actual);

                actual = Sha1.Parse("0x" + sha1.ToString("D"));
                Assert.Equal(sha1, actual);

                Assert.Throws<FormatException>(() => Sha1.Parse(sha1.ToString("D") + "a"));
            }

            // "S"
            {
                var actual = Sha1.Parse(sha1.ToString("S"));
                Assert.Equal(sha1, actual);

                actual = Sha1.Parse("0x" + sha1.ToString("S"));
                Assert.Equal(sha1, actual);

                Assert.Throws<FormatException>(() => Sha1.Parse(sha1.ToString("S") + "a"));
            }

            // null
            {
                Assert.Throws<ArgumentNullException>(() => Sha1.Parse((string)null));
            }

            // Empty
            {
                Assert.Throws<FormatException>(() => Sha1.Parse(""));
            }

            // Whitespace
            {
                Assert.Throws<FormatException>(() => Sha1.Parse(" "));
                Assert.Throws<FormatException>(() => Sha1.Parse("\t"));
                Assert.Throws<FormatException>(() => Sha1.Parse(" " + expected_N));
                Assert.Throws<FormatException>(() => Sha1.Parse(expected_N + " "));
            }

            // "0x"
            {
                Assert.Throws<FormatException>(() => Sha1.Parse("0x"));
            }
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(When_lexographic_compare_sha1))]
        public static void When_lexographic_compare_sha1()
        {
            var byt1 = new byte[20] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19 };
            var sha1 = new Sha1(byt1);
            var str1 = sha1.ToString();
            Assert.Equal("000102030405060708090a0b0c0d0e0f10111213", str1);

            for (var n = 0; n < 20; n++)
            {
                for (var i = n + 1; i <= byte.MaxValue; i++)
                {
                    // Bump blit[n]
                    byt1[n] = (byte)i;
                    var sha2 = new Sha1(byt1);
                    Assert.True(sha2 > sha1);
                }

                byt1[n] = (byte)n;
            }
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(When_compare_sha1))]
        public static void When_compare_sha1()
        {
            var sha1 = Sha1.Hash("abc"); // a9993e36-4706816a-ba3e2571-7850c26c-9cd0d89d
            Assert.True(default(Sha1) < sha1);
            Assert.True(sha1 > default(Sha1));

            var sha2 = sha1; // a9993e36-4706816a-ba3e2571-7850c26c-9cd0d89d
            Assert.True(sha2 <= sha1);
            Assert.True(sha2 >= sha1);
            Assert.False(sha2 < sha1);
            Assert.False(sha2 > sha1);

            var sha3 = Sha1.Hash("def"); // 589c2233-5a381f12-2d129225-f5c0ba30-56ed5811
            Assert.True(sha1.CompareTo(sha2) == 0);
            Assert.True(sha1.CompareTo(sha3) != 0);

            var span = new Span<byte>(new byte[Sha1.ByteLength]);
            sha1.CopyTo(span);
            span[Sha1.ByteLength - 1]++;
            var sha4 = new Sha1(span); // a9993e36-4706816a-ba3e2571-7850c26c-9cd0d89e
            Assert.True(sha4 >= sha1);
            Assert.True(sha4 > sha1);
            Assert.False(sha4 < sha1);
            Assert.False(sha4 <= sha1);
            Assert.True(sha1.CompareTo(sha4) < 0);

            var list = new[] { sha4, sha1, sha2, sha3 };
            var comparer = Sha1Comparer.Default;
            Array.Sort(list, comparer.Compare);
            Assert.True(list[0] < list[1]);
            Assert.True(list[1] == list[2]);
            Assert.True(list[3] > list[2]);
        }
    }
}
