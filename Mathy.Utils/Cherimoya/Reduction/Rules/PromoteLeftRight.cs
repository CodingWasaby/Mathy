using Cherimoya.Expressions;

namespace Cherimoya.Reduction.Rules
{
    class PromoteLeftRight : ReductionRule
    {
        public override Expression Reduce(Expression root, ExpressionReductor reductor)
        {
            if (root is BinaryExpression)
            {
                Expression left = reductor.Reduce((root as BinaryExpression).Left);
                Expression right = reductor.Reduce((root as BinaryExpression).Right);
                if (left is BinaryExpression && ExpressionSnippets.GetOperatorLevel((left as BinaryExpression).Operator) == ExpressionSnippets.GetOperatorLevel((root as BinaryExpression).Operator))
                {
                    Expression ll = reductor.Reduce((left as BinaryExpression).Left);
                    Expression lr = reductor.Reduce((left as BinaryExpression).Right);

                    if (lr.IsConstantExpression())
                    {
                        return BinaryExpression.Create((left as BinaryExpression).Operator, ll, BinaryExpression.Create((root as BinaryExpression).Operator, lr, right));
                    }
                }
            }


            return root;
        }
    }
}
