using System;
using System.Globalization;
using System.Threading.Tasks;

namespace SourceCode.Clay.Javascript.Ast
{
    partial class JSWriter
    {
        public ValueTask WriteAsync(object value)
        {
            if (value is IJSNode node) return WriteNodeAsync(node);
            else return WriteLiteralAsync(value);
        }

        public virtual ValueTask WriteNodeAsync<T>(T node)
            where T : IJSNode
        {
            if (node == null)
            {
                if (typeof(T) == typeof(JSStatement))
                {
                    if (Minify) return WriteAsync(";");
                    else return new ValueTask(WriteLineAsync(";"));
                }
                else
                {
                    return WriteLiteralAsync();
                }
            }
            switch (node.Type)
            {
                case JSNodeType.Identifier:
                    return WriteNodeAsync((JSIdentifier)(IJSNode)node);
                case JSNodeType.Literal:
                    return WriteNodeAsync((JSLiteral)(IJSNode)node);
                case JSNodeType.Program:
                    return WriteNodeAsync((JSProgram)(IJSNode)node);
                case JSNodeType.ExpressionStatement:
                    return WriteNodeAsync((JSExpressionStatement)(IJSNode)node);
                case JSNodeType.BlockStatement:
                    return WriteBlockAsync((JSBlockStatement)(IJSNode)node, false, true);
                case JSNodeType.DebuggerStatement:
                    return WriteNodeAsync((JSDebuggerStatement)(IJSNode)node);
                case JSNodeType.WithStatement:
                    return WriteNodeAsync((JSWithStatement)(IJSNode)node);
                case JSNodeType.ReturnStatement:
                    return WriteNodeAsync((JSReturnStatement)(IJSNode)node);
                case JSNodeType.LabeledStatement:
                    return WriteNodeAsync((JSLabeledStatement)(IJSNode)node);
                case JSNodeType.BreakStatement:
                    return WriteNodeAsync((JSBreakStatement)(IJSNode)node);
                case JSNodeType.ContinueStatement:
                    return WriteNodeAsync((JSContinueStatement)(IJSNode)node);
                case JSNodeType.IfStatement:
                    return WriteNodeAsync((JSIfStatement)(IJSNode)node);
                case JSNodeType.SwitchStatement:
                    return WriteNodeAsync((JSSwitchStatement)(IJSNode)node);
                case JSNodeType.SwitchCase:
                    return WriteNodeAsync((JSSwitchCase)(IJSNode)node);
                case JSNodeType.ThrowStatement:
                    return WriteNodeAsync((JSThrowStatement)(IJSNode)node);
                case JSNodeType.TryStatement:
                    return WriteNodeAsync((JSTryStatement)(IJSNode)node);
                case JSNodeType.CatchClause:
                    return WriteNodeAsync((JSCatchClause)(IJSNode)node);
                case JSNodeType.WhileStatement:
                    return WriteNodeAsync((JSWhileStatement)(IJSNode)node);
                case JSNodeType.DoWhileStatement:
                    return WriteNodeAsync((JSDoWhileStatement)(IJSNode)node);
                case JSNodeType.ForStatement:
                    return WriteNodeAsync((JSForStatement)(IJSNode)node);
                case JSNodeType.ForInStatement:
                    return WriteNodeAsync((JSForInStatement)(IJSNode)node);
                case JSNodeType.FunctionDeclaration:
                    return WriteNodeAsync((JSFunctionDeclaration)(IJSNode)node);
                case JSNodeType.VariableDeclaration:
                    return WriteNodeAsync((JSVariableDeclaration)(IJSNode)node);
                case JSNodeType.VariableDeclarator:
                    return WriteNodeAsync((JSVariableDeclarator)(IJSNode)node);
                case JSNodeType.ThisExpression:
                    return WriteNodeAsync((JSThisExpression)(IJSNode)node);
                case JSNodeType.ArrayExpression:
                    return WriteNodeAsync((JSArrayExpression)(IJSNode)node);
                case JSNodeType.ObjectExpression:
                    return WriteNodeAsync((JSObjectExpression)(IJSNode)node);
                case JSNodeType.Property:
                    return WriteNodeAsync((JSProperty)(IJSNode)node);
                case JSNodeType.FunctionExpression:
                    return WriteNodeAsync((JSFunctionExpression)(IJSNode)node);
                case JSNodeType.UnaryExpression:
                    return WriteNodeAsync((JSUnaryExpression)(IJSNode)node);
                case JSNodeType.BinaryExpression:
                    return WriteNodeAsync((JSBinaryExpression)(IJSNode)node);
                case JSNodeType.MemberExpression:
                    return WriteNodeAsync((JSMemberExpression)(IJSNode)node);
                case JSNodeType.ConditionalExpression:
                    return WriteNodeAsync((JSConditionalExpression)(IJSNode)node);
                case JSNodeType.CallExpression:
                    return WriteNodeAsync((JSCallExpression)(IJSNode)node);
                case JSNodeType.NewExpression:
                    return WriteNodeAsync((JSNewExpression)(IJSNode)node);
                case JSNodeType.SequenceExpression:
                    return WriteNodeAsync((JSSequenceExpression)(IJSNode)node);
                default: throw new NotSupportedException($"The node type {node.Type} is not supported.");
            }
        }

