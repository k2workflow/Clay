#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace SourceCode.Clay.OpenApi.Tests.Mock
{
    public class MockOasOperationBuilder : OasOperationBuilder, IOasBuilder<MockOasOperation>
    {
        #region Properties

        public ulong? OperationId { get; set; }

        #endregion

        #region Constructors

        public MockOasOperationBuilder()
            : base()
        {
        }

        /// <summary>
        /// Creates a new instance of the <see cref="OasOperationBuilder"/> class.
        /// </summary>
        /// <param name="value">The <see cref="OasOperation"/> to copy values from.</param>
        public MockOasOperationBuilder(MockOasOperation value)
            : base(value)
        {
            OperationId = value.OperationId;
        }

        #endregion

        #region Methods

        public override OasOperation Build() => new MockOasOperation(
            tags: new ReadOnlyCollection<string>(Tags),
            summary: Summary,
            description: Description,
            externalDocumentation: ExternalDocumentation,
            operationIdentifier: OperationIdentifier,
            parameters: new ReadOnlyDictionary<OasParameterKey, OasReferable<OasParameterBody>>(Parameters),
            requestBody: RequestBody,
            responses: new ReadOnlyDictionary<OasResponseKey, OasReferable<OasResponse>>(Responses),
            callbacks: new ReadOnlyDictionary<string, OasReferable<OasCallback>>(Callbacks),
            options: Options,
            security: new ReadOnlyCollection<OasSecurityScheme>(Security),
            servers: new ReadOnlyCollection<OasServer>(Servers),
            operationId: OperationId);

        MockOasOperation IOasBuilder<MockOasOperation>.Build() => (MockOasOperation)Build();

        #endregion
    }
}
