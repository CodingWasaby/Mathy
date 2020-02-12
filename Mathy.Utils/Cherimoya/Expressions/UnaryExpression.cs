namespace Cherimoya.Expressions
{
    public class UnaryExpression : Expression
    {
        public UnaryExpression(UnaryOperator op, Expression operand, int fromPosition, int toPosition) :
            base(fromPosition, toPosition)
        {

            Operator = op;
            Operand = operand;
        }

        public UnaryOperator Operator { get; private set; }

        public Expression Operand { get; private set; }


        public override bool IsConstantExpression()
        {
            return Operand.IsConstantExpression();
        }

        public override System.Collections.Generic.IEnumerable<Expression> GetChildren()
        {
            yield return Operand;
        }
    }
}