        protected virtual async ValueTask WriteNodeAsync(JSSequenceExpression node)
        {
            if (node == null) throw new ArgumentNullException(nameof(node));
            for (var i = 0; i < node.Expressions.Count; i++)
            {
                if (i != 0) await WriteAsync(Minify ? "," : ", ").ConfigureAwait(false);
                await WriteNodeAsync(node.Expressions[i]).ConfigureAwait(false);
            }
        }

        protected virtual async ValueTask WriteNodeAsync(JSNewExpression node)
        {
            if (node == null) throw new ArgumentNullException(nameof(node));
            await WriteAsync("new ").ConfigureAwait(false);
            await WriteNodeAsync((JSCallExpression)node).ConfigureAwait(false);
        }

        protected virtual async ValueTask WriteNodeAsync(JSCallExpression node)
        {
            if (node == null) throw new ArgumentNullException(nameof(node));
            if (node.Callee != null)
            {
                var isFunction = node.Callee is IJSFunction;
                if (isFunction) await WriteAsync("(").ConfigureAwait(false);
                await WriteNodeAsync(node.Callee).ConfigureAwait(false);
                if (isFunction) await WriteAsync(")").ConfigureAwait(false);
            }
            await WriteAsync("(").ConfigureAwait(false);
            for (var i = 0; i < node.Arguments.Count; i++)
            {
                if (i != 0) await WriteAsync(Minify ? "," : ", ").ConfigureAwait(false);
                await WriteNodeAsync(node.Arguments[i]).ConfigureAwait(false);
            }
            await WriteAsync(")").ConfigureAwait(false);
        }

        protected virtual async ValueTask WriteNodeAsync(JSConditionalExpression node)
        {
            if (node == null) throw new ArgumentNullException(nameof(node));
            await WriteNodeAsync(node.Test).ConfigureAwait(false);
            await WriteAsync(Minify ? "?" : " ? ").ConfigureAwait(false);
            await WriteNodeAsync(node.Consequent).ConfigureAwait(false);
            await WriteAsync(Minify ? ":" : " : ").ConfigureAwait(false);
            await WriteNodeAsync(node.Alternate).ConfigureAwait(false);
        }

        protected virtual async ValueTask WriteNodeAsync(JSMemberExpression node)
        {
            if (node == null) throw new ArgumentNullException(nameof(node));
            await WriteNodeAsync(node.Object).ConfigureAwait(false);
            for (var i = 0; i < node.Indices.Count; i++)
            {
                await WriteAsync(node.IsComputed ? "[" : ".").ConfigureAwait(false);
                await WriteNodeAsync(node.Indices[i]).ConfigureAwait(false);
                if (node.IsComputed) await WriteAsync("]").ConfigureAwait(false);
            }
        }

        protected virtual async ValueTask WriteNodeAsync(JSBinaryExpression node)
        {
            if (node == null) throw new ArgumentNullException(nameof(node));
            await WriteAsync("(").ConfigureAwait(false);
            await WriteNodeAsync(node.Left).ConfigureAwait(false);
            for (var i = 0; i < node.Right.Count; i++)
            {
                await WriteNodeAsync(node.Operator).ConfigureAwait(false);
                await WriteNodeAsync(node.Right[i]).ConfigureAwait(false);
            }
            await WriteAsync(")").ConfigureAwait(false);
        }

