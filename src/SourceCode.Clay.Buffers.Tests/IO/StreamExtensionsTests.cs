#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using SourceCode.Clay.Buffers.Tests;
using SourceCode.Clay.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SourceCode.Clay.Buffers.IO.Tests
{
    public static class StreamExtensionsTests
    {
        #region Methods

        [Fact(DisplayName = nameof(StreamExtensions_Write))]
        public static void StreamExtensions_Write()
        {
            var buffer = BufferComparerTests.GenerateSegment(0, 64, 1);
            var memory = new ReadOnlyMemory<byte>(buffer.Array, buffer.Offset, buffer.Count);

            using (var specimen = new MemoryStream())
            {
                specimen.Write(memory);
                Assert.Equal((IEnumerable<byte>)buffer, specimen.ToArray());
            }

            using (var specimen = new MemoryStream())
            {
                specimen.Write(memory, 13);
                Assert.Equal((IEnumerable<byte>)buffer, specimen.ToArray());
            }
        }

        [Fact(DisplayName = nameof(StreamExtensions_WriteAsync))]
        public static async Task StreamExtensions_WriteAsync()
        {
            var buffer = BufferComparerTests.GenerateSegment(0, 64, 1);
            var memory = new ReadOnlyMemory<byte>(buffer.Array, buffer.Offset, buffer.Count);

            using (var specimen = new MemoryStream())
            {
                await specimen.WriteAsync(memory);
                Assert.Equal((IEnumerable<byte>)buffer, specimen.ToArray());
            }

            using (var specimen = new MemoryStream())
            {
                await specimen.WriteAsync(memory, 13);
                Assert.Equal((IEnumerable<byte>)buffer, specimen.ToArray());
            }
        }

        #endregion
    }
}
