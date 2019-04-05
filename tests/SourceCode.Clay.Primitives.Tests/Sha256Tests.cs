#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xunit;
using crypt = System.Security.Cryptography;

namespace SourceCode.Clay.Tests
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public static class Sha256Tests
    {
        private static readonly crypt.SHA256 s_sha256 = crypt.SHA256.Create();

        [Trait("Type", "Unit")]
        [Fact]
        public static void When_check_default()
        {
            var expected = default(Sha256);

            var actual = Sha256.Parse(Sha256TestVectors.Zero);

            Assert.Equal(expected, actual);
            Assert.Equal(expected.GetHashCode(), actual.GetHashCode());
        }

        [Trait("Type", "Unit")]
        [Fact]
        public static void When_check_empty()
        {
            var expected = Sha256.Parse(Sha256TestVectors.Empty);

            // Empty Array singleton
            Sha256 actual = s_sha256.HashData(Array.Empty<byte>());
            Assert.Equal(expected, actual);
            Assert.Equal(expected.GetHashCode(), actual.GetHashCode());

            // Empty Array
            actual = s_sha256.HashData(new byte[0]);
            Assert.Equal(expected, actual);
            Assert.Equal(expected.GetHashCode(), actual.GetHashCode());

            // Empty default ArraySegment
            actual = s_sha256.HashData(default(ArraySegment<byte>));
            Assert.Equal(expected, actual);
            Assert.Equal(expected.GetHashCode(), actual.GetHashCode());

            // Empty new ArraySegment
            actual = s_sha256.HashData(new ArraySegment<byte>(new byte[0], 0, 0));
            Assert.Equal(expected, actual);
            Assert.Equal(expected.GetHashCode(), actual.GetHashCode());

            // Empty default Span
            actual = s_sha256.HashData(default(Span<byte>));
            Assert.Equal(expected, actual);
            Assert.Equal(expected.GetHashCode(), actual.GetHashCode());

            // Empty new Span
            actual = s_sha256.HashData(new Span<byte>(new byte[0], 0, 0));
            Assert.Equal(expected, actual);
            Assert.Equal(expected.GetHashCode(), actual.GetHashCode());

            // Empty default ReadOnlySpan
            actual = s_sha256.HashData(default(ReadOnlySpan<byte>));
            Assert.Equal(expected, actual);
            Assert.Equal(expected.GetHashCode(), actual.GetHashCode());

            // Empty new ReadOnlySpan
            actual = s_sha256.HashData(new ReadOnlySpan<byte>(new byte[0], 0, 0));
            Assert.Equal(expected, actual);
            Assert.Equal(expected.GetHashCode(), actual.GetHashCode());

            // Empty String
            actual = s_sha256.HashData(string.Empty);
            Assert.Equal(expected, actual);
            Assert.Equal(expected.GetHashCode(), actual.GetHashCode());
        }

        [Trait("Type", "Unit")]
        [Fact]
        public static void When_create_null_sha256()
        {
            var expected = default(Sha256);

            Assert.Throws<ArgumentOutOfRangeException>(() => new Sha256(null));
            Assert.Throws<ArgumentOutOfRangeException>(() => new Sha256(new Span<byte>()));
            Assert.Throws<ArgumentOutOfRangeException>(() => new Sha256(new Span<byte>(new byte[0])));
            Assert.Throws<ArgumentOutOfRangeException>(() => new Sha256(new Span<byte>(new byte[1] { 0 })));

            Assert.True(default == expected);
            Assert.False(default != expected);
            Assert.True(expected.Equals((object)expected));
            Assert.Equal(new KeyValuePair<string, string>("", Sha256TestVectors.Zero), expected.Split(0, false));
            Assert.Equal(new KeyValuePair<string, string>("", Sha256TestVectors.Zero), expected.Split(0, true));
            Assert.Equal(new KeyValuePair<string, string>("00", Sha256TestVectors.Zero.Left(Sha256.HexLength - 2)), expected.Split(2, false));
            Assert.Equal(new KeyValuePair<string, string>("00", Sha256TestVectors.Zero.Left(Sha256.HexLength - 2).ToUpperInvariant()), expected.Split(2, true));
            Assert.Equal(new KeyValuePair<string, string>(Sha256TestVectors.Zero, ""), expected.Split(Sha256.HexLength, false));
            Assert.Equal(new KeyValuePair<string, string>(Sha256TestVectors.Zero, ""), expected.Split(Sha256.HexLength, true));

            // Null string
            Assert.Throws<ArgumentNullException>(() => s_sha256.HashData((string)null));

            // Null bytes
            Assert.Throws<ArgumentNullException>(() => s_sha256.HashData((byte[])null));

            Assert.Throws<ArgumentNullException>(() => s_sha256.HashData(null, 0, 0));

            // Stream
            Assert.Throws<ArgumentNullException>(() => s_sha256.HashData((Stream)null));

            // Roundtrip string
            var actual = Sha256.Parse(expected.ToString());
            Assert.Equal(expected, actual);
            Assert.Equal(expected.GetHashCode(), actual.GetHashCode());

            // Roundtrip formatted
            actual = Sha256.Parse(expected.ToString("D"));
            Assert.Equal(expected, actual);
            Assert.Equal(expected.GetHashCode(), actual.GetHashCode());

            actual = Sha256.Parse(expected.ToString("S"));
            Assert.Equal(expected, actual);
            Assert.Equal(expected.GetHashCode(), actual.GetHashCode());

            // With hex specifier
            actual = Sha256.Parse("0x" + expected.ToString("D"));
            Assert.Equal(expected, actual);
            Assert.Equal(expected.GetHashCode(), actual.GetHashCode());

            actual = Sha256.Parse("0x" + expected.ToString("S"));
            Assert.Equal(expected, actual);
            Assert.Equal(expected.GetHashCode(), actual.GetHashCode());
        }

        [Trait("Type", "Unit")]
        [Fact]
        public static void When_construct_sha256_from_Bytes()
        {
            Sha256 expected = s_sha256.HashData("abc");
            byte[] buffer = new byte[Sha256.ByteLength + 10];

            Assert.Throws<ArgumentOutOfRangeException>(() => new Sha256(new byte[Sha256.ByteLength - 1])); // Too short
            Assert.Throws<ArgumentOutOfRangeException>(() => new Sha256(new byte[Sha256.ByteLength].AsSpan().Slice(1))); // Bad offset

            // Construct Byte[]
            expected.CopyTo(buffer.AsSpan());
            var actual = new Sha256(buffer);
            Assert.Equal(expected, actual);
            Assert.Equal(expected.GetHashCode(), actual.GetHashCode());

            // Construct Byte[] with offset 0
            expected.CopyTo(buffer.AsSpan());
            actual = new Sha256(buffer);
            Assert.Equal(expected, actual);
            Assert.Equal(expected.GetHashCode(), actual.GetHashCode());

            // Construct Byte[] with offset N
            expected.CopyTo(buffer.AsSpan().Slice(5));
            actual = new Sha256(buffer.AsSpan().Slice(5));
            Assert.Equal(expected, actual);
            Assert.Equal(expected.GetHashCode(), actual.GetHashCode());
        }

        [Trait("Type", "Unit")]
        [Fact]
        public static void When_construct_sha256_from_ArraySegment()
        {
            Sha256 expected = s_sha256.HashData("abc");
            byte[] buffer = new byte[Sha256.ByteLength + 10];

            // Construct Byte[] with offset N
            expected.CopyTo(buffer.AsSpan().Slice(5));
            var actual = new Sha256(buffer.AsSpan().Slice(5));
            Assert.Equal(expected, actual);
            Assert.Equal(expected.GetHashCode(), actual.GetHashCode());

            // Construct Segment
            expected.CopyTo(buffer.AsSpan());
            var seg = new ArraySegment<byte>(buffer);
            actual = new Sha256(seg);
            Assert.Equal(expected, actual);
            Assert.Equal(expected.GetHashCode(), actual.GetHashCode());

            // Construct Segment with offset 0
            expected.CopyTo(buffer.AsSpan());
            seg = new ArraySegment<byte>(buffer, 0, Sha256.ByteLength);
            actual = new Sha256(seg);
            Assert.Equal(expected, actual);
            Assert.Equal(expected.GetHashCode(), actual.GetHashCode());

            // Construct Segment with offset N
            expected.CopyTo(buffer.AsSpan().Slice(5));
            seg = new ArraySegment<byte>(buffer, 5, Sha256.ByteLength);
            actual = new Sha256(seg);
            Assert.Equal(expected, actual);
            Assert.Equal(expected.GetHashCode(), actual.GetHashCode());
        }

        [Trait("Type", "Unit")]
        [Fact]
        public static void When_construct_sha256_from_Memory()
        {
            Sha256 expected = s_sha256.HashData("abc");
            byte[] buffer = new byte[Sha256.ByteLength + 10];

            Assert.Throws<ArgumentOutOfRangeException>(() => new Sha256(Memory<byte>.Empty.Span)); // Empty
            Assert.Throws<ArgumentOutOfRangeException>(() => new Sha256(new Memory<byte>(new byte[Sha256.ByteLength - 1]).Span)); // Too short

            // Construct Memory
            expected.CopyTo(buffer.AsSpan());
            var mem = new Memory<byte>(buffer);
            var actual = new Sha256(mem.Span);
            Assert.Equal(expected, actual);
            Assert.Equal(expected.GetHashCode(), actual.GetHashCode());

            // Construct Memory with offset 0
            expected.CopyTo(buffer.AsSpan());
            mem = new Memory<byte>(buffer, 0, Sha256.ByteLength);
            actual = new Sha256(mem.Span);
            Assert.Equal(expected, actual);
            Assert.Equal(expected.GetHashCode(), actual.GetHashCode());

            // Construct Memory with offset N
            expected.CopyTo(buffer.AsSpan().Slice(5));
            mem = new Memory<byte>(buffer, 5, Sha256.ByteLength);
            actual = new Sha256(mem.Span);
            Assert.Equal(expected, actual);
            Assert.Equal(expected.GetHashCode(), actual.GetHashCode());
        }

        [Trait("Type", "Unit")]
        [Fact]
        public static void When_construct_sha256_from_ReadOnlyMemory()
        {
            Sha256 expected = s_sha256.HashData("abc");
            byte[] buffer = new byte[Sha256.ByteLength + 10];

            // Construct ReadOnlyMemory
            expected.CopyTo(buffer.AsSpan());
            var mem = new ReadOnlyMemory<byte>(buffer);
            var actual = new Sha256(mem.Span);
            Assert.Equal(expected, actual);
            Assert.Equal(expected.GetHashCode(), actual.GetHashCode());

            // Construct ReadOnlyMemory with offset 0
            expected.CopyTo(buffer.AsSpan());
            mem = new ReadOnlyMemory<byte>(buffer, 0, Sha256.ByteLength);
            actual = new Sha256(mem.Span);
            Assert.Equal(expected, actual);
            Assert.Equal(expected.GetHashCode(), actual.GetHashCode());

            // Construct ReadOnlyMemory with offset N
            expected.CopyTo(buffer.AsSpan().Slice(5));
            mem = new ReadOnlyMemory<byte>(buffer, 5, Sha256.ByteLength);
            actual = new Sha256(mem.Span);
            Assert.Equal(expected, actual);
            Assert.Equal(expected.GetHashCode(), actual.GetHashCode());
        }

        [Trait("Type", "Unit")]
        [Fact]
        public static void When_construct_sha256_from_Stream()
        {
            byte[] buffer = Encoding.UTF8.GetBytes(Guid.NewGuid().ToString());

            // Construct MemoryStream
            var mem = new MemoryStream(buffer);
            Sha256 expected = s_sha256.HashData(buffer);
            Sha256 actual = s_sha256.HashData(mem);
            Assert.Equal(expected, actual);
            Assert.Equal(expected.GetHashCode(), actual.GetHashCode());

            // Construct MemoryStream with offset 0
            mem = new MemoryStream(buffer, 0, buffer.Length);
            expected = s_sha256.HashData(buffer, 0, buffer.Length);
            actual = s_sha256.HashData(mem);
            Assert.Equal(expected, actual);
            Assert.Equal(expected.GetHashCode(), actual.GetHashCode());

            // Construct MemoryStream with offset N
            mem = new MemoryStream(buffer, 5, buffer.Length - 5);
            expected = s_sha256.HashData(buffer, 5, buffer.Length - 5);
            actual = s_sha256.HashData(mem);
            Assert.Equal(expected, actual);
            Assert.Equal(expected.GetHashCode(), actual.GetHashCode());
        }

        [Trait("Type", "Unit")]
        [Fact]
        public static void When_construct_sha256_from_Span()
        {
            Sha256 expected = s_sha256.HashData("abc");
            byte[] buffer = new byte[Sha256.ByteLength + 10];

            // Construct Span
            expected.CopyTo(buffer.AsSpan());
            var span = new Span<byte>(buffer);
            var actual = new Sha256(span);
            Assert.Equal(expected, actual);
            Assert.Equal(expected.GetHashCode(), actual.GetHashCode());

            // Construct Span with offset 0
            expected.CopyTo(buffer.AsSpan());
            span = new Span<byte>(buffer, 0, Sha256.ByteLength);
            actual = new Sha256(span);
            Assert.Equal(expected, actual);
            Assert.Equal(expected.GetHashCode(), actual.GetHashCode());

            // Construct Span with offset N
            expected.CopyTo(buffer.AsSpan().Slice(5));
            span = new Span<byte>(buffer, 5, Sha256.ByteLength);
            actual = new Sha256(span);
            Assert.Equal(expected, actual);
            Assert.Equal(expected.GetHashCode(), actual.GetHashCode());
        }

        [Trait("Type", "Unit")]
        [Fact]
        public static void When_copyto_with_null_buffer()
        {
            // Arrange
            byte[] buffer = null;
            var sha256 = new Sha256();

            // Action
            ArgumentException actual = Assert.Throws<ArgumentException>(() => sha256.CopyTo(buffer.AsSpan()));
        }

        [Trait("Type", "Unit")]
        [Fact]
        public static void When_construct_sha256_from_ReadOnlySpan()
        {
            Sha256 expected = s_sha256.HashData("abc");
            byte[] buffer = new byte[Sha256.ByteLength + 10];

            // Construct ReadOnlySpan
            expected.CopyTo(buffer.AsSpan());
            var span = new ReadOnlySpan<byte>(buffer);
            var actual = new Sha256(span);
            Assert.Equal(expected, actual);
            Assert.Equal(expected.GetHashCode(), actual.GetHashCode());

            // Construct ReadOnlySpan with offset 0
            expected.CopyTo(buffer.AsSpan());
            span = new ReadOnlySpan<byte>(buffer, 0, Sha256.ByteLength);
            actual = new Sha256(span);
            Assert.Equal(expected, actual);
            Assert.Equal(expected.GetHashCode(), actual.GetHashCode());

            // Construct ReadOnlySpan with offset N
            expected.CopyTo(buffer.AsSpan().Slice(5));
            span = new ReadOnlySpan<byte>(buffer, 5, Sha256.ByteLength);
            actual = new Sha256(span);
            Assert.Equal(expected, actual);
            Assert.Equal(expected.GetHashCode(), actual.GetHashCode());
        }

        [Trait("Type", "Unit")]
        [Theory]
        [ClassData(typeof(Sha256TestVectors))] // http://www.di-mgt.com.au/sha_testvectors.html
        public static void When_create_test_vectors(string input, string expected)
        {
            var sha256 = Sha256.Parse(expected);
            {
                // String
                string actual = s_sha256.HashData(input).ToString();
                Assert.Equal(expected, actual);
                Assert.Equal(expected.GetHashCode(), actual.GetHashCode());
                Assert.Equal(Sha256.HexLength, Encoding.UTF8.GetByteCount(actual));

                // Bytes
                byte[] bytes = Encoding.UTF8.GetBytes(input);
                actual = s_sha256.HashData(bytes).ToString();
                Assert.Equal(expected, actual);
                Assert.Equal(expected.GetHashCode(), actual.GetHashCode());

                actual = s_sha256.HashData(bytes, 0, bytes.Length).ToString();
                Assert.Equal(expected, actual);
                Assert.Equal(expected.GetHashCode(), actual.GetHashCode());

                // Object
                Assert.True(expected.Equals((object)actual));

                // Roundtrip string
                actual = sha256.ToString();
                Assert.Equal(expected, actual);
                Assert.Equal(expected.GetHashCode(), actual.GetHashCode());

                // Roundtrip formatted
                actual = Sha256.Parse(sha256.ToString("D")).ToString();
                Assert.Equal(expected, actual);
                Assert.Equal(expected.GetHashCode(), actual.GetHashCode());

                actual = Sha256.Parse(sha256.ToString("S")).ToString();
                Assert.Equal(expected, actual);
                Assert.Equal(expected.GetHashCode(), actual.GetHashCode());

                // With hex specifier
                actual = Sha256.Parse("0x" + sha256.ToString("D")).ToString();
                Assert.Equal(expected, actual);
                Assert.Equal(expected.GetHashCode(), actual.GetHashCode());

                actual = Sha256.Parse("0x" + sha256.ToString("S")).ToString();
                Assert.Equal(expected, actual);
                Assert.Equal(expected.GetHashCode(), actual.GetHashCode());

                // Get Byte[]
                byte[] buffer = new byte[Sha256.ByteLength];
                sha256.CopyTo(buffer.AsSpan());

                // Roundtrip Byte[]
                actual = new Sha256(buffer).ToString();
                Assert.Equal(expected, actual);
                Assert.Equal(expected.GetHashCode(), actual.GetHashCode());

                // Roundtrip Segment
                actual = new Sha256(new ArraySegment<byte>(buffer)).ToString();
                Assert.Equal(expected, actual);
                Assert.Equal(expected.GetHashCode(), actual.GetHashCode());
            }
        }

        [Trait("Type", "Unit")]
        [Fact]
        public static void When_create_sha256_from_empty_ArraySegment()
        {
            Sha256 expected = s_sha256.HashData(Array.Empty<byte>());

            // Empty segment
            Sha256 actual = s_sha256.HashData(default(ArraySegment<byte>));
            Assert.Equal(expected, actual);
            Assert.Equal(expected.GetHashCode(), actual.GetHashCode());
        }

        [Trait("Type", "Unit")]
        [Fact]
        public static void When_create_sha_from_bytes()
        {
            byte[] buffer = new byte[Sha256.ByteLength];
            byte[] tinyBuffer = new byte[Sha256.ByteLength - 1];

            for (int i = 1; i < 200; i++)
            {
                string str = new string(char.MinValue, i);
                byte[] byt = Encoding.UTF8.GetBytes(str);
                Sha256 sha = s_sha256.HashData(byt);

                Assert.NotEqual(default, sha);
                Assert.Equal(Sha256.HexLength, Encoding.UTF8.GetByteCount(sha.ToString()));

                bool copied = sha.TryCopyTo(buffer);
                Assert.True(copied);

                var shb = new Sha256(buffer);
                Assert.Equal(sha, shb);

                copied = sha.TryCopyTo(tinyBuffer);
                Assert.False(copied);
            }
        }

        [Trait("Type", "Unit")]
        [Fact]
        public static void When_create_sha_from_empty_string()
        {
            const string expected = Sha256TestVectors.Empty; // http://www.di-mgt.com.au/sha_testvectors.html
            var sha256 = Sha256.Parse(expected);
            {
                const string input = "";

                // String
                string actual = s_sha256.HashData(input).ToString();
                Assert.Equal(expected, actual);
                Assert.Equal(Sha256.HexLength, Encoding.UTF8.GetByteCount(actual));

                // Empty array
                actual = s_sha256.HashData(Array.Empty<byte>()).ToString();
                Assert.Equal(expected, actual);

                actual = s_sha256.HashData(Array.Empty<byte>(), 0, 0).ToString();
                Assert.Equal(expected, actual);

                // Empty segment
                actual = s_sha256.HashData(new ArraySegment<byte>(Array.Empty<byte>())).ToString();
                Assert.Equal(expected, actual);

                // Bytes
                byte[] bytes = Encoding.UTF8.GetBytes(input);
                actual = s_sha256.HashData(bytes).ToString();
                Assert.Equal(expected, actual);

                // Object
                Assert.True(expected.Equals((object)actual));

                // Roundtrip string
                actual = sha256.ToString();
                Assert.Equal(expected, actual);

                // Roundtrip formatted
                actual = Sha256.Parse(sha256.ToString("D")).ToString();
                Assert.Equal(expected, actual);

                actual = Sha256.Parse(sha256.ToString("S")).ToString();
                Assert.Equal(expected, actual);

                // With hex specifier
                actual = Sha256.Parse("0x" + sha256.ToString("D")).ToString();
                Assert.Equal(expected, actual);

                actual = Sha256.Parse("0x" + sha256.ToString("S")).ToString();
                Assert.Equal(expected, actual);

                // Get Byte[]
                byte[] buffer = new byte[Sha256.ByteLength];
                sha256.CopyTo(buffer.AsSpan());

                // Roundtrip Byte[]
                actual = new Sha256(buffer).ToString();
                Assert.Equal(expected, actual);

                // Roundtrip Segment
                actual = new Sha256(new ArraySegment<byte>(buffer)).ToString();
                Assert.Equal(expected, actual);
            }
        }

        [Trait("Type", "Unit")]
        [Fact]
        public static void When_create_sha_from_narrow_string()
        {
            for (int i = 1; i < 200; i++)
            {
                string str = new string(char.MinValue, i);
                Sha256 sha256 = s_sha256.HashData(str);

                Assert.NotEqual(default, sha256);
                Assert.Equal(Sha256.HexLength, Encoding.UTF8.GetByteCount(sha256.ToString()));
            }
        }

        [Trait("Type", "Unit")]
        [Fact]
        public static void When_create_sha_from_wide_string_1()
        {
            for (int i = 1; i < 200; i++)
            {
                string str = new string(char.MaxValue, i);
                Sha256 sha256 = s_sha256.HashData(str);

                Assert.NotEqual(default, sha256);
                Assert.Equal(Sha256.HexLength, Encoding.UTF8.GetByteCount(sha256.ToString()));
            }
        }

        [Trait("Type", "Unit")]
        [Fact]
        public static void When_create_sha_from_wide_string_2()
        {
            string str = string.Empty;
            for (int i = 1; i < 200; i++)
            {
                str += TestConstants.SurrogatePair;
                Sha256 sha256 = s_sha256.HashData(str);

                Assert.NotEqual(default, sha256);
                Assert.Equal(Sha256.HexLength, Encoding.UTF8.GetByteCount(sha256.ToString()));
            }
        }

        [Trait("Type", "Unit")]
        [Fact]
        public static void When_format_sha256()
        {
            // http://www.di-mgt.com.au/sha_testvectors.html

            const string expected_N = "cdc76e5c9914fb9281a1c7e284d73e67f1809a48a497200e046d39ccc7112cd0";
            var sha256 = Sha256.Parse(expected_N);

            // ("N")
            {
                string actual = sha256.ToString(); // Default format
                Assert.Equal(expected_N, actual);

                actual = sha256.ToString("N");
                Assert.Equal(expected_N.ToUpperInvariant(), actual);

                actual = sha256.ToString("n");
                Assert.Equal(expected_N, actual);
            }

            // ("D")
            {
                const string expected_D = "cdc76e5c-9914fb92-81a1c7e2-84d73e67-f1809a48-a497200e-046d39cc-c7112cd0";

                string actual = sha256.ToString("D");
                Assert.Equal(expected_D.ToUpperInvariant(), actual);

                actual = sha256.ToString("d");
                Assert.Equal(expected_D, actual);
            }

            // ("S")
            {
                const string expected_S = "cdc76e5c 9914fb92 81a1c7e2 84d73e67 f1809a48 a497200e 046d39cc c7112cd0";

                string actual = sha256.ToString("S");
                Assert.Equal(expected_S.ToUpperInvariant(), actual);

                actual = sha256.ToString("s");
                Assert.Equal(expected_S, actual);
            }

            // (null)
            {
                Assert.Throws<FormatException>(() => sha256.ToString(null));
            }

            // ("")
            {
                Assert.Throws<FormatException>(() => sha256.ToString(""));
            }

            // (" ")
            {
                Assert.Throws<FormatException>(() => sha256.ToString(" "));
            }

            // ("x")
            {
                Assert.Throws<FormatException>(() => sha256.ToString("x"));
            }

            // ("ss")
            {
                Assert.Throws<FormatException>(() => sha256.ToString("nn"));
            }
        }

        [Trait("Type", "Unit")]
        [Fact]
        public static void When_parse_sha256()
        {
            // http://www.di-mgt.com.au/sha_testvectors.html

            const string expected_N = "cdc76e5c9914fb9281a1c7e284d73e67f1809a48a497200e046d39ccc7112cd0";
            var sha256 = Sha256.Parse(expected_N);

            Assert.True(Sha256.TryParse(expected_N, out _));
            Assert.True(Sha256.TryParse(expected_N.AsSpan(), out _));

            Assert.True(Sha256.TryParse("0x" + expected_N, out _));
            Assert.True(Sha256.TryParse("0X" + expected_N, out _));

            Assert.False(Sha256.TryParse(expected_N.Substring(10), out _));
            Assert.False(Sha256.TryParse("0x" + expected_N.Substring(10), out _));
            Assert.False(Sha256.TryParse("0X" + expected_N.Substring(10), out _));

            Assert.False(Sha256.TryParse("0x" + expected_N.Substring(Sha256.HexLength - 2), out _));
            Assert.False(Sha256.TryParse("0X" + expected_N.Substring(Sha256.HexLength - 2), out _));

            Assert.False(Sha256.TryParse(expected_N.Replace('8', 'G'), out _));
            Assert.False(Sha256.TryParse(expected_N.Replace('8', 'G').AsSpan(), out _));
            Assert.Throws<FormatException>(() => Sha256.Parse(expected_N.Replace('8', 'G').AsSpan()));

            Assert.False(Sha256.TryParse($"0x{new string('1', Sha256.HexLength - 2)}", out _));
            Assert.False(Sha256.TryParse($"0x{new string('1', Sha256.HexLength - 1)}", out _));
            Assert.True(Sha256.TryParse($"0x{new string('1', Sha256.HexLength)}", out _));

            // "N"
            {
                var actual = Sha256.Parse(sha256.ToString()); // Default format
                Assert.Equal(sha256, actual);

                actual = Sha256.Parse(sha256.ToString("N"));
                Assert.Equal(sha256, actual);

                actual = Sha256.Parse("0x" + sha256.ToString("N"));
                Assert.Equal(sha256, actual);

                Assert.Throws<FormatException>(() => Sha256.Parse(sha256.ToString("N") + "a"));
            }

            // "D"
            {
                var actual = Sha256.Parse(sha256.ToString("D"));
                Assert.Equal(sha256, actual);

                actual = Sha256.Parse("0x" + sha256.ToString("D"));
                Assert.Equal(sha256, actual);

                Assert.Throws<FormatException>(() => Sha256.Parse(sha256.ToString("D") + "a"));
            }

            // "S"
            {
                var actual = Sha256.Parse(sha256.ToString("S"));
                Assert.Equal(sha256, actual);

                actual = Sha256.Parse("0x" + sha256.ToString("S"));
                Assert.Equal(sha256, actual);

                Assert.Throws<FormatException>(() => Sha256.Parse(sha256.ToString("S") + "a"));
            }

            // null
            {
                Assert.Throws<ArgumentNullException>(() => Sha256.Parse(null));
            }

            // Empty
            {
                Assert.Throws<FormatException>(() => Sha256.Parse(""));
            }

            // Whitespace
            {
                Assert.Throws<FormatException>(() => Sha256.Parse(" "));
                Assert.Throws<FormatException>(() => Sha256.Parse("\t"));
            }

            // "0x"
            {
                Assert.Throws<FormatException>(() => Sha256.Parse("0x"));
            }
        }

        [Trait("Type", "Unit")]
        [Fact]
        public static void When_lexographic_compare_sha256()
        {
            byte[] byt1 = new byte[32] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31 };
            var sha256 = new Sha256(byt1);
            string str1 = sha256.ToString();
            Assert.Equal("000102030405060708090a0b0c0d0e0f101112131415161718191a1b1c1d1e1f", str1);

            for (int n = 0; n < 20; n++)
            {
                for (int i = n + 1; i <= byte.MaxValue; i++)
                {
                    // Bump blit[n]
                    byt1[n] = (byte)i;
                    var sha2 = new Sha256(byt1);
                    Assert.True(sha2 > sha256);
                }

                byt1[n] = (byte)n;
            }
        }

        [Trait("Type", "Unit")]
        [Fact]
        public static void When_compare_sha256()
        {
            Sha256 sha256 = s_sha256.HashData("abc"); // ba7816bf-8f01cfea-414140de-5dae2223-b00361a3-96177a9c-b410ff61-f20015ad
            Assert.True(default(Sha256) < sha256);
            Assert.True(sha256 > default(Sha256));

            Sha256 sha2 = sha256; // ba7816bf-8f01cfea-414140de-5dae2223-b00361a3-96177a9c-b410ff61-f20015ad
            Assert.True(sha2 <= sha256);
            Assert.True(sha2 >= sha256);
            Assert.False(sha2 < sha256);
            Assert.False(sha2 > sha256);

            Sha256 sha3 = s_sha256.HashData("def"); // cb8379ac-2098aa16-5029e393-8a51da0b-cecfc008-fd6795f4-01178647-f96c5b34
            Assert.True(sha256.CompareTo(sha2) == 0);
            Assert.True(sha256.CompareTo(sha3) != 0);

            var span = new Span<byte>(new byte[Sha256.ByteLength]);
            sha256.CopyTo(span);
            span[Sha256.ByteLength - 1]++;
            var sha4 = new Sha256(span); // ba7816bf-8f01cfea-414140de-5dae2223-b00361a3-96177a9c-b410ff61-f20015ae
            Assert.True(sha4 >= sha256);
            Assert.True(sha4 > sha256);
            Assert.False(sha4 < sha256);
            Assert.False(sha4 <= sha256);
            Assert.True(sha256.CompareTo(sha4) < 0);

            Sha256[] list = new[] { sha4, sha256, sha2, sha3 };
            Sha256Comparer comparer = Sha256Comparer.Default;
            Array.Sort(list, comparer.Compare);
            Assert.True(list[0] == list[1]);
            Assert.True(list[2] > list[1]);
            Assert.True(list[2] < list[3]);
        }

        [Trait("Type", "Unit")]
        [Fact]
        public static void When_parse_sha256_whitespace()
        {
            const string whitespace = " \n  \t \r ";

            Sha256 expected = s_sha256.HashData("abc"); // ba7816bf-8f01cfea-414140de-5dae2223-b00361a3-96177a9c-b410ff61-f20015ad
            string str = expected.ToString();

            // Leading whitespace
            var actual = Sha256.Parse(whitespace + str);
            Assert.Equal(expected, actual);

            // Trailing whitespace
            actual = Sha256.Parse(str + whitespace);
            Assert.Equal(expected, actual);

            // Both
            actual = Sha256.Parse(whitespace + str + whitespace);
            Assert.Equal(expected, actual);

            // Fail
            Assert.False(Sha256.TryParse("1" + str, out _));
            Assert.False(Sha256.TryParse(str + "1", out _));
            Assert.False(Sha256.TryParse("1" + whitespace + str, out _));
            Assert.False(Sha256.TryParse(str + whitespace + "1", out _));
            Assert.False(Sha256.TryParse("1" + whitespace + str + whitespace, out _));
            Assert.False(Sha256.TryParse(whitespace + str + whitespace + "1", out _));
        }

        [Trait("Type", "Unit")]
        [Fact]
        public static void When_sha256_equality()
        {
            var sha0 = default(Sha256);
            Sha256 sha1 = s_sha256.HashData("abc");
            Sha256 sha2 = s_sha256.HashData("abc");
            Sha256 sha3 = s_sha256.HashData("def");

            Assert.True(sha1 == sha2);
            Assert.False(sha1 != sha2);
            Assert.True(sha1.Equals((object)sha2));
            Assert.False(sha1.Equals(new object()));

            Assert.Equal(sha1, sha2);
            Assert.Equal(sha1.GetHashCode(), sha2.GetHashCode());
            Assert.Equal(sha1.ToString(), sha2.ToString());

            Assert.NotEqual(sha0, sha1);
            Assert.NotEqual(sha0.GetHashCode(), sha1.GetHashCode());
            Assert.NotEqual(sha0.ToString(), sha1.ToString());

            Assert.NotEqual(sha3, sha1);
            Assert.NotEqual(sha3.GetHashCode(), sha1.GetHashCode());
            Assert.NotEqual(sha3.ToString(), sha1.ToString());
        }
    }
}
