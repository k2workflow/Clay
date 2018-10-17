#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using Xunit;

namespace SourceCode.Clay.Tests
{
    public static class VoidResultTests
    {
        [Fact(DisplayName = nameof(VoidResult_Equality_New))]
        public static void VoidResult_Equality_New()
        {
            var @new = new VoidResult();

            Assert.Equal(default, @new);
            Assert.True(default == @new);
            Assert.False(default != @new);
            Assert.Equal(default(VoidResult).GetHashCode(), @new.GetHashCode());
        }

        [Fact(DisplayName = nameof(VoidResult_Equality_Default))]
        public static void VoidResult_Equality_Default()
        {
            var @default = default(VoidResult);

            Assert.Equal(default, @default);
            Assert.True(default == @default);
            Assert.False(default != @default);
            Assert.Equal(default(VoidResult).GetHashCode(), @default.GetHashCode());
        }

        [Fact(DisplayName = nameof(VoidResult_Equality_New_Default))]
        public static void VoidResult_Equality_New_Default()
        {
            var @new = new VoidResult();
            var @default = default(VoidResult);

            Assert.Equal(@new, @default);
            Assert.True(@new == @default);
            Assert.False(@new != @default);
            Assert.Equal(@new.GetHashCode(), @default.GetHashCode());
        }

        [Fact(DisplayName = nameof(VoidResult_Equality_Object))]
        public static void VoidResult_Equality_Object()
        {
            object @object = new VoidResult();

            Assert.Equal(default(VoidResult), @object);
            Assert.Equal(default(VoidResult).GetHashCode(), @object.GetHashCode());
        }
    }
}
