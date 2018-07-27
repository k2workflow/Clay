#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using Newtonsoft.Json.Linq;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;

namespace SourceCode.Clay.OpenApi.Serialization
{
    partial class OasSerializer
    {
        private static readonly ConcurrentDictionary<RuntimeTypeHandle, SerializerInfo> _serializers = new ConcurrentDictionary<RuntimeTypeHandle, SerializerInfo>();
        private SerializerInfo _mySerializers;

#pragma warning disable CA2231

        private readonly struct SerializerKey : IEquatable<SerializerKey>
        {
            public RuntimeTypeHandle GenericArgumentType { get; }
            public RuntimeTypeHandle InstanceType { get; }

            public SerializerKey(RuntimeTypeHandle genericArgumentType, RuntimeTypeHandle instanceType)
            {
                GenericArgumentType = genericArgumentType;
                InstanceType = instanceType;
            }

            public override bool Equals(object obj) => obj is SerializerKey o && Equals(o);

            public bool Equals(SerializerKey other)
                => GenericArgumentType.Equals(other.GenericArgumentType)
                && InstanceType.Equals(other.InstanceType);

            public override int GetHashCode() => HashCode.Combine(
                GenericArgumentType,
                InstanceType
            );
        }

#pragma warning restore CA2231

        private sealed class SerializerInfo
        {
            private readonly ConcurrentDictionary<SerializerKey, Delegate> _delegates = new ConcurrentDictionary<SerializerKey, Delegate>();
            private readonly Type _serializerType;
            private readonly MethodInfo[] _methods;

            public SerializerInfo(Type serializerType)
            {
                _serializerType = serializerType;
                _methods = serializerType.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                    .Where(x => !x.IsAbstract && !x.IsGenericMethod && x.ReturnType == typeof(JToken) && x.GetParameters().Length == 1)
                    .ToArray();
            }

            private Delegate CreateSerializer<T>(SerializerKey key)
            {
                var argType = typeof(T);
                var instType = Type.GetTypeFromHandle(key.InstanceType);

                MethodInfo method = default;
                for (; instType != typeof(ValueType) && instType != typeof(object); instType = instType.BaseType)
                {
                    method = _methods.FirstOrDefault(x => x.GetParameters()[0].ParameterType == instType);
                    if (!(method is null)) break;
                }

                if (method == default)
                {
                    instType = Type.GetTypeFromHandle(key.InstanceType);
                    return new Func<OasSerializer, T, JToken>(
                        (x, y) => throw new NotSupportedException($"Serializing the type {instType.FullName} is not supported."));
                }

                var serParam = Expression.Parameter(typeof(OasSerializer), "serializer");
                var param = Expression.Parameter(argType, "value");

                var ser = _serializerType == serParam.Type
                    ? (Expression)serParam
                    : Expression.Convert(serParam, _serializerType);

                var conv = argType == instType
                    ? (Expression)param
                    : Expression.Convert(param, instType);
                var call = Expression.Call(ser, method, conv);
                return Expression.Lambda<Func<OasSerializer, T, JToken>>(call, serParam, param).Compile();
            }

            public JToken Serialize<T>(OasSerializer serializer, T value)
            {
                var key = new SerializerKey(typeof(T).TypeHandle, value.GetType().TypeHandle);
                var del = _delegates.GetOrAdd(key, CreateSerializer<T>);
                return ((Func<OasSerializer, T, JToken>)del)(serializer, value);
            }
        }

        /// <summary>
        /// Serializes an unknown object type.
        /// </summary>
        /// <typeparam name="T">The type of the object. This may not reflect the exact object type.</typeparam>
        /// <param name="value">The object value.</param>
        /// <returns>The serialized <see cref="JToken"/>.</returns>
        protected virtual JToken SerializeUnknown<T>(T value)
        {
            if (ReferenceEquals(value, default(T))) return null;

            var mySerializers = _mySerializers;
            if (mySerializers is null)
            {
                Thread.MemoryBarrier();
                mySerializers = _serializers.GetOrAdd(GetType().TypeHandle, th => new SerializerInfo(Type.GetTypeFromHandle(th)));
                _mySerializers = mySerializers;
            }

            return _mySerializers.Serialize(this, value);
        }
    }
}
