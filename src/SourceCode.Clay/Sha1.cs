#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using System;
using System.Buffers;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace SourceCode.Clay
{
    /// <summary>
    /// Represents a <see cref="Sha1"/> value.
    /// </summary>
    /// <seealso cref="System.Security.Cryptography.SHA1" />
    /// <seealso cref="System.IEquatable{T}" />
    /// <seealso cref="System.IComparable{T}" />
    [DebuggerDisplay("{ToString(\"D\"),nq,ac}")]
    [StructLayout(LayoutKind.Sequential, Size = ByteLength)]
    public readonly struct Sha1 : IEquatable<Sha1>, IComparable<Sha1>
    {
        // Use a thread-local instance of the underlying crypto algorithm.
        private static readonly ThreadLocal<System.Security.Cryptography.SHA1> t_sha1 = new ThreadLocal<System.Security.Cryptography.SHA1>(System.Security.Cryptography.SHA1.Create);

        /// <summary>
        /// The standard byte length of a <see cref="Sha1"/> value.
        /// </summary>
        public const byte ByteLength = 20;

        /// <summary>
        /// The number of hex characters required to represent a <see cref="Sha1"/> value.
        /// </summary>
        public const byte HexLength = ByteLength * 2;
        
        private static readonly Sha1 s_empty = HashImpl(ReadOnlySpan<byte>.Empty);

        // We choose to use value types for primary storage so that we can live on the stack
        // Using byte[] or String means a dereference to the heap (& 'fixed byte' would require unsafe)

        private readonly byte _a0;
        private readonly byte _a1;
        private readonly byte _a2;
        private readonly byte _a3;

        private readonly byte _b0;
        private readonly byte _b1;
        private readonly byte _b2;
        private readonly byte _b3;

        private readonly byte _c0;
        private readonly byte _c1;
        private readonly byte _c2;
        private readonly byte _c3;

        private readonly byte _d0;
        private readonly byte _d1;
        private readonly byte _d2;
        private readonly byte _d3;

        private readonly byte _e0;
        private readonly byte _e1;
        private readonly byte _e2;
        private readonly byte _e3;

        /// <summary>
        /// Deserializes a <see cref="Sha1"/> value from the provided <see cref="ReadOnlyMemory{T}"/>.
        /// </summary>
        /// <param name="source">The buffer.</param>
        public Sha1(in ReadOnlySpan<byte> source)
            : this() // Compiler doesn't know we're indirectly setting all the fields
        {
            var src = source.Slice(0, ByteLength);
            var dst = MemoryMarshal.CreateSpan(ref _a0, ByteLength);
            src.CopyTo(dst);
        }

        /// <summary>
        /// Hashes the specified bytes.
        /// </summary>
        /// <param name="span">The bytes to hash.</param>
        /// <returns></returns>
        public static Sha1 Hash(in ReadOnlySpan<byte> span)
        {
            if (span.Length == 0) return s_empty;

            var sha1 = HashImpl(span);
            return sha1;
        }

        /// <summary>
        /// Hashes the specified value using utf8 encoding.
        /// </summary>
        /// <param name="value">The string to hash.</param>
        /// <returns></returns>
        public static Sha1 Hash(in string value)
        {
            if (value is null) throw new ArgumentNullException(nameof(value));
            if (value.Length == 0) return s_empty;

            var maxLen = Encoding.UTF8.GetMaxByteCount(value.Length); // Utf8 is 1-4 bpc

            // Rent buffer
            var rented = ArrayPool<byte>.Shared.Rent(maxLen);
            try
            {
                var count = Encoding.UTF8.GetBytes(value, 0, value.Length, rented, 0);

                var span = new ReadOnlySpan<byte>(rented, 0, count);

                var sha1 = HashImpl(span);
                return sha1;
            }
            finally
            {
                // Return buffer
                ArrayPool<byte>.Shared.Return(rented);
            }
        }

        /// <summary>
        /// Hashes the specified bytes.
        /// </summary>
        /// <param name="bytes">The bytes to hash.</param>
        /// <returns></returns>
        public static Sha1 Hash(in byte[] bytes)
        {
            if (bytes is null) throw new ArgumentNullException(nameof(bytes));
            if (bytes.Length == 0) return s_empty;

            var span = new ReadOnlySpan<byte>(bytes);

            var sha1 = HashImpl(span);
            return sha1;
        }

        /// <summary>
        /// Hashes the specified <paramref name="bytes"/>, starting at the specified <paramref name="start"/> and <paramref name="length"/>.
        /// </summary>
        /// <param name="bytes">The bytes to hash.</param>
        /// <param name="start">The offset.</param>
        /// <param name="length">The count.</param>
        /// <returns></returns>
        public static Sha1 Hash(in byte[] bytes, int start, int length)
        {
            if (bytes is null) throw new ArgumentNullException(nameof(bytes));

            // Do this first to check validity of start/length
            var span = new ReadOnlySpan<byte>(bytes, start, length);

            if (length == 0) return s_empty;

            var sha1 = HashImpl(span);
            return sha1;
        }

        /// <summary>
        /// Hashes the specified stream.
        /// </summary>
        /// <param name="stream">The stream to hash.</param>
        /// <returns></returns>
        public static Sha1 Hash(Stream stream)
        {
            if (stream is null) throw new ArgumentNullException(nameof(stream));
            // Note that length=0 should NOT short-circuit

            var hash = t_sha1.Value.ComputeHash(stream);

            var sha1 = new Sha1(hash);
            return sha1;
        }

        private static Sha1 HashImpl(in ReadOnlySpan<byte> span)
        {
            // Do NOT short-circuit here; rely on call-sites to do so

            Span<byte> hash = stackalloc byte[ByteLength];
            t_sha1.Value.TryComputeHash(span, hash, out _);

            var sha1 = new Sha1(hash);
            return sha1;
        }

        /// <summary>
        /// Copies the <see cref="Sha1"/> value to the provided buffer.
        /// </summary>
        /// <param name="destination">The buffer to copy to.</param>
        public void CopyTo(Span<byte> destination)
        {
            unsafe
            {
                fixed (byte* ptr = &_a0)
                {
                    var source = new ReadOnlySpan<byte>(ptr, ByteLength);
                    source.CopyTo(destination);
                }
            }
        }

        /// <summary>
        /// Tries to copy the <see cref="Sha1"/> value to the provided buffer.
        /// </summary>
        /// <param name="destination">The buffer to copy to.</param>
        /// <returns>True if successful</returns>
        public bool TryCopyTo(Span<byte> destination)
        {
            unsafe
            {
                fixed (byte* ptr = &_a0)
                {
                    var source = new ReadOnlySpan<byte>(ptr, ByteLength);
                    return source.TryCopyTo(destination);
                }
            }
        }

        /// <summary>
        /// Returns a string representation of the <see cref="Sha1"/> instance using the 'N' format.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            // Text is treated as 5 groups of 8 chars (5 x 4 bytes)
            Span<char> span = stackalloc char[HexLength];
            
            unsafe
            {
                fixed (byte* src = &_a0)
                {
                    var pos = 0;
                    for (var i = 0; i < ByteLength; i++) // 20
                    {
                        // Each byte is two hexits (convention is lowercase)
                        var byt = src[i];

                        var b = byt >> 4; // == b / 16
                        span[pos++] = (char)(b < 10 ? b + '0' : b - 10 + 'a'); // Inline for perf

                        b = byt & 0x0F; // == b % 16
                        span[pos++] = (char)(b < 10 ? b + '0' : b - 10 + 'a'); // Inline for perf
                    }
                }
            }

            var str = new string(span);
            return str;
        }

        /// <summary>
        /// Returns a string representation of the <see cref="Sha1"/> instance.
        /// N: a9993e364706816aba3e25717850c26c9cd0d89d,
        /// D: a9993e36-4706816a-ba3e2571-7850c26c-9cd0d89d,
        /// S: a9993e36 4706816a ba3e2571 7850c26c 9cd0d89d
        /// </summary>
        /// <param name="format"></param>
        /// <returns></returns>
        public string ToString(string format)
        {
            if (string.IsNullOrWhiteSpace(format))
                throw new FormatException($"Empty format specification");

            if (format.Length != 1)
                throw new FormatException($"Invalid format specification length {format.Length}");

            switch (format[0])
            {
                // a9993e364706816aba3e25717850c26c9cd0d89d
                case 'n':
                case 'N': return ToString();

                // a9993e36-4706816a-ba3e2571-7850c26c-9cd0d89d
                case 'd':
                case 'D': return Format('-');

                // a9993e36 4706816a ba3e2571 7850c26c 9cd0d89d
                case 's':
                case 'S': return Format(' ');
            }

            throw new FormatException($"Invalid format specification '{format}'");
        }

        private string Format(char separator)
        {
            Debug.Assert(separator == '-' || separator == ' ');

            // Text is treated as 5 groups of 8 chars (5 x 4 bytes) with 4 separators
            Span<char> span = stackalloc char[HexLength + 4];

            unsafe
            {
                fixed (byte* src = &_a0)
                {
                    var pos = 0;
                    var sep = 8;
                    for (var i = 0; i < ByteLength; i++) // 20
                    {
                        // Each byte is two hexits (convention is lowercase)
                        var byt = src[i];

                        var b = byt >> 4; // == b / 16
                        span[pos++] = (char)(b < 10 ? b + '0' : b - 10 + 'a'); // Inline for perf

                        b = byt & 0x0F; // == b % 16
                        span[pos++] = (char)(b < 10 ? b + '0' : b - 10 + 'a'); // Inline for perf

                        // Append a separator if required
                        if (pos == sep) // pos >= 2, sep = 0|N
                        {
                            span[pos++] = separator;

                            sep = pos + 8;
                            if (sep >= span.Length)
                                sep = 0; // Prevent IndexOutOfRangeException
                        }
                    }
                }
            }

            var str = new string(span);
            return str;
        }

        /// <summary>
        /// Converts the <see cref="Sha1"/> instance to a string using the 'N' format,
        /// and returns the value split into two tokens.
        /// </summary>
        /// <param name="prefixLength">The length of the first token.</param>
        /// <returns></returns>
        public KeyValuePair<string, string> Split(int prefixLength)
        {
            // Text is treated as 5 groups of 8 chars (5 x 4 bytes)
            Span<char> span = stackalloc char[HexLength];

            unsafe
            {
                fixed (byte* src = &_a0)
                {
                    var pos = 0;
                    for (var i = 0; i < ByteLength; i++) // 20
                    {
                        // Each byte is two hexits (convention is lowercase)
                        var byt = src[i];

                        var b = byt >> 4; // == b / 16
                        span[pos++] = (char)(b < 10 ? b + '0' : b - 10 + 'a'); // Inline for perf

                        b = byt & 0x0F; // == b % 16
                        span[pos++] = (char)(b < 10 ? b + '0' : b - 10 + 'a'); // Inline for perf
                    }
                }
            }

            if (prefixLength >= HexLength)
            {
                var pfx = new string(span);
                return new KeyValuePair<string, string>(pfx, string.Empty);
            }

            if (prefixLength <= 0)
            {
                var ext = new string(span);
                return new KeyValuePair<string, string>(string.Empty, ext);
            }

            var prefix = new string(span.Slice(0, prefixLength));
            var extra = new string(span.Slice(prefixLength, HexLength - prefixLength));
            return new KeyValuePair<string, string>(prefix, extra);
        }

        // Sentinel value for n/a (128)
        private const byte __ = 0b_1000_0000;

        // '0'=48, '9'=57
        // 'A'=65, 'F'=70
        // 'a'=97, 'f'=102
        private static readonly byte[] s_hexits = new byte['f' - '0' + 1] // 102 - 48 + 1 = 55
        {
            00, 01, 02, 03, 04, 05, 06, 07, 08, 09, // [00-09]       = 48..57 = '0'..'9'
            __, __, __, __, __, __, __, 10, 11, 12, // [10-16,17-19] = 65..67 = 'A'..'C'
            13, 14, 15, __, __, __, __, __, __, __, // [20-22,23-29] = 68..70 = 'D'..'F'
            __, __, __, __, __, __, __, __, __, __, // [30-39]
            __, __, __, __, __, __, __, __, __, 10, // [40-48,49]    = 97..97 = 'a'
            11, 12, 13, 14, 15                      // [50-54]       = 98..102= 'b'..'f'
        };

        /// <summary>
        /// Tries to parse the specified hexadecimal.
        /// </summary>
        /// <param name="hex">The hexadecimal.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static bool TryParse(in ReadOnlySpan<char> hex, out Sha1 value)
        {
            value = default;

            // Length must be at least 40
            if (hex.Length < HexLength)
                return false;

            // Check if the hex specifier '0x' is present
            var slice = hex;
            if (slice[0] == '0' && (slice[1] == 'x' || slice[1] == 'X'))
            {
                // Length must be at least 42
                if (slice.Length < 2 + HexLength)
                    return false;

                // Skip '0x'
                slice = slice.Slice(2);
            }

            Span<byte> span = stackalloc byte[ByteLength];

            // Text is treated as 5 groups of 8 chars (4 bytes); 4 separators optional
            // "34aa973c-d4c4daa4-f61eeb2b-dbad2731-6534016f"
            var pos = 0;
            for (var i = 0; i < 5; i++)
            {
                for (var j = 0; j < 4; j++)
                {
                    // Two hexits per byte: aaaa bbbb
                    if (!TryParseHexit(slice[pos++], out var h1)
                        || !TryParseHexit(slice[pos++], out var h2))
                        return false;

                    span[i * 4 + j] = (byte)((h1 << 4) | h2);
                }

                if (pos < HexLength && (slice[pos] == '-' || slice[pos] == ' '))
                    pos++;
            }

            // TODO: Is this correct: do we not already permit longer strings to be passed in?
            // If the string is not fully consumed, it had an invalid length
            if (pos != slice.Length)
                return false;

            value = new Sha1(span);
            return true;

            // Local functions

            bool TryParseHexit(char c, out byte b)
            {
                b = 0;

                if (c < '0' || c > 'f')
                    return false;

                var bex = s_hexits[c - '0'];
                if (bex == __) // Sentinel value for n/a (128)
                    return false;

                b = bex;
                return true;
            }
        }

        /// <summary>
        /// Tries to parse the specified hexadecimal.
        /// </summary>
        /// <param name="hex">The hexadecimal.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static bool TryParse(string hex, out Sha1 value)
        {
            value = default;

            if (hex is null)
                return false;

            var span = hex.AsSpan();
            return TryParse(span, out value);
        }

        /// <summary>
        /// Parses the specified hexadecimal.
        /// </summary>
        /// <param name="hex">The hexadecimal.</param>
        /// <returns></returns>
        /// <exception cref="FormatException">Sha1</exception>
        public static Sha1 Parse(in ReadOnlySpan<char> hex)
        {
            if (!TryParse(hex, out var sha1))
                throw new FormatException($"Input was not recognized as a valid {nameof(Sha1)}");

            return sha1;
        }

        /// <summary>
        /// Parses the specified hexadecimal.
        /// </summary>
        /// <param name="hex">The hexadecimal.</param>
        /// <returns></returns>
        /// <exception cref="FormatException">Sha1</exception>
        public static Sha1 Parse(string hex)
        {
            if (hex is null) throw new ArgumentNullException(nameof(hex));

            var span = hex.AsSpan();
            return Parse(span);
        }

        public bool Equals(Sha1 other)
        {
            unsafe
            {
                fixed (byte* src = &_a0)
                {
                    var dst = &other._a0;

                    for (var i = 0; i < ByteLength; i++)
                        if (src[i] != dst[i])
                            return false;
                }

                return true;
            }
        }

        public override bool Equals(object obj)
            => obj is Sha1 other
            && Equals(other);

        public override int GetHashCode()
        {
            var hash = new HashCode();

            hash.Add(_a0);
            hash.Add(_a1);
            hash.Add(_a2);
            hash.Add(_a3);

            hash.Add(_b0);
            hash.Add(_b1);
            hash.Add(_b2);
            hash.Add(_b3);

            hash.Add(_c0);
            hash.Add(_c1);
            hash.Add(_c2);
            hash.Add(_c3);

            hash.Add(_d0);
            hash.Add(_d1);
            hash.Add(_d2);
            hash.Add(_d3);

            hash.Add(_e0);
            hash.Add(_e1);
            hash.Add(_e2);
            hash.Add(_e3);

            var hc = hash.ToHashCode();
            return hc;
        }

        public int CompareTo(Sha1 other)
        {
            unsafe
            {
                fixed (byte* src = &_a0)
                {
                    var dst = &other._a0;

                    for (var i = 0; i < ByteLength; i++)
                    {
                        var cmp = src[i].CompareTo(dst[i]); // CLR returns a-b for byte comparisons
                        if (cmp == 0)
                            continue;

                        return cmp < 0 ? -1 : 1; // Normalize to [-1, 0, +1]
                    }
                }
            }

            return 0;
        }

        public static bool operator ==(Sha1 x, Sha1 y) => x.Equals(y);

        public static bool operator !=(Sha1 x, Sha1 y) => !(x == y);

        public static bool operator >=(Sha1 x, Sha1 y) => x.CompareTo(y) >= 0;

        public static bool operator >(Sha1 x, Sha1 y) => x.CompareTo(y) > 0;

        public static bool operator <=(Sha1 x, Sha1 y) => x.CompareTo(y) <= 0;

        public static bool operator <(Sha1 x, Sha1 y) => x.CompareTo(y) < 0;
    }
}
