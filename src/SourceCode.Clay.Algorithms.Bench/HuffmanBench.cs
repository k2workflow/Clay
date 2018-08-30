// Derived from:
// https://raw.githubusercontent.com/aspnet/KestrelHttpServer/64127e6c766b221cf147383c16079d3b7aad2ded/test/Kestrel.Core.Tests/HuffmanTests.cs

// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using BenchmarkDotNet.Attributes;
using System.Buffers;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SourceCode.Clay.Algorithms.Bench
{
    [MemoryDiagnoser]
    public class HuffmanBench
    {
        private const int _iterations = 1000;

        private const int _simpleCount = 25;
        private static readonly (byte[] encoded, byte[] expected)[] _simpleData = new (byte[], byte[])[_simpleCount]
        {
            // Single 5-bit symbol
            ( new byte[] { 0x07 }, Encoding.ASCII.GetBytes("0") ),
            // Single 6-bit symbol
            ( new byte[] { 0x57 }, Encoding.ASCII.GetBytes("%") ),
            // Single 7-bit symbol
            ( new byte[] { 0xb9 }, Encoding.ASCII.GetBytes(":") ),
            // Single 8-bit symbol
            ( new byte[] { 0xf8 }, Encoding.ASCII.GetBytes("&") ),
            // Single 10-bit symbol
            ( new byte[] { 0xfe, 0x3f }, Encoding.ASCII.GetBytes("!") ),
            // Single 11-bit symbol
            ( new byte[] { 0xff, 0x7f }, Encoding.ASCII.GetBytes("+") ),
            // Single 12-bit symbol
            ( new byte[] { 0xff, 0xaf }, Encoding.ASCII.GetBytes("#") ),
            // Single 13-bit symbol
            ( new byte[] { 0xff, 0xcf }, Encoding.ASCII.GetBytes("$") ),
            // Single 14-bit symbol
            ( new byte[] { 0xff, 0xf3 }, Encoding.ASCII.GetBytes("^") ),
            // Single 15-bit symbol
            ( new byte[] { 0xff, 0xf9 }, Encoding.ASCII.GetBytes("<") ),
            // Single 19-bit symbol
            ( new byte[] { 0xff, 0xfe, 0x1f }, Encoding.ASCII.GetBytes("\\") ),
            // Single 20-bit symbol
            ( new byte[] { 0xff, 0xfe, 0x6f }, new byte[] { 0x80 } ),
            // Single 21-bit symbol
            ( new byte[] { 0xff, 0xfe, 0xe7 }, new byte[] { 0x99 } ),
            // Single 22-bit symbol
            ( new byte[] { 0xff, 0xff, 0x4b }, new byte[] { 0x81 } ),
            // Single 23-bit symbol
            ( new byte[] { 0xff, 0xff, 0xb1 }, new byte[] { 0x01 } ),
            // Single 24-bit symbol
            ( new byte[] { 0xff, 0xff, 0xea }, new byte[] { 0x09 } ),
            // Single 25-bit symbol
            ( new byte[] { 0xff, 0xff, 0xf6, 0x7f }, new byte[] { 0xc7 } ),
            // Single 26-bit symbol
            ( new byte[] { 0xff, 0xff, 0xf8, 0x3f }, new byte[] { 0xc0 } ),
            // Single 27-bit symbol
            ( new byte[] { 0xff, 0xff, 0xfb, 0xdf }, new byte[] { 0xcb } ),
            // Single 28-bit symbol
            ( new byte[] { 0xff, 0xff, 0xfe, 0x2f }, new byte[] { 0x02 } ),
            // Single 30-bit symbol
            ( new byte[] { 0xff, 0xff, 0xff, 0xf3 }, new byte[] { 0x0a } ),

            //               h      e         l          l      o         *
            ( new byte[] { 0b100111_00, 0b101_10100, 0b0_101000_0, 0b0111_1111 }, Encoding.ASCII.GetBytes("hello") ),

            //               h      e         l          l      o         <space>    W           O          R            L            D     \                              >                *
            ( new byte[] { 0b100111_00, 0b101_10100, 0b0_101000_0, 0b0111_0101, 0b00_111001, 0b0_1101010, 0b1101101_1, 0b100111_10, 0b11111_111, 0b11111111, 0b11110000, 0b11111111, 0b1011_1111 }, Encoding.ASCII.GetBytes(@"hello WORLD\>") ),

            // Sequences that uncovered errors
            ( new byte[] { 0xb6, 0xb9, 0xac, 0x1c, 0x85, 0x58, 0xd5, 0x20, 0xa4, 0xb6, 0xc2, 0xad, 0x61, 0x7b, 0x5a, 0x54, 0x25, 0x1f }, Encoding.ASCII.GetBytes("upgrade-insecure-requests") ),
            ( new byte[] { 0xfe, 0x53 }, Encoding.ASCII.GetBytes("\"t") )
        };

        private const int _headerCount = 350; // From line-count in HuffmanHeaders.txt
        private static readonly (byte[] encoded, string decodedValue)[] s_headerData = new (byte[], string)[_headerCount];

        public HuffmanBench()
        { }

        [GlobalSetup]
        public void Setup()
        {
            using (var reader = File.OpenText(@".\HuffmanHeaders.txt"))
            {
                var i = 0;
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    var bytes = Encode(line);
                    s_headerData[i++] = (bytes, line);
                }
            }
        }

        [Benchmark(Baseline = false, OperationsPerInvoke = (_simpleCount + _headerCount) * _iterations)]
        public ulong OrigOpt()
        {
            var sum = 0ul;

            var rented = ArrayPool<byte>.Shared.Rent(4096);
            {
                for (var j = 0; j < _iterations; j++)
                {
                    // Simple
                    for (var i = 0; i < _simpleData.Length; i++)
                    {
                        var encoded = _simpleData[i].encoded;
                        //var expected = _test[i].expected;

                        var actualLength = HuffmanOrigOpt.Decode(encoded, 0, encoded.Length, rented);
                        sum += (uint)actualLength;
                    }

                    // Headers
                    for (var i = 0; i < s_headerData.Length; i++)
                    {
                        var encoded = s_headerData[i].encoded;

                        var actualLength = HuffmanOrigOpt.Decode(encoded, 0, encoded.Length, rented);
                        sum += (uint)actualLength;
                    }
                }
            }
            ArrayPool<byte>.Shared.Return(rented);

            return sum;
        }

        [Benchmark(Baseline = true, OperationsPerInvoke = (_simpleCount + _headerCount) * _iterations)]
        public ulong Orig()
        {
            var sum = 0ul;

            var rented = ArrayPool<byte>.Shared.Rent(4096);
            {
                for (var j = 0; j < _iterations; j++)
                {
                    // Simple
                    for (var i = 0; i < _simpleData.Length; i++)
                    {
                        var encoded = _simpleData[i].encoded;
                        //var expected = _test[i].expected;

                        var actualLength = HuffmanOrig.Decode(encoded, 0, encoded.Length, rented);
                        sum += (uint)actualLength;
                    }

                    // Headers
                    for (var i = 0; i < s_headerData.Length; i++)
                    {
                        var encoded = s_headerData[i].encoded;

                        var actualLength = HuffmanOrig.Decode(encoded, 0, encoded.Length, rented);
                        sum += (uint)actualLength;
                    }
                }
            }
            ArrayPool<byte>.Shared.Return(rented);

            return sum;
        }

        private static byte[] Encode(string value)
        {
            var encodedBytes = new List<byte>();
            byte workingByte = 0;
            var bitsLeftInByte = 8;

            for (var i = 0; i < value.Length; i++)
            {
                var character = value[i];
                var encoded = HuffmanOrig.Encode(character);

                while (encoded.bitLength > 0)
                {
                    var bitsToWrite = bitsLeftInByte;
                    workingByte |= (byte)(encoded.encoded >> 24 + (8 - bitsToWrite));
                    if (encoded.bitLength >= bitsLeftInByte)
                    {
                        encoded.encoded <<= bitsLeftInByte;

                        encodedBytes.Add(workingByte);
                        workingByte = 0;
                        bitsLeftInByte = 8;
                    }
                    else
                    {
                        bitsLeftInByte -= encoded.bitLength;
                    }

                    encoded.bitLength -= bitsToWrite;
                }
            }

            if (bitsLeftInByte < 8)
            {
                // Pad remaning bits with 1s
                workingByte |= (byte)((0x1 << bitsLeftInByte) - 1);
                encodedBytes.Add(workingByte);
            }

            return encodedBytes.ToArray();
        }
    }
}
