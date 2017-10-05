#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using Xunit;

namespace SourceCode.Clay.Collections.Generic.Tests
{
    public static class DictionaryTests
    {
        #region Methods

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(Dictionary_Empty))]
        public static void Dictionary_Empty()
        {
            var empty = Dictionary.Empty<string, int>();

            Assert.Equal(0, empty.Count);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(ReadOnlyDictionary_Empty))]
        public static void ReadOnlyDictionary_Empty()
        {
            var empty = ReadOnlyDictionary.Empty<string, int>();

            Assert.Equal(0, empty.Count);
        }

        #endregion
    }
}
