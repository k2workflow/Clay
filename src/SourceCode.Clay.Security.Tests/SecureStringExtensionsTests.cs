#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using SourceCode.Clay.Tests;
using System.Security;
using Xunit;

namespace SourceCode.Clay.Security.Tests
{
    public static class SecureStringExtensionsTests
    {
        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(When_convert_securestring))]
        public static void When_convert_securestring()
        {
            // Null
            SecureString actual = null;
            Assert.Null(actual.ToUnsecureString());

            // Empty
            actual = new SecureString();
            actual.MakeReadOnly();
            Assert.Equal(string.Empty, actual.ToUnsecureString());

            // Single
            actual = new SecureString();
            actual.AppendChar((char)0);
            actual.MakeReadOnly();
            Assert.Equal(string.Empty, actual.ToUnsecureString());

            // Single
            actual = new SecureString();
            actual.AppendChar('h');
            actual.MakeReadOnly();
            Assert.Equal("h", actual.ToUnsecureString());

            // Surrogate
            actual = new SecureString();
            actual.AppendChar(Sha1TestVectors.SurrogatePair[0]);
            actual.AppendChar(Sha1TestVectors.SurrogatePair[1]);
            actual.MakeReadOnly();
            Assert.Equal(Sha1TestVectors.SurrogatePair, actual.ToUnsecureString());

            // Short
            actual = new SecureString();
            actual.AppendChar('h');
            actual.AppendChar('e');
            actual.AppendChar('l');
            actual.AppendChar('L');
            actual.AppendChar('0');
            actual.MakeReadOnly();
            Assert.Equal("helL0", actual.ToUnsecureString());

            // Large
            actual = new SecureString();
            for (var i = 0; i < Sha1TestVectors.LongStr.Length; i++)
                actual.AppendChar(Sha1TestVectors.LongStr[i]);
            actual.MakeReadOnly();
            Assert.Equal(Sha1TestVectors.LongStr, actual.ToUnsecureString());
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
            Assert.Equal(string.Empty, ss.ToUnsecureString());

            // Single
            actual = new string(new char[] { (char)0 });
            ss = actual.ToSecureString();
            Assert.True(ss.IsReadOnly());
            Assert.Equal(string.Empty, ss.ToUnsecureString());

            // Single
            actual = "h";
            ss = actual.ToSecureString();
            Assert.True(ss.IsReadOnly());
            Assert.Equal("h", ss.ToUnsecureString());

            // Surrogate
            actual = Sha1TestVectors.SurrogatePair;
            ss = actual.ToSecureString();
            Assert.True(ss.IsReadOnly());
            Assert.Equal(Sha1TestVectors.SurrogatePair, ss.ToUnsecureString());

            // Short
            actual = "helL0";
            ss = actual.ToSecureString();
            Assert.True(ss.IsReadOnly());
            Assert.Equal("helL0", ss.ToUnsecureString());

            // Large
            actual = Sha1TestVectors.LongStr;
            ss = actual.ToSecureString();
            Assert.True(ss.IsReadOnly());
            Assert.Equal(Sha1TestVectors.LongStr, ss.ToUnsecureString());
        }
    }
}
