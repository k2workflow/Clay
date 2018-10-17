#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using SourceCode.Clay.Buffers;
using SourceCode.Clay.Buffers.Tests;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SourceCode.Clay.IO.Tests
{
    public static class MemoryBufferStreamTests
    {
        [Fact(DisplayName = nameof(MemoryBufferStream_Read_SmallBuffer))]
        public static void MemoryBufferStream_Read_SmallBuffer()
        {
            ReadOnlyMemory<byte> truth = BufferComparerTests.GenerateSegment(13, 203, 1).AsReadOnlyMemory();
            var buffer = new byte[100];
            using (var sut = new MemoryBufferStream(truth))
            {
                Assert.Equal(50, sut.Read(buffer, 0, 50));
                Assert.Equal(buffer.AsReadOnlyMemory().Slice(0, 50), truth.Slice(0, 50), BufferComparer.Memory);

                Assert.Equal(100, sut.Read(buffer));
                Assert.Equal(buffer.AsReadOnlyMemory(), truth.Slice(50, 100), BufferComparer.Memory);

                Assert.Equal(53, sut.Read(buffer));
                Assert.Equal(buffer.AsReadOnlyMemory().Slice(0, 53), truth.Slice(150, 53), BufferComparer.Memory);

                // Ensure nothing was copied.

                Assert.Equal(0, sut.Read(buffer));
                Assert.Equal(buffer.AsReadOnlyMemory().Slice(0, 53), truth.Slice(150, 53), BufferComparer.Memory);

                Assert.Equal(0, sut.Read(buffer));
                Assert.Equal(buffer.AsReadOnlyMemory().Slice(0, 53), truth.Slice(150, 53), BufferComparer.Memory);
            }
        }

        [Fact(DisplayName = nameof(MemoryBufferStream_Read_BigBuffer))]
        public static void MemoryBufferStream_Read_BigBuffer()
        {
            ReadOnlyMemory<byte> truth = BufferComparerTests.GenerateSegment(13, 107, 1).AsReadOnlyMemory();
            var buffer = new byte[1024];
            using (var sut = new MemoryBufferStream(truth))
            {
                Assert.Equal(107, sut.Read(buffer));
                Assert.Equal(buffer.AsReadOnlyMemory().Slice(0, 107), truth, BufferComparer.Memory);

                for (var i = 107; i < 1024; i++)
                    Assert.Equal(0, buffer[i]);

                Assert.Equal(0, sut.Read(buffer));
                Assert.Equal(buffer.AsReadOnlyMemory().Slice(0, 107), truth, BufferComparer.Memory);
            }
        }

        [Fact(DisplayName = nameof(MemoryBufferStream_AsyncPatternRead_SmallBuffer))]
        public static void MemoryBufferStream_AsyncPatternRead_SmallBuffer()
        {
            ReadOnlyMemory<byte> truth = BufferComparerTests.GenerateSegment(13, 203, 1).AsReadOnlyMemory();
            var buffer = new byte[100];
            using (var sut = new MemoryBufferStream(truth))
            {
                Assert.Equal(50, sut.SyncAsyncRead(buffer, 0, 50));
                Assert.Equal(buffer.AsReadOnlyMemory().Slice(0, 50), truth.Slice(0, 50), BufferComparer.Memory);

                Assert.Equal(100, sut.SyncAsyncRead(buffer, 0, 100));
                Assert.Equal(buffer.AsReadOnlyMemory(), truth.Slice(50, 100), BufferComparer.Memory);

                Assert.Equal(53, sut.SyncAsyncRead(buffer, 0, 100));
                Assert.Equal(buffer.AsReadOnlyMemory().Slice(0, 53), truth.Slice(150, 53), BufferComparer.Memory);

                // Ensure nothing was copied.

                Assert.Equal(0, sut.SyncAsyncRead(buffer, 0, 100));
                Assert.Equal(buffer.AsReadOnlyMemory().Slice(0, 53), truth.Slice(150, 53), BufferComparer.Memory);

                Assert.Equal(0, sut.SyncAsyncRead(buffer, 0, 100));
                Assert.Equal(buffer.AsReadOnlyMemory().Slice(0, 53), truth.Slice(150, 53), BufferComparer.Memory);
            }
        }

        [Fact(DisplayName = nameof(MemoryBufferStream_AsyncPatternRead_BigBuffer))]
        public static void MemoryBufferStream_AsyncPatternRead_BigBuffer()
        {
            ReadOnlyMemory<byte> truth = BufferComparerTests.GenerateSegment(13, 107, 1).AsReadOnlyMemory();
            var buffer = new byte[1024];
            using (var sut = new MemoryBufferStream(truth))
            {
                Assert.Equal(107, sut.SyncAsyncRead(buffer, 0, 1024));
                Assert.Equal(buffer.AsReadOnlyMemory().Slice(0, 107), truth, BufferComparer.Memory);

                for (var i = 107; i < 1024; i++)
                    Assert.Equal(0, buffer[i]);

                Assert.Equal(0, sut.SyncAsyncRead(buffer, 0, 1024));
                Assert.Equal(buffer.AsReadOnlyMemory().Slice(0, 107), truth, BufferComparer.Memory);
            }
        }

        public static int SyncAsyncRead(this Stream stream, byte[] buffer, int offset, int count)
        {
            var expectedState = "Result state was set.";
            using (var wait = new ManualResetEventSlim())
            {
                wait.Reset();

                var result = 0;
                var actualState = "Result state was not set.";
                var cb = new AsyncCallback(iar =>
                {
                    actualState = iar.AsyncState as string;
                    result = stream.EndRead(iar);
                    wait.Set();
                });

                IAsyncResult ar = stream.BeginRead(buffer, offset, count, cb, expectedState);

                Assert.True(ar.CompletedSynchronously);
                Assert.True(ar.IsCompleted);
                Assert.True(ar.AsyncWaitHandle.WaitOne(1000));

                Assert.True(wait.Wait(1000));
                Assert.Equal(expectedState, actualState);

                return result;
            }
        }

        [Fact(DisplayName = nameof(MemoryBufferStream_ReadAsync_SmallBuffer))]
        public static async Task MemoryBufferStream_ReadAsync_SmallBuffer()
        {
            ReadOnlyMemory<byte> truth = BufferComparerTests.GenerateSegment(13, 203, 1).AsReadOnlyMemory();
            var buffer = new byte[100];
            using (var sut = new MemoryBufferStream(truth))
            {
                Assert.Equal(50, await sut.ReadAsync(buffer, 0, 50));
                Assert.Equal(buffer.AsReadOnlyMemory().Slice(0, 50), truth.Slice(0, 50), BufferComparer.Memory);

                Assert.Equal(100, await sut.ReadAsync(buffer));
                Assert.Equal(buffer.AsReadOnlyMemory(), truth.Slice(50, 100), BufferComparer.Memory);

                Assert.Equal(53, await sut.ReadAsync(buffer));
                Assert.Equal(buffer.AsReadOnlyMemory().Slice(0, 53), truth.Slice(150, 53), BufferComparer.Memory);

                // Ensure nothing was copied.

                Assert.Equal(0, await sut.ReadAsync(buffer));
                Assert.Equal(buffer.AsReadOnlyMemory().Slice(0, 53), truth.Slice(150, 53), BufferComparer.Memory);

                Assert.Equal(0, await sut.ReadAsync(buffer));
                Assert.Equal(buffer.AsReadOnlyMemory().Slice(0, 53), truth.Slice(150, 53), BufferComparer.Memory);
            }
        }

        [Fact(DisplayName = nameof(MemoryBufferStream_ReadAsync_BigBuffer))]
        public static async Task MemoryBufferStream_ReadAsync_BigBuffer()
        {
            ReadOnlyMemory<byte> truth = BufferComparerTests.GenerateSegment(13, 107, 1).AsReadOnlyMemory();
            var buffer = new byte[1024];
            using (var sut = new MemoryBufferStream(truth))
            {
                Assert.Equal(107, await sut.ReadAsync(buffer));
                Assert.Equal(buffer.AsReadOnlyMemory().Slice(0, 107), truth, BufferComparer.Memory);

                for (var i = 107; i < 1024; i++)
                    Assert.Equal(0, buffer[i]);

                Assert.Equal(0, await sut.ReadAsync(buffer));
                Assert.Equal(buffer.AsReadOnlyMemory().Slice(0, 107), truth, BufferComparer.Memory);
            }
        }

        [Fact(DisplayName = nameof(MemoryBufferStream_ReadByte))]
        public static void MemoryBufferStream_ReadByte()
        {
            ReadOnlyMemory<byte> truth = BufferComparerTests.GenerateSegment(13, 107, 1).AsReadOnlyMemory();
            using (var sut = new MemoryBufferStream(truth))
            {
                for (var i = 0; i < truth.Length; i++)
                    Assert.Equal(truth.Span[i], sut.ReadByte());
                Assert.Equal(-1, sut.ReadByte());
                Assert.Equal(-1, sut.ReadByte());
            }
        }

        [Fact(DisplayName = nameof(MemoryBufferStream_Seek))]
        public static void MemoryBufferStream_Seek()
        {
            using (var sut = new MemoryBufferStream(new byte[1027]))
            {
                Assert.Equal(0, sut.Seek(-100, SeekOrigin.Begin));
                Assert.Equal(0, sut.Position);

                Assert.Equal(0, sut.Seek(0, SeekOrigin.Begin));
                Assert.Equal(0, sut.Position);

                Assert.Equal(100, sut.Seek(100, SeekOrigin.Begin));
                Assert.Equal(100, sut.Position);

                Assert.Equal(1027, sut.Seek(2000, SeekOrigin.Begin));
                Assert.Equal(1027, sut.Position);

                Assert.Equal(927, sut.Seek(-100, SeekOrigin.Current));
                Assert.Equal(927, sut.Position);

                Assert.Equal(1027, sut.Seek(100, SeekOrigin.Current));
                Assert.Equal(1027, sut.Position);

                Assert.Equal(1027, sut.Seek(0, SeekOrigin.Current));
                Assert.Equal(1027, sut.Position);

                Assert.Equal(0, sut.Seek(-2000, SeekOrigin.Current));
                Assert.Equal(0, sut.Position);

                Assert.Equal(927, sut.Seek(-100, SeekOrigin.End));
                Assert.Equal(927, sut.Position);

                Assert.Equal(1027, sut.Seek(100, SeekOrigin.End));
                Assert.Equal(1027, sut.Position);

                Assert.Equal(1027, sut.Seek(0, SeekOrigin.End));
                Assert.Equal(1027, sut.Position);

                Assert.Equal(0, sut.Seek(-2000, SeekOrigin.End));
                Assert.Equal(0, sut.Position);
            }
        }

        [Fact(DisplayName = nameof(MemoryBufferStream_Copy))]
        public static void MemoryBufferStream_Copy()
        {
            ReadOnlyMemory<byte> truth = BufferComparerTests.GenerateSegment(13, 99834, 1).AsReadOnlyMemory();
            using (var sut = new MemoryBufferStream(truth))
            using (var ms = new MemoryStream())
            {
                sut.CopyTo(ms, 500);

                var buf = ms.ToArray();
                Assert.Equal(truth, buf.AsReadOnlyMemory(), BufferComparer.Memory);
                Assert.Equal(truth.Length, sut.Position);
            }
        }

        [Fact(DisplayName = nameof(MemoryBufferStream_CopyAsync))]
        public static void MemoryBufferStream_CopyAsync()
        {
            ReadOnlyMemory<byte> truth = BufferComparerTests.GenerateSegment(13, 99834, 1).AsReadOnlyMemory();
            using (var sut = new MemoryBufferStream(truth))
            using (var ms = new MemoryStream())
            {
                sut.CopyToAsync(ms, 500);

                var buf = ms.ToArray();
                Assert.Equal(truth, buf.AsReadOnlyMemory(), BufferComparer.Memory);
                Assert.Equal(truth.Length, sut.Position);
            }
        }

        [Fact(DisplayName = nameof(MemoryBufferStream_NotSupported))]
        public static void MemoryBufferStream_NotSupported()
        {
            var sut = new MemoryBufferStream(new byte[10]);
            Assert.Throws<NotSupportedException>(() => sut.SetLength(10));
            Assert.Throws<NotSupportedException>(() => sut.Write(default, default, default));
            Assert.ThrowsAsync<NotSupportedException>(() => sut.WriteAsync(default, default, default));
            Assert.Throws<NotSupportedException>(() => sut.BeginWrite(default, default, default, default, default));
            Assert.Throws<NotSupportedException>(() => sut.EndWrite(default));
            Assert.Throws<NotSupportedException>(() => sut.WriteByte(default));
        }

        [Fact(DisplayName = nameof(MemoryBufferStream_Properties))]
        public static void MemoryBufferStream_Properties()
        {
            var sut = new MemoryBufferStream(new byte[10]);
            Assert.True(sut.CanRead);
            Assert.True(sut.CanSeek);
            Assert.False(sut.CanTimeout);
            Assert.False(sut.CanWrite);
        }
    }
}
