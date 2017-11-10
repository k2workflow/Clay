#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace SourceCode.Clay.Json
{
    public sealed class JTokenComparer : IEqualityComparer<JToken>
    {
        #region Constants

        public static IEqualityComparer<JToken> Default { get; } = new JTokenEqualityComparer();

        #endregion

        #region Constructors

        private JTokenComparer()
        { }

        #endregion

        #region IEqualityComparer

        public bool Equals(JToken x, JToken y) => Default.Equals(x, y);

        public int GetHashCode(JToken obj) => Default.GetHashCode(obj);

        #endregion
    }
}
