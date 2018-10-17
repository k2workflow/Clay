#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using Xunit;

namespace SourceCode.Clay.Tests
{
    public static class SemanticVersionComparerTests
    {
        [InlineData(
            0, 1, 2, null, null,
            0, 1, 2, null, null,
            true, true
        )]
        [InlineData(
            0, 1, 2, "pre1", null,
            0, 1, 2, "pre1", null,
            true, true
        )]
        [InlineData(
            0, 1, 2, "pre1", "build1",
            0, 1, 2, "pre1", "build1",
            true, true
        )]
        [InlineData(
            0, 1, 2, null, null,
            1, 1, 2, null, null,
            false, false
        )]
        [InlineData(
            1, 1, 2, null, null,
            0, 1, 2, null, null,
            false, false
        )]
        [InlineData(
            0, 1, 2, null, null,
            0, 2, 2, null, null,
            false, false
        )]
        [InlineData(
            0, 2, 2, null, null,
            0, 1, 2, null, null,
            false, false
        )]
        [InlineData(
            0, 1, 3, null, null,
            0, 1, 2, null, null,
            false, false
        )]
        [InlineData(
            0, 1, 2, null, null,
            0, 1, 3, null, null,
            false, false
        )]
        [InlineData(
            0, 1, 2, "pre1", null,
            0, 1, 2, null, null,
            false, false
        )]
        [InlineData(
            0, 1, 2, null, "build1",
            0, 1, 2, null, null,
            true, false
        )]
        [InlineData(
            0, 1, 2, null, null,
            0, 1, 2, "pre1", null,
            false, false
        )]
        [InlineData(
            0, 1, 2, null, null,
            0, 1, 2, null, "build1",
            true, false
        )]
        [InlineData(
            0, 1, 2, "pre1", null,
            0, 1, 2, "pre2", null,
            false, false
        )]
        [InlineData(
            0, 1, 2, null, "build1",
            0, 1, 2, null, "build2",
            true, false
        )]
        [InlineData(
            0, 1, 2, "pre2", null,
            0, 1, 2, "pre1", null,
            false, false
        )]
        [InlineData(
            0, 1, 2, null, "build2",
            0, 1, 2, null, "build1",
            true, false
        )]
        [Theory(DisplayName = nameof(SemanticVersionComparer_Equals))]
        public static void SemanticVersionComparer_Equals(
            int major1, int minor1, int patch1, string pre1, string meta1,
            int major2, int minor2, int patch2, string pre2, string meta2,
            bool standardEquals, bool strictEquals)
        {
            var sut1 = new SemanticVersion(major1, minor1, patch1, pre1, meta1);
            var sut2 = new SemanticVersion(major2, minor2, patch2, pre2, meta2);

            if (standardEquals)
                Assert.True(SemanticVersionComparer.Standard.Equals(sut1, sut2));
            else
                Assert.False(SemanticVersionComparer.Standard.Equals(sut1, sut2));

            if (strictEquals)
                Assert.True(SemanticVersionComparer.Strict.Equals(sut1, sut2));
            else
                Assert.False(SemanticVersionComparer.Strict.Equals(sut1, sut2));
        }

        [InlineData(
            0, 1, 2, null, null,
            0, 1, 2, null, null,
            0, 0
        )]
        [InlineData(
            0, 1, 2, "pre1", null,
            0, 1, 2, "pre1", null,
            0, 0
        )]
        [InlineData(
            0, 1, 2, "pre1", "build1",
            0, 1, 2, "pre1", "build1",
            0, 0
        )]
        [InlineData(
            0, 1, 2, null, null,
            1, 1, 2, null, null,
            -1, -1
        )]
        [InlineData(
            1, 1, 2, null, null,
            0, 1, 2, null, null,
            1, 1
        )]
        [InlineData(
            0, 1, 2, null, null,
            0, 2, 2, null, null,
            -1, -1
        )]
        [InlineData(
            0, 2, 2, null, null,
            0, 1, 2, null, null,
            1, 1
        )]
        [InlineData(
            0, 1, 2, null, null,
            0, 1, 3, null, null,
            -1, -1
        )]
        [InlineData(
            0, 1, 3, null, null,
            0, 1, 2, null, null,
            1, 1
        )]
        [InlineData(
            0, 1, 2, "pre1", null,
            0, 1, 2, null, null,
            1, 1
        )]
        [InlineData(
            0, 1, 2, null, "build1",
            0, 1, 2, null, null,
            0, 1
        )]
        [InlineData(
            0, 1, 2, null, null,
            0, 1, 2, "pre1", null,
            -1, -1
        )]
        [InlineData(
            0, 1, 2, null, null,
            0, 1, 2, null, "build1",
            0, -1
        )]
        [InlineData(
            0, 1, 2, "pre1", null,
            0, 1, 2, "pre2", null,
            -1, -1
        )]
        [InlineData(
            0, 1, 2, null, "build1",
            0, 1, 2, null, "build2",
            0, -1
        )]
        [InlineData(
            0, 1, 2, "pre2", null,
            0, 1, 2, "pre1", null,
            1, 1
        )]
        [InlineData(
            0, 1, 2, null, "build2",
            0, 1, 2, null, "build1",
            0, 1
        )]
        [Theory(DisplayName = nameof(SemanticVersionComparer_Compare))]
        public static void SemanticVersionComparer_Compare(
            int major1, int minor1, int patch1, string pre1, string meta1,
            int major2, int minor2, int patch2, string pre2, string meta2,
            int standardResult, int strictResult)
        {
            var sut1 = new SemanticVersion(major1, minor1, patch1, pre1, meta1);
            var sut2 = new SemanticVersion(major2, minor2, patch2, pre2, meta2);

            var standardActual = SemanticVersionComparer.Standard.Compare(sut1, sut2);
            var strictActual = SemanticVersionComparer.Strict.Compare(sut1, sut2);

            Assert.Equal(standardResult < 0, standardActual < 0);
            Assert.Equal(standardResult > 0, standardActual > 0);

            Assert.Equal(strictResult < 0, strictActual < 0);
            Assert.Equal(strictResult > 0, strictActual > 0);
        }
    }
}
