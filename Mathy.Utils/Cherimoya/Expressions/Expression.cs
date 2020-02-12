using System;
using System.Collections.Generic;

namespace Cherimoya.Expressions
{
    public abstract class Expression
    {
        protected Expression(int fromPosition, int toPosition)
        {
            FromPosition = fromPosition;
            ToPosition = toPosition - 1;
        }

        internal int FromPosition { get; private set; }

        internal int ToPosition { get; private set; }

        public Type Type { get; set; }


        public ConstantExpression GetConstant(object value)
        {
            ConstantExpression result = new ConstantExpression(value, FromPosition, ToPosition);
            result.Type = Type;
            return result;
        }

        public Expression[] Flatten()
        {
            List<Expression> items = new List<Expression>();

            List<Expression> queue = new List<Expression>();
            queue.Add(this);

            while (queue.Count > 0)
            {
                Expression current = queue[0];
                items.Add(current);

                queue.RemoveAt(0);
                queue.AddRange(current.GetChildren());
            }

            return items.ToArray();
        }


        public abstract bool IsConstantExpression();

        public abstract IEnumerable<Expression> GetChildren();
    }
}