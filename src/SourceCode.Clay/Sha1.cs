#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using System;
using System.Buffers;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using crypt = System.Security.Cryptography;

namespace SourceCode.Clay
{
    /// <summary>
    /// Represents a <see cref="Sha1"/> value.
    /// </summary>
    /// <seealso cref="crypt.SHA1" />
    /// <seealso cref="System.IEquatable{T}" />
    /// <seealso cref="System.IComparable{T}" />
    [DebuggerDisplay("{ToString(\"D\"),nq,ac}")]
    public readonly struct Sha1 : IEquatable<Sha1>, IComparable<Sha1>
    {
        // Use a thread-local instance of the underlying crypto algorithm.
        private static readonly ThreadLocal<crypt.SHA1> t_sha = new ThreadLocal<crypt.SHA1>(crypt.SHA1.Create);

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
        // TODO: In C# 7.4+ we can use 'readonly fixed byte'

        private readonly Block _bytes;

        [StructLayout(LayoutKind.Sequential, Pack = 1, Size = ByteLength)]
        private unsafe struct Block
        {
#pragma warning disable IDE0044 // Add readonly modifier (vs bug)
            private fixed byte _bytes[ByteLength];
#pragma warning restore IDE0044 // Add readonly modifier (vs bug)
        }

        /// <summary>
        /// Deserializes a <see cref="Sha1"/> value from the provided <see cref="ReadOnlyMemory{T}"/>.
        /// </summary>
        /// <param name="source">The buffer.</param>
        public Sha1(ReadOnlySpan<byte> source)
            : this() // Compiler doesn't know we're indirectly setting all the fields
        {
            if (source.Length < ByteLength) throw new ArgumentOutOfRangeException(nameof(source));

            ReadOnlySpan<byte> src = source.Slice(0, ByteLength);

            unsafe
            {
                fixed (Block* ptr = &_bytes)
                {
                    var dst = new Span<byte>(ptr, ByteLength);
                    src.CopyTo(dst);
                }
            }
        }

        /// <summary>
        /// Hashes the specified bytes.
        /// </summary>
        /// <param name="span">The bytes to hash.</param>
        /// <returns></returns>
        public static Sha1 Hash(ReadOnlySpan<byte> span)
        {
            if (span.Length == 0) return s_empty;

            Sha1 sha = HashImpl(span);
            return sha;
        }

        /// <summary>
        /// Hashes the specified value using utf8 encoding.
        /// </summary>
        /// <param name="value">The string to hash.</param>
        /// <returns></returns>
        public static Sha1 Hash(string value)
        {
            if (value is null) throw new ArgumentNullException(nameof(value));
            if (value.Length == 0) return s_empty;

            int maxLen = Encoding.UTF8.GetMaxByteCount(value.Length); // Utf8 is 1-4 bpc

            byte[] rented = ArrayPool<byte>.Shared.Rent(maxLen);
            try
            {
                int count = Encoding.UTF8.GetBytes(value, 0, value.Length, rented, 0);

                var span = new ReadOnlySpan<byte>(rented, 0, count);

                Sha1 sha = HashImpl(span);
                return sha;
            }
            finally
            {
                ArrayPool<byte>.Shared.Return(rented);
            }
        }

        /// <summary>
        /// Hashes the specified bytes.
        /// </summary>
        /// <param name="bytes">The bytes to hash.</param>
        /// <returns></returns>
        public static Sha1 Hash(byte[] bytes)
        {
            if (bytes is null) throw new ArgumentNullException(nameof(bytes));
            if (bytes.Length == 0) return s_empty;

            var span = new ReadOnlySpan<byte>(bytes);

            Sha1 sha = HashImpl(span);
            return sha;
        }

        /// <summary>
        /// Hashes the specified <paramref name="bytes"/>, starting at the specified <paramref name="start"/> and <paramref name="length"/>.
        /// </summary>
        /// <param name="bytes">The bytes to hash.</param>
        /// <param name="start">The offset.</param>
        /// <param name="length">The count.</param>
        /// <returns></returns>
        public static Sha1 Hash(byte[] bytes, in int start, in int length)
        {
            if (bytes is null) throw new ArgumentNullException(nameof(bytes));

            // Do this first to check validity of start/length
            var span = new ReadOnlySpan<byte>(bytes, start, length);

            if (length == 0) return s_empty;

            Sha1 sha = HashImpl(span);
            return sha;
        }

        /// <summary>
        /// Hashes the specified stream.
        /// </summary>
        /// <param name="stream">The stream to hash.</param>
        /// <returns></returns>
        public static Sha1 Hash(in Stream stream)
        {
            if (stream is null) throw new ArgumentNullException(nameof(stream));
            // Note that length=0 should NOT short-circuit

            byte[] hash = t_sha.Value.ComputeHash(stream);

            var sha = new Sha1(hash);
            return sha;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Sha1 HashImpl(ReadOnlySpan<byte> span)
        {
            // Do NOT short-circuit here; rely on call-sites to do so

            Span<byte> hash = stackalloc byte[ByteLength];
            t_sha.Value.TryComputeHash(span, hash, out _);

            var sha = new Sha1(hash);
            return sha;
        }

        /// <summary>
        /// Copies the <see cref="Sha1"/> value to the provided buffer.
        /// </summary>
        /// <param name="destination">The buffer to copy to.</param>
        public void CopyTo(Span<byte> destination)
        {
            unsafe
            {
                fixed (Block* ptr = &_bytes)
                {
                    var src = new ReadOnlySpan<byte>(ptr, ByteLength);
                    src.CopyTo(destination);
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
                fixed (Block* ptr = &_bytes)
                {
                    var src = new ReadOnlySpan<byte>(ptr, ByteLength);
                    return src.TryCopyTo(destination);
                }
            }
        }

        /// <summary>
        /// Returns a string representation of the <see cref="Sha1"/> instance using the 'n' format.
        /// </summary>
        /// <returns></returns>
        public override string ToString() => ToString("n");

        /// <summary>
        /// Returns a string representation of the <see cref="Sha1"/> instance.
        /// n: a9993e364706816aba3e25717850c26c9cd0d89d,
        /// d: a9993e36-4706816a-ba3e2571-7850c26c-9cd0d89d,
        /// s: a9993e36 4706816a ba3e2571 7850c26c 9cd0d89d
        /// </summary>
        /// <param name="format"></param>
        public string ToString(string format)
        {
            if (string.IsNullOrWhiteSpace(format))
                throw new FormatException($"Empty format specification");

            if (format.Length != 1)
                throw new FormatException($"Invalid format specification length {format.Length}");

            unsafe
            {
                fixed (Block* ptr = &_bytes)
                {
                    var sha = new ReadOnlySpan<byte>(ptr, ByteLength);

                    switch (format[0])
                    {
                        // a9993e364706816aba3e25717850c26c9cd0d89d
                        case 'n': return ShaUtil.ToString(sha, ShaUtil.HexCasing.Lower);
                        case 'N': return ShaUtil.ToString(sha, ShaUtil.HexCasing.Upper);

                        // a9993e36-4706816a-ba3e2571-7850c26c-9cd0d89d
                        case 'd': return ShaUtil.ToString(sha, '-', ShaUtil.HexCasing.Lower);
                        case 'D': return ShaUtil.ToString(sha, '-', ShaUtil.HexCasing.Upper);

                        // a9993e36 4706816a ba3e2571 7850c26c 9cd0d89d
                        case 's': return ShaUtil.ToString(sha, ' ', ShaUtil.HexCasing.Lower);
                        case 'S': return ShaUtil.ToString(sha, ' ', ShaUtil.HexCasing.Upper);
                    }
                }
            }

            throw new FormatException($"Invalid format specification '{format}'");
        }

        /// <summary>
        /// Converts the <see cref="Sha1"/> instance to a string using the 'n' or 'N' format,
        /// and returns the value split into two tokens.
        /// </summary>
        /// <param name="prefixLength">The length of the first token.</param>
        /// <param name="uppercase">If True, output uppercase, else output lowercase.</param>
        public KeyValuePair<string, string> Split(in int prefixLength, bool uppercase = false)
        {
            ShaUtil.HexCasing casing = uppercase ? ShaUtil.HexCasing.Upper : ShaUtil.HexCasing.Lower;

            unsafe
            {
                fixed (Block* ptr = &_bytes)
                {
                    var sha = new ReadOnlySpan<byte>(ptr, ByteLength);

                    KeyValuePair<string, string> kvp = ShaUtil.Split(sha, prefixLength, casing);
                    return kvp;
                }
            }
        }

        /// <summary>
        /// Tries to parse the specified hexadecimal.
        /// </summary>
        /// <param name="hex">The hexadecimal.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static bool TryParse(ReadOnlySpan<char> hex, out Sha1 value)
        {
            value = default;

            Span<byte> sha = stackalloc byte[ByteLength];
            if (!ShaUtil.TryParse(hex, sha))
                return false;

            value = new Sha1(sha);
            return true;
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

            if (hex is null || hex.Length < HexLength)
                return false;

            Span<byte> sha = stackalloc byte[ByteLength];
            if (!ShaUtil.TryParse(hex.AsSpan(), sha))
                return false;

            value = new Sha1(sha);
            return true;
        }

        /// <summary>
        /// Parses the specified hexadecimal.
        /// </summary>
        /// <param name="hex">The hexadecimal.</param>
        /// <returns></returns>
        /// <exception cref="FormatException">Sha1</exception>
        public static Sha1 Parse(ReadOnlySpan<char> hex)
        {
            Span<byte> sha = stackalloc byte[ByteLength];
            if (!ShaUtil.TryParse(hex, sha))
                throw new FormatException($"Input was not recognized as a valid {nameof(Sha1)}");

            var value = new Sha1(sha);
            return value;
        }

        /// <summary>
        /// Parses the specified hexadecimal.
        /// </summary>
        /// <param name="hex">The hexadecimal.</param>
        /// <returns></returns>
        /// <exception cref="FormatException">Sha1</exception>
        public static Sha1 Parse(string hex)
        {
            if (hex is null)
                throw new ArgumentNullException(nameof(hex));

            if (hex.Length < HexLength)
                throw new FormatException($"Input was not recognized as a valid {nameof(Sha1)}");

            Span<byte> sha = stackalloc byte[ByteLength];
            if (!ShaUtil.TryParse(hex.AsSpan(), sha))
                throw new FormatException($"Input was not recognized as a valid {nameof(Sha1)}");

            var value = new Sha1(sha);
            return value;
        }

        public bool Equals(Sha1 other)
            => CompareTo(other) == 0;

        public override bool Equals(object obj)
            => obj is Sha1 other
            && Equals(other);

        public override int GetHashCode()
        {
            var hash = new HashCode();

            unsafe
            {
                fixed (Block* src = &_bytes)
                {
                    hash.Add(src[0]);
                    hash.Add(src[4]);
                    hash.Add(src[9]);
                    hash.Add(src[14]);
                    hash.Add(src[19]);
                }
            }

            int hc = hash.ToHashCode();
            return hc;
        }

        public int CompareTo(Sha1 other)
        {
            unsafe
            {
                fixed (Block* src = &_bytes)
                {
                    Block* dst = &other._bytes;

                    int cmp = ShaUtil.NativeMethods.MemCompare((byte*)src, (byte*)dst, ByteLength);
                    return cmp;
                }
            }
        }

        public static bool operator ==(in Sha1 x, in Sha1 y) => x.Equals(y);

        public static bool operator !=(in Sha1 x, in Sha1 y) => !(x == y);

        public static bool operator >=(in Sha1 x, in Sha1 y) => x.CompareTo(y) >= 0;

        public static bool operator >(in Sha1 x, in Sha1 y) => x.CompareTo(y) > 0;

        public static bool operator <=(in Sha1 x, in Sha1 y) => x.CompareTo(y) <= 0;

        public static bool operator <(in Sha1 x, in Sha1 y) => x.CompareTo(y) < 0;
    }
}
