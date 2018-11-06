using System;
using System.Collections.Generic;
using System.Linq;

namespace SourceCode.Clay
{
    /// <summary>
    /// Implements a mechanism for transient error handling, whereby
    /// the backoff timings should be randomized.
    /// See background here: https://www.awsarchitectureblog.com/2015/03/backoff.html.
    /// </summary>
    public sealed class DecorrelatedJitter
    {
        private static readonly Random s_random = new Random(); // Default ctor uses a time-based seed
        private readonly Random _random;

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
        public DecorrelatedJitter(int retryCount, TimeSpan minDelay, TimeSpan maxDelay, Random random = null)
        {
            if (retryCount < 0) throw new ArgumentOutOfRangeException(nameof(retryCount));
            if (minDelay < TimeSpan.Zero) throw new ArgumentOutOfRangeException(nameof(minDelay));
            if (maxDelay < minDelay) throw new ArgumentOutOfRangeException(nameof(maxDelay));

            _random = random ?? s_random;
            RetryCount = retryCount;
            MinDelay = minDelay;
            MaxDelay = maxDelay;
        }

        /// <summary>
        /// Create a series of random <see cref="TimeSpan"/> values from a Uniform distribution.
        /// Uses the original AWS style of computing the values.
        /// </summary>
        /// <remarks>A new enumerator should be created for every execution.</remarks>
        public IEnumerable<TimeSpan> Default()
        {
            double ms = MinDelay.TotalMilliseconds;

            for (int i = 0; i < RetryCount; i++)
            {
                ms *= 3.0 * _random.NextDouble(); // [0, 3N)
                ms = ms.Clamp(MinDelay.TotalMilliseconds, MaxDelay.TotalMilliseconds); // [min, max]

                yield return TimeSpan.FromMilliseconds(ms);
            }
        }

        /// <summary>
        /// Create a series of random <see cref="TimeSpan"/> values from a Normal distribution.
        /// </summary>
        /// <remarks>A new enumerator should be created for every execution.</remarks>
        public IEnumerable<TimeSpan> Normal()
            => _random
            .ClampedNormalSample(RetryCount, MinDelay.TotalMilliseconds, MaxDelay.TotalMilliseconds)
            .Select(n => TimeSpan.FromMilliseconds(n));
    }
}
