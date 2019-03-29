using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace SourceCode.Clay.Net.Http.Tests
{
    public static class UriTokenTests
    {
        [Fact]
        public static void UriToken_Ctor_Collection()
        {
            var token = new UriToken(UriTokenType.Value, "Users[]", null, null);
            Assert.Equal(UriTokenType.Collection, token.Type);
            Assert.Equal("Users", token.Name);
        }

        [Fact]
        public static void UriToken_Ctor_SubName()
        {
            var token = new UriToken(UriTokenType.Value, "Options.Flag", null, null);
            Assert.Equal(UriTokenType.Value, token.Type);
            Assert.Equal("Options", token.Name);
            Assert.Equal("Flag", token.SubName);
        }
    }
}
