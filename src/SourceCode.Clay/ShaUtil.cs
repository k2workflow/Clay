using System;
using System.Collections.Generic;
using System.Diagnostics;

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

        // Each byte is two hexits (our convention is lowercase)
        private const string HexChars = "0123456789abcdef";

        private static bool TryParseHexit(in char c, out byte b)
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

        /// <summary>
        /// Tries to parse the specified hexadecimal.
        /// </summary>
        /// <param name="hex">The hexadecimal.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        internal static bool TryParse(in ReadOnlySpan<char> hex, in Span<byte> sha)
        {
            var byteLength = sha.Length;
            Debug.Assert(byteLength == Sha1.ByteLength || byteLength == Sha256.ByteLength);

            // Text is treated as 5|8 groups of 8 chars (5|8 x 4 bytes)
            var hexLength = byteLength * 2; // 40|64
            if (hex.Length < hexLength)
                return false;

            // Check if the hex specifier '0x' is present
            ReadOnlySpan<char> slice = hex;
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
            var pos = 0;
            for (var i = 0; i < byteLength; i++) // 20|32
            {
                // We read 4x2 chars at a time, 2 hexits per byte: aaaa bbbb
                if (!TryParseHexit(slice[pos++], out var h1)
                    || !TryParseHexit(slice[pos++], out var h2))
                    return false;

                sha[i] = (byte)((h1 << 4) | h2);

                if (pos < hexLength && (slice[pos] == '-' || slice[pos] == ' '))
                    pos++;
            }

            // TODO: Is this correct: do we not already permit longer strings to be passed in?

            // If the string is not fully consumed, it had an invalid length
            if (pos != slice.Length)
                return false;

            return true;
        }

        /// <summary>
        /// Converts the <see cref="Sha1"/> or <see cref="Sha256"/> instance to a string using the 'N' format,
        /// and returns the value split into two tokens.
        /// </summary>
        /// <param name="prefixLength">The length of the first token.</param>
        /// <returns></returns>
        internal static KeyValuePair<string, string> Split(in ReadOnlySpan<byte> sha, int prefixLength)
        {
            var byteLength = sha.Length; // 20|32
            Debug.Assert(byteLength == Sha1.ByteLength || byteLength == Sha256.ByteLength);

            // Text is treated as 5|8 groups of 8 chars (5|8 x 4 bytes)
            var hexLength = byteLength * 2;

            Span<char> span = stackalloc char[hexLength];

            var pos = 0;
            for (var i = 0; i < byteLength; i++) // 20|32
            {
                // Each byte is two hexits
                var byt = sha[i];
                span[pos++] = HexChars[byt >> 4]; // == b / 16
                span[pos++] = HexChars[byt & 15]; // == b % 16
            }

            if (prefixLength >= hexLength)
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
            var extra = new string(span.Slice(prefixLength, hexLength - prefixLength));

            var kvp = new KeyValuePair<string, string>(prefix, extra);
            return kvp;
        }

        /// <summary>
        /// Returns a string representation of the <see cref="Sha1"/> or <see cref="Sha256"/> instance using the 'N' format.
        /// </summary>
        /// <returns></returns>
        internal static string ToString(in ReadOnlySpan<byte> sha)
        {
            var byteLength = sha.Length; // 20|32
            Debug.Assert(byteLength == Sha1.ByteLength || byteLength == Sha256.ByteLength);

            // Text is treated as 5|8 groups of 8 chars (5|8 x 4 bytes)
            var hexLength = byteLength * 2; // 40|64
            Span<char> span = stackalloc char[hexLength];

            var pos = 0;
            for (var i = 0; i < byteLength; i++) // 20|32
            {
                // Each byte is two hexits
                var byt = sha[i];
                span[pos++] = HexChars[byt >> 4]; // == b / 16
                span[pos++] = HexChars[byt & 15]; // == b % 16
            }

            var str = new string(span);
            return str;
        }

        /// <summary>
        /// Returns a string representation of the <see cref="Sha1"/> or <see cref="Sha256"/> instance.
        /// N: a9993e364706816aba3e25717850c26c9cd0d89d, cdc76e5c9914fb9281a1c7e284d73e67f1809a48a497200e046d39ccc7112cd0
        /// D: a9993e36-4706816a-ba3e2571-7850c26c-9cd0d89d, cdc76e5c-9914fb92-81a1c7e2-84d73e67-f1809a48-a497200e-046d39cc-c7112cd0
        /// S: a9993e36 4706816a ba3e2571 7850c26c 9cd0d89d, cdc76e5c 9914fb92 81a1c7e2 84d73e67 f1809a48 a497200e 046d39cc c7112cd0
        /// </summary>
        /// <param name="sha"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        internal static string ToString(in ReadOnlySpan<byte> sha, in char separator)
        {
            Debug.Assert(separator == '-' || separator == ' ');

            var byteLength = sha.Length; // 20|32
            Debug.Assert(byteLength == Sha1.ByteLength || byteLength == Sha256.ByteLength);

            // Text is treated as 5|8 groups of 8 chars (5|8 x 4 bytes) with 4|7 separators
            var hexLength = byteLength * 2; // 40|64
            var n = hexLength / 8; // 5|8
            Span<char> span = stackalloc char[hexLength + n - 1]; // + 4|7

            var pos = 0;
            var sep = 8;
            for (var i = 0; i < byteLength; i++) // 20|32
            {
                // Each byte is two hexits (convention is lowercase)
                var byt = sha[i];
                span[pos++] = HexChars[byt >> 4]; // == b / 16
                span[pos++] = HexChars[byt & 15]; // == b % 16

                // Append a separator if required
                if (pos == sep) // pos >= 2, sep = 0|N
                {
                    span[pos++] = separator;

                    sep = pos + 8;
                    if (sep >= span.Length)
                        sep = 0; // Prevent IndexOutOfRangeException
                }
            }

            var str = new string(span);
            return str;
        }
    }
}
