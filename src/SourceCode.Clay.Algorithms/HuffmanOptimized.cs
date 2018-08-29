#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

namespace SourceCode.Clay.Algorithms
{
    /// <summary>
    /// 
    /// </summary>
    public static class HuffmanOptimized
    {
        // TODO: this can be constructed from _decodingTable
        private static readonly (uint code, int bitLength)[] s_encodingTable = new (uint code, int bitLength)[]
        {
            // 0
            (0b11111111_11000000_00000000_00000000, 13),

            // 1
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

            // 11
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

            // 21
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

            // 31
            (0b11111111_11111111_11111111_10110000, 28),
            (0b01010000_00000000_00000000_00000000, 6),  // <space>
            (0b11111110_00000000_00000000_00000000, 10), // !
            (0b11111110_01000000_00000000_00000000, 10), // "
            (0b11111111_10100000_00000000_00000000, 12), // #
            (0b11111111_11001000_00000000_00000000, 13), // $
            (0b01010100_00000000_00000000_00000000, 6),  // %
            (0b11111000_00000000_00000000_00000000, 8),  // &
            (0b11111111_01000000_00000000_00000000, 11), // '
            (0b11111110_10000000_00000000_00000000, 10), // (

            // 41
            (0b11111110_11000000_00000000_00000000, 10), // )
            (0b11111001_00000000_00000000_00000000, 8),  // *
            (0b11111111_01100000_00000000_00000000, 11), // +
            (0b11111010_00000000_00000000_00000000, 8),  // ,
            (0b01011000_00000000_00000000_00000000, 6),  // -
            (0b01011100_00000000_00000000_00000000, 6),  // .
            (0b01100000_00000000_00000000_00000000, 6),  // /
            (0b00000000_00000000_00000000_00000000, 5),  // 0
            (0b00001000_00000000_00000000_00000000, 5),  // 1
            (0b00010000_00000000_00000000_00000000, 5),  // 2

            // 51
            (0b01100100_00000000_00000000_00000000, 6),  // 3
            (0b01101000_00000000_00000000_00000000, 6),  // 4
            (0b01101100_00000000_00000000_00000000, 6),  // 5
            (0b01110000_00000000_00000000_00000000, 6),  // 6
            (0b01110100_00000000_00000000_00000000, 6),  // 7
            (0b01111000_00000000_00000000_00000000, 6),  // 8
            (0b01111100_00000000_00000000_00000000, 6),  // 9
            (0b10111000_00000000_00000000_00000000, 7),  // :
            (0b11111011_00000000_00000000_00000000, 8),  // ;
            (0b11111111_11111000_00000000_00000000, 15), // <

            // 61
            (0b10000000_00000000_00000000_00000000, 6),  // =
            (0b11111111_10110000_00000000_00000000, 12), // >
            (0b11111111_00000000_00000000_00000000, 10), // ?
            (0b11111111_11010000_00000000_00000000, 13), // @
            (0b10000100_00000000_00000000_00000000, 6),  // A
            (0b10111010_00000000_00000000_00000000, 7),  // B
            (0b10111100_00000000_00000000_00000000, 7),  // C
            (0b10111110_00000000_00000000_00000000, 7),  // D
            (0b11000000_00000000_00000000_00000000, 7),  // E
            (0b11000010_00000000_00000000_00000000, 7),  // F
                                                         
            // 71                                        
            (0b11000100_00000000_00000000_00000000, 7),  // G
            (0b11000110_00000000_00000000_00000000, 7),  // H
            (0b11001000_00000000_00000000_00000000, 7),  // I
            (0b11001010_00000000_00000000_00000000, 7),  // J
            (0b11001100_00000000_00000000_00000000, 7),  // K
            (0b11001110_00000000_00000000_00000000, 7),  // L
            (0b11010000_00000000_00000000_00000000, 7),  // M
            (0b11010010_00000000_00000000_00000000, 7),  // N
            (0b11010100_00000000_00000000_00000000, 7),  // O
            (0b11010110_00000000_00000000_00000000, 7),  // P
                                                         
            // 81                                        
            (0b11011000_00000000_00000000_00000000, 7),  // Q
            (0b11011010_00000000_00000000_00000000, 7),  // R
            (0b11011100_00000000_00000000_00000000, 7),  // S
            (0b11011110_00000000_00000000_00000000, 7),  // T
            (0b11100000_00000000_00000000_00000000, 7),  // U
            (0b11100010_00000000_00000000_00000000, 7),  // V
            (0b11100100_00000000_00000000_00000000, 7),  // W
            (0b11111100_00000000_00000000_00000000, 8),  // X
            (0b11100110_00000000_00000000_00000000, 7),  // Y
            (0b11111101_00000000_00000000_00000000, 8),  // Z 

            // 91
            (0b11111111_11011000_00000000_00000000, 13), // [
            (0b11111111_11111110_00000000_00000000, 19), // \
            (0b11111111_11100000_00000000_00000000, 13), // ]
            (0b11111111_11110000_00000000_00000000, 14), // ^
            (0b10001000_00000000_00000000_00000000, 6),  // _
            (0b11111111_11111010_00000000_00000000, 15), // `
            (0b00011000_00000000_00000000_00000000, 5),  // a
            (0b10001100_00000000_00000000_00000000, 6),  // b
            (0b00100000_00000000_00000000_00000000, 5),  // c
            (0b10010000_00000000_00000000_00000000, 6),  // d

            // 101
            (0b00101000_00000000_00000000_00000000, 5),  // e
            (0b10010100_00000000_00000000_00000000, 6),  // f
            (0b10011000_00000000_00000000_00000000, 6),  // g
            (0b10011100_00000000_00000000_00000000, 6),  // h
            (0b00110000_00000000_00000000_00000000, 5),  // i
            (0b11101000_00000000_00000000_00000000, 7),  // j
            (0b11101010_00000000_00000000_00000000, 7),  // k
            (0b10100000_00000000_00000000_00000000, 6),  // l
            (0b10100100_00000000_00000000_00000000, 6),  // m
            (0b10101000_00000000_00000000_00000000, 6),  // n

            // 111
            (0b00111000_00000000_00000000_00000000, 5),  // o
            (0b10101100_00000000_00000000_00000000, 6),  // p
            (0b11101100_00000000_00000000_00000000, 7),  // q
            (0b10110000_00000000_00000000_00000000, 6),  // r
            (0b01000000_00000000_00000000_00000000, 5),  // s
            (0b01001000_00000000_00000000_00000000, 5),  // t
            (0b10110100_00000000_00000000_00000000, 6),  // u
            (0b11101110_00000000_00000000_00000000, 7),  // v
            (0b11110000_00000000_00000000_00000000, 7),  // w
            (0b11110010_00000000_00000000_00000000, 7),  // x

            // 121
            (0b11110100_00000000_00000000_00000000, 7),  // y
            (0b11110110_00000000_00000000_00000000, 7),  // z
            (0b11111111_11111100_00000000_00000000, 15), // {
            (0b11111111_10000000_00000000_00000000, 11), // |
            (0b11111111_11110100_00000000_00000000, 14), // }
            (0b11111111_11101000_00000000_00000000, 13), // ~
            (0b11111111_11111111_11111111_11000000, 28),
            (0b11111111_11111110_01100000_00000000, 20),
            (0b11111111_11111111_01001000_00000000, 22),
            (0b11111111_11111110_01110000_00000000, 20),

            // 131
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

            // 141
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

            // 151
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

            // 161
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

            // 171
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

            // 181
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

            // 191
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

            // 201
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

            // 211
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

            // 221
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

            // 231
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

            // 241
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

            // 251
            (0b11111111_11111111_11111101_10100000, 27),
            (0b11111111_11111111_11111101_11000000, 27),
            (0b11111111_11111111_11111101_11100000, 27),
            (0b11111111_11111111_11111110_00000000, 27),
            (0b11111111_11111111_11111011_10000000, 26),
            (0b11111111_11111111_11111111_11111100, 30)
        };

