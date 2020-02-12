using Cherimoya.Expressions;

namespace Cherimoya.Reduction.Rules
{
    class ReduceConstantExpression : ReductionRule
    {
        public override Expression Reduce(Expression root, ExpressionReductor reductor)
        {
            if (root.IsConstantExpression())
            {
                return root.GetConstant(reductor.LanguageService.CreateEvaluator().Evaluate(root, new VariableContext()));
            }
            else
            {
                return root;
            }
        }
    }
}
