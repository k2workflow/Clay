#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using SourceCode.Clay.OpenApi.Expressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Reflection;
using Xunit;

namespace SourceCode.Clay.OpenApi.Tests
{
    public static class BuilderTests
    {
        #region Methods

        private static void TestBuilder(object builder)
        {
            var builderType = builder.GetType();
            var buildMethod = builderType.GetMethod("Build", BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance);
            var builtType = buildMethod.ReturnType;
            var built = buildMethod.Invoke(builder, new object[0]);
            var reverseBuilder = builderType.GetConstructors().First(x => x.GetParameters().Length == 1).Invoke(new object[] { built });

            foreach (var builderProp in builderType.GetProperties())
            {
                var builtProp = builtType.GetProperty(builderProp.Name);

                var builderValue = builderProp.GetValue(builder);
                var builtValue = builtProp.GetValue(built);
                var reverseValue = builderProp.GetValue(reverseBuilder);

                Assert.Equal(builderValue, builtValue);
                Assert.Equal(builtValue, reverseValue);
            }
        }

        #endregion

        #region Build

        [Fact(DisplayName = nameof(ApiKeySecuritySchemeBuilder_Build))]
        public static void ApiKeySecuritySchemeBuilder_Build()
        {
            TestBuilder(new OasApiKeySecuritySchemeBuilder()
            {
                Description = "Description",
                Location = OasParameterLocation.Header,
                Name = "ParamName"
            });
        }

        [Fact(DisplayName = nameof(CallbackBuilder_Build))]
        public static void CallbackBuilder_Build()
        {
            var cbb = new OasCallbackBuilder()
            {
                [OasExpression.Parse("http://test/{$statusCode}")] = new OasReferable<OasPath>("#/test"),
                [OasExpression.Parse("http://test/1/{$statusCode}")] = new OasReferable<OasPath>("#/test/1"),
            };
            var cb = cbb.Build();
            var rcb = new OasCallbackBuilder(cb);

            Assert.Equal(cbb, cb);
            Assert.Equal(cbb, rcb);
        }

        [Fact(DisplayName = nameof(ContactBuilder_Build))]
        public static void ContactBuilder_Build()
        {
            TestBuilder(new OasContactBuilder()
            {
                Name = "Jonathan",
                Url = new Uri("http://example.org/jonathan"),
                Email = new MailAddress("jonathan@example.org")
            });
        }

        [Fact(DisplayName = nameof(ComponentsBuilder_Build))]
        public static void ComponentsBuilder_Build()
        {
            TestBuilder(new OasComponentsBuilder()
            {
                Callbacks =
                {
                    ["cb1"] = "#/components/callbacks/1"
                },
                Examples =
                {
                    ["ex1"] = "#/components/examples/1"
                },
                Headers =
                {
                    ["hd1"] = "#/components/headers/1"
                },
                Links =
                {
                    ["li1"] = "#/components/links/1"
                },
                Parameters =
                {
                    ["pa1"] = "#/components/parameters/1"
                },
                RequestBodies =
                {
                    ["req1"] = "#/components/requestBodies/1"
                },
                Responses =
                {
                    ["res1"] = "#/components/responses/1"
                },
                Schemas =
                {
                    ["sch1"] = "#/components/schemas/1"
                },
                SecuritySchemes =
                {
                    ["sec1"] = "#/components/securitySchemas/1"
                }
            });
        }

        [Fact(DisplayName = nameof(DocumentBuilder_Build))]
        public static void DocumentBuilder_Build()
        {
            var schemas = new Dictionary<string, OasReferable<OasSchema>>()
            {
                ["schema1"] = "http://example.org/schema1"
            };

            TestBuilder(new OasDocumentBuilder()
            {
                Version = new SemanticVersion(1, 0, 0),
                Info = new OasInformation("Example API", default, default, default, default, default),
                Servers = { new OasServer(default, "Server 1", default) },
                Paths = { ["/test"] = "#/components/path0" },
                Components = new OasComponents(schemas, default, default, default, default, default, default, default, default),
                Security = { new OasReferable<OasSecurityScheme>(new OasHttpSecurityScheme("Http", default, default)) },
                Tags = { new OasTag("Tag1", default, default) },
                ExternalDocumentation = new OasExternalDocumentation("External docs", default)
            });
        }

