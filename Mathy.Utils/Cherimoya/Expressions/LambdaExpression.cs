using System;

namespace Cherimoya.Expressions
{
    public class LambdaExpression : Expression
    {
        public LambdaExpression(string[] variableNames, Expression body, int level, int fromPosition, int toPosition) :
            base(fromPosition, toPosition)
        {
            VariableNames = variableNames;
            Body = body;
            Level = level;
        }

        public int Level { get; private set; }

        public Type[] VariableTypes { get; set; }

        public string[] VariableNames { get; private set; }

        public Expression Body { get; private set; }


        public LambdaExpression GetCopy(Expression body)
        {
            LambdaExpression result = new LambdaExpression(VariableNames, body, Level, FromPosition, ToPosition);
            result.VariableTypes = VariableTypes;
            result.Type = Type;


            return result;
        }


        public override bool IsConstantExpression()
        {
            return false;
        }

        public override System.Collections.Generic.IEnumerable<Expression> GetChildren()
        {
            yield return Body;
        }
    }
}