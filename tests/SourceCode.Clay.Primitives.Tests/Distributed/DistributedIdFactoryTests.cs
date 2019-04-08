#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using System;
using System.Collections.Generic;
using System.Threading;
using Xunit;

namespace SourceCode.Clay.Distributed.Tests
{
    public sealed class DistributedIdFactoryTests
    {
        [Fact]
        public void DistributedIdFactory_Properties()
        {
            DateTime epoch = DateTime.UtcNow.AddSeconds(-1);
            var factory = new DistributedIdFactory(epoch, 123);
            Assert.Equal(epoch.ToUniversalTime(), factory.Epoch);
            Assert.Equal(123, factory.MachineId);
        }

        [Fact]
        public void DistributedIdFactory_Create_AfterTime()
        {
            var factory = new DistributedIdFactory(DateTime.UtcNow, 123);
            Thread.Sleep(1000);
            factory.Create();
        }

        [Fact]
        public void DistributedIdFactory_Create_Competing()
        {
            const int threadCount = 10;
            const int iterations = 10000;

            var factory = new DistributedIdFactory(DateTime.UtcNow.AddSeconds(-1), 0);
            var threads = new Thread[threadCount];
            var bigHs = new HashSet<DistributedId>(threadCount * iterations);
            Exception ex = null;

            using (var mut = new ManualResetEvent(false))
            {
                for (var i = 0; i < threads.Length; i++)
                {
                    threads[i] = new Thread(() =>
                    {
                        try
                        {
                            mut.WaitOne();
                            var hs = new HashSet<DistributedId>();
                            for (var j = 0; j < iterations; j++)
                                hs.Add(factory.Create());
                            lock (bigHs) bigHs.UnionWith(hs);
                        }
                        catch (Exception e)
                        {
                            ex = e;
                        }
                    });
                    threads[i].Start();
                }

                mut.Set();

                for (var i = 0; i < threads.Length; i++)
                {
                    threads[i].Join();
                }

                Assert.Null(ex);
                Assert.Equal(threadCount * iterations, bigHs.Count);
            }
        }

        [Fact]
        public void DistributedIdFactory_Create_Exhaust()
        {
            var offset = 0b1111_1111_1111_1111_1111_1111_1111_1111_1111_1111ul + 5000;
            var factory = new DistributedIdFactory(offset, 0);
            InvalidOperationException e = Assert.Throws<InvalidOperationException>(() => factory.Create());
            Assert.Equal("All distributed identifiers have been exhausted for the epoch.", e.Message);
        }

        [Fact]
        public void DistributedIdFactory_Create_InvalidMachineID()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new DistributedIdFactory(DateTime.UtcNow, 16384));
        }

        [Fact]
        public void DistributedIdFactory_Create_InvalidEpoch()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new DistributedIdFactory(DateTime.Now, 0));
        }

        [Fact]
        public void DistributedIdFactory_Create_FutureEpoch()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new DistributedIdFactory(DateTime.UtcNow.AddSeconds(10), 0));
        }
    }
}