        protected virtual ValueTask WriteNodeAsync(JSBinaryOperator @operator)
        {
            switch (@operator)
            {
                case JSBinaryOperator.IdentityEquality:
                    return WriteAsync(Minify ? "===" : " === ");
                case JSBinaryOperator.IdentityInequality:
                    return WriteAsync(Minify ? "!==" : " !== ");
                case JSBinaryOperator.CoercedEquality:
                    return WriteAsync(Minify ? "==" : " == ");
                case JSBinaryOperator.CoercedInequality:
                    return WriteAsync(Minify ? "!=" : " != ");
                case JSBinaryOperator.LessThan:
                    return WriteAsync(Minify ? "<" : " < ");
                case JSBinaryOperator.LessThanOrEqual:
                    return WriteAsync(Minify ? "<=" : " <= ");
                case JSBinaryOperator.GreaterThan:
                    return WriteAsync(Minify ? ">" : " > ");
                case JSBinaryOperator.GreaterThanOrEqual:
                    return WriteAsync(Minify ? ">=" : " >= ");
                case JSBinaryOperator.UnsignedLeftShift:
                    return WriteAsync(Minify ? "<<" : " << ");
                case JSBinaryOperator.SignedRightShift:
                    return WriteAsync(Minify ? ">>" : " >> ");
                case JSBinaryOperator.UnsignedRightShift:
                    return WriteAsync(Minify ? ">>>" : " >>> ");
                case JSBinaryOperator.Add:
                    return WriteAsync(Minify ? "+" : " + ");
                case JSBinaryOperator.Subtract:
                    return WriteAsync(Minify ? "-" : " - ");
                case JSBinaryOperator.Multiply:
                    return WriteAsync(Minify ? "*" : " * ");
                case JSBinaryOperator.Divide:
                    return WriteAsync(Minify ? "/" : " / ");
                case JSBinaryOperator.Modulus:
                    return WriteAsync(Minify ? "%" : " % ");
                case JSBinaryOperator.BitwiseOr:
                    return WriteAsync(Minify ? "|" : " | ");
                case JSBinaryOperator.BitwiseXor:
                    return WriteAsync(Minify ? "^" : " ^ ");
                case JSBinaryOperator.BitwiseAnd:
                    return WriteAsync(Minify ? "&" : " & ");
                case JSBinaryOperator.In:
                    return WriteAsync(" in ");
                case JSBinaryOperator.InstanceOf:
                    return WriteAsync(" instanceof ");
                case JSBinaryOperator.Assign:
                    return WriteAsync(Minify ? "=" : " = ");
                case JSBinaryOperator.AddAssign:
                    return WriteAsync(Minify ? "+=" : " += ");
                case JSBinaryOperator.SubtractAssign:
                    return WriteAsync(Minify ? "-=" : " -= ");
                case JSBinaryOperator.MultiplyAssign:
                    return WriteAsync(Minify ? "*=" : " *= ");
                case JSBinaryOperator.DivideAssign:
                    return WriteAsync(Minify ? "/=" : " /= ");
                case JSBinaryOperator.ModulusAssign:
                    return WriteAsync(Minify ? "%=" : " %= ");
                case JSBinaryOperator.UnsignedLeftShiftAssign:
                    return WriteAsync(Minify ? "<<=" : " <<= ");
                case JSBinaryOperator.SignedRightShiftAssign:
                    return WriteAsync(Minify ? ">>=" : " >>= ");
                case JSBinaryOperator.UnsignedRightShiftAssign:
                    return WriteAsync(Minify ? ">>>=" : " >>>= ");
                case JSBinaryOperator.BitwiseOrAssign:
                    return WriteAsync(Minify ? "|=" : " |= ");
                case JSBinaryOperator.BitwiseXorAssign:
                    return WriteAsync(Minify ? "^=" : " ^= ");
                case JSBinaryOperator.BitwiseAndAssign:
                    return WriteAsync(Minify ? "&=" : " &= ");
                case JSBinaryOperator.LogicalAnd:
                    return WriteAsync(Minify ? "&&" : " && ");
                case JSBinaryOperator.LogicalOr:
                    return WriteAsync(Minify ? "||" : " || ");
                default: throw new NotSupportedException($"The binary operator {@operator} is not supported.");
            }
        }

        protected virtual async ValueTask WriteNodeAsync(JSUnaryExpression node)
        {
            if (node == null) throw new ArgumentNullException(nameof(node));

            switch (node.Operator)
            {
                case JSUnaryOperator.Negative:
                    await WriteAsync("-").ConfigureAwait(false);
                    await WriteNodeAsync(node.Expression).ConfigureAwait(false);
                    break;
                case JSUnaryOperator.Positive:
                    await WriteAsync("+").ConfigureAwait(false);
                    await WriteNodeAsync(node.Expression).ConfigureAwait(false);
                    break;
                case JSUnaryOperator.LogicalNot:
                    await WriteAsync("!").ConfigureAwait(false);
                    await WriteNodeAsync(node.Expression).ConfigureAwait(false);
                    break;
                case JSUnaryOperator.BitwiseNot:
                    await WriteAsync("~").ConfigureAwait(false);
                    await WriteNodeAsync(node.Expression).ConfigureAwait(false);
                    break;
                case JSUnaryOperator.TypeOf:
                    await WriteAsync("typeof ");
                    await WriteNodeAsync(node.Expression).ConfigureAwait(false);
                    break;
                case JSUnaryOperator.Void:
                    await WriteAsync("void ").ConfigureAwait(false);
                    await WriteNodeAsync(node.Expression).ConfigureAwait(false);
                    break;
                case JSUnaryOperator.Delete:
                    await WriteAsync("delete ").ConfigureAwait(false);
                    await WriteNodeAsync(node.Expression).ConfigureAwait(false);
                    break;
                case JSUnaryOperator.PreIncrement:
                    await WriteAsync("++").ConfigureAwait(false);
                    await WriteNodeAsync(node.Expression).ConfigureAwait(false);
                    break;
                case JSUnaryOperator.PreDecrement:
                    await WriteAsync("--").ConfigureAwait(false);
                    await WriteNodeAsync(node.Expression).ConfigureAwait(false);
                    break;
                case JSUnaryOperator.PostIncrement:
                    await WriteNodeAsync(node.Expression).ConfigureAwait(false);
                    await WriteAsync("++").ConfigureAwait(false);
                    break;
                case JSUnaryOperator.PostDecrement:
                    await WriteNodeAsync(node.Expression).ConfigureAwait(false);
                    await WriteAsync("--").ConfigureAwait(false);
                    break;
                default: throw new ArgumentOutOfRangeException($"The unary operator {node.Operator} is not supported.");
            }
        }

        protected virtual async ValueTask WriteNodeAsync(JSFunctionExpression node)
        {
            if (node == null) throw new ArgumentNullException(nameof(node));
            await WriteAsync("function(").ConfigureAwait(false);
            for (var i = 0; i < node.Parameters.Count; i++)
            {
                if (i != 0) await WriteAsync(Minify ? "," : ", ").ConfigureAwait(false);
                await WriteNodeAsync(node.Parameters[i]).ConfigureAwait(false);
            }
            await WriteAsync(Minify ? ")" : ") ").ConfigureAwait(false);
            await WriteBlockAsync(node, false, false).ConfigureAwait(false);
        }

