using Cherimoya.Expressions;

namespace Cherimoya.Evaluation
{
    public abstract class TypeChecker
    {
        public abstract void PerformTypeChecking(Expression expression, TypeCheckingContext context);
    }
}
