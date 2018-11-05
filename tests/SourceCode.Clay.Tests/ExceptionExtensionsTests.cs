#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using System;
using Xunit;

namespace SourceCode.Clay.Tests
{
    public static class ExceptionExtensionsTests
    {
        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(When_suppress_exception))]
        public static void When_suppress_exception()
        {
            var expected = new Exception();

            try
            {
                throw expected;
            }
            catch (Exception e)
            {
                Exception actual = e.Suppress();

                Assert.Equal(expected, actual);
            }
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(When_fatal_exception))]
        public static void When_fatal_exception()
        {
            Assert.True(new OutOfMemoryException().IsFatal());
            Assert.False(new InsufficientMemoryException().IsFatal());

            Assert.True(new AccessViolationException().IsFatal());
            Assert.True(new System.Runtime.InteropServices.SEHException().IsFatal());
            Assert.True(new StackOverflowException().IsFatal());

            Assert.False(new TypeInitializationException("foo", new Exception()).IsFatal());
            Assert.True(new TypeInitializationException("foo", new OutOfMemoryException()).IsFatal());

            Assert.False(new System.Reflection.TargetInvocationException(new Exception()).IsFatal());
            Assert.True(new System.Reflection.TargetInvocationException(new OutOfMemoryException()).IsFatal());

            Assert.False(new System.Reflection.TargetInvocationException(new TypeInitializationException("foo", new InsufficientMemoryException())).IsFatal());
        }
    }
}