        protected virtual async ValueTask WriteNodeAsync(JSProperty node)
        {
            if (node == null) throw new ArgumentNullException(nameof(node));

            switch (node.Key)
            {
                case var d when d.IsItem1: await WriteNodeAsync(d.Item1).ConfigureAwait(false); break;
                case var d when d.IsItem2: await WriteNodeAsync(d.Item2).ConfigureAwait(false); break;
                default: await WriteLiteralAsync().ConfigureAwait(false); break;
            }

            await WriteAsync(Minify ? ":" : ": ").ConfigureAwait(false);
            await WriteNodeAsync(node.Value).ConfigureAwait(false);
        }

        protected virtual async ValueTask WriteNodeAsync(JSObjectExpression node)
        {
            if (node == null) throw new ArgumentNullException(nameof(node));
            if (node.Properties.Count == 0)
            {
                await WriteAsync("{}").ConfigureAwait(false);
                return;
            }

            if (Minify) await WriteAsync("{").ConfigureAwait(false);
            else
            {
                await WriteLineAsync("{").ConfigureAwait(false);
                Indent++;
            }

            for (var i = 0; i < node.Properties.Count; i++)
            {
                if (i != 0)
                {
                    if (Minify) await WriteAsync(",").ConfigureAwait(false);
                    else await WriteLineAsync(",").ConfigureAwait(false);
                }
                await WriteNodeAsync(node.Properties[i]);
            }

            if (Minify) await WriteAsync("}").ConfigureAwait(false);
            else
            {
                await WriteLineAsync().ConfigureAwait(false);
                Indent--;
                await WriteAsync("}").ConfigureAwait(false);
            }
        }

        protected virtual async ValueTask WriteNodeAsync(JSArrayExpression node)
        {
            if (node == null) throw new ArgumentNullException(nameof(node));
            if (node.Elements.Count == 0)
            {
                await WriteAsync("[]").ConfigureAwait(false);
                return;
            }

            if (Minify) await WriteAsync("[").ConfigureAwait(false);
            else
            {
                await WriteLineAsync("[").ConfigureAwait(false);
                Indent++;
            }

            for (var i = 0; i < node.Elements.Count; i++)
            {
                if (i != 0)
                {
                    if (Minify) await WriteAsync(",").ConfigureAwait(false);
                    else await WriteLineAsync(",").ConfigureAwait(false);
                }
                await WriteNodeAsync(node.Elements[i]).ConfigureAwait(false);
            }

            if (Minify) await WriteAsync("]").ConfigureAwait(false);
            else
            {
                Indent--;
                await WriteLineAsync().ConfigureAwait(false);
                await WriteAsync("]").ConfigureAwait(false);
            }
        }

        protected virtual async ValueTask WriteNodeAsync(JSThisExpression node)
        {
            if (node == null) throw new ArgumentNullException(nameof(node));
            await WriteAsync("this").ConfigureAwait(false);
        }

        protected virtual async ValueTask WriteNodeAsync(JSVariableDeclarator node)
        {
            if (node == null) throw new ArgumentNullException(nameof(node));
            await WriteNodeAsync(node.Identifier).ConfigureAwait(false);
            if (node.Initializer != null)
            {
                await WriteAsync(Minify ? "=" : " = ").ConfigureAwait(false);
                await WriteNodeAsync(node.Initializer).ConfigureAwait(false);
            }
        }

        protected virtual async ValueTask WriteNodeAsync(JSVariableDeclaration node)
        {
            if (node == null) throw new ArgumentNullException(nameof(node));
            await WriteNodeAsync(node.Kind).ConfigureAwait(false);
            if (!Minify) Indent++;
            for (var i = 0; i < node.Declarations.Count; i++)
            {
                if (i != 0)
                {
                    if (Minify) await WriteAsync(",").ConfigureAwait(false);
                    else await WriteLineAsync(",").ConfigureAwait(false);
                }
                await WriteNodeAsync(node.Declarations[i]);
            }
            if (Minify) await WriteAsync(";").ConfigureAwait(false);
            else
            {
                Indent--;
                await WriteLineAsync(";").ConfigureAwait(false);
            }
        }

        protected virtual async ValueTask WriteNodeAsync(JSVariableDeclarationKind kind)
        {
            await WriteAsync("var ").ConfigureAwait(false);
        }

        protected virtual async ValueTask WriteNodeAsync(JSFunctionDeclaration node)
        {
            if (node == null) throw new ArgumentNullException(nameof(node));
            await WriteAsync("function").ConfigureAwait(false);
            if (node.Identifier != null)
            {
                await WriteAsync(" ");
                await WriteNodeAsync(node.Identifier);
            }
            await WriteAsync("(").ConfigureAwait(false);
            for (var i = 0; i < node.Parameters.Count; i++)
            {
                if (i != 0) await WriteAsync(Minify ? "," : ", ").ConfigureAwait(false);
                await WriteNodeAsync(node.Parameters[i]).ConfigureAwait(false);
            }
            await WriteAsync(Minify ? ")" : ") ").ConfigureAwait(false);
            await WriteBlockAsync(node, false, true).ConfigureAwait(false);
        }

