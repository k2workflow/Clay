#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SourceCode.Clay.Json.LinkedData
{
#   pragma warning disable CA2227 // Collection properties should be read only
#   pragma warning disable CA1815 // Override equals and operator equals on value types

    public class LinkedDataOptions
    {
        protected internal string Base { get; }
        protected internal LinkedDataContext ExpandContext { get; }

        public LinkedDataOptions(string @base = null)
            : this(@base, default)
        { }

        protected LinkedDataOptions(
            string @base = null,
            LinkedDataContext expandContext = default)
        {
            Base = @base;
            ExpandContext = expandContext.HasValue ? expandContext : new LinkedDataContext(this);
        }

        public virtual async ValueTask<LinkedDataOptions> WithContextAsync(
            JToken localContext,
            CancellationToken cancellationToken = default)
        {
            var expanded = await CreateContextAsync(localContext, cancellationToken).ConfigureAwait(false);
            return new LinkedDataOptions(Base, expanded);
        }

        protected async ValueTask<LinkedDataContext> CreateContextAsync(
            JToken localContext,
            CancellationToken cancellationToken)
        {
            if (!(localContext is JObject o) || !o.TryGetValue(LinkedDataKeywords.Context, out localContext))
                throw new LinkedDataException(LinkedDataErrorCode.InvalidExpandContext);
            var remoteContext = await ExpandContext.ParseAsync(localContext, cancellationToken).ConfigureAwait(false);
            return remoteContext;
        }

        public virtual async ValueTask<JToken> GetContextAsync(string iri, CancellationToken cancellationToken)
        {
            var wr = WebRequest.CreateHttp(new Uri(iri));
            wr.Method = "GET";

            using (var response = await wr.GetResponseAsync().ConfigureAwait(false))
            using (var responseStream = response.GetResponseStream())
            using (var streamReader = new StreamReader(responseStream))
            using (var jsonReader = new JsonTextReader(streamReader))
            {
                return await JToken.LoadAsync(jsonReader, cancellationToken).ConfigureAwait(false);
            }
        }
    }

#   pragma warning restore CA1815 // Override equals and operator equals on value types
#   pragma warning restore CA2227 // Collection properties should be read only
}
