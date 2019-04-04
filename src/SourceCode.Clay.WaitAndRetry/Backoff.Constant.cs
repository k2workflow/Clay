#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using System;
using System.Collections.Generic;

namespace SourceCode.Clay.WaitAndRetry
{
    partial class Backoff // .Constant
    {
        /// <summary>
        /// Generates sleep durations as a constant value.
        /// The formula used is: Duration = <paramref name="delay"/>.
        /// For example: 200ms, 200ms, 200ms, ...
        /// </summary>
        /// <param name="delay">The constant wait duration before each retry.</param>
        /// <param name="retryCount">The maximum number of retries to use, in addition to the original call.</param>
        /// <param name="fastFirst">Whether the first retry will be immediate or not.</param>
        public static IEnumerable<TimeSpan> ConstantBackoff(TimeSpan delay, int retryCount, bool fastFirst = false)
        {
            if (delay < TimeSpan.Zero) throw new ArgumentOutOfRangeException(nameof(delay), delay, "should be >= 0ms");
            if (retryCount < 0) throw new ArgumentOutOfRangeException(nameof(retryCount), retryCount, "should be >= 0");

            if (retryCount == 0)
                return Empty();

            return Enumerate(delay, retryCount, fastFirst);

            IEnumerable<TimeSpan> Enumerate(TimeSpan timeSpan, int retry, bool fast)
            {
                int i = 0;
                if (fast)
                {
                    i++;
                    yield return TimeSpan.Zero;
                }

                for (; i < retry; i++)
                {
                    yield return timeSpan;
                }
            }
        }
    }
}