        protected virtual async ValueTask WriteNodeAsync(JSForInStatement node)
        {
            if (node == null) throw new ArgumentNullException(nameof(node));
            await WriteAsync(Minify ? "for(" : "for (").ConfigureAwait(false);

            switch (node.Left)
            {
                case var d when d.IsItem1:
                    await WriteNodeAsync(d.Item1.Kind).ConfigureAwait(false);
                    if (d.Item1.Declarations.Count > 0 && d.Item1.Declarations[0] != null)
                        await WriteNodeAsync(d.Item1.Declarations[0]).ConfigureAwait(false);
                    break;

                case var d when d.IsItem2:
                    await WriteNodeAsync(d.Item2).ConfigureAwait(false);
                    break;

                default:
                    await WriteLiteralAsync().ConfigureAwait(false);
                    break;
            }

            await WriteAsync(" in ").ConfigureAwait(false);
            await WriteNodeAsync(node.Right).ConfigureAwait(false);
            await WriteAsync(")").ConfigureAwait(false);
            await WriteBodyAsync(node, false, true).ConfigureAwait(false);
        }

        protected virtual async ValueTask<bool> WriteBodyAsync(JSBodyStatement node, bool spacer, bool newLine)
        {
            if (node.Body is JSBlockStatement block)
            {
                if (Minify && block.Body.Count == 1)
                {
                    await WriteNodeAsync(block.Body[0]).ConfigureAwait(false);
                    return false;
                }
                await WriteBlockAsync(block, spacer || !Minify, newLine).ConfigureAwait(false);
                return true;
            }
            else if (node.Body != null)
            {
                if (Minify && spacer) await WriteAsync(" ").ConfigureAwait(false);
                else if (!Minify)
                {
                    await WriteLineAsync().ConfigureAwait(false);
                    Indent++;
                }

                await WriteNodeAsync(node.Body).ConfigureAwait(false);

                if (!Minify) Indent--;
            }
            return false;
        }

        protected virtual async ValueTask WriteNodeAsync(JSForStatement node)
        {
            if (node == null) throw new ArgumentNullException(nameof(node));
            await WriteAsync(Minify ? "for(" : "for (").ConfigureAwait(false);

            switch (node.Initializer)
            {
                case var d when d.IsItem1:
                    await WriteNodeAsync(d.Item1.Kind).ConfigureAwait(false);
                    if (d.Item1.Declarations.Count > 0 && d.Item1.Declarations[0] != null)
                        await WriteNodeAsync(d.Item1.Declarations[0]).ConfigureAwait(false);
                    break;
                case var d when d.IsItem2:
                    await WriteNodeAsync(d.Item2);
                    break;
            }

            await WriteAsync(Minify ? ";" : "; ").ConfigureAwait(false);
            if (node.Test != null) await WriteNodeAsync(node.Test).ConfigureAwait(false);
            await WriteAsync(Minify ? ";" : "; ").ConfigureAwait(false);
            if (node.Update != null) await WriteNodeAsync(node.Update).ConfigureAwait(false);
            await WriteAsync(")").ConfigureAwait(false);
            await WriteBodyAsync(node, false, true).ConfigureAwait(false);
        }

        protected virtual async ValueTask WriteNodeAsync(JSDoWhileStatement node)
        {
            if (node == null) throw new ArgumentNullException(nameof(node));

            await WriteAsync("do").ConfigureAwait(false);
            var isBlock = await WriteBodyAsync(node, true, false).ConfigureAwait(false);
            if (Minify) await WriteAsync("while(").ConfigureAwait(false);
            else if (isBlock) await WriteAsync(" while (").ConfigureAwait(false);
            else await WriteAsync("while (").ConfigureAwait(false);
            await WriteNodeAsync(node.Test).ConfigureAwait(false);
            if (Minify) await WriteAsync(");").ConfigureAwait(false);
            else await WriteLineAsync(");").ConfigureAwait(false);
        }

        protected virtual async ValueTask WriteNodeAsync(JSWhileStatement node)
        {
            if (node == null) throw new ArgumentNullException(nameof(node));
            await WriteAsync(Minify ? "while(" : "while (").ConfigureAwait(false);
            await WriteNodeAsync(node.Test).ConfigureAwait(false);
            await WriteAsync(")").ConfigureAwait(false);
            await WriteBodyAsync(node, false, true).ConfigureAwait(false);
        }

        protected virtual ValueTask WriteNodeAsync(JSCatchClause node) => WriteNodeAsync(node, true);

        protected virtual async ValueTask WriteNodeAsync(JSCatchClause node, bool newLine)
        {
            if (node == null) throw new ArgumentNullException(nameof(node));
            await WriteAsync(Minify ? "catch(" : "catch (").ConfigureAwait(false);
            await WriteNodeAsync(node.Parameter).ConfigureAwait(false);
            await WriteAsync(")").ConfigureAwait(false);
            await WriteBlockAsync(node, true, newLine).ConfigureAwait(false);
        }

        protected virtual async ValueTask WriteNodeAsync(JSTryStatement node)
        {
            if (node == null) throw new ArgumentNullException(nameof(node));
            await WriteAsync("try").ConfigureAwait(false);
            await WriteBlockAsync(node, true, node.Handler == null && node.Finalizer == null).ConfigureAwait(false);
            if (node.Handler != null)
            {
                if (!Minify) await WriteAsync(" ").ConfigureAwait(false);
                await WriteNodeAsync(node.Handler, node.Finalizer == null).ConfigureAwait(false);
            }
            if (node.Finalizer.Body.Count > 0)
            {
                await WriteAsync(Minify ? "finally" : " finally ").ConfigureAwait(false);
                await WriteNodeAsync(node.Finalizer).ConfigureAwait(false);
            }
        }

