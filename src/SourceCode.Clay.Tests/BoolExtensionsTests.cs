#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using Xunit;

namespace SourceCode.Clay.Tests
{
    public static class BoolExtensionsTests
    {
        [Fact(DisplayName = nameof(BoolToByte))]
        public static void BoolToByte()
        {
            Assert.Equal(1, true.ToByte());
            Assert.Equal(0, false.ToByte());

            Assert.Equal(1, true.ToByteUnsafe());
            Assert.Equal(0, false.ToByteUnsafe());
        }
    }
}