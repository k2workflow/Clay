#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using System;
using System.Threading;

namespace SourceCode.Clay.IO
{
    internal sealed class CompletedWaitHandle : WaitHandle
    {
        public static readonly CompletedWaitHandle Instance = new CompletedWaitHandle();

        private CompletedWaitHandle()
        {
            GC.SuppressFinalize(this);
        }

        protected override void Dispose(bool explicitDisposing)
        {
            // Prevent the SafeWaitHandle from being disposed - it is a singleton.
        }

        public override bool WaitOne() => true;

        public override bool WaitOne(int millisecondsTimeout) => true;

        public override bool WaitOne(int millisecondsTimeout, bool exitContext) => true;

        public override bool WaitOne(TimeSpan timeout) => true;

        public override bool WaitOne(TimeSpan timeout, bool exitContext) => true;

        public override void Close()
        {
        }
    }
}
