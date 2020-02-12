using Cherimoya.Evaluation;
using Mathy.Utils.Dandelion;
using System;

namespace Cherimoya.Expressions
{

    public class ExpressionFeatures
    {

        public static bool ResolvesToConstant(Expression expression, object value, ExpressionEvaluator evaluator)
        {
            return expression.IsConstantExpression() ? IsEquals(evaluator.Evaluate(expression, null), value) : false;
        }

        public static bool CompareTo(Expression expression, BinaryOperator op, double value, ExpressionEvaluator evaluator)
        {


            if (!expression.IsConstantExpression())
            {
                return false;
            }


            Object t = evaluator.Evaluate(expression, null);
            if (t == null || !Types.IsNumberType(t.GetType()))
            {
                return false;
            }


            double actual = Types.CoerceValue<double>(t);
            double expected = value;

            if (op == BinaryOperator.LessThan)
            {
                return actual < expected;
            }
            else if (op == BinaryOperator.LessEqualThan)
            {
                return actual <= expected;
            }
            else if (op == BinaryOperator.GreaterThan)
            {
                return actual > expected;
            }
            else if (op == BinaryOperator.GreaterEqualThan)
            {
                return actual >= expected;
            }
            else if (op == BinaryOperator.Equal)
            {
                return actual == expected;
            }
            else if (op == BinaryOperator.NotEqual)
            {
                return actual != expected;
            }


            throw new Exception();
        }


        private static bool IsEquals(Object t1, Object t2)
        {

            if (t1 == null || t2 == null)
            {
                return t1 == null && t2 == null;
            }
            else
            {
                return t1.Equals(Types.ConvertValue(t2, t1.GetType()));
            }
        }
    }
}