        protected virtual async ValueTask WriteNodeAsync(JSThrowStatement node)
        {
            if (node == null) throw new ArgumentNullException(nameof(node));
            await WriteAsync("throw ").ConfigureAwait(false);
            await WriteNodeAsync(node.Expression).ConfigureAwait(false);
            if (Minify) await WriteAsync(";").ConfigureAwait(false);
            else await WriteLineAsync(";").ConfigureAwait(false);
        }

        protected virtual async ValueTask WriteNodeAsync(JSSwitchCase node)
        {
            if (node == null) throw new ArgumentNullException(nameof(node));

            if (node.Test != null)
            {
                await WriteAsync("case ").ConfigureAwait(false);
                await WriteNodeAsync(node.Test).ConfigureAwait(false);
            }
            else
                await WriteAsync("default").ConfigureAwait(false);

            if (Minify) await WriteAsync(":").ConfigureAwait(false);
            else
            {
                await WriteLineAsync(":").ConfigureAwait(false);
                Indent++;
            }

            for (var i = 0; i < node.Body.Count; i++)
            {
                await WriteNodeAsync(node.Body[i]).ConfigureAwait(false);
            }

            if (!Minify) Indent--;
        }

        protected virtual async ValueTask WriteNodeAsync(JSSwitchStatement node)
        {
            if (node == null) throw new ArgumentNullException(nameof(node));
            await WriteAsync(Minify ? "switch(" : "switch (").ConfigureAwait(false);
            await WriteNodeAsync(node.Discriminant).ConfigureAwait(false);
            await WriteAsync(Minify ? "){" : ") {").ConfigureAwait(false);
            if (!Minify)
            {
                await WriteLineAsync().ConfigureAwait(false);
                Indent++;
            }
            for (var i = 0; i < node.Cases.Count; i++)
            {
                await WriteNodeAsync(node.Cases[i]).ConfigureAwait(false);
            }
            if (Minify) await WriteAsync("}").ConfigureAwait(false);
            else
            {
                Indent--;
                await WriteLineAsync("}").ConfigureAwait(false);
            }
        }

        protected virtual async ValueTask WriteNodeAsync(JSIfStatement node)
        {
            if (node == null) throw new ArgumentNullException(nameof(node));

            if (Minify &&
                node.Body is JSExpressionStatement consequent &&
                node.Alternate is JSExpressionStatement alternate)
            {
                await WriteNodeAsync(node.Test).ConfigureAwait(false);
                await WriteAsync("?").ConfigureAwait(false);
                await WriteNodeAsync(consequent.Expression).ConfigureAwait(false);
                await WriteAsync(":").ConfigureAwait(false);
                await WriteNodeAsync(alternate.Expression).ConfigureAwait(false);
                await WriteAsync(";").ConfigureAwait(false);
                return;
            }

            await WriteAsync(Minify ? "if(" : "if (").ConfigureAwait(false);
            await WriteNodeAsync(node.Test).ConfigureAwait(false);
            await WriteAsync(")").ConfigureAwait(false);

            var isBlock = await WriteBodyAsync(node, false, node.Alternate == null).ConfigureAwait(false);

            if (node.Alternate != null)
            {
                if (isBlock && !Minify) await WriteAsync(" ").ConfigureAwait(false);
                await WriteAsync("else").ConfigureAwait(false);
                if (node.Alternate is JSBlockStatement block)
                {
                    if (Minify && block.Body.Count == 1)
                    {
                        await WriteAsync(" ").ConfigureAwait(false);
                        await WriteNodeAsync(block.Body[0]).ConfigureAwait(false);
                        return;
                    }

                    if (!Minify) await WriteAsync(" ").ConfigureAwait(false);
                    await WriteNodeAsync(node.Alternate).ConfigureAwait(false);
                    return;
                }

                if (Minify) await WriteAsync(" ").ConfigureAwait(false);
                else
                {
                    await WriteLineAsync().ConfigureAwait(false);
                    Indent++;
                }

                await WriteNodeAsync(node.Alternate).ConfigureAwait(false);
                if (!Minify) Indent--;
            }
        }

        protected virtual async ValueTask WriteNodeAsync(JSContinueStatement node)
        {
            if (node == null) throw new ArgumentNullException(nameof(node));
            await WriteAsync(node.Label == null ? "continue" : "continue ").ConfigureAwait(false);
            if (node.Label != null) await WriteNodeAsync(node.Label).ConfigureAwait(false);
            if (Minify) await WriteAsync(";").ConfigureAwait(false);
            else await WriteLineAsync(";").ConfigureAwait(false);
        }

        protected virtual async ValueTask WriteNodeAsync(JSBreakStatement node)
        {
            if (node == null) throw new ArgumentNullException(nameof(node));
            await WriteAsync(node.Label == null ? "break" : "break ").ConfigureAwait(false);
            if (node.Label != null) await WriteNodeAsync(node.Label).ConfigureAwait(false);
            if (Minify) await WriteAsync(";").ConfigureAwait(false);
            else await WriteLineAsync(";").ConfigureAwait(false);
        }

