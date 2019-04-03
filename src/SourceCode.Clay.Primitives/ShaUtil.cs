using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace SourceCode.Clay
{
    internal static class ShaUtil
    {
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

        // Each byte is two hexits
        private const string HexChars = "0123456789ABCDEF";

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool TryParseHexit(char c, out byte b)
        {
            b = 0;

            if (c < '0' || c > 'f')
                return false;

            byte bex = s_hexits[c - '0'];
            if (bex == __) // Sentinel value for n/a (128)
                return false;

            b = bex;
            return true;
        }

        /// <summary>
        /// Tries to parse the specified hexadecimal.
        /// </summary>
        /// <param name="hex">The hexadecimal.</param>
        /// <param name="sha">The buffer to populate with the Sha value.</param>
        internal static bool TryParse(ReadOnlySpan<char> hex, Span<byte> sha)
        {
            int byteLength = sha.Length;
            Debug.Assert(byteLength == Sha1.ByteLength || byteLength == Sha256.ByteLength);

            // Text is treated as 5|8 groups of 8 chars (5|8 x 4 bytes)
            int hexLength = byteLength * 2; // 40|64
            if (hex.Length < hexLength)
                return false;

            // Skip leading whitespace
            int pos = 0;
            while (pos < hex.Length && char.IsWhiteSpace(hex[pos]))
                pos++;

            // Check if the hex specifier '0x' is present
            ReadOnlySpan<char> slice = hex.Slice(pos);
            if (slice[0] == '0' && (slice[1] == 'x' || slice[1] == 'X'))
            {
                // Length must be at least 40+2|64+2
                if (slice.Length < 2 + hexLength)
                    return false;

                // Skip '0x'
                slice = slice.Slice(2);
            }

            // Text is treated as 5|8 groups of 8 chars (4 bytes); 4|7 separators optional
            // "34aa973c-d4c4daa4-f61eeb2b-dbad2731-6534016f"
            // "cdc76e5c-9914fb92-81a1c7e2-84d73e67-f1809a48-a497200e-046d39cc-c7112cd0"
            pos = 0;
            for (int i = 0; i < byteLength; i++) // 20|32
            {
                // We read 4x2 chars at a time, 2 hexits per byte: aaaa bbbb
                if (!TryParseHexit(slice[pos++], out byte h1)
                    || !TryParseHexit(slice[pos++], out byte h2))
                    return false;

                sha[i] = (byte)((h1 << 4) | h2);

                if (pos < hexLength && (slice[pos] == '-' || slice[pos] == ' '))
                    pos++;
            }

            // Skip trailing whitespace
            while (pos < slice.Length && char.IsWhiteSpace(slice[pos]))
                pos++;

            // If the string is not fully consumed, it had an invalid length
            if (pos != slice.Length)
                return false;

            return true;
        }

        // https://github.com/dotnet/corefx/blob/master/src/System.Memory/src/System/Buffers/Text/Utf8Formatter/FormattingHelpers.cs
        internal enum HexCasing : uint
        {
            Upper = 0,

            // '0'..'9' (48-57) already have 0x20 bit set, so OR 0x20 is a no-op.
            // 'A'..'F' (65-90) doesn't have 0x20 bit set, so OR maps to 'a'-'f'.
            Lower = 0x20u,
        }

        /// <summary>
        /// Converts the <see cref="Sha1"/> or <see cref="Sha256"/> instance to a string using the 'n' or 'N' format,
        /// and returns the value split into two tokens.
        /// </summary>
        /// <param name="sha">The sha value.</param>
        /// <param name="prefixLength">The length of the first token.</param>
        /// <param name="casing">The ASCII casing to use.</param>
        /// <returns></returns>
        internal static KeyValuePair<string, string> Split(ReadOnlySpan<byte> sha, int prefixLength, HexCasing casing)
        {
            int byteLength = sha.Length; // 20|32
            Debug.Assert(byteLength == Sha1.ByteLength || byteLength == Sha256.ByteLength);

            // Text is treated as 5|8 groups of 8 chars (5|8 x 4 bytes)
            int hexLength = byteLength * 2;
#if !NETSTANDARD2_0
            Span<char> span = stackalloc char[hexLength];
#else
            var span = new char[hexLength];
#endif
            int pos = 0;
            for (int i = 0; i < byteLength; i++) // 20|32
            {
                // Each byte is two hexits
                // Output *highest* index first so codegen can elide all but first bounds check
                byte byt = sha[i];
                span[pos + 1] = (char)(HexChars[byt & 15] | (uint)casing); // == b % 16
                span[pos] = (char)(HexChars[byt >> 4] | (uint)casing); // == b / 16

                pos += 2;
            }

            if (prefixLength >= hexLength)
            {
                string pfx = new string(span);
                return new KeyValuePair<string, string>(pfx, string.Empty);
            }

            if (prefixLength <= 0)
            {
                string ext = new string(span);
                return new KeyValuePair<string, string>(string.Empty, ext);
            }

#if !NETSTANDARD2_0
            Span<char> p = span.Slice(0, prefixLength);
            Span<char> e = span.Slice(prefixLength, hexLength - prefixLength);

            string prefix = new string(p);
            string extra = new string(e);
#else
            string prefix = new string(span, 0, prefixLength);
            string extra = new string(span, prefixLength, hexLength - prefixLength);
#endif
            var kvp = new KeyValuePair<string, string>(prefix, extra);
            return kvp;
        }

        /// <summary>
        /// Returns a string representation of the <see cref="Sha1"/> or <see cref="Sha256"/> instance using the 'n' or 'N' format.
        /// </summary>
        /// <param name="sha"></param>
        /// <param name="casing">The ASCII casing to use.</param>
        /// <returns></returns>
        internal static string ToString(ReadOnlySpan<byte> sha, HexCasing casing)
        {
            int byteLength = sha.Length; // 20|32
            Debug.Assert(byteLength == Sha1.ByteLength || byteLength == Sha256.ByteLength);

            // Text is treated as 5|8 groups of 8 chars (5|8 x 4 bytes)
            int hexLength = byteLength * 2; // 40|64
#if !NETSTANDARD2_0
            Span<char> span = stackalloc char[hexLength];
#else
            var span = new char[hexLength];
#endif
            int pos = 0;
            for (int i = 0; i < byteLength; i++) // 20|32
            {
                // Each byte is two hexits
                // Output *highest* index first so codegen can elide all but first bounds check
                byte byt = sha[i];
                span[pos + 1] = (char)(HexChars[byt & 15] | (uint)casing); // == b % 16
                span[pos] = (char)(HexChars[byt >> 4] | (uint)casing); // == b / 16

                pos += 2;
            }

            string str = new string(span);
            return str;
        }

        /// <summary>
        /// Returns a string representation of the <see cref="Sha1"/> or <see cref="Sha256"/> instance.
        /// n: a9993e364706816aba3e25717850c26c9cd0d89d, cdc76e5c9914fb9281a1c7e284d73e67f1809a48a497200e046d39ccc7112cd0
        /// d: a9993e36-4706816a-ba3e2571-7850c26c-9cd0d89d, cdc76e5c-9914fb92-81a1c7e2-84d73e67-f1809a48-a497200e-046d39cc-c7112cd0
        /// s: a9993e36 4706816a ba3e2571 7850c26c 9cd0d89d, cdc76e5c 9914fb92 81a1c7e2 84d73e67 f1809a48 a497200e 046d39cc c7112cd0
        /// </summary>
        /// <param name="sha"></param>
        /// <param name="separator"></param>
        /// <param name="casing">The ASCII casing to use.</param>
        /// <returns></returns>
        internal static string ToString(ReadOnlySpan<byte> sha, char separator, HexCasing casing)
        {
            Debug.Assert(separator == '-' || separator == ' ');

            int byteLength = sha.Length; // 20|32
            Debug.Assert(byteLength == Sha1.ByteLength || byteLength == Sha256.ByteLength);

            // Text is treated as 5|8 groups of 8 chars (5|8 x 4 bytes) with 4|7 separators
            int hexLength = byteLength * 2; // 40|64
            int n = hexLength / 8; // 5|8
#if !NETSTANDARD2_0
            Span<char> span = stackalloc char[hexLength + n - 1]; // + 4|7
#else
            var span = new char[hexLength + n - 1]; // + 4|7
#endif
            int pos = 0;
            int sep = 8;
            for (int i = 0; i < byteLength; i++) // 20|32
            {
                // Each byte is two hexits (convention is lowercase)
                // Output *highest* index first so codegen can elide all but first bounds check
                byte byt = sha[i];
                span[pos + 1] = (char)(HexChars[byt & 15] | (uint)casing); // == b % 16
                span[pos] = (char)(HexChars[byt >> 4] | (uint)casing); // == b / 16

                pos += 2;

                // Append a separator if required
                if (pos == sep) // pos >= 2, sep = 0|N
                {
                    span[pos++] = separator;

                    sep = pos + 8;
                    if (sep >= span.Length)
                        sep = 0; // Prevent IndexOutOfRangeException
                }
            }

            string str = new string(span);
            return str;
        }

        internal static unsafe class NativeMethods
        {
            [System.Security.SecurityCritical]
            [DllImport("msvcrt.dll", EntryPoint = "memcmp", CallingConvention = CallingConvention.Cdecl, SetLastError = false)]
            public static extern int MemCompare(byte* x, byte* y, int count);
        }
    }
}
