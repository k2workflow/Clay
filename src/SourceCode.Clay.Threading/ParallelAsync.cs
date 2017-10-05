using SourceCode.Clay.Collections.Generic;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace SourceCode.Clay.Threading
{
    /// <summary>
    ///   Provides support for asynchronous looping.
    /// </summary>
    public static class ParallelAsync
    {
        #region For

        /// <summary>
        ///   Executes a For loop in which executions may run asynchronously.
        /// </summary>
        /// <param name="fromInclusive">The start index, inclusive.</param>
        /// <param name="toExclusive">The end index, exclusive.</param>
        /// <param name="options">Options that control the loop execution.</param>
        /// <param name="action">The delegate that is invoked once per iteration.</param>
        /// <returns></returns>
        public static async Task ForAsync(int fromInclusive, int toExclusive, ParallelOptions options, Func<int, Task> action)
        {
            if (toExclusive < fromInclusive) throw new ArgumentOutOfRangeException(nameof(toExclusive));
            if (action == null) throw new ArgumentNullException(nameof(action));

            var opt = Build(options);

            var block = new ActionBlock<int>(action, opt);

            // Send
            for (var i = fromInclusive; i < toExclusive; i++)
            {
                await block.SendAsync(i, opt.CancellationToken).ConfigureAwait(false);
            }

            block.Complete();
            await block.Completion;
        }

        /// <summary>
        ///   Executes a For loop in which executions may run in parallel. Returns a set of
        ///   correlated values.
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
            if (func == null) throw new ArgumentNullException(nameof(func));

            var dict = new ConcurrentDictionary<int, TValue>();

            var opt = Build(options);

            var block = new TransformBlock<int, TValue>(func, opt);

            // Send
            for (var i = fromInclusive; i < toExclusive; i++)
            {
                await block.SendAsync(i, opt.CancellationToken).ConfigureAwait(false);
            }

            // Receive
            for (var i = fromInclusive; i < toExclusive; i++)
            {
                var value = await block.ReceiveAsync(opt.CancellationToken).ConfigureAwait(false);
                dict[i] = value;
            }

            block.Complete();
            await block.Completion;

            return dict;
        }

        #endregion For

        #region ForEach

        /// <summary>
        ///   Executes a ForEach loop in which executions may run in parallel.
        /// </summary>
        /// <typeparam name="TSource">The type of items in the source data.</typeparam>
        /// <param name="source">An enumerable data source.</param>
        /// <param name="options">Options that control the loop execution.</param>
        /// <param name="action">The delegate that is invoked once per iteration.</param>
        /// <returns></returns>
        public static async Task ForEachAsync<TSource>(IEnumerable<TSource> source, ParallelOptions options, Func<TSource, Task> action)
        {
            if (source == null) return;
            if (action == null) throw new ArgumentNullException(nameof(action));

            var opt = Build(options);

            var block = new ActionBlock<TSource>(action, opt);

            // Send
            foreach (var item in source)
            {
                await block.SendAsync(item, opt.CancellationToken).ConfigureAwait(false);
            }

            block.Complete();
            await block.Completion;
        }

        /// <summary>
        ///   Executes a ForEach loop in which executions may run in parallel. Returns a set of
        ///   correlated values.
        /// </summary>
        /// <typeparam name="TSource">The type of items in the source data.</typeparam>
        /// <typeparam name="TValue">The type of items to return.</typeparam>
        /// <param name="source">An enumerable data source.</param>
        /// <param name="options">Options that control the loop execution.</param>
        /// <param name="func">The delegate that is invoked once per iteration.</param>
        /// <returns></returns>
        public static async ValueTask<IReadOnlyDictionary<TSource, TValue>> ForEachAsync<TSource, TValue>(IEnumerable<TSource> source, ParallelOptions options, Func<TSource, Task<KeyValuePair<TSource, TValue>>> func)
        {
            if (source == null) return EmptyDictionaryImpl<TSource, TValue>.ReadOnlyValue;
            if (func == null) throw new ArgumentNullException(nameof(func));

            var dict = new ConcurrentDictionary<TSource, TValue>();

            var opt = Build(options);

            var block = new TransformBlock<TSource, KeyValuePair<TSource, TValue>>(func, opt);

            // Send
            var count = 0;
            foreach (var item in source)
            {
                await block.SendAsync(item, opt.CancellationToken).ConfigureAwait(false);
                count++;
            }

            // Receive
            for (var i = 0; i < count; i++)
            {
                var value = await block.ReceiveAsync(opt.CancellationToken).ConfigureAwait(false);
                dict[value.Key] = value.Value;
            }

            block.Complete();
            await block.Completion;

            return dict;
        }

        #endregion ForEach

        #region Helpers

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static ExecutionDataflowBlockOptions Build(ParallelOptions options)
        => new ExecutionDataflowBlockOptions
        {
            CancellationToken = options == null ? CancellationToken.None : options.CancellationToken,
            MaxDegreeOfParallelism = options == null ? -1 : options.MaxDegreeOfParallelism
        };

        #endregion Helpers
    }
}
