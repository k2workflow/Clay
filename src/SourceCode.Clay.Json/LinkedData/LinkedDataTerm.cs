#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace SourceCode.Clay.Json.LinkedData
{
    public readonly struct LinkedDataTerm
    {
        private static readonly HashSet<ContainerMappings> ValidContainerMappings = new HashSet<ContainerMappings>()
        {
            // which must be either @graph, @id, @index, @language, @list, @set, or @type
            ContainerMappings.Graph,
            ContainerMappings.Id,
            ContainerMappings.Index,
            ContainerMappings.Language,
            ContainerMappings.List,
            ContainerMappings.Set,
            ContainerMappings.Type,

            // an array containing @graph and either @id or @index optionally including @set
            ContainerMappings.Graph | ContainerMappings.Id,
            ContainerMappings.Graph | ContainerMappings.Id | ContainerMappings.Set,
            ContainerMappings.Graph | ContainerMappings.Index,
            ContainerMappings.Graph | ContainerMappings.Index | ContainerMappings.Set,

            // TODO: This is probably wrong
            // an array containing a combination of @set and any of @index, @id, @type, @language in any order
            ContainerMappings.Set | ContainerMappings.Index | ContainerMappings.Id,
            ContainerMappings.Set | ContainerMappings.Index | ContainerMappings.Id | ContainerMappings.Type,
            ContainerMappings.Set | ContainerMappings.Index | ContainerMappings.Id | ContainerMappings.Type | ContainerMappings.Language,

            ContainerMappings.Set | ContainerMappings.Id,
            ContainerMappings.Set | ContainerMappings.Id | ContainerMappings.Type,
            ContainerMappings.Set | ContainerMappings.Id | ContainerMappings.Type | ContainerMappings.Language,

            ContainerMappings.Set | ContainerMappings.Index,
            ContainerMappings.Set | ContainerMappings.Index | ContainerMappings.Type,
            ContainerMappings.Set | ContainerMappings.Index | ContainerMappings.Type | ContainerMappings.Language,

            ContainerMappings.Set | ContainerMappings.Index | ContainerMappings.Id,
            ContainerMappings.Set | ContainerMappings.Index | ContainerMappings.Id,
            ContainerMappings.Set | ContainerMappings.Index | ContainerMappings.Id | ContainerMappings.Language,

            ContainerMappings.Set | ContainerMappings.Index | ContainerMappings.Id,
            ContainerMappings.Set | ContainerMappings.Index | ContainerMappings.Id | ContainerMappings.Type,
            ContainerMappings.Set | ContainerMappings.Index | ContainerMappings.Id | ContainerMappings.Type,
        };

        public string Term { get; }
        public LinkedDataTermOptions Options { get; }
        public string TypeMapping { get; }
        public string IriMapping { get; }
        public ContainerMappings ContainerMappings { get; }
        public string Language { get; }

        public LinkedDataTerm(string term, LinkedDataTermOptions options, string typeMapping, string iriMapping, ContainerMappings containerMappings, string language)
        {
            Term = term;
            Options = options;
            TypeMapping = typeMapping;
            IriMapping = iriMapping;
            ContainerMappings = containerMappings;
            Language = language;
        }

        public static IEnumerable<LinkedDataTerm> Parse(LinkedDataContext activeContext, JObject localContext)
        {
            var defined = new Dictionary<string, bool>(StringComparer.Ordinal);
            foreach (var item in localContext)
            {
                if (item.Key == LDKeywords.Base || item.Key == LDKeywords.Vocab ||
                    item.Key == LDKeywords.Language || item.Key == LDKeywords.Version)
                    continue;

                if (defined.TryGetValue(item.Key, out var created))
                {
                    if (created) continue;
                    throw new LinkedDataException(LinkedDataErrorCode.CyclicIriMapping);
                }

                foreach (var result in Parse(activeContext, localContext, item, defined))
                    yield return result;
            }
        }

        internal static IEnumerable<LinkedDataTerm> Parse(LinkedDataContext activeContext, JObject localContext, KeyValuePair<string, JToken> termPair, Dictionary<string, bool> defined)
        {
            var (term, value) = termPair;
            defined[term] = false;

            // 3) Since keywords cannot be overridden, term must not be a keyword. Otherwise, a
            //    keyword redefinition error has been detected and processing is aborted.
            if (LDKeywords.IsKeyword(term))
                throw new LinkedDataException(LinkedDataErrorCode.KeywordRedefinition);

            // 6) If value is null or value is a dictionary containing the key-value pair @id-null,
            //    set the term definition in active context to null, set the value associated with
            //    defined's key term to true, and return.
            if (value.Type == JTokenType.Null ||
                (value is JObject o && o.TryGetValue(LDKeywords.Id, out var idToken) && idToken.Type == JTokenType.Null))
                yield break;

            var options = LinkedDataTermOptions.None;
            string typeMapping = null;
            string iriMapping = null;
            var containerMappings = ContainerMappings.None;
            string language = null;

            // 7) Otherwise, if value is a string, convert it to a dictionary consisting of a
            //    single member whose key is @id and whose value is value. Set simple term to true.
            if (value.Type == JTokenType.String)
            {
                value = new JObject() { [LDKeywords.Id] = (string)value };
                options |= LinkedDataTermOptions.SimpleTerm;
            }

            if (value.Type != JTokenType.Object)
                throw new LinkedDataException(LinkedDataErrorCode.InvalidTermDefinition);

            var valueObject = (JObject)value;

            // 10) If value contains the key @type:
            if (valueObject.TryGetValue(LDKeywords.Type, out var typeToken))
            {
                if (typeToken.Type != JTokenType.String)
                    throw new LinkedDataException(LinkedDataErrorCode.InvalidTypeMapping);
                typeMapping = Utils.ExpandIri(activeContext, (string)value, localContext: localContext, vocab: true, defined: defined);
            }

            // 11) If value contains the key @reverse:
            if (valueObject.TryGetValue(LDKeywords.Reverse, out var reverseToken))
            {
                if (valueObject.ContainsKey(LDKeywords.Id) || valueObject.ContainsKey(LDKeywords.Nest))
                    throw new LinkedDataException(LinkedDataErrorCode.InvalidReverseProperty);
                if (reverseToken.Type != JTokenType.String)
                    throw new LinkedDataException(LinkedDataErrorCode.InvalidIriMapping);

                iriMapping = Utils.ExpandIri(activeContext, (string)reverseToken, localContext: localContext, vocab: true, defined: defined);
                if (iriMapping.Contains(':', StringComparison.Ordinal))
                    throw new LinkedDataException(LinkedDataErrorCode.InvalidIriMapping);

                if (valueObject.TryGetValue(LDKeywords.Container, out var reverseContainerToken))
                {
                    if (reverseContainerToken.Type != JTokenType.String && reverseContainerToken.Type != JTokenType.Null)
                        throw new LinkedDataException(LinkedDataErrorCode.InvalidReverseProperty);

                    containerMappings = Utils.ParseContainerMapping((string)reverseContainerToken);
                    if (containerMappings != ContainerMappings.Set && containerMappings != ContainerMappings.Index &&
                        containerMappings != ContainerMappings.None)
                        throw new LinkedDataException(LinkedDataErrorCode.InvalidReverseProperty);
                }

                options |= LinkedDataTermOptions.ReverseProperty;
                defined[term] = true;
                yield return new LinkedDataTerm(term, options, typeMapping, iriMapping, containerMappings, language);
                yield break;
            }

            // 13) If value contains the key @id and its value does not equal term:
            if (valueObject.TryGetValue(LDKeywords.Id, out idToken))
            {
                if (idToken.Type == JTokenType.String)
                    throw new LinkedDataException(LinkedDataErrorCode.InvalidIriMapping);

                iriMapping = Utils.ExpandIri(activeContext, (string)idToken, localContext: localContext, vocab: true, defined: defined);
                if (!LDKeywords.IsKeyword(iriMapping) && !Uri.TryCreate(iriMapping, UriKind.Absolute, out var tmp))
                    throw new LinkedDataException(LinkedDataErrorCode.InvalidIriMapping);
                if (iriMapping == LDKeywords.Context)
                    throw new LinkedDataException(LinkedDataErrorCode.InvalidKeywordAlias);

                if (!iriMapping.Contains(':', StringComparison.Ordinal))
                    options |= LinkedDataTermOptions.SimpleTerm;

                if (Utils.UrlGenDelims.Contains(iriMapping[iriMapping.Length - 1]))
                    options |= LinkedDataTermOptions.Prefix;
            }
            // 14) Otherwise if the term contains a colon (:):
            else if (term.Contains(':', StringComparison.OrdinalIgnoreCase))
            {
                var index = term.IndexOf(':', StringComparison.Ordinal);
                var prefix = term.Substring(0, index);

                if (index > 0 && localContext.TryGetValue(prefix, out var prefixTokenDeclaration))
                {
                    foreach (var item in Parse(activeContext, localContext, new KeyValuePair<string, JToken>(prefix, prefixTokenDeclaration), defined))
                        yield return item;
                }

                if (activeContext.TryGetTerm(prefix, out var existingTerm))
                    iriMapping = existingTerm.IriMapping + term.Substring(index + 1);
                else
                    iriMapping = term;
            }
            // 15) Otherwise, if active context has a vocabulary mapping
            else if (activeContext.Vocabulary != null)
            {
                iriMapping = activeContext.Vocabulary + term;
            }
            else
                throw new LinkedDataException(LinkedDataErrorCode.InvalidIriMapping);

            // 16) If value contains the key @container:
            if (valueObject.TryGetValue(LDKeywords.Container, out var containerToken))
            {
                containerMappings = Utils.ParseContainerMapping(containerToken);
                if (!ValidContainerMappings.Contains(containerMappings))
                    throw new LinkedDataException(LinkedDataErrorCode.InvalidContainerMapping);
            }

            // 17) If value contains the key @context:
            if (valueObject.TryGetValue(LDKeywords.Context, out var contextToken))
            {
                throw new NotImplementedException();
            }

            // 18) If value contains the key @language and does not contain the key @type:
            if (valueObject.TryGetValue(LDKeywords.Language, out var languageToken) &&
                !valueObject.ContainsKey(LDKeywords.Type))
            {
                if (languageToken.Type == JTokenType.String)
                {
#                   pragma warning disable CA1308 // Normalize strings to uppercase
                    language = ((string)languageToken).ToLowerInvariant();
#                   pragma warning restore CA1308 // Normalize strings to uppercase
                }
                else if (languageToken.Type != JTokenType.Null)
                    throw new LinkedDataException(LinkedDataErrorCode.InvalidLanguageMapping);
            }

            // 19) If value contains the key @nest:
            if (valueObject.TryGetValue(LDKeywords.Nest, out var nestToken))
            {
                throw new NotImplementedException();
            }

            // 20) If value contains the key @prefix:
            if (valueObject.TryGetValue(LDKeywords.Prefix, out var prefixToken))
            {
                if (prefixToken.Type != JTokenType.Boolean)
                    throw new LinkedDataException(LinkedDataErrorCode.InvalidPrefixValue);

                if ((bool)prefixToken)
                    options |= LinkedDataTermOptions.Prefix;
            }

            defined[term] = true;
            yield return new LinkedDataTerm(term, options, typeMapping, iriMapping, containerMappings, language);
        }
    }
}
