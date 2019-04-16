using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SourceCode.Clay.Threading.Tests
{
    public static class AsyncLockTests
    {
        [Fact(Timeout = 1000)]
        public static void AsyncLock_Acquire_Once()
        {
            using (var l = new AsyncLock())
            using (AsyncLockCookie t = l.Acquire())
            {
                Assert.True(t.IsAcquired);
                Assert.Equal(t.IsAcquired, t);
            }
        }

        [Fact(Timeout = 1000)]
        public static void AsyncLock_Acquire_Twice()
        {
            using (var l = new AsyncLock())
            {
                using (AsyncLockCookie t = l.Acquire())
                {
                    Assert.True(t.IsAcquired);
                    Assert.Equal(t.IsAcquired, t);
                }
                using (AsyncLockCookie t = l.Acquire())
                {
                    Assert.True(t.IsAcquired);
                }
            }
        }

        [Fact(Timeout = 1000)]
        public static void AsyncLock_Acquire_Fail()
        {
            using (var l = new AsyncLock())
            {
                using (AsyncLockCookie t1 = l.Acquire())
                {
                    Assert.True(t1.IsAcquired);
                    using (AsyncLockCookie t2 = l.Acquire(0))
                    {
                        Assert.False(t2.IsAcquired);
                        Assert.Equal(t2.IsAcquired, t2);
                    }

                    using (AsyncLockCookie t2 = l.Acquire(TimeSpan.Zero))
                    {
                        Assert.False(t2.IsAcquired);
                        Assert.Equal(t2.IsAcquired, t2);
                    }
                }
            }
        }

        [Fact(Timeout = 1000)]
        public static void AsyncLock_Acquire_Wait()
        {
            using (var l = new AsyncLock())
            {
                AsyncLockCookie t1 = l.Acquire();
                Assert.True(t1.IsAcquired);

                var sw = Stopwatch.StartNew();
                ThreadPool.QueueUserWorkItem(_ =>
                {
                    Thread.Sleep(500);
                    t1.Dispose();
                });
                using (AsyncLockCookie t2 = l.Acquire())
                {
                    Assert.True(t2);
                }
                Assert.True(sw.Elapsed.TotalMilliseconds >= 400);
            }
        }

        [Fact(Timeout = 1000)]
        public static void AsyncLock_Acquire_Timeout()
        {
            using (var l = new AsyncLock())
            {
                var sw = Stopwatch.StartNew();
                using (AsyncLockCookie t1 = l.Acquire())
                using (AsyncLockCookie t2 = l.Acquire(100))
                {
                    Assert.True(t1.IsAcquired);
                    Assert.False(t2.IsAcquired);
                    Assert.True(sw.Elapsed.TotalMilliseconds >= 80);
                }
            }
        }

        [Fact(Timeout = 1000)]
        public static void AsyncLock_Acquire_Timeout_Timespan()
        {
            using (var l = new AsyncLock())
            {
                var sw = Stopwatch.StartNew();
                using (AsyncLockCookie t1 = l.Acquire())
                using (AsyncLockCookie t2 = l.Acquire(TimeSpan.FromMilliseconds(100)))
                {
                    Assert.True(t1.IsAcquired);
                    Assert.False(t2.IsAcquired);
                    Assert.True(sw.Elapsed.TotalMilliseconds >= 80);
                }
            }
        }

        [Fact(Timeout = 1000)]
        public static async Task AsyncLock_AcquireAsync_Once()
        {
            using (var l = new AsyncLock())
            using (AsyncLockCookie t = await l.AcquireAsync())
            {
                Assert.True(t.IsAcquired);
                Assert.Equal(t.IsAcquired, t);
            }
        }

        [Fact(Timeout = 1000)]
        public static async Task AsyncLock_AcquireAsync_Twice()
        {
            using (var l = new AsyncLock())
            {
                using (AsyncLockCookie t = await l.AcquireAsync())
                {
                    Assert.True(t.IsAcquired);
                    Assert.Equal(t.IsAcquired, t);
                }
                using (AsyncLockCookie t = await l.AcquireAsync())
                {
                    Assert.True(t.IsAcquired);
                }
            }
        }

        [Fact(Timeout = 1000)]
        public static async Task AsyncLock_AcquireAsync_Fail()
        {
            using (var l = new AsyncLock())
            {
                using (AsyncLockCookie t1 = await l.AcquireAsync())
                {
                    Assert.True(t1.IsAcquired);
                    using (AsyncLockCookie t2 = await l.AcquireAsync(0))
                    {
                        Assert.False(t2.IsAcquired);
                        Assert.Equal(t2.IsAcquired, t2);
                    }

                    using (AsyncLockCookie t2 = await l.AcquireAsync(TimeSpan.Zero))
                    {
                        Assert.False(t2.IsAcquired);
                        Assert.Equal(t2.IsAcquired, t2);
                    }
                }
            }
        }

        [Fact(Timeout = 1000)]
        public static async Task AsyncLock_AcquireAsync_Wait()
        {
            using (var l = new AsyncLock())
            {
                AsyncLockCookie t1 = await l.AcquireAsync();
                Assert.True(t1.IsAcquired);

                var sw = Stopwatch.StartNew();
                ThreadPool.QueueUserWorkItem(_ =>
                {
                    Thread.Sleep(500);
                    t1.Dispose();
                });
                using (AsyncLockCookie t2 = await l.AcquireAsync())
                {
                    Assert.True(t2);
                }
                Assert.True(sw.Elapsed.TotalMilliseconds >= 400);
            }
        }

        [Fact(Timeout = 1000)]
        public static async Task AsyncLock_AcquireAsync_Timeout()
        {
            using (var l = new AsyncLock())
            {
                var sw = Stopwatch.StartNew();
                using (AsyncLockCookie t1 = await l.AcquireAsync())
                using (AsyncLockCookie t2 = await l.AcquireAsync(100))
                {
                    Assert.True(t1.IsAcquired);
                    Assert.False(t2.IsAcquired);
                    Assert.True(sw.Elapsed.TotalMilliseconds >= 80);
                }
            }
        }

        [Fact(Timeout = 1000)]
        public static async Task AsyncLock_AcquireAsync_Timeout_Timespan()
        {
            using (var l = new AsyncLock())
            {
                var sw = Stopwatch.StartNew();
                using (AsyncLockCookie t1 = await l.AcquireAsync())
                using (AsyncLockCookie t2 = await l.AcquireAsync(TimeSpan.FromMilliseconds(100)))
                {
                    Assert.True(t1.IsAcquired);
                    Assert.False(t2.IsAcquired);
                    Assert.True(sw.Elapsed.TotalMilliseconds >= 80);
                }
            }
        }
    }
}
