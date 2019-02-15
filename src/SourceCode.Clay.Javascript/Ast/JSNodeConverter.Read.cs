using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace SourceCode.Clay.Javascript.Ast
{
    partial class JSNodeConverter
    {
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var token = JToken.ReadFrom(reader);
            return ReadJson(token, existingValue, serializer);
        }

        protected virtual object ReadJson(JToken token, object existingValue, JsonSerializer serializer)
        {
            if (token is null) return null;
            if (token.Type == JTokenType.Null) return null;
            if (!(token is JObject obj))
                throw new InvalidOperationException("Javascript AST nodes must be null or JSON objects.");
            if (!obj.TryGetValue("type", out JToken typeToken) || typeToken.Type != JTokenType.String)
                throw new InvalidOperationException("Javascript AST nodes must contain a type property.");

            return ReadJson(obj, (string)typeToken, existingValue, serializer);
        }

        protected virtual void ReadArray<T>(JArray array, IList<T> list, JsonSerializer serializer, bool allowNull = false)
        {
            var i = 0;
            var end = Math.Min(array.Count, list.Count);
            for (; i < end; i++)
            {
                JToken item = array[i];

                if (!allowNull &&
                    (item is null || item.Type == JTokenType.Null))
                    throw new InvalidOperationException($"Elements can not be null.");

                var element = ReadJson(item, list[i], serializer);
                if (!(element is T) && !(element is null))
                    throw new InvalidOperationException($"Elements must be null or {typeof(T)}.");

                list[i] = (T)element;
            }

            for (; i < array.Count; i++)
            {
                JToken item = array[i];

                if (!allowNull &&
                    (item is null || item.Type == JTokenType.Null))
                    throw new InvalidOperationException($"Elements can not be null.");

                var element = ReadJson(item, null, serializer);
                if (!(element is T) && !(element is null))
                    throw new InvalidOperationException($"Elements must be null or {typeof(T)}.");

                list.Add((T)element);
            }

            for (var j = list.Count - 1; j >= i; j--)
                list.RemoveAt(j);
        }

        protected virtual object ReadJson(JObject @object, string typeToken, object existingValue, JsonSerializer serializer)
        {
            switch (typeToken)
            {
                case "ArrayExpression": return ReadArrayExpression(@object, existingValue as JSArrayExpression, serializer);
                case "AssignmentExpression":
                case "BinaryExpression":
                case "LogicalExpression": return ReadBinaryExpression(@object, existingValue as JSBinaryExpression, serializer);
                case "BlockStatement": return ReadBlockStatement(@object, existingValue as JSBlockStatement, serializer);
                case "BreakStatement": return ReadBreakStatement(@object, existingValue as JSBreakStatement, serializer);
                case "CallExpression": return ReadCallExpression(@object, existingValue as JSCallExpression, serializer);
                case "CatchClause": return ReadCatchClause(@object, existingValue as JSCatchClause, serializer);
                case "ConditionalExpression": return ReadConditionalExpression(@object, existingValue as JSConditionalExpression, serializer);
                case "ContinueStatement": return ReadContinueStatement(@object, existingValue as JSContinueStatement, serializer);
                case "DebuggerStatement": return ReadDebuggerStatement(@object, existingValue as JSDebuggerStatement, serializer);
                case "DoWhileStatement": return ReadDoWhileStatement(@object, existingValue as JSDoWhileStatement, serializer);
                case "EmptyStatement": return ReadEmptyStatement(@object, existingValue, serializer);
                case "ExpressionStatement": return ReadExpressionStatement(@object, existingValue as JSExpressionStatement, serializer);
                case "ForInStatement": return ReadForInStatement(@object, existingValue as JSForInStatement, serializer);
                case "ForStatement": return ReadForStatement(@object, existingValue as JSForStatement, serializer);
                case "FunctionDeclaration": return ReadFunctionDeclaration(@object, existingValue as JSFunctionDeclaration, serializer);
                case "FunctionExpression": return ReadFunctionExpression(@object, existingValue as JSFunctionExpression, serializer);
                case "Identifier": return ReadIdentifier(@object, existingValue as JSIdentifier, serializer);
                case "IfStatement": return ReadIfStatement(@object, existingValue as JSIfStatement, serializer);
                case "LabeledStatement": return ReadLabeledStatement(@object, existingValue as JSLabeledStatement, serializer);
                case "Literal": return ReadLiteral(@object, existingValue as JSLiteral, serializer);
                case "MemberExpression": return ReadMemberExpression(@object, existingValue as JSMemberExpression, serializer);
                case "NewExpression": return ReadNewExpression(@object, existingValue as JSNewExpression, serializer);
                case "ObjectExpression": return ReadObjectExpression(@object, existingValue as JSObjectExpression, serializer);
                case "Program": return ReadProgram(@object, existingValue as JSProgram, serializer);
                case "Property": return ReadProperty(@object, existingValue as JSProperty, serializer);
                case "ReturnStatement": return ReadReturnStatement(@object, existingValue as JSReturnStatement, serializer);
                case "SequenceExpression": return ReadSequenceExpression(@object, existingValue as JSSequenceExpression, serializer);
                case "SwitchCase": return ReadSwitchCase(@object, existingValue as JSSwitchCase, serializer);
                case "SwitchStatement": return ReadSwitchStatement(@object, existingValue as JSSwitchStatement, serializer);
                case "ThisExpression": return ReadThisExpression(@object, existingValue as JSThisExpression, serializer);
                case "ThrowStatement": return ReadThrowStatement(@object, existingValue as JSThrowStatement, serializer);
                case "TryStatement": return ReadTryStatement(@object, existingValue as JSTryStatement, serializer);
                case "UpdateExpression":
                case "UnaryExpression": return ReadUnaryExpression(@object, existingValue as JSUnaryExpression, serializer);
                case "VariableDeclaration": return ReadVariableDeclaration(@object, existingValue as JSVariableDeclaration, serializer);
                case "VariableDeclarator": return ReadVariableDeclarator(@object, existingValue as JSVariableDeclarator, serializer);
                case "WhileStatement": return ReadWhileStatement(@object, existingValue as JSWhileStatement, serializer);
                case "WithStatement": return ReadWithStatement(@object, existingValue as JSWithStatement, serializer);
                default: throw new NotSupportedException($"The AST node type {typeToken} is not supported.");
            }
        }

        protected virtual JSArrayExpression ReadArrayExpression(JObject @object, JSArrayExpression existingValue, JsonSerializer serializer)
        {
            if (!@object.TryGetValue("elements", out JToken elementsToken) || !(elementsToken is JArray elementsArray))
                throw new InvalidOperationException("ArrayExpression must have elements property of type array.");

            if (existingValue is null) existingValue = new JSArrayExpression();

            ReadArray(elementsArray, existingValue.Elements, serializer, true);

            return existingValue;
        }

        protected virtual JSLiteral ReadLiteral(JObject @object, JSLiteral existingValue, JsonSerializer serializer)
        {
            if (@object.TryGetValue("regex", out JToken regexToken))
            {
                if (!(regexToken is JObject regexObject))
                    throw new InvalidOperationException("If regex is present on Literal AST, it must be an object.");
                return ReadRegexLiteral(regexObject, existingValue, serializer);
            }

            if (!@object.TryGetValue("value", out JToken valueToken) || !(valueToken is JValue valueValue))
                throw new InvalidOperationException("If regex is not present on Literal AST, value must be present.");

            if (existingValue is null) existingValue = new JSLiteral();
            existingValue.Value = valueValue.Value;

            return existingValue;
        }

        protected virtual JSLiteral ReadRegexLiteral(JObject @object, JSLiteral existingValue, JsonSerializer serializer)
        {
            if (!@object.TryGetValue("pattern", out JToken patternToken) || !(patternToken.Type == JTokenType.String))
                throw new InvalidOperationException("On a regex Literal AST, regex.pattern must be present and be a string.");
            if (!@object.TryGetValue("flags", out JToken flagsToken) || !(flagsToken.Type == JTokenType.String))
                throw new InvalidOperationException("On a regex Literal AST, regex.flags must be present and be a string.");

            var pattern = (string)patternToken;
            var flags = (string)flagsToken;

            JSRegexOptions options = JSRegexOptions.None;
            for (var i = 0; i < flags.Length; i++)
            {
                switch (flags[i])
                {
                    case 'g': options |= JSRegexOptions.Global; break;
                    case 'i': options |= JSRegexOptions.IgnoreCase; break;
                    case 'm': options |= JSRegexOptions.Multiline; break;
                    case 'u': options |= JSRegexOptions.Unicode; break;
                    case 'y': options |= JSRegexOptions.Sticky; break;
                    default: throw new NotSupportedException($"Regex flag {flags[i]} is not supported.");
                }
            }

            if (existingValue is null) existingValue = new JSLiteral();
            existingValue.Value = new JSRegex(pattern, options);

            return existingValue;
        }

        protected virtual JSBinaryExpression ReadBinaryExpression(JObject @object, JSBinaryExpression existingValue, JsonSerializer serializer)
        {
            if (!@object.TryGetValue("left", out JToken leftToken) || !(leftToken.Type == JTokenType.Object))
                throw new InvalidOperationException("BinaryExpression must have left property of type object.");
            if (!@object.TryGetValue("operator", out JToken operatorToken) || !(operatorToken.Type == JTokenType.String))
                throw new InvalidOperationException("BinaryExpression must have operator property of type string.");
            if (!@object.TryGetValue("right", out JToken rightToken) || !(rightToken.Type == JTokenType.Object))
                throw new InvalidOperationException("BinaryExpression must have right property of type object.");

            JSBinaryOperator @operator = ConvertBinaryOperator((string)operatorToken);
            var left = ReadJson(leftToken, existingValue?.Left, serializer) as JSExpression;
            var right = ReadJson(rightToken, null, serializer) as JSExpression;

            if (left is null)
                throw new InvalidOperationException("BinaryExpression nmust have left property of type expression.");
            if (right is null)
                throw new InvalidOperationException("BinaryExpression nmust have left property of type expression.");

            if (existingValue is null) existingValue = new JSBinaryExpression();
            existingValue.Right.Clear();

            existingValue.Operator = @operator;
            existingValue.Left = left;
            existingValue.Add(right);

            return existingValue;
        }

        protected virtual JSBinaryOperator ConvertBinaryOperator(string @operator)
        {
            switch (@operator)
            {
                case "==": return JSBinaryOperator.CoercedEquality;
                case "!=": return JSBinaryOperator.CoercedInequality;
                case "===": return JSBinaryOperator.IdentityEquality;
                case "!==": return JSBinaryOperator.IdentityInequality;
                case "<": return JSBinaryOperator.LessThan;
                case "<=": return JSBinaryOperator.LessThanOrEqual;
                case ">": return JSBinaryOperator.GreaterThan;
                case ">=": return JSBinaryOperator.GreaterThanOrEqual;
                case "<<": return JSBinaryOperator.UnsignedLeftShift;
                case ">>": return JSBinaryOperator.SignedRightShift;
                case ">>>": return JSBinaryOperator.UnsignedRightShift;
                case "+": return JSBinaryOperator.Add;
                case "-": return JSBinaryOperator.Subtract;
                case "*": return JSBinaryOperator.Multiply;
                case "/": return JSBinaryOperator.Divide;
                case "%": return JSBinaryOperator.Modulus;
                case "|": return JSBinaryOperator.BitwiseOr;
                case "^": return JSBinaryOperator.BitwiseXor;
                case "&": return JSBinaryOperator.BitwiseAnd;
                case "in": return JSBinaryOperator.In;
                case "instanceof": return JSBinaryOperator.InstanceOf;

                case "=": return JSBinaryOperator.Assign;
                case "+=": return JSBinaryOperator.AddAssign;
                case "-=": return JSBinaryOperator.SubtractAssign;
                case "*=": return JSBinaryOperator.MultiplyAssign;
                case "/=": return JSBinaryOperator.DivideAssign;
                case "%=": return JSBinaryOperator.ModulusAssign;
                case "<<=": return JSBinaryOperator.UnsignedLeftShiftAssign;
                case ">>=": return JSBinaryOperator.SignedRightShiftAssign;
                case ">>>=": return JSBinaryOperator.UnsignedRightShiftAssign;
                case "|=": return JSBinaryOperator.BitwiseOrAssign;
                case "^=": return JSBinaryOperator.BitwiseXorAssign;
                case "&=": return JSBinaryOperator.BitwiseAndAssign;

                case "&&": return JSBinaryOperator.LogicalAnd;
                case "||": return JSBinaryOperator.LogicalOr;

                default: throw new InvalidOperationException();
            }
        }

        protected virtual JSBlockStatement ReadBlockStatement(JObject @object, JSBlockStatement existingValue, JsonSerializer serializer)
        {
            if (!@object.TryGetValue("body", out JToken bodyToken) || !(bodyToken is JArray bodyArray))
                throw new InvalidOperationException("Block must have body property of type array.");

            if (existingValue is null) existingValue = new JSBlockStatement();

            ReadArray(bodyArray, existingValue.Body, serializer);

            return existingValue;
        }

        protected virtual JSBreakStatement ReadBreakStatement(JObject @object, JSBreakStatement existingValue, JsonSerializer serializer)
        {
            if (!@object.TryGetValue("label", out JToken labelToken) || !(labelToken.Type == JTokenType.Object || labelToken.Type == JTokenType.Null))
                throw new InvalidOperationException("BreakStatement must have label property of type expression or null.");

            var label = ReadJson(labelToken, existingValue?.Label, serializer);

            if (!(label is null) && !(label is JSIdentifier))
                throw new InvalidOperationException("BreakStatement must have label property of type identifier or null.");

            if (existingValue is null) existingValue = new JSBreakStatement();

            existingValue.Label = (JSIdentifier)label;

            return existingValue;
        }

        protected virtual JSCallExpression ReadCallExpression(JObject @object, JSCallExpression existingValue, JsonSerializer serializer)
        {
            if (!@object.TryGetValue("callee", out JToken calleeToken) || !(calleeToken.Type == JTokenType.Object))
                throw new InvalidOperationException("CallExpression must have callee property of type object.");
            if (!@object.TryGetValue("arguments", out JToken argumentsToken) || !(argumentsToken is JArray argumentsArray))
                throw new InvalidOperationException("CallExpression must have arguments property of type array.");

            var callee = ReadJson(calleeToken, existingValue?.Callee, serializer);

            if (!(callee is JSExpression calleeExpression))
                throw new InvalidOperationException("CallExpression must have callee property of type expression.");

            if (existingValue is null) existingValue = new JSCallExpression();

            existingValue.Callee = calleeExpression;
            ReadArray(argumentsArray, existingValue.Arguments, serializer);

            return existingValue;
        }

        protected virtual JSCatchClause ReadCatchClause(JObject @object, JSCatchClause existingValue, JsonSerializer serializer)
        {
            if (!@object.TryGetValue("param", out JToken paramToken) || !(paramToken.Type == JTokenType.Object))
                throw new InvalidOperationException("CatchClause must have param property of type object.");
            if (!@object.TryGetValue("body", out JToken bodyToken) || !(bodyToken is JObject bodyObject) ||
                !bodyObject.TryGetValue("type", out JToken bodyTypeToken) || bodyTypeToken.Type != JTokenType.String || (string)bodyTypeToken != "BlockStatement" ||
                !bodyObject.TryGetValue("body", out bodyToken) || !(bodyToken is JArray bodyArray))
                throw new InvalidOperationException("CatchClause must have body property of type BlockStatement.");

            var param = ReadJson(paramToken, existingValue?.Parameter, serializer);

            if (!(param is IJSPattern paramPattern))
                throw new InvalidOperationException("CatchClause must have param property of type pattern.");

            if (existingValue is null) existingValue = new JSCatchClause();

            existingValue.Parameter = paramPattern;
            ReadArray(bodyArray, existingValue.Body, serializer);

            return existingValue;
        }

        protected virtual JSConditionalExpression ReadConditionalExpression(JObject @object, JSConditionalExpression existingValue, JsonSerializer serializer)
        {
            if (!@object.TryGetValue("test", out JToken testToken) || !(testToken.Type == JTokenType.Object))
                throw new InvalidOperationException("ConditionalExpression must have test property of type object.");
            if (!@object.TryGetValue("consequent", out JToken consToken) || !(consToken.Type == JTokenType.Object))
                throw new InvalidOperationException("ConditionalExpression must have consequent property of type object.");
            if (!@object.TryGetValue("alternate", out JToken altToken) || !(altToken.Type == JTokenType.Object))
                throw new InvalidOperationException("ConditionalExpression must have alternate property of type object.");

            var test = ReadJson(testToken, existingValue?.Test, serializer);
            var cons = ReadJson(consToken, existingValue?.Consequent, serializer);
            var alt = ReadJson(altToken, existingValue?.Alternate, serializer);

            if (!(test is JSExpression testExpression))
                throw new InvalidOperationException("ConditionalExpression must have test property of type expression.");
            if (!(cons is JSExpression consExpression))
                throw new InvalidOperationException("ConditionalExpression must have consequent property of type expression.");
            if (!(alt is JSExpression altExpression))
                throw new InvalidOperationException("ConditionalExpression must have alternate property of type expression.");

            if (existingValue is null) existingValue = new JSConditionalExpression();

            existingValue.Test = testExpression;
            existingValue.Consequent = consExpression;
            existingValue.Alternate = altExpression;

            return existingValue;
        }

        protected virtual JSContinueStatement ReadContinueStatement(JObject @object, JSContinueStatement existingValue, JsonSerializer serializer)
        {
            if (!@object.TryGetValue("label", out JToken labelToken) || !(labelToken.Type == JTokenType.Object || labelToken.Type == JTokenType.Null))
                throw new InvalidOperationException("ContinueStatement must have label property of type expression or null.");

            var label = ReadJson(labelToken, existingValue?.Label, serializer);

            if (!(label is null) && !(label is JSIdentifier))
                throw new InvalidOperationException("ContinueStatement must have label property of type identifier or null.");

            if (existingValue is null) existingValue = new JSContinueStatement();

            existingValue.Label = (JSIdentifier)label;

            return existingValue;
        }

        protected virtual JSDebuggerStatement ReadDebuggerStatement(JObject @object, JSDebuggerStatement existingValue, JsonSerializer serializer)
        {
            return existingValue ?? new JSDebuggerStatement();
        }

        protected virtual JSDoWhileStatement ReadDoWhileStatement(JObject @object, JSDoWhileStatement existingValue, JsonSerializer serializer)
        {
            if (!@object.TryGetValue("test", out JToken testToken) || !(testToken.Type == JTokenType.Object))
                throw new InvalidOperationException("DoWhileStatement must have property test of type object.");
            if (!@object.TryGetValue("body", out JToken bodyToken) || !(testToken.Type == JTokenType.Object))
                throw new InvalidOperationException("DoWhileStatement must have property body of type object.");

            var test = ReadJson(testToken, existingValue?.Test, serializer);
            var body = ReadJson(bodyToken, existingValue?.Body, serializer);

            if (!(test is JSExpression testExpression))
                throw new InvalidOperationException("DoWhileStatement must have property test of type expression.");
            if (!(body is null) && !(body is JSStatement))
                throw new InvalidOperationException("DoWhileStatement must have property body of type statement.");

            if (existingValue is null) existingValue = new JSDoWhileStatement();

            existingValue.Test = testExpression;
            existingValue.Body = (JSStatement)body;

            return existingValue;
        }

        protected virtual JSExpressionStatement ReadExpressionStatement(JObject @object, JSExpressionStatement existingValue, JsonSerializer serializer)
        {
            if (!@object.TryGetValue("expression", out JToken expressionToken) || !(expressionToken.Type == JTokenType.Object))
                throw new InvalidOperationException("ExpressionStatement must have property expression of type object.");

            var expression = ReadJson(expressionToken, existingValue?.Expression, serializer);

            if (!(expression is JSExpression expressionExpression))
                throw new InvalidOperationException("ExpressionStatement must have property expression of type expression.");

            if (existingValue is null) existingValue = new JSExpressionStatement();
            existingValue.Expression = expressionExpression;

            return existingValue;
        }

        protected virtual object ReadEmptyStatement(JObject @object, object existingValue, JsonSerializer serializer)
        {
            return null;
        }

        protected virtual JSForInStatement ReadForInStatement(JObject @object, JSForInStatement existingValue, JsonSerializer serializer)
        {
            if (!@object.TryGetValue("left", out JToken leftToken) || !(leftToken.Type == JTokenType.Object))
                throw new InvalidOperationException("ForInStatement must have property left of type object.");
            if (!@object.TryGetValue("right", out JToken rightToken) || !(rightToken.Type == JTokenType.Object))
                throw new InvalidOperationException("ForInStatement must have property right of type object.");
            if (!@object.TryGetValue("body", out JToken bodyToken) || !(bodyToken.Type == JTokenType.Object))
                throw new InvalidOperationException("ForInStatement must have property body of type object.");

            var left = ReadJson(leftToken, existingValue?.Left, serializer);
            var right = ReadJson(rightToken, existingValue?.Right, serializer);
            var body = ReadJson(bodyToken, existingValue?.Body, serializer);

            if (!(left is IJSPatternDeclaration leftExpression))
                throw new InvalidOperationException("ForInStatement must have property left of type pattern or variable declaration.");
            if (!(right is JSExpression rightExpression))
                throw new InvalidOperationException("ForInStatement must have property right of type expression.");
            if (!(body is null) && !(body is JSStatement))
                throw new InvalidOperationException("ForInStatement must have property body of type statement.");

            if (existingValue is null) existingValue = new JSForInStatement();

            existingValue.Left = leftExpression;
            existingValue.Right = rightExpression;
            existingValue.Body = (JSStatement)body;

            return existingValue;
        }

        protected virtual JSForStatement ReadForStatement(JObject @object, JSForStatement existingValue, JsonSerializer serializer)
        {
            if (!@object.TryGetValue("init", out JToken initToken) || !(initToken.Type == JTokenType.Object || initToken.Type == JTokenType.Null))
                throw new InvalidOperationException("ForStatement must have property init of type object or null.");
            if (!@object.TryGetValue("test", out JToken testToken) || !(testToken.Type == JTokenType.Object || testToken.Type == JTokenType.Null))
                throw new InvalidOperationException("ForStatement must have property right of type object or null.");
            if (!@object.TryGetValue("update", out JToken updateToken) || !(updateToken.Type == JTokenType.Object || updateToken.Type == JTokenType.Null))
                throw new InvalidOperationException("ForStatement must have property update of type object or null.");
            if (!@object.TryGetValue("body", out JToken bodyToken) || !(bodyToken.Type == JTokenType.Object))
                throw new InvalidOperationException("ForStatement must have property body of type object.");

            var init = ReadJson(initToken, existingValue?.Initializer, serializer);
            var test = ReadJson(testToken, existingValue?.Test, serializer);
            var update = ReadJson(updateToken, existingValue?.Update, serializer);
            var body = ReadJson(bodyToken, existingValue?.Body, serializer);

            if (!(init is IJSInitializer) && !(init is null))
                throw new InvalidOperationException("ForStatement must have property init of type expression, variable declaration or null.");
            if (!(test is JSExpression) && !(test is null))
                throw new InvalidOperationException("ForStatement must have property test of type expression or null.");
            if (!(update is JSExpression) && !(update is null))
                throw new InvalidOperationException("ForStatement must have property update of type expression or null.");
            if (!(body is null) && !(body is JSStatement))
                throw new InvalidOperationException("ForStatement must have property body of type statement.");

            if (existingValue is null) existingValue = new JSForStatement();

            existingValue.Initializer = (IJSInitializer)init;
            existingValue.Test = (JSExpression)test;
            existingValue.Update = (JSExpression)update;
            existingValue.Body = (JSStatement)body;

            return existingValue;
        }

        protected virtual JSFunctionDeclaration ReadFunctionDeclaration(JObject @object, JSFunctionDeclaration existingValue, JsonSerializer serializer)
        {
            if (!@object.TryGetValue("id", out JToken idToken) || !(idToken.Type == JTokenType.Object))
                throw new InvalidOperationException("FunctionDeclaration must have property id of type object.");
            if (!@object.TryGetValue("params", out JToken paramsToken) || !(paramsToken is JArray paramsArray))
                throw new InvalidOperationException("FunctionDeclaration must have property params of type array.");
            if (!@object.TryGetValue("body", out JToken bodyToken) || !(bodyToken is JObject bodyObject) ||
                !bodyObject.TryGetValue("type", out JToken bodyTypeToken) || !(bodyTypeToken.Type == JTokenType.String && (string)bodyTypeToken == "BlockStatement") ||
                !bodyObject.TryGetValue("body", out bodyToken) || !(bodyToken is JArray bodyArray))
                throw new InvalidOperationException("FunctionDeclaration must have property body of block statement.");

            var id = ReadJson(idToken, existingValue?.Identifier, serializer);

            if (!(id is JSIdentifier idIdentifier))
                throw new InvalidOperationException("FunctionDeclaration must have property id of type identifier.");

            if (existingValue is null) existingValue = new JSFunctionDeclaration();

            existingValue.Identifier = idIdentifier;
            ReadArray(paramsArray, existingValue.Parameters, serializer);
            ReadArray(bodyArray, existingValue.Body, serializer);

            return existingValue;
        }

        protected virtual JSFunctionExpression ReadFunctionExpression(JObject @object, JSFunctionExpression existingValue, JsonSerializer serializer)
        {
            if (!@object.TryGetValue("id", out JToken idToken) || !(idToken.Type == JTokenType.Object || idToken.Type == JTokenType.Null))
                throw new InvalidOperationException("FunctionExpression must have property id of type object or null.");
            if (!@object.TryGetValue("params", out JToken paramsToken) || !(paramsToken is JArray paramsArray))
                throw new InvalidOperationException("FunctionExpression must have property params of type array.");
            if (!@object.TryGetValue("body", out JToken bodyToken) || !(bodyToken is JObject bodyObject) ||
                !bodyObject.TryGetValue("type", out JToken bodyTypeToken) || !(bodyTypeToken.Type == JTokenType.String && (string)bodyTypeToken == "BlockStatement") ||
                !bodyObject.TryGetValue("body", out bodyToken) || !(bodyToken is JArray bodyArray))
                throw new InvalidOperationException("FunctionExpression must have property body of block statement.");

            var id = ReadJson(idToken, existingValue?.Identifier, serializer);

            if (!(id is null) && !(id is JSIdentifier))
                throw new InvalidOperationException("FunctionExpression must have property id of type identifier or null.");

            if (existingValue is null) existingValue = new JSFunctionExpression();

            existingValue.Identifier = (JSIdentifier)id;
            ReadArray(paramsArray, existingValue.Parameters, serializer);
            ReadArray(bodyArray, existingValue.Body, serializer);

            return existingValue;
        }

        protected virtual JSIdentifier ReadIdentifier(JObject @object, JSIdentifier existingValue, JsonSerializer serializer)
        {
            if (!@object.TryGetValue("name", out JToken nameToken) || !(nameToken.Type == JTokenType.String))
                throw new InvalidOperationException("Identifier must have name property of type string.");

            if (existingValue is null) existingValue = new JSIdentifier();

            existingValue.Name = (string)nameToken;

            return existingValue;
        }

        protected virtual JSIfStatement ReadIfStatement(JObject @object, JSIfStatement existingValue, JsonSerializer serializer)
        {
            if (!@object.TryGetValue("test", out JToken testToken) || !(testToken.Type == JTokenType.Object))
                throw new InvalidOperationException("IfStatement must have property test of type object.");
            if (!@object.TryGetValue("consequent", out JToken consequentToken) || !(testToken.Type == JTokenType.Object))
                throw new InvalidOperationException("IfStatement must have property consequent of type object.");
            if (!@object.TryGetValue("alternate", out JToken alternateToken) || !(alternateToken.Type == JTokenType.Object || alternateToken.Type == JTokenType.Null))
                throw new InvalidOperationException("IfStatement must have property alternate of type object or null.");

            var test = ReadJson(testToken, existingValue?.Test, serializer);
            var body = ReadJson(consequentToken, existingValue?.Body, serializer);
            var alternate = ReadJson(alternateToken, existingValue?.Alternate, serializer);

            if (!(test is JSExpression testExpression))
                throw new InvalidOperationException("IfStatement must have property test of type expression.");
            if (!(body is null) && !(body is JSStatement))
                throw new InvalidOperationException("IfStatement must have property consequent of type statement.");
            if (!(alternate is null) && !(alternate is JSStatement))
                throw new InvalidOperationException("IfStatement must have property alternate of type statement or null.");

            if (existingValue is null) existingValue = new JSIfStatement();

            existingValue.Test = testExpression;
            existingValue.Body = (JSStatement)body;
            existingValue.Alternate = (JSStatement)alternate;

            return existingValue;
        }

        protected virtual JSLabeledStatement ReadLabeledStatement(JObject @object, JSLabeledStatement existingValue, JsonSerializer serializer)
        {
            if (!@object.TryGetValue("label", out JToken labelToken) || !(labelToken.Type == JTokenType.Object))
                throw new InvalidOperationException("LabeledStatement must have property label of type object.");
            if (!@object.TryGetValue("body", out JToken bodyToken) || !(bodyToken.Type == JTokenType.Object))
                throw new InvalidOperationException("LabeledStatement must have property body of type object.");

            var label = ReadJson(labelToken, existingValue?.Label, serializer);
            var body = ReadJson(bodyToken, existingValue?.Body, serializer);

            if (!(label is JSIdentifier labelIdentifier))
                throw new InvalidOperationException("LabeledStatement must have property test of type identifier.");
            if (!(body is null) && !(body is JSStatement))
                throw new InvalidOperationException("LabeledStatement must have property body of type statement.");

            if (existingValue is null) existingValue = new JSLabeledStatement();

            existingValue.Label = labelIdentifier;
            existingValue.Body = (JSStatement)body;

            return existingValue;
        }

        protected virtual JSMemberExpression ReadMemberExpression(JObject @object, JSMemberExpression existingValue, JsonSerializer serializer)
        {
            if (!@object.TryGetValue("computed", out JToken computedToken) || !(computedToken.Type == JTokenType.Boolean))
                throw new InvalidOperationException("MemberExpression must have property computed of type boolean.");
            if (!@object.TryGetValue("object", out JToken objectToken) || !(objectToken.Type == JTokenType.Object))
                throw new InvalidOperationException("MemberExpression must have property object of type object.");
            if (!@object.TryGetValue("property", out JToken propertyToken) || !(propertyToken.Type == JTokenType.Object))
                throw new InvalidOperationException("MemberExpression must have property property of type object.");

            var @objectV = ReadJson(objectToken, existingValue?.Object, serializer);
            var property = ReadJson(propertyToken, existingValue?.Indices, serializer);

            if (!(@objectV is JSExpression objectExpression))
                throw new InvalidOperationException("MemberExpression must have property object of type expression.");
            if (!(property is JSExpression propertyExpression))
                throw new InvalidOperationException("MemberExpression must have property property of type expression.");

            if (existingValue is null) existingValue = new JSMemberExpression();
            existingValue.Indices.Clear();

            existingValue.IsComputed = (bool)computedToken;
            existingValue.Object = objectExpression;
            existingValue.Add(propertyExpression);

            return existingValue;
        }

        protected virtual JSNewExpression ReadNewExpression(JObject @object, JSNewExpression existingValue, JsonSerializer serializer)
        {
            if (!@object.TryGetValue("callee", out JToken calleeToken) || !(calleeToken.Type == JTokenType.Object))
                throw new InvalidOperationException("NewExpression must have callee property of type object.");
            if (!@object.TryGetValue("arguments", out JToken argumentsToken) || !(argumentsToken is JArray argumentsArray))
                throw new InvalidOperationException("NewExpression must have arguments property of type array.");

            var callee = ReadJson(calleeToken, existingValue?.Callee, serializer);

            if (!(callee is JSExpression calleeExpression))
                throw new InvalidOperationException("NewExpression must have callee property of type expression.");

            if (existingValue is null) existingValue = new JSNewExpression();

            existingValue.Callee = calleeExpression;
            ReadArray(argumentsArray, existingValue.Arguments, serializer);

            return existingValue;
        }

        protected virtual JSObjectExpression ReadObjectExpression(JObject @object, JSObjectExpression existingValue, JsonSerializer serializer)
        {
            if (!@object.TryGetValue("properties", out JToken propertiesToken) || !(propertiesToken is JArray propertiesArray))
                throw new InvalidOperationException("ObjectExpression must have properties property of type array.");

            if (existingValue is null) existingValue = new JSObjectExpression();

            ReadArray(propertiesArray, existingValue.Properties, serializer);

            return existingValue;
        }

        protected virtual JSProgram ReadProgram(JObject @object, JSProgram existingValue, JsonSerializer serializer)
        {
            if (!@object.TryGetValue("body", out JToken bodyToken) || !(bodyToken is JArray bodyArray))
                throw new InvalidOperationException("Program must have property body of type array.");

            if (existingValue is null) existingValue = new JSProgram();

            ReadArray(bodyArray, existingValue.Body, serializer);

            return existingValue;
        }

        protected virtual JSProperty ReadProperty(JObject @object, JSProperty existingValue, JsonSerializer serializer)
        {
            if (!@object.TryGetValue("kind", out JToken kindToken) || !(kindToken.Type == JTokenType.String))
                throw new InvalidOperationException("Property must have kind property of type string.");
            if (!@object.TryGetValue("key", out JToken keyToken) || !(keyToken.Type == JTokenType.Object))
                throw new InvalidOperationException("Property must have key property of type object.");
            if (!@object.TryGetValue("value", out JToken valueToken) || !(valueToken.Type == JTokenType.Object))
                throw new InvalidOperationException("Property must have value property of type object.");

            JSPropertyKind kind = ConvertPropertyKind((string)kindToken);
            var key = ReadJson(keyToken, existingValue?.Key, serializer);
            var value = ReadJson(valueToken, existingValue?.Value, serializer);

            if (!(key is IJSIndexer keyIndexer))
                throw new InvalidOperationException("Property must have key property of type literal or identifier.");
            if (!(value is JSExpression valueExpression))
                throw new InvalidOperationException("Property must have value property of type expression.");

            if (existingValue is null) existingValue = new JSProperty();

            existingValue.Key = keyIndexer;
            existingValue.Value = valueExpression;
            existingValue.Kind = kind;

            return existingValue;
        }

        protected virtual JSPropertyKind ConvertPropertyKind(string @operator)
        {
            switch (@operator)
            {
                case "init": return JSPropertyKind.Initializer;
                case "get": return JSPropertyKind.Get;
                case "set": return JSPropertyKind.Set;
                default: throw new InvalidOperationException();
            }
        }

        protected virtual JSReturnStatement ReadReturnStatement(JObject @object, JSReturnStatement existingValue, JsonSerializer serializer)
        {
            if (!@object.TryGetValue("argument", out JToken argumentToken) || !(argumentToken.Type == JTokenType.Object || argumentToken.Type == JTokenType.Null))
                throw new InvalidOperationException("ReturnStatement must have argument property of type object or null.");

            var argument = ReadJson(argumentToken, existingValue?.Expression, serializer);

            if (!(argument is null) && !(argument is JSExpression))
                throw new InvalidOperationException("ReturnStatement must have argument property of type expression or null.");

            if (existingValue is null) existingValue = new JSReturnStatement();

            existingValue.Expression = (JSExpression)argument;

            return existingValue;
        }

        protected virtual JSSequenceExpression ReadSequenceExpression(JObject @object, JSSequenceExpression existingValue, JsonSerializer serializer)
        {
            if (!@object.TryGetValue("expressions", out JToken expressionsToken) || !(expressionsToken is JArray expressionsArray))
                throw new InvalidOperationException("SequenceExpression must have expressions property of type array.");

            if (existingValue is null) existingValue = new JSSequenceExpression();

            ReadArray(expressionsArray, existingValue.Expressions, serializer);

            return existingValue;
        }

        protected virtual JSSwitchCase ReadSwitchCase(JObject @object, JSSwitchCase existingValue, JsonSerializer serializer)
        {
            if (!@object.TryGetValue("test", out JToken testToken) || !(testToken.Type == JTokenType.Object || testToken.Type == JTokenType.Null))
                throw new InvalidOperationException("SwitchCase must have test property of type object or null.");
            if (!@object.TryGetValue("consequent", out JToken consequentToken) || !(consequentToken is JArray consequentArray))
                throw new InvalidOperationException("SwitchCase must have consequent property of type array.");

            var test = ReadJson(testToken, existingValue?.Test, serializer);

            if (!(test is null) && !(test is JSExpression))
                throw new InvalidOperationException("SwitchCase must have test property of type expression or null.");

            if (existingValue is null) existingValue = new JSSwitchCase();

            existingValue.Test = (JSExpression)test;
            ReadArray(consequentArray, existingValue.Body, serializer);

            return existingValue;
        }

        protected virtual JSSwitchStatement ReadSwitchStatement(JObject @object, JSSwitchStatement existingValue, JsonSerializer serializer)
        {
            if (!@object.TryGetValue("discriminant", out JToken testToken) || !(testToken.Type == JTokenType.Object))
                throw new InvalidOperationException("SwitchStatement must have discriminant property of type object.");
            if (!@object.TryGetValue("cases", out JToken casesToken) || !(casesToken is JArray casesArray))
                throw new InvalidOperationException("SwitchStatement must have cases property of type array.");

            var discriminant = ReadJson(testToken, existingValue?.Discriminant, serializer);

            if (!(discriminant is JSExpression discriminantExpression))
                throw new InvalidOperationException("SwitchStatement must have discriminant property of type expression.");

            if (existingValue is null) existingValue = new JSSwitchStatement();

            existingValue.Discriminant = discriminantExpression;
            ReadArray(casesArray, existingValue.Cases, serializer);

            return existingValue;
        }

        protected virtual JSThisExpression ReadThisExpression(JObject @object, JSThisExpression existingValue, JsonSerializer serializer)
        {
            return existingValue ?? new JSThisExpression();
        }

        protected virtual JSThrowStatement ReadThrowStatement(JObject @object, JSThrowStatement existingValue, JsonSerializer serializer)
        {
            if (!@object.TryGetValue("argument", out JToken argumentToken) || !(argumentToken.Type == JTokenType.Object))
                throw new InvalidOperationException("ThrowStatement must have argument property of type object.");

            var argument = ReadJson(argumentToken, existingValue?.Expression, serializer);

            if (!(argument is JSExpression argumentExpression))
                throw new InvalidOperationException("ThrowStatement must have argument property of type expression.");

            if (existingValue is null) existingValue = new JSThrowStatement();

            existingValue.Expression = argumentExpression;

            return existingValue;
        }

        protected virtual JSTryStatement ReadTryStatement(JObject @object, JSTryStatement existingValue, JsonSerializer serializer)
        {
            if (!@object.TryGetValue("block", out JToken bodyToken) || !(bodyToken is JObject bodyObject) ||
                !bodyObject.TryGetValue("type", out JToken bodyTypeToken) || !(bodyTypeToken.Type == JTokenType.String && (string)bodyTypeToken == "BlockStatement") ||
                !bodyObject.TryGetValue("body", out bodyToken) || !(bodyToken is JArray bodyArray))
                throw new InvalidOperationException("TryStatement must have property block of block statement.");

            if (!@object.TryGetValue("handler", out JToken handlerToken) || !(handlerToken.Type == JTokenType.Object || handlerToken.Type == JTokenType.Null))
                throw new InvalidOperationException("TryStatement must have property handler of type object or null.");

            if (!@object.TryGetValue("finalizer", out JToken finalizerToken) || !(finalizerToken.Type == JTokenType.Object || finalizerToken.Type == JTokenType.Null))
                throw new InvalidOperationException("TryStatement must have property finalizer of type object or null.");
            else if (finalizerToken is JObject finalizerObject && (
                !finalizerObject.TryGetValue("type", out bodyTypeToken) || !(bodyTypeToken.Type == JTokenType.String && (string)bodyTypeToken == "BlockStatement") ||
                !finalizerObject.TryGetValue("body", out bodyToken) || !(bodyToken is JArray)))
                throw new InvalidOperationException("TryStatement must have property finalizer of type block statement or null.");

            if (handlerToken.Type == JTokenType.Null && finalizerToken.Type == JTokenType.Null)
                throw new InvalidOperationException("TryStatement must have either finalizer, handler or both.");

            var handler = ReadJson(handlerToken, existingValue?.Handler, serializer);

            if (!(handler is null) && !(handler is JSCatchClause))
                throw new InvalidOperationException("TryStatement must have handler property of type catch clause or null.");

            if (existingValue is null) existingValue = new JSTryStatement();

            existingValue.Handler = (JSCatchClause)handler;
            ReadArray(bodyArray, existingValue.Body, serializer);
            if (finalizerToken.Type != JTokenType.Null)
                ReadArray((JArray)finalizerToken["body"], existingValue.Finalizer.Body, serializer);

            return existingValue;
        }

        protected virtual JSUnaryExpression ReadUnaryExpression(JObject @object, JSUnaryExpression existingValue, JsonSerializer serializer)
        {
            if (!@object.TryGetValue("prefix", out JToken prefixToken) || !(prefixToken.Type == JTokenType.Boolean))
                throw new InvalidOperationException("UnaryExpression must have prefix property of type boolean.");
            if (!@object.TryGetValue("operator", out JToken operatorToken) || !(operatorToken.Type == JTokenType.String))
                throw new InvalidOperationException("UnaryExpression must have operator property of type string.");
            if (!@object.TryGetValue("argument", out JToken argumentToken) || !(argumentToken.Type == JTokenType.Object))
                throw new InvalidOperationException("UnaryExpression must have argument property of type object.");

            var argument = ReadJson(argumentToken, existingValue?.Expression, serializer);
            JSUnaryOperator @operator = ConvertUnaryOperator((bool)prefixToken, (string)operatorToken);

            if (!(argument is JSExpression argumentExpression))
                throw new InvalidOperationException("UnaryExpression must have argument property of type expression.");

            if (existingValue is null) existingValue = new JSUnaryExpression();

            existingValue.Expression = argumentExpression;
            existingValue.Operator = @operator;

            return existingValue;
        }

        protected virtual JSUnaryOperator ConvertUnaryOperator(bool prefix, string @operator)
        {
            switch (@operator)
            {
                case "-": return JSUnaryOperator.Negative;
                case "+": return JSUnaryOperator.Positive;
                case "!": return JSUnaryOperator.LogicalNot;
                case "~": return JSUnaryOperator.BitwiseNot;
                case "typeof": return JSUnaryOperator.TypeOf;
                case "void": return JSUnaryOperator.Void;
                case "delete": return JSUnaryOperator.Delete;
                case "++": return prefix ? JSUnaryOperator.PreIncrement : JSUnaryOperator.PostIncrement;
                case "--": return prefix ? JSUnaryOperator.PreDecrement : JSUnaryOperator.PostDecrement;
                default: throw new InvalidOperationException();
            }
        }

        protected virtual JSVariableDeclaration ReadVariableDeclaration(JObject @object, JSVariableDeclaration existingValue, JsonSerializer serializer)
        {
            if (!@object.TryGetValue("kind", out JToken kindToken) && !(kindToken.Type == JTokenType.String && (string)kindToken == "var"))
                throw new InvalidOperationException("VariableDeclaration must have property type with value 'var'.");
            if (!@object.TryGetValue("declarations", out JToken declToken) || !(declToken is JArray declArray))
                throw new InvalidOperationException("VariableDeclarator must have property declarations of type array.");

            if (existingValue is null) existingValue = new JSVariableDeclaration();

            existingValue.Kind = JSVariableDeclarationKind.Var;
            ReadArray(declArray, existingValue.Declarations, serializer);

            return existingValue;
        }

        protected virtual JSVariableDeclarator ReadVariableDeclarator(JObject @object, JSVariableDeclarator existingValue, JsonSerializer serializer)
        {
            if (!@object.TryGetValue("id", out JToken idToken) || !(idToken.Type == JTokenType.Object))
                throw new InvalidOperationException("VariableDeclarator must have property id of type object.");
            if (!@object.TryGetValue("init", out JToken initToken) || !(initToken.Type == JTokenType.Object || initToken.Type == JTokenType.Null))
                throw new InvalidOperationException("VariableDeclarator must have property init of type object or null.");

            var id = ReadJson(idToken, existingValue?.Identifier, serializer);
            var init = ReadJson(initToken, existingValue?.Initializer, serializer);

            if (!(id is IJSPattern idPattern))
                throw new InvalidOperationException("VariableDeclarator must have property id of type pattern.");
            if (!(init is null) && !(init is JSExpression))
                throw new InvalidOperationException("VariableDeclarator must have property init of type expression or null.");

            if (existingValue is null) existingValue = new JSVariableDeclarator();

            existingValue.Identifier = idPattern;
            existingValue.Initializer = (JSExpression)init;

            return existingValue;
        }

        protected virtual JSWhileStatement ReadWhileStatement(JObject @object, JSWhileStatement existingValue, JsonSerializer serializer)
        {
            if (!@object.TryGetValue("test", out JToken testToken) || !(testToken.Type == JTokenType.Object))
                throw new InvalidOperationException("WhileStatement must have property test of type object.");
            if (!@object.TryGetValue("body", out JToken bodyToken) || !(testToken.Type == JTokenType.Object))
                throw new InvalidOperationException("WhileStatement must have property body of type object.");

            var test = ReadJson(testToken, existingValue?.Test, serializer);
            var body = ReadJson(bodyToken, existingValue?.Body, serializer);

            if (!(test is JSExpression testExpression))
                throw new InvalidOperationException("WhileStatement must have property test of type expression.");
            if (!(body is null) && !(body is JSStatement))
                throw new InvalidOperationException("WhileStatement must have property body of type statement.");

            if (existingValue is null) existingValue = new JSWhileStatement();

            existingValue.Test = testExpression;
            existingValue.Body = (JSStatement)body;

            return existingValue;
        }

        protected virtual JSWithStatement ReadWithStatement(JObject @object, JSWithStatement existingValue, JsonSerializer serializer)
        {
            if (!@object.TryGetValue("object", out JToken objectToken) || !(objectToken.Type == JTokenType.Object))
                throw new InvalidOperationException("WithStatement must have property test of type object.");
            if (!@object.TryGetValue("body", out JToken bodyToken) || !(objectToken.Type == JTokenType.Object))
                throw new InvalidOperationException("WithStatement must have property body of type object.");

            var objectV = ReadJson(objectToken, existingValue?.Object, serializer);
            var body = ReadJson(bodyToken, existingValue?.Body, serializer);

            if (!(objectV is JSExpression objectExpression))
                throw new InvalidOperationException("WithStatement must have property test of type expression.");
            if (!(body is null) && !(body is JSStatement))
                throw new InvalidOperationException("WithStatement must have property body of type statement.");

            if (existingValue is null) existingValue = new JSWithStatement();

            existingValue.Object = objectExpression;
            existingValue.Body = (JSStatement)body;

            return existingValue;
        }
    }
}
