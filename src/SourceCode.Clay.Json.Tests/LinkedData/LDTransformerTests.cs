#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SourceCode.Clay.Json.LinkedData;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SourceCode.Clay.Json.Tests.LinkedData
{
#pragma warning disable xUnit1026 // Theory methods should use all of their parameters

    public static class LDTransformerTests
    {
        private const int CaseMin = 1;
        private const int CaseMax = 77;

        [Theory(DisplayName = nameof(LDTransformer_Expand))]
        [MemberData(nameof(Expand))]
        public static async Task LDTransformer_Expand(string name, string input, string output)
        {
            var inputObject = JToken.Parse(input);
            var outputObject = JToken.Parse(output);

            var sut = new LDTransformer();
            var actual = await sut.ExpandAsync(new Uri("http://example.org"), inputObject, CancellationToken.None);
            Assert.Equal(outputObject.ToString(Formatting.Indented), actual.ToString(Newtonsoft.Json.Formatting.Indented));
        }

        public static IEnumerable<object[]> Expand
        {
            get
            {
                for (var i = CaseMin; i <= CaseMax; i++)
                {
                    yield return new object[]
                    {
                        $"Expand {i}",
                        GetString(i, "expand", "in"),
                        GetString(i, "expand", "out")
                    };
                }
            }
        }

        private static string GetString(int caseId, string batch, string state)
        {
            var name = FormattableString.Invariant($"{batch}-{caseId:0000}-{state}.jsonld");
            var resource = FormattableString.Invariant($"{typeof(LDTransformerTests).Namespace}.Cases.{name}");
            using (var res = typeof(LDTransformerTests).Assembly.GetManifestResourceStream(resource))
            using (var reader = new StreamReader(res))
            {
                return reader.ReadToEnd();
            }
        }
    }

#pragma warning restore xUnit1026 // Theory methods should use all of their parameters
}
