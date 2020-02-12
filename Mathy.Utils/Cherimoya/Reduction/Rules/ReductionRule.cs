using Cherimoya.Expressions;

namespace Cherimoya.Reduction.Rules
{
    abstract class ReductionRule
    {
        public abstract Expression Reduce(Expression root, ExpressionReductor reductor);
    }
}
