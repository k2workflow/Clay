#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace SourceCode.Clay.Json.LinkedData
{
    internal readonly struct LinkedDataTerm
    {
        private static readonly HashSet<ContainerMappings> ValidContainerMappings = new HashSet<ContainerMappings>()
        {
            // which must be either @list, @set, @index, or @language
            ContainerMappings.Index,
            ContainerMappings.Language,
            ContainerMappings.List,
            ContainerMappings.Set
        };

        public string Term { get; }
        public LinkedDataTermOptions Options { get; }
        public string TypeMapping { get; }
        public string IriMapping { get; }
        public ContainerMappings ContainerMappings { get; }
        public string Language { get; }

        public LinkedDataTerm(
            string term,
            LinkedDataTermOptions options,
            string typeMapping,
            string iriMapping,
            ContainerMappings containerMappings,
            string language)
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
                if (item.Key == LinkedDataKeywords.Base || item.Key == LinkedDataKeywords.Vocab ||
                    item.Key == LinkedDataKeywords.Language)
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

        internal static IEnumerable<LinkedDataTerm> Parse(
            LinkedDataContext activeContext,
            JObject localContext,
            KeyValuePair<string, JToken> termPair,
            Dictionary<string, bool> defined)
        {
            var (term, value) = termPair;
            defined[term] = false;

            // 3) Since keywords cannot be overridden, term must not be a keyword. Otherwise, a keyword redefinition
            //    error has been detected and processing is aborted.
            if (LinkedDataKeywords.IsKeyword(term))
                throw new LinkedDataException(LinkedDataErrorCode.KeywordRedefinition);

            // 6) If value is null or value is a dictionary containing the key-value pair @id-null, set the term
            //    definition in active context to null, set the value associated with defined's key term to true, and
            //    return.
            if (value.Is(JTokenType.Null) ||
                (value is JObject o && o.TryGetValue(LinkedDataKeywords.Id, out var idToken) &&
                    idToken.Is(JTokenType.Null)))
            {
                defined[term] = true;
                yield return new LinkedDataTerm(
                    term,
                    LinkedDataTermOptions.ClearTerm,
                    null,
                    null,
                    ContainerMappings.None,
                    null);
                yield break;
            }

            var options = LinkedDataTermOptions.None;
            string typeMapping = null;
            string iriMapping = null;
            var containerMappings = ContainerMappings.None;
            string language = null;

            // 7) Otherwise, if value is a string, convert it to a dictionary consisting of a single member whose key
            //    is @id and whose value is value. Set simple term to true.
            if (value.Is(JTokenType.String))
            {
                value = new JObject() { [LinkedDataKeywords.Id] = (string)value };
                options |= LinkedDataTermOptions.SimpleTerm;
            }

            if (!value.Is(JTokenType.Object))
                throw new LinkedDataException(LinkedDataErrorCode.InvalidTermDefinition);

            // 8) Otherwise, value must be a JSON object, if not, an invalid term definition error has been detected
            //    and processing is aborted.
            var valueObject = (JObject)value;

            // 10) If value contains the key @type:
            if (valueObject.TryGetValue(LinkedDataKeywords.Type, out var typeToken))
            {
                if (!typeToken.Is(JTokenType.String))
                    throw new LinkedDataException(LinkedDataErrorCode.InvalidTypeMapping);
                typeMapping = LinkedDataTransformation.ExpandIri(
                    activeContext,
                    (string)typeToken,
                    localContext: localContext,
                    vocab: true,
                    defined: defined);
                // If the expanded type is neither @id, nor @vocab, nor an absolute IRI, an invalid type mapping error
                // has been detected and processing is aborted.
                if (typeMapping != LinkedDataKeywords.Id && typeMapping != LinkedDataKeywords.Vocab &&
                    !Uri.IsWellFormedUriString(typeMapping, UriKind.Absolute))
                    throw new LinkedDataException(LinkedDataErrorCode.InvalidTypeMapping);
            }

            // 11) If value contains the key @reverse:
            if (valueObject.TryGetValue(LinkedDataKeywords.Reverse, out var reverseToken))
            {
                // 11.1) If value contains an @id, member, an invalid reverse property error has been detected and
                //       processing is aborted.
                if (valueObject.ContainsKey(LinkedDataKeywords.Id))
                    throw new LinkedDataException(LinkedDataErrorCode.InvalidReverseProperty);
                // 11.2) If the value associated with the @reverse key is not a string, an invalid IRI mapping error
                //       has been detected and processing is aborted.
                if (!reverseToken.Is(JTokenType.String))
                    throw new LinkedDataException(LinkedDataErrorCode.InvalidIriMapping);

                // 11.3) Otherwise, set the IRI mapping of definition to the result of using the IRI Expansion
                //       algorithm,
                iriMapping = LinkedDataTransformation.ExpandIri(
                    activeContext,
                    (string)reverseToken,
                    localContext: localContext,
                    vocab: true,
                    defined: defined);
                // If the result is neither an absolute IRI nor a blank node identifier, i.e., it contains no colon
                // (:), an invalid IRI mapping error has been detected and processing is aborted.
                if (!iriMapping.Contains(':', StringComparison.Ordinal))
                    throw new LinkedDataException(LinkedDataErrorCode.InvalidIriMapping);

                // 11.4) If value contains an @container member, set the container mapping of definition to its value
                if (valueObject.TryGetValue(LinkedDataKeywords.Container, out var reverseContainerToken))
                {
                    if (!reverseContainerToken.Is(JTokenType.String))
                        throw new LinkedDataException(LinkedDataErrorCode.InvalidReverseProperty);

                    containerMappings = Utils.ParseContainerMapping((string)reverseContainerToken);

                    // if its value is neither @set, nor @index, nor null, an invalid reverse property error has been
                    // detected
                    if (containerMappings != ContainerMappings.Set && containerMappings != ContainerMappings.Index &&
                        containerMappings != ContainerMappings.None)
                        throw new LinkedDataException(LinkedDataErrorCode.InvalidReverseProperty);
                }

                // 11.5) Set the reverse property flag of definition to true.
                options |= LinkedDataTermOptions.ReverseProperty;
                defined[term] = true;
                yield return new LinkedDataTerm(term, options, typeMapping, iriMapping, containerMappings, language);
                yield break;
            }

            // 13) If value contains the key @id and its value does not equal term:
            if (valueObject.TryGetValue(LinkedDataKeywords.Id, out idToken) &&
                (!idToken.Is(JTokenType.String) || (string)idToken != term))
            {
                if (!idToken.Is(JTokenType.String))
                    throw new LinkedDataException(LinkedDataErrorCode.InvalidIriMapping);

                // 13.2) Otherwise, set the IRI mapping of definition to the result of using the IRI Expansion
                //       algorithm,
                iriMapping = LinkedDataTransformation.ExpandIri(
                    activeContext,
                    (string)idToken,
                    localContext: localContext,
                    vocab: true,
                    defined: defined);
                if (!LinkedDataKeywords.IsKeyword(iriMapping) &&
                    !Uri.IsWellFormedUriString(iriMapping, UriKind.Absolute) &&
                    !iriMapping.StartsWith("_:", StringComparison.Ordinal))
                    throw new LinkedDataException(LinkedDataErrorCode.InvalidIriMapping);
                if (iriMapping == LinkedDataKeywords.Context)
                    throw new LinkedDataException(LinkedDataErrorCode.InvalidKeywordAlias);
            }
            // 14) Otherwise if the term contains a colon (:):
            else if (term.Contains(':', StringComparison.Ordinal))
            {
                var index = term.IndexOf(':', StringComparison.Ordinal);
                var prefix = term.Substring(0, index);

                // 14.1) If term is a compact IRI with a prefix that is a key in local context a dependency has been
                //       found.
                if (index > 0 && localContext.TryGetValue(prefix, out var prefixTokenDeclaration))
                {
                    // Use this algorithm recursively
                    foreach (var item in Parse(
                        activeContext,
                        localContext,
                        new KeyValuePair<string, JToken>(prefix, prefixTokenDeclaration),
                        defined))
                        yield return item;
                }

                // 14.2) If term's prefix has a term definition in active context, set the IRI mapping of definition to
                //       the result of concatenating the value associated with the prefix's IRI mapping and the term's
                //       suffix.
                if (activeContext.TryGetTerm(prefix, out var existingTerm))
                    iriMapping = existingTerm.IriMapping + term.Substring(index + 1);
                // 14.3) Otherwise, term is an absolute IRI or blank node identifier. Set the IRI mapping of definition
                //       to term.
                else
                    iriMapping = term;
            }
            // 15) Otherwise, if active context has a vocabulary mapping, the IRI mapping of definition is set to the
            //     result of concatenating the value associated with the vocabulary mapping and term.
            else if (activeContext.Vocabulary != null)
            {
                iriMapping = activeContext.Vocabulary + term;
            }
            // If it does not have a vocabulary mapping, an invalid IRI mapping error been detected and processing is
            // aborted.
            else
                throw new LinkedDataException(LinkedDataErrorCode.InvalidIriMapping);

            // 16) If value contains the key @container:
            if (valueObject.TryGetValue(LinkedDataKeywords.Container, out var containerToken))
            {
                containerMappings = Utils.ParseContainerMapping(containerToken);
                if (!ValidContainerMappings.Contains(containerMappings))
                    throw new LinkedDataException(LinkedDataErrorCode.InvalidContainerMapping);
            }

            // 17) If value contains the key @language and does not contain the key @type:
            if (valueObject.TryGetValue(LinkedDataKeywords.Language, out var languageToken) &&
                !valueObject.ContainsKey(LinkedDataKeywords.Type))
            {
                if (languageToken.Is(JTokenType.Null))
                    options |= LinkedDataTermOptions.ClearLanguage;
                else if (languageToken.Is(JTokenType.String))
                {
#                   pragma warning disable CA1308 // Normalize strings to uppercase
                    language = ((string)languageToken).ToLowerInvariant();
#                   pragma warning restore CA1308 // Normalize strings to uppercase
                }
                else
                    throw new LinkedDataException(LinkedDataErrorCode.InvalidLanguageMapping);
            }

            defined[term] = true;
            yield return new LinkedDataTerm(term, options, typeMapping, iriMapping, containerMappings, language);
        }
    }
}
