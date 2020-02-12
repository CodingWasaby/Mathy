using System;

namespace Cherimoya.Expressions
{
    public class CreateArrayExpression : Expression
    {
        public CreateArrayExpression(Expression[] elements, Type elementType, int fromPosition, int toPosition) :
            base(fromPosition, toPosition)
        {
            Elements = elements;
            ElementType = elementType;
        }

        public Expression[] Elements { get; private set; }

        public Type ElementType { get; private set; }


        public override bool IsConstantExpression()
        {

            foreach (Expression element in Elements)
            {
                if (!element.IsConstantExpression())
                {
                    return false;
                }
            }


            return true;
        }

        public override System.Collections.Generic.IEnumerable<Expression> GetChildren()
        {
            return Elements;
        }
    }
}