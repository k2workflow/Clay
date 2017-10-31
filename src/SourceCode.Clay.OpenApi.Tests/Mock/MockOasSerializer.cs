#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using SourceCode.Clay.OpenApi.Serialization;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Json;
using System.Text;

namespace SourceCode.Clay.OpenApi.Tests.Mock
{
#pragma warning disable S1168 // Empty arrays and collections should be returned instead of null

    public class MockOasSerializer : OasSerializer
    {
        protected static class GatewayPropertyConstants
        {
            public const string OperationId = "x-k2-operation-id";
        }

        public MockOasSerializer()
        {
        }

        protected virtual JsonValue SerializeMockOperation(MockOasOperation mockOperation)
        {
            if (mockOperation is null) return null;

            var json = (JsonObject)SerializeOperation(mockOperation);

            if (mockOperation.OperationId.HasValue)
                json[GatewayPropertyConstants.OperationId] = mockOperation.OperationId.Value.ToString("x16", CultureInfo.InvariantCulture);

            return json;
        }
    }

#pragma warning restore S1168 // Empty arrays and collections should be returned instead of null
}
