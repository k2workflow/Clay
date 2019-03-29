using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace SourceCode.Clay.Net.Http
{
    internal static class TemplateCompiler
    {
#pragma warning disable IDE1006 // Naming Styles
        private static readonly Type TypeofStringBuilder = typeof(StringBuilder);
        private static readonly ConstructorInfo ConstructorOfStringBuilder = TypeofStringBuilder.GetConstructor(
            Type.EmptyTypes);
        private static readonly MethodInfo MethodofStringBuilder_Append = TypeofStringBuilder.GetMethod(
            nameof(StringBuilder.Append), new[] { typeof(string) }, null);
        private static readonly MethodInfo MethodofStringBuilder_ToString = TypeofStringBuilder.GetMethod(
            nameof(StringBuilder.ToString), Type.EmptyTypes, null);

        private static readonly Type TypeofUri = typeof(Uri);
        private static readonly MethodInfo MethodofUri_EscapeDataString = TypeofUri.GetMethod(
            nameof(Uri.EscapeDataString), new[] { typeof(string) }, null);
        private static readonly MethodInfo MethodofUri_EscapeUriString = TypeofUri.GetMethod(
            nameof(Uri.EscapeUriString), new[] { typeof(string) }, null);

        private static readonly Type TypeofTemplateCompiler = typeof(TemplateCompiler);
        private static readonly MethodInfo MethodOfTemplateCompiler_ToStringFormattable = TypeofTemplateCompiler.GetMethods(
            BindingFlags.NonPublic | BindingFlags.Static).First(x => x.Name == nameof(ToString) && x.GetParameters().Length == 3);
        private static readonly MethodInfo MethodOfTemplateCompiler_ToString = TypeofTemplateCompiler.GetMethods(
            BindingFlags.NonPublic | BindingFlags.Static).First(x => x.Name == nameof(ToString) && x.GetParameters().Length == 2);

        private static readonly Type TypeofIFormattable = typeof(IFormattable);
#pragma warning restore IDE1006 // Naming Styles

        private const string QueryStart = "?";
        private const string QueryAssign = "=";
        private const string QuerySeparator = "&";

        public static Func<object, string> Compile(RawUriTemplate raw, RuntimeTypeHandle typeHandle)
        {
            ParameterExpression param = Expression.Parameter(typeof(object), "value");
            BlockExpression body = Compile(raw, Expression.Convert(param, Type.GetTypeFromHandle(typeHandle)));
            var lambda = Expression.Lambda<Func<object, string>>(body, param);
            return lambda.Compile();
        }

        public static Func<T, string> Compile<T>(RawUriTemplate raw)
        {
            ParameterExpression param = Expression.Parameter(typeof(T), "value");
            BlockExpression body = Compile(raw, param);
            var lambda = Expression.Lambda<Func<T, string>>(body, param);
            return lambda.Compile();
        }

        private static BlockExpression Compile(RawUriTemplate raw, Expression value)
        {
            ParameterExpression sb = Expression.Variable(TypeofStringBuilder, "sb");
            var statements = new List<Expression>
            {
                Expression.Assign(sb, Expression.New(ConstructorOfStringBuilder))
            };

            for (var i = 0; i < raw.Path.Count; i++)
                AppendToken(value, sb, statements, raw.Path[i], MethodofUri_EscapeUriString);

            for (var i = 0; i < raw.Query.Count; i++)
            {
                statements.Add(Expression.Call(sb, MethodofStringBuilder_Append, Constant(i == 0 ? QueryStart : QuerySeparator)));

                UriQuery query = raw.Query[i];
                for (var j = 0; j < query.Name.Count; j++)
                    AppendToken(value, sb, statements, query.Name[j], MethodofUri_EscapeDataString);

                for (var j = 0; j < query.Value.Count; j++)
                {
                    if (j == 0)
                        statements.Add(Expression.Call(sb, MethodofStringBuilder_Append, Constant(QueryAssign)));
                    AppendToken(value, sb, statements, query.Value[j], MethodofUri_EscapeDataString);
                }
            }

            statements.Add(Expression.Call(sb, MethodofStringBuilder_ToString));
            BlockExpression body = Expression.Block(new[] { sb }, statements);
            return body;
        }

        private static void AppendToken(Expression param, ParameterExpression sb, List<Expression> statements, UriToken token, MethodInfo escape)
        {
            if (token.Type == UriTokenType.Literal)
                statements.Add(Expression.Call(sb, MethodofStringBuilder_Append, Constant(token.Default)));
            else
            {
                MemberExpression prop = Expression.PropertyOrField(param, token.Name);
                Expression toString;
                if (TypeofIFormattable.IsAssignableFrom(prop.Type))
                {
                    MethodInfo method = MethodOfTemplateCompiler_ToStringFormattable.MakeGenericMethod(prop.Type);
                    toString = Expression.Call(null, method, prop, Constant(token.Format), Constant(token.Default));
                }
                else
                {
                    MethodInfo method = MethodOfTemplateCompiler_ToString.MakeGenericMethod(prop.Type);
                    toString = Expression.Call(null, method, prop, Constant(token.Default));
                    toString = Expression.Call(null, escape, toString);
                }
                statements.Add(Expression.Call(sb, MethodofStringBuilder_Append, toString));
            }
        }

        private static ConstantExpression Constant<T>(T value) => Expression.Constant(value, typeof(T));

        private static string ToString<T>(T value, string formatString, string @default)
            where T : IFormattable
            => value?.ToString(formatString, CultureInfo.InvariantCulture) ?? @default ?? string.Empty;

        private static string ToString<T>(T value, string @default)
            => value?.ToString() ?? @default ?? string.Empty;
    }
}
