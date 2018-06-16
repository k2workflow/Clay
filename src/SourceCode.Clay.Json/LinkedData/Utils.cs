#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SourceCode.Clay.Json.LinkedData
{
    internal static class Utils
    {
        public static IEnumerable<KeyValuePair<string, JToken>> OrderLexicographically(
            this IEnumerable<KeyValuePair<string, JToken>> jObject)
            => jObject.OrderBy(x => x.Key, StringComparer.Ordinal);

        public static bool Is(this JToken jToken, JTokenType type)
        {
            if (type == JTokenType.Null)
                return jToken == null || jToken.Type == JTokenType.Null;
            if (jToken == null)
                return false;

            switch (jToken.Type)
            {
                case JTokenType.String:
                case JTokenType.Date:
                case JTokenType.Raw:
                case JTokenType.Bytes:
                case JTokenType.Guid:
                case JTokenType.Uri:
                case JTokenType.TimeSpan:
                case JTokenType.Null:
                    return type == JTokenType.String;
            }

            return jToken.Type == type;
        }

        public static string Resolve(string baseUri, string pathToResolve)
        {
            if (baseUri == null) return pathToResolve;
            if (string.IsNullOrWhiteSpace(pathToResolve)) return baseUri;

            if (Uri.TryCreate(baseUri, UriKind.Absolute, out var uri))
            {
                var builder = new UriBuilder(baseUri);

                if (builder.Scheme == "http" && builder.Port == 80)
                    builder.Port = -1;
                else if (builder.Scheme == "https" && builder.Port == 443)
                    builder.Port = -1;

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
            if (containerMapping.Is(JTokenType.Null)) return default;

            if (containerMapping.Is(JTokenType.String))
                return ParseSingleContainerMapping(containerMapping);
            else if (!containerMapping.Is(JTokenType.Array))
                throw new LinkedDataException(LinkedDataErrorCode.InvalidContainerMapping);

            var result = ContainerMappings.None;
            foreach (var raw in (JArray)containerMapping)
                result |= ParseSingleContainerMapping(raw);
            return result;
        }

        private static ContainerMappings ParseSingleContainerMapping(JToken containerMapping)
        {
            if (!containerMapping.Is(JTokenType.String))
                throw new LinkedDataException(LinkedDataErrorCode.InvalidContainerMapping);

            switch ((string)containerMapping)
            {
                case LinkedDataKeywords.Index: return ContainerMappings.Index;
                case LinkedDataKeywords.Language: return ContainerMappings.Language;
                case LinkedDataKeywords.List: return ContainerMappings.List;
                case LinkedDataKeywords.Set: return ContainerMappings.Set;
                default: throw new LinkedDataException(LinkedDataErrorCode.InvalidContainerMapping);
            }
        }
    }
}
