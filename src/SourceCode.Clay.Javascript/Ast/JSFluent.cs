using System.Collections.Generic;
using System.Linq;

namespace SourceCode.Clay.Javascript.Ast
{
    using Expression = JSExpression;
    using Statement = JSStatement;

    public static partial class JSFluent
    {
        #region Block Collection
        #endregion

        #region Array
        public static JSArrayExpression JSArray(IEnumerable<Expression> elements)
            => new JSArrayExpression() { elements };

        public static JSArrayExpression JSArray(params Expression[] elements)
            => JSArray((IEnumerable<Expression>)elements);
        #endregion

        #region Binary
        public static JSBinaryExpression JSBinary(this Expression left, JSBinaryOperator @operator, IEnumerable<Expression> right)
        {
            var result = left is JSBinaryExpression leftBinary && leftBinary.Operator == @operator
                ? leftBinary
                : new JSBinaryExpression(left, @operator);
            result.Add(right);
            return result;
        }

        public static JSBinaryExpression JSBinary(this Expression left, JSBinaryOperator @operator, params Expression[] right)
            => JSBinary(left, @operator, (IEnumerable<Expression>)right);

        public static JSBinaryExpression JSIdentityEquality(this Expression left, IEnumerable<Expression> right) => JSBinary(left, JSBinaryOperator.IdentityEquality, right);
        public static JSBinaryExpression JSIdentityEquality(this Expression left, params Expression[] right) => JSBinary(left, JSBinaryOperator.IdentityEquality, right);

        public static JSBinaryExpression JSIdentityInequality(this Expression left, IEnumerable<Expression> right) => JSBinary(left, JSBinaryOperator.IdentityInequality, right);
        public static JSBinaryExpression JSIdentityInequality(this Expression left, params Expression[] right) => JSBinary(left, JSBinaryOperator.IdentityInequality, right);

        public static JSBinaryExpression JSCoercedEquality(this Expression left, IEnumerable<Expression> right) => JSBinary(left, JSBinaryOperator.CoercedEquality, right);
        public static JSBinaryExpression JSCoercedEquality(this Expression left, params Expression[] right) => JSBinary(left, JSBinaryOperator.CoercedEquality, right);

        public static JSBinaryExpression JSCoercedInequality(this Expression left, IEnumerable<Expression> right) => JSBinary(left, JSBinaryOperator.CoercedInequality, right);
        public static JSBinaryExpression JSCoercedInequality(this Expression left, params Expression[] right) => JSBinary(left, JSBinaryOperator.CoercedInequality, right);

        public static JSBinaryExpression JSLessThan(this Expression left, IEnumerable<Expression> right) => JSBinary(left, JSBinaryOperator.LessThan, right);
        public static JSBinaryExpression JSLessThan(this Expression left, params Expression[] right) => JSBinary(left, JSBinaryOperator.LessThan, right);

        public static JSBinaryExpression JSLessThanOrEqual(this Expression left, IEnumerable<Expression> right) => JSBinary(left, JSBinaryOperator.LessThanOrEqual, right);
        public static JSBinaryExpression JSLessThanOrEqual(this Expression left, params Expression[] right) => JSBinary(left, JSBinaryOperator.LessThanOrEqual, right);

        public static JSBinaryExpression JSGreaterThan(this Expression left, IEnumerable<Expression> right) => JSBinary(left, JSBinaryOperator.GreaterThan, right);
        public static JSBinaryExpression JSGreaterThan(this Expression left, params Expression[] right) => JSBinary(left, JSBinaryOperator.GreaterThan, right);

        public static JSBinaryExpression JSGreaterThanOrEqual(this Expression left, IEnumerable<Expression> right) => JSBinary(left, JSBinaryOperator.GreaterThanOrEqual, right);
        public static JSBinaryExpression JSGreaterThanOrEqual(this Expression left, params Expression[] right) => JSBinary(left, JSBinaryOperator.GreaterThanOrEqual, right);

