using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace SourceCode.Clay.Javascript.Ast
{
    partial class JSNodeConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value is null)
                writer.WriteNull();
            else if (!(value is IJSNode node))
                throw new ArgumentOutOfRangeException(nameof(value));
            else
                WriteJson(writer, node, serializer);
        }

        protected virtual void WriteJson(JsonWriter writer, IJSNode value, JsonSerializer serializer)
        {
            if (value is null)
            {
                writer.WriteNull();
                return;
            }
            else if (value is JSExpression expression)
            {
                WriteJson(writer, expression, serializer);
                return;
            }
            else if (value is JSStatement statement)
            {
                WriteJson(writer, statement, serializer);
                return;
            }

            switch (value.Type)
            {
                case JSNodeType.Program:
                    WriteJson(writer, (JSProgram)value, serializer);
                    break;
                case JSNodeType.SwitchCase:
                    WriteJson(writer, (JSSwitchCase)value, serializer);
                    break;
                case JSNodeType.CatchClause:
                    WriteJson(writer, (JSCatchClause)value, serializer);
                    break;
                case JSNodeType.VariableDeclarator:
                    WriteJson(writer, (JSVariableDeclarator)value, serializer);
                    break;
                case JSNodeType.Property:
                    WriteJson(writer, (JSProperty)value, serializer);
                    break;
                default: throw new NotSupportedException($"The AST node type {value.Type} is not supported.");
            }
        }

        protected virtual void WriteJson(JsonWriter writer, JSExpression value, JsonSerializer serializer)
        {
            if (value is null)
            {
                writer.WriteNull();
                return;
            }

            switch (value.Type)
            {
                case JSNodeType.Identifier:
                    WriteJson(writer, (JSIdentifier)value, serializer);
                    break;
                case JSNodeType.Literal:
                    WriteJson(writer, (JSLiteral)value, serializer);
                    break;
                case JSNodeType.ThisExpression:
                    WriteJson(writer, (JSThisExpression)value, serializer);
                    break;
                case JSNodeType.ArrayExpression:
                    WriteJson(writer, (JSArrayExpression)value, serializer);
                    break;
                case JSNodeType.ObjectExpression:
                    WriteJson(writer, (JSObjectExpression)value, serializer);
                    break;
                case JSNodeType.FunctionExpression:
                    WriteJson(writer, (JSFunctionExpression)value, serializer);
                    break;
                case JSNodeType.UnaryExpression:
                    WriteJson(writer, (JSUnaryExpression)value, serializer);
                    break;
                case JSNodeType.BinaryExpression:
                    WriteJson(writer, (JSBinaryExpression)value, serializer);
                    break;
                case JSNodeType.MemberExpression:
                    WriteJson(writer, (JSMemberExpression)value, serializer);
                    break;
                case JSNodeType.ConditionalExpression:
                    WriteJson(writer, (JSConditionalExpression)value, serializer);
                    break;
                case JSNodeType.CallExpression:
                    WriteJson(writer, (JSCallExpression)value, serializer);
                    break;
                case JSNodeType.NewExpression:
                    WriteJson(writer, (JSNewExpression)value, serializer);
                    break;
                case JSNodeType.SequenceExpression:
                    WriteJson(writer, (JSSequenceExpression)value, serializer);
                    break;
                default: throw new NotSupportedException($"The AST node type {value.Type} is not supported.");
            }
        }

        protected virtual void WriteJson(JsonWriter writer, JSStatement value, JsonSerializer serializer)
        {
            if (value is null)
            {
                writer.WriteStartObject();
                writer.WritePropertyName("type");
                writer.WriteValue("EmptyStatement");
                writer.WriteEndObject();
                return;
            }

            switch (value.Type)
            {
                case JSNodeType.BlockStatement:
                    WriteJson(writer, (JSBlockStatement)value, serializer);
                    break;
                case JSNodeType.DebuggerStatement:
                    WriteJson(writer, (JSDebuggerStatement)value, serializer);
                    break;
                case JSNodeType.WithStatement:
                    WriteJson(writer, (JSWithStatement)value, serializer);
                    break;
                case JSNodeType.ReturnStatement:
                    WriteJson(writer, (JSReturnStatement)value, serializer);
                    break;
                case JSNodeType.LabeledStatement:
                    WriteJson(writer, (JSLabeledStatement)value, serializer);
                    break;
                case JSNodeType.BreakStatement:
                    WriteJson(writer, (JSBreakStatement)value, serializer);
                    break;
                case JSNodeType.ContinueStatement:
                    WriteJson(writer, (JSContinueStatement)value, serializer);
                    break;
                case JSNodeType.ExpressionStatement:
                    WriteJson(writer, (JSExpressionStatement)value, serializer);
                    break;
                case JSNodeType.IfStatement:
                    WriteJson(writer, (JSIfStatement)value, serializer);
                    break;
                case JSNodeType.SwitchStatement:
                    WriteJson(writer, (JSSwitchStatement)value, serializer);
                    break;
                case JSNodeType.ThrowStatement:
                    WriteJson(writer, (JSThrowStatement)value, serializer);
                    break;
                case JSNodeType.TryStatement:
                    WriteJson(writer, (JSTryStatement)value, serializer);
                    break;
                case JSNodeType.WhileStatement:
                    WriteJson(writer, (JSWhileStatement)value, serializer);
                    break;
                case JSNodeType.DoWhileStatement:
                    WriteJson(writer, (JSDoWhileStatement)value, serializer);
                    break;
                case JSNodeType.ForStatement:
                    WriteJson(writer, (JSForStatement)value, serializer);
                    break;
                case JSNodeType.ForInStatement:
                    WriteJson(writer, (JSForInStatement)value, serializer);
                    break;
                case JSNodeType.FunctionDeclaration:
                    WriteJson(writer, (JSFunctionDeclaration)value, serializer);
                    break;
                case JSNodeType.VariableDeclaration:
                    WriteJson(writer, (JSVariableDeclaration)value, serializer);
                    break;
                default: throw new NotSupportedException($"The AST node type {value.Type} is not supported.");
            }
        }

        protected virtual void WriteJson(JsonWriter writer, JSArrayExpression node, JsonSerializer serializer)
        {
            writer.WriteStartObject();

            writer.WritePropertyName("type");
            writer.WriteValue("ArrayExpression");

            writer.WritePropertyName("elements");
            writer.WriteStartArray();

            for (var i = 0; i < node.Elements.Count; i++)
            {
                var item = node.Elements[i];
                WriteJson(writer, item, serializer);
            }

            writer.WriteEndArray();

            writer.WriteEndObject();
        }

        protected virtual void WriteJson(JsonWriter writer, JSBinaryExpression node, JsonSerializer serializer)
        {
            if (node.Right.Count == 0)
            {
                WriteJson(writer, node.Left, serializer);
                return;
            }

            writer.WriteStartObject();

            var (type, op) = ConvertBinaryOperator(node.Operator);

            writer.WritePropertyName("type");
            writer.WriteValue(type);

            writer.WritePropertyName("operator");
            writer.WriteValue(op);

            writer.WritePropertyName("left");
            WriteJson(writer, node.Left, serializer);

            writer.WritePropertyName("right");
            for (var i = 0; i < node.Right.Count - 1; i++)
            {
                writer.WriteStartObject();

                writer.WritePropertyName("type");
                writer.WriteValue(type);

                writer.WritePropertyName("operator");
                writer.WriteValue(op);

                writer.WritePropertyName("left");
                WriteJson(writer, node.Right[i], serializer);

                writer.WritePropertyName("right");
            }

            WriteJson(writer, node.Right[node.Right.Count - 1], serializer);

            for (var i = 0; i < node.Right.Count - 1; i++)
            {
                writer.WriteEndObject();
            }

            writer.WriteEndObject();
        }

        protected virtual (string Type, string Operator) ConvertBinaryOperator(JSBinaryOperator @operator)
        {
            switch (@operator)
            {
                case JSBinaryOperator.IdentityEquality: return ("BinaryExpression", "===");
                case JSBinaryOperator.IdentityInequality: return ("BinaryExpression", "!==");
                case JSBinaryOperator.CoercedEquality: return ("BinaryExpression", "==");
                case JSBinaryOperator.CoercedInequality: return ("BinaryExpression", "!=");
                case JSBinaryOperator.LessThan: return ("BinaryExpression", "<");
                case JSBinaryOperator.LessThanOrEqual: return ("BinaryExpression", "<=");
                case JSBinaryOperator.GreaterThan: return ("BinaryExpression", ">");
                case JSBinaryOperator.GreaterThanOrEqual: return ("BinaryExpression", ">=");
                case JSBinaryOperator.UnsignedLeftShift: return ("BinaryExpression", "<<");
                case JSBinaryOperator.SignedRightShift: return ("BinaryExpression", ">>");
                case JSBinaryOperator.UnsignedRightShift: return ("BinaryExpression", ">>>");
                case JSBinaryOperator.Add: return ("BinaryExpression", "+");
                case JSBinaryOperator.Subtract: return ("BinaryExpression", "-");
                case JSBinaryOperator.Multiply: return ("BinaryExpression", "*");
                case JSBinaryOperator.Divide: return ("BinaryExpression", "/");
                case JSBinaryOperator.Modulus: return ("BinaryExpression", "%");
                case JSBinaryOperator.BitwiseOr: return ("BinaryExpression", "|");
                case JSBinaryOperator.BitwiseXor: return ("BinaryExpression", "^");
                case JSBinaryOperator.BitwiseAnd: return ("BinaryExpression", "&");
                case JSBinaryOperator.In: return ("BinaryExpression", "in");
                case JSBinaryOperator.InstanceOf: return ("BinaryExpression", "instanceof");
                case JSBinaryOperator.Assign: return ("AssignmentExpression", "=");
                case JSBinaryOperator.AddAssign: return ("AssignmentExpression", "+=");
                case JSBinaryOperator.SubtractAssign: return ("AssignmentExpression", "-=");
                case JSBinaryOperator.MultiplyAssign: return ("AssignmentExpression", "*=");
                case JSBinaryOperator.DivideAssign: return ("AssignmentExpression", "/=");
                case JSBinaryOperator.ModulusAssign: return ("AssignmentExpression", "%=");
                case JSBinaryOperator.UnsignedLeftShiftAssign: return ("AssignmentExpression", "<<=");
                case JSBinaryOperator.SignedRightShiftAssign: return ("AssignmentExpression", ">>=");
                case JSBinaryOperator.UnsignedRightShiftAssign: return ("AssignmentExpression", ">>>=");
                case JSBinaryOperator.BitwiseOrAssign: return ("AssignmentExpression", "|=");
                case JSBinaryOperator.BitwiseXorAssign: return ("AssignmentExpression", "^=");
                case JSBinaryOperator.BitwiseAndAssign: return ("AssignmentExpression", "&=");
                case JSBinaryOperator.LogicalAnd: return ("LogicalExpression", "&&");
                case JSBinaryOperator.LogicalOr: return ("LogicalExpression", "||");
                default: throw new NotSupportedException($"The JS binary operator {@operator} is not supported.");
            }
        }

        protected virtual void WriteJson(JsonWriter writer, JSBlockStatement node, JsonSerializer serializer)
        {
            writer.WriteStartObject();

            writer.WritePropertyName("type");
            writer.WriteValue("BlockStatement");

            writer.WritePropertyName("body");
            writer.WriteStartArray();

            for (var i = 0; i < node.Body.Count; i++)
            {
                var item = node.Body[i];
                WriteJson(writer, item, serializer);
            }

            writer.WriteEndArray();

            writer.WriteEndObject();
        }

        protected virtual void WriteJson(JsonWriter writer, JSBreakStatement node, JsonSerializer serializer)
        {
            writer.WriteStartObject();

            writer.WritePropertyName("type");
            writer.WriteValue("BreakStatement");

            writer.WritePropertyName("label");
            if (node.Label is null)
                writer.WriteNull();
            else
                WriteJson(writer, node.Label, serializer);

            writer.WriteEndObject();
        }

        protected virtual void WriteJson(JsonWriter writer, JSCallExpression node, JsonSerializer serializer)
        {
            writer.WriteStartObject();

            writer.WritePropertyName("type");
            writer.WriteValue("CallExpression");

            writer.WritePropertyName("callee");
            WriteJson(writer, node.Callee, serializer);

            writer.WritePropertyName("arguments");
            writer.WriteStartArray();

            for (var i = 0; i < node.Arguments.Count; i++)
            {
                var item = node.Arguments[i];
                WriteJson(writer, item, serializer);
            }

            writer.WriteEndArray();

            writer.WriteEndObject();
        }

        protected virtual void WriteJson(JsonWriter writer, JSCatchClause node, JsonSerializer serializer)
        {
            writer.WriteStartObject();

            writer.WritePropertyName("type");
            writer.WriteValue("CatchClause");

            writer.WritePropertyName("param");
            WriteJson(writer, node.Parameter, serializer);

            writer.WritePropertyName("body");
            WriteJson(writer, node.Body, serializer);

            writer.WriteEndObject();
        }

        protected virtual void WriteJson(JsonWriter writer, IList<JSStatement> items, JsonSerializer serializer)
        {
            writer.WriteStartObject();

            writer.WritePropertyName("type");
            writer.WriteValue("BlockStatement");

            writer.WritePropertyName("body");
            writer.WriteStartArray();

            for (var i = 0; i < items.Count; i++)
            {
                var item = items[i];
                WriteJson(writer, item, serializer);
            }

            writer.WriteEndArray();
            writer.WriteEndObject();
        }

        protected virtual void WriteJson(JsonWriter writer, JSConditionalExpression node, JsonSerializer serializer)
        {
            writer.WriteStartObject();

            writer.WritePropertyName("type");
            writer.WriteValue("ConditionalExpression");

            writer.WritePropertyName("test");
            WriteJson(writer, node.Test, serializer);

            writer.WritePropertyName("consequent");
            WriteJson(writer, node.Consequent, serializer);

            writer.WritePropertyName("alternate");
            WriteJson(writer, node.Alternate, serializer);

            writer.WriteEndObject();
        }

        protected virtual void WriteJson(JsonWriter writer, JSContinueStatement node, JsonSerializer serializer)
        {
            writer.WriteStartObject();

            writer.WritePropertyName("type");
            writer.WriteValue("ContinueStatement");

            writer.WritePropertyName("label");
            if (node.Label is null)
                writer.WriteNull();
            else
                WriteJson(writer, node.Label, serializer);

            writer.WriteEndObject();
        }

        protected virtual void WriteJson(JsonWriter writer, JSDebuggerStatement node, JsonSerializer serializer)
        {
            writer.WriteStartObject();

            writer.WritePropertyName("type");
            writer.WriteValue("DebuggerStatement");

            writer.WriteEndObject();
        }

        protected virtual void WriteJson(JsonWriter writer, JSDoWhileStatement node, JsonSerializer serializer)
        {
            writer.WriteStartObject();

            writer.WritePropertyName("type");
            writer.WriteValue("DoWhileStatement");

            writer.WritePropertyName("body");
            WriteJson(writer, node.Body, serializer);

            writer.WritePropertyName("test");
            WriteJson(writer, node.Test, serializer);

            writer.WriteEndObject();
        }

        protected virtual void WriteJson(JsonWriter writer, JSExpressionStatement node, JsonSerializer serializer)
        {
            writer.WriteStartObject();

            writer.WritePropertyName("type");
            writer.WriteValue("ExpressionStatement");

            writer.WritePropertyName("expression");
            WriteJson(writer, node.Expression, serializer);

            if (node.Expression is JSLiteral literal &&
                literal.Value is string directive)
            {
                writer.WritePropertyName("directive");
                writer.WriteValue(directive);
            }

            writer.WriteEndObject();
        }

        protected virtual void WriteJson(JsonWriter writer, JSForInStatement node, JsonSerializer serializer)
        {
            writer.WriteStartObject();

            writer.WritePropertyName("type");
            writer.WriteValue("ForInStatement");

            writer.WritePropertyName("left");

            switch (node.Left)
            {
                case var d when d.IsItem1:
                    WriteJson(writer, d.Item1, serializer);
                    break;
                case var d when d.IsItem2:
                    WriteJson(writer, d.Item2, serializer);
                    break;
                default:
                    WriteJson(writer, (JSExpression)null, serializer);
                    break;
            }

            writer.WritePropertyName("right");
            WriteJson(writer, node.Right, serializer);

            writer.WritePropertyName("body");
            WriteJson(writer, node.Body, serializer);

            writer.WriteEndObject();
        }

        protected virtual void WriteJson(JsonWriter writer, JSForStatement node, JsonSerializer serializer)
        {
            writer.WriteStartObject();

            writer.WritePropertyName("type");
            writer.WriteValue("ForStatement");

            writer.WritePropertyName("init");

            switch (node.Initializer)
            {
                case var d when d.IsItem1:
                    WriteJson(writer, d.Item1, serializer);
                    break;
                case var d when d.IsItem2:
                    WriteJson(writer, d.Item2, serializer);
                    break;
                default:
                    WriteJson(writer, (JSExpression)null, serializer);
                    break;
            }

            writer.WritePropertyName("test");
            WriteJson(writer, node.Test, serializer);

            writer.WritePropertyName("update");
            WriteJson(writer, node.Update, serializer);

            writer.WritePropertyName("body");
            WriteJson(writer, node.Body, serializer);

            writer.WriteEndObject();
        }

        protected virtual void WriteJson(JsonWriter writer, JSFunctionDeclaration node, JsonSerializer serializer)
        {
            writer.WriteStartObject();

            writer.WritePropertyName("type");
            writer.WriteValue("FunctionDeclaration");

            writer.WritePropertyName("id");
            if (node.Identifier is null)
                writer.WriteNull();
            else
                WriteJson(writer, node.Identifier, serializer);

            writer.WritePropertyName("params");
            writer.WriteStartArray();

            for (var i = 0; i < node.Parameters.Count; i++)
                WriteJson(writer, node.Parameters[i], serializer);

            writer.WriteEndArray();

            writer.WritePropertyName("body");
            WriteJson(writer, node.Body, serializer);

            writer.WriteEndObject();
        }

        protected virtual void WriteJson(JsonWriter writer, JSFunctionExpression node, JsonSerializer serializer)
        {
            writer.WriteStartObject();

            writer.WritePropertyName("type");
            writer.WriteValue("FunctionExpression");

            writer.WritePropertyName("id");
            if (node.Identifier is null)
                writer.WriteNull();
            else
                WriteJson(writer, node.Identifier, serializer);

            writer.WritePropertyName("params");
            writer.WriteStartArray();

            for (var i = 0; i < node.Parameters.Count; i++)
                WriteJson(writer, node.Parameters[i], serializer);

            writer.WriteEndArray();

            writer.WritePropertyName("body");
            WriteJson(writer, node.Body, serializer);

            writer.WriteEndObject();
        }

        protected virtual void WriteJson(JsonWriter writer, JSIdentifier node, JsonSerializer serializer)
        {
            writer.WriteStartObject();

            writer.WritePropertyName("type");
            writer.WriteValue("Identifier");

            writer.WritePropertyName("name");
            writer.WriteValue(node.Name);

            writer.WriteEndObject();
        }

        protected virtual void WriteJson(JsonWriter writer, JSIfStatement node, JsonSerializer serializer)
        {
            writer.WriteStartObject();

            writer.WritePropertyName("type");
            writer.WriteValue("IfStatement");

            writer.WritePropertyName("test");
            WriteJson(writer, node.Test, serializer);

            writer.WritePropertyName("consequent");
            WriteJson(writer, node.Body, serializer);

            writer.WritePropertyName("alternate");
            WriteJson(writer, node.Alternate, serializer);

            writer.WriteEndObject();
        }

        protected virtual void WriteJson(JsonWriter writer, JSLabeledStatement node, JsonSerializer serializer)
        {
            writer.WriteStartObject();

            writer.WritePropertyName("type");
            writer.WriteValue("LabeledStatement");

            writer.WritePropertyName("label");
            WriteJson(writer, node.Label, serializer);

            writer.WritePropertyName("body");
            WriteJson(writer, node.Body, serializer);

            writer.WriteEndObject();
        }

        protected virtual void WriteJson(JsonWriter writer, JSLiteral node, JsonSerializer serializer)
        {
            writer.WriteStartObject();

            writer.WritePropertyName("type");
            writer.WriteValue("Literal");

            if (node.Value is JSRegex regex)
            {
                writer.WritePropertyName("regex");
                WriteJson(writer, regex, serializer);
            }
            else
            {
                writer.WritePropertyName("value");
                writer.WriteValue(node.Value);
            }

            writer.WriteEndObject();
        }

        protected virtual void WriteJson(JsonWriter writer, JSRegex regex, JsonSerializer serializer)
        {
            writer.WriteStartObject();

            writer.WritePropertyName("pattern");
            writer.WriteValue(regex.Pattern);

            writer.WritePropertyName("flags");

            var flags = "";
            if (regex.Options.HasFlag(JSRegexOptions.Global)) flags += "g";
            if (regex.Options.HasFlag(JSRegexOptions.IgnoreCase)) flags += "i";
            if (regex.Options.HasFlag(JSRegexOptions.Multiline)) flags += "m";
            if (regex.Options.HasFlag(JSRegexOptions.Unicode)) flags += "u";
            if (regex.Options.HasFlag(JSRegexOptions.Sticky)) flags += "y";

            writer.WriteValue(flags);

            writer.WriteEndObject();
        }

        protected virtual void WriteJson(JsonWriter writer, JSMemberExpression node, JsonSerializer serializer)
        {
            if (node.Indices.Count == 0)
            {
                WriteJson(writer, node.Object, serializer);
                return;
            }

            writer.WriteStartObject();

            writer.WritePropertyName("type");
            writer.WriteValue("MemberExpression");

            writer.WritePropertyName("object");

            for (var i = 0; i < node.Indices.Count - 1; i++)
            {
                writer.WriteStartObject();

                writer.WritePropertyName("type");
                writer.WriteValue("MemberExpression");

                writer.WritePropertyName("object");
            }

            WriteJson(writer, node.Object, serializer);

            for (var i = 0; i < node.Indices.Count - 1; i++)
            {
                writer.WritePropertyName("property");
                WriteJson(writer, node.Indices[i], serializer);

                writer.WritePropertyName("computed");
                writer.WriteValue(node.IsComputed);

                writer.WriteEndObject();
            }

            writer.WritePropertyName("property");
            WriteJson(writer, node.Indices[node.Indices.Count - 1], serializer);

            writer.WritePropertyName("computed");
            writer.WriteValue(node.IsComputed);

            writer.WriteEndObject();
        }

        protected virtual void WriteJson(JsonWriter writer, JSNewExpression node, JsonSerializer serializer)
        {
            writer.WriteStartObject();

            writer.WritePropertyName("type");
            writer.WriteValue("NewExpression");

            writer.WritePropertyName("callee");
            WriteJson(writer, node.Callee, serializer);

            writer.WritePropertyName("arguments");
            writer.WriteStartArray();

            for (var i = 0; i < node.Arguments.Count; i++)
            {
                var item = node.Arguments[i];
                WriteJson(writer, item, serializer);
            }

            writer.WriteEndArray();

            writer.WriteEndObject();
        }

        protected virtual void WriteJson(JsonWriter writer, JSObjectExpression node, JsonSerializer serializer)
        {
            writer.WriteStartObject();

            writer.WritePropertyName("type");
            writer.WriteValue("ObjectExpression");

            writer.WritePropertyName("properties");
            writer.WriteStartArray();

            for (var i = 0; i < node.Properties.Count; i++)
            {
                var item = node.Properties[i];
                WriteJson(writer, item, serializer);
            }

            writer.WriteEndArray();

            writer.WriteEndObject();
        }

        protected virtual void WriteJson(JsonWriter writer, JSProgram node, JsonSerializer serializer)
        {
            writer.WriteStartObject();

            writer.WritePropertyName("type");
            writer.WriteValue("Program");

            writer.WritePropertyName("body");
            writer.WriteStartArray();

            for (var i = 0; i < node.Body.Count; i++)
            {
                var item = node.Body[i];
                WriteJson(writer, item, serializer);
            }

            writer.WriteEndArray();

            writer.WriteEndObject();
        }

        protected virtual void WriteJson(JsonWriter writer, JSProperty node, JsonSerializer serializer)
        {
            writer.WriteStartObject();

            writer.WritePropertyName("type");
            writer.WriteValue("Property");

            writer.WritePropertyName("key");
            switch (node.Key)
            {
                case var d when d.IsItem1:
                    WriteJson(writer, d.Item1, serializer);
                    break;
                case var d when d.IsItem2:
                    WriteJson(writer, d.Item2, serializer);
                    break;
                default:
                    WriteJson(writer, (JSExpression)null, serializer);
                    break;
            }

            writer.WritePropertyName("value");
            WriteJson(writer, node.Value, serializer);

            writer.WritePropertyName("kind");
            writer.WriteValue(ConvertPropertyKind(node.Kind));

            writer.WriteEndObject();
        }

        protected virtual string ConvertPropertyKind(JSPropertyKind kind)
        {
            switch (kind)
            {
                case JSPropertyKind.Initializer: return "init";
                case JSPropertyKind.Get: return "get";
                case JSPropertyKind.Set: return "set";
                default: throw new NotSupportedException($"Property kind {kind} is not supported.");
            }
        }

        protected virtual void WriteJson(JsonWriter writer, JSReturnStatement node, JsonSerializer serializer)
        {
            writer.WriteStartObject();

            writer.WritePropertyName("type");
            writer.WriteValue("ReturnStatement");

            writer.WritePropertyName("argument");
            WriteJson(writer, node.Expression, serializer);

            writer.WriteEndObject();
        }

        protected virtual void WriteJson(JsonWriter writer, JSSequenceExpression node, JsonSerializer serializer)
        {
            writer.WriteStartObject();

            writer.WritePropertyName("type");
            writer.WriteValue("SequenceExpression");

            writer.WritePropertyName("expressions");
            writer.WriteStartArray();

            for (var i = 0; i < node.Expressions.Count; i++)
                WriteJson(writer, node.Expressions[i], serializer);

            writer.WriteEndArray();

            writer.WriteEndObject();
        }

        protected virtual void WriteJson(JsonWriter writer, JSSwitchCase node, JsonSerializer serializer)
        {
            writer.WriteStartObject();

            writer.WritePropertyName("type");
            writer.WriteValue("SwitchCase");

            writer.WritePropertyName("test");
            WriteJson(writer, node.Test, serializer);

            writer.WritePropertyName("consequent");
            writer.WriteStartArray();

            for (var i = 0; i < node.Body.Count; i++)
                WriteJson(writer, node.Body[i], serializer);

            writer.WriteEndArray();

            writer.WriteEndObject();
        }

        protected virtual void WriteJson(JsonWriter writer, JSSwitchStatement node, JsonSerializer serializer)
        {
            writer.WriteStartObject();

            writer.WritePropertyName("type");
            writer.WriteValue("SwitchStatement");

            writer.WritePropertyName("discriminant");
            WriteJson(writer, node.Discriminant, serializer);

            writer.WritePropertyName("cases");
            writer.WriteStartArray();

            for (var i = 0; i < node.Cases.Count; i++)
                WriteJson(writer, node.Cases[i], serializer);

            writer.WriteEndArray();

            writer.WriteEndObject();
        }

        protected virtual void WriteJson(JsonWriter writer, JSThisExpression node, JsonSerializer serializer)
        {
            writer.WriteStartObject();

            writer.WritePropertyName("type");
            writer.WriteValue("ThisExpression");

            writer.WriteEndObject();
        }

        protected virtual void WriteJson(JsonWriter writer, JSThrowStatement node, JsonSerializer serializer)
        {
            writer.WriteStartObject();

            writer.WritePropertyName("type");
            writer.WriteValue("ThrowStatement");

            writer.WritePropertyName("argument");
            WriteJson(writer, node.Expression, serializer);

            writer.WriteEndObject();
        }

        protected virtual void WriteJson(JsonWriter writer, JSTryStatement node, JsonSerializer serializer)
        {
            writer.WriteStartObject();

            writer.WritePropertyName("type");
            writer.WriteValue("TryStatement");

            writer.WritePropertyName("block");
            WriteJson(writer, node.Body, serializer);

            writer.WritePropertyName("handler");
            if (node.Handler is null)
                writer.WriteNull();
            else
                WriteJson(writer, node.Handler, serializer);

            writer.WritePropertyName("finalizer");
            if (!(node.Handler is null) && node.Finalizer.Body.Count == 0)
                writer.WriteNull();
            else
                WriteJson(writer, node.Finalizer, serializer);

            writer.WriteEndObject();
        }

        protected virtual void WriteJson(JsonWriter writer, JSUnaryExpression node, JsonSerializer serializer)
        {
            writer.WriteStartObject();

            var (type, op, prefix) = ConvertUnaryOperator(node.Operator);

            writer.WritePropertyName("type");
            writer.WriteValue(type);

            writer.WritePropertyName("operator");
            writer.WriteValue(op);

            writer.WritePropertyName("prefix");
            writer.WriteValue(prefix);

            writer.WritePropertyName("argument");
            WriteJson(writer, node.Expression, serializer);

            writer.WriteEndObject();
        }

        protected virtual (string Type, string Operator, bool Prefix) ConvertUnaryOperator(JSUnaryOperator @operator)
        {
            switch (@operator)
            {
                case JSUnaryOperator.Negative: return ("UnaryExpression", "-", true);
                case JSUnaryOperator.Positive: return ("UnaryExpression", "+", true);
                case JSUnaryOperator.LogicalNot: return ("UnaryExpression", "!", true);
                case JSUnaryOperator.BitwiseNot: return ("UnaryExpression", "~", true);
                case JSUnaryOperator.TypeOf: return ("UnaryExpression", "typeof", true);
                case JSUnaryOperator.Void: return ("UnaryExpression", "void", true);
                case JSUnaryOperator.Delete: return ("UnaryExpression", "delete", true);
                case JSUnaryOperator.PreIncrement: return ("UpdateExpression", "++", true);
                case JSUnaryOperator.PreDecrement: return ("UpdateExpression", "--", true);
                case JSUnaryOperator.PostIncrement: return ("UpdateExpression", "++", false);
                case JSUnaryOperator.PostDecrement: return ("UpdateExpression", "--", false);
                default: throw new NotSupportedException($"The unary operator {@operator} is not supported.");
            }
        }

        protected virtual void WriteJson(JsonWriter writer, JSVariableDeclaration node, JsonSerializer serializer)
        {
            writer.WriteStartObject();

            writer.WritePropertyName("type");
            writer.WriteValue("VariableDeclaration");

            writer.WritePropertyName("declarations");
            writer.WriteStartArray();

            for (var i = 0; i < node.Declarations.Count; i++)
                WriteJson(writer, node.Declarations[i], serializer);

            writer.WriteEndArray();

            writer.WritePropertyName("kind");
            writer.WriteValue("var");

            writer.WriteEndObject();
        }

        protected virtual void WriteJson(JsonWriter writer, JSVariableDeclarator node, JsonSerializer serializer)
        {
            writer.WriteStartObject();

            writer.WritePropertyName("type");
            writer.WriteValue("VariableDeclarator");

            writer.WritePropertyName("id");
            WriteJson(writer, node.Identifier, serializer);

            writer.WritePropertyName("init");
            WriteJson(writer, node.Initializer, serializer);

            writer.WriteEndObject();
        }

        protected virtual void WriteJson(JsonWriter writer, JSWhileStatement node, JsonSerializer serializer)
        {
            writer.WriteStartObject();

            writer.WritePropertyName("type");
            writer.WriteValue("WhileStatement");

            writer.WritePropertyName("test");
            WriteJson(writer, node.Test, serializer);

            writer.WritePropertyName("body");
            WriteJson(writer, node.Body, serializer);

            writer.WriteEndObject();
        }

        protected virtual void WriteJson(JsonWriter writer, JSWithStatement node, JsonSerializer serializer)
        {
            writer.WriteStartObject();

            writer.WritePropertyName("type");
            writer.WriteValue("WithStatement");

            writer.WritePropertyName("object");
            WriteJson(writer, node.Object, serializer);

            writer.WritePropertyName("body");
            WriteJson(writer, node.Body, serializer);

            writer.WriteEndObject();
        }
    }
}
