using Mathy.Language;

namespace Mathy.Graphing
{
    public class ExpressionDataSource : ContinousDataSource
    {
        public VariableContextExpression Expression { get; set; }


        protected override double Map(double x)
        {
            Expression.VariableContext.Set(Expression.Variables[0].Name, x);
            return (double)Expression.Evaluator.Evaluate(Expression.Expression, Expression.VariableContext);
        }
    }
}
