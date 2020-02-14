using Cherimoya.Expressions;
using Mathy.Language;
using Mathy.Visualization.Expressions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Mathy.Planning
{
    class PlanAnalyzer
    {
        public PlanAnalyzer(Plan plan)
        {
            this.plan = plan;
        }


        private Plan plan;


        public EvaluationContext Analyze()
        {
            VariableContext vc = CreateVariebleContext();


            MathyLanguageService service = new MathyLanguageService();

            List<Step> steps = new List<Step>();

            List<string> existingInVariables = new List<string>();


            int index = 0;

            foreach (SourceExpression expression in plan.Expressions)
            {
                Step step = new Step();
                step.SourceExpression = expression;

                try
                {
                    step.Expressions = service.Compile(expression.Expression, vc);
                }
                catch (CompileException ex)
                {
                    throw new Exception(string.Format("步骤{0}表达式编译错误：\r\n{1}", index + 1, ex.Message));
                }

                if (string.IsNullOrEmpty(expression.Condition))
                {
                    step.Conditions = new Expression[] { };
                }
                else
                {
                    try
                    {
                        step.Conditions = service.Compile(expression.Condition, vc);
                    }
                    catch (CompileException ex)
                    {
                        throw new Exception(string.Format("步骤{0}条件编译错误：\r\n{1}", index + 1, ex.Message));
                    }
                }

                Bitmap expressionImage = new NodeVisualizer(
                   step.Expressions
                   .Select(i => new NodeConverter().Convert(i))
                   .ToArray()).VisulizeAsBitmap();

                Bitmap conditionImage = step.Conditions.Length == 0 ? null :
                    new NodeVisualizer(
                     step.Conditions
                     .Select(i => new NodeConverter().Convert(i))
                     .ToArray()).VisulizeAsBitmap();

                step.Image = StackImages(expressionImage, conditionImage);
                step.ImageData = Funcs.ImageToBytes(step.Image);
                step.OutVariables = CollectOutVariables(step.Expressions).ToArray();


                string[] inVariables = CollectVariables(step.Expressions)
                    .Where(i => !step.OutVariables.Contains(i))
                    .ToArray();

                string[] conditionVariables = CollectVariables(step.Conditions);

                step.InSourceVariables = plan.Variables.Where(i => inVariables.Contains(i.Name) && !existingInVariables.Contains(i.Name)).ToArray();
                step.InValues = new object[step.InSourceVariables.Length];
                step.DependentVariables = plan.Variables.Where(i => inVariables.Contains(i.Name)).ToArray();
                step.DependentValues = new object[step.DependentVariables.Length];
                step.InTempVariables = inVariables.Where(i => plan.Variables.All(j => j.Name != i)).ToArray();
                step.ConditionVariables = conditionVariables;

                existingInVariables.AddRange(step.InSourceVariables.Select(i => i.Name));

                steps.Add(step);


                index++;
            }

            foreach (Step step in steps)
            {
                List<Step> froms = new List<Step>();
                foreach (string tempVariable in step.InTempVariables)
                {
                    froms.AddRange(steps.Where(i => i.OutVariables.Contains(tempVariable)));
                }

                foreach (string tempVariable in step.ConditionVariables)
                {
                    froms.AddRange(steps.Where(i => i.OutVariables.Contains(tempVariable)));
                }


                step.Froms = froms.Where(i => i != null).Distinct().ToArray();
            }


            EvaluationContext context = new EvaluationContext(plan, steps.ToArray());

            foreach (SourceVariable variable in plan.Variables)
            {
                context.SetValue(variable.Name, null);
            }


            return context;
        }

        private Bitmap StackImages(Bitmap expressionImage, Bitmap conditionImage)
        {
            if (conditionImage == null)
            {
                return expressionImage;
            }

            Bitmap image = new Bitmap(Math.Max(expressionImage.Width, conditionImage.Width), expressionImage.Height + conditionImage.Height + 20);
            Graphics g = Graphics.FromImage(image);
            g.DrawImage(conditionImage, 0, 0);
            Pen p = new Pen(Brushes.LightGray);
            g.DrawLine(p, 0, 10 + conditionImage.Height, image.Width, 10 + conditionImage.Height);
            p.Dispose();
            g.DrawImage(expressionImage, 0, 20 + conditionImage.Height);

            return image;
        }

        private VariableContext CreateVariebleContext()
        {
            VariableContext vc = MathyLanguageService.CreateVariableContext();

            foreach (SourceVariable variable in plan.Variables)
            {
                vc.Set(variable.Name, DataFuncs.CreateDefaultValue(variable.Type));
            }


            return vc;
        }

        private static string[] CollectVariables(Expression[] expressions)
        {
            List<Expression> items = new List<Expression>();
            foreach (Expression expression in expressions)
            {
                items.AddRange(expression.Flatten());
            }


            List<string> inVariables = new List<string>();

            foreach (Expression expression in items)
            {
                inVariables.AddRange(items.Where(i => i is VariableExpression).Cast<VariableExpression>().Select(i => i.VariableName));
            }

            inVariables = inVariables.Where(i => i != null).Distinct().ToList();


            foreach (Expression expression in items)
            {
                if (expression is IterationSumExpression)
                {
                    foreach (string variableName in (expression as IterationSumExpression).Variables.Select(i => i.Name))
                    {
                        inVariables.Remove(variableName);
                    }
                }
                else if (expression is VariableContextExpression)
                {
                    foreach (string variableName in (expression as VariableContextExpression).Variables.Select(i => i.Name))
                    {
                        inVariables.Remove(variableName);
                    }
                }
                else if (expression is LambdaExpression)
                {
                    foreach (string variableName in (expression as LambdaExpression).VariableNames)
                    {
                        inVariables.Remove(variableName);
                    }
                }
            }


            return inVariables.ToArray();
        }

        private static IEnumerable<string> CollectOutVariables(Expression[] expressions)
        {
            foreach (Expression expression in expressions
                .Where(i => i is BinaryExpression && (i as BinaryExpression).Operator == BinaryOperator.Assign))
            {
                BinaryExpression binary = expression as BinaryExpression;

                if (binary.Left is VariableExpression)
                {
                    yield return (binary.Left as VariableExpression).VariableName;
                }
                else if (binary.Left is MultipleVariableExpression)
                {
                    foreach (string variable in (binary.Left as MultipleVariableExpression).Variables)
                    {
                        yield return variable;
                    }
                }
            }
        }
    }
}