        [Fact(DisplayName = nameof(ExampleBuilder_Build))]
        public static void ExampleBuilder_Build()
        {
            TestBuilder(new OasExampleBuilder()
            {
                Summary = "Summary",
                Description = "Description",
                ExternalValue = new Uri("http://example.org/docs")
            });
        }

        [Fact(DisplayName = nameof(ExternalDocumentationBuilder_Build))]
        public static void ExternalDocumentationBuilder_Build()
        {
            TestBuilder(new OasExternalDocumentationBuilder()
            {
                Description = "Description",
                Url = new Uri("http://example.org/docs")
            });
        }

        [Fact(DisplayName = nameof(HttpSecuritySchemeBuilder_Build))]
        public static void HttpSecuritySchemeBuilder_Build()
        {
            TestBuilder(new OasHttpSecuritySchemeBuilder()
            {
                Description = "Description",
                Scheme = "Scheme",
                BearerFormat = "Bearer Format"
            });
        }

        [Fact(DisplayName = nameof(InformationBuilder_Build))]
        public static void InformationBuilder_Build()
        {
            TestBuilder(new OasInformationBuilder()
            {
                Title = "Title",
                Description = "Description",
                TermsOfService = new Uri("http://example.org/tos"),
                Contact = new OasContact("Name", default, default),
                License = new OasLicense("MIT", default),
                Version = new SemanticVersion(1, 0, 0)
            });
        }

        [Fact(DisplayName = nameof(LicenseBuilder_Build))]
        public static void LicenseBuilder_Build()
        {
            TestBuilder(new OasLicenseBuilder()
            {
                Name = "MIT",
                Url = new Uri("https://opensource.org/licenses/MIT")
            });
        }

        [Fact(DisplayName = nameof(LinkBuilder_Build))]
        public static void LinkBuilder_Build()
        {
            TestBuilder(new OasLinkBuilder()
            {
                OperationReference = "http://example.org/linkOp",
                OperationIdentifier = "Operation ID",
                Description = "Description",
                Server = new OasServer(default, "Description", default)
            });
        }

        [Fact(DisplayName = nameof(MediaTypeBuilder_Build))]
        public static void MediaTypeBuilder_Build()
        {
            TestBuilder(new OasMediaTypeBuilder()
            {
                Schema = "#/components/schemas/schema1",
                Examples =
                {
                    ["example1"] = "#/components/examples/example1"
                },
                Encoding =
                {
                    ["application/json"] = new OasPropertyEncoding(
                        new ContentType("application/json"),
                        default,
                        default,
                        default
                    )
                }
            });
        }

        [Fact(DisplayName = nameof(OAuth2SecuritySchemeBuilder_Build))]
        public static void OAuth2SecuritySchemeBuilder_Build()
        {
            TestBuilder(new OasOAuth2SecuritySchemeBuilder()
            {
                Description = "Description",
                ImplicitFlow = new OasOAuthFlow(new Uri("http://example.org/implicit"), default, default, default),
                PasswordFlow = new OasOAuthFlow(new Uri("http://example.org/password"), default, default, default),
                ClientCredentialsFlow = new OasOAuthFlow(new Uri("http://example.org/credential"), default, default, default),
                AuthorizationCodeFlow = new OasOAuthFlow(new Uri("http://example.org/authorization"), default, default, default)
            });
        }

