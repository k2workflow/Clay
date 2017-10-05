using SourceCode.Clay.Collections.Generic;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace SourceCode.Clay.Threading
{
    public static class ParallelAsync
    {
        #region For

        public static async Task ForAsync(int from, int to, ParallelOptions options, Func<int, Task> action)
        {
            if (to < from) throw new ArgumentOutOfRangeException(nameof(to));
            if (action == null) throw new ArgumentNullException(nameof(action));

            var opt = new ExecutionDataflowBlockOptions
            {
                CancellationToken = options == null ? CancellationToken.None : options.CancellationToken,
                MaxDegreeOfParallelism = options == null ? -1 : options.MaxDegreeOfParallelism
            };

            var block = new ActionBlock<int>(action, opt);

            // Send
            for (var i = from; i < to; i++)
            {
                await block.SendAsync(i, opt.CancellationToken).ConfigureAwait(false);
            }

            block.Complete();
            await block.Completion;
        }

        public static async Task<IReadOnlyDictionary<int, TValue>> ForAsync<TValue>(int from, int to, ParallelOptions options, Func<int, Task<TValue>> func)
        {
            if (to < from) throw new ArgumentOutOfRangeException(nameof(to));
            if (func == null) throw new ArgumentNullException(nameof(func));

            var dict = new ConcurrentDictionary<int, TValue>();

            var opt = new ExecutionDataflowBlockOptions
            {
                CancellationToken = options == null ? CancellationToken.None : options.CancellationToken,
                MaxDegreeOfParallelism = options == null ? -1 : options.MaxDegreeOfParallelism
            };

            var block = new TransformBlock<int, TValue>(func, opt);

            // Send
            for (var i = from; i < to; i++)
            {
                await block.SendAsync(i, opt.CancellationToken).ConfigureAwait(false);
            }

            // Receive
            for (var i = from; i < to; i++)
            {
                var value = await block.ReceiveAsync(opt.CancellationToken).ConfigureAwait(false);
                dict[i] = value;
            }

            block.Complete();
            await block.Completion;

            return dict;
        }

        #endregion

        #region ForEach

        public static async Task ForEachAsync<TSource>(IEnumerable<TSource> source, ParallelOptions options, Func<TSource, Task> action)
        {
            if (source == null) return;
            if (action == null) throw new ArgumentNullException(nameof(action));

            var opt = new ExecutionDataflowBlockOptions
            {
                CancellationToken = options == null ? CancellationToken.None : options.CancellationToken,
                MaxDegreeOfParallelism = options == null ? -1 : options.MaxDegreeOfParallelism
            };

            var block = new ActionBlock<TSource>(action, opt);

            // Send
            foreach (var item in source)
            {
                await block.SendAsync(item, opt.CancellationToken).ConfigureAwait(false);
            }

            block.Complete();
            await block.Completion;
        }

        public static async ValueTask<IReadOnlyDictionary<TSource, TValue>> ForEachAsync<TSource, TValue>(IEnumerable<TSource> source, ParallelOptions options, Func<TSource, Task<KeyValuePair<TSource, TValue>>> func)
        {
            if (source == null) return EmptyDictionaryImpl<TSource, TValue>.ReadOnlyValue;
            if (func == null) throw new ArgumentNullException(nameof(func));

            var dict = new ConcurrentDictionary<TSource, TValue>();

            var opt = new ExecutionDataflowBlockOptions
            {
                CancellationToken = options == null ? CancellationToken.None : options.CancellationToken,
                MaxDegreeOfParallelism = options == null ? -1 : options.MaxDegreeOfParallelism
            };

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

        #endregion
    }
}
