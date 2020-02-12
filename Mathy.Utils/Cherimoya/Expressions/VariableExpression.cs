namespace Cherimoya.Expressions
{
    public class VariableExpression : Expression
    {
        public VariableExpression(string variableName, int fromPosition, int toPosition) :
            base(fromPosition, toPosition)
        {
            VariableName = variableName;
        }

        public string VariableName { get; private set; }


        public override bool IsConstantExpression()
        {
            return false;
        }

        public override System.Collections.Generic.IEnumerable<Expression> GetChildren()
        {
            return new Expression[] { };
        }
    }
}