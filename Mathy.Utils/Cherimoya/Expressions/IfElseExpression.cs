namespace Cherimoya.Expressions
{
    public class IfElseExpression : Expression
    {
        internal IfElseExpression(Expression condition, Expression positiveBranch, Expression negativeBranch, int fromPosition, int toPosition) :
            base(fromPosition, toPosition)
        {
            Condition = condition;
            PositiveBranch = positiveBranch;
            NegativeBranch = negativeBranch;
        }


        public Expression Condition { get; private set; }

        public Expression PositiveBranch { get; private set; }

        public Expression NegativeBranch { get; private set; }


        public override bool IsConstantExpression()
        {
            return Condition.IsConstantExpression() && PositiveBranch.IsConstantExpression() && NegativeBranch.IsConstantExpression();
        }

        public override System.Collections.Generic.IEnumerable<Expression> GetChildren()
        {
            yield return Condition;
            yield return PositiveBranch;
            yield return NegativeBranch;
        }
    }
}