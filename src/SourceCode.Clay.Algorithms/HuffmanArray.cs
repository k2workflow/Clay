// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace SourceCode.Clay.Algorithms
{
    internal static class HuffmanArray
    {
        private static readonly (uint code, int bitLength)[] s_encodingTable = new (uint code, int bitLength)[]
        {
            (0b11111111_11000000_00000000_00000000, 13),
            (0b11111111_11111111_10110000_00000000, 23),
            (0b11111111_11111111_11111110_00100000, 28),
            (0b11111111_11111111_11111110_00110000, 28),
            (0b11111111_11111111_11111110_01000000, 28),
            (0b11111111_11111111_11111110_01010000, 28),
            (0b11111111_11111111_11111110_01100000, 28),
            (0b11111111_11111111_11111110_01110000, 28),
            (0b11111111_11111111_11111110_10000000, 28),
            (0b11111111_11111111_11101010_00000000, 24),
            (0b11111111_11111111_11111111_11110000, 30),
            (0b11111111_11111111_11111110_10010000, 28),
            (0b11111111_11111111_11111110_10100000, 28),
            (0b11111111_11111111_11111111_11110100, 30),
            (0b11111111_11111111_11111110_10110000, 28),
            (0b11111111_11111111_11111110_11000000, 28),
            (0b11111111_11111111_11111110_11010000, 28),
            (0b11111111_11111111_11111110_11100000, 28),
            (0b11111111_11111111_11111110_11110000, 28),
            (0b11111111_11111111_11111111_00000000, 28),
            (0b11111111_11111111_11111111_00010000, 28),
            (0b11111111_11111111_11111111_00100000, 28),
            (0b11111111_11111111_11111111_11111000, 30),
            (0b11111111_11111111_11111111_00110000, 28),
            (0b11111111_11111111_11111111_01000000, 28),
            (0b11111111_11111111_11111111_01010000, 28),
            (0b11111111_11111111_11111111_01100000, 28),
            (0b11111111_11111111_11111111_01110000, 28),
            (0b11111111_11111111_11111111_10000000, 28),
            (0b11111111_11111111_11111111_10010000, 28),
            (0b11111111_11111111_11111111_10100000, 28),
            (0b11111111_11111111_11111111_10110000, 28),
            (0b01010000_00000000_00000000_00000000, 6),
            (0b11111110_00000000_00000000_00000000, 10),
            (0b11111110_01000000_00000000_00000000, 10),
            (0b11111111_10100000_00000000_00000000, 12),
            (0b11111111_11001000_00000000_00000000, 13),
            (0b01010100_00000000_00000000_00000000, 6),
            (0b11111000_00000000_00000000_00000000, 8),
            (0b11111111_01000000_00000000_00000000, 11),
            (0b11111110_10000000_00000000_00000000, 10),
            (0b11111110_11000000_00000000_00000000, 10),
            (0b11111001_00000000_00000000_00000000, 8),
            (0b11111111_01100000_00000000_00000000, 11),
            (0b11111010_00000000_00000000_00000000, 8),
            (0b01011000_00000000_00000000_00000000, 6),
            (0b01011100_00000000_00000000_00000000, 6),
            (0b01100000_00000000_00000000_00000000, 6),
            (0b00000000_00000000_00000000_00000000, 5),
            (0b00001000_00000000_00000000_00000000, 5),
            (0b00010000_00000000_00000000_00000000, 5),
            (0b01100100_00000000_00000000_00000000, 6),
            (0b01101000_00000000_00000000_00000000, 6),
            (0b01101100_00000000_00000000_00000000, 6),
            (0b01110000_00000000_00000000_00000000, 6),
            (0b01110100_00000000_00000000_00000000, 6),
            (0b01111000_00000000_00000000_00000000, 6),
            (0b01111100_00000000_00000000_00000000, 6),
            (0b10111000_00000000_00000000_00000000, 7),
            (0b11111011_00000000_00000000_00000000, 8),
            (0b11111111_11111000_00000000_00000000, 15),
            (0b10000000_00000000_00000000_00000000, 6),
            (0b11111111_10110000_00000000_00000000, 12),
            (0b11111111_00000000_00000000_00000000, 10),
            (0b11111111_11010000_00000000_00000000, 13),
            (0b10000100_00000000_00000000_00000000, 6),
            (0b10111010_00000000_00000000_00000000, 7),
            (0b10111100_00000000_00000000_00000000, 7),
            (0b10111110_00000000_00000000_00000000, 7),
            (0b11000000_00000000_00000000_00000000, 7),
            (0b11000010_00000000_00000000_00000000, 7),
            (0b11000100_00000000_00000000_00000000, 7),
            (0b11000110_00000000_00000000_00000000, 7),
            (0b11001000_00000000_00000000_00000000, 7),
            (0b11001010_00000000_00000000_00000000, 7),
            (0b11001100_00000000_00000000_00000000, 7),
            (0b11001110_00000000_00000000_00000000, 7),
            (0b11010000_00000000_00000000_00000000, 7),
            (0b11010010_00000000_00000000_00000000, 7),
            (0b11010100_00000000_00000000_00000000, 7),
            (0b11010110_00000000_00000000_00000000, 7),
            (0b11011000_00000000_00000000_00000000, 7),
            (0b11011010_00000000_00000000_00000000, 7),
            (0b11011100_00000000_00000000_00000000, 7),
            (0b11011110_00000000_00000000_00000000, 7),
            (0b11100000_00000000_00000000_00000000, 7),
            (0b11100010_00000000_00000000_00000000, 7),
            (0b11100100_00000000_00000000_00000000, 7),
            (0b11111100_00000000_00000000_00000000, 8),
            (0b11100110_00000000_00000000_00000000, 7),
            (0b11111101_00000000_00000000_00000000, 8),
            (0b11111111_11011000_00000000_00000000, 13),
            (0b11111111_11111110_00000000_00000000, 19),
            (0b11111111_11100000_00000000_00000000, 13),
            (0b11111111_11110000_00000000_00000000, 14),
            (0b10001000_00000000_00000000_00000000, 6),
            (0b11111111_11111010_00000000_00000000, 15),
            (0b00011000_00000000_00000000_00000000, 5),
            (0b10001100_00000000_00000000_00000000, 6),
            (0b00100000_00000000_00000000_00000000, 5),
            (0b10010000_00000000_00000000_00000000, 6),
            (0b00101000_00000000_00000000_00000000, 5),
            (0b10010100_00000000_00000000_00000000, 6),
            (0b10011000_00000000_00000000_00000000, 6),
            (0b10011100_00000000_00000000_00000000, 6),
            (0b00110000_00000000_00000000_00000000, 5),
            (0b11101000_00000000_00000000_00000000, 7),
            (0b11101010_00000000_00000000_00000000, 7),
            (0b10100000_00000000_00000000_00000000, 6),
            (0b10100100_00000000_00000000_00000000, 6),
            (0b10101000_00000000_00000000_00000000, 6),
            (0b00111000_00000000_00000000_00000000, 5),
            (0b10101100_00000000_00000000_00000000, 6),
            (0b11101100_00000000_00000000_00000000, 7),
            (0b10110000_00000000_00000000_00000000, 6),
            (0b01000000_00000000_00000000_00000000, 5),
            (0b01001000_00000000_00000000_00000000, 5),
            (0b10110100_00000000_00000000_00000000, 6),
            (0b11101110_00000000_00000000_00000000, 7),
            (0b11110000_00000000_00000000_00000000, 7),
            (0b11110010_00000000_00000000_00000000, 7),
            (0b11110100_00000000_00000000_00000000, 7),
            (0b11110110_00000000_00000000_00000000, 7),
            (0b11111111_11111100_00000000_00000000, 15),
            (0b11111111_10000000_00000000_00000000, 11),
            (0b11111111_11110100_00000000_00000000, 14),
            (0b11111111_11101000_00000000_00000000, 13),
            (0b11111111_11111111_11111111_11000000, 28),
            (0b11111111_11111110_01100000_00000000, 20),
            (0b11111111_11111111_01001000_00000000, 22),
            (0b11111111_11111110_01110000_00000000, 20),
            (0b11111111_11111110_10000000_00000000, 20),
            (0b11111111_11111111_01001100_00000000, 22),
            (0b11111111_11111111_01010000_00000000, 22),
            (0b11111111_11111111_01010100_00000000, 22),
            (0b11111111_11111111_10110010_00000000, 23),
            (0b11111111_11111111_01011000_00000000, 22),
            (0b11111111_11111111_10110100_00000000, 23),
            (0b11111111_11111111_10110110_00000000, 23),
            (0b11111111_11111111_10111000_00000000, 23),
            (0b11111111_11111111_10111010_00000000, 23),
            (0b11111111_11111111_10111100_00000000, 23),
            (0b11111111_11111111_11101011_00000000, 24),
            (0b11111111_11111111_10111110_00000000, 23),
            (0b11111111_11111111_11101100_00000000, 24),
            (0b11111111_11111111_11101101_00000000, 24),
            (0b11111111_11111111_01011100_00000000, 22),
            (0b11111111_11111111_11000000_00000000, 23),
            (0b11111111_11111111_11101110_00000000, 24),
            (0b11111111_11111111_11000010_00000000, 23),
            (0b11111111_11111111_11000100_00000000, 23),
            (0b11111111_11111111_11000110_00000000, 23),
            (0b11111111_11111111_11001000_00000000, 23),
            (0b11111111_11111110_11100000_00000000, 21),
            (0b11111111_11111111_01100000_00000000, 22),
            (0b11111111_11111111_11001010_00000000, 23),
            (0b11111111_11111111_01100100_00000000, 22),
            (0b11111111_11111111_11001100_00000000, 23),
            (0b11111111_11111111_11001110_00000000, 23),
            (0b11111111_11111111_11101111_00000000, 24),
            (0b11111111_11111111_01101000_00000000, 22),
            (0b11111111_11111110_11101000_00000000, 21),
            (0b11111111_11111110_10010000_00000000, 20),
            (0b11111111_11111111_01101100_00000000, 22),
            (0b11111111_11111111_01110000_00000000, 22),
            (0b11111111_11111111_11010000_00000000, 23),
            (0b11111111_11111111_11010010_00000000, 23),
            (0b11111111_11111110_11110000_00000000, 21),
            (0b11111111_11111111_11010100_00000000, 23),
            (0b11111111_11111111_01110100_00000000, 22),
            (0b11111111_11111111_01111000_00000000, 22),
            (0b11111111_11111111_11110000_00000000, 24),
            (0b11111111_11111110_11111000_00000000, 21),
            (0b11111111_11111111_01111100_00000000, 22),
            (0b11111111_11111111_11010110_00000000, 23),
            (0b11111111_11111111_11011000_00000000, 23),
            (0b11111111_11111111_00000000_00000000, 21),
            (0b11111111_11111111_00001000_00000000, 21),
            (0b11111111_11111111_10000000_00000000, 22),
            (0b11111111_11111111_00010000_00000000, 21),
            (0b11111111_11111111_11011010_00000000, 23),
            (0b11111111_11111111_10000100_00000000, 22),
            (0b11111111_11111111_11011100_00000000, 23),
            (0b11111111_11111111_11011110_00000000, 23),
            (0b11111111_11111110_10100000_00000000, 20),
            (0b11111111_11111111_10001000_00000000, 22),
            (0b11111111_11111111_10001100_00000000, 22),
            (0b11111111_11111111_10010000_00000000, 22),
            (0b11111111_11111111_11100000_00000000, 23),
            (0b11111111_11111111_10010100_00000000, 22),
            (0b11111111_11111111_10011000_00000000, 22),
            (0b11111111_11111111_11100010_00000000, 23),
            (0b11111111_11111111_11111000_00000000, 26),
            (0b11111111_11111111_11111000_01000000, 26),
            (0b11111111_11111110_10110000_00000000, 20),
            (0b11111111_11111110_00100000_00000000, 19),
            (0b11111111_11111111_10011100_00000000, 22),
            (0b11111111_11111111_11100100_00000000, 23),
            (0b11111111_11111111_10100000_00000000, 22),
            (0b11111111_11111111_11110110_00000000, 25),
            (0b11111111_11111111_11111000_10000000, 26),
            (0b11111111_11111111_11111000_11000000, 26),
            (0b11111111_11111111_11111001_00000000, 26),
            (0b11111111_11111111_11111011_11000000, 27),
            (0b11111111_11111111_11111011_11100000, 27),
            (0b11111111_11111111_11111001_01000000, 26),
            (0b11111111_11111111_11110001_00000000, 24),
            (0b11111111_11111111_11110110_10000000, 25),
            (0b11111111_11111110_01000000_00000000, 19),
            (0b11111111_11111111_00011000_00000000, 21),
            (0b11111111_11111111_11111001_10000000, 26),
            (0b11111111_11111111_11111100_00000000, 27),
            (0b11111111_11111111_11111100_00100000, 27),
            (0b11111111_11111111_11111001_11000000, 26),
            (0b11111111_11111111_11111100_01000000, 27),
            (0b11111111_11111111_11110010_00000000, 24),
            (0b11111111_11111111_00100000_00000000, 21),
            (0b11111111_11111111_00101000_00000000, 21),
            (0b11111111_11111111_11111010_00000000, 26),
            (0b11111111_11111111_11111010_01000000, 26),
            (0b11111111_11111111_11111111_11010000, 28),
            (0b11111111_11111111_11111100_01100000, 27),
            (0b11111111_11111111_11111100_10000000, 27),
            (0b11111111_11111111_11111100_10100000, 27),
            (0b11111111_11111110_11000000_00000000, 20),
            (0b11111111_11111111_11110011_00000000, 24),
            (0b11111111_11111110_11010000_00000000, 20),
            (0b11111111_11111111_00110000_00000000, 21),
            (0b11111111_11111111_10100100_00000000, 22),
            (0b11111111_11111111_00111000_00000000, 21),
            (0b11111111_11111111_01000000_00000000, 21),
            (0b11111111_11111111_11100110_00000000, 23),
            (0b11111111_11111111_10101000_00000000, 22),
            (0b11111111_11111111_10101100_00000000, 22),
            (0b11111111_11111111_11110111_00000000, 25),
            (0b11111111_11111111_11110111_10000000, 25),
            (0b11111111_11111111_11110100_00000000, 24),
            (0b11111111_11111111_11110101_00000000, 24),
            (0b11111111_11111111_11111010_10000000, 26),
            (0b11111111_11111111_11101000_00000000, 23),
            (0b11111111_11111111_11111010_11000000, 26),
            (0b11111111_11111111_11111100_11000000, 27),
            (0b11111111_11111111_11111011_00000000, 26),
            (0b11111111_11111111_11111011_01000000, 26),
            (0b11111111_11111111_11111100_11100000, 27),
            (0b11111111_11111111_11111101_00000000, 27),
            (0b11111111_11111111_11111101_00100000, 27),
            (0b11111111_11111111_11111101_01000000, 27),
            (0b11111111_11111111_11111101_01100000, 27),
            (0b11111111_11111111_11111111_11100000, 28),
            (0b11111111_11111111_11111101_10000000, 27),
            (0b11111111_11111111_11111101_10100000, 27),
            (0b11111111_11111111_11111101_11000000, 27),
            (0b11111111_11111111_11111101_11100000, 27),
            (0b11111111_11111111_11111110_00000000, 27),
            (0b11111111_11111111_11111011_10000000, 26),
            (0b11111111_11111111_11111111_11111100, 30)
        };

        public static readonly short[][] s_decodingArray = new short[15][];

        public static (uint encoded, int bitLength) Encode(int data) => s_encodingTable[data];

        /// <summary>
        /// Decodes a Huffman encoded string from a byte array.
        /// </summary>
        /// <param name="src">The source byte array containing the encoded data.</param>
        /// <param name="offset">The offset in the byte array where the coded data starts.</param>
        /// <param name="count">The number of bytes to decode.</param>
        /// <param name="dst">The destination byte array to store the decoded data.</param>
        /// <returns>The number of decoded symbols.</returns>
        public static int Decode(byte[] src, int offset, int count, byte[] dst)
        {
            var i = offset;
            var j = 0;
            var lastDecodedBits = 0;
            var edgeIndex = count - 1;
            var decodingArray = s_decodingArray;
            var encodingTable = s_encodingTable;

            while (i < count)
            {
                var next = (uint)(src[i] << 24 + lastDecodedBits);
                if (i + 1 < src.Length)
                {
                    next |= (uint)(src[i + 1] << 16 + lastDecodedBits);

                    if (i + 2 < src.Length)
                    {
                        next |= (uint)(src[i + 2] << 8 + lastDecodedBits);

                        if (i + 3 < src.Length)
                        {
                            next |= (uint)(src[i + 3] << lastDecodedBits);

                            if (i + 4 < src.Length)
                            {
                                next |= (uint)(src[i + 4] >> (8 - lastDecodedBits));
                            }
                        }
                    }
                }

                var remainingBits = 8 - lastDecodedBits;

                // The remaining 7 or less bits are all 1, which is padding.
                // We specifically check that lastDecodedBits > 0 because padding
                // longer than 7 bits should be treated as a decoding error.
                // http://httpwg.org/specs/rfc7541.html#rfc.section.5.2
                if (i == edgeIndex && lastDecodedBits > 0)
                {
                    var ones = (uint)(int.MinValue >> remainingBits - 1);

                    if ((next & ones) == ones)
                        return j;
                }

                if (j == dst.Length)
                {
                    // Destination is too small.
                    throw new HuffmanDecodingException();
                }

                // The longest possible symbol size is 30 bits. If we're at the last 4 bytes
                // of the input, we need to make sure we pass the correct number of valid bits
                // left, otherwise the trailing 0s in next may form a valid symbol.
                var validBits = remainingBits + (edgeIndex - i) * 8;
                if (validBits > 30)
                    validBits = 30; // Equivalent to Math.Min(30, validBits)

                var ch = DecodeImpl(decodingArray, encodingTable, next, validBits, out var decodedBits);
                if (ch == -1 || ch == 256)
                {
                    // -1: No valid symbol could be decoded with the bits in next.

                    // 256: A Huffman-encoded string literal containing the EOS symbol MUST be treated as a decoding error.
                    // http://httpwg.org/specs/rfc7541.html#rfc.section.5.2
                    throw new HuffmanDecodingException();
                }

                dst[j++] = (byte)ch;

                // If we crossed a byte boundary, advance i so we start at the next byte that's not fully decoded.
                lastDecodedBits += decodedBits;
                i += lastDecodedBits / 8;

                // Modulo 8 since we only care about how many bits were decoded in the last byte that we processed.
                lastDecodedBits %= 8;
            }

            return j;
        }

        /// <summary>
        /// Decodes a single symbol from a 32-bit word.
        /// </summary>
        /// <param name="data">A 32-bit word containing a Huffman encoded symbol.</param>
        /// <param name="validBits">
        /// The number of bits in <paramref name="data"/> that may contain an encoded symbol.
        /// This is not the exact number of bits that encode the symbol. Instead, it prevents
        /// decoding the lower bits of <paramref name="data"/> if they don't contain any
        /// encoded data.
        /// </param>
        /// <param name="decodedBits">The number of bits decoded from <paramref name="data"/>.</param>
        /// <returns>The decoded symbol.</returns>
        public static int Decode(uint data, int validBits, out int decodedBits)
            => DecodeImpl(s_decodingArray, s_encodingTable, data, validBits, out decodedBits);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int DecodeImpl(short[][] decodingArray, (uint code, int bitLength)[] encodingTable, in uint data, in int validBits, out int decodedBits)
        {
            var arrayIndex = 0;

            // Decode in a max of 4
            // Grab data one byte at a time starting at the left

            // Unroll loop
            var value = DecodeImpl(decodingArray, encodingTable, (data >> 24) & 0xFF, ref arrayIndex, validBits, out decodedBits);
            if (value >= 0) return value;
            if (value == -2) return -1;

            value = DecodeImpl(decodingArray, encodingTable, (data >> 16) & 0xFF, ref arrayIndex, validBits, out decodedBits);
            if (value >= 0) return value;
            if (value == -2) return -1;

            value = DecodeImpl(decodingArray, encodingTable, (data >> 8) & 0xFF, ref arrayIndex, validBits, out decodedBits);
            if (value >= 0) return value;
            if (value == -2) return -1;

            value = DecodeImpl(decodingArray, encodingTable, (data >> 0) & 0xFF, ref arrayIndex, validBits, out decodedBits);
            if (value >= 0) return value;

            // no luck. signal to caller that we could not decode
            return -1;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int DecodeImpl(short[][] decodingArray, (uint code, int bitLength)[] encodingTable, in uint workingByte, ref int arrayIndex, in int validBits, out int decodedBits)
        {
            decodedBits = 0;

            // key into array
            var value = decodingArray[arrayIndex][workingByte];

            // if the value is positive then we have a pointer into the encoding table
            if (value >= 0)
            {
                var (_, bitLength) = encodingTable[value];
                if (bitLength > validBits)
                    return -2;  // we only found a value by incorporating bits beyond the the valid remaining length of the data stream

                decodedBits = bitLength;
                return value;   // the index is also the value
            }

            // pointer to the next array will be stored as a negative
            arrayIndex = -value;

            // no luck. signal to caller that we could not decode
            return -1;
        }

        static HuffmanArray()
        {
            short nextAvailableSubIndex = 1;
            
            // loop through each entry in the encoding table and create entries for it in our decoding array
            for (short i = 0; i < s_encodingTable.Length; i++)
            {
                var (code, bitLength) = s_encodingTable[i];    // keep from having to do "s_encodingTable[i]" everywhere
                var currentArrayIndex = 0;              // which array are we working with

                // loop for however many bytes the value occupies
                for (var j = 0; j <= Math.Ceiling(bitLength / 8.0); j++)
                {
                    if (s_decodingArray[currentArrayIndex] == null)
                        s_decodingArray[currentArrayIndex] = new short[256];

                    var byteOffset = 8 * (3 - j);       // how many bits is the working byte offset from the right
                    var totalLength = 8 * (j + 1);      // how many bits of the entry can consume total so far

                    var codeByte = (code >> byteOffset) & 0xFF;  // extract the working byte and shift it all the way to the right

                    // we can finish the entry this time around. store the remaning bits and bail on the loop
                    if (bitLength <= totalLength)
                    {
                        // we need to store all permutations of the bits that are beyond the length of the code
                        var loopMax = 0x1 << (totalLength - bitLength); // have to create entries for all of these values
                        for (uint k = 0; k < loopMax; k++)
                            s_decodingArray[currentArrayIndex][codeByte + k] = i;   // each entry returns the same index into the encoding table

                        break;  // we're done with this entry. bail on the loop
                    }
                    // else: we need to split the entry into one or more sub-arrays

                    // let's see if anyone before us has already claimed a sub-array with our bit pattern
                    var subArrayIndex = s_decodingArray[currentArrayIndex][codeByte];

                    // negative values are used as pointers to the next array. zeros are unused. positive values are a successful decode
                    if (subArrayIndex < 0)
                        subArrayIndex = (short)-subArrayIndex;
                    else
                    {
                        subArrayIndex = nextAvailableSubIndex++;    // if no one has our bit battern then we'll stake our claim on the next available array
                        s_decodingArray[currentArrayIndex][codeByte] = (short)-subArrayIndex;  // blaze the trail for the next guy
                    }

                    currentArrayIndex = subArrayIndex;  // we've left a pointer behind us and we're moving on to the next array
                }
            }
        }

        internal static void VerifyDecodingArray()
        {
            for (var i = 0; i < s_encodingTable.Length; i++)
            {
                (var code, var bitLength) = s_encodingTable[i];
                var decoded = Decode(code, bitLength, out var decodedBits);
                Debug.Assert(-1 != decoded);
                Debug.Assert(bitLength == decodedBits);
                Debug.Assert(i == decoded);
            }
        }
    }
}