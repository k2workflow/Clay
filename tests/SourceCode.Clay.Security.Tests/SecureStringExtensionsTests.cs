#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using System.Security;
using SourceCode.Clay.Tests;
using Xunit;

namespace SourceCode.Clay.Security.Tests
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public static class SecureStringExtensionsTests
    {
        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(When_convert_securestring))]
        public static void When_convert_securestring()
        {
            // Null
            SecureString actual = null;
            Assert.Null(actual.ToClearText());

            // Empty
            actual = new SecureString();
            actual.MakeReadOnly();
            Assert.Equal(string.Empty, actual.ToClearText());

            // Single
            actual = new SecureString();
            actual.AppendChar((char)0);
            actual.MakeReadOnly();
            Assert.Equal(string.Empty, actual.ToClearText());

            // Single
            actual = new SecureString();
            actual.AppendChar('h');
            actual.MakeReadOnly();
            Assert.Equal("h", actual.ToClearText());

            // Surrogate
            actual = new SecureString();
            actual.AppendChar(TestConstants.SurrogatePair[0]);
            actual.AppendChar(TestConstants.SurrogatePair[1]);
            actual.MakeReadOnly();
            Assert.Equal(TestConstants.SurrogatePair, actual.ToClearText());

            // Short
            actual = new SecureString();
            actual.AppendChar('h');
            actual.AppendChar('e');
            actual.AppendChar('l');
            actual.AppendChar('L');
            actual.AppendChar('0');
            actual.MakeReadOnly();
            Assert.Equal("helL0", actual.ToClearText());

            // Large
            actual = new SecureString();
            for (var i = 0; i < TestConstants.LongStr.Length; i++)
                actual.AppendChar(TestConstants.LongStr[i]);
            actual.MakeReadOnly();
            Assert.Equal(TestConstants.LongStr, actual.ToClearText());
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(When_convert_string))]
        public static void When_convert_string()
        {
            // Null
            string actual = null;
            Assert.Null(actual.ToSecureString());

            // Empty
            actual = string.Empty;
            var ss = actual.ToSecureString();
            Assert.True(ss.IsReadOnly());
            Assert.Equal(string.Empty, ss.ToClearText());

            // Single
            actual = new string(new char[] { (char)0 });
            ss = actual.ToSecureString();
            Assert.True(ss.IsReadOnly());
            Assert.Equal(string.Empty, ss.ToClearText());

            // Single
            actual = "h";
            ss = actual.ToSecureString();
            Assert.True(ss.IsReadOnly());
            Assert.Equal("h", ss.ToClearText());

            // Surrogate
            actual = TestConstants.SurrogatePair;
            ss = actual.ToSecureString();
            Assert.True(ss.IsReadOnly());
            Assert.Equal(TestConstants.SurrogatePair, ss.ToClearText());

            // Short
            actual = "helL0";
            ss = actual.ToSecureString();
            Assert.True(ss.IsReadOnly());
            Assert.Equal("helL0", ss.ToClearText());

            // Large
            actual = TestConstants.LongStr;
            ss = actual.ToSecureString();
            Assert.True(ss.IsReadOnly());
            Assert.Equal(TestConstants.LongStr, ss.ToClearText());
        }
    }
}
