#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using Newtonsoft.Json.Linq;
using SourceCode.Clay.OpenApi.Serialization;
using System.Globalization;

namespace SourceCode.Clay.OpenApi.Tests.Mock
{
    public class MockOasSerializer : OasSerializer
    {
        protected virtual JToken SerializeMockOperation(MockOasOperation mockOperation)
        {
            if (mockOperation is null) return null;

            var json = (JObject)SerializeOperation(mockOperation);

            if (mockOperation.OperationId.HasValue)
                json[GatewayPropertyConstants.OperationId] = mockOperation.OperationId.Value.ToString("x16", CultureInfo.InvariantCulture);

            return json;
        }

        protected static class GatewayPropertyConstants
        {
            public const string OperationId = "x-k2-operation-id";
        }

        public MockOasSerializer()
        {
        }
    }
}
