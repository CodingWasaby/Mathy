namespace Cherimoya.Expressions
{
    public class IndexingExpression : Expression
    {
        public IndexingExpression(Expression operand, Expression indexer, int fromPosition, int toPosition) :
            base(fromPosition, toPosition)
        {
            Operand = operand;
            Indexer = indexer;
        }

        public Expression Operand { get; private set; }

        public Expression Indexer { get; private set; }


        public override bool IsConstantExpression()
        {
            return Operand.IsConstantExpression() && Indexer.IsConstantExpression();
        }

        public override System.Collections.Generic.IEnumerable<Expression> GetChildren()
        {
            yield return Operand;
            yield return Indexer;
        }
    }
}
