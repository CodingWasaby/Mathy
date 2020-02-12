using Cherimoya.Expressions;

namespace Cherimoya.Reduction.Rules
{
    class ReduceBinaryExpression : ReductionRule
    {
        public override Expression Reduce(Expression root, ExpressionReductor reductor)
        {
            if (!(root is BinaryExpression))
            {
                return root;
            }


            Expression left = reductor.Reduce((root as BinaryExpression).Left);
            Expression right = reductor.Reduce((root as BinaryExpression).Right);

            BinaryOperator op = (root as BinaryExpression).Operator;

            if (op == BinaryOperator.Add)
            {
                if (left.IsConstant(0))
                {
                    return right;
                }
                else if (right.IsConstant(0))
                {
                    return left;
                }
            }
            else if (op == BinaryOperator.Subtract)
            {
                if (left.IsConstant(0))
                {
                    return new UnaryExpression(UnaryOperator.Negation, right, 0, 0);
                }
                else if (right.IsConstant(0))
                {
                    return left;
                }
            }
            else if (op == BinaryOperator.Multiply)
            {
                if (left.IsConstant(0))
                {
                    return ConstantExpression.create(0, 0, 0);
                }
                else if (right.IsConstant(0))
                {
                    return ConstantExpression.create(0, 0, 0);
                }
                else if (left.IsConstant(1))
                {
                    return right;
                }
                else if (right.IsConstant(1))
                {
                    return left;
                }
            }
            else if (op == BinaryOperator.Divide)
            {
                if (left.IsConstant(0))
                {
                    return ConstantExpression.create(0, 0, 0);
                }
                else if (right.IsConstant(0))
                {
                    return ConstantExpression.create(double.NaN, 0, 0);
                }
                else if (left.IsConstant(1))
                {
                }
                else if (right.IsConstant(1))
                {
                    return left;
                }
            }

            return root;
        }
    }
}
