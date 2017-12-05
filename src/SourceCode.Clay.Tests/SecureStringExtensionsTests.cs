#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using System;
using System.Security;
using Xunit;

namespace SourceCode.Clay.Tests
{
    public static class SecureStringExtensionsTests
    {
        #region Methods

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(When_convert_securestring))]
        public static void When_convert_securestring()
        {
            // Null
            SecureString actual = null;
            Assert.Null(actual.ToUnsecureString());

            // Empty
            actual = new SecureString();
            Assert.Equal(string.Empty, actual.ToUnsecureString());

            // Single
            actual.AppendChar('h');
            Assert.Equal("h", actual.ToUnsecureString());

            // Multi
            actual.AppendChar('e');
            actual.AppendChar('l');
            actual.AppendChar('L');
            actual.AppendChar('0');
            Assert.Equal("helL0", actual.ToUnsecureString());
        }

        #endregion
    }
}
