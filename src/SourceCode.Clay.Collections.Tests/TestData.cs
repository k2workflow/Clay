#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using System;
using System.Collections.Generic;

namespace SourceCode.Clay.Collections.Tests
{
    internal static class TestData
    {
        #region Constants

        public static readonly string[] List =
        {
            "foo",
            "bar",
            "baz",
            "nin"
        };

        public static readonly HashSet<string> Set = new HashSet<string>(List, StringComparer.Ordinal);

        public static readonly Dictionary<string, string> Dict = new Dictionary<string, string>()
        {
            ["foo"] = "foo1",
            ["bar"] = "bar1",
            ["baz"] = "baz1",
            ["nin"] = "nin1"
        };

        public static readonly string[] Null = null;

        #endregion
    }
}
