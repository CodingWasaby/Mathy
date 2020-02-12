using System;
using System.Linq;
using System.Reflection;

namespace Cherimoya.Expressions
{
    public class CreateObjectExpression : Expression
    {
        public CreateObjectExpression(Expression[] parameters, Type objectType, int fromPosition, int toPosition) :
            base(fromPosition, toPosition)
        {

            Parameters = parameters;
            ObjectType = objectType;
        }


        public Expression[] Parameters { get; private set; }

        public Type ObjectType { get; private set; }

        public ConstructorInfo Constructor { get; set; }


        public override bool IsConstantExpression()
        {
            return Parameters.All(i => i.IsConstantExpression());
        }

        public override System.Collections.Generic.IEnumerable<Expression> GetChildren()
        {
            return Parameters;
        }
    }
}