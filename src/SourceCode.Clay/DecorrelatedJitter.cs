using System;
using System.Collections.Generic;

namespace SourceCode.Clay
{
    /// <summary>
    /// Implements a mechanism for transient error handling, whereby
    /// the backoff timings should be randomized.
    /// See background here: https://www.awsarchitectureblog.com/2015/03/backoff.html.
    /// </summary>
    public sealed class DecorrelatedJitter
    {
        private readonly RandomDistribution _random;

        public int RetryCount { get; }
        public TimeSpan MinDelay { get; }
        public TimeSpan MaxDelay { get; }

        /// <summary>
        /// Create a new instance of the <see cref="DecorrelatedJitter"/> class.
        /// </summary>
        /// <param name="retryCount">The number of retries to make (in addition to the original execution).</param>
        /// <param name="minDelay">The minimum time delay between retries.</param>
        /// <param name="maxDelay">The maximum time delay between retries.</param>
        /// <param name="random">A custom <see cref="Random"/> instance to use, perhaps using a deterministic seed.
        /// If not specified, will use a datetime-seeded instance</param>
        public DecorrelatedJitter(int retryCount, TimeSpan minDelay, TimeSpan maxDelay, RandomDistribution random = null)
        {
            if (retryCount < 0) throw new ArgumentOutOfRangeException(nameof(retryCount));
            if (minDelay < TimeSpan.Zero) throw new ArgumentOutOfRangeException(nameof(minDelay));
            if (maxDelay < minDelay) throw new ArgumentOutOfRangeException(nameof(maxDelay));

            _random = random ?? RandomDistribution.Uniform;
            RetryCount = retryCount;
            MinDelay = minDelay;
            MaxDelay = maxDelay;
        }

        /// <summary>
        /// Create a sequence of random <see cref="TimeSpan"/> values.
        /// </summary>
        /// <remarks>A new enumerator should be created for every execution.</remarks>
        public IEnumerable<TimeSpan> Generate()
        {
            double range = MaxDelay.TotalMilliseconds - MinDelay.Milliseconds;

            for (int i = 0; i < RetryCount; i++)
            {
                var ms = range * _random.NextDouble(); // Range
                ms += MinDelay.TotalMilliseconds; // Floor
                ms = Math.Min(ms, MaxDelay.TotalMilliseconds); // Ceiling

                yield return TimeSpan.FromMilliseconds(ms);
            }
        }
    }
}
