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
        public string Base { get; }
        public LinkedDataContext ExpandContext { get; }
        public bool? Embed { get; }
        public bool? Explicit { get; }
        public bool? OmitDefault { get; }

        public LinkedDataOptions(
            string @base = null,
            LinkedDataContext expandContext = default,
            bool? embed = null,
            bool? @explicit = null,
            bool? omitDefault = null)
        {
            Base = @base;
            Embed = embed;
            Explicit = @explicit;
            OmitDefault = omitDefault;
            ExpandContext = expandContext.HasValue ? expandContext : new LinkedDataContext(this);
        }

        public virtual async ValueTask<JToken> GetContextAsync(string iri, CancellationToken cancellationToken)
        {
            var wr = WebRequest.CreateHttp(new Uri(iri));
            wr.Method = "GET";

            using (var response = await wr.GetResponseAsync().ConfigureAwait(false))
            using (var responseStream = await wr.GetRequestStreamAsync().ConfigureAwait(false))
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
