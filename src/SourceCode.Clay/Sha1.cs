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
    [StructLayout(LayoutKind.Sequential, Size = ByteLength)]
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
        // Using byte[] or String means a dereference to the heap (& 'fixed byte' would require unsafe)
        // In C# 7.3+ we can use 'readonly fixed byte'

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

            var sha = HashImpl(span);
            return sha;
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

            var rented = ArrayPool<byte>.Shared.Rent(maxLen);
            try
            {
                var count = Encoding.UTF8.GetBytes(value, 0, value.Length, rented, 0);

                var span = new ReadOnlySpan<byte>(rented, 0, count);

                var sha = HashImpl(span);
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
        public static Sha1 Hash(in byte[] bytes)
        {
            if (bytes is null) throw new ArgumentNullException(nameof(bytes));
            if (bytes.Length == 0) return s_empty;

            var span = new ReadOnlySpan<byte>(bytes);

            var sha = HashImpl(span);
            return sha;
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

            var sha = HashImpl(span);
            return sha;
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

            var hash = t_sha.Value.ComputeHash(stream);

            var sha = new Sha1(hash);
            return sha;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Sha1 HashImpl(in ReadOnlySpan<byte> span)
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
                    var src = new ReadOnlySpan<byte>(ptr, ByteLength);
                    var ok = src.TryCopyTo(destination);
                    return ok;
                }
            }
        }

        /// <summary>
        /// Returns a string representation of the <see cref="Sha1"/> instance using the 'N' format.
        /// </summary>
        /// <returns></returns>
        public override string ToString() => ToString("N");

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

            unsafe
            {
                fixed (byte* src = &_a0)
                {
                    var sha = new ReadOnlySpan<byte>(src, ByteLength);

                    switch (format[0])
                    {
                        // a9993e364706816aba3e25717850c26c9cd0d89d
                        case 'n':
                        case 'N':
                            {
                                var str = ShaUtil.ToString(sha);
                                return str;
                            }

                        // a9993e36-4706816a-ba3e2571-7850c26c-9cd0d89d
                        case 'd':
                        case 'D':
                            {
                                var str = ShaUtil.ToString(sha, '-');
                                return str;
                            }

                        // a9993e36 4706816a ba3e2571 7850c26c 9cd0d89d
                        case 's':
                        case 'S':
                            {
                                var str = ShaUtil.ToString(sha, ' ');
                                return str;
                            }
                    }
                }
            }

            throw new FormatException($"Invalid format specification '{format}'");
        }

        /// <summary>
        /// Converts the <see cref="Sha1"/> instance to a string using the 'N' format,
        /// and returns the value split into two tokens.
        /// </summary>
        /// <param name="prefixLength">The length of the first token.</param>
        /// <returns></returns>
        public KeyValuePair<string, string> Split(int prefixLength)
        {
            unsafe
            {
                fixed (byte* src = &_a0)
                {
                    var sha = new ReadOnlySpan<byte>(src, ByteLength);

                    var kvp = ShaUtil.Split(sha, prefixLength);
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
        public static bool TryParse(in ReadOnlySpan<char> hex, out Sha1 value)
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
        public static Sha1 Parse(in ReadOnlySpan<char> hex)
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
                        var cmp = src[i].CompareTo(dst[i]); // CLR returns (a - b) for byte comparisons
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
