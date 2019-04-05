#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using System.Collections;
using System.Collections.Generic;

namespace SourceCode.Clay.Tests
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    internal sealed class Sha256TestVectors : IEnumerable<object[]>
    {
        public const string Zero = "0000000000000000000000000000000000000000000000000000000000000000";
        public const string Empty = "e3b0c44298fc1c149afbf4c8996fb92427ae41e4649b934ca495991b7852b855";

        private static readonly IList<object[]> s_data = new object[5][]
        {
            // http://www.di-mgt.com.au/sha_testvectors.html

            // Test Vector 1
            new object[]{ string.Empty, Empty },

            // Test Vector 2
            new object[]{ "abc", "ba7816bf8f01cfea414140de5dae2223b00361a396177a9cb410ff61f20015ad" },

            // Test Vector 3
            new object[]{ "abcdbcdecdefdefgefghfghighijhijkijkljklmklmnlmnomnopnopq", "248d6a61d20638b8e5c026930c3e6039a33ce45964ff2167f6ecedd419db06c1" },

            // Test Vector 4
            new object[]{ "abcdefghbcdefghicdefghijdefghijkefghijklfghijklmghijklmnhijklmnoijklmnopjklmnopqklmnopqrlmnopqrsmnopqrstnopqrstu", "cf5b16a778af8380036ce59e7b0492370b249b11e8f07a51afac45037afee9d1" },

            // Test Vector 5
            new object[]{ new string('a', 1000_000), "cdc76e5c9914fb9281a1c7e284d73e67f1809a48a497200e046d39ccc7112cd0" },
        };

        public IEnumerator<object[]> GetEnumerator() => s_data.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
