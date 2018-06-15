#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace SourceCode.Clay.Json.LinkedData
{
    internal sealed class Utils
    {
        public static HashSet<char> UrlGenDelims { get; } = new HashSet<char>()
        {
            ':', '/', '?', '#', '[', ']', '@'
        };

        public static string Resolve(string baseUri, string pathToResolve)
        {
            if (baseUri == null) return pathToResolve;
            if (string.IsNullOrWhiteSpace(pathToResolve)) return baseUri;

            if (Uri.TryCreate(baseUri, UriKind.Absolute, out var uri))
            {
                var builder = new UriBuilder(baseUri);

                if (pathToResolve.StartsWith("?", StringComparison.Ordinal))
                {
                    builder.Fragment = null;
                    builder.Query = pathToResolve;
                    return builder.ToString();
                }

                return new Uri(uri, pathToResolve).ToString();
            }

            return pathToResolve;
        }

        public static ContainerMappings ParseContainerMapping(JToken containerMapping)
        {
            if (containerMapping.Type == JTokenType.Null) return default;

            if (containerMapping.Type == JTokenType.String)
                return ParseSingleContainerMapping(containerMapping);
            else if (containerMapping.Type != JTokenType.Array)
                throw new LinkedDataException(LinkedDataErrorCode.InvalidContainerMapping);

            var result = ContainerMappings.None;
            foreach (var raw in (JArray)containerMapping)
                result |= ParseSingleContainerMapping(raw);
            return result;
        }

        private static ContainerMappings ParseSingleContainerMapping(JToken containerMapping)
        {
            if (containerMapping.Type == JTokenType.String)
                throw new LinkedDataException(LinkedDataErrorCode.InvalidContainerMapping);

            switch ((string)containerMapping)
            {
                case LDKeywords.Graph: return ContainerMappings.Graph;
                case LDKeywords.Id: return ContainerMappings.Id;
                case LDKeywords.Index: return ContainerMappings.Index;
                case LDKeywords.Language: return ContainerMappings.Language;
                case LDKeywords.List: return ContainerMappings.List;
                case LDKeywords.Set: return ContainerMappings.Set;
                case LDKeywords.Type: return ContainerMappings.Type;
                default: throw new LinkedDataException(LinkedDataErrorCode.InvalidContainerMapping);
            }
        }
    }
}
