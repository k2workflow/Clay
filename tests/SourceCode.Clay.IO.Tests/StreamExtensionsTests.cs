#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace SourceCode.Clay.IO.Tests
{
    public static class StreamExtensionsTests
    {
        [Fact]
        public static void StreamExtensions_Write()
        {
            ArraySegment<byte> buffer = MemoryBufferStreamTests.GenerateSegment(0, 64, 1);
            var memory = new ReadOnlyMemory<byte>(buffer.Array, buffer.Offset, buffer.Count);

            using (var specimen = new MemoryStream())
            {
                specimen.Write(memory.Span);
                Assert.Equal((IEnumerable<byte>)buffer, specimen.ToArray());
            }
        }

        [Fact]
        public static async Task StreamExtensions_WriteAsync()
        {
            ArraySegment<byte> buffer = MemoryBufferStreamTests.GenerateSegment(0, 64, 1);
            var memory = new ReadOnlyMemory<byte>(buffer.Array, buffer.Offset, buffer.Count);

            using (var specimen = new MemoryStream())
            {
                await specimen.WriteAsync(memory);
                Assert.Equal((IEnumerable<byte>)buffer, specimen.ToArray());
            }
        }
    }
}
