using Cherimoya.Expressions;

namespace Cherimoya.Reduction.Rules
{
    class ReduceFunctionCallExpression : ReductionRule
    {
        public override Expression Reduce(Expression root, ExpressionReductor reductor)
        {
            if (!(root is FunctionCallExpression))
            {
                return root;
            }


            FunctionCallExpression func = root as FunctionCallExpression;

            if (func.MethodName == "pow")
            {
                if (func.Parameters[1].IsConstant(1))
                {
                    return func.Parameters[0];
                }
                else if (func.Parameters[1].IsConstant(0))
                {
                    return ConstantExpression.create(1, 0, 0);
                }
            }



            return root;
        }
    }
}
