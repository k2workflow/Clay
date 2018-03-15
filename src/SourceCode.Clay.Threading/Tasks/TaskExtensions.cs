#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace SourceCode.Clay.Threading.Tasks
{
    /// <summary>
    /// Represents <see cref="Task"/> extensions.
    /// </summary>
    public static class TaskExtensions
    {
        #region Methods

        [DebuggerStepThrough]
        public static ConfiguredTaskAwaitable AnyContext(this Task task)
            => task.ConfigureAwait(false);

        [DebuggerStepThrough]
        public static ConfiguredTaskAwaitable<TResult> AnyContext<TResult>(this Task<TResult> task)
            => task.ConfigureAwait(false);

        #endregion
    }
}
