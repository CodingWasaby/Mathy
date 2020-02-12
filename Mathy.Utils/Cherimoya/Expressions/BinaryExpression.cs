namespace Cherimoya.Expressions
{

    public class BinaryExpression : Expression
    {

        internal BinaryExpression(BinaryOperator op, Expression left, Expression right, int fromPosition, int toPosition) :
            base(fromPosition, toPosition)
        {
            Operator = op;
            Left = left;
            Right = right;
        }


        public BinaryOperator Operator { get; private set; }

        public Expression Left { get; private set; }

        public Expression Right { get; private set; }


        public static BinaryExpression Create(BinaryOperator op, Expression left, Expression right)
        {

            BinaryExpression ins = new BinaryExpression(op, left, right, left.FromPosition, right.ToPosition);
            //	ins.CheckType(left.Type, right.Type, null);

            return ins;
        }

        public BinaryExpression GetCopy(Expression left, Expression right)
        {
            BinaryExpression result = new BinaryExpression(Operator, left, right, FromPosition, ToPosition);
            result.Type = Type;
            return result;
        }


        public override bool IsConstantExpression()
        {
            return Left.IsConstantExpression() && Right.IsConstantExpression();
        }

        public override System.Collections.Generic.IEnumerable<Expression> GetChildren()
        {
            yield return Left;
            yield return Right;
        }
    }
}