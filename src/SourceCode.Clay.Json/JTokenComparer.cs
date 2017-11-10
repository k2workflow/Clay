#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace SourceCode.Clay.Json
{
    public static class JTokenComparer
    {
        #region Constants

        public static IEqualityComparer<JToken> Default { get; } = new JTokenEqualityComparer();

        #endregion
    }
}
