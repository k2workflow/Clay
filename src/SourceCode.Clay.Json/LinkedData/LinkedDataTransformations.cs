#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SourceCode.Clay.Json.LinkedData
{
    public static partial class LinkedDataTransformations
    {
        public static ValueTask<JToken> ExpandAsync(JToken token, LinkedDataOptions options, CancellationToken cancellationToken)
        {
            var activeContext = new LinkedDataContext(options);
            string activeProperty = null;
            var element = token;
            return ExpandAsync(element, activeContext, activeProperty, cancellationToken);
        }

        private static ValueTask<JToken> ExpandAsync(JToken element, LinkedDataContext activeContext, string activeProperty, CancellationToken cancellationToken)
        {
            return ExpandItemAsync(element, activeContext, activeProperty, cancellationToken);
        }

        private static async ValueTask<JToken> ExpandItemAsync(JToken element, LinkedDataContext activeContext, string activeProperty, CancellationToken cancellationToken)
        {
            JToken result;
            switch (element.Type)
            {
                // 5.1.1
                case JTokenType.String:
                case JTokenType.Integer:
                case JTokenType.Boolean:
                case JTokenType.Float:
                    result = await ExpandScalarAsync((JValue)element, activeContext, activeProperty, cancellationToken).ConfigureAwait(false);
                    break;

                case JTokenType.Array:
                    result = await ExpandArrayAsync((JArray)element, activeContext, activeProperty, cancellationToken).ConfigureAwait(false);
                    break;

                case JTokenType.Object:
                    result = await ExpandDictionaryAsync((JObject)element, activeContext, activeProperty, cancellationToken).ConfigureAwait(false);
                    break;

                case JTokenType.Null:
                    result = await ExpandNullAsync((JValue)element, activeContext, activeProperty, cancellationToken).ConfigureAwait(false);
                    break;

                default:
                    throw new NotSupportedException();
            }

            return result;
        }

        private static ValueTask<JToken> ExpandNullAsync(JValue element, LinkedDataContext activeContext, string activeProperty, CancellationToken cancellationToken)
            => new ValueTask<JToken>(default(JToken));

        private static ValueTask<JToken> ExpandScalarAsync(JValue element, LinkedDataContext activeContext, string activeProperty, CancellationToken cancellationToken)
        {
            if (activeProperty == null || activeProperty == LDKeywords.Graph)
                return new ValueTask<JToken>(default(JToken));
            return new ValueTask<JToken>(ExpandValue(activeContext, activeProperty, element));
        }

        private static async ValueTask<JToken> ExpandArrayAsync(JArray element, LinkedDataContext activeContext, string activeProperty, CancellationToken cancellationToken)
        {
            var result = new JArray();

            for (var i = 0; i < element.Count; i++)
            {
                var item = element[i];

                // 4.2.1) Initialize expanded item to the result of using this algorithm recursively,
                //        passing active context, active property, item as element
                var expandedItem = await ExpandItemAsync(element, activeContext, activeProperty, cancellationToken);

                if (activeProperty == LDKeywords.List ||
                    (activeContext.TryGetTerm(activeProperty, out var term) && term.ContainerMappings.HasFlag(ContainerMappings.List)))
                {
                    if (expandedItem.Type == JTokenType.Array ||
                        (expandedItem is JObject o && o.ContainsKey(LDKeywords.List)))
                        throw new LinkedDataException(LinkedDataErrorCode.ListOfLists);
                }

                // 4.2.3) If expanded item is an array, append each of its items to result.
                if (expandedItem.Type == JTokenType.Array)
                {
                    var expandedArray = (JArray)expandedItem;
                    for (var j = 0; j < expandedArray.Count; i++)
                    {
                        var expandedArrayItem = expandedArray[i];
                        expandedArray[i] = null;
                        result.Add(expandedArrayItem);
                    }
                }
                // Otherwise, if expanded item is not null, append it to result.
                else
                {
                    result.Add(expandedItem);
                }
            }

            return result;
        }

        private static async ValueTask<JToken> ExpandDictionaryAsync(JObject element, LinkedDataContext activeContext, string activeProperty, CancellationToken cancellationToken)
        {
            // 6) If element contains the key @context, set active context to the result of the Context Processing algorithm, passing active context and the value of the @context key as local context.
            if (element.TryGetValue(LDKeywords.Context, out var contextToken))
                activeContext = await activeContext.ParseAsync(contextToken, cancellationToken).ConfigureAwait(false);

            var result = new JObject();
            foreach (var kvp in element)
            {
                if (kvp.Key == LDKeywords.Context) continue;

                var expandedProperty = ExpandIri(activeContext, kvp.Key, vocab: true);

                // 9.3) If expanded property is null or it neither contains a colon (:) nor it is
                //      a keyword, drop key by continuing to the next key.
                if (expandedProperty == null ||
                    !expandedProperty.Contains(':', StringComparison.OrdinalIgnoreCase) ||
                    !LDKeywords.IsKeyword(expandedProperty))
                    continue;

                // 9.4) If expanded property is a keyword:
                if (LDKeywords.IsKeyword(expandedProperty))
                {
                    await ExpandKeywordAsync(expandedProperty, result, kvp.Value, activeContext, activeProperty, cancellationToken).ConfigureAwait(false);
                    continue;
                }
            }
        }

        private static async Task ExpandKeywordAsync(string expandedProperty, JObject result, JToken value, LinkedDataContext activeContext, string activeProperty, CancellationToken cancellationToken)
        {
            if (result.ContainsKey(expandedProperty))
                throw new LinkedDataException(LinkedDataErrorCode.CollidingKeywords);

            JArray expandedArray;
            JToken expandedValue;
            switch (expandedProperty)
            {
                case LDKeywords.Reverse: throw new LinkedDataException(LinkedDataErrorCode.InvalidReversePropertyMap);
                case LDKeywords.Id:
                    if (value.Type != JTokenType.String)
                        throw new LinkedDataException(LinkedDataErrorCode.InvalidIdValue);
                    expandedValue = ExpandIri(activeContext, (string)value, documentRelative: true);
                    break;

                case LDKeywords.Type:
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
                        var expandedItem = (JValue)expandedArray[i];
                        expandedItem.Value = ExpandIri(activeContext, (string)expandedItem.Value, vocab: true, documentRelative: true);
                    }
                    expandedValue = expandedArray;
                    break;

                case LDKeywords.Graph:
                    expandedValue = await ExpandAsync(value, activeContext, LDKeywords.Graph, cancellationToken).ConfigureAwait(false);
                    if (expandedValue.Type != JTokenType.Array)
                        throw new LinkedDataException(LinkedDataErrorCode.InvalidGraph);
                    expandedArray = (JArray)expandedValue;
                    for (var i = 0; i < expandedArray.Count; i++)
                    {
                        if (expandedArray[i].Type != JTokenType.Object)
                            throw new LinkedDataException(LinkedDataErrorCode.InvalidGraph);
                    }
                    break;
            }
        }
    }
}
