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
            var timeout = TimeSpan.FromSeconds(1);

            // Act
            CancellationTokenSource cts = ct.WithTimeout(timeout);

            // Assert
            Assert.NotEqual(CancellationToken.None, cts.Token);
        }

        [Fact]
        public void When_using_an_InfiniteTimeout_creates_new_Token()
        {
            // Arrange
            CancellationToken ct = CancellationToken.None;
            TimeSpan timeout = Timeout.InfiniteTimeSpan;

            // Act
            CancellationTokenSource cts = ct.WithTimeout(timeout);

            // Assert
            Assert.NotEqual(CancellationToken.None, cts.Token);
        }

        [Fact]
        public void When_using_a_valid_token_and_timeout_uses_Token()
        {
            // Arrange
            var ct = new CancellationToken();
            var timeout = TimeSpan.FromSeconds(5);

            // Act
            CancellationTokenSource cts = ct.WithTimeout(timeout);

            // Assert
            Assert.Equal(ct, cts.Token);
        }
    }
}
