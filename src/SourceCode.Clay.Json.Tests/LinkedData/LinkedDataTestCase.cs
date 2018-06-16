#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SourceCode.Clay.Json.LinkedData;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace SourceCode.Clay.Json.Tests.LinkedData
{
    public sealed class LinkedDataTestCase
    {
        public string Name { get; }
        public JToken Input { get; }
        public JToken Expected { get; }
        public LinkedDataOptions Options { get; }

        public LinkedDataTestCase(string name, JToken input, JToken expected, LinkedDataOptions options)
        {
            Name = name;
            Input = input;
            Expected = expected;
            Options = options;
        }

        public override string ToString() => Name;

        public static async ValueTask<IEnumerable<ValueTask<LinkedDataTestCase>>> ReadAsync(string resourceName)
        {
            var manifest = await ReadResourceAsync(resourceName);
            return ReadAsync((JObject)manifest);
        }

        private static IEnumerable<ValueTask<LinkedDataTestCase>> ReadAsync(JObject manifest)
        {
            var baseIri = (string)manifest["baseIri"];
            var sequence = (JArray)manifest["sequence"];
            for (var i = 0; i < sequence.Count; i++)
            {
                var caseJson = sequence[i];
                var name = baseIri + (string)caseJson["@id"];
                var input = (string)caseJson["input"];
                var expect = (string)caseJson["expect"];
                var option = (JObject)caseJson["option"];
                yield return ReadAsync(baseIri, name, input, expect, option);
            }
        }

        private static async ValueTask<LinkedDataTestCase> ReadAsync(string baseIri, string name, string input, string expect, JObject option)
        {
            baseIri += input;
            var inputDoc = await ReadResourceAsync(input);
            var expectDoc = await ReadResourceAsync(expect);
            var options = new LinkedDataOptions(baseIri);

            if (option != null)
            {
                if (option.TryGetValue("base", out var baseToken))
                {
                    options = new LinkedDataOptions((string)baseToken);
                }
                if (option.TryGetValue("expandContext", out var expandContextToken))
                {
                    var expandObject = await ReadResourceAsync((string)expandContextToken);
                    options = await options.WithContextAsync(expandObject);
                }
            }

            return new LinkedDataTestCase(name, inputDoc, expectDoc, options);
        }

        private static async ValueTask<JToken> ReadResourceAsync(string resource)
        {
            resource = typeof(LinkedDataTestCase).Namespace + ".Cases." + resource;
            using (var res = typeof(LinkedDataTestCase).Assembly.GetManifestResourceStream(resource))
            using (var reader = new StreamReader(res))
            using (var jreader = new JsonTextReader(reader))
            {
                return await JToken.ReadFromAsync(jreader);
            }
        }
    }
}
