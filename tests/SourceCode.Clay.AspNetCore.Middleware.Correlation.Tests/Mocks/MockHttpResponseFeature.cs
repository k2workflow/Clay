using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;

namespace SourceCode.Clay.AspNetCore.Middleware.Correlation.Tests.Mocks
{
    public class MockHttpResponseFeature : IHttpResponseFeature
    {
        #region IHttpResponseFeature Members

        public int StatusCode { get; set; }
        public string ReasonPhrase { get; set; }
#pragma warning disable CA2227 // Collection properties should be read only - Part of interface
        public IHeaderDictionary Headers { get; set; } = new MockHttpHeadersDictionary();
#pragma warning restore CA2227 // Collection properties should be read only - Part of interface
        public Stream Body { get; set; }
        public bool HasStarted { get; set; }

        public void OnCompleted(Func<object, Task> callback, object state)
        {
            OnCompletedCallbacks.Enqueue(new Tuple<Func<object, Task>, object>(callback, state));
        }

        public void OnStarting(Func<object, Task> callback, object state)
        {
            OnStartingCallbacks.Enqueue(new Tuple<Func<object, Task>, object>(callback, state));
        }

        #endregion

        public async Task StartAsync()
        {
            HasStarted = true;
            while (OnStartingCallbacks.Count > 0)
            {
                (Func<object, Task> callback, object state) = OnStartingCallbacks.Dequeue();
                if (callback != null)
                {
                    await callback.Invoke(state).ConfigureAwait(false);
                }
            }
        }

        public async Task CompleteAsync()
        {
            if (!HasStarted)
            {
                await StartAsync().ConfigureAwait(false);
            }
            while (OnCompletedCallbacks.Count > 0)
            {
                (Func<object, Task> callback, object state) = OnCompletedCallbacks.Dequeue();
                if (callback != null)
                {
                    await callback.Invoke(state)
                        .ConfigureAwait(false);
                }
            }
        }

        public Queue<Tuple<Func<object, Task>, object>> OnCompletedCallbacks { get; } = new Queue<Tuple<Func<object, Task>, object>>();
        public Queue<Tuple<Func<object, Task>, object>> OnStartingCallbacks { get; } = new Queue<Tuple<Func<object, Task>, object>>();
    }
}