        public static JSBinaryExpression JSUnsignedLeftShift(this Expression left, IEnumerable<Expression> right) => JSBinary(left, JSBinaryOperator.UnsignedLeftShift, right);
        public static JSBinaryExpression JSUnsignedLeftShift(this Expression left, params Expression[] right) => JSBinary(left, JSBinaryOperator.UnsignedLeftShift, right);

        public static JSBinaryExpression JSSignedRightShift(this Expression left, IEnumerable<Expression> right) => JSBinary(left, JSBinaryOperator.SignedRightShift, right);
        public static JSBinaryExpression JSSignedRightShift(this Expression left, params Expression[] right) => JSBinary(left, JSBinaryOperator.SignedRightShift, right);

        public static JSBinaryExpression JSUnsignedRightShift(this Expression left, IEnumerable<Expression> right) => JSBinary(left, JSBinaryOperator.UnsignedRightShift, right);
        public static JSBinaryExpression JSUnsignedRightShift(this Expression left, params Expression[] right) => JSBinary(left, JSBinaryOperator.UnsignedRightShift, right);

        public static JSBinaryExpression JSAdd(this Expression left, IEnumerable<Expression> right) => JSBinary(left, JSBinaryOperator.Add, right);
        public static JSBinaryExpression JSAdd(this Expression left, params Expression[] right) => JSBinary(left, JSBinaryOperator.Add, right);

        public static JSBinaryExpression JSSubtract(this Expression left, IEnumerable<Expression> right) => JSBinary(left, JSBinaryOperator.Subtract, right);
        public static JSBinaryExpression JSSubtract(this Expression left, params Expression[] right) => JSBinary(left, JSBinaryOperator.Subtract, right);

        public static JSBinaryExpression JSMultiply(this Expression left, IEnumerable<Expression> right) => JSBinary(left, JSBinaryOperator.Multiply, right);
        public static JSBinaryExpression JSMultiply(this Expression left, params Expression[] right) => JSBinary(left, JSBinaryOperator.Multiply, right);

        public static JSBinaryExpression JSDivide(this Expression left, IEnumerable<Expression> right) => JSBinary(left, JSBinaryOperator.Divide, right);
        public static JSBinaryExpression JSDivide(this Expression left, params Expression[] right) => JSBinary(left, JSBinaryOperator.Divide, right);

        public static JSBinaryExpression JSModulus(this Expression left, IEnumerable<Expression> right) => JSBinary(left, JSBinaryOperator.Modulus, right);
        public static JSBinaryExpression JSModulus(this Expression left, params Expression[] right) => JSBinary(left, JSBinaryOperator.Modulus, right);

        public static JSBinaryExpression JSBitwiseOr(this Expression left, IEnumerable<Expression> right) => JSBinary(left, JSBinaryOperator.BitwiseOr, right);
        public static JSBinaryExpression JSBitwiseOr(this Expression left, params Expression[] right) => JSBinary(left, JSBinaryOperator.BitwiseOr, right);

        public static JSBinaryExpression JSBitwiseXor(this Expression left, IEnumerable<Expression> right) => JSBinary(left, JSBinaryOperator.BitwiseXor, right);
        public static JSBinaryExpression JSBitwiseXor(this Expression left, params Expression[] right) => JSBinary(left, JSBinaryOperator.BitwiseXor, right);

        public static JSBinaryExpression JSBitwiseAnd(this Expression left, IEnumerable<Expression> right) => JSBinary(left, JSBinaryOperator.BitwiseAnd, right);
        public static JSBinaryExpression JSBitwiseAnd(this Expression left, params Expression[] right) => JSBinary(left, JSBinaryOperator.BitwiseAnd, right);

        public static JSBinaryExpression JSIn(this Expression left, IEnumerable<Expression> right) => JSBinary(left, JSBinaryOperator.In, right);
        public static JSBinaryExpression JSIn(this Expression left, params Expression[] right) => JSBinary(left, JSBinaryOperator.In, right);

        public static JSBinaryExpression JSInstanceOf(this Expression left, IEnumerable<Expression> right) => JSBinary(left, JSBinaryOperator.InstanceOf, right);
        public static JSBinaryExpression JSInstanceOf(this Expression left, params Expression[] right) => JSBinary(left, JSBinaryOperator.InstanceOf, right);

