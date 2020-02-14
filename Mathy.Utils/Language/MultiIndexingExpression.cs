using Cherimoya.Expressions;
using System.Collections.Generic;
using System.Linq;

namespace Mathy.Language
{
    public class MultiIndexingExpression : Expression
    {
        public MultiIndexingExpression(Expression operand, Expression[] indexers, int fromPosition, int toPosition) :
            base(fromPosition, toPosition)
        {
            Operand = operand;
            Indexers = indexers;
        }

        public Expression Operand { get; private set; }

        public Expression[] Indexers { get; private set; }


        public override bool IsConstantExpression()
        {
            return Operand.IsConstantExpression() && Indexers.All(i => i.IsConstantExpression());
        }

        public override IEnumerable<Expression> GetChildren()
        {
            yield return Operand;
            foreach (Expression indexer in Indexers)
            {
                yield return indexer;
            }
        }
    }
}
