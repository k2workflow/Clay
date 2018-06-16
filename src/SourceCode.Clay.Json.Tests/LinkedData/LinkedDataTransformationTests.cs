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

    public static class LinkedDataTransformationTests
    {
        public sealed class Case
        {
            public int Id { get; }
            public string Identifier { get; }
            public string Input { get; }
            public string Expected { get; }
            public string InputName { get; }
            public string OutputName { get; }

            public Case(int id, string identifier, string inputName, string input, string outputName, string expected)
            {
                Id = id;
                Identifier = identifier;
                Input = input;
                Expected = expected;
                InputName = inputName;
                OutputName = outputName;
            }

            public override string ToString() => Identifier;
        }

        private const int ExpandMin = 1;
        private const int ExpandMax = 77;

        [Theory(DisplayName = nameof(LinkedDataTransformation_Expand))]
        [MemberData(nameof(Expand))]
        public static async Task LinkedDataTransformation_Expand(LinkedDataTestCase testCase)
        {
            var actualObject = await LinkedDataTransformation.ExpandAsync(
                testCase.Input,
                testCase.Options,
                CancellationToken.None);

            if (!JToken.DeepEquals(testCase.Expected, actualObject))
            {
                var expected = testCase.Expected.ToString(Formatting.Indented);
                var actual = actualObject.ToString(Formatting.Indented);
                Assert.Equal(expected, actual);
            }
        }

        public static IEnumerable<object[]> Expand
        {
            get
            {
                foreach (var asyncItem in LinkedDataTestCase.ReadAsync("expand-manifest.jsonld").Result)
                {
                    var item = asyncItem.Result;
                    yield return new object[] { item };
                }
            }
        }
    }

#pragma warning restore xUnit1026 // Theory methods should use all of their parameters
}
