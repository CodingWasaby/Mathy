namespace Cherimoya.Expressions
{
    public class ConstantExpressionChecker
    {
        public bool IsConstantExpression(Expression expression)
        {
            return expression.IsConstantExpression();
        }
    }
}