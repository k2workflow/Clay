#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

namespace SourceCode.Clay.OpenApi.Serialization
{
    partial class OpenApiSerializer
    {
        #region Constants

        /// <summary>
        /// Represents constants for OpenAPI enum values.
        /// </summary>
        protected static class EnumConstants
        {
            #region Fields

            /// <summary>
            /// Gets a constant containing "matrix".
            /// </summary>
            public const string Matrix = "matrix";

            /// <summary>
            /// Gets a constant containing "label".
            /// </summary>
            public const string Label = "label";

            /// <summary>
            /// Gets a constant containing "form".
            /// </summary>
            public const string Form = "form";

            /// <summary>
            /// Gets a constant containing "simple".
            /// </summary>
            public const string Simple = "simple";

            /// <summary>
            /// Gets a constant containing "spaceDelimited".
            /// </summary>
            public const string SpaceDelimited = "spaceDelimited";

            /// <summary>
            /// Gets a constant containing "pipeDelimited".
            /// </summary>
            public const string PipeDelimited = "pipeDelimited";

            /// <summary>
            /// Gets a constant containing "deepObject".
            /// </summary>
            public const string DeepObject = "deepObject";

            /// <summary>
            /// Gets a constant containing "query".
            /// </summary>
            public const string Query = "query";

            /// <summary>
            /// Gets a constant containing "header".
            /// </summary>
            public const string Header = "header";

            /// <summary>
            /// Gets a constant containing "path".
            /// </summary>
            public const string Path = "path";

            /// <summary>
            /// Gets a constant containing "cookie".
            /// </summary>
            public const string Cookie = "cookie";

            /// <summary>
            /// Gets a constant containing "array".
            /// </summary>
            public const string Array = "array";

            /// <summary>
            /// Gets a constant containing "boolean".
            /// </summary>
            public const string Boolean = "boolean";

            /// <summary>
            /// Gets a constant containing "number".
            /// </summary>
            public const string Number = "number";

            /// <summary>
            /// Gets a constant containing "object".
            /// </summary>
            public const string Object = "object";

            /// <summary>
            /// Gets a constant containing "string".
            /// </summary>
            public const string String = "string";

            /// <summary>
            /// Gets a constant containing "integer".
            /// </summary>
            public const string Integer = "integer";

            /// <summary>
            /// Gets a constant containing "apiKey".
            /// </summary>
            public const string ApiKey = "apiKey";

            /// <summary>
            /// Gets a constant containing "http".
            /// </summary>
            public const string Http = "http";

            /// <summary>
            /// Gets a constant containing "oauth2".
            /// </summary>
            public const string OAuth2 = "oauth2";

            /// <summary>
            /// Gets a constant containing "openIdConnect".
            /// </summary>
            public const string OpenIdConnect = "openIdConnect";

            #endregion
        }

        /// <summary>
        /// Represents constants for OpenAPI property names.
        /// </summary>
        internal static class PropertyConstants
        {
            #region Fields

            /// <summary>
            /// Gets a constant containing "$ref".
            /// </summary>
            public const string Reference = "$ref";

            /// <summary>
            /// Gets a constant containing "name".
            /// </summary>
            public const string Name = "name";

            /// <summary>
            /// Gets a constant containing "tags".
            /// </summary>
            public const string Tags = "tags";

            /// <summary>
            /// Gets a constant containing "externalDocs".
            /// </summary>
            public const string ExternalDocs = "externalDocs";

            /// <summary>
            /// Gets a constant containing "description".
            /// </summary>
            public const string Description = "description";

            /// <summary>
            /// Gets a constant containing "type".
            /// </summary>
            public const string Type = "type";

            /// <summary>
            /// Gets a constant containing "title".
            /// </summary>
            public const string Title = "title";

            /// <summary>
            /// Gets a constant containing "format".
            /// </summary>
            public const string Format = "format";

            /// <summary>
            /// Gets a constant containing "multipleOf".
            /// </summary>
            public const string MultipleOf = "multipleOf";

            /// <summary>
            /// Gets a constant containing "maximum".
            /// </summary>
            public const string Maximum = "maximum";

            /// <summary>
            /// Gets a constant containing "exclusiveMaximum".
            /// </summary>
            public const string ExclusiveMaximum = "exclusiveMaximum";

            /// <summary>
            /// Gets a constant containing "minimum".
            /// </summary>
            public const string Minimum = "minimum";

            /// <summary>
            /// Gets a constant containing "exclusiveMinimum".
            /// </summary>
            public const string ExclusiveMinimum = "exclusiveMinimum";

