#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SourceCode.Clay.Json.LinkedData
{
    partial class LinkedDataTransformation
    {
        // https://www.w3.org/TR/json-ld-api/#expansion-algorithm

        public static async ValueTask<JArray> ExpandAsync(
            JToken token,
            LinkedDataOptions options,
            CancellationToken cancellationToken = default)
        {
            var activeContext = options.ExpandContext;
            string activeProperty = null;
            var element = token;
            var item = await ExpandAsync(element, activeContext, activeProperty, cancellationToken);

            // If, after the above algorithm is run, the result is a JSON object that contains only an @graph key, set
            // the result to the value of @graph's value.
            if (item is JObject o && o.Count == 1 && o.TryGetValue(LinkedDataKeywords.Graph, out var graph))
            {
                item = graph;
            }
            // Otherwise, if the result is null, set it to an empty array.
            else if (item.Type == JTokenType.Null) return new JArray();

            // Finally, if the result is not an array, then set the result to an array containing only the result.
            return item.Type == JTokenType.Array
            ? (JArray)item
            : new JArray(item);
        }

        private static async ValueTask<JToken> ExpandAsync(
            JToken element,
            LinkedDataContext activeContext,
            string activeProperty,
            CancellationToken cancellationToken)
        {
            JToken result;
            switch (element.Type)
            {
                // 5.1.1
                case JTokenType.String:
                case JTokenType.Integer:
                case JTokenType.Boolean:
                case JTokenType.Float:
                case JTokenType.Bytes:
                case JTokenType.Guid:
                case JTokenType.TimeSpan:
                case JTokenType.Date:
                case JTokenType.Uri:
                    result = await ExpandScalarAsync(
                        (JValue)element,
                        activeContext,
                        activeProperty,
                        cancellationToken)
                        .ConfigureAwait(false);
                    break;

                case JTokenType.Array:
                    result = await ExpandArrayAsync(
                        (JArray)element,
                        activeContext,
                        activeProperty,
                        cancellationToken)
                        .ConfigureAwait(false);
                    break;

                case JTokenType.Object:
                    result = await ExpandDictionaryAsync(
                        (JObject)element,
                        activeContext,
                        activeProperty,
                        cancellationToken)
                        .ConfigureAwait(false);
                    break;

                case JTokenType.Null:
                    result = await ExpandNullAsync(
                        (JValue)element,
                        activeContext,
                        activeProperty,
                        cancellationToken)
                        .ConfigureAwait(false);
                    break;

                default:
                    throw new NotSupportedException();
            }

            return result;
        }

        private static ValueTask<JToken> ExpandNullAsync(
            JValue element,
            LinkedDataContext
            activeContext,
            string activeProperty,
            CancellationToken cancellationToken)
            => new ValueTask<JToken>(JValue.CreateNull());

        private static ValueTask<JToken> ExpandScalarAsync(
            JValue element,
            LinkedDataContext activeContext,
            string activeProperty,
            CancellationToken cancellationToken)
        {
            if (activeProperty == null || activeProperty == LinkedDataKeywords.Graph)
                return new ValueTask<JToken>(JValue.CreateNull());
            return new ValueTask<JToken>(ExpandValue(activeContext, activeProperty, element));
        }

        private static async ValueTask<JToken> ExpandArrayAsync(
            JArray element,
            LinkedDataContext activeContext,
            string activeProperty,
            CancellationToken cancellationToken)
        {
            var result = new JArray();

            for (var i = 0; i < element.Count; i++)
            {
                var item = element[i];

                // 3.2.1) Initialize expanded item to the result of using this algorithm recursively, passing active
                //        context, active property, item as element
                var expandedItem = await ExpandAsync(item, activeContext, activeProperty, cancellationToken);

                if (activeProperty == LinkedDataKeywords.List ||
                    (activeContext.TryGetTerm(activeProperty, out var term) &&
                        term.ContainerMappings.HasFlag(ContainerMappings.List)))
                {
                    if (expandedItem.Type == JTokenType.Array ||
                        (expandedItem is JObject o && o.ContainsKey(LinkedDataKeywords.List)))
                        throw new LinkedDataException(LinkedDataErrorCode.ListOfLists);
                }

                // 3.2.3) If expanded item is an array, append each of its items to result.
                if (expandedItem.Type == JTokenType.Array)
                {
                    var expandedArray = (JArray)expandedItem;
                    for (var j = 0; j < expandedArray.Count; j++)
                    {
                        var expandedArrayItem = expandedArray[j];
                        if (expandedArrayItem.Type == JTokenType.Null) continue;
                        expandedArray[j] = null;
                        result.Add(expandedArrayItem);
                    }
                }
                // Otherwise, if expanded item is not null, append it to result.
                else if (expandedItem.Type != JTokenType.Null)
                {
                    result.Add(expandedItem);
                }
            }

            return result;
        }

        private static async ValueTask<JToken> ExpandDictionaryAsync(
            JObject element,
            LinkedDataContext activeContext,
            string activeProperty,
            CancellationToken cancellationToken)
        {
            // 5) If element contains the key @context, set active context to the result of the Context Processing
            //    algorithm, passing active context and the value of the @context key as local context.
            if (element.TryGetValue(LinkedDataKeywords.Context, out var contextToken))
                activeContext = await activeContext.ParseAsync(contextToken, cancellationToken).ConfigureAwait(false);

            var result = new JObject();
            JToken resultToken = result;
            foreach (var (key, value) in element.OrderLexicographically())
            {
                // 7.1) If key is @context, continue to the next key.
                if (key == LinkedDataKeywords.Context) continue;

                // 7.2) Set expanded property to the result of using the IRI Expansion algorithm, passing active
                //      context, key for value, and true for vocab.
                var expandedProperty = ExpandIri(activeContext, key, vocab: true);

                // 7.4) If expanded property is a keyword:
                if (LinkedDataKeywords.IsKeyword(expandedProperty))
                {
                    await ExpandKeywordAsync(
                        expandedProperty,
                        result,
                        value,
                        activeContext,
                        activeProperty,
                        cancellationToken)
                        .ConfigureAwait(false);
                    // 7.4.13) Continue with the next key from element.
                    continue;
                }

                // 7.3) If expanded property is null or it neither contains a colon (:) nor it is
                //      a keyword, drop key by continuing to the next key.
                if (expandedProperty == null ||
                    !expandedProperty.Contains(':', StringComparison.OrdinalIgnoreCase))
                    continue;

                JToken expandedValue = JValue.CreateNull();
                var hasTerm = activeContext.TryGetTerm(key, out var term);

                if (hasTerm && term.Options.HasFlag(LinkedDataTermOptions.ClearTerm))
                    continue;

                // 7.5) Otherwise, if key's container mapping in active context is @language and value is a JSON object
                //      then value is expanded from a language map as follows:
                if (hasTerm && term.ContainerMappings.HasFlag(ContainerMappings.Language) &&
                    value.Type == JTokenType.Object)
                {
                    // 7.5.1) Initialize expanded value to an empty array.
                    var expandedArray = new JArray();
                    expandedValue = expandedArray;

                    // 7.5.2) For each key-value pair language-language value in value, ordered lexicographically by
                    //        language:
                    foreach (var (lang, langValue) in ((JObject)value).OrderLexicographically())
                    {
#                       pragma warning disable CA1308 // Normalize strings to uppercase
                        var lowerLang = lang.ToLowerInvariant();
#                       pragma warning restore CA1308 // Normalize strings to uppercase
                        // 7.5.2.1) If language value is not an array set it to an array containing only language
                        //          value.
                        var langArray = langValue is JArray
                            ? (JArray)langValue.DeepClone()
                            : new JArray(langValue.DeepClone());

                        // 7.5.2.2) For each item in language value:
                        for (var i = 0; i < langArray.Count; i++)
                        {
                            var item = langArray[i];

                            // 7.5.2.1) item must be a string, otherwise an invalid language map value error has been
                            //          detected and processing is aborted.
                            if (item.Type != JTokenType.String)
                                throw new LinkedDataException(LinkedDataErrorCode.InvalidLanguageMapValue);
                            expandedArray.Add(new JObject()
                            {
                                [LinkedDataKeywords.Value] = item,
                                [LinkedDataKeywords.Language] = lang
                            });
                        }
                    }
                }
                // 7.6) Otherwise, if key's container mapping in active context is @index and value is a JSON object
                //      then value is expanded from an index map as follows:
                else if (hasTerm && term.ContainerMappings == ContainerMappings.Index &&
                    value.Type == JTokenType.Object)
                {
                    // 7.6.1) Initialize expanded value to an empty array.
                    var expandedArray = new JArray();
                    expandedValue = expandedArray;

                    // 7.6.2) For each key-value pair index-index value in value, ordered lexicographically by index:
                    foreach (var (index, indexValue) in ((JObject)value).OrderLexicographically())
                    {
                        // 7.6.2.1) If index value is not an array set it to an array containing only index value.
                        // 7.6.2.2) Initialize index value to the result of using this algorithm recursively, passing
                        //          active context, key as active property, and index value as element.
                        var expandedIndexValue = await ExpandAsync(
                            indexValue,
                            activeContext,
                            key,
                            cancellationToken)
                            .ConfigureAwait(false);

                        var expandedIndexValueArray = expandedIndexValue.Type == JTokenType.Array
                            ? (JArray)expandedIndexValue
                            : new JArray(expandedIndexValue);

                        // 7.6.2.3) For each item in index value:
                        for (var i = 0; i < expandedIndexValueArray.Count; i++)
                        {
                            var item = expandedIndexValueArray[i];
                            if (item.Type == JTokenType.Null) continue;
                            // 7.6.2.3.1) If item does not have the key @index, add the key-value pair (@index-index)
                            //            to item.
                            if (item is JObject o && !o.ContainsKey(LinkedDataKeywords.Index))
                                o.Add(LinkedDataKeywords.Index, index);
                            // 7.6.2.3.2) Append item to expanded value.
                            expandedArray.Add(item);
                        }
                    }
                }
                // 7.7) Otherwise, initialize expanded value to the result of using this algorithm recursively, passing
                //      active context, key for active property, and value for element.
                else
                {
                    expandedValue = await ExpandAsync(
                        value,
                        activeContext,
                        key,
                        cancellationToken)
                        .ConfigureAwait(false);
                }

                // 7.8) If expanded value is null, ignore key by continuing to the next key from element.
                if (expandedValue.Type == JTokenType.Null) continue;

                // 7.9) If the container mapping associated to key in active context is @list and expanded value is not
                //      already a list object, convert expanded value to a list object by first setting it to an array
                //      containing only expanded value if it is not already an array, and then by setting it to a JSON
                //      object containing the key-value pair @list-expanded value.
                if (hasTerm && term.ContainerMappings == ContainerMappings.List &&
                    (expandedValue.Type != JTokenType.Object ||
                        !((JObject)expandedValue).ContainsKey(LinkedDataKeywords.List)))
                {
                    expandedValue = expandedValue.Type == JTokenType.Array
                        ? expandedValue
                        : new JArray(expandedValue);
                    expandedValue = new JObject()
                    {
                        [LinkedDataKeywords.List] = expandedValue
                    };
                }

                // 7.10) Otherwise, if the term definition associated to key indicates that it is a reverse property
                if (hasTerm && term.Options.HasFlag(LinkedDataTermOptions.ReverseProperty))
                {
                    // 7.10.1) If result has no @reverse member, create one and initialize its value to an empty JSON
                    //         object.
                    if (!result.TryGetValue(LinkedDataKeywords.Reverse, out var reverseToken))
                        result.Add(LinkedDataKeywords.Reverse, reverseToken = new JObject());

                    // 7.10.2) Reference the value of the @reverse member in result using the variable reverse map.
                    var reverseMap = (JObject)reverseToken;

                    // 7.10.3) If expanded value is not an array, set it to an array containing expanded value.
                    var expandedArray = expandedValue.Type == JTokenType.Array
                        ? (JArray)expandedValue
                        : new JArray(expandedValue);
                    expandedValue = expandedArray;

                    // 7.10.4) For each item in expanded value
                    for (var i = 0; i < expandedArray.Count; i++)
                    {
                        var item = expandedArray[i];
                        if (item.Type == JTokenType.Null) continue;
                        if (!(item is JObject o) ||
                            (o.ContainsKey(LinkedDataKeywords.Value) || o.ContainsKey(LinkedDataKeywords.List)))
                            throw new LinkedDataException(LinkedDataErrorCode.InvalidReversePropertyValue);
                        if (!reverseMap.TryGetValue(expandedProperty, out var reverseMapItemToken))
                            reverseMap.Add(expandedProperty, reverseMapItemToken = new JArray());
                        ((JArray)reverseMapItemToken).Add(item);
                    }
                }
                // 7.11) Otherwise, if key is not a reverse property:
                else
                {
                    if (!result.TryGetValue(expandedProperty, out var propertyToken))
                        result.Add(expandedProperty, propertyToken = new JArray());

                    var resultContainerArray = (JArray)propertyToken;
                    if (expandedValue.Type == JTokenType.Array)
                    {
                        var expandedArray = (JArray)expandedValue;
                        for (var i = 0; i < expandedArray.Count; i++)
                        {
                            if (expandedArray[i].Type == JTokenType.Null) continue;
                            resultContainerArray.Add(expandedArray[i]);
                        }
                    }
                    else if (expandedValue.Type != JTokenType.Null)
                    {
                        resultContainerArray.Add(expandedValue);
                    }
                }
            }

            // 8) If result contains the key @value:
            if (result.TryGetValue(LinkedDataKeywords.Value, out var valueToken))
            {
                var count = result.Count - 1; // - 1 for @value
                if (result.ContainsKey(LinkedDataKeywords.Language) &&
                    !result.ContainsKey(LinkedDataKeywords.Type)) count--;
                if (result.ContainsKey(LinkedDataKeywords.Type)) count--;
                if (result.ContainsKey(LinkedDataKeywords.Index)) count--;
                if (count != 0) throw new LinkedDataException(LinkedDataErrorCode.InvalidValueObject);
                if (valueToken.Type == JTokenType.Null) resultToken = JValue.CreateNull();
                else if (result.ContainsKey(LinkedDataKeywords.Language) && valueToken.Type != JTokenType.String)
                    throw new LinkedDataException(LinkedDataErrorCode.InvalidLanguageTaggedValue);
                //else if (result.TryGetValue(LDKeywords.Type, out var typeToken) &&
                //    !Uri.TryCreate((string)typeToken, UriKind.Absolute, out var tmp))
                //    throw new LinkedDataException(LinkedDataErrorCode.InvalidTypeValue);
            }
            // 9) Otherwise, if result contains the key @type and its associated value
            //    is not an array, set it to an array containing only the associated
            //    value.
            else if (result.TryGetValue(LinkedDataKeywords.Type, out var typeToken) &&
                typeToken.Type != JTokenType.Array)
            {
                result[LinkedDataKeywords.Type] = new JArray(typeToken);
            }
            // 10) Otherwise, if result contains the key @set or @list:
            else if (result.ContainsKey(LinkedDataKeywords.Set) || result.ContainsKey(LinkedDataKeywords.List))
            {
                var count = result.Count;
                if (result.ContainsKey(LinkedDataKeywords.Index)) count--;
                if (result.Count != 1)
                    throw new LinkedDataException(LinkedDataErrorCode.InvalidSetOrListObject);
                if (result.TryGetValue(LinkedDataKeywords.Set, out var setToken))
                    resultToken = setToken;
            }

            if (result.Count == 1 && result.ContainsKey(LinkedDataKeywords.Language))
                resultToken = JValue.CreateNull();

            if (activeProperty == null || activeProperty == LinkedDataKeywords.Graph)
            {
                if (result.Count == 0)
                    resultToken = JValue.CreateNull();
                else if (result.ContainsKey(LinkedDataKeywords.Value) || result.ContainsKey(LinkedDataKeywords.List))
                    resultToken = JValue.CreateNull();
                else if (result.Count == 1 && result.ContainsKey(LinkedDataKeywords.Id))
                    resultToken = JValue.CreateNull();
            }

            return resultToken;
        }

        private static async Task ExpandKeywordAsync(
            string expandedProperty,
            JObject result,
            JToken value,
            LinkedDataContext activeContext,
            string activeProperty,
            CancellationToken cancellationToken)
        {
            // 7.4.1) If active property equals @reverse, an invalid reverse property map error has been detected and
            //        processing is aborted.
            if (activeProperty == LinkedDataKeywords.Reverse)
                throw new LinkedDataException(LinkedDataErrorCode.InvalidReversePropertyMap);

            // 7.4.2) If result has already an expanded property member, an colliding keywords error has been detected
            //        and processing is aborted.
            if (result.ContainsKey(expandedProperty))
                throw new LinkedDataException(LinkedDataErrorCode.CollidingKeywords);

            JArray expandedArray;
            JToken expandedValue;
            switch (expandedProperty)
            {
                case LinkedDataKeywords.Id:
                    // 7.4.3) If expanded property is @id and value is not a string, an invalid @id value error has
                    //        been detected and processing is aborted.
                    if (value.Type != JTokenType.String)
                        throw new LinkedDataException(LinkedDataErrorCode.InvalidIdValue);
                    // Otherwise, set expanded value to the result of using the IRI Expansion algorithm, passing active
                    // context, value, and true for document relative.
                    expandedValue = ExpandIri(activeContext, (string)value, documentRelative: true);
                    break;

                case LinkedDataKeywords.Type:
                    // 7.4.4) If expanded property is @type and value is neither a string nor an array of strings, an
                    //        invalid type value error has been detected and processing is aborted. Otherwise, set
                    //        expanded value to the result of using the IRI Expansion algorithm, passing active
                    //        context, true for vocab, and true for document relative to expand the value or each of
                    //        its items.
                    if (value.Type == JTokenType.String)
                        expandedValue = ExpandIri(activeContext,
                            (string)value.DeepClone(),
                            vocab: true,
                            documentRelative: true);
                    else if (value.Type == JTokenType.Array)
                    {
                        expandedArray = (JArray)value.DeepClone();
                        for (var i = 0; i < expandedArray.Count; i++)
                        {
                            if (expandedArray[i].Type != JTokenType.String)
                                throw new LinkedDataException(LinkedDataErrorCode.InvalidTypeValue);
                            var expandedItem = (JValue)expandedArray[i];
                            expandedItem.Value = ExpandIri(
                                activeContext,
                                (string)expandedItem.Value,
                                vocab: true,
                                documentRelative: true);
                        }
                        expandedValue = expandedArray;
                    }
                    else
                        throw new LinkedDataException(LinkedDataErrorCode.InvalidTypeValue);
                    break;

                case LinkedDataKeywords.Graph:
                    // 7.4.5) If expanded property is @graph, set expanded value to the result of using this algorithm
                    //        recursively passing active context, @graph for active property, and value for element.
                    expandedValue = await ExpandAsync(
                        value,
                        activeContext,
                        LinkedDataKeywords.Graph,
                        cancellationToken).ConfigureAwait(false);

                    // LD1.1
                    expandedArray = (JArray)expandedValue;
                    for (var i = 0; i < expandedArray.Count; i++)
                    {
                        if (expandedArray[i].Type != JTokenType.Object)
                            throw new LinkedDataException(LinkedDataErrorCode.InvalidGraph);
                    }
                    break;

                case LinkedDataKeywords.Value:
                    // 7.4.6) If expanded property is @value and value is not a scalar or null, an invalid value object
                    //        value error has been detected and processing is aborted. Otherwise, set expanded value to
                    //        value. If expanded value is null, set the @value member of result to null and continue
                    //        with the next key from element. Null values need to be preserved in this case as the
                    //        meaning of an @type member depends on the existence of an @value member.
                    if (!(value is JValue))
                        throw new LinkedDataException(LinkedDataErrorCode.InvalidValueObjectValue);

                    expandedValue = value.DeepClone();
                    result[LinkedDataKeywords.Value] = expandedValue;
                    return;

                case LinkedDataKeywords.Language:
                    // 7.4.7) If expanded property is @language and value is not a string, an invalid language-tagged
                    //        string error has been detected and processing is aborted. Otherwise, set expanded value
                    //        to lowercased value.
                    if (value.Type != JTokenType.String)
                        throw new LinkedDataException(LinkedDataErrorCode.InvalidLanguageTaggedString);

#                   pragma warning disable CA1308 // Normalize strings to uppercase
                    expandedValue = ((string)((JValue)value).Value).ToLowerInvariant();
#                   pragma warning restore CA1308 // Normalize strings to uppercase
                    break;

                case LinkedDataKeywords.Index:
                    // 7.4.8) If expanded property is @index and value is not a string, an invalid @index value error
                    //        has been detected and processing is aborted. Otherwise, set expanded value to value.
                    if (value.Type != JTokenType.String)
                        throw new LinkedDataException(LinkedDataErrorCode.InvalidIndexValue);
                    expandedValue = value.DeepClone();
                    result[LinkedDataKeywords.Index] = expandedValue;
                    break;

                case LinkedDataKeywords.List:
                    // 7.4.9.1) If active property is null or @graph, continue with the next key from element to remove
                    //          the free-floating list.
                    if (activeProperty == null || activeProperty == LinkedDataKeywords.Graph)
                        return;

                    // 7.4.9.2) Otherwise, initialize expanded value to the result of using this algorithm recursively
                    //          passing active context, active property, and value for element.
                    expandedValue = await ExpandAsync(
                        value,
                        activeContext,
                        activeProperty,
                        cancellationToken)
                        .ConfigureAwait(false);

                    expandedArray = expandedValue.Type == JTokenType.Array
                        ? (JArray)expandedValue
                        : new JArray(expandedValue);
                    expandedValue = expandedArray;

                    for (var i = 0; i < expandedArray.Count; i++)
                    {
                        // 7.4.9.3) If expanded value is a list object, a list of lists error has been detected and
                        //          processing is aborted.
                        var expandedElement = expandedArray[i];
                        if (expandedElement is JObject o && o.ContainsKey(LinkedDataKeywords.List))
                            throw new LinkedDataException(LinkedDataErrorCode.ListOfLists);
                    }
                    break;

                case LinkedDataKeywords.Set:
                    // 7.4.10) If expanded property is @set, set expanded value to the result of using this algorithm
                    //         recursively, passing active context, active property, and value for element.
                    expandedValue = await ExpandAsync(
                        value,
                        activeContext,
                        activeProperty,
                        cancellationToken)
                        .ConfigureAwait(false);
                    break;

                case LinkedDataKeywords.Reverse:
                    // 7.4.11) If expanded property is @reverse and value is not a JSON object, an
                    //         invalid @reverse value error has been detected and processing is aborted.
                    if (value.Type != JTokenType.Object)
                        throw new LinkedDataException(LinkedDataErrorCode.InvalidReversePropertyValue);

                    // 7.4.11.1) Initialize expanded value to the result of using this algorithm
                    //           recursively, passing active context, @reverse as active property,
                    //           and value as element.
                    var expandedObject = (JObject)await ExpandAsync(
                        value,
                        activeContext,
                        LinkedDataKeywords.Reverse,
                        cancellationToken)
                        .ConfigureAwait(false);

                    // 7.4.11.2) If expanded value contains an @reverse member, i.e., properties that
                    //           are reversed twice, execute for each of its property and item the
                    //           following steps:
                    var count = expandedObject.Count;
                    if (expandedObject.TryGetValue(LinkedDataKeywords.Reverse, out var reverseToken))
                    {
                        if (reverseToken.Type != JTokenType.Object)
                            throw new LinkedDataException(LinkedDataErrorCode.InvalidReversePropertyValue);

                        count--;
                        foreach (var (property, item) in (JObject)reverseToken)
                        {
                            // 7.4.11.2.1) If result does not have a property member, create one and set its value to
                            //             an empty array.
                            if (!result.TryGetValue(property, out var propToken))
                                result.Add(property, propToken = new JArray());
                            if (propToken.Type != JTokenType.Array)
                                throw new LinkedDataException(LinkedDataErrorCode.InvalidReverseProperty);

                            var itemArray = item.Type == JTokenType.Array
                                ? (JArray)item
                                : new JArray(item);

                            // 7.4.11.2.2) Append item to the value of the property member of result.
                            for (var i = 0; i < itemArray.Count; i++)
                                ((JArray)propToken).Add(itemArray[i]);
                        }
                    }

                    // 7.4.11.3) If expanded value contains members other than @reverse:
                    if (count > 0)
                    {
                        // 7.4.11.3.1) If result does not have an @reverse member, create one and set
                        //             its value to an empty JSON object.
                        if (!result.TryGetValue(LinkedDataKeywords.Reverse, out var reverseMapToken))
                            result.Add(LinkedDataKeywords.Reverse, reverseMapToken = new JObject());

                        // 7.4.11.3.2) Reference the value of the @reverse member in result using
                        //             the variable reverse map.
                        var reverseMap = (JObject)reverseMapToken;

                        // 7.4.11.3.3) For each property and items in expanded value other than
                        //             @reverse:
                        foreach (var prop in expandedObject)
                        {
                            if (prop.Key == LinkedDataKeywords.Reverse) continue;

                            // 7.4.11.3.3.1) For each item in items:
                            foreach (var item in (JArray)prop.Value)
                            {
                                if (!(item is JObject o) ||
                                    (o.ContainsKey(LinkedDataKeywords.Value) ||
                                        o.ContainsKey(LinkedDataKeywords.List)))
                                    throw new LinkedDataException(LinkedDataErrorCode.InvalidReversePropertyValue);

                                if (!reverseMap.TryGetValue(prop.Key, out var items))
                                    reverseMap.Add(prop.Key, items = new JArray());
                                ((JArray)items).Add(item);
                            }
                        }
                    }

                    // 7.4.11.4) Continue with the next key from element.
                    return;

                default:
                    throw new NotSupportedException();
            }

            // 7.4.12) Unless expanded value is null, set the expanded property member of result to expanded value.
            if (expandedValue.Type != JTokenType.Null)
                result[expandedProperty] = expandedValue;
        }
    }
}
