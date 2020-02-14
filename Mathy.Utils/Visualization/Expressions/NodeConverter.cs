using Mathy.Utils.Dandelion;
using Mathy.Visualization.Expressions.Nodes;
using System;
using System.Linq;

namespace Mathy.Visualization.Expressions
{
    class NodeConverter
    {
        public Expression Convert(Cherimoya.Expressions.Expression root)
        {
            return Convert(root, 0);
        }

        private Expression Convert(Cherimoya.Expressions.Expression root, int parantheseLevel)
        {
            if (root is Cherimoya.Expressions.ConstantExpression)
            {
                return ConvertConstantExpression(root as Cherimoya.Expressions.ConstantExpression);
            }
            else if (root is Cherimoya.Expressions.BinaryExpression)
            {
                return ConvertBinaryExpression(root as Cherimoya.Expressions.BinaryExpression, parantheseLevel);
            }
            else if (root is Cherimoya.Expressions.FunctionCallExpression)
            {
                return ConvertFunctionCallExpression(root as Cherimoya.Expressions.FunctionCallExpression, parantheseLevel);
            }
            else if (root is Cherimoya.Expressions.VariableExpression)
            {
                return ConvertVariableExpression(root as Cherimoya.Expressions.VariableExpression);
            }
            else if (root is Cherimoya.Expressions.LambdaExpression)
            {
                return ConvertLambdaExpression(root as Cherimoya.Expressions.LambdaExpression, parantheseLevel);
            }
            else if (root is Mathy.Language.MatrixExpression)
            {
                return ConvertMatrixExpression(root as Language.MatrixExpression, parantheseLevel);
            }
            else if (root is Mathy.Language.MultiIndexingExpression)
            {
                return ConvertMultiIndexingExpression(root as Mathy.Language.MultiIndexingExpression, parantheseLevel);
            }
            else if (root is Mathy.Language.VariableContextExpression)
            {
                return ConvertVariableContextExpression(root as Mathy.Language.VariableContextExpression, parantheseLevel);
            }
            else if (root is Mathy.Language.MultipleVariableExpression)
            {
                return ConvertMultipleVariableExpression(root as Mathy.Language.MultipleVariableExpression, parantheseLevel);
            }
            else if (root is Mathy.Language.DictionaryExpression)
            {
                return ConvertDictionaryExpression(root as Mathy.Language.DictionaryExpression, parantheseLevel);
            }
            else if (root is Mathy.Language.ArrayExpression)
            {
                return ConvertArrayExpression(root as Mathy.Language.ArrayExpression, parantheseLevel);
            }
            else if (root is Cherimoya.Expressions.IfElseExpression)
            {
                return ConvertIfElseExpression(root as Cherimoya.Expressions.IfElseExpression, parantheseLevel);
            }
            else if (root is Mathy.Language.IterationSumExpression)
            {
                return ConvertIterationSumExpression(root as Mathy.Language.IterationSumExpression, parantheseLevel);
            }
            else if (root is Mathy.Language.PredefinedConstantExpression)
            {
                return ConvertPredefinedConstantExpression(root as Mathy.Language.PredefinedConstantExpression);
            }


            throw new Exception(string.Format("Cannot convert {0}.", root));
        }


        private Expression ConvertConstantExpression(Cherimoya.Expressions.ConstantExpression e)
        {
            return new ConstantExpression() { Value = e.Value is string ? "\"" + e.Value + "\"" : e.Value.ToString() };
        }


        private static bool IsInvididualExpression(Cherimoya.Expressions.Expression expression)
        {
            return expression is Cherimoya.Expressions.VariableExpression ||
                   expression is Cherimoya.Expressions.ConstantExpression ||
                   expression is Cherimoya.Expressions.FunctionCallExpression;
        }


