#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using System;
using System.Threading;

namespace SourceCode.Clay.Threading
{
    /// <summary>
    /// Represents <see cref="TimeSpan"/> extensions.
    /// </summary>
    public static class TimeSpanExtensions
    {
        #region Methods

        /// <summary>
        /// Creates a <see cref="CancellationTokenSource"/> using the provided <see cref="TimeSpan"/> value as the delay value.
        /// A positive <see cref="TimeSpan"/> value is passed through to the <see cref="CancellationTokenSource"/> constructor.
        /// A zero or negative <see cref="TimeSpan"/> value will generate an already-cancelled <see cref="CancellationTokenSource"/> instance.
        /// </summary>
        /// <param name="delay">The time interval to wait before canceling this System.Threading.CancellationTokenSource.</param>
        /// <returns></returns>
        public static CancellationTokenSource ToCancellationTokenSource(this TimeSpan delay)
        {
            // If the timeout is positive, honor it
            if (delay.Ticks > 0)
                return new CancellationTokenSource(delay);

            // If the timeout is zero or less, return an already-cancelled value
            var source = new CancellationTokenSource();
            source.Cancel();
            return source;
        }

        /// <summary>
        /// Creates a <see cref="CancellationTokenSource"/> using the provided <see cref="TimeSpan"/> value as the delay value.
        /// A positive <see cref="TimeSpan"/> value is passed through to the <see cref="CancellationTokenSource"/> constructor.
        /// A zero or negative <see cref="TimeSpan"/> value will generate an already-cancelled <see cref="CancellationTokenSource"/> instance.
        /// A null <see cref="TimeSpan"/> value will call the default (empty) <see cref="CancellationTokenSource"/> constructor.
        /// </summary>
        /// <param name="delay">The time interval to wait before canceling this System.Threading.CancellationTokenSource.</param>
        /// <returns></returns>
        public static CancellationTokenSource ToCancellationTokenSource(this TimeSpan? delay)
        {
            // If no timeout specified, return the default value
            if (delay == null)
                return new CancellationTokenSource();

            var source = ToCancellationTokenSource(delay.Value);
            return source;
        }

        #endregion
    }
}
