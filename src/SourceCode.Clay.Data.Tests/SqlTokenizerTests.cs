#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using SourceCode.Clay.Data.SqlParser;
using Xunit;

namespace SourceCode.Clay.Data.SqlClient.Tests
{
    public static class SqlTokenizerTests
    {
        #region Methods

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(When_quote_square_null_empty))]
        public static void When_quote_square_null_empty()
        {
            Assert.Null(SqlTokenizer.EncodeNameSquare(null));
            Assert.Equal(string.Empty, SqlTokenizer.EncodeNameSquare(string.Empty));
            Assert.Equal("[ ]", SqlTokenizer.EncodeNameSquare(" "));
        }

        [Trait("Type", "Unit")]
        [Theory(DisplayName = nameof(When_quote_square))]
        [InlineData("a", "[a]")]
        [InlineData(" a", "[ a]")]
        [InlineData("a ", "[a ]")]
        [InlineData("a]", "[a]]]")]
        [InlineData("[a", "[[a]")]
        [InlineData("[a]", "[[a]]]")]
        public static void When_quote_square(string identifier, string expected)
        {
            var actual = SqlTokenizer.EncodeNameSquare(identifier);
            Assert.Equal(expected, actual);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(When_quote_quote_null_empty))]
        public static void When_quote_quote_null_empty()
        {
            Assert.Null(SqlTokenizer.EncodeNameQuotes(null));
            Assert.Equal(string.Empty, SqlTokenizer.EncodeNameQuotes(string.Empty));
            Assert.Equal("\" \"", SqlTokenizer.EncodeNameQuotes(" "));
        }

        [Trait("Type", "Unit")]
        [Theory(DisplayName = nameof(When_quote_quote))]
        [InlineData("a", "\"a\"")]
        [InlineData(" a", "\" a\"")]
        [InlineData("a ", "\"a \"")]
        [InlineData("a\"", "\"a\"\"\"")]
        [InlineData("\"a", "\"\"\"a\"")]
        [InlineData("\"a\"", "\"\"\"a\"\"\"")]
        public static void When_quote_quote(string identifier, string expected)
        {
            var actual = SqlTokenizer.EncodeNameQuotes(identifier);
            Assert.Equal(expected, actual);
        }

        #endregion
    }
}
