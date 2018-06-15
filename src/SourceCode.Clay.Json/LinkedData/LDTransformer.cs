#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SourceCode.Clay.Json.LinkedData
{
    public partial class LDTransformer
    {
        // https://json-ld.org/spec/FCGS/json-ld-api/20180607/#expansion-algorithm

        public async ValueTask<JArray> ExpandAsync(Uri baseIri, JToken jToken, CancellationToken cancellationToken)
        {
            var context = new Context(baseIri);
            var result = await ExpandAsync(context, jToken, cancellationToken).ConfigureAwait(false);
            return result;
        }

        protected async ValueTask<JArray> ExpandAsync(Context context, JToken jToken, CancellationToken cancellationToken)
        {
            var result = await ExpandToSingleValueAsync(context, jToken, cancellationToken).ConfigureAwait(false);

            if (result == null) return new JArray();
            if (result.Type == JTokenType.Array) return (JArray)result;
            return new JArray(result);
        }

        protected async Task<JToken> ExpandToSingleValueAsync(Context context, JToken jToken, CancellationToken cancellationToken)
        {
            JToken result;
            switch (jToken.Type)
            {
                // 5.1.1
                case JTokenType.String:
                case JTokenType.Integer:
                case JTokenType.Boolean:
                case JTokenType.Float:
                    result = await ExpandScalarAsync(context, jToken, cancellationToken);
                    break;

                case JTokenType.Array:
                    result = await ExpandArrayAsync(context, (JArray)jToken, cancellationToken);
                    break;

                case JTokenType.Object:
                    result = await ExpandDictionaryAsync(context, jToken, cancellationToken);
                    break;

                case JTokenType.Null:
                    result = await ExpandNullAsync(context, jToken, cancellationToken);
                    break;

                default:
                    throw new NotSupportedException();
            }

            return result;
        }

        private ValueTask<JToken> ExpandNullAsync(Context context, JToken jToken, CancellationToken cancellationToken) => new ValueTask<JToken>(null);

        private async ValueTask<JToken> ExpandDictionaryAsync(Context context, JToken jToken, CancellationToken cancellationToken)
        {
            var jObject = (JObject)jToken;
            // 6) If element contains the key @context, set active context to the result of the
            //    Context Processing algorithm, passing active context and the value of the @context
            //    key as local context.
            if (jObject.TryGetValue(LDKeywords.Context, out var contextToken))
                context = await ProcessContextAsync(context, contextToken, cancellationToken);

            var result = new JObject();
            foreach (var kvp in jObject)
            {
                // 7.1) If key is @context, continue to the next key.
                if (kvp.Key == LDKeywords.Context) continue;

                // 7.2) Set expanded property to the result of using the IRI Expansion algorithm,
                //      passing active context, key for value, and true for vocab.
                var expandedProperty = ExpandIri(context, kvp.Key, true);

                // 7.3) If expanded property is null or it neither contains a colon(:) nor it is a
                //      keyword, drop key by continuing to the next key.
                if (expandedProperty is null || (expandedProperty.IndexOf(':', StringComparison.Ordinal) < 0 &&
                    !LDKeywords.IsKeyword(expandedProperty)))
                    continue;

                // 7.4) If expanded property is a keyword:
                if (LDKeywords.IsKeyword(expandedProperty))
                {
                    await ExpandKeywordAsync(context, result, expandedProperty, kvp.Value, cancellationToken);
                }
            }
        }

        protected virtual async ValueTask ExpandKeywordAsync(Context context, JObject result, string expandedProperty, JToken value, CancellationToken cancellationToken)
        {
            // 7.4.1) If active property equals @reverse, an invalid reverse property map
            //        error has been detected and processing is aborted.
            if (context.ActiveProperty == LDKeywords.Reverse)
                throw new LinkedDataException(LinkedDataErrorCode.InvalidReversePropertyMap);

            // 7.4.2) If result has already an expanded property member, an colliding keywords
            //        error has been detected and processing is aborted.
            if (result.ContainsKey(expandedProperty))
                throw new LinkedDataException(LinkedDataErrorCode.CollidingKeywords);

            JToken expandedValue;
            JArray expandedArray;

            switch (expandedProperty)
            {
                case LDKeywords.Id:
                    // 7.4.3) If expanded property is @id and value is not a string, an invalid @id
                    //        value error has been detected and processing is aborted.
                    if (value.Type != JTokenType.String)
                        throw new LinkedDataException(LinkedDataErrorCode.InvalidIdValue);

                    // Otherwise, set expanded value to the result of using the IRI Expansion
                    // algorithm, passing active context, value, and true for document relative.
                    expandedValue = ExpandIri(context, (string)((JValue)value).Value, documentRelative: true);
                    break;

                case LDKeywords.Type:
                    // 7.4.4) If expanded property is @type and value is neither a string nor an array
                    //        of strings, an invalid type value error has been detected and processing
                    //        is aborted.
                    if (value.Type == JTokenType.String)
                        expandedArray = new JArray(value.DeepClone());
                    else if (value.Type == JTokenType.Array)
                        expandedArray = (JArray)value.DeepClone();
                    else
                        throw new LinkedDataException(LinkedDataErrorCode.InvalidTypeValue);

                    for (var i = 0; i < expandedArray.Count; i++)
                    {
                        if (expandedArray[i].Type != JTokenType.String)
                            throw new LinkedDataException(LinkedDataErrorCode.InvalidTypeValue);

                        // Otherwise, set expanded value to the result of using the IRI Expansion
                        // algorithm, passing active context, true for vocab, and true for document
                        // relative to expand the value or each of its items.
                        var expandedItem = (JValue)expandedArray[i];
                        expandedItem.Value = ExpandIri(context, (string)expandedItem.Value, vocab: true, documentRelative: true);
                    }
                    expandedValue = expandedArray;
                    break;

                case LDKeywords.Graph:
                    // 7.4.5) If expanded property is @graph, set expanded value to the result of using
                    //        this algorithm recursively passing active context, @graph for active property,
                    //        and value for element.
                    context = context.CreateUpdated(activeProperty: LDKeywords.Graph);
                    expandedValue = await ExpandAsync(context, value, cancellationToken);
                    break;

                case LDKeywords.Value:
                    // 7.4.6) If expanded property is @value and value is not a scalar or null, an invalid value
                    //        object value error has been detected and processing is aborted.
                    if (!(value is JValue))
                        throw new LinkedDataException(LinkedDataErrorCode.InvalidValueObjectValue);

                    // Otherwise, set expanded value to value.
                    expandedValue = value.DeepClone();
                    result.Add(LDKeywords.Value, expandedProperty);

                    // If expanded value is null, set the @value member of result to null and
                    // continue with the next key from element.
                    return;

                case LDKeywords.Language:
                    // 7.4.7) If expanded property is @language and value is not a string, an invalid
                    //        language-tagged string error has been detected and processing is aborted.
                    if (value.Type != JTokenType.String)
                        throw new LinkedDataException(LinkedDataErrorCode.InvalidLanguageTaggedString);

                    // Otherwise, set expanded value to lowercased value.
#                   pragma warning disable CA1308 // Normalize strings to uppercase
                    expandedValue = ((string)((JValue)value).Value).ToLowerInvariant();
#                   pragma warning restore CA1308 // Normalize strings to uppercase
                    break;

                case LDKeywords.Index:
                    // 7.4.8) If expanded property is @index and value is not a string, an
                    //        invalid @index value error has been detected and processing is aborted.
                    if (value.Type != JTokenType.String)
                        throw new LinkedDataException(LinkedDataErrorCode.InvalidIndexValue);
                    expandedValue = value.DeepClone();
                    result.Add(LDKeywords.Index, expandedValue);
                    break;

                case LDKeywords.List:
                    // 7.4.9.1) If active property is null or @graph, continue with the next key from
                    //          element to remove the free-floating list.
                    if (context.ActiveProperty == null || context.ActiveProperty == LDKeywords.Graph)
                        return;

                    // 7.4.9.2) Otherwise, initialize expanded value to the result of using this
                    //          algorithm recursively passing active context, active property, and
                    //          value for element.
                    expandedValue = expandedArray = await ExpandAsync(context, value, cancellationToken);

                    // 7.4.9.3) If expanded value is a list object, a list of lists error has been
                    //          detected and processing is aborted.
                    for (var i = 0; i < expandedArray.Count; i++)
                    {
                        var expandedElement = expandedArray[i];
                        if (expandedElement is JObject o && o.ContainsKey(LDKeywords.List))
                            throw new LinkedDataException(LinkedDataErrorCode.ListOfLists);
                    }
                    break;

                case LDKeywords.Set:
                    // 7.4.10) If expanded property is @set, set expanded value to the result of using
                    // this algorithm recursively, passing active context, active property, and value
                    // for element.

                    expandedValue = await ExpandAsync(context, value, cancellationToken);
                    break;

                case LDKeywords.Reverse:
                    // 7.4.11) If expanded property is @reverse and value is not a JSON object, an
                    //         invalid @reverse value error has been detected and processing is aborted.
                    if (value.Type != JTokenType.Object)
                        throw new LinkedDataException(LinkedDataErrorCode.InvalidReverseValue);

                    // 7.4.11.1) Initialize expanded value to the result of using this algorithm
                    //           recursively, passing active context, @reverse as active property,
                    //           and value as element.
                    context = context.CreateUpdated(activeProperty: LDKeywords.Reverse);
                    var expandedObject = (JObject)await ExpandToSingleValueAsync(context, value, cancellationToken).ConfigureAwait(false);

                    // 7.4.11.2) If expanded value contains an @reverse member, i.e., properties that
                    //           are reversed twice, execute for each of its property and item the
                    //           following steps:
                    var count = expandedObject.Count;
                    if (expandedObject.TryGetValue(LDKeywords.Reverse, out var reverseToken))
                    {
                        if (reverseToken.Type != JTokenType.Object)
                            throw new LinkedDataException(LinkedDataErrorCode.InvalidReverseValue);

                        count--;
                        foreach (var prop in (JObject)reverseToken)
                        {
                            if (result.ContainsKey(prop.Key)) continue;
                            var arr = new JArray(prop.Value.DeepClone());
                            result.Add(prop.Key, arr);
                        }
                    }

                    // 7.4.11.3) If expanded value contains members other than @reverse:
                    if (count > 0)
                    {
                        // 7.4.11.3.2) Reference the value of the @reverse member in result using
                        //             the variable reverse map.
                        var reverseMap = new JObject();

                        // 7.4.11.3.1) If result does not have an @reverse member, create one and set
                        //             its value to an empty JSON object.
                        if (!result.ContainsKey(LDKeywords.Reverse))
                            result.Add(LDKeywords.Reverse, reverseMap);

                        // 7.4.11.3.3) For each property and items in expanded value other than
                        //             @reverse:
                        foreach (var prop in expandedObject)
                        {
                            if (prop.Key == LDKeywords.Reverse) continue;
                            if (prop.Value.Type != JTokenType.Array) continue;

                            // 7.4.11.3.3.1) For each item in items:
                            foreach (var item in (JArray)prop.Value)
                            {
                                // 7.4.11.3.3.1.1) If item is a value object or list object, an
                                //                 invalid reverse property value has been detected
                                //                 and processing is aborted.
                                if (item is JObject o &&
                                    (o.ContainsKey(LDKeywords.Value) || o.ContainsKey(LDKeywords.List)))
                                    throw new LinkedDataException(LinkedDataErrorCode.InvalidReverseValue);

                                if (!reverseMap.TryGetValue(prop.Key, out var items))
                                    reverseMap.Add(prop.Key, items = new JArray());
                                ((JArray)items).Add(item.DeepClone());
                            }
                        }
                    }

                    // 7.4.11.4) Continue with the next key from element.
                    return;

                default:
                    expandedValue = null;
                    break;
            }

            if (expandedValue != null)
                result.Add(expandedProperty, expandedValue);
        }

        private string ExpandIri(Context context, string value, bool vocab = false, bool documentRelative = false)
        {
            throw new NotImplementedException();
        }

        private async ValueTask<JToken> ExpandArrayAsync(Context context, JArray jArray, CancellationToken cancellationToken)
        {
            // 4.1) Initialize an empty array, result.
            var newArray = new JArray();

            // 4.2) For each item in element:
            for (var i = 0; i < jArray.Count; i++)
            {
                // 4.2.1) Initialize expanded item to the result of using this algorithm recursively,
                //        passing active context, active property, item as element, and the frame
                //        expansion flag.
                var item = jArray[i];
                var expanded = await ExpandToSingleValueAsync(context, jArray, cancellationToken).ConfigureAwait(false);

                // 4.2.2) If the active property is @list or its container mapping includes @list,
                //        the expanded item must not be an array or a list object, otherwise a list
                //        of lists error has been detected and processing is aborted
                // TODO: Abort on list-of-lists

                // 4.2.3) If expanded item is an array, append each of its items to result. Otherwise,
                //        if expanded item is not null, append it to result.
                if (expanded is null) continue;
                if (expanded.Type == JTokenType.Array)
                {
                    var expandedArray = (JArray)expanded;
                    for (var j = 0; j < expandedArray.Count; j++)
                    {
                        expanded = expandedArray[j];
                        expandedArray[j] = null;
                        newArray.Add(expanded);
                    }
                }
                else
                {
                    newArray.Add(item);
                }
            }

            return newArray;
        }

        private ValueTask<JToken> ExpandScalarAsync(Context context, JToken jToken, CancellationToken cancellationToken)
        {
            // 3.1) If active property is null or @graph, drop the free-floating scalar by
            //      returning null.
            if (context.ActiveProperty == null || context.ActiveProperty == LDKeywords.Graph)
                return new ValueTask<JToken>(default(JToken));

            // 3.2) Return the result of the Value Expansion algorithm, passing the active
            //      context, active property, and element as value.
            return ExpandValueAsync(context, ((JValue)jToken).Value, cancellationToken);
        }

        private ValueTask<JToken> ExpandValueAsync(Context context, object value, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        private async ValueTask<Context> ProcessContextAsync(Context context, JToken jToken, CancellationToken cancellationToken)
        {
            var result = context;

            // 2) If local context is not an array, set it to an array containing only local context.
            foreach (var item in ManyOrSelf(jToken))
            {
                if (item.Type == JTokenType.Null)
                {
                    // 3.1) If context is null, set result to a newly-initialized active context
                    //      and continue with the next context.
                    result = result.CreateRoot();
                    continue;
                }
                else if (item.Type == JTokenType.String)
                {
                    var iri = new Uri(result.DocumentIri, (string)((JValue)item).Value);

                    // 3.2.2) If context is in the remote contexts array, a recursive context
                    //        inclusion error has been detected and processing is aborted; otherwise,
                    //        add context to remote contexts.
                    if (!result.TryCreateRemote(iri, out result))
                        throw new LinkedDataException(LinkedDataErrorCode.RecursiveContextInclusion);

                    var remoteContext = await GetContextAsync(iri, cancellationToken);

                    if (!remoteContext.TryGetValue(LDKeywords.Context, out var remoteToken))
                        throw new LinkedDataException(LinkedDataErrorCode.InvalidRemoteContext);

                    // 3.2.3) Set result to the result of recursively calling this algorithm,
                    //        passing result for active context, context for local context,
                    //        and remote contexts.
                    result = await ProcessContextAsync(result, remoteToken, cancellationToken);
                    continue;
                }
                else if (item.Type != JTokenType.Object)
                {
                    // 3.3) If context is not a JSON object, an invalid local context error has been detected and processing is aborted.
                    throw new LinkedDataException(LinkedDataErrorCode.InvalidLocalContext);
                }

                var obj = (JObject)item;

                // 3.4) If context has an @base key and remote contexts is empty, i.e., the currently being processed context is not a remote context:
                if (result.IsLocalContext && obj.TryGetValue(LDKeywords.Base, out var baseToken))
                {
                    // 3.4.2) If value is null, remove the base IRI of result.
                    if (baseToken.Type == JTokenType.Null)
                        result = result.CreateRemoved(baseIri: true);
                    else if (baseToken.Type != JTokenType.String)
                        throw new LinkedDataException(LinkedDataErrorCode.InvalidBaseValue);

                    var value = (string)((JValue)baseToken).Value;
                    // 3.4.3) Otherwise, if value is an absolute IRI, the base IRI of result
                    //        is set to value.
                    if (Uri.TryCreate(value, UriKind.Absolute, out var absolute))
                        result = result.CreateUpdated(baseIri: absolute);
                    // 3.4.4) Otherwise, if value is a relative IRI and the base IRI of result is
                    //        not null, set the base IRI of result to the result of resolving value
                    //        against the current base IRI of result.
                    else if (Uri.TryCreate(value, UriKind.Relative, out var relative) && result.BaseIri != null)
                        result = result.CreateUpdated(baseIri: new Uri(result.BaseIri, value));
                    // 3.4.5) Otherwise, an invalid base IRI error has been detected and
                    //        processing is aborted.
                    else
                        throw new LinkedDataException(LinkedDataErrorCode.InvalidBaseIri);
                }

                // 3.5.3) If context has an @vocab key:
                if (obj.TryGetValue(LDKeywords.Vocab, out var vocabToken))
                {
                    // 3.5.2) If value is null, remove any vocabulary mapping from result.
                    if (vocabToken.Type == JTokenType.Null)
                        result = result.CreateRemoved(vocabularyMapping: true);
                    else if (vocabToken.Type != JTokenType.String)
                        throw new LinkedDataException(LinkedDataErrorCode.InvalidVocabMapping);
                    else
                    {
                        var value = (string)((JValue)vocabToken).Value;

                        // 3.5.3) Otherwise, if value is an absolute IRI or blank node identifier,
                        //        the vocabulary mapping of result is set to value.
                        if (Uri.TryCreate(value, UriKind.Absolute, out var tmp) || value.StartsWith("_:", StringComparison.Ordinal))
                            result = result.CreateUpdated(vocabularyMapping: value);
                        // If it is not an absolute IRI or blank node identifier, an invalid vocab
                        // mapping error has been detected and processing is aborted.
                        else
                            throw new LinkedDataException(LinkedDataErrorCode.InvalidVocabMapping);
                    }
                }

                // 3.6) If context has an @language key:
                if (obj.TryGetValue(LDKeywords.Language, out var languageToken))
                {
                    // 3.6.2) If value is null, remove any default language from result.
                    if (vocabToken.Type == JTokenType.Null)
                        result = result.CreateRemoved(defaultLanguage: true);
                    // If it is not a string, an invalid default language error has been detected
                    // and processing is aborted.
                    else if (vocabToken.Type != JTokenType.String)
                        throw new LinkedDataException(LinkedDataErrorCode.InvalidDefaultLanguage);
                    // 3.6.3) Otherwise, if value is string, the default language of result
                    //        is set to lowercased value.
                    else
                    {
                        var value = (string)((JValue)vocabToken).Value;
#                       pragma warning disable CA1308 // Normalize strings to uppercase
                        result = result.CreateUpdated(defaultLanguage: value.ToLowerInvariant());
#                       pragma warning restore CA1308 // Normalize strings to uppercase
                    }
                }

                var defined = new Dictionary<string, bool>(StringComparer.Ordinal);
                var terms = new Dictionary<string, TermDefinition>(result.Terms, StringComparer.Ordinal);
                foreach (var prop in obj)
                {
                    if (prop.Key == LDKeywords.Base || prop.Key == LDKeywords.Vocab ||
                        prop.Key == LDKeywords.Language)
                        continue;

                    // 1) If defined contains the key term and the associated value is true
                    //    (indicating that the term definition has already been created),
                    //    return. Otherwise, if the value is false, a cyclic IRI mapping
                    //    error has been detected and processing is aborted.
                    if (defined.TryGetValue(prop.Key, out var uncyclic))
                    {
                        if (uncyclic) continue;
                        throw new LinkedDataException(LinkedDataErrorCode.CyclicIriMapping);
                    }

                    // 2) Set the value associated with defined's term key to false.
                    defined[prop.Key] = false;

                    // 3) Since keywords cannot be overridden, term must not be a keyword.
                    //    Otherwise, a keyword redefinition error has been detected and
                    //    processing is aborted.
                    if (LDKeywords.IsKeyword(prop.Key))
                        throw new LinkedDataException(LinkedDataErrorCode.KeywordRedefinition);

                    // 4) Remove any existing term definition for term in active context.
                    terms.Remove(prop.Key);

                    // 5) Initialize value to a copy of the value associated with the key
                    //    term in local context.
                    var value = prop.Value;

                    // 6) If value is null or value is a JSON object containing the
                    //    key -value pair @id-null,
                    if (value.Type == JTokenType.Null ||
                        (value is JObject o && o.TryGetValue(LDKeywords.Id, out var idToken) && idToken.Type == JTokenType.Null))
                    {
                        terms[prop.Key] = default;
                        defined[prop.Key] = true;
                        continue;
                    }

                    // 7) Otherwise, if value is a string, convert it to a JSON object consisting
                    //    of a single member whose key is @id and whose value is value.
                    if (value.Type == JTokenType.String)
                    {
                        value = new JObject()
                        {
                            [LDKeywords.Id] = value.DeepClone()
                        };
                    }
                    // 8) Otherwise, value must be a JSON object, if not, an invalid term definition
                    //    error has been detected and processing is aborted.
                    else if (value.Type != JTokenType.Object)
                    {
                        throw new LinkedDataException(LinkedDataErrorCode.InvalidTermDefinition);
                    }
                }
            }
        }

        private static IEnumerable<JToken> ManyOrSelf(JToken token)
        {
            if (token is JArray arr)
            {
                for (var i = 0; i < arr.Count; i++) yield return arr[i];
            }
            else
            {
                yield return token;
            }
        }
    }
}
