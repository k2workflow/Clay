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

namespace SourceCode.Clay.Json.Tests.LinkedData
{
    public sealed class LinkedDataTestOptions : LinkedDataOptions
    {
        private readonly Dictionary<string, JToken> _contexts = 
            new Dictionary<string, JToken>(StringComparer.OrdinalIgnoreCase);

        public LinkedDataTestOptions(string @base = null)
            : base(@base)
        {
        }

        private LinkedDataTestOptions(string @base = null, LinkedDataContext expandContext = default) 
            : base(@base, expandContext)
        {
        }

        public void AddContext(string iri, JToken context)
        {
            _contexts.Add(iri, context);
        }

        public override async ValueTask<LinkedDataOptions> WithContextAsync(JToken localContext, CancellationToken cancellationToken = default)
        {
            var context = await CreateContextAsync(localContext, cancellationToken);
            return new LinkedDataTestOptions(Base, context);
        }

        public override ValueTask<JToken> GetContextAsync(string iri, CancellationToken cancellationToken)
        {
            if (_contexts.TryGetValue(iri, out var result)) return new ValueTask<JToken>(result);
            throw new FileNotFoundException();
        }
    }

    public sealed class LinkedDataTestCase
    {
        public string Id { get; }
        public string Name { get; }
        public JToken Input { get; }
        public JToken Expected { get; }
        public LinkedDataOptions Options { get; }

        public LinkedDataTestCase(string id, string name, JToken input, JToken expected, LinkedDataOptions options)
        {
            Id = id;
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
                var id = baseIri + (string)caseJson["@id"];
                var name = (string)caseJson["name"];
                var input = (string)caseJson["input"];
                var expect = (string)caseJson["expect"];
                var option = (JObject)caseJson["option"];
                var contexts = (JObject)caseJson["contexts"];
                yield return ReadAsync(baseIri, id, name, input, expect, option, contexts);
            }
        }

        private static async ValueTask<LinkedDataTestCase> ReadAsync(string baseIri, string id, string name, string input, string expect, JObject option, JObject contexts)
        {
            baseIri += input;
            var inputDoc = await ReadResourceAsync(input);
            var expectDoc = await ReadResourceAsync(expect);
            var options = new LinkedDataTestOptions(baseIri);

            if (option != null)
            {
                if (option.TryGetValue("base", out var baseToken))
                {
                    options = new LinkedDataTestOptions((string)baseToken);
                }
                if (option.TryGetValue("expandContext", out var expandContextToken))
                {
                    var expandObject = await ReadResourceAsync((string)expandContextToken);
                    options = (LinkedDataTestOptions)await options.WithContextAsync(expandObject);
                }
            }

            if (contexts != null)
            {
                foreach (var (iri, contextValue) in contexts)
                {
                    var wrappedContext = new JObject()
                    {
                        [LinkedDataKeywords.Context] = contextValue
                    };
                    options.AddContext(iri, wrappedContext);
                }
            }

            return new LinkedDataTestCase(id, name, inputDoc, expectDoc, options);
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
