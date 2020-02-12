using Cherimoya.Evaluation;

namespace Cherimoya.Expressions
{
    public class ExpressionSimplifier
    {
        public ExpressionSimplifier(ExpressionReductor reductor, ConstantExpressionChecker constantExpressionChecker, ExpressionEvaluator evaluator)
        {
            this.reductor = reductor;
            this.constantExpressionChecker = constantExpressionChecker;
            this.evaluator = evaluator;
        }


        private ExpressionReductor reductor;

        private ConstantExpressionChecker constantExpressionChecker;

        private ExpressionEvaluator evaluator;


        public Expression Simplify(Expression expression)
        {
            Expression s = expression;

            if (constantExpressionChecker.IsConstantExpression(s))
            {
                s = s.GetConstant(evaluator.Evaluate(s, null));
            }
            else if (reductor != null)
            {
                s = reductor.Reduce(s);
                if (constantExpressionChecker.IsConstantExpression(s))
                {
                    s = s.GetConstant(evaluator.Evaluate(s, null));
                }
            }


            if (s is BinaryExpression)
            {
                return SimplifyBinary((BinaryExpression)s);
            }
            else if (s is MethodCallExpression)
            {
                return SimplifyMethodCall((MethodCallExpression)s);
            }
            else if (s is LambdaExpression)
            {
                return SimplifyLambda((LambdaExpression)s);
            }


            return s;
        }


        private Expression SimplifyBinary(BinaryExpression expression)
        {
            Expression left = expression.Left;
            Expression right = expression.Right;

            if (expression.Operator == BinaryOperator.And)
            {
                if (ExpressionFeatures.ResolvesToConstant(left, true, evaluator))
                {
                    return Simplify(right);
                }
                else if (ExpressionFeatures.ResolvesToConstant(right, true, evaluator))
                {
                    return Simplify(left);
                }
            }

            return expression.GetCopy(Simplify(left), Simplify(right));
        }

        private Expression SimplifyMethodCall(MethodCallExpression expression)
        {
            Expression[] parameters = new Expression[expression.Parameters.Length];

            for (int i = 0; i <= parameters.Length - 1; i++)
            {
                parameters[i] = Simplify(expression.Parameters[i]);
            }

            return expression.GetCopy(expression.Operand == null ? null : Simplify(expression.Operand), parameters);
        }

        private Expression SimplifyLambda(LambdaExpression expression)
        {
            return expression.GetCopy(Simplify(expression.Body));
        }
    }
}