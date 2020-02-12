namespace Cherimoya.Expressions
{
    public class ConstantExpression : Expression
    {
        internal ConstantExpression(object value, int fromPosition, int toPosition) :
            base(fromPosition, toPosition)
        {
            Value = value;
        }

        public static ConstantExpression create(object value, int fromPosition, int toPosition)
        {
            ConstantExpression ins = new ConstantExpression(value, fromPosition, toPosition);
            // ins.PerformTypeChecking(null);

            return ins;
        }


        public object Value { get; private set; }


        public override bool IsConstantExpression()
        {
            return true;
        }


        public override System.Collections.Generic.IEnumerable<Expression> GetChildren()
        {
            return new Expression[] { };
        }
    }
}
