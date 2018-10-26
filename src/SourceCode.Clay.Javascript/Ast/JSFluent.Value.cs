using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace SourceCode.Clay.Javascript.Ast
{
    partial class JSFluent
    {
        private static readonly Type s_typeofString = typeof(string);
        private static readonly Type s_typeofObject = typeof(object);
        private static readonly Type s_typeofJSIdentifier = typeof(JSIdentifier);
        private static readonly Type s_typeofJSExpression = typeof(JSExpression);
        private static readonly Type s_typeofJSObjectExpression = typeof(JSObjectExpression);
        private static readonly Type s_typeofJSLiteral = typeof(JSLiteral);
        private static readonly Type s_typeofJSIndexer = typeof(IJSIndexer);
        private static readonly MethodInfo s_methodOfJSObjectExpression_Property = s_typeofJSObjectExpression.GetMethod(nameof(JSObjectExpression.Add), BindingFlags.Public | BindingFlags.Instance, null, new[] { s_typeofJSIndexer, s_typeofJSExpression }, null);
        private static readonly ConstructorInfo s_constructorOfJSIdentifier = s_typeofJSIdentifier.GetConstructor(new[] { s_typeofString });
        private static readonly ConstructorInfo s_constructorOfJSLiteral = s_typeofJSLiteral.GetConstructor(new[] { s_typeofObject });

        private static readonly ConcurrentDictionary<RuntimeTypeHandle, Func<object, JSExpression>> _factories = new ConcurrentDictionary<RuntimeTypeHandle, Func<object, JSExpression>>();
        private static readonly Dictionary<RuntimeTypeHandle, Func<Type, Expression<Func<object, JSExpression>>>> _typeFactories = new Dictionary<RuntimeTypeHandle, Func<Type, Expression<Func<object, JSExpression>>>>()
        {
            { typeof(JSExpression).TypeHandle, CreateJSExpressionExpression },
            { typeof(string).TypeHandle, CreateJSLiteralExpression },
            { typeof(int).TypeHandle, CreateJSLiteralExpression },
            { typeof(long).TypeHandle, CreateJSLiteralExpression },
            { typeof(bool).TypeHandle, CreateJSLiteralExpression },
        };

        public static JSExpression JSValue<T>(T value)
        {
            if (Equals(value, default(T))) // TODO: ReferenceEquals
                return JSNull();
            if (value is JSExpression expression) return expression;
            RuntimeTypeHandle rth = typeof(T) == typeof(object)
                ? Type.GetTypeHandle(value)
                : typeof(T).TypeHandle;
            Func<object, JSExpression> factory = _factories.GetOrAdd(rth, CreateFactory);
            if (factory is null) throw new NotSupportedException();
            return factory(value);
        }

        private static Func<object, JSExpression> CreateFactory(RuntimeTypeHandle typeHandle)
        {
            var type = Type.GetTypeFromHandle(typeHandle);

            Expression<Func<object, JSExpression>> factory = CreateAnyExpression(type);
            if (factory is null) return default;

            return factory.Compile();
        }

        private static Expression<Func<object, JSExpression>> CreateAnyExpression(Type propertyType)
        {
            return s_typeofJSExpression.IsAssignableFrom(propertyType)
                ? CreateJSExpressionExpression(propertyType)
                : _typeFactories.TryGetValue(propertyType.TypeHandle, out Func<Type, Expression<Func<object, JSExpression>>> factoryFactory)
                    ? factoryFactory(propertyType)
                    : CreateFactoryExpression(propertyType);
        }

        private static Expression<Func<object, JSExpression>> CreateFactoryExpression(Type type)
        {
            if (type.GetCustomAttribute<CompilerGeneratedAttribute>(false) is null) return default;

            ParameterExpression param = Expression.Parameter(s_typeofObject, "value");
            UnaryExpression convertedParam = Expression.Convert(param, type);

            ParameterExpression variable = Expression.Variable(s_typeofJSObjectExpression);
            var result = (Expression)Expression.New(s_typeofJSObjectExpression);

            PropertyInfo[] props = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            for (var i = 0; i < props.Length; i++)
            {
                PropertyInfo prop = props[i];
                Type propertyType = prop.PropertyType;
                UnaryExpression accessor = Expression.Convert(Expression.Property(convertedParam, prop), s_typeofObject);
                UnaryExpression identifier = Expression.Convert(Expression.New(s_constructorOfJSIdentifier, Expression.Constant(prop.Name)), s_typeofJSIndexer);

                Expression<Func<object, JSExpression>> factory = CreateAnyExpression(propertyType);
                if (factory is null) return null;

                InvocationExpression invoked = Expression.Invoke(factory, accessor);
                result = Expression.Call(result, s_methodOfJSObjectExpression_Property, identifier, invoked);
            }

            result = Expression.Convert(result, s_typeofJSExpression);
            return Expression.Lambda<Func<object, JSExpression>>(result, param);
        }

        private static Expression<Func<object, JSExpression>> CreateJSExpressionExpression(Type type)
        {
            ParameterExpression param = Expression.Parameter(s_typeofObject, "value");
            UnaryExpression result = Expression.Convert(param, s_typeofJSExpression);
            return Expression.Lambda<Func<object, JSExpression>>(result, param);
        }

        private static Expression<Func<object, JSExpression>> CreateJSLiteralExpression(Type type)
        {
            ParameterExpression param = Expression.Parameter(s_typeofObject, "value");
            NewExpression constant = Expression.New(s_constructorOfJSLiteral, param);
            UnaryExpression result = Expression.Convert(constant, s_typeofJSExpression);
            return Expression.Lambda<Func<object, JSExpression>>(result, param);
        }
    }
}