        private Expression ConvertBinaryExpression(Cherimoya.Expressions.BinaryExpression e, int parantheseLevel)
        {
            Cherimoya.Expressions.BinaryOperator oper = e.Operator;
            BinaryOperator op = GetBinaryOpeator(oper);
            BinaryOperator prevOp = op;


            Cherimoya.Expressions.Expression leftExpr = e.Left;
            Cherimoya.Expressions.Expression rightExpr = e.Right;


            Boolean swap = false;

            if (op == BinaryOperator.Multiply)
            {
                if (IsInvididualExpression(e.Right) && !IsInvididualExpression(e.Left) &&
                    !(e.Left is Cherimoya.Expressions.BinaryExpression))
                {
                    swap = true;
                }
                else if (e.Right is Cherimoya.Expressions.ConstantExpression && !(e.Left is Cherimoya.Expressions.ConstantExpression))
                {
                    swap = true;
                }
            }

            if (swap)
            {
                Cherimoya.Expressions.Expression t = leftExpr;
                leftExpr = rightExpr;
                rightExpr = t;
            }


            bool leftParanthese = false;
            bool rightParanthese = false;

            if (op != BinaryOperator.Assign)
            {
                if (op != BinaryOperator.Divide)
                {
                    if (leftExpr is Cherimoya.Expressions.BinaryExpression)
                    {
                        Cherimoya.Expressions.BinaryOperator leftOp = (leftExpr as Cherimoya.Expressions.BinaryExpression).Operator;

                        if (GetOperatorLevel(oper) > GetOperatorLevel(leftOp))
                        {
                            leftParanthese = true;
                        }
                    }

                    if (rightExpr is Cherimoya.Expressions.BinaryExpression)
                    {
                        Cherimoya.Expressions.BinaryOperator rightOp = (rightExpr as Cherimoya.Expressions.BinaryExpression).Operator;

                        if (GetBinaryOpeator(rightOp) == op)
                        {
                            rightParanthese = true;
                        }
                        else if (GetOperatorLevel(oper) > GetOperatorLevel(rightOp))
                        {
                            rightParanthese = true;
                        }
                    }
                }

                if (rightExpr is Cherimoya.Expressions.FunctionCallExpression && (rightExpr as Cherimoya.Expressions.FunctionCallExpression).MethodName == "root")
                {
                    rightParanthese = true;
                }
            }

            if (op == BinaryOperator.Multiply)
            {
                if (!(leftExpr is Cherimoya.Expressions.ConstantExpression & rightExpr is Cherimoya.Expressions.ConstantExpression))
                {
                    op = BinaryOperator.None;
                }
            }


            Expression left = Convert(leftExpr, parantheseLevel + (leftParanthese ? 1 : 0));
            Expression right = Convert(rightExpr, parantheseLevel + (rightParanthese ? 1 : 0));

            if (leftParanthese)
            {
                left = new ParantheseExpression() { Body = left, ParantheseLevel = parantheseLevel };
            }

            if (rightParanthese)
            {
                right = new ParantheseExpression() { Body = right, ParantheseLevel = parantheseLevel };
            }


            return new BinaryExpression()
            {
                Left = left,
                Operator = new BinaryOperatorNode() { Operator = op },
                ActualOperator = new BinaryOperatorNode() { Operator = prevOp },
                Right = right
            };
        }