        [Fact(DisplayName = nameof(OAuth2FlowBuilder_Build))]
        public static void OAuth2FlowBuilder_Build()
        {
            TestBuilder(new OasOAuthFlowBuilder()
            {
                AuthorizationUrl = new Uri("http://example.org/authorization"),
                TokenUrl = new Uri("http://example.org/token"),
                RefreshUrl = new Uri("http://example.org/refresh"),
                Scopes =
                {
                    ["user:name"] = "Get the user name"
                }
            });
        }

        [Fact(DisplayName = nameof(OpenIdConnectSecuritySchemeBuilder_Build))]
        public static void OpenIdConnectSecuritySchemeBuilder_Build()
        {
            TestBuilder(new OasOpenIdConnectSecuritySchemeBuilder()
            {
                Description = "Description",
                Url = new Uri("http://example.org/openid")
            });
        }

        [Fact(DisplayName = nameof(OperationBuilder_Build))]
        public static void OperationBuilder_Build()
        {
            TestBuilder(new OasOperationBuilder()
            {
                Tags = { "Tag1" },
                Summary = "Summary",
                Description = "Description",
                ExternalDocumentation = new OasExternalDocumentation("Description", default),
                OperationIdentifier = "Operation 1",
                Parameters =
                {
                    [new OasParameterKey("param1", OasParameterLocation.Header)] = "#/components/parameters/param1"
                },
                RequestBody = "#/components/requestBodies/rb1",
                Responses =
                {
                    [HttpStatusCode.InternalServerError] = "#/components/responses/error"
                },
                Callbacks =
                {
                    ["result"] = "#/components/callbacks/cb1"
                },
                Options = OasOperationOptions.Deprecated,
                Security = { new OasHttpSecurityScheme("Description", default, default) },
                Servers = { new OasServer(new Uri("http://example.org"), default, default) }
            });
        }

        [Fact(DisplayName = nameof(ParameterBuilder_Build))]
        public static void ParameterBuilder_Build()
        {
            TestBuilder(new OasParameterBuilder()
            {
                Name = "Name",
                Location = OasParameterLocation.Header,
                Description = "Description",
                Options = OasParameterOptions.AllowReserved,
                Style = OasParameterStyle.PipeDelimited,
                Schema = "#/components/schemas/schema1",
                Examples =
                {
                    [new ContentType("application/json")] = "#/components/examples/example1"
                },
                Content =
                {
                    [new ContentType("application/json")] = new OasMediaType(
                        "#/components/schemas/schema1",
                        default,
                        default
                    )
                }
            });
        }

        [Fact(DisplayName = nameof(ParameterBodyBuilder_Build))]
        public static void ParameterBodyBuilder_Build()
        {
            TestBuilder(new OasParameterBodyBuilder()
            {
                Description = "Description",
                Options = OasParameterOptions.AllowReserved,
                Style = OasParameterStyle.PipeDelimited,
                Schema = "#/components/schemas/schema1",
                Examples =
                {
                    [new ContentType("application/json")] = "#/components/examples/example1"
                },
                Content =
                {
                    [new ContentType("application/json")] = new OasMediaType(
                        "#/components/schemas/schema1",
                        default,
                        default
                    )
                }
            });
        }

        [Fact(DisplayName = nameof(PathBuilder_Build))]
        public static void PathBuilder_Build()
        {
            TestBuilder(new OasPathBuilder()
            {
                Summary = "Summary",
                Description = "Description",
                Get = new OasOperation(default, "Get", default, default, default, default, default, default, default, default, default, default),
                Put = new OasOperation(default, "Put", default, default, default, default, default, default, default, default, default, default),
                Post = new OasOperation(default, "Post", default, default, default, default, default, default, default, default, default, default),
                Delete = new OasOperation(default, "Delete", default, default, default, default, default, default, default, default, default, default),
                Options = new OasOperation(default, "Options", default, default, default, default, default, default, default, default, default, default),
                Head = new OasOperation(default, "Head", default, default, default, default, default, default, default, default, default, default),
                Patch = new OasOperation(default, "Patch", default, default, default, default, default, default, default, default, default, default),
                Trace = new OasOperation(default, "Trace", default, default, default, default, default, default, default, default, default, default),
                Servers = { new OasServer(new Uri("http://example.org"), default, default) },
                Parameters =
                {
                    [new OasParameterKey("Param1", default)] = "#/components/parameters/param1"
                }
            });
        }

