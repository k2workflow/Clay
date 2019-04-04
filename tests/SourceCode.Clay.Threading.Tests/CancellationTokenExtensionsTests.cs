using System;
using System.Threading;
using Xunit;

namespace SourceCode.Clay.Threading.Tests
{
    public sealed class CancellationTokenExtensionsTests
    {
        [Fact]
        public void When_using_a_default_Token_creates_new_Token()
        {
            // Arrange
            CancellationToken ct = CancellationToken.None;
            var timeout = TimeSpan.FromSeconds(2);

            // Act
            using (CancellationTokenSource cts = ct.WithTimeout(timeout))
            {
                // Assert
                Assert.NotEqual(CancellationToken.None, cts.Token);

                Assert.False(cts.Token.IsCancellationRequested);
                cts.Token.WaitHandle.WaitOne();
                Assert.True(cts.Token.IsCancellationRequested);
            }
        }

        [Fact]
        public void When_using_an_InfiniteTimeout_creates_new_Token()
        {
            // Arrange
            CancellationToken ct = CancellationToken.None;
            TimeSpan timeout = Timeout.InfiniteTimeSpan;

            // Act
            Action act = () => ct.WithTimeout(timeout);

            // Assert
            Assert.Throws<ArgumentOutOfRangeException>(act);
        }

        [Fact]
        public void When_using_a_valid_token_and_timeout_uses_Token()
        {
            // Arrange
            var ct = new CancellationToken();
            var timeout = TimeSpan.FromSeconds(2);

            // Act
            using (CancellationTokenSource cts = ct.WithTimeout(timeout))
            {
                // Assert
                Assert.False(cts.Token.IsCancellationRequested);
                cts.Token.WaitHandle.WaitOne();
                Assert.True(cts.Token.IsCancellationRequested);
            }
        }
    }
}
