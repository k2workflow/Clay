#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace SourceCode.Clay.Threading
{
    /// <summary>
    /// Provides support for asynchronous looping.
    /// </summary>
    public static class ParallelAsync
    {
        /// <summary>
        /// Executes a For loop in which executions may run asynchronously.
        /// </summary>
        /// <param name="fromInclusive">The start index, inclusive.</param>
        /// <param name="toExclusive">The end index, exclusive.</param>
        /// <param name="options">Options that control the loop execution.</param>
        /// <param name="action">The delegate that is invoked once per iteration.</param>
        /// <returns></returns>
        public static async Task ForAsync(int fromInclusive, int toExclusive, ParallelOptions options, Func<int, Task> action)
        {
            if (toExclusive < fromInclusive) throw new ArgumentOutOfRangeException(nameof(toExclusive));
            if (action is null) throw new ArgumentNullException(nameof(action));

            ExecutionDataflowBlockOptions opt = Build(options);

            var block = new ActionBlock<int>(action, opt);

            // Send
            for (var i = fromInclusive; i < toExclusive; i++)
            {
                await block.SendAsync(i, opt.CancellationToken).ConfigureAwait(false);
            }

            block.Complete();
            await block.Completion.ConfigureAwait(false);
        }

        /// <summary>
        /// Executes a For loop in which executions may run in parallel. Returns a set of correlated values.
        /// </summary>
        /// <typeparam name="TValue">The type of items to return.</typeparam>
        /// <param name="fromInclusive">The start index, inclusive.</param>
        /// <param name="toExclusive">The end index, exclusive.</param>
        /// <param name="options">Options that control the loop execution.</param>
        /// <param name="func">The delegate that is invoked once per iteration.</param>
        /// <returns></returns>
        public static async Task<IReadOnlyDictionary<int, TValue>> ForAsync<TValue>(int fromInclusive, int toExclusive, ParallelOptions options, Func<int, Task<TValue>> func)
        {
            if (toExclusive < fromInclusive) throw new ArgumentOutOfRangeException(nameof(toExclusive));
            if (func is null) throw new ArgumentNullException(nameof(func));

            var dict = new ConcurrentDictionary<int, TValue>();

            ExecutionDataflowBlockOptions opt = Build(options);

            var block = new TransformBlock<int, TValue>(func, opt);

            // Send
            for (var i = fromInclusive; i < toExclusive; i++)
            {
                await block.SendAsync(i, opt.CancellationToken).ConfigureAwait(false);
            }

            // Receive
            for (var i = fromInclusive; i < toExclusive; i++)
            {
                TValue value = await block.ReceiveAsync(opt.CancellationToken).ConfigureAwait(false);
                dict[i] = value;
            }

            block.Complete();
            await block.Completion.ConfigureAwait(false);

            return dict;
        }

        /// <summary>
        /// Executes a ForEach loop in which executions may run in parallel.
        /// </summary>
        /// <typeparam name="TSource">The type of items in the source data.</typeparam>
        /// <param name="source">An enumerable data source.</param>
        /// <param name="options">Options that control the loop execution.</param>
        /// <param name="action">The delegate that is invoked once per iteration.</param>
        /// <returns></returns>
        public static async Task ForEachAsync<TSource>(IEnumerable<TSource> source, ParallelOptions options, Func<TSource, Task> action)
        {
            if (source is null) return;
            if (action is null) throw new ArgumentNullException(nameof(action));

            ExecutionDataflowBlockOptions opt = Build(options);

            var block = new ActionBlock<TSource>(action, opt);

            // Send
            foreach (TSource item in source)
            {
                await block.SendAsync(item, opt.CancellationToken).ConfigureAwait(false);
            }

            block.Complete();
            await block.Completion.ConfigureAwait(false);
        }

        /// <summary>
        /// Executes a ForEach loop in which executions may run in parallel. Returns a set of correlated values.
        /// </summary>
        /// <typeparam name="TSource">The type of items in the source data.</typeparam>
        /// <typeparam name="TValue">The type of items to return.</typeparam>
        /// <param name="source">An enumerable data source.</param>
        /// <param name="options">Options that control the loop execution.</param>
        /// <param name="func">The delegate that is invoked once per iteration.</param>
        /// <returns></returns>
        public static async Task<IReadOnlyDictionary<TSource, TValue>> ForEachAsync<TSource, TValue>(IEnumerable<TSource> source, ParallelOptions options, Func<TSource, Task<KeyValuePair<TSource, TValue>>> func)
        {
            if (source is null)
                return new Dictionary<TSource, TValue>(0);

            if (func is null) throw new ArgumentNullException(nameof(func));

            var dict = new ConcurrentDictionary<TSource, TValue>();

            ExecutionDataflowBlockOptions opt = Build(options);
            var block = new TransformBlock<TSource, KeyValuePair<TSource, TValue>>(func, opt);

            // Send
            var count = 0;
            foreach (TSource item in source)
            {
                await block.SendAsync(item, opt.CancellationToken).ConfigureAwait(false);
                count++;
            }

            // Receive
            for (var i = 0; i < count; i++)
            {
                KeyValuePair<TSource, TValue> value = await block.ReceiveAsync(opt.CancellationToken).ConfigureAwait(false);
                dict[value.Key] = value.Value;
            }

            block.Complete();
            await block.Completion.ConfigureAwait(false);

            return dict;
        }

        private static ExecutionDataflowBlockOptions Build(ParallelOptions options) => new ExecutionDataflowBlockOptions
        {
            CancellationToken = options is null ? CancellationToken.None : options.CancellationToken,
            MaxDegreeOfParallelism = options is null ? -1 : options.MaxDegreeOfParallelism
        };
    }
}
