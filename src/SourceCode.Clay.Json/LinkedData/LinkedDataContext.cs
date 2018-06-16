#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SourceCode.Clay.Json.LinkedData
{
#   pragma warning disable CA1815 // Override equals and operator equals on value types

    public readonly struct LinkedDataContext
    {
        private readonly HashSet<string> _remoteContexts;
        private readonly Dictionary<string, LinkedDataContext> _resolvedContexts;
        private readonly Dictionary<string, LinkedDataTerm> _terms;

        internal LinkedDataOptions Options { get; }
        internal string Base { get; }
        internal string Vocabulary { get; }
        internal string Language { get; }
        internal bool HasValue => _remoteContexts != null;

        internal LinkedDataContext(
            LinkedDataOptions options)
            : this(
                  new HashSet<string>(StringComparer.OrdinalIgnoreCase),
                  new Dictionary<string, LinkedDataContext>(StringComparer.OrdinalIgnoreCase),
                  new Dictionary<string, LinkedDataTerm>(StringComparer.Ordinal),
                  options,
                  options.Base,
                  null,
                  null)
        {
        }

        private LinkedDataContext(
            HashSet<string> remoteContexts,
            Dictionary<string, LinkedDataContext> resolvedContexts,
            Dictionary<string, LinkedDataTerm> terms,
            LinkedDataOptions options,
            string @base,
            string vocabulary,
            string language)
        {
            _remoteContexts = remoteContexts;
            _resolvedContexts = resolvedContexts;
            _terms = terms;
            Options = options;
            Base = @base;
            Vocabulary = vocabulary;
            Language = language;
        }

        internal bool TryGetTerm(string term, out LinkedDataTerm result)
        {
            if (term == null)
            {
                result = default;
                return false;
            }
            return _terms.TryGetValue(term, out result);
        }

        internal void AddTerms(LinkedDataContext activeContext, JObject contextObject)
            => AddTerms(LinkedDataTerm.Parse(activeContext, contextObject));

        internal void AddTerm(
            LinkedDataContext activeContext,
            JObject localContext,
            string term,
            JToken token,
            Dictionary<string, bool> defined)
            => AddTerms(LinkedDataTerm.Parse(activeContext,
                localContext,
                new KeyValuePair<string, JToken>(term, token),
                defined));

        private void AddTerms(IEnumerable<LinkedDataTerm> terms)
        {
            foreach (var term in terms)
                _terms[term.Term] = term;
        }

        internal async ValueTask<LinkedDataContext> ParseAsync(
            JToken localContext,
            CancellationToken cancellationToken)
        {
            if (_remoteContexts is null) throw new InvalidOperationException();

            // 1) Initialize result to the result of cloning active context.
            var result = new LinkedDataContext(
                _remoteContexts,
                _resolvedContexts,
                new Dictionary<string, LinkedDataTerm>(_terms, StringComparer.Ordinal),
                Options,
                Base,
                Vocabulary,
                Language);

            // 2) If local context is not an array, set it to an array containing only local context.
            var localContextArray = localContext.Is(JTokenType.Array)
                ? (JArray)localContext
                : new JArray(localContext);

            foreach (var context in localContextArray)
            {
                // 3.1) If context is null, set result to a newly-initialized active context
                //      and continue with the next context.
                if (context.Is(JTokenType.Null))
                {
                    result = new LinkedDataContext(
                        _remoteContexts,
                        _resolvedContexts,
                        new Dictionary<string, LinkedDataTerm>(StringComparer.Ordinal),
                        result.Options,
                        result.Options.Base,
                        result.Vocabulary,
                        result.Language);
                    continue;
                }

                // 3.2) If context is a string,
                if (context.Is(JTokenType.String))
                {
                    // 3.2.1) Set context to the result of resolving value against the base IRI
                    var uri = result.Base;
                    uri = Utils.Resolve(uri, (string)context);

                    // 3.2.2) If context is in the remote contexts array, a recursive context
                    //        inclusion error has been detected and processing is aborted; otherwise,
                    //        add context to remote contexts.
                    if (!_remoteContexts.Add(uri))
                        throw new LinkedDataException(LinkedDataErrorCode.RecursiveContextInclusion);

                    // LD 1.1:
                    // 3.2.3) If context was previously dereferenced, then the processor MUST NOT do
                    //        a further dereference, and context is set to the previously established
                    //        internal representation.
                    if (_resolvedContexts.TryGetValue(uri, out var resolvedContext))
                    {
                        _remoteContexts.Remove(uri);
                        result = resolvedContext;
                        continue;
                    }

                    // 3.2.3) Otherwise, dereference context,
                    JToken remoteContext;
                    try
                    {
                        remoteContext = await Options.GetContextAsync(uri, cancellationToken).ConfigureAwait(false);
                    }
                    catch (OperationCanceledException) { throw; }
                    catch (Exception e)
                    {
                        throw new LinkedDataException(LinkedDataErrorCode.LoadingRemoteContextFailed, e);
                    }

                    if (!(remoteContext is JObject o) || !o.TryGetValue(LinkedDataKeywords.Context, out remoteContext))
                        throw new LinkedDataException(LinkedDataErrorCode.InvalidRemoteContext);

                    // 3.2.4) Set result to the result of recursively calling this algorithm, passing
                    //        result for active context, context for local context, and a copy of remote
                    //        contexts.
                    result = await ParseAsync(remoteContext, cancellationToken).ConfigureAwait(false);
                    _remoteContexts.Remove(uri);
                    continue;
                }

                // 3.3) If context is not a JSON object, an invalid local context error has
                //      been detected and processing is aborted.
                if (!context.Is(JTokenType.Object))
                    throw new LinkedDataException(LinkedDataErrorCode.InvalidLocalContext);

                var contextObject = (JObject)context;

                // 3.4) If context has an @base key and remote contexts is empty, i.e., the
                //      currently being processed context is not a remote context:
                if (_remoteContexts.Count == 0 &&
                    contextObject.TryGetValue(LinkedDataKeywords.Base, out var baseToken))
                {
                    if (!baseToken.Is(JTokenType.String))
                        throw new LinkedDataException(LinkedDataErrorCode.InvalidBaseIri);

                    var value = (string)baseToken;

                    // 3.4.2) If value is null, remove the base IRI of result.
                    if (value == null) { }
                    // 3.4.3) Otherwise, if value is an absolute IRI, the base IRI of
                    //        result is set to value.
                    else if (Uri.IsWellFormedUriString(value, UriKind.Absolute)) { }
                    // 3.4.4) Otherwise, if value is a relative IRI and the base IRI
                    //        of result is not null, set the base IRI of result to the
                    //        result of resolving value against the current base IRI of
                    //        result.
                    else if (Uri.IsWellFormedUriString(value, UriKind.Relative) && result.Base != null)
                        value = Utils.Resolve(result.Base, value);
                    else throw new LinkedDataException(LinkedDataErrorCode.InvalidBaseIri);

                    result = new LinkedDataContext(
                        _remoteContexts,
                        _resolvedContexts,
                        result._terms,
                        result.Options,
                        value,
                        result.Vocabulary,
                        result.Language);
                }

                // 3.5) If context has an @vocab key:
                if (contextObject.TryGetValue(LinkedDataKeywords.Vocab, out var vocabToken))
                {
                    if (!vocabToken.Is(JTokenType.String))
                        throw new LinkedDataException(LinkedDataErrorCode.InvalidVocabMapping);

                    var value = (string)vocabToken;
                    // 3.5.2) If value is null, remove any vocabulary mapping from result.
                    if (value == null) { }
                    // 3.5.3) Otherwise, if value is an absolute IRI or blank node identifier,
                    //        the vocabulary mapping of result is set to value.
                    else if (value.StartsWith("_:", StringComparison.Ordinal) ||
                        Uri.IsWellFormedUriString(value, UriKind.Absolute)) { }
                    else throw new LinkedDataException(LinkedDataErrorCode.InvalidVocabMapping);

                    result = new LinkedDataContext(
                        _remoteContexts,
                        _resolvedContexts,
                        result._terms,
                        result.Options,
                        result.Base,
                        value,
                        result.Language);
                }

                // 3.6) If context has an @language key:
                if (contextObject.TryGetValue(LinkedDataKeywords.Language, out var langToken))
                {
                    if (!langToken.Is(JTokenType.String))
                        throw new LinkedDataException(LinkedDataErrorCode.InvalidDefaultLanguage);

#                   pragma warning disable CA1308 // Normalize strings to uppercase
                    var value = (string)langToken;
                    if (value == null) { }
                    else value = value.ToLowerInvariant();
#                   pragma warning restore CA1308 // Normalize strings to uppercase

                    result = new LinkedDataContext(
                        _remoteContexts,
                        _resolvedContexts,
                        result._terms,
                        result.Options,
                        result.Base,
                        result.Vocabulary,
                        value);
                }

                result.AddTerms(result, contextObject);
            }

            return result;
        }
    }

#   pragma warning restore CA1815 // Override equals and operator equals on value types
}
