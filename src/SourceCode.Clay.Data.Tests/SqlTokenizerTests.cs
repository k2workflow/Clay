﻿using System;
using System.Data;
using Xunit;

namespace SourceCode.Clay.Data.SqlParser.Tests
{
    public static class SqlTokenizerTests
    {
        //     // [abc]]def]
        //     // [abc[]]def]
        //     private const string sql0 = @"
        //CReaTE /*blah*/
        //	FunCTIOn            --etc
        //[abc]]def].[abc[]]def]
        //( --test--
        //)AS RETURNS
        //    inT
        //BEGIN RETURN 1;
        //END;";

        //     [Trait("Type", "Unit")]
        //     [Fact(DisplayName = nameof(Tokenize_escaped_names))]
        //     public static void Tokenize_escaped_names()
        //     {
        //         var @params = SqlParamBlockParser.ParseFunction(sql0, out var errors);

        //         Assert.True(@params == null || @params.Count == 0);
        //     }

        private const string sql1 = @"
			CReaTE /*blah*/
				PROCedure       --etc
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

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(Tokenize_messy_proc_2_params))]
        public static void Tokenize_messy_proc_2_params()
        {
            Console.Out.WriteLine(sql1);
            var tokens = SqlTokenizer.Tokenize(sql1, false);
            Console.Out.WriteLine($"Tokens = {tokens.Count}:");
            foreach (var token in tokens)
                Console.Out.Write($"{token}, ");

            var @params = SqlParamBlockParser.ParseProcedure(sql1, out var errors);

            if (errors != null && errors.Count > 0)
            {
                var equal = StringComparer.OrdinalIgnoreCase.Equals("PROCEDURE", "PROCedure");
                throw new Exception($"{equal}=" + errors[0]);
            }

            Assert.Equal(2, @params.Count);

            // @p1
            var found = @params.TryGetValue("@p1", out var param);
            Assert.True(found);

            Assert.Equal(ParameterDirection.InputOutput, param.Direction);
            Assert.True(param.HasDefault);
            Assert.False(param.IsNullable);
            Assert.False(param.IsReadOnly);

            // @p2
            found = @params.TryGetValue("@p_2", out param);
            Assert.True(found);

            Assert.Equal(ParameterDirection.Input, param.Direction);
            Assert.False(param.HasDefault);
            Assert.False(param.IsNullable);
            Assert.True(param.IsReadOnly);
        }

        //        private const string sql2 = @"
        ///*
        //	Refactored from [Utility].[SplitMax].
        //	See that function for implementation details.

        //	SELECT * FROM [ServerLog].[GetContainerList](N'a b c d e e');
        //	SELECT * FROM [ServerLog].[GetContainerList](N'a,b,c,d,e,e');
        //	SELECT DISTINCT N'-' + [Value] + N'-' FROM [ServerLog].[GetContainerList](N'a,b,c  ,d,e,e');
        //	SELECT DISTINCT N'-' + [Value] + N'-' FROM [ServerLog].[GetContainerList](N'a,b,c,  ,d,e,e');
        //	SELECT * FROM [ServerLog].[GetContainerList](N'a b c d e');
        //	SELECT * FROM [ServerLog].[GetContainerList](N'');
        //*/
        //CREATE FUNCTION [ServerLog].[GetContainerList]
        //(
        //	@Containers		NVARCHAR(MAX),
        //	@foo            AS UNIQUEIDENTIFIER = NULL,
        //)
        //RETURNS TABLE
        //WITH SCHEMABINDING
        //AS RETURN
        //(
        //	WITH

        //	-- Create virtual numbers table
        //	c1(N) AS (SELECT n FROM (VALUES (0), (1)) AS v(n)), -- 10^1 = 10
        //	-- Elided --
        //	WHERE
        //		[Identity] IS NOT NULL
        //);
        //		";

        //        [Trait("Type", "Unit")]
        //        [Fact(DisplayName = nameof(Tokenize_real_function_2_params))]
        //        public static void Tokenize_real_function_2_params()
        //        {
        //            var @params = SqlParamBlockParser.ParseFunction(sql2, out var errors);

        //            Assert.Equal(2, @params.Count);

        //            // @Containers
        //            var found = @params.TryGetValue("@Containers", out var param);
        //            Assert.True(found);

        //            Assert.Equal(ParameterDirection.Input, param.Direction);
        //            Assert.False(param.HasDefault);
        //            Assert.False(param.IsNullable);
        //            Assert.False(param.IsReadOnly);

        //            // @foo
        //            found = @params.TryGetValue("@foo", out param);
        //            Assert.True(found);

        //            Assert.Equal(ParameterDirection.Input, param.Direction);
        //            Assert.True(param.HasDefault);
        //            Assert.True(param.IsNullable);
        //            Assert.False(param.IsReadOnly);
        //        }

        //        private const string sql3 = @"
        //			CReaTE /*blah*/
        //				PROCedure            --etc
        //			abc.[def]
        //					/*
        //						xyz -- /*
        //					*/
        //			@p  --test--
        //			/* yada
        //			*/
        //			NVARCHAR(MAX) = 3 OUT -- /* -- */ -- */
        //			WITH ( OPTION -- opt
        //               /* AS */ RECOMPILE) AS
        //			/**/
        //			BEGIN --
        //			--
        //			--blah */ etc
        //				/* ok -- */
        //			END;";

        //        //[Trait("Type", "Unit")]
        //        //[Fact(DisplayName = nameof(Tokenize_messy_proc_2_no_parenthesis))]
        //        public static void Tokenize_messy_proc_2_no_parenthesis()
        //        {
        //            var @params = SqlParamBlockParser.ParseProcedure(sql3, out var errors);

        //            if (errors != null && errors.Count > 0)
        //                throw new Exception(errors[0]);

        //            Assert.Equal(1, @params.Count);

        //            // @p
        //            var found = @params.TryGetValue("@p", out var param);
        //            Assert.True(found);

        //            Assert.Equal(ParameterDirection.InputOutput, param.Direction);
        //            Assert.True(param.HasDefault);
        //            Assert.False(param.IsNullable);
        //            Assert.False(param.IsReadOnly);
        //        }
    }
}
