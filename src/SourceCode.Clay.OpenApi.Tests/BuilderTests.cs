#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using SourceCode.Clay.OpenApi.Expressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Reflection;
using System.Text;
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
            TestBuilder(new ApiKeySecuritySchemeBuilder()
            {
                Description = "Description",
                Location = ParameterLocation.Header,
                Name = "ParamName"
            });
        }

        [Fact(DisplayName = nameof(CallbackBuilder_Build))]
        public static void CallbackBuilder_Build()
        {
            var cbb = new CallbackBuilder()
            {
                [CompoundExpression.Parse("http://test/{$statusCode}")] = new Referable<Path>("#/test"),
                [CompoundExpression.Parse("http://test/1/{$statusCode}")] = new Referable<Path>("#/test/1"),
            };
            var cb = cbb.Build();
            var rcb = new CallbackBuilder(cb);

            Assert.Equal(cbb, cb);
            Assert.Equal(cbb, rcb);
        }

        [Fact(DisplayName = nameof(ContactBuilder_Build))]
        public static void ContactBuilder_Build()
        {
            TestBuilder(new ContactBuilder()
            {
                Name = "Jonathan",
                Url = new Uri("http://example.org/jonathan"),
                Email = new MailAddress("jonathan@example.org")
            });
        }

        [Fact(DisplayName = nameof(ComponentsBuilder_Build))]
        public static void ComponentsBuilder_Build()
        {
            TestBuilder(new ComponentsBuilder()
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
            var schemas = new Dictionary<string, Referable<Schema>>()
            {
                ["schema1"] = "http://example.org/schema1"
            };

            TestBuilder(new DocumentBuilder()
            {
                Version = new SemanticVersion(1, 0, 0),
                Info = new Information("Example API", default, default, default, default, default),
                Servers = { new Server(default, "Server 1", default) },
                Paths = { ["/test"] = "#/components/path0" },
                Components = new Components(schemas, default, default, default, default, default, default, default, default),
                Security = { new Referable<SecurityScheme>(new HttpSecurityScheme("Http", default, default)) },
                Tags = { new Tag("Tag1", default, default) },
                ExternalDocumentation = new ExternalDocumentation("External docs", default)
            });
        }

        [Fact(DisplayName = nameof(ExampleBuilder_Build))]
        public static void ExampleBuilder_Build()
        {
            TestBuilder(new ExampleBuilder()
            {
                Summary = "Summary",
                Description = "Description",
                ExternalValue = new Uri("http://example.org/docs")
            });
        }

        [Fact(DisplayName = nameof(ExternalDocumentationBuilder_Build))]
        public static void ExternalDocumentationBuilder_Build()
        {
            TestBuilder(new ExternalDocumentationBuilder()
            {
                Description = "Description",
                Url = new Uri("http://example.org/docs")
            });
        }

        [Fact(DisplayName = nameof(HttpSecuritySchemeBuilder_Build))]
        public static void HttpSecuritySchemeBuilder_Build()
        {
            TestBuilder(new HttpSecuritySchemeBuilder()
            {
                Description = "Description",
                Scheme = "Scheme",
                BearerFormat = "Bearer Format"
            });
        }

        [Fact(DisplayName = nameof(InformationBuilder_Build))]
        public static void InformationBuilder_Build()
        {
            TestBuilder(new InformationBuilder()
            {
                Title = "Title",
                Description = "Description",
                TermsOfService = new Uri("http://example.org/tos"),
                Contact = new Contact("Name", default, default),
                License = new License("MIT", default),
                Version = new SemanticVersion(1, 0, 0)
            });
        }

        [Fact(DisplayName = nameof(LicenseBuilder_Build))]
        public static void LicenseBuilder_Build()
        {
            TestBuilder(new LicenseBuilder()
            {
                Name = "MIT",
                Url = new Uri("https://opensource.org/licenses/MIT")
            });
        }

        [Fact(DisplayName = nameof(LinkBuilder_Build))]
        public static void LinkBuilder_Build()
        {
            TestBuilder(new LinkBuilder()
            {
                OperationReference = "http://example.org/linkOp",
                OperationIdentifier = "Operation ID",
                Description = "Description",
                Server = new Server(default, "Description", default)
            });
        }

        [Fact(DisplayName = nameof(MediaTypeBuilder_Build))]
        public static void MediaTypeBuilder_Build()
        {
            TestBuilder(new MediaTypeBuilder()
            {
                Schema = "#/components/schemas/schema1",
                Examples =
                {
                    ["example1"] = "#/components/examples/example1"
                },
                Encoding =
                {
                    ["application/json"] = new PropertyEncoding(
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
            TestBuilder(new OAuth2SecuritySchemeBuilder()
            {
                Description = "Description",
                ImplicitFlow = new OAuthFlow(new Uri("http://example.org/implicit"), default, default, default),
                PasswordFlow = new OAuthFlow(new Uri("http://example.org/password"), default, default, default),
                ClientCredentialsFlow = new OAuthFlow(new Uri("http://example.org/credential"), default, default, default),
                AuthorizationCodeFlow = new OAuthFlow(new Uri("http://example.org/authorization"), default, default, default)
            });
        }

        [Fact(DisplayName = nameof(OAuth2FlowBuilder_Build))]
        public static void OAuth2FlowBuilder_Build()
        {
            TestBuilder(new OAuthFlowBuilder()
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
            TestBuilder(new OpenIdConnectSecuritySchemeBuilder()
            {
                Description = "Description",
                Url = new Uri("http://example.org/openid")
            });
        }

        [Fact(DisplayName = nameof(OperationBuilder_Build))]
        public static void OperationBuilder_Build()
        {
            TestBuilder(new OperationBuilder()
            {
                Tags = { "Tag1" },
                Summary = "Summary",
                Description = "Description",
                ExternalDocumentation = new ExternalDocumentation("Description", default),
                OperationIdentifier = "Operation 1",
                Parameters =
                {
                    [new ParameterKey("param1", ParameterLocation.Header)] = "#/components/parameters/param1"
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
                Options = OperationOptions.Deprecated,
                Security = { new HttpSecurityScheme("Description", default, default) },
                Servers = { new Server(new Uri("http://example.org"), default, default) }
            });
        }

        [Fact(DisplayName = nameof(ParameterBuilder_Build))]
        public static void ParameterBuilder_Build()
        {
            TestBuilder(new ParameterBuilder()
            {
                Name = "Name",
                Location = ParameterLocation.Header,
                Description = "Description",
                Options = ParameterOptions.AllowReserved,
                Style = ParameterStyle.PipeDelimited,
                Schema = "#/components/schemas/schema1",
                Examples =
                {
                    [new ContentType("application/json")] = "#/components/examples/example1"
                },
                Content =
                {
                    [new ContentType("application/json")] = new MediaType(
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
            TestBuilder(new ParameterBodyBuilder()
            {
                Description = "Description",
                Options = ParameterOptions.AllowReserved,
                Style = ParameterStyle.PipeDelimited,
                Schema = "#/components/schemas/schema1",
                Examples =
                {
                    [new ContentType("application/json")] = "#/components/examples/example1"
                },
                Content =
                {
                    [new ContentType("application/json")] = new MediaType(
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
            TestBuilder(new PathBuilder()
            {
                Summary = "Summary",
                Description = "Description",
                Get = new Operation(default, "Get", default, default, default, default, default, default, default, default, default, default),
                Put = new Operation(default, "Put", default, default, default, default, default, default, default, default, default, default),
                Post = new Operation(default, "Post", default, default, default, default, default, default, default, default, default, default),
                Delete = new Operation(default, "Delete", default, default, default, default, default, default, default, default, default, default),
                Options = new Operation(default, "Options", default, default, default, default, default, default, default, default, default, default),
                Head = new Operation(default, "Head", default, default, default, default, default, default, default, default, default, default),
                Patch = new Operation(default, "Patch", default, default, default, default, default, default, default, default, default, default),
                Trace = new Operation(default, "Trace", default, default, default, default, default, default, default, default, default, default),
                Servers = { new Server(new Uri("http://example.org"), default, default) },
                Parameters =
                {
                    [new ParameterKey("Param1", default)] = "#/components/parameters/param1"
                }
            });
        }

        [Fact(DisplayName = nameof(PropertyEncodingBuilder_Build))]
        public static void PropertyEncodingBuilder_Build()
        {
            TestBuilder(new PropertyEncodingBuilder()
            {
                ContentType = new ContentType("application/json"),
                Headers = {
                    ["header1"] = "#/components/parameters/param1"
                },
                Style = ParameterStyle.PipeDelimited,
                Options = PropertyEncodingOptions.Explode
            });
        }

        [Fact(DisplayName = nameof(RequestBodyBuilder_Build))]
        public static void RequestBodyBuilder_Build()
        {
            TestBuilder(new RequestBodyBuilder()
            {
                Description = "Description",
                Content =
                {
                    [new ContentType("application/json")] = new MediaType("#/components/schemas/schema1")
                },
                Options = RequestBodyOptions.Required
            });
        }

        [Fact(DisplayName = nameof(ResponseBuilder_Build))]
        public static void ResponseBuilder_Build()
        {
            TestBuilder(new ResponseBuilder()
            {
                Description = "Description",
                Headers =
                {
                    ["header1"] = "#/components/parameters/param1"
                },
                Content =
                {
                    [new ContentType("application/json")] = new MediaType("#/components/schemas/schema1")
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
            TestBuilder(new SchemaBuilder()
            {
                Type = SchemaType.String,
                Format = "uuid",
                Title = "Unique Identifier",
                Description = "Description",
                NumberRange = new NumberRange(1, 100),
                ItemsRange = new CountRange(2, 200),
                LengthRange = new CountRange(3, 300),
                PropertiesRange = new CountRange(4, 400),
                Options = SchemaOptions.Required,
                Pattern = "[a-z]",
                Enum = { new ScalarValue(true) },
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
                ExternalDocumentation = new ExternalDocumentation("Description", default)
            });
        }

        [Fact(DisplayName = nameof(ServerBuilder_Build))]
        public static void ServerBuilder_Build()
        {
            TestBuilder(new ServerBuilder()
            {
                Url = new Uri("http://example.org"),
                Description = "Description",
                Variables =
                {
                    ["var1"] = new ServerVariable(description: "var1")
                }
            });
        }

        [Fact(DisplayName = nameof(ServerVariableBuilder_Build))]
        public static void ServerVariableBuilder_Build()
        {
            TestBuilder(new ServerVariableBuilder()
            {
                Enum = { "Val1" },
                Default = "Val1",
                Description = "Variable1"
            });
        }

        [Fact(DisplayName = nameof(TagBuilder_Build))]
        public static void TagBuilder_Build()
        {
            TestBuilder(new TagBuilder()
            {
                Name = "Name",
                Description = "Description",
                ExternalDocumentation = new ExternalDocumentation("Description", default)
            });
        }

        #endregion
    }
}
