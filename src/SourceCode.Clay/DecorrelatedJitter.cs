using System;
using System.Collections;
using System.Collections.Generic;

namespace SourceCode.Clay
{
    // Background: https://www.awsarchitectureblog.com/2015/03/backoff.html
    public sealed class DecorrelatedJitter
    {
        private readonly Random _random;

        public int RetryCount { get; }
        public TimeSpan MinDelay { get; }
        public TimeSpan MaxDelay { get; }

        public DecorrelatedJitter(int retryCount, TimeSpan minDelay, TimeSpan maxDelay, Random random = null)
        {
            if (retryCount < 0) throw new ArgumentOutOfRangeException(nameof(retryCount));
            if (minDelay < TimeSpan.Zero) throw new ArgumentOutOfRangeException(nameof(minDelay));
            if (maxDelay < minDelay) throw new ArgumentOutOfRangeException(nameof(maxDelay));

            _random = random ?? new Random((int)DateTime.Now.Ticks);
            RetryCount = retryCount;
            MinDelay = minDelay;
            MaxDelay = maxDelay;
        }

        public IEnumerable<TimeSpan> Create() => new Impl(this);

        private sealed class Impl : IEnumerable<TimeSpan>
        {
            private readonly TimeSpan[] _delay;

            public Impl(DecorrelatedJitter jitter)
            {
                _delay = new TimeSpan[jitter.RetryCount];

                var ms = jitter.MinDelay.TotalMilliseconds;
                for (var i = 0; i < _delay.Length; i++)
                {
                    ms *= 3.0 * jitter._random.NextDouble(); // [0.0, 3.0)
                    ms = Math.Max(jitter.MinDelay.TotalMilliseconds, ms); // [min, N]
                    ms = Math.Min(jitter.MaxDelay.TotalMilliseconds, ms); // [min, max]

                    _delay[i] = TimeSpan.FromMilliseconds(ms);
                }
            }

            public IEnumerator<TimeSpan> GetEnumerator() => ((IEnumerable<TimeSpan>)_delay).GetEnumerator();

            IEnumerator IEnumerable.GetEnumerator() => _delay.GetEnumerator();
        }
    }
}