        protected virtual async ValueTask WriteNodeAsync(JSLabeledStatement node)
        {
            if (node == null) throw new ArgumentNullException(nameof(node));
            if (node.Label != null) await WriteNodeAsync(node.Label).ConfigureAwait(false);
            if (Minify) await WriteAsync(":").ConfigureAwait(false);
            else await WriteLineAsync(":").ConfigureAwait(false);
            if (node.Body != null) await WriteNodeAsync(node.Body).ConfigureAwait(false);
        }

        protected virtual async ValueTask WriteNodeAsync(JSReturnStatement node)
        {
            if (node == null) throw new ArgumentNullException(nameof(node));
            await WriteAsync("return").ConfigureAwait(false);
            if (node.Expression != null)
            {
                await WriteAsync(" ").ConfigureAwait(false);
                await WriteNodeAsync(node.Expression).ConfigureAwait(false);
            }
            if (Minify) await WriteAsync(";").ConfigureAwait(false);
            else await WriteLineAsync(";").ConfigureAwait(false);
        }

        protected virtual async ValueTask WriteNodeAsync(JSWithStatement node)
        {
            if (node == null) throw new ArgumentNullException(nameof(node));
            await WriteAsync(Minify ? "with(" : "with (").ConfigureAwait(false);
            await WriteNodeAsync(node.Object).ConfigureAwait(false);
            await WriteAsync(")").ConfigureAwait(false);
            await WriteBodyAsync(node, false, true).ConfigureAwait(false);
        }

        protected virtual async ValueTask WriteNodeAsync(JSDebuggerStatement node)
        {
            if (node == null) throw new ArgumentNullException(nameof(node));
            if (Minify) await WriteAsync("debugger;").ConfigureAwait(false);
            else await WriteLineAsync("debugger;").ConfigureAwait(false);
        }

        protected virtual async ValueTask WriteNodeAsync(JSExpressionStatement node)
        {
            if (node == null) throw new ArgumentNullException(nameof(node));
            if (node.Expression != null) await WriteNodeAsync(node.Expression);
            if (Minify) await WriteAsync(";").ConfigureAwait(false);
            else await WriteLineAsync(";").ConfigureAwait(false);
        }

        protected virtual async ValueTask WriteBlockAsync(IJSBlock node, bool spacer, bool newLine)
        {
            if (!Minify && spacer) await WriteAsync(" ").ConfigureAwait(false);

            if (node.Body.Count == 0)
            {
                if (!newLine || Minify) await WriteAsync("{}").ConfigureAwait(false);
                else await WriteLineAsync("{}").ConfigureAwait(false);
                return;
            }

            if (Minify) await WriteAsync("{").ConfigureAwait(false);
            else
            {
                await WriteLineAsync("{").ConfigureAwait(false);
                Indent++;
            }

            for (var i = 0; i < node.Body.Count; i++)
            {
                await WriteNodeAsync(node.Body[i]).ConfigureAwait(false);
            }

            if (!Minify) Indent--;
            if (!newLine || Minify) await WriteAsync("}").ConfigureAwait(false);
            else await WriteLineAsync("}").ConfigureAwait(false);
        }

        protected virtual ValueTask WriteNodeAsync(IJSFunction node) => WriteNodeAsync(node, true);

        protected virtual async ValueTask WriteNodeAsync(IJSFunction node, bool newLine)
        {
            if (node == null) throw new ArgumentNullException(nameof(node));

            if (node.Body.Count == 0)
            {
                if (!newLine || Minify) await WriteAsync("{}").ConfigureAwait(false);
                else await WriteLineAsync("{}").ConfigureAwait(false);
                return;
            }

            if (Minify) await WriteAsync("{").ConfigureAwait(false);
            else
            {
                await WriteLineAsync("{").ConfigureAwait(false);
                Indent++;
            }

            for (var i = 0; i < node.Body.Count; i++)
            {
                await WriteNodeAsync(node.Body[i]).ConfigureAwait(false);
            }

            if (!Minify) Indent--;
            if (!newLine || Minify) await WriteAsync("}").ConfigureAwait(false);
            else await WriteLineAsync("}").ConfigureAwait(false);
        }

        protected virtual async ValueTask WriteNodeAsync(JSProgram node)
        {
            if (node == null) throw new ArgumentNullException(nameof(node));

            for (var i = 0; i < node.Body.Count; i++)
            {
                var body = node.Body[i];
                await WriteNodeAsync(body).ConfigureAwait(false);
            }
        }

        protected virtual async ValueTask WriteNodeAsync(JSIdentifier node)
        {
            if (node == null) throw new ArgumentNullException(nameof(node));
            await WriteAsync(node.Name).ConfigureAwait(false);
        }