        public static JSBinaryExpression JSAssign(this Expression left, IEnumerable<Expression> right) => JSBinary(left, JSBinaryOperator.Assign, right);
        public static JSBinaryExpression JSAssign(this Expression left, params Expression[] right) => JSBinary(left, JSBinaryOperator.Assign, right);

        public static JSBinaryExpression JSAddAssign(this Expression left, IEnumerable<Expression> right) => JSBinary(left, JSBinaryOperator.AddAssign, right);
        public static JSBinaryExpression JSAddAssign(this Expression left, params Expression[] right) => JSBinary(left, JSBinaryOperator.AddAssign, right);

        public static JSBinaryExpression JSSubtractAssign(this Expression left, IEnumerable<Expression> right) => JSBinary(left, JSBinaryOperator.SubtractAssign, right);
        public static JSBinaryExpression JSSubtractAssign(this Expression left, params Expression[] right) => JSBinary(left, JSBinaryOperator.SubtractAssign, right);

        public static JSBinaryExpression JSMultiplyAssign(this Expression left, IEnumerable<Expression> right) => JSBinary(left, JSBinaryOperator.MultiplyAssign, right);
        public static JSBinaryExpression JSMultiplyAssign(this Expression left, params Expression[] right) => JSBinary(left, JSBinaryOperator.MultiplyAssign, right);

        public static JSBinaryExpression JSDivideAssign(this Expression left, IEnumerable<Expression> right) => JSBinary(left, JSBinaryOperator.DivideAssign, right);
        public static JSBinaryExpression JSDivideAssign(this Expression left, params Expression[] right) => JSBinary(left, JSBinaryOperator.DivideAssign, right);

        public static JSBinaryExpression JSModulusAssign(this Expression left, IEnumerable<Expression> right) => JSBinary(left, JSBinaryOperator.ModulusAssign, right);
        public static JSBinaryExpression JSModulusAssign(this Expression left, params Expression[] right) => JSBinary(left, JSBinaryOperator.ModulusAssign, right);

        public static JSBinaryExpression JSUnsignedLeftShiftAssign(this Expression left, IEnumerable<Expression> right) => JSBinary(left, JSBinaryOperator.UnsignedLeftShiftAssign, right);
        public static JSBinaryExpression JSUnsignedLeftShiftAssign(this Expression left, params Expression[] right) => JSBinary(left, JSBinaryOperator.UnsignedLeftShiftAssign, right);

        public static JSBinaryExpression JSSignedRightShiftAssign(this Expression left, IEnumerable<Expression> right) => JSBinary(left, JSBinaryOperator.SignedRightShiftAssign, right);
        public static JSBinaryExpression JSSignedRightShiftAssign(this Expression left, params Expression[] right) => JSBinary(left, JSBinaryOperator.SignedRightShiftAssign, right);

        public static JSBinaryExpression JSUnsignedRightShiftAssign(this Expression left, IEnumerable<Expression> right) => JSBinary(left, JSBinaryOperator.UnsignedRightShiftAssign, right);
        public static JSBinaryExpression JSUnsignedRightShiftAssign(this Expression left, params Expression[] right) => JSBinary(left, JSBinaryOperator.UnsignedRightShiftAssign, right);

        public static JSBinaryExpression JSBitwiseOrAssign(this Expression left, IEnumerable<Expression> right) => JSBinary(left, JSBinaryOperator.BitwiseOrAssign, right);
        public static JSBinaryExpression JSBitwiseOrAssign(this Expression left, params Expression[] right) => JSBinary(left, JSBinaryOperator.BitwiseOrAssign, right);

        public static JSBinaryExpression JSBitwiseXorAssign(this Expression left, IEnumerable<Expression> right) => JSBinary(left, JSBinaryOperator.BitwiseXorAssign, right);
        public static JSBinaryExpression JSBitwiseXorAssign(this Expression left, params Expression[] right) => JSBinary(left, JSBinaryOperator.BitwiseXorAssign, right);

