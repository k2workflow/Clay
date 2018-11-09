#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using System;
using System.Buffers;
using Xunit;

namespace SourceCode.Clay.Buffers.Tests
{
    public static class IMemoryOwnerExtensionsTests
    {
        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(MemoryPool_byte_WrapSlice_start_noop))]
        public static void MemoryPool_byte_WrapSlice_start_noop()
        {
            MemoryPool<byte> pool = MemoryPool<byte>.Shared;
            IMemoryOwner<byte> owner1 = pool.Rent(99);
            IMemoryOwner<byte> owner2 = owner1.WrapSlice(10);
            Assert.False(ReferenceEquals(owner1, owner2));

            IMemoryOwner<byte> owner3 = pool.Rent(0);
            IMemoryOwner<byte> owner4 = owner3.WrapSlice(0);
            Assert.True(ReferenceEquals(owner3, owner4));
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(MemoryPool_byte_WrapSlice_start_length_noop))]
        public static void MemoryPool_byte_WrapSlice_start_length_noop()
        {
            MemoryPool<byte> pool = MemoryPool<byte>.Shared;
            IMemoryOwner<byte> owner1 = pool.Rent(99);
            IMemoryOwner<byte> owner2 = owner1.WrapSlice(0, 10);
            Assert.False(ReferenceEquals(owner1, owner2));

            IMemoryOwner<byte> owner3 = pool.Rent(0);
            IMemoryOwner<byte> owner4 = owner3.WrapSlice(0, 0);
            Assert.True(ReferenceEquals(owner3, owner4));

            IMemoryOwner<byte> owner5 = pool.Rent(10);
            IMemoryOwner<byte> owner6 = owner5.WrapSlice(0, owner5.Memory.Length);
            Assert.True(ReferenceEquals(owner5, owner6));
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(MemoryPool_byte_WrapSlice_start_guards))]
        public static void MemoryPool_byte_WrapSlice_start_guards()
        {
            Assert.Throws<ArgumentNullException>(() => IMemoryOwnerExtensions.WrapSlice<byte>(null, 0));

            MemoryPool<byte> pool = MemoryPool<byte>.Shared;
            IMemoryOwner<byte> owner1 = pool.Rent(99);

            Assert.Throws<ArgumentOutOfRangeException>(() => IMemoryOwnerExtensions.WrapSlice(owner1, -1));
            Assert.Throws<ArgumentOutOfRangeException>(() => IMemoryOwnerExtensions.WrapSlice(owner1, owner1.Memory.Length + 1));
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(MemoryPool_byte_WrapSlice_start_length_guards))]
        public static void MemoryPool_byte_WrapSlice_start_length_guards()
        {
            Assert.Throws<ArgumentNullException>(() => IMemoryOwnerExtensions.WrapSlice<byte>(null, 0, 0));

            MemoryPool<byte> pool = MemoryPool<byte>.Shared;
            IMemoryOwner<byte> owner1 = pool.Rent(99);

            Assert.Throws<ArgumentOutOfRangeException>(() => IMemoryOwnerExtensions.WrapSlice(owner1, -1, 0));
            Assert.Throws<ArgumentOutOfRangeException>(() => IMemoryOwnerExtensions.WrapSlice(owner1, owner1.Memory.Length, 0));

            Assert.Throws<ArgumentOutOfRangeException>(() => IMemoryOwnerExtensions.WrapSlice(owner1, 0, -1));
            Assert.Throws<ArgumentOutOfRangeException>(() => IMemoryOwnerExtensions.WrapSlice(owner1, 0, owner1.Memory.Length + 1));
            Assert.Throws<ArgumentOutOfRangeException>(() => IMemoryOwnerExtensions.WrapSlice(owner1, 1, owner1.Memory.Length));
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(MemoryPool_byte_WrapSlice_start))]
        public static void MemoryPool_byte_WrapSlice_start()
        {
            MemoryPool<byte> pool = MemoryPool<byte>.Shared;

            IMemoryOwner<byte> owner1 = pool.Rent(99);
            Assert.True(owner1.Memory.Length > 99);

            IMemoryOwner<byte> owner2 = owner1.WrapSlice(23);
            Assert.Equal(owner1.Memory.Length - 23, owner2.Memory.Length);

            owner2.Dispose();
            Assert.Throws<ObjectDisposedException>(() => owner1.Memory.Length);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(MemoryPool_byte_WrapSlice_start_length))]
        public static void MemoryPool_byte_WrapSlice_start_length()
        {
            MemoryPool<byte> pool = MemoryPool<byte>.Shared;

            IMemoryOwner<byte> owner1 = pool.Rent(99);
            Assert.True(owner1.Memory.Length > 99);

            IMemoryOwner<byte> owner2 = owner1.WrapSlice(10, 81);
            Assert.Equal(81, owner2.Memory.Length);

            owner2.Dispose();
            Assert.Throws<ObjectDisposedException>(() => owner1.Memory.Length);
        }
    }
}