        protected virtual async ValueTask WriteNodeAsync(JSLiteral node)
        {
            if (node == null) throw new ArgumentNullException(nameof(node));

            if (node.Value is string s) await WriteLiteralAsync(s).ConfigureAwait(false);
            else if (node.Value is bool b) await WriteLiteralAsync(b).ConfigureAwait(false);
            else if (node.Value == null) await WriteLiteralAsync().ConfigureAwait(false);
            else if (node.Value is JSRegex r) await WriteLiteralAsync(r).ConfigureAwait(false);
            else if (node.Value is byte uint8) await WriteLiteralAsync(uint8).ConfigureAwait(false);
            else if (node.Value is sbyte int8) await WriteLiteralAsync(int8).ConfigureAwait(false);
            else if (node.Value is ushort uint16) await WriteLiteralAsync(uint16).ConfigureAwait(false);
            else if (node.Value is short int16) await WriteLiteralAsync(int16).ConfigureAwait(false);
            else if (node.Value is uint uint32) await WriteLiteralAsync(uint32).ConfigureAwait(false);
            else if (node.Value is int int32) await WriteLiteralAsync(int32).ConfigureAwait(false);
            else if (node.Value is ulong uint64) await WriteLiteralAsync(uint64).ConfigureAwait(false);
            else if (node.Value is long int64) await WriteLiteralAsync(int64).ConfigureAwait(false);
            else if (node.Value is float float32) await WriteLiteralAsync(float32).ConfigureAwait(false);
            else if (node.Value is double float64) await WriteLiteralAsync(float64).ConfigureAwait(false);
            else if (node.Value is decimal decimal128) await WriteLiteralAsync(decimal128).ConfigureAwait(false);
            else if (node.Value is IFormattable formattable) await WriteLiteralAsync(formattable).ConfigureAwait(false);
            else await WriteLiteralAsync(node.Value).ConfigureAwait(false);
        }

        protected virtual ValueTask WriteLiteralAsync(object value) => new ValueTask(base.WriteAsync(value?.ToString()));

        protected virtual ValueTask WriteLiteralAsync(IFormattable value) => WriteAsync(value.ToString(null, CultureInfo.InvariantCulture));

        protected virtual ValueTask WriteLiteralAsync(decimal value) => WriteAsync(value);

        protected virtual ValueTask WriteLiteralAsync(double value) => WriteAsync(value);

        protected virtual ValueTask WriteLiteralAsync(float value) => WriteAsync(value);

        protected virtual ValueTask WriteLiteralAsync(long value) => WriteAsync(value);

        protected virtual ValueTask WriteLiteralAsync(ulong value) => WriteAsync(value);

        protected virtual ValueTask WriteLiteralAsync(int value) => WriteAsync(value);

        protected virtual ValueTask WriteLiteralAsync(uint value) => WriteAsync(value);

        protected virtual ValueTask WriteLiteralAsync(short value) => WriteAsync(value);

        protected virtual ValueTask WriteLiteralAsync(ushort value) => WriteAsync(value);

        protected virtual ValueTask WriteLiteralAsync(sbyte value) => WriteAsync(value);

        protected virtual ValueTask WriteLiteralAsync(byte value) => WriteAsync(value);

        protected virtual ValueTask WriteLiteralAsync(bool value) => WriteAsync(value ? "true" : "false");

        protected virtual ValueTask WriteLiteralAsync() => WriteAsync("null");

        protected virtual async ValueTask WriteLiteralAsync(JSRegex value)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));

            await WriteAsync("/").ConfigureAwait(false);
            for (var i = 0; i < value.Pattern.Length; i++)
            {
                var c = value.Pattern[i];
                if (c == '/') await WriteAsync("\\/").ConfigureAwait(false);
                else await WriteAsync(c).ConfigureAwait(false);
            }
            await WriteAsync("/").ConfigureAwait(false);
            if (value.Options.HasFlag(JSRegexOptions.Global)) await WriteAsync("g").ConfigureAwait(false);
            if (value.Options.HasFlag(JSRegexOptions.IgnoreCase)) await WriteAsync("i").ConfigureAwait(false);
            if (value.Options.HasFlag(JSRegexOptions.Multiline)) await WriteAsync("m").ConfigureAwait(false);
            if (value.Options.HasFlag(JSRegexOptions.Unicode)) await WriteAsync("u").ConfigureAwait(false);
            if (value.Options.HasFlag(JSRegexOptions.Sticky)) await WriteAsync("y").ConfigureAwait(false);
        }

        protected virtual async ValueTask WriteLiteralAsync(string value)
        {
            if (value == null)
            {
                await WriteLiteralAsync().ConfigureAwait(false);
                return;
            }

            await WriteAsync("'").ConfigureAwait(false);
            for (var i = 0; i < value.Length; i++)
            {
                var c = value[i];
                switch (c)
                {
                    case '\\': await WriteAsync("\\\\").ConfigureAwait(false); break;
                    case '\'': await WriteAsync("\\'").ConfigureAwait(false); break;
                    case '\n': await WriteAsync("\\n").ConfigureAwait(false); break;
                    case '\r': await WriteAsync("\\r").ConfigureAwait(false); break;
                    case '\t': await WriteAsync("\\t").ConfigureAwait(false); break;
                    case '\b': await WriteAsync("\\b").ConfigureAwait(false); break;
                    case '\f': await WriteAsync("\\f").ConfigureAwait(false); break;
                    case '\v': await WriteAsync("\\v").ConfigureAwait(false); break;
                    case '\0': await WriteAsync("\\0").ConfigureAwait(false); break;
                    default:
                        if (c < ' ' || c > '~' && c < 256)
                        {
                            await WriteAsync("\\x").ConfigureAwait(false);
                            await WriteAsync(((int)c).ToString("X2", CultureInfo.InvariantCulture)).ConfigureAwait(false);
                        }
                        else
                        {
                            await WriteAsync(c).ConfigureAwait(false);
                        }
                        break;
                }
            }
            await WriteAsync("'").ConfigureAwait(false);
        }
    }
}
