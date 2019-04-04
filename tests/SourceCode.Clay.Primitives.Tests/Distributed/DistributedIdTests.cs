#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using Xunit;

namespace SourceCode.Clay.Distributed.Tests
{
    public sealed class DistributedIdTests
    {
#pragma warning disable CS1718 // Comparison made to same variable
        [Fact]
        public void DistributedId_Comparison()
        {
            var id1 = new DistributedId(1);
            var id2 = new DistributedId(2);
            var id3 = new DistributedId(3);

            Assert.True(id1 < id2);
            Assert.True(id1 <= id2);
            Assert.True(id2 <= id2);
            Assert.True(id2 <= id3);
            Assert.True(id2 < id3);

            Assert.True(id1.CompareTo(id2) < 0);
            Assert.True(id2.CompareTo(id1) > 0);
            Assert.True(id2.CompareTo(id2) == 0);

            Assert.True(id3 > id2);
            Assert.True(id3 >= id2);
            Assert.True(id2 >= id1);
            Assert.True(id2 >= id1);
            Assert.True(id2 > id1);
        }

        [Fact]
        public void DistributedId_Equals()
        {
            var id1 = new DistributedId(1);
            var id2 = new DistributedId(2);

            Assert.True(id1.Equals(id1));
            Assert.True(id2.Equals(id2));
            Assert.True(id1.Equals((object)id1));
            Assert.True(id2.Equals((object)id2));
            Assert.True(id1 == id1);
            Assert.True(id2 == id2);

            Assert.True(!id1.Equals(id2));
            Assert.True(!id2.Equals(id1));
            Assert.True(!id1.Equals((object)id2));
            Assert.True(!id2.Equals((object)id1));
            Assert.True(id1 != id2);
            Assert.True(id2 != id1);

            Assert.False(id1.Equals((object)1));
        }

        [Fact]
        public void DistributedId_MaxValue() => Assert.Equal(ulong.MaxValue, DistributedId.MaxValue.Value);

        [Fact]
        public void DistributedId_MinValue() => Assert.Equal(ulong.MinValue, DistributedId.MinValue.Value);

        [Fact]
        public void DistributedId_ToString()
        {
            var id1 = new DistributedId(1);
            var id2 = new DistributedId(2);

            Assert.Equal("1", id1.ToString());
            Assert.Equal("2", id2.ToString());
        }

        [Fact]
        public void DistributedId_Parse()
        {
            Assert.Equal(123ul, DistributedId.Parse("7B").Value);
        }

        [Fact]
        public void DistributedId_TryParse_Valid()
        {
            Assert.True(DistributedId.TryParse("7B", out DistributedId distributedId));
            Assert.Equal(123ul, distributedId.Value);
        }

        [Fact]
        public void DistributedId_TryParse_Invalid()
        {
            Assert.False(DistributedId.TryParse("not hex", out DistributedId distributedId));
            Assert.Equal(DistributedId.MinValue, distributedId);
        }
#pragma warning restore CS1718 // Comparison made to same variable
    }
}
