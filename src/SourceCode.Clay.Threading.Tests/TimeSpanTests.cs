#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using System;
using System.Threading.Tasks;
using Xunit;

namespace SourceCode.Clay.Threading.Tests
{
    public static class TimeSpanTests
    {
        #region Methods

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(When_Create_CancellationTokenSource_From_TimeSpan_Null))]
        public static void When_Create_CancellationTokenSource_From_TimeSpan_Null()
        {
            var tokenSource = ((TimeSpan?)null).ToCancellationTokenSource();
            Assert.False(tokenSource.IsCancellationRequested);

            try
            {
                var task = Task.Delay(100, tokenSource.Token);
                task.Wait();

                Assert.True(task.IsCompleted);
            }
            finally
            {
                tokenSource.Cancel();
            }
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(When_Create_CancellationTokenSource_From_TimeSpan_0))]
        public static void When_Create_CancellationTokenSource_From_TimeSpan_0()
        {
            var delay = TimeSpan.Zero;

            var tokenSource = delay.ToCancellationTokenSource();
            Assert.True(tokenSource.IsCancellationRequested);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(When_Create_CancellationTokenSource_From_TimeSpan_Negative))]
        public static void When_Create_CancellationTokenSource_From_TimeSpan_Negative()
        {
            var delay = TimeSpan.FromMilliseconds(-100);

            var tokenSource = delay.ToCancellationTokenSource();
            Assert.True(tokenSource.IsCancellationRequested);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(When_Create_CancellationTokenSource_From_TimeSpan_Positive))]
        public static void When_Create_CancellationTokenSource_From_TimeSpan_Positive()
        {
            var delay = TimeSpan.FromMilliseconds(500);

            // Timeout is not hit
            var tokenSource = delay.ToCancellationTokenSource();
            Assert.False(tokenSource.IsCancellationRequested);

            try
            {
                var task = Task.Delay(1);
                task.Wait(tokenSource.Token);

                Assert.True(task.IsCompleted);
                Assert.False(task.IsCanceled);
            }
            catch
            {
                Assert.False(true); // Should never be hit
            }
            finally
            {
                tokenSource.Cancel();
            }

            // Force timeout to hit
            tokenSource = delay.ToCancellationTokenSource();
            Assert.False(tokenSource.IsCancellationRequested);

            try
            {
                Task.Delay(10000).Wait(tokenSource.Token);

                Assert.False(true); // Should never be hit
            }
            catch (Exception e)
            {
                Assert.True(e is OperationCanceledException);
            }
            finally
            {
                tokenSource.Cancel();
            }
        }

        #endregion
    }
}