            /// <summary>
            /// Gets a constant containing "minLength".
            /// </summary>
            public const string MinLength = "minLength";

            /// <summary>
            /// Gets a constant containing "maxLength".
            /// </summary>
            public const string MaxLength = "maxLength";

            /// <summary>
            /// Gets a constant containing "minProperties".
            /// </summary>
            public const string MinProperties = "minProperties";

            /// <summary>
            /// Gets a constant containing "maxProperties".
            /// </summary>
            public const string MaxProperties = "maxProperties";

            /// <summary>
            /// Gets a constant containing "uniqueItems".
            /// </summary>
            public const string UniqueItems = "uniqueItems";

            /// <summary>
            /// Gets a constant containing "required".
            /// </summary>
            public const string Required = "required";

            /// <summary>
            /// Gets a constant containing "enum".
            /// </summary>
            public const string EnumValue = "enum";

            /// <summary>
            /// Gets a constant containing "allOf".
            /// </summary>
            public const string AllOf = "allOf";

            /// <summary>
            /// Gets a constant containing "anyOf".
            /// </summary>
            public const string AnyOf = "anyOf";

            /// <summary>
            /// Gets a constant containing "oneOf".
            /// </summary>
            public const string OneOf = "oneOf";

            /// <summary>
            /// Gets a constant containing "not".
            /// </summary>
            public const string Not = "not";

            /// <summary>
            /// Gets a constant containing "items".
            /// </summary>
            public const string Items = "items";

            /// <summary>
            /// Gets a constant containing "properties".
            /// </summary>
            public const string Properties = "properties";

            /// <summary>
            /// Gets a constant containing "additionalProperties".
            /// </summary>
            public const string AdditionalProperties = "additionalProperties";

            /// <summary>
            /// Gets a constant containing "nullable".
            /// </summary>
            public const string Nullable = "nullable";

            /// <summary>
            /// Gets a constant containing "deprecated".
            /// </summary>
            public const string Deprecated = "deprecated";

            /// <summary>
            /// Gets a constant containing "pattern".
            /// </summary>
            public const string Pattern = "pattern";

            /// <summary>
            /// Gets a constant containing "schemas".
            /// </summary>
            public const string Schemas = "schemas";

            /// <summary>
            /// Gets a constant containing "schema".
            /// </summary>
            public const string Schema = "schema";

            /// <summary>
            /// Gets a constant containing "responses".
            /// </summary>
            public const string Responses = "responses";

            /// <summary>
            /// Gets a constant containing "parameters".
            /// </summary>
            public const string Parameters = "parameters";

            /// <summary>
            /// Gets a constant containing "examples".
            /// </summary>
            public const string Examples = "examples";

            /// <summary>
            /// Gets a constant containing "requestBodies".
            /// </summary>
            public const string RequestBodies = "requestBodies";

            /// <summary>
            /// Gets a constant containing "requestBody".
            /// </summary>
            public const string RequestBody = "requestBody";

            /// <summary>
            /// Gets a constant containing "headers".
            /// </summary>
            public const string Headers = "headers";

            /// <summary>
            /// Gets a constant containing "securityScheme".
            /// </summary>
            public const string SecuritySchemes = "securitySchemes";

            /// <summary>
            /// Gets a constant containing "links".
            /// </summary>
            public const string Links = "links";

            /// <summary>
            /// Gets a constant containing "callback".
            /// </summary>
            public const string Callbacks = "callbacks";

            /// <summary>
            /// Gets a constant containing "url".
            /// </summary>
            public const string Url = "url";

            /// <summary>
            /// Gets a constant containing "email".
            /// </summary>
            public const string Email = "email";

            /// <summary>
            /// Gets a constant containing "openapi".
            /// </summary>
            public const string OpenApi = "openapi";

            /// <summary>
            /// Gets a constant containing "info".
            /// </summary>
            public const string Info = "info";

            /// <summary>
            /// Gets a constant containing "servers".
            /// </summary>
            public const string Servers = "servers";

            /// <summary>
            /// Gets a constant containing "server".
            /// </summary>
            public const string Server = "server";

            /// <summary>
            /// Gets a constant containing "paths".
            /// </summary>
            public const string Paths = "paths";

            /// <summary>
            /// Gets a constant containing "components".
            /// </summary>
            public const string Components = "components";

            /// <summary>
            /// Gets a constant containing "security".
            /// </summary>
            public const string Security = "security";

            /// <summary>
            /// Gets a constant containing "contentType".
            /// </summary>
            public const string ContentType = "contentType";

            /// <summary>
            /// Gets a constant containing "style".
            /// </summary>
            public const string Style = "style";

