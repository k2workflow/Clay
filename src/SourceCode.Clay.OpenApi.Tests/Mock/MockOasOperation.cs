#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using System;
using System.Collections.Generic;

namespace SourceCode.Clay.OpenApi.Tests.Mock
{
    public class MockOasOperation : OasOperation, IEquatable<MockOasOperation>
    {
        public ulong? OperationId { get; }

        public MockOasOperation(
            IReadOnlyList<string> tags = default,
            string summary = default,
            string description = default,
            OasExternalDocumentation externalDocumentation = default,
            string operationIdentifier = default,
            IReadOnlyDictionary<OasParameterKey, OasReferable<OasParameterBody>> parameters = default,
            OasReferable<OasRequestBody> requestBody = default,
            IReadOnlyDictionary<OasResponseKey, OasReferable<OasResponse>> responses = default,
            IReadOnlyDictionary<string, OasReferable<OasCallback>> callbacks = default,
            OasOperationOptions options = default,
            IReadOnlyList<OasSecurityScheme> security = default,
            IReadOnlyList<OasServer> servers = default,
            ulong? operationId = default)
            : base(tags, summary, description, externalDocumentation, operationIdentifier, parameters, requestBody, responses, callbacks, options, security, servers)
        {
            OperationId = operationId;
        }

        public static bool operator ==(MockOasOperation operation1, MockOasOperation operation2)
        {
            if (operation1 is null) return operation2 is null;
            return operation1.Equals(operation2);
        }

        public static bool operator !=(MockOasOperation operation1, MockOasOperation operation2)
            => !(operation1 == operation2);

        public override bool Equals(object obj) => Equals(obj as MockOasOperation);

        public bool Equals(MockOasOperation other)
        {
            return !(other is null) &&
                   base.Equals(other) &&
                   OperationId == other.OperationId;
        }

        public override int GetHashCode()
        {
            var hashCode = 209929755;
            hashCode = hashCode * -1521134295 + base.GetHashCode();
            hashCode = hashCode * -1521134295 + OperationId.GetHashCode();
            return hashCode;
        }
    }
}
