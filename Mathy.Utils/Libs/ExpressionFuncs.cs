using Cherimoya.Diff;
using Cherimoya.Expressions;
using Mathy.Utils.Dandelion;
using Mathy.Language;
using System.Collections.Generic;

namespace Mathy.Libs
{
    public class ExpressionFuncs
    {
        public static VariableContextExpression pdiff(VariableContextExpression expression, string variableName)
        {
            return diff(expression, variableName);
        }

        public static VariableContextExpression diff(VariableContextExpression expression, string variableName)
        {
            VariableContext c = expression.VariableContext;
            Expression diff = new Differ(expression.Expression, variableName).Calculate(c);

            TypeCheckingContext context = new MathyLanguageService().CreateTypeCheckingContext(c);

            foreach (VariableInfo variable in expression.Variables)
            {
                c.Set(variable.Name, context.CreateAutoCreatedVariableValue(variable.Type));
            }

            VariableContextExpression result = new VariableContextExpression(diff, expression.Variables, 0, 0) { VariableContext = expression.VariableContext };
            new MathyLanguageService().CreateTypeChecker().PerformTypeChecking(result, context);

            result = new VariableContextExpression(new Cherimoya.Reduction.ExpressionReductor(new MathyLanguageService()).Reduce(diff), expression.Variables, 0, 0) { VariableContext = expression.VariableContext };
            new MathyLanguageService().CreateTypeChecker().PerformTypeChecking(result, context);

            foreach (VariableInfo variable in expression.Variables)
            {
                context.VariableContext.Remove(variable.Name);
            }


            return result;
        }

        public static object eval(Expression expression, VariableContext vc)
        {
            return eval(expression, new Dictionary<string, object>(), vc);
        }

        public static object eval(Expression expression, Dictionary<string, object> variables, VariableContext vc)
        {
            foreach (string variableName in variables.Keys)
            {
                vc.Set(variableName, variables[variableName]);
            }

            object result = new MathyLanguageService().CreateEvaluator().Evaluate(expression is VariableContextExpression ? (expression as VariableContextExpression).Expression : expression, vc);

            foreach (string variableName in variables.Keys)
            {
                vc.Remove(variableName);
            }


            return result;
        }

        public static double evald(Expression expression, VariableContext vc)
        {
            return Types.ConvertValue<double>(eval(expression, new Dictionary<string, object>(), vc));
        }

        public static double evald(Expression expression, Dictionary<string, object> variables, VariableContext vc)
        {
            return Types.ConvertValue<double>(eval(expression, variables, vc));
        }
    }
}
