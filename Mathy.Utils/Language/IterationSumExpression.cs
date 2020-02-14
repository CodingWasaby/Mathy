using Cherimoya.Expressions;
using System.Collections.Generic;
using System.Linq;

namespace Mathy.Language
{
    public class IterationSumVariable
    {
        public Expression From { get; set; }

        public Expression To { get; set; }

        public string Name { get; set; }
    }

    public class IterationSumExpression : Expression
    {
        public IterationSumExpression(IterationSumVariable[] variables, Expression body, int fromPosition, int toPosition)
            : base(fromPosition, toPosition)
        {
            Variables = variables;
            Body = body;
        }

        public IterationSumVariable[] Variables { get; private set; }

        public Expression Body { get; private set; }

        public override bool IsConstantExpression()
        {
            return Variables.All(i => i.From.IsConstantExpression() && i.To.IsConstantExpression()) && Body.IsConstantExpression();
        }

        public override IEnumerable<Expression> GetChildren()
        {
            yield return Body;
            foreach (IterationSumVariable variable in Variables)
            {
                yield return variable.From;
                yield return variable.To;
            }
        }
    }
}