        public static JSBinaryExpression JSBitwiseAndAssign(this Expression left, IEnumerable<Expression> right) => JSBinary(left, JSBinaryOperator.BitwiseAndAssign, right);
        public static JSBinaryExpression JSBitwiseAndAssign(this Expression left, params Expression[] right) => JSBinary(left, JSBinaryOperator.BitwiseAndAssign, right);

        public static JSBinaryExpression JSLogicalAnd(this Expression left, IEnumerable<Expression> right) => JSBinary(left, JSBinaryOperator.LogicalAnd, right);
        public static JSBinaryExpression JSLogicalAnd(this Expression left, params Expression[] right) => JSBinary(left, JSBinaryOperator.LogicalAnd, right);

        public static JSBinaryExpression JSLogicalOr(this Expression left, IEnumerable<Expression> right) => JSBinary(left, JSBinaryOperator.LogicalOr, right);
        public static JSBinaryExpression JSLogicalOr(this Expression left, params Expression[] right) => JSBinary(left, JSBinaryOperator.LogicalOr, right);
        #endregion

        #region Block
        public static JSBlockStatement JSBlock(IEnumerable<Statement> elements)
            => new JSBlockStatement() { elements };

        public static JSBlockStatement JSBlock(params Statement[] elements)
            => JSBlock((IEnumerable<Statement>)elements);
        #endregion

        #region Break
        public static JSBreakStatement JSBreak(JSIdentifier identifier) => new JSBreakStatement(identifier);
        public static JSBreakStatement JSBreak() => JSBreak((JSIdentifier)null);
        public static JSBreakStatement JSBreak(string identifier) => JSBreak(identifier is null ? null : new JSIdentifier(identifier));
        #endregion

        #region Call
        public static JSCallExpression JSCall(this Expression callee, IEnumerable<Expression> arguments)
            => new JSCallExpression(callee) { arguments };

        public static JSCallExpression JSCall(this Expression callee, params Expression[] elements)
            => JSCall(callee, (IEnumerable<Expression>)elements);
        #endregion

        #region Catch
        public static JSCatchClause JSCatch(IJSPattern parameter, IEnumerable<Statement> statements)
            => new JSCatchClause(parameter) { statements };

        public static JSCatchClause JSCatch(IJSPattern parameter, params Statement[] statements)
            => JSCatch(parameter, (IEnumerable<Statement>)statements);

        public static JSCatchClause JSCatch(string parameter, IEnumerable<Statement> statements)
            => JSCatch(JSIdentifier(parameter));

        public static JSCatchClause JSCatch(string parameter, params Statement[] statements)
            => JSCatch(JSIdentifier(parameter), statements);
        #endregion

        #region Conditional
        public static JSConditionalExpression JSConditional(this Expression test, Expression consequent, Expression alternate)
            => new JSConditionalExpression(test, consequent, alternate);
        #endregion

        #region Continue
        public static JSContinueStatement JSContinue(JSIdentifier identifier) => new JSContinueStatement(identifier);
        public static JSContinueStatement JSContinue() => JSContinue((JSIdentifier)null);
        public static JSContinueStatement JSContinue(string identifier) => JSContinue(identifier is null ? null : JSIdentifier(identifier));
        #endregion

        #region Debugger
        public static JSDebuggerStatement JSDebugger() => new JSDebuggerStatement();
        #endregion

        #region Directive
        public static JSExpressionStatement JSDirective(string directive) => new JSExpressionStatement(JSLiteral(directive));
        #endregion

        #region DoWhile
        public static JSDoWhileStatement JSDoWhile(Expression test, IEnumerable<Statement> statement)
            => new JSDoWhileStatement(test) { statement };

        public static JSDoWhileStatement JSDoWhile(Expression test, params Statement[] statement)
            => JSDoWhile(test, (IEnumerable<Statement>)statement);
        #endregion

        #region ForIn
        public static JSForInStatement JSForIn(Discriminated<JSVariableDeclaration, IJSPattern> left, Expression right, IEnumerable<Statement> statements)
            => new JSForInStatement(left, right) { statements };

        public static JSForInStatement JSForIn(Discriminated<JSVariableDeclaration, IJSPattern> left, Expression right, params Statement[] statements)
            => JSForIn(left, right, (IEnumerable<Statement>)statements);
        #endregion

