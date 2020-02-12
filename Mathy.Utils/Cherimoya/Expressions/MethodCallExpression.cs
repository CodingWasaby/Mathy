using System;
using System.Linq;
using System.Reflection;

namespace Cherimoya.Expressions
{
    public class MethodCallExpression : Expression
    {
        internal MethodCallExpression(Expression operand, string methodName, Expression[] parameters, int fromPosition, int toPosition) :

            base(fromPosition, toPosition)
        {

            MethodName = methodName;
            Operand = operand;
            Parameters = parameters;
        }

        internal MethodCallExpression(Type ownerType, string methodName, Expression[] parameters, int fromPosition, int toPosition) :

            base(fromPosition, toPosition)
        {

            OwnerType = ownerType;
            MethodName = methodName;
            Parameters = parameters;
            IsStatic = true;
        }


        internal Type OwnerType { get; private set; }

        internal string MethodName { get; private set; }

        public Expression Operand { get; private set; }

        public MethodInfo Method { get; set; }

        public Expression[] Parameters { get; private set; }

        public bool IsExtension { get; set; }

        public bool IsStatic { get; private set; }


        private MethodCallExpression(int fromPosition, int toPosition) :
            base(fromPosition, toPosition)
        {
        }

        public MethodCallExpression GetCopy(Expression operand, Expression[] parameters)
        {
            MethodCallExpression result = new MethodCallExpression(FromPosition, ToPosition);

            result.IsExtension = IsExtension;
            result.IsStatic = IsStatic;
            result.Method = Method;
            result.MethodName = MethodName;
            result.Operand = operand;
            result.OwnerType = OwnerType;
            result.Parameters = Parameters;
            result.Type = Type;

            return result;
        }


        public override bool IsConstantExpression()
        {
            if (Operand != null && !Operand.IsConstantExpression())
            {
                return false;
            }


            return Parameters.All(i => i.IsConstantExpression());
        }

        public override System.Collections.Generic.IEnumerable<Expression> GetChildren()
        {
            return new Expression[] { Operand }.Concat(Parameters);
        }
    }
}