        private Expression ConvertFunctionCallExpression(Cherimoya.Expressions.FunctionCallExpression e, int parantheseLevel)
        {
            if (e.Method.Name == "root")
            {
                if ((e.Parameters[1] is Cherimoya.Expressions.ConstantExpression)
                    && Types.ConvertValue<int>((e.Parameters[1] as Cherimoya.Expressions.ConstantExpression).Value) == 2)
                {
                    return new RootExpression() { X = Convert(e.Parameters[0], parantheseLevel) };
                }
                else
                {
                    return new RootExpression() { X = Convert(e.Parameters[0], parantheseLevel), Root = Convert(e.Parameters[1], parantheseLevel) };
                }
            }
            else if (e.Method.Name == "abs")
            {
                return new AbsExpression() { Operand = Convert(e.Parameters[0], parantheseLevel) };
            }
            else if (e.Method.Name == "pow")
            {
                bool hasParanthese = e.Parameters[0] is Cherimoya.Expressions.BinaryExpression;

                Expression operand = Convert(e.Parameters[0], parantheseLevel + (hasParanthese ? 1 : 0));

                if (hasParanthese)
                {
                    operand = new ParantheseExpression() { Body = operand };
                }


                return new PowExpression() { X = operand, Pow = Convert(e.Parameters[1], parantheseLevel) };
            }
            else if (e.Method.Name == "sum")
            {
                Cherimoya.Expressions.Expression right = e.Parameters[0];


                bool hasParanthese = right is Cherimoya.Expressions.BinaryExpression;

                Expression operand = Convert(right, parantheseLevel + (hasParanthese ? 1 : 0));

                if (hasParanthese)
                {
                    operand = new ParantheseExpression() { Body = operand };
                }

                return new SumExpression()
                {
                    Sigmas = new SigmaConfig[] { new SigmaConfig() },
                    Operand = operand
                };
            }
            else if (e.Method.Name == "lg" || e.Method.Name == "ln" || e.Method.Name == "log")
            {
                Cherimoya.Expressions.Expression xExpr = e.Method.Name == "log" ? e.Parameters[1] : e.Parameters[0];

                bool hasParanthese = xExpr is Cherimoya.Expressions.BinaryExpression && (xExpr as Cherimoya.Expressions.BinaryExpression).Operator != Cherimoya.Expressions.BinaryOperator.Divide;

                Expression b = e.Method.Name == "log" ? Convert(e.Parameters[0], parantheseLevel) : null;
                Expression x = hasParanthese ? new ParantheseExpression() { Body = Convert(xExpr, parantheseLevel + 1) } : Convert(xExpr, parantheseLevel);

                return new LogExpression(e.Method.Name, b, x);
            }
            else if (e.Method.Name == "transpose")
            {
                return new SuperscriptExpression() { Body = Convert(e.Parameters[0], parantheseLevel), Superscript = new TextExpression("T", false), HasBracket = true };
            }
            else if (e.Method.Name == "stdvar")
            {
                return new SubscriptExpression() { Body = new TextExpression("σ", false), Subscript = Convert(e.Parameters[0], parantheseLevel), ShiftOffset = 10 };
            }
            else if (e.Method.Name == "average")
            {
                return new UpperlineExpression() { Body = Convert(e.Parameters[0], parantheseLevel) };
            }
            else if (e.Method.Name == "diff" || e.Method.Name == "pdiff")
            {
                return ConvertDiffExpression(e);
            }
            else
            {
                return new FunctionExpression()
                {
                    Name = e.MethodName,
                    Operands = e.Parameters.Select(i => Convert(i, parantheseLevel + 1)).ToArray(),
                    ParantheseLevel = parantheseLevel
                };
            }
        }

        private Expression ConvertDiffExpression(Cherimoya.Expressions.FunctionCallExpression e)
        {
            Expression f = Convert(e.Parameters[0]);

            return new DiffExpression(
                   e.MethodName == "pdiff",
                   f is BinaryExpression ? new ParantheseExpression() { Body = f } : f,
                   new TextExpression((string)(e.Parameters[1] as Cherimoya.Expressions.ConstantExpression).Value, false));
        }

        private Expression ConvertVariableExpression(Cherimoya.Expressions.VariableExpression e)
        {
            return new TextExpression(e.VariableName, true);
        }

        private Expression ConvertLambdaExpression(Cherimoya.Expressions.LambdaExpression e, int parantheseLevel)
        {
            return new LambdaExpression() { Variables = e.VariableNames.Select(i => new TextExpression(i, true)).ToArray(), Body = Convert(e.Body, parantheseLevel) };
        }

        private Expression ConvertMatrixExpression(Language.MatrixExpression e, int parantheseLevel)
        {
            return new MatrixExpression() { Rows = e.Rows.Select(i => i.Select(j => Convert(j, parantheseLevel)).ToArray()).ToArray() };
        }

        private Expression ConvertMultiIndexingExpression(Mathy.Language.MultiIndexingExpression e, int parantheseLevel)
        {
            return new MultiIndexingExpression() { Operand = Convert(e.Operand, parantheseLevel), Indexers = e.Indexers.Select(i => Convert(i, parantheseLevel)).ToArray() };
        }

        private Expression ConvertVariableContextExpression(Mathy.Language.VariableContextExpression e, int parantheseLevel)
        {
            return Convert(e.Expression, parantheseLevel);
        }

        private Expression ConvertMultipleVariableExpression(Mathy.Language.MultipleVariableExpression e, int parantheseLevel)
        {
            return new ArrayExpression() { Items = e.Variables.Select(i => new TextExpression(i, true)).ToArray() };
        }

