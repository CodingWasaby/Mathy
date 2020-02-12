using Cherimoya.Expressions;

namespace Cherimoya.Evaluation
{
    public abstract class ExpressionEvaluator
    {
        public abstract object Evaluate(Expression expression, VariableContext context);
    }
}
