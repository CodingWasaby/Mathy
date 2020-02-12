using System;

namespace Cherimoya.Expressions
{
    public class CondensedArrayExpression : Expression
    {
        public CondensedArrayExpression(Type elementType, Expression from, Expression to, Expression by, int fromPosition, int toPosition) :
            base(fromPosition, toPosition)
        {
            ElementType = elementType;
            From = from;
            To = to;
            By = by;
        }

        public Type ElementType { get; private set; }

        public Expression From { get; private set; }

        public Expression To { get; private set; }

        public Expression By { get; private set; }

        /*
        internal override void PerformTypeChecking(TypeCheckingContext context)
        {
            From.PerformTypeChecking(context);
            To.PerformTypeChecking(context);
            By.PerformTypeChecking(context);

            if (Types.IsNumberType(ElementType) &&
                ReflectionSnippets.Accepts(ElementType, From.Type) &&
                ReflectionSnippets.Accepts(ElementType, To.Type) &&
                ReflectionSnippets.Accepts(ElementType, By.Type)
                )
            {
                Type = ElementType.MakeArrayType(0);
            }
            else
            {
                context.ErrorProvider.ThrowException(string.Format("{0} ... {1} by {2} is not supported.", From.Type, To.Type, By.Type), this);
            }
        }
        */

        public override bool IsConstantExpression()
        {
            return From.IsConstantExpression() && To.IsConstantExpression() && By.IsConstantExpression();
        }

        public override System.Collections.Generic.IEnumerable<Expression> GetChildren()
        {
            yield return From;
            yield return To;
            yield return By;
        }
    }
}