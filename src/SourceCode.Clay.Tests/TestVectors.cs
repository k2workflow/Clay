#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using System.Collections;
using System.Collections.Generic;

namespace SourceCode.Clay.Tests
{
    internal sealed class TestVectors : IEnumerable<object[]>
    {
        private static readonly List<object[]> _data = new List<object[]>
        {
            // http://www.di-mgt.com.au/sha_testvectors.html
            
            // Test Vector 1
            new object[]{ "", "da39a3ee5e6b4b0d3255bfef95601890afd80709" },

            // Test Vector 2
            new object[]{ "abc", "a9993e364706816aba3e25717850c26c9cd0d89d" },

            // Test Vector 3
            new object[]{ "abcdbcdecdefdefgefghfghighijhijkijkljklmklmnlmnomnopnopq", "84983e441c3bd26ebaae4aa1f95129e5e54670f1" },

            // Test Vector 4
            new object[]{ "abcdefghbcdefghicdefghijdefghijkefghijklfghijklmghijklmnhijklmnoijklmnopjklmnopqklmnopqrlmnopqrsmnopqrstnopqrstu", "a49b2446a02c645bf419f995b67091253a04a259" },

            // Test Vector 5
            new object[]{ new string('a', 1000_000), "34aa973cd4c4daa4f61eeb2bdbad27316534016f" },
        };

        // https://en.wikipedia.org/wiki/Special:CiteThisPage?page=Rick_Astley
        public const string LongStr = @"From Wikipedia: Astley was born on 6 February 1966 in Newton-le-Willows in Lancashire, the fourth child of his family. His parents divorced when he was five, and Astley was brought up by his father.[9] His musical career started when he was ten, singing in the local church choir.[10] During his schooldays, Astley formed and played the drums in a number of local bands, where he met guitarist David Morris.[2][11] After leaving school at sixteen, Astley was employed during the day as a driver in his father's market-gardening business and played drums on the Northern club circuit at night in bands such as Give Way – specialising in covering Beatles and Shadows songs – and FBI, which won several local talent competitions.[10]";

        public const string SurrogatePair = "\uD869\uDE01";

        public IEnumerator<object[]> GetEnumerator() => _data.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
