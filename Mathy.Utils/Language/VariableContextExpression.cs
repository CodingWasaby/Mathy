using Cherimoya.Evaluation;
using Cherimoya.Expressions;
using System;
using System.Collections.Generic;

namespace Mathy.Language
{
    public class VariableInfo
    {
        public string Name { get; set; }

        public Type Type { get; set; }
    }

    public class VariableContextExpression : Expression
    {
        public VariableContextExpression(Expression expression, VariableInfo[] variables, int fromPosition, int toPosition) :
            base(fromPosition, toPosition)
        {
            Expression = expression;
            Variables = variables;
        }


        public Expression Expression { get; private set; }

        public VariableInfo[] Variables { get; private set; }

        public VariableContext VariableContext { get; set; }

        public ExpressionEvaluator Evaluator { get; set; }


        public override bool IsConstantExpression()
        {
            return Expression.IsConstantExpression();
        }

        public override IEnumerable<Expression> GetChildren()
        {
            yield return Expression;
        }
    }
}
