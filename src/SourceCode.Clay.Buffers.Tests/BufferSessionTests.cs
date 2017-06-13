using System;
using Xunit;

namespace SourceCode.Clay.Buffers.Tests
{
    public class BufferSessionTests
    {
        [Trait("Type", "Unit")]
        [Fact(DisplayName = "BufferSession Full Lifecycle")]
        public static void BufferSession_Full_Lifecycle()
        {
            var buffer = BufferSession.RentBuffer(100);
            Assert.True(buffer.Length >= 100);

            var result = new ArraySegment<byte>(buffer);
            result.Array[0] = 123;

            var session = new BufferSession(buffer, result);
            {
                Assert.Equal(123, session.Result.Array[0]);
                Assert.True(session.Buffer.Length == buffer.Length);
            }
            session.Dispose(); // Call explicitly

            Assert.Null(session.Buffer);
        }
    }
}
