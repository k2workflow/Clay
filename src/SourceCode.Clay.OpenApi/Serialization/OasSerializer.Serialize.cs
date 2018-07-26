#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using Newtonsoft.Json.Linq;
using SourceCode.Clay.Json.Validation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SourceCode.Clay.OpenApi.Serialization
{
    partial class OasSerializer
    {
        /// <summary>
        /// Serializes a <see cref="OasResponse"/> value.
        /// </summary>
        /// <param name="value">The <see cref="OasResponse"/> value to serialize.</param>
        /// <returns>The <see cref="JToken"/>.</returns>
        protected virtual JToken SerializeResponse(OasResponse value)
        {
            if (value is null) return null;

            var json = new JObject();

            var content = value.Content
                .Select(x => ValueTuple.Create(x.Key?.ToString(), x.Value));

            SetJsonValue(json, PathConstants.Description, value.Description, true);
            SetJsonMap(json, PathConstants.Headers, value.Headers);
            SetJsonMap(json, PathConstants.Content, content);
            SetJsonMap(json, PathConstants.Links, value.Links);

            return json;
        }

        /// <summary>
        /// Serializes a <see cref="OasRequestBody"/> value.
        /// </summary>
        /// <param name="value">The <see cref="OasRequestBody"/> value to serialize.</param>
        /// <returns>The <see cref="JToken"/>.</returns>
        protected virtual JToken SerializeRequestBody(OasRequestBody value)
        {
            if (value is null) return null;

            var json = new JObject();

            var content = value.Content
                .Select(x => ValueTuple.Create(x.Key?.ToString(), x.Value));

            SetJsonValue(json, PathConstants.Description, value.Description);
            SetJsonMap(json, PathConstants.Content, content);
            SetJsonFlag(json, PathConstants.Required, value.Options, OasRequestBodyOptions.Required);

            return json;
        }

        /// <summary>
        /// Serializes a <see cref="OasPropertyEncoding"/> value.
        /// </summary>
        /// <param name="value">The <see cref="OasPropertyEncoding"/> value to serialize.</param>
        /// <returns>The <see cref="JToken"/>.</returns>
        protected virtual JToken SerializePropertyEncoding(OasPropertyEncoding value)
        {
            if (value is null) return null;

            var json = new JObject();

            SetJsonValue(json, PathConstants.ContentType, value.ContentType);
            SetJsonMap(json, PathConstants.Headers, value.Headers);
            SetJsonValue(json, PathConstants.Style, ToJsonValue(value.Style));
            SetJsonFlag(json, PathConstants.Explode, value.Options, OasPropertyEncodingOptions.Explode);
            SetJsonFlag(json, PathConstants.AllowReserved, value.Options, OasPropertyEncodingOptions.AllowReserved);

            return json;
        }

        /// <summary>
        /// Serializes a <see cref="OasPath"/> value.
        /// </summary>
        /// <param name="value">The <see cref="OasPath"/> value to serialize.</param>
        /// <returns>The <see cref="JToken"/>.</returns>
        protected virtual JToken SerializePath(OasPath value)
        {
            if (value is null) return null;

            var json = new JObject();

            SetJsonValue(json, PathConstants.Summary, value.Summary);
            SetJsonValue(json, PathConstants.Description, value.Description);
            SetJsonObject(json, PathConstants.Get, value.Get);
            SetJsonObject(json, PathConstants.Put, value.Put);
            SetJsonObject(json, PathConstants.Post, value.Post);
            SetJsonObject(json, PathConstants.Delete, value.Delete);
            SetJsonObject(json, PathConstants.Options, value.Options);
            SetJsonObject(json, PathConstants.Head, value.Head);
            SetJsonObject(json, PathConstants.Patch, value.Patch);
            SetJsonObject(json, PathConstants.Trace, value.Trace);
            SetJsonArray(json, PathConstants.Servers, value.Servers);
            SetJsonArray(json, PathConstants.Parameters, value.Parameters);

            return json;
        }

        /// <summary>
        /// Serializes a <see cref="OasParameterBody"/> value.
        /// </summary>
        /// <param name="value">The <see cref="OasParameterBody"/> value to serialize.</param>
        /// <returns>The <see cref="JToken"/>.</returns>
        protected virtual JToken SerializeParameterBody(OasParameterBody value)
        {
            if (value is null) return null;

            var json = new JObject();

            SerializeParameterBody(value, json);

            return json;
        }

        /// <summary>
        /// Serializes a <see cref="OasParameterBody"/> value into an existing <see cref="JObject"/> instance.
        /// </summary>
        /// <param name="value">The <see cref="OasParameterBody"/> value to serialize.</param>
        /// <param name="json">The <see cref="JObject"/> to populate.</param>
        protected virtual void SerializeParameterBody(OasParameterBody value, JObject json)
        {
            if (value is null) return;

            var examples = value.Examples
                .Select(x => ValueTuple.Create(x.Key?.ToString(), x.Value));

            var content = value.Content
                .Select(x => ValueTuple.Create(x.Key?.ToString(), x.Value));

            SetJsonValue(json, PathConstants.Description, value.Description);
            SetJsonFlag(json, PathConstants.Required, value.Options, OasParameterOptions.Required);
            SetJsonFlag(json, PathConstants.Deprecated, value.Options, OasParameterOptions.Deprecated);
            SetJsonFlag(json, PathConstants.AllowEmptyValue, value.Options, OasParameterOptions.AllowEmptyValue);

            SetJsonValue(json, PathConstants.Style, ToJsonValue(value.Style));
            SetJsonFlag(json, PathConstants.Explode, value.Options, OasParameterOptions.Explode);
            SetJsonFlag(json, PathConstants.AllowReserved, value.Options, OasParameterOptions.AllowReserved);
            SetJsonObject(json, PathConstants.Schema, value.Schema);
            SetJsonMap(json, PathConstants.Examples, examples);

            SetJsonMap(json, PathConstants.Content, content);
        }

        /// <summary>
        /// Serializes a <see cref="OasParameter"/> value.
        /// </summary>
        /// <param name="value">The <see cref="OasParameter"/> value to serialize.</param>
        /// <returns>The <see cref="JToken"/>.</returns>
        protected virtual JToken SerializeParameter(OasParameter value)
        {
            if (value is null) return null;

            var json = new JObject();

            SetJsonValue(json, PathConstants.Name, value.Name, true);
            SetJsonValue(json, PathConstants.In, ToJsonValue(value.Location));
            SerializeParameterBody(value, json);

            return json;
        }

        /// <summary>
        /// Serializes a <see cref="OasParameter"/> value according to its components.
        /// </summary>
        /// <param name="key">The <see cref="OasParameterKey"/> component to serialize.</param>
        /// <param name="value">The referable <see cref="OasParameterBody"/> component to serialize.</param>
        /// <returns>The <see cref="JToken"/>.</returns>
        protected virtual JToken SerializeParameter(OasParameterKey key, OasReferable<OasParameterBody> value)
        {
            if (value.IsReference) return SerializeReferable(value);
            return SerializeParameter(key, value.Value);
        }

        /// <summary>
        /// Serializes a <see cref="OasParameter"/> value according to its components.
        /// </summary>
        /// <param name="key">The <see cref="OasParameterKey"/> component to serialize.</param>
        /// <param name="value">The <see cref="OasParameterBody"/> component to serialize.</param>
        /// <returns>The <see cref="JToken"/>.</returns>
        protected virtual JToken SerializeParameter(OasParameterKey key, OasParameterBody value)
        {
            if (value is null) return null;

            var json = new JObject();

            SetJsonValue(json, PathConstants.Name, key.Name, true);
            SetJsonValue(json, PathConstants.In, ToJsonValue(key.Location));
            SerializeParameterBody(value, json);

            return json;
        }

        /// <summary>
        /// Serializes a <see cref="OasOperation"/> value.
        /// </summary>
        /// <param name="value">The <see cref="OasOperation"/> value to serialize.</param>
        /// <returns>The <see cref="JToken"/>.</returns>
        protected virtual JToken SerializeOperation(OasOperation value)
        {
            if (value is null) return null;

            var json = new JObject();

            var responses = value.Responses
                .Select(x => ValueTuple.Create(x.Key.ToString(), x.Value));

            SetJsonArray(json, PathConstants.Tags, value.Tags);
            SetJsonValue(json, PathConstants.Summary, value.Summary);
            SetJsonValue(json, PathConstants.Description, value.Description);
            SetJsonObject(json, PathConstants.ExternalDocs, value.ExternalDocumentation);
            SetJsonValue(json, PathConstants.OperationId, value.OperationIdentifier);
            SetJsonArray(json, PathConstants.Parameters, value.Parameters);
            SetJsonObject(json, PathConstants.RequestBody, value.RequestBody);
            SetJsonMap(json, PathConstants.Responses, responses);
            SetJsonMap(json, PathConstants.Callbacks, value.Callbacks);
            SetJsonFlag(json, PathConstants.Deprecated, value.Options, OasOperationOptions.Deprecated);
            SetJsonArray(json, PathConstants.Security, value.Security);
            SetJsonArray(json, PathConstants.Servers, value.Servers);

            return json;
        }

        /// <summary>
        /// Serializes a <see cref="OasOidcSecurityScheme"/> value.
        /// </summary>
        /// <param name="value">The <see cref="OasOidcSecurityScheme"/> value to serialize.</param>
        /// <returns>The <see cref="JToken"/>.</returns>
        protected virtual JToken SerializeOpenIdConnectSecurityScheme(OasOidcSecurityScheme value)
        {
            if (value is null) return null;

            var json = new JObject();

            SetJsonValue(json, PathConstants.Type, EnumConstants.OpenIdConnect);
            SetJsonValue(json, PathConstants.Description, value.Description);
            SetJsonValue(json, PathConstants.OpenIdConnectUrl, value.Url);

            return json;
        }

        /// <summary>
        /// Serializes a <see cref="OasOAuthFlow"/> value.
        /// </summary>
        /// <param name="value">The <see cref="OasOAuthFlow"/> value to serialize.</param>
        /// <returns>The <see cref="JToken"/>.</returns>
        protected virtual JToken SerializeOAuthFlow(OasOAuthFlow value)
        {
            if (value is null) return null;

            var json = new JObject();

            SetJsonValue(json, PathConstants.AuthorizationUrl, value.AuthorizationUrl, true);
            SetJsonValue(json, PathConstants.TokenUrl, value.TokenUrl, true);
            SetJsonValue(json, PathConstants.RefreshUrl, value.RefreshUrl);
            SetJsonMap(json, PathConstants.Scopes, value.Scopes);

            return json;
        }

        /// <summary>
        /// Serializes a <see cref="OasOAuth2SecurityScheme"/> value.
        /// </summary>
        /// <param name="value">The <see cref="OasOAuth2SecurityScheme"/> value to serialize.</param>
        /// <returns>The <see cref="JToken"/>.</returns>
        protected virtual JToken SerializeOAuth2SecurityScheme(OasOAuth2SecurityScheme value)
        {
            if (value is null) return null;

            var json = new JObject();

            SetJsonValue(json, PathConstants.Type, EnumConstants.OAuth2);
            SetJsonValue(json, PathConstants.Description, value.Description);

            var flows = new JObject();

            SetJsonObject(flows, PathConstants.Implicit, value.ImplicitFlow);
            SetJsonObject(flows, PathConstants.Password, value.PasswordFlow);
            SetJsonObject(flows, PathConstants.ClientCredentials, value.ClientCredentialsFlow);
            SetJsonObject(flows, PathConstants.AuthorizationCode, value.AuthorizationCodeFlow);

            json.Add(PathConstants.Flows, flows);

            return json;
        }

        /// <summary>
        /// Serializes a <see cref="OasMediaType"/> value.
        /// </summary>
        /// <param name="value">The <see cref="OasMediaType"/> value to serialize.</param>
        /// <returns>The <see cref="JToken"/>.</returns>
        protected virtual JToken SerializeMediaType(OasMediaType value)
        {
            if (value is null) return null;

            var json = new JObject();

            SetJsonObject(json, PathConstants.Schema, value.Schema);
            SetJsonMap(json, PathConstants.Examples, value.Examples);
            SetJsonMap(json, PathConstants.Encoding, value.Encoding);

            return json;
        }

        /// <summary>
        /// Serializes a <see cref="OasLink"/> value.
        /// </summary>
        /// <param name="value">The <see cref="OasLink"/> value to serialize.</param>
        /// <returns>The <see cref="JToken"/>.</returns>
        protected virtual JToken SerializeLink(OasLink value)
        {
            if (value is null) return null;

            var json = new JObject();

            SetJsonValue(json, PathConstants.OperationRef, SerializeReference(value.OperationReference));
            SetJsonValue(json, PathConstants.OperationId, value.OperationIdentifier);
            SetJsonValue(json, PathConstants.Description, value.Description);
            SetJsonObject(json, PathConstants.Server, value.Server);

            return json;
        }

        /// <summary>
        /// Serializes a <see cref="OasLicense"/> value.
        /// </summary>
        /// <param name="value">The <see cref="OasLicense"/> value to serialize.</param>
        /// <returns>The <see cref="JToken"/>.</returns>
        protected virtual JToken SerializeLicense(OasLicense value)
        {
            if (value is null) return null;

            var json = new JObject();

            SetJsonValue(json, PathConstants.Name, value.Name);
            SetJsonValue(json, PathConstants.Url, value.Url);

            return json;
        }

        /// <summary>
        /// Serializes a <see cref="OasInformation"/> value.
        /// </summary>
        /// <param name="value">The <see cref="OasInformation"/> value to serialize.</param>
        /// <returns>The <see cref="JToken"/>.</returns>
        protected virtual JToken SerializeInformation(OasInformation value)
        {
            if (value is null) return null;

            var json = new JObject();

            SetJsonValue(json, PathConstants.Title, value.Title);
            SetJsonValue(json, PathConstants.Description, value.Description);
            SetJsonValue(json, PathConstants.TermsOfService, value.TermsOfService);
            SetJsonObject(json, PathConstants.Contact, value.Contact);
            SetJsonObject(json, PathConstants.License, value.License);
            SetJsonValue(json, PathConstants.Version, value.Version.ToString());

            return json;
        }

        /// <summary>
        /// Serializes a <see cref="OasHttpSecurityScheme"/> value.
        /// </summary>
        /// <param name="value">The <see cref="OasHttpSecurityScheme"/> value to serialize.</param>
        /// <returns>The <see cref="JToken"/>.</returns>
        protected virtual JToken SerializeHttpSecurityScheme(OasHttpSecurityScheme value)
        {
            if (value is null) return null;

            var json = new JObject();

            SetJsonValue(json, PathConstants.Type, EnumConstants.Http);
            SetJsonValue(json, PathConstants.Description, value.Description);
            SetJsonValue(json, PathConstants.Scheme, value.Scheme, true);
            SetJsonValue(json, PathConstants.BearerFormat, value.BearerFormat);

            return json;
        }

        /// <summary>
        /// Serializes a <see cref="OasReference"/> value.
        /// </summary>
        /// <param name="value">The <see cref="OasReference"/> value to serialize.</param>
        /// <returns>The <see cref="JToken"/>.</returns>
        protected virtual JToken SerializeReference(OasReference value)
        {
            if (!value.HasValue) return null;
            return value.ToUri()?.ToString();
        }

        /// <summary>
        /// Serializes a <see cref="IOasReferable"/> value.
        /// </summary>
        /// <param name="value">The <see cref="IOasReferable"/> value to serialize.</param>
        /// <returns>The <see cref="JToken"/>.</returns>
        protected virtual JToken SerializeReferable(IOasReferable value)
        {
            if (!value.HasValue) return null;
            if (value.IsReference)
            {
                var reference = Serialize(value.Reference);
                return new JObject()
                {
                    [PathConstants.Reference] = reference
                };
            }
            return SerializeUnknown(value.Value);
        }

        /// <summary>
        /// Serializes a <see cref="OasContact"/> value.
        /// </summary>
        /// <param name="value">The <see cref="OasContact"/> value to serialize.</param>
        /// <returns>The <see cref="JToken"/>.</returns>
        protected virtual JToken SerializeComponents(OasComponents value)
        {
            if (value is null) return null;

            var json = new JObject();

            SetJsonMap(json, PathConstants.Schemas, value.Schemas);
            SetJsonMap(json, PathConstants.Responses, value.Responses);
            SetJsonMap(json, PathConstants.Parameters, value.Parameters);
            SetJsonMap(json, PathConstants.Examples, value.Examples);
            SetJsonMap(json, PathConstants.RequestBodies, value.RequestBodies);
            SetJsonMap(json, PathConstants.Headers, value.Headers);
            SetJsonMap(json, PathConstants.SecuritySchemes, value.SecuritySchemes);
            SetJsonMap(json, PathConstants.Links, value.Links);
            SetJsonMap(json, PathConstants.Callbacks, value.Callbacks);

            return json;
        }

        /// <summary>
        /// Serializes a <see cref="OasContact"/> value.
        /// </summary>
        /// <param name="value">The <see cref="OasContact"/> value to serialize.</param>
        /// <returns>The <see cref="JToken"/>.</returns>
        protected virtual JToken SerializeContact(OasContact value)
        {
            if (value is null) return null;

            var json = new JObject();

            SetJsonValue(json, PathConstants.Name, value.Name);
            SetJsonValue(json, PathConstants.Url, value.Url);
            SetJsonValue(json, PathConstants.Email, value.Email);

            return json;
        }

        /// <summary>
        /// Serializes a <see cref="OasExternalDocumentation"/> value.
        /// </summary>
        /// <param name="value">The <see cref="OasExternalDocumentation"/> value to serialize.</param>
        /// <returns>The <see cref="JToken"/>.</returns>
        protected virtual JToken SerializeExternalDocumentation(OasExternalDocumentation value)
        {
            if (value is null) return null;

            var json = new JObject();

            SetJsonValue(json, PathConstants.Description, value.Description);
            SetJsonValue(json, PathConstants.Url, value.Url, true);

            return json;
        }

        /// <summary>
        /// Serializes a <see cref="OasExample"/> value.
        /// </summary>
        /// <param name="value">The <see cref="OasExample"/> value to serialize.</param>
        /// <returns>The <see cref="JToken"/>.</returns>
        protected virtual JToken SerializeExample(OasExample value)
        {
            if (value is null) return null;

            var json = new JObject();

            SetJsonValue(json, PathConstants.Summary, value.Summary);
            SetJsonValue(json, PathConstants.Description, value.Description);
            SetJsonValue(json, PathConstants.ExternalValue, value.ExternalValue);

            return json;
        }

        /// <summary>
        /// Serializes a <see cref="OasDocument"/> value.
        /// </summary>
        /// <param name="value">The <see cref="OasDocument"/> value to serialize.</param>
        /// <returns>The <see cref="JToken"/>.</returns>
        protected virtual JToken SerializeDocument(OasDocument value)
        {
            if (value is null) return null;

            var json = new JObject();

            SetJsonValue(json, PathConstants.OpenApi, value.Version.ToString());
            SetJsonObject(json, PathConstants.Info, value.Info, true);
            SetJsonArray(json, PathConstants.Servers, value.Servers);
            SetJsonMap(json, PathConstants.Paths, value.Paths, true);
            SetJsonObject(json, PathConstants.Components, value.Components);
            SetJsonArray(json, PathConstants.Security, value.Security);
            SetJsonArray(json, PathConstants.Tags, value.Tags);
            SetJsonObject(json, PathConstants.ExternalDocs, value.ExternalDocumentation);

            return json;
        }

        /// <summary>
        /// Serializes a <see cref="OasCallback"/> value.
        /// </summary>
        /// <param name="value">The <see cref="OasCallback"/> value to serialize.</param>
        /// <returns>The <see cref="JToken"/>.</returns>
        protected virtual JToken SerializeCallback(OasCallback value)
        {
            if (value is null) return null;

            var json = new JObject();

            foreach (var item in value)
            {
                var k = item.Key.ToString();
                var v = Serialize(item.Value);
                json.Add(k, v);
            }

            return json;
        }

        /// <summary>
        /// Serializes a <see cref="OasApiKeySecurityScheme"/> value.
        /// </summary>
        /// <param name="value">The <see cref="OasApiKeySecurityScheme"/> value to serialize.</param>
        /// <returns>The <see cref="JToken"/>.</returns>
        protected virtual JToken SerializeApiKeySecurityScheme(OasApiKeySecurityScheme value)
        {
            if (value is null) return null;

            var json = new JObject();

            SetJsonValue(json, PathConstants.Type, EnumConstants.ApiKey);
            SetJsonValue(json, PathConstants.Description, value.Description);
            SetJsonValue(json, PathConstants.Name, value.Name);
            SetJsonValue(json, PathConstants.In, ToJsonValue(value.Location));

            return json;
        }

        /// <summary>
        /// Serializes a <see cref="OasTag"/> value.
        /// </summary>
        /// <param name="value">The <see cref="OasTag"/> value to serialize.</param>
        /// <returns>The <see cref="JToken"/>.</returns>
        protected virtual JToken SerializeTag(OasTag value)
        {
            if (value is null) return null;

            var json = new JObject();

            SetJsonValue(json, PathConstants.Name, value.Name);
            SetJsonValue(json, PathConstants.Description, value.Description);
            SetJsonObject(json, PathConstants.ExternalDocs, value.ExternalDocumentation);

            return json;
        }

        /// <summary>
        /// Serializes a <see cref="OasServerVariable"/> value.
        /// </summary>
        /// <param name="value">The <see cref="OasServerVariable"/> value to serialize.</param>
        /// <returns>The <see cref="JToken"/>.</returns>
        protected virtual JToken SerializeServerVariable(OasServerVariable value)
        {
            if (value is null) return null;

            var json = new JObject();

            SetJsonArray(json, PathConstants.Enum, value.Enum);
            SetJsonValue(json, PathConstants.Default, value.Default);
            SetJsonValue(json, PathConstants.Description, value.Description);

            return json;
        }

        /// <summary>
        /// Serializes a <see cref="OasServer"/> value.
        /// </summary>
        /// <param name="value">The <see cref="OasServer"/> value to serialize.</param>
        /// <returns>The <see cref="JToken"/>.</returns>
        protected virtual JToken SerializeServer(OasServer value)
        {
            if (value is null) return null;

            var json = new JObject();

            SetJsonValue(json, PathConstants.Url, value.Url);
            SetJsonValue(json, PathConstants.Description, value.Description);
            SetJsonMap(json, PathConstants.Variables, value.Variables);

            return json;
        }

        /// <summary>
        /// Serializes a <see cref="OasSecurityScheme"/> value.
        /// </summary>
        /// <param name="value">The <see cref="OasSecurityScheme"/> value to serialize.</param>
        /// <returns>The <see cref="JToken"/>.</returns>
        protected virtual JToken SerializeSecurityScheme(OasSecurityScheme value) =>
            SerializeUnknown(value);

        /// <summary>
        /// Serializes a <see cref="OasSchema"/> value.
        /// </summary>
        /// <param name="value">The schema value to serialize.</param>
        /// <returns>The <see cref="JToken"/>.</returns>
        protected virtual JToken SerializeSchema(OasSchema value)
        {
            if (value is null) return null;

            var json = new JObject();

            SetJsonValue(json, PathConstants.Title, value.Title);
            SetJsonValue(json, PathConstants.Description, value.Description);
            SetJsonValue(json, PathConstants.Type, ToJsonValue(value.JsonType));
            SetJsonValue(json, PathConstants.Format, value.Format);

            SetJsonValue(json, PathConstants.Pattern, value.Pattern);
            SetJsonArray(json, PathConstants.EnumValue, value.Enum);

            SetJsonFlag(json, PathConstants.Deprecated, value.Options, OasSchemaOptions.Deprecated);
            SetJsonFlag(json, PathConstants.Nullable, value.Options, OasSchemaOptions.Nullable);
            SetJsonFlag(json, PathConstants.Required, value.Options, OasSchemaOptions.Required);
            SetJsonFlag(json, PathConstants.UniqueItems, value.Options, OasSchemaOptions.UniqueItems);

            SetJsonArray(json, PathConstants.AllOf, value.AllOf);
            SetJsonArray(json, PathConstants.AnyOf, value.AnyOf);
            SetJsonArray(json, PathConstants.OneOf, value.OneOf);
            SetJsonArray(json, PathConstants.Not, value.Not);

            if (value.NumberRange.IsConstrained)
            {
                SetJsonNumber(json, PathConstants.Minimum, value.NumberRange.Minimum);
                if (value.NumberRange.Minimum.HasValue)
                    SetJsonFlag(json, PathConstants.ExclusiveMinimum, value.NumberRange.RangeOptions, RangeOptions.MinimumInclusive, invert: true);

                SetJsonNumber(json, PathConstants.Maximum, value.NumberRange.Maximum);
                if (value.NumberRange.Maximum.HasValue)
                    SetJsonFlag(json, PathConstants.ExclusiveMaximum, value.NumberRange.RangeOptions, RangeOptions.MaximumInclusive, invert: true);

                SetJsonNumber(json, PathConstants.MultipleOf, value.NumberRange.MultipleOf);
            }

            if (value.ItemsRange.IsBounded)
            {
                SetJsonValue(json, PathConstants.MinItems, value.ItemsRange.Minimum);
                SetJsonValue(json, PathConstants.MaxItems, value.ItemsRange.Maximum);
            }

            if (value.LengthRange.IsBounded)
            {
                SetJsonValue(json, PathConstants.MinLength, value.LengthRange.Minimum);
                SetJsonValue(json, PathConstants.MaxLength, value.LengthRange.Maximum);
            }

            if (value.PropertiesRange.IsBounded)
            {
                SetJsonValue(json, PathConstants.MinProperties, value.PropertiesRange.Minimum);
                SetJsonValue(json, PathConstants.MaxProperties, value.PropertiesRange.Maximum);
            }

            SetJsonObject(json, PathConstants.Items, value.Items);
            SetJsonMap(json, PathConstants.Properties, value.Properties);
            SetJsonMap(json, PathConstants.AdditionalProperties, value.AdditionalProperties);

            SetJsonObject(json, PathConstants.ExternalDocs, value.ExternalDocumentation);

            return json;
        }

        /// <summary>Serializes the specified OpenAPI object to a JSON value.</summary>
        /// <typeparam name="T">The type of the OpenAPI object.</typeparam>
        /// <param name="value">The instance of the OpenAPI object.</param>
        /// <returns>The serialized value.</returns>
        public virtual JToken Serialize<T>(T value)
        {
            if (ReferenceEquals(value, null)) return null;

            if (typeof(T) == typeof(string)) return (string)(object)value;

            if (value is IOasReferable referable) return SerializeReferable(referable);

            if (typeof(T) == typeof(KeyValuePair<OasParameterKey, OasParameterBody>))
            {
                var kvp = (KeyValuePair<OasParameterKey, OasParameterBody>)(object)value;
                return SerializeParameter(kvp.Key, kvp.Value);
            }

            if (typeof(T) == typeof(KeyValuePair<OasParameterKey, OasReferable<OasParameterBody>>))
            {
                var kvp = (KeyValuePair<OasParameterKey, OasReferable<OasParameterBody>>)(object)value;
                return SerializeParameter(kvp.Key, kvp.Value);
            }

            return SerializeUnknown(value);
        }
    }
}