            /// <summary>
            /// Gets a constant containing "explode".
            /// </summary>
            public const string Explode = "explode";

            /// <summary>
            /// Gets a constant containing "allowReserved".
            /// </summary>
            public const string AllowReserved = "allowReserved";

            /// <summary>
            /// Gets a constant containing "summary".
            /// </summary>
            public const string Summary = "summary";

            /// <summary>
            /// Gets a constant containing "value".
            /// </summary>
            public const string Value = "value";

            /// <summary>
            /// Gets a constant containing "externalValue".
            /// </summary>
            public const string ExternalValue = "externalValue";

            /// <summary>
            /// Gets a constant containing "termsOfService".
            /// </summary>
            public const string TermsOfService = "termsOfService";

            /// <summary>
            /// Gets a constant containing "contact".
            /// </summary>
            public const string Contact = "contact";

            /// <summary>
            /// Gets a constant containing "license".
            /// </summary>
            public const string License = "license";

            /// <summary>
            /// Gets a constant containing "version".
            /// </summary>
            public const string Version = "version";

            /// <summary>
            /// Gets a constant containing "operationId".
            /// </summary>
            public const string OperationId = "operationId";

            /// <summary>
            /// Gets a constant containing "operationRef".
            /// </summary>
            public const string OperationRef = "operationRef";

            /// <summary>
            /// Gets a constant containing "encoding".
            /// </summary>
            public const string Encoding = "encoding";

            /// <summary>
            /// Gets a constant containing "allowEmptyValue".
            /// </summary>
            public const string AllowEmptyValue = "allowEmptyValue";

            /// <summary>
            /// Gets a constant containing "content".
            /// </summary>
            public const string Content = "content";

            /// <summary>
            /// Gets a constant containing "in".
            /// </summary>
            public const string In = "in";

            /// <summary>
            /// Gets a constant containing "delete".
            /// </summary>
            public const string Delete = "delete";

            /// <summary>
            /// Gets a constant containing "get".
            /// </summary>
            public const string Get = "get";

            /// <summary>
            /// Gets a constant containing "head".
            /// </summary>
            public const string Head = "head";

            /// <summary>
            /// Gets a constant containing "options".
            /// </summary>
            public const string Options = "options";

            /// <summary>
            /// Gets a constant containing "patch".
            /// </summary>
            public const string Patch = "patch";

            /// <summary>
            /// Gets a constant containing "post".
            /// </summary>
            public const string Post = "post";

            /// <summary>
            /// Gets a constant containing "put".
            /// </summary>
            public const string Put = "put";

            /// <summary>
            /// Gets a constant containing "trace".
            /// </summary>
            public const string Trace = "trace";

            /// <summary>
            /// Gets a constant containing "default".
            /// </summary>
            public const string Default = "default";

            /// <summary>
            /// Gets a constant containing "enum".
            /// </summary>
            public const string Enum = "enum";

            /// <summary>
            /// Gets a constant containing "variables".
            /// </summary>
            public const string Variables = "variables";

            /// <summary>
            /// Gets a constant containing "scheme".
            /// </summary>
            public const string Scheme = "scheme";

            /// <summary>
            /// Gets a constant containing "bearerFormat".
            /// </summary>
            public const string BearerFormat = "bearerFormat";

            /// <summary>
            /// Gets a constant containing "flows".
            /// </summary>
            public const string Flows = "flows";

            /// <summary>
            /// Gets a constant containing "implicit".
            /// </summary>
            public const string Implicit = "implicit";

            /// <summary>
            /// Gets a constant containing "password".
            /// </summary>
            public const string Password = "pass" + "word";

            /// <summary>
            /// Gets a constant containing "clientCredentials".
            /// </summary>
            public const string ClientCredentials = "clientCredentials";

            /// <summary>
            /// Gets a constant containing "authorizationCode".
            /// </summary>
            public const string AuthorizationCode = "authorizationCode";

            /// <summary>
            /// Gets a constant containing "authorizationUrl".
            /// </summary>
            public const string AuthorizationUrl = "authorizationUrl";

            /// <summary>
            /// Gets a constant containing "tokenUrl".
            /// </summary>
            public const string TokenUrl = "tokenUrl";

            /// <summary>
            /// Gets a constant containing "refreshUrl".
            /// </summary>
            public const string RefreshUrl = "refreshUrl";

            /// <summary>
            /// Gets a constant containing "scopes".
            /// </summary>
            public const string Scopes = "scopes";

            /// <summary>
            /// Gets a constant containing "openIdConnectUrl".
            /// </summary>
            public const string OpenIdConnectUrl = "openIdConnectUrl";

            #endregion
        }

        #endregion
    }
}
