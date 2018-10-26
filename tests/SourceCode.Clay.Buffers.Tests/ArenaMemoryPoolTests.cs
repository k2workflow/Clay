#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using System.Buffers;
using Xunit;

namespace SourceCode.Clay.Buffers.Tests
{
    public static class ArenaMemoryPoolTests
    {
        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(ArenaMemoryPool_Rent))]
        public static void ArenaMemoryPool_Rent()
        {
            ArenaMemoryPool<byte> pool;

            using (pool = new ArenaMemoryPool<byte>())
            {
                Assert.Equal(0, pool.Count);

                IMemoryOwner<byte> owner1 = pool.Rent(100);
                Assert.True(owner1.Memory.Length >= 100);
                Assert.Equal(1, pool.Count);

                IMemoryOwner<byte> owner2 = pool.Rent(200);
                Assert.True(owner2.Memory.Length >= 200);
                Assert.Equal(2, pool.Count);
            }

            Assert.Equal(0, pool.Count);
        }
    }
}
