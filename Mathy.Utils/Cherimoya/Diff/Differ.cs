using Cherimoya.Expressions;
using System;
using System.Linq;

namespace Cherimoya.Diff
{
    public class Differ
    {
        public Differ(Expression root, string x)
        {
            this.root = root;
            this.x = x;
        }


        private Expression root;

        private string x;


        public Expression Calculate(VariableContext variableContext)
        {
            return CalculateDiff(root);
        }

        private bool IsConstant(Expression e)
        {
            return e.IsConstantExpression() || e.Flatten().All(i => i is VariableExpression && (i as VariableExpression).VariableName != x);
        }

        private Expression CalculateDiff(Expression e)
        {
            if (IsConstant(e))
            {
                return ConstantExpression.create(0, e.FromPosition, e.ToPosition);
            }
            else if (e is VariableExpression)
            {
                return ConstantExpression.create(1, e.FromPosition, e.ToPosition);
            }
            else if (e is FunctionCallExpression)
            {
                FunctionCallExpression func = e as FunctionCallExpression;

                // (x^n)' = n * x^(n-1)
                if (func.MethodName == "pow")
                {
                    Expression p0 = func.Parameters[0];
                    Expression p1 = func.Parameters[1];

                    if (IsConstant(p1))
                    {
                        return BinaryExpression.Create(BinaryOperator.Multiply,
                                CalculateDiff(p0),
                                BinaryExpression.Create(BinaryOperator.Multiply,
                                    p1,
                                    new FunctionCallExpression("pow", new Expression[]
                                        {
                                            p0,
                                            BinaryExpression.Create(BinaryOperator.Subtract, p1, ConstantExpression.create(1, p1.FromPosition, p1.ToPosition))
                                        },
                                        p0.FromPosition,
                                        p0.ToPosition))
                               );
                    }
                }
                // (sin(u))' = cos(u) * u'
                else if (func.MethodName == "sin")
                {
                    Expression p0 = func.Parameters[0];
                    return BinaryExpression.Create(BinaryOperator.Multiply,
                               CalculateDiff(p0),
                               new FunctionCallExpression("cos", new Expression[] { p0 }, 0, 0)
                              );
                }
                // (cos(u))' = -sin(u) * u'
                else if (func.MethodName == "cos")
                {
                    Expression p0 = func.Parameters[0];
                    return BinaryExpression.Create(BinaryOperator.Multiply,
                               CalculateDiff(p0),
                               new FunctionCallExpression("sin", new Expression[] { p0 }, 0, 0)
                              );
                }
            }
            else if (e is UnaryExpression)
            {
                UnaryExpression unary = e as UnaryExpression;

                // (-x)' = -(x')
                if (unary.Operator == UnaryOperator.Negation)
                {
                    return new UnaryExpression(UnaryOperator.Negation, CalculateDiff(unary.Operand), unary.FromPosition, unary.ToPosition);
                }
            }
            else if (e is BinaryExpression)
            {
                BinaryExpression binary = e as BinaryExpression;

                // (u + v)' = u' + v'
                if (binary.Operator == BinaryOperator.Add)
                {
                    return BinaryExpression.Create(BinaryOperator.Add, CalculateDiff(binary.Left), CalculateDiff(binary.Right));
                }
                // (u - v)' = u' - v'
                else if (binary.Operator == BinaryOperator.Subtract)
                {
                    return BinaryExpression.Create(BinaryOperator.Subtract, CalculateDiff(binary.Left), CalculateDiff(binary.Right));
                }
                else if (binary.Operator == BinaryOperator.Multiply)
                {
                    // (c*u)' = c(u')
                    if (IsConstant(binary.Left))
                    {
                        return BinaryExpression.Create(
                            BinaryOperator.Multiply,
                            binary.Left,
                            CalculateDiff(binary.Right));
                    }
                    // (u*c)' = c(u')
                    else if (IsConstant(binary.Right))
                    {
                        return BinaryExpression.Create(
                          BinaryOperator.Multiply,
                          binary.Right,
                          CalculateDiff(binary.Left));
                    }
                    // (u*v)' = uv' + vu'
                    else
                    {
                        return BinaryExpression.Create(BinaryOperator.Add,
                            BinaryExpression.Create(BinaryOperator.Multiply, binary.Right, CalculateDiff(binary.Left)),
                            BinaryExpression.Create(BinaryOperator.Multiply, binary.Left, CalculateDiff(binary.Right)));
                    }
                }
                else if (binary.Operator == BinaryOperator.Divide)
                {
                    // (c/u)' = -cu^(-2)
                    if (IsConstant(binary.Left))
                    {
                        return BinaryExpression.Create(
                          BinaryOperator.Multiply,
                          new UnaryExpression(UnaryOperator.Negation, binary.Left, binary.Left.FromPosition, binary.Left.ToPosition),
                          new FunctionCallExpression("pow",
                              new Expression[]
                              {
                                  binary.Right,
                                  ConstantExpression.create(-2, binary.Right.FromPosition, binary.Right.ToPosition)
                              },
                              binary.Right.FromPosition,
                              binary.Right.ToPosition));
                    }
                    // (u/c)' = (1/c)u'
                    else if (IsConstant(binary.Right))
                    {
                        return BinaryExpression.Create(
                          BinaryOperator.Multiply,
                          BinaryExpression.Create(BinaryOperator.Divide,
                            ConstantExpression.create(1, binary.Right.FromPosition, binary.Right.ToPosition),
                            binary.Right),
                          CalculateDiff(binary.Left));
                    }
                    // (u/v)' = (vu'-uv')/v^2
                    else
                    {
                        return
                            BinaryExpression.Create(BinaryOperator.Divide,
                                BinaryExpression.Create(BinaryOperator.Subtract,
                                    BinaryExpression.Create(BinaryOperator.Multiply, binary.Left, CalculateDiff(binary.Right)),
                                    BinaryExpression.Create(BinaryOperator.Multiply, binary.Right, CalculateDiff(binary.Left))),
                                new FunctionCallExpression("pow", new Expression[] { binary.Right }, binary.Right.FromPosition, binary.Right.ToPosition)
                           );
                    }
                }
            }


            throw new Exception();
        }
    }
}