        private static readonly (byte codeLength, byte deltaLength, ushort[] codes)[] s_decodingTable = new[]
        {
            ((byte)05, (byte)0, new[] { (ushort)048, (ushort)049, (ushort)050, (ushort)097, (ushort)099, (ushort)101, (ushort)105, (ushort)111, (ushort)115, (ushort)116 }), // deltaLength must be 0 for related optimizations to work
            ((byte)06, (byte)1, new[] { (ushort)032, (ushort)037, (ushort)045, (ushort)046, (ushort)047, (ushort)051, (ushort)052, (ushort)053, (ushort)054, (ushort)055, (ushort)056, (ushort)057, (ushort)061, (ushort)065, (ushort)095, (ushort)098, (ushort)100, (ushort)102, (ushort)103, (ushort)104, (ushort)108, (ushort)109, (ushort)110, (ushort)112, (ushort)114, (ushort)117 }),
            ((byte)07, (byte)1, new[] { (ushort)058, (ushort)066, (ushort)067, (ushort)068, (ushort)069, (ushort)070, (ushort)071, (ushort)072, (ushort)073, (ushort)074, (ushort)075, (ushort)076, (ushort)077, (ushort)078, (ushort)079, (ushort)080, (ushort)081, (ushort)082, (ushort)083, (ushort)084, (ushort)085, (ushort)086, (ushort)087, (ushort)089, (ushort)106, (ushort)107, (ushort)113, (ushort)118, (ushort)119, (ushort)120, (ushort)121, (ushort)122 }),
            ((byte)08, (byte)1, new[] { (ushort)038, (ushort)042, (ushort)044, (ushort)059, (ushort)088, (ushort)090 }),
            ((byte)10, (byte)2, new[] { (ushort)033, (ushort)034, (ushort)040, (ushort)041, (ushort)063 }),
            ((byte)11, (byte)1, new[] { (ushort)039, (ushort)043, (ushort)124 }),
            ((byte)12, (byte)1, new[] { (ushort)035, (ushort)062 }),
            ((byte)13, (byte)1, new[] { (ushort)000, (ushort)036, (ushort)064, (ushort)091, (ushort)093, (ushort)126 }),
            ((byte)14, (byte)1, new[] { (ushort)094, (ushort)125 }),
            ((byte)15, (byte)1, new[] { (ushort)060, (ushort)096, (ushort)123 }),
            ((byte)19, (byte)4, new[] { (ushort)092, (ushort)195, (ushort)208 }),
            ((byte)20, (byte)1, new[] { (ushort)128, (ushort)130, (ushort)131, (ushort)162, (ushort)184, (ushort)194, (ushort)224, (ushort)226 }),
            ((byte)21, (byte)1, new[] { (ushort)153, (ushort)161, (ushort)167, (ushort)172, (ushort)176, (ushort)177, (ushort)179, (ushort)209, (ushort)216, (ushort)217, (ushort)227, (ushort)229, (ushort)230 }),
            ((byte)22, (byte)1, new[] { (ushort)129, (ushort)132, (ushort)133, (ushort)134, (ushort)136, (ushort)146, (ushort)154, (ushort)156, (ushort)160, (ushort)163, (ushort)164, (ushort)169, (ushort)170, (ushort)173, (ushort)178, (ushort)181, (ushort)185, (ushort)186, (ushort)187, (ushort)189, (ushort)190, (ushort)196, (ushort)198, (ushort)228, (ushort)232, (ushort)233 }),
            ((byte)23, (byte)1, new[] { (ushort)001, (ushort)135, (ushort)137, (ushort)138, (ushort)139, (ushort)140, (ushort)141, (ushort)143, (ushort)147, (ushort)149, (ushort)150, (ushort)151, (ushort)152, (ushort)155, (ushort)157, (ushort)158, (ushort)165, (ushort)166, (ushort)168, (ushort)174, (ushort)175, (ushort)180, (ushort)182, (ushort)183, (ushort)188, (ushort)191, (ushort)197, (ushort)231, (ushort)239 }),
            ((byte)24, (byte)1, new[] { (ushort)009, (ushort)142, (ushort)144, (ushort)145, (ushort)148, (ushort)159, (ushort)171, (ushort)206, (ushort)215, (ushort)225, (ushort)236, (ushort)237 }),
            ((byte)25, (byte)1, new[] { (ushort)199, (ushort)207, (ushort)234, (ushort)235 }),
            ((byte)26, (byte)1, new[] { (ushort)192, (ushort)193, (ushort)200, (ushort)201, (ushort)202, (ushort)205, (ushort)210, (ushort)213, (ushort)218, (ushort)219, (ushort)238, (ushort)240, (ushort)242, (ushort)243, (ushort)255 }),
            ((byte)27, (byte)1, new[] { (ushort)203, (ushort)204, (ushort)211, (ushort)212, (ushort)214, (ushort)221, (ushort)222, (ushort)223, (ushort)241, (ushort)244, (ushort)245, (ushort)246, (ushort)247, (ushort)248, (ushort)250, (ushort)251, (ushort)252, (ushort)253, (ushort)254 }),
            ((byte)28, (byte)1, new[] { (ushort)002, (ushort)003, (ushort)004, (ushort)005, (ushort)006, (ushort)007, (ushort)008, (ushort)011, (ushort)012, (ushort)014, (ushort)015, (ushort)016, (ushort)017, (ushort)018, (ushort)019, (ushort)020, (ushort)021, (ushort)023, (ushort)024, (ushort)025, (ushort)026, (ushort)027, (ushort)028, (ushort)029, (ushort)030, (ushort)031, (ushort)127, (ushort)220, (ushort)249 }),
            ((byte)30, (byte)2, new[] { (ushort)010, (ushort)013, (ushort)022, (ushort)256 })
        };

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
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
            var edgeIndex = count - 1; // Cache common calc
            while (i <= edgeIndex)
            {
                var remainingBits = 8 - lastDecodedBits; // Cache common calc

                var next = (uint)(src[i] << 24 + lastDecodedBits);
                if (i + 1 < src.Length)
                {
                    next |= (uint)(src[i + 1] << 16 + lastDecodedBits);

                    if (i + 2 < src.Length)
                    {
                        next |= (uint)(src[i + 2] << 8 + lastDecodedBits);

                        if (i + 3 < src.Length)
                            next |= (uint)(src[i + 3] << lastDecodedBits);
                    }
                }

                var ones = (uint)(int.MinValue >> remainingBits - 1);
                if (i == edgeIndex && lastDecodedBits > 0 && (next & ones) == ones)
                {
                    // The remaining 7 or less bits are all 1, which is padding.
                    // We specifically check that lastDecodedBits > 0 because padding
                    // longer than 7 bits should be treated as a decoding error.
                    // http://httpwg.org/specs/rfc7541.html#rfc.section.5.2
                    break;
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

                var ch = Decode(next, validBits, out var decodedBits);

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
        {
            // The code below implements the decoding logic for a canonical Huffman code.
            //
            // To decode a symbol, we scan the decoding table, which is sorted by ascending symbol bit length.
            // For each bit length b, we determine the maximum b-bit encoded value, plus one (that is codeMax).
            // This is done with the following logic:
            //
            // if we're at the first entry in the table,
            //    codeMax = the # of symbols encoded in b bits
            // else,
            //    left-shift codeMax by the difference between b and the previous entry's bit length,
            //    then increment codeMax by the # of symbols encoded in b bits
            //
            // Next, we look at the value v encoded in the highest b bits of data. If v is less than codeMax,
            // those bits correspond to a Huffman encoded symbol. We find the corresponding decoded
            // symbol in the list of values associated with bit length b in the decoding table by indexing it
            // with codeMax - v.

            var codeMax = 0;

            for (var i = 0; i < s_decodingTable.Length; i++)
            {
                // deltaLength is precomputed codeMax delta
                var (codeLength, deltaLength, codes) = s_decodingTable[i];

                // Move check out of for-loop to leverage cached value for codeLength
                if (codeLength > validBits)
                    break;

                // Mitigate the if (i > 0) branch by ensuring s_decodingTable[0].deltaLength==0
                codeMax = (codeMax << deltaLength) + codes.Length;

                var mask = int.MinValue >> (codeLength - 1);
                var masked = (data & mask) >> (32 - codeLength);

                if (masked < codeMax)
                {
                    decodedBits = codeLength;
                    var result = codes[codes.Length - (codeMax - masked)];
                    return result;
                }
            }

            decodedBits = 0;
            return -1;
        }
    }
}