        [Fact(DisplayName = nameof(PropertyEncodingBuilder_Build))]
        public static void PropertyEncodingBuilder_Build()
        {
            TestBuilder(new OasPropertyEncodingBuilder()
            {
                ContentType = new ContentType("application/json"),
                Headers = {
                    ["header1"] = "#/components/parameters/param1"
                },
                Style = OasParameterStyle.PipeDelimited,
                Options = OasPropertyEncodingOptions.Explode
            });
        }

        [Fact(DisplayName = nameof(RequestBodyBuilder_Build))]
        public static void RequestBodyBuilder_Build()
        {
            TestBuilder(new OasRequestBodyBuilder()
            {
                Description = "Description",
                Content =
                {
                    [new ContentType("application/json")] = new OasMediaType("#/components/schemas/schema1")
                },
                Options = OasRequestBodyOptions.Required
            });
        }

        [Fact(DisplayName = nameof(ResponseBuilder_Build))]
        public static void ResponseBuilder_Build()
        {
            TestBuilder(new OasResponseBuilder()
            {
                Description = "Description",
                Headers =
                {
                    ["header1"] = "#/components/parameters/param1"
                },
                Content =
                {
                    [new ContentType("application/json")] = new OasMediaType("#/components/schemas/schema1")
                },
                Links =
                {
                    ["link"] = "#/components/links/link1"
                }
            });
        }

        [Fact(DisplayName = nameof(SchemaBuilder_Build))]
        public static void SchemaBuilder_Build()
        {
            TestBuilder(new OasSchemaBuilder()
            {
                JsonType = OasSchemaType.String,
                Format = "uuid",
                Title = "Unique Identifier",
                Description = "Description",
                NumberRange = new OasNumberRange(1, 100),
                ItemsRange = new OasCountRange(2, 200),
                LengthRange = new OasCountRange(3, 300),
                PropertiesRange = new OasCountRange(4, 400),
                Options = OasSchemaOptions.Required,
                Pattern = "[a-z]",
                Enum = { new OasScalarValue(true) },
                AllOf = { "#/components/schemas/allOf" },
                OneOf = { "#/components/schemas/oneOf" },
                AnyOf = { "#/components/schemas/anyOf" },
                Not = { "#/components/schemas/not" },
                Items = "#/components/schemas/items",
                Properties =
                {
                    ["prop1"] = "#/components/schemas/prop1"
                },
                AdditionalProperties =
                {
                    ["prop1"] = "#/components/schemas/adprop1"
                },
                ExternalDocumentation = new OasExternalDocumentation("Description", default)
            });
        }

        [Fact(DisplayName = nameof(ServerBuilder_Build))]
        public static void ServerBuilder_Build()
        {
            TestBuilder(new OasServerBuilder()
            {
                Url = new Uri("http://example.org"),
                Description = "Description",
                Variables =
                {
                    ["var1"] = new OasServerVariable(description: "var1")
                }
            });
        }

        [Fact(DisplayName = nameof(ServerVariableBuilder_Build))]
        public static void ServerVariableBuilder_Build()
        {
            TestBuilder(new OasServerVariableBuilder()
            {
                Enum = { "Val1" },
                Default = "Val1",
                Description = "Variable1"
            });
        }

        [Fact(DisplayName = nameof(TagBuilder_Build))]
        public static void TagBuilder_Build()
        {
            TestBuilder(new OasTagBuilder()
            {
                Name = "Name",
                Description = "Description",
                ExternalDocumentation = new OasExternalDocumentation("Description", default)
            });
        }

        #endregion
    }
}
