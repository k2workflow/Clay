#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using System.Data;
using Xunit;

namespace SourceCode.Clay.Data.SqlParser.Tests
{
    public static class SqlParamBlockParserTests
    {
        #region Fields

        // Block comment is not closed
        private const string sqlBadComment = @"
			CReaTE /*blah
				FUNCTION abc.def()
            AS RETURNS INT
			BEGIN
                RETURN 1;
			END;";

        // [abc]]def]
        // [abc[]]def]
        private const string sqlEscapedNames = @"
			CReaTE /*blah*/
				FunCTIOn            --etc
			[abc]]def].[abc[]]def]
			( --test--
			)AS RETURNS
			    inT
			BEGIN RETURN 1;
			END; /**/";

        private const string sqlMessyProcWithParen = @"
			CReaTE /*blah*/
				PROCedure            --etc
			abc.[def]
					/*
						xyz -- /*
					*/
			( @p1  --test--
			/* yada
			*/
			NVARCHAR(MAX) = 3 OUT -- /* -- */ -- */,
            @p_2 /*@p2*/ dbo.[MyTvp] /*=NULL*/ READONLY
			) AS
			/**/
			BEGIN --
			--
			--blah */ etc
				/* ok -- */
			END;";

        private const string sqlRealFunction = @"
/*
	Refactored from [Utility].[SplitMax].
	See that function for implementation details.

	SELECT * FROM [ServerLog].[GetContainerList](N'a b c d e e');
	SELECT * FROM [ServerLog].[GetContainerList](N'a,b,c,d,e,e');
	SELECT DISTINCT N'-' + [Value] + N'-' FROM [ServerLog].[GetContainerList](N'a,b,c  ,d,e,e');
	SELECT DISTINCT N'-' + [Value] + N'-' FROM [ServerLog].[GetContainerList](N'a,b,c,  ,d,e,e');
	SELECT * FROM [ServerLog].[GetContainerList](N'a b c d e');
	SELECT * FROM [ServerLog].[GetContainerList](N'');
*/
CREATE FUNCTION [ServerLog].[GetContainerList]
(
	@Containers		NVARCHAR(MAX),
	@foo            AS UNIQUEIDENTIFIER = NULL,
)
RETURNS TABLE
WITH SCHEMABINDING
AS RETURN
(
	WITH

	-- Create virtual numbers table
	c1(N) AS (SELECT n FROM (VALUES (0), (1)) AS v(n)), -- 10^1 = 10
	-- Elided --
	WHERE
		[Identity] IS NOT NULL
);
		";

        private const string sqlMessyProcNoParen = @"
			CReaTE /*blah*/
				PROCedure            --etc
			abc.[def]
					/*
						xyz -- /*
					*/
			@p  --test--
			/* yada
			*/
			NVARCHAR(MAX) = 3 OUT -- /* -- */ -- */
			WITH ( OPTION -- opt
               /* AS */ RECOMPILE) AS
			/**/
			BEGIN --
			--
			--blah */ etc
				/* ok -- */
			END;";

        #endregion

        #region Methods

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(Tokenize_bad_comment))]
        public static void Tokenize_bad_comment()
        {
            var @params = SqlParamBlockParser.ParseFunction(sqlBadComment);

            Assert.True(@params.Count == 0);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(Tokenize_escaped_names))]
        public static void Tokenize_escaped_names()
        {
            var @params = SqlParamBlockParser.ParseFunction(sqlEscapedNames);

            Assert.True(@params.Count == 0);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(Tokenize_messy_proc_2_params))]
        public static void Tokenize_messy_proc_2_params()
        {
            var @params = SqlParamBlockParser.ParseProcedure(sqlMessyProcWithParen);

            Assert.Equal(2, @params.Count);

            // @p1
            var found = @params.TryGetValue("@p1", out var param);
            Assert.True(found);

            Assert.Equal(ParameterDirection.InputOutput, param.Direction);
            Assert.True(param.HasDefault);
            Assert.False(param.IsNullable);
            Assert.False(param.IsReadOnly);

            // @p_2
            found = @params.TryGetValue("@p_2", out param);
            Assert.True(found);

            Assert.Equal(ParameterDirection.Input, param.Direction);
            Assert.False(param.HasDefault);
            Assert.False(param.IsNullable);
            Assert.True(param.IsReadOnly);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(Tokenize_real_function_2_params))]
        public static void Tokenize_real_function_2_params()
        {
            var @params = SqlParamBlockParser.ParseFunction(sqlRealFunction);

            Assert.Equal(2, @params.Count);

            // @Containers
            var found = @params.TryGetValue("@Containers", out var param);
            Assert.True(found);

            Assert.Equal(ParameterDirection.Input, param.Direction);
            Assert.False(param.HasDefault);
            Assert.False(param.IsNullable);
            Assert.False(param.IsReadOnly);

            // @foo
            found = @params.TryGetValue("@foo", out param);
            Assert.True(found);

            Assert.Equal(ParameterDirection.Input, param.Direction);
            Assert.True(param.HasDefault);
            Assert.True(param.IsNullable);
            Assert.False(param.IsReadOnly);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(Tokenize_messy_proc_2_no_parenthesis))]
        public static void Tokenize_messy_proc_2_no_parenthesis()
        {
            var @params = SqlParamBlockParser.ParseProcedure(sqlMessyProcNoParen);

            Assert.Equal(1, @params.Count);

            // @p
            var found = @params.TryGetValue("@p", out var param);
            Assert.True(found);

            Assert.Equal(ParameterDirection.InputOutput, param.Direction);
            Assert.True(param.HasDefault);
            Assert.False(param.IsNullable);
            Assert.False(param.IsReadOnly);
        }

        #endregion
    }
}
