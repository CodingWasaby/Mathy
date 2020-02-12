using System.Linq;
using System.Reflection;

namespace Cherimoya.Expressions
{
    public class FunctionCallExpression : Expression
    {
        public FunctionCallExpression(string methodName, Expression[] parameters, int fromPosition, int toPosition) :
            base(fromPosition, toPosition)
        {
            MethodName = methodName;
            Parameters = parameters;
        }

        public string MethodName { get; private set; }

        public bool IsCustomFunc { get; set; }

        public MethodInfo Method { get; set; }

        public Expression[] Parameters { get; private set; }


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