        private Expression ConvertDictionaryExpression(Mathy.Language.DictionaryExpression e, int parantheseLevel)
        {
            return new DictionaryExpression() { Dictionary = e.Dictionary.ToDictionary(i => i.Key, i => Convert(i.Value, parantheseLevel)) };
        }

        private Expression ConvertArrayExpression(Mathy.Language.ArrayExpression e, int parantheseLevel)
        {
            return new ArrayExpression() { Items = e.Items.Select(i => Convert(i, 0)).ToArray() };
        }

        private Expression ConvertIfElseExpression(Cherimoya.Expressions.IfElseExpression e, int parantheseLevel)
        {
            return new IfElseExpression()
            {
                Condition = Convert(e.Condition, parantheseLevel),
                PositiveBranch = Convert(e.PositiveBranch, parantheseLevel),
                NegativeBranch = Convert(e.NegativeBranch, parantheseLevel)
            };
        }

        private Expression ConvertIterationSumExpression(Mathy.Language.IterationSumExpression e, int parantheseLevel)
        {
            bool hasParanthese = e.Body is Cherimoya.Expressions.BinaryExpression;


            Expression operand = Convert(e.Body, parantheseLevel + (hasParanthese ? 1 : 0));

            if (hasParanthese)
            {
                operand = new ParantheseExpression() { Body = operand };
            }


            return new SumExpression()
            {
                Sigmas = e.Variables.Select(i => new SigmaConfig()
                {
                    To = Convert(i.To, parantheseLevel),
                    From = new BinaryExpression()
                    {
                        Left = new TextExpression(i.Name, true),
                        Right = Convert(i.From, parantheseLevel),
                        Operator = new BinaryOperatorNode() { Operator = BinaryOperator.Equal },
                        ActualOperator = new BinaryOperatorNode() { Operator = BinaryOperator.Equal }
                    }
                }).ToArray(),
                Operand = operand
            };
        }


        private static BinaryOperator GetBinaryOpeator(Cherimoya.Expressions.BinaryOperator oper)
        {
            BinaryOperator op = BinaryOperator.None;

            if (oper == Cherimoya.Expressions.BinaryOperator.Add)
            {
                op = BinaryOperator.Add;
            }
            else if (oper == Cherimoya.Expressions.BinaryOperator.Subtract)
            {
                op = BinaryOperator.Subtract;
            }
            else if (oper == Cherimoya.Expressions.BinaryOperator.Multiply)
            {
                op = BinaryOperator.Multiply;
            }
            else if (oper == Cherimoya.Expressions.BinaryOperator.Divide)
            {
                op = BinaryOperator.Divide;
            }
            else if (oper == Cherimoya.Expressions.BinaryOperator.Assign)
            {
                op = BinaryOperator.Assign;
            }
            else if (oper == Cherimoya.Expressions.BinaryOperator.LessThan)
            {
                op = BinaryOperator.LessThan;
            }
            else if (oper == Cherimoya.Expressions.BinaryOperator.LessEqualThan)
            {
                op = BinaryOperator.LessEqualThan;
            }
            else if (oper == Cherimoya.Expressions.BinaryOperator.GreaterThan)
            {
                op = BinaryOperator.GreaterThan;
            }
            else if (oper == Cherimoya.Expressions.BinaryOperator.GreaterEqualThan)
            {
                op = BinaryOperator.GreaterEqualThan;
            }
            else if (oper == Cherimoya.Expressions.BinaryOperator.Equal)
            {
                op = BinaryOperator.Equal;
            }
            else if (oper == Cherimoya.Expressions.BinaryOperator.NotEqual)
            {
                op = BinaryOperator.NotEqual;
            }

            return op;
        }


        private int GetOperatorLevel(Cherimoya.Expressions.BinaryOperator op)
        {
            if (op == Cherimoya.Expressions.BinaryOperator.Add || op == Cherimoya.Expressions.BinaryOperator.Subtract)
            {
                return 0;
            }
            else
            {
                return 1;
            }
        }


        private Expression ConvertPredefinedConstantExpression(Language.PredefinedConstantExpression e)
        {
            return new TextExpression(e.Name, false);
        }
    }
}