        #region For
        public static JSForStatement JSFor(Discriminated<JSVariableDeclaration, Expression> initializer, Expression test, Expression update, IEnumerable<Statement> statements)
            => new JSForStatement(initializer, test, update) { statements };

        public static JSForStatement JSFor(Discriminated<JSVariableDeclaration, Expression> initializer, Expression test, Expression update, params Statement[] statements)
            => JSFor(initializer, test, update, (IEnumerable<Statement>)statements);
        #endregion

        #region FunctionDeclaration
        public static JSFunctionDeclaration JSFunctionDeclaration(JSIdentifier identifier)
            => new JSFunctionDeclaration(identifier);

        public static JSFunctionDeclaration JSFunctionDeclaration(JSIdentifier identifier, IEnumerable<IJSPattern> parameters)
            => new JSFunctionDeclaration(identifier, parameters);

        public static JSFunctionDeclaration JSFunctionDeclaration(JSIdentifier identifier, params IJSPattern[] parameters)
            => JSFunctionDeclaration(identifier, (IEnumerable<IJSPattern>)parameters);

        public static JSFunctionDeclaration JSFunctionDeclaration(JSIdentifier identifier, params string[] parameters)
            => JSFunctionDeclaration(identifier, parameters.Select(x => new JSIdentifier(x)));

        public static JSFunctionDeclaration JSFunctionDeclaration(string identifier)
            => JSFunctionDeclaration(JSIdentifier(identifier));

        public static JSFunctionDeclaration JSFunctionDeclaration(string identifier, IEnumerable<IJSPattern> parameters)
            => JSFunctionDeclaration(JSIdentifier(identifier), parameters);

        public static JSFunctionDeclaration JSFunctionDeclaration(string identifier, params IJSPattern[] parameters)
            => JSFunctionDeclaration(JSIdentifier(identifier), parameters);

        public static JSFunctionDeclaration JSFunctionDeclaration(string identifier, params string[] parameters)
            => JSFunctionDeclaration(JSIdentifier(identifier), parameters);
        #endregion

        #region FunctionExpression
        public static JSFunctionExpression JSFunction()
            => new JSFunctionExpression();

        public static JSFunctionExpression JSFunction(IEnumerable<IJSPattern> parameters)
            => new JSFunctionExpression(null, parameters);

        public static JSFunctionExpression JSFunction(params IJSPattern[] parameters)
            => JSFunction((IEnumerable<IJSPattern>)parameters);

        public static JSFunctionExpression JSFunction(params string[] parameters)
            => JSFunction(parameters.Select(x => new JSIdentifier(x)));
        #endregion

        #region Identifier
        public static JSIdentifier JSIdentifier(string identifier) => new JSIdentifier(identifier);
        #endregion

        #region If
        public static JSIfStatement JSIf(Expression condition, IEnumerable<Statement> consequent)
            => new JSIfStatement(condition) { consequent };

        public static JSIfStatement JSIf(Expression condition, params Statement[] consequent)
            => JSIf(condition, (IEnumerable<Statement>)consequent);
        #endregion

        #region Labeled
        public static JSLabeledStatement JSLabel(JSIdentifier label, IEnumerable<Statement> statements)
            => new JSLabeledStatement(label) { statements };

        public static JSLabeledStatement JSLabel(JSIdentifier label, params Statement[] statements)
            => JSLabel(label, (IEnumerable<Statement>)statements);

        public static JSLabeledStatement JSLabel(string label, IEnumerable<Statement> statements)
            => JSLabel(JSIdentifier(label));

        public static JSLabeledStatement JSLabel(string label, params Statement[] statements)
            => JSLabel(JSIdentifier(label), statements);
        #endregion

        #region Literal
        public static JSLiteral JSNull() => new JSLiteral();
        public static JSLiteral JSLiteral(object value) => new JSLiteral(value);
        public static JSLiteral JSRegex(string regex) => new JSLiteral(new JSRegex(regex));
        public static JSLiteral JSRegex(string regex, JSRegexOptions options) => new JSLiteral(new JSRegex(regex, options));
        #endregion

