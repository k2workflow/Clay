#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using System;
using System.Collections.Generic;

namespace SourceCode.Clay.WaitAndRetry
{
    /// <summary>
    /// Helper methods for creating backoff strategies.
    /// </summary>
    public static partial class Backoff
    {
        private static IEnumerable<TimeSpan> Empty()
        {
            yield break;
        }
    }
}
