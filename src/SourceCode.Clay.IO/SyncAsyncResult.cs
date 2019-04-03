#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using System;
using System.Threading;

namespace SourceCode.Clay.IO
{
    internal sealed class SyncAsyncResult : IAsyncResult
    {
        public object AsyncState { get; }

        public WaitHandle AsyncWaitHandle => CompletedWaitHandle.Instance;

        public bool CompletedSynchronously => true;

        public bool IsCompleted => true;

        public int BytesCopied { get; }

        public AsyncCallback AsyncCallback { get; }

        public SyncAsyncResult(object asyncState, int bytesCopied, AsyncCallback asyncCallback)
        {
            AsyncState = asyncState;
            BytesCopied = bytesCopied;
            AsyncCallback = asyncCallback;
        }

        public void ThreadPoolWorkItem(object state)
        {
            using (AsyncWaitHandle)
            {
                AsyncCallback?.Invoke(this);
            }
        }
    }
}
