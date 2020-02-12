using System;

namespace Cherimoya.Expressions
{
    public class CastExpression : Expression
    {
        internal CastExpression(Expression operand, string className, int fromPosition, int toPosition) :
            base(fromPosition, toPosition)
        {
            Operand = operand;
            ClassName = className;
        }

        public string ClassName { get; private set; }

        public Expression Operand { get; private set; }

        public Type TargetType { get; set; }


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