        #region Member
        public static JSMemberExpression JSMember(this Expression @object, IEnumerable<JSIdentifier> properties)
            => new JSMemberExpression(@object) { properties };

        public static JSMemberExpression JSMember(this Expression @object, params JSIdentifier[] properties)
            => JSMember(@object, (IEnumerable<JSIdentifier>)properties);

        public static JSMemberExpression JSMember(this Expression @object, IEnumerable<string> properties)
            => JSMember(@object, properties.Select(x => JSIdentifier(x)));

        public static JSMemberExpression JSMember(this Expression @object, params string[] properties)
            => JSMember(@object, (IEnumerable<string>)properties);

        public static JSMemberExpression JSIndexer(this Expression @object, IEnumerable<Expression> indices)
            => new JSMemberExpression(@object) { IsComputed = true }.Add(indices);

        public static JSMemberExpression JSIndexer(this Expression @object, params Expression[] indices)
            => JSIndexer(@object, (IEnumerable<Expression>)indices);
        #endregion

        #region New
        public static JSNewExpression JSNew(this Expression callee, IEnumerable<Expression> arguments)
            => new JSNewExpression(callee) { arguments };

        public static JSNewExpression JSNew(this Expression callee, params Expression[] elements)
            => JSNew(callee, (IEnumerable<Expression>)elements);
        #endregion

        #region Object
        public static JSObjectExpression JSObject(IEnumerable<JSProperty> properties)
            => new JSObjectExpression() { properties };

        public static JSObjectExpression JSObject(params JSProperty[] properties)
            => JSObject((IEnumerable<JSProperty>)properties);
        #endregion

        #region Program
        public static JSProgram JSProgram(IEnumerable<Statement> statements)
            => new JSProgram() { statements };

        public static JSProgram JSProgram(params Statement[] statements)
            => JSProgram((IEnumerable<Statement>)statements);
        #endregion

        #region Property
        public static JSProperty JSProperty(JSPropertyKind kind, Discriminated<JSLiteral, JSIdentifier> key, Expression value)
            => new JSProperty(kind, key, value);

        public static JSProperty JSProperty(Discriminated<JSLiteral, JSIdentifier> key, Expression value)
            => new JSProperty(key, value);

        public static JSProperty JSProperty(this JSLiteral key, JSPropertyKind kind, Expression value)
            => JSProperty(kind, key, value);

        public static JSProperty JSProperty(this JSIdentifier key, JSPropertyKind kind, Expression value)
            => JSProperty(kind, key, value);

        public static JSProperty JSProperty(this JSLiteral key, Expression value)
            => JSProperty(new Discriminated<JSLiteral, JSIdentifier>(key), value);

        public static JSProperty JSProperty(this JSIdentifier key, Expression value)
            => JSProperty(new Discriminated<JSLiteral, JSIdentifier>(key), value);
        #endregion

        #region Return
        public static JSReturnStatement JSReturn() => new JSReturnStatement();

        public static JSReturnStatement JSReturn(Expression argument)
            => new JSReturnStatement(argument);
        #endregion

        #region Sequence
        public static JSSequenceExpression JSSequence(IEnumerable<Expression> expressions)
            => new JSSequenceExpression() { expressions };

        public static JSSequenceExpression JSSequence(params Expression[] expressions)
            => JSSequence((IEnumerable<Expression>)expressions);
        #endregion

        #region SwitchCase
        public static JSSwitchCase JSDefaultCase(IEnumerable<Statement> statements)
            => new JSSwitchCase() { statements };

        public static JSSwitchCase JSDefaultCase(params Statement[] statements)
            => JSDefaultCase((IEnumerable<Statement>)statements);

        public static JSSwitchCase JSCase(Expression test, IEnumerable<Statement> statements)
            => new JSSwitchCase(test) { statements };

        public static JSSwitchCase JSCase(Expression test, params Statement[] statements)
            => JSCase(test, (IEnumerable<Statement>)statements);
        #endregion

