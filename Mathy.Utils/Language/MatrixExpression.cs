using Cherimoya.Expressions;
using System.Collections.Generic;
using System.Linq;

namespace Mathy.Language
{
    class MatrixExpression : Expression
    {
        public MatrixExpression(Expression[][] rows, int fromPosition, int toPosition) :
            base(fromPosition, toPosition)
        {
            Rows = rows;
            RowCount = Rows.Length;
            ColumnCount = Rows[0].Length;
        }

        public Expression[][] Rows { get; private set; }

        public int RowCount { get; private set; }

        public int ColumnCount { get; private set; }

        public override bool IsConstantExpression()
        {
            return Rows.All(i => i.All(j => j.IsConstantExpression()));
        }

        public override IEnumerable<Expression> GetChildren()
        {
            foreach (Expression[] row in Rows)
            {
                foreach (Expression expression in row)
                {
                    yield return expression;
                }
            }
        }
    }
}