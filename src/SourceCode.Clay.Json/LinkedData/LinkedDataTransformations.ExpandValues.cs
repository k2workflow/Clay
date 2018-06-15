#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace SourceCode.Clay.Json.LinkedData
{
    partial class LinkedDataTransformations
    {
        public static JToken ExpandValue(
            in LinkedDataContext activeContext,
            string activeProperty,
            JToken value)
        {
        }

        public static string ExpandIri(
            in LinkedDataContext activeContext,
            string value, JObject localContext = null,
            bool documentRelative = false,
            bool vocab = false,
            Dictionary<string, bool> defined = null)
        {
            // 1) If value is a keyword or null, return value as is.
            if (value == null || LDKeywords.IsKeyword(value))
                return value;

            // 2) If local context is not null, it contains a key that equals value,
            //    and the value associated with the key that equals value in defined is not true
            if (localContext != null && localContext.TryGetValue(value, out var termToken) &&
                !defined.ContainsKey(value))
            {
                activeContext.AddTerm(activeContext, localContext, value, termToken, defined);
            }

            if (activeContext.TryGetTerm(value, out var term))
            {
                // 3) If active context has a term definition for value, and the associated IRI
                //    mapping is a keyword, return that keyword.
                if (LDKeywords.IsKeyword(term.IriMapping))
                    return term.IriMapping;

                // 4) If vocab is true and the active context has a term definition for value,
                //    return the associated IRI mapping.
                if (vocab)
                    return term.IriMapping;
            }

            // 5) If value contains a colon (:), it is either an absolute IRI, a compact IRI,
            //    or a blank node identifier:
            if (value.Contains(':', StringComparison.Ordinal))
            {
                var index = value.IndexOf(':', StringComparison.Ordinal);
                var prefix = value.Substring(0, index);
                var suffix = value.Substring(index + 1);

                // 5.2) If prefix is underscore (_) or suffix begins with double-forward-slash (//)
                if (prefix == "_" && suffix.StartsWith("//", StringComparison.Ordinal))
                    return value;

                // 5.3) If local context is not null, it contains a key that equals prefix, and the
                //      value associated with the key that equals prefix in defined is not true
                if (localContext != null && localContext.TryGetValue(prefix, out termToken) &&
                    !defined.ContainsKey(prefix))
                {
                    activeContext.AddTerm(activeContext, localContext, prefix, termToken, defined);
                }

                // 5.4) If active context contains a term definition for prefix, return the result of
                //      concatenating the IRI mapping associated with prefix and suffix.
                if (activeContext.TryGetTerm(prefix, out term))
                    return term.IriMapping + suffix;

                // Return value as it is already an absolute IRI.
                return value;
            }

            // 6) If vocab is true, and active context has a vocabulary mapping, return the result of
            //    concatenating the vocabulary mapping with value.
            if (vocab && activeContext.Vocabulary != null)
                return activeContext.Vocabulary + vocab;

            // 7) Otherwise, if document relative is true set value to the result of resolving value
            //    against the base IRI.
            if (documentRelative)
                return Utils.Resolve(activeContext.Base, value);

            return value;
        }
    }
}