        #region Switch
        public static JSSwitchStatement JSSwitch(Expression discriminant, IEnumerable<JSSwitchCase> cases)
            => new JSSwitchStatement(discriminant) { cases };

        public static JSSwitchStatement JSSwitch(Expression discriminant, params JSSwitchCase[] cases)
            => JSSwitch(discriminant, (IEnumerable<JSSwitchCase>)cases);
        #endregion

        #region This
        public static JSThisExpression JSThis() => new JSThisExpression();
        #endregion

        #region Throw
        public static JSThrowStatement JSThrow(Expression argument) => new JSThrowStatement(argument);
        #endregion

        #region Try
        public static JSTryStatement JSTry(IEnumerable<Statement> statements)
            => new JSTryStatement() { statements };

        public static JSTryStatement JSTry(params Statement[] statements)
            => JSTry((IEnumerable<Statement>)statements);
        #endregion

        #region Unary
        public static JSUnaryExpression JSUnary(this Expression argument, JSUnaryOperator @operator)
             => new JSUnaryExpression(@operator, argument);

        public static JSUnaryExpression JSNegative(this Expression argument) => JSUnary(argument, JSUnaryOperator.Negative);
        public static JSUnaryExpression JSPositive(this Expression argument) => JSUnary(argument, JSUnaryOperator.Positive);
        public static JSUnaryExpression JSLogicalNot(this Expression argument) => JSUnary(argument, JSUnaryOperator.LogicalNot);
        public static JSUnaryExpression JSBitwiseNot(this Expression argument) => JSUnary(argument, JSUnaryOperator.BitwiseNot);
        public static JSUnaryExpression JSTypeOf(this Expression argument) => JSUnary(argument, JSUnaryOperator.TypeOf);
        public static JSUnaryExpression JSVoid(this Expression argument) => JSUnary(argument, JSUnaryOperator.Void);
        public static JSUnaryExpression JSDelete(this Expression argument) => JSUnary(argument, JSUnaryOperator.Delete);
        public static JSUnaryExpression JSPreIncrement(this Expression argument) => JSUnary(argument, JSUnaryOperator.PreIncrement);
        public static JSUnaryExpression JSPreDecrement(this Expression argument) => JSUnary(argument, JSUnaryOperator.PreDecrement);
        public static JSUnaryExpression JSPostIncrement(this Expression argument) => JSUnary(argument, JSUnaryOperator.PostIncrement);
        public static JSUnaryExpression JSPostDecrement(this Expression argument) => JSUnary(argument, JSUnaryOperator.PostDecrement);
        #endregion

        #region Variable
        public static JSVariableDeclaration JSVar(JSIdentifier identifier)
            => new JSVariableDeclaration() { new JSVariableDeclarator(identifier) };
        public static JSVariableDeclaration JSVar(JSIdentifier identifier, Expression initializer)
            => new JSVariableDeclaration() { new JSVariableDeclarator(identifier, initializer) };
        public static JSVariableDeclaration JSVar(string identifier)
            => JSVar(JSIdentifier(identifier));
        public static JSVariableDeclaration JSVar(string identifier, Expression initializer)
            => JSVar(JSIdentifier(identifier), initializer);

        public static JSVariableDeclaration JSVarList(IEnumerable<JSVariableDeclarator> declarators)
            => new JSVariableDeclaration() { declarators };

        public static JSVariableDeclaration JSVarList(params JSVariableDeclarator[] declarators)
            => JSVarList((IEnumerable<JSVariableDeclarator>)declarators);
        #endregion

        #region While
        public static JSWhileStatement JSWhile(Expression test, IEnumerable<Statement> statement)
            => new JSWhileStatement(test) { statement };

        public static JSWhileStatement JSWhile(Expression test, params Statement[] statement)
            => JSWhile(test, (IEnumerable<Statement>)statement);
        #endregion

        #region With
        public static JSWithStatement JSWith(Expression @object, IEnumerable<Statement> statements)
            => new JSWithStatement(@object) { statements };

        public static JSWithStatement JSWith(Expression @object, params Statement[] statements)
            => JSWith(@object, (IEnumerable<Statement>)statements);
        #endregion
    }
}
