using Cherimoya.Expressions;
using Mathy.Language;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Mathy.Planning
{
    public class Step
    {
        public SourceExpression SourceExpression { get; internal set; }

        public Bitmap Image { get; internal set; }

        public byte[] ImageData { get; internal set; }

        public Expression[] Conditions { get; internal set; }

        public Expression[] Expressions { get; internal set; }

        public string[] ConditionVariables { get; internal set; }

        public SourceVariable[] InSourceVariables { get; internal set; }

        public string[] InTempVariables { get; internal set; }

        public string[] OutVariables { get; internal set; }

        public Step To { get; internal set; }

        public Step[] Froms { get; internal set; }

        public StepState State { get; internal set; }

        public object[] InValues { get; set; }

        public SourceVariable[] DependentVariables { get; internal set; }

        public object[] DependentValues { get; set; }

        public EvaluationContext Evaluation { get; set; }

        public void Evaluate(VariableContext vc)
        {
            foreach (Expression expression in Expressions)
            {
                new MathyLanguageService().CreateEvaluator().Evaluate(expression, vc);
            }
        }

        public bool EvaluateConditions(VariableContext vc)
        {
            List<bool> conditions = new List<bool>();

            int index = 0;
            foreach (Expression expression in Conditions)
            {
                object result = new MathyLanguageService().CreateEvaluator().Evaluate(expression, vc);
                if (!(result is bool))
                {
                    throw new Exception(string.Format("Condition {0} does not evaluate to boolean.", index));
                }
                else
                {
                    conditions.Add((bool)result);
                }
                index++;
            }


            return conditions.All(i => i);
        }
    }
}
