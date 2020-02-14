using Cherimoya.Expressions;
using Mathy.Language;
using Mathy.Tree;
using Mathy.Visualization.Expressions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Mathy.Visualization.Tree
{
    class RenderTreeConverter
    {
        public static RenderBranch Convert(ExpressionTree expressionTree, Dictionary<string, object> variables, Graphics g, Font font)
        {
            VariableContext c = Mathy.Funcs.CreateVariableContext(variables);

            return Create(expressionTree.X, expressionTree.Y, expressionTree.Angle, expressionTree.PrimaryBranch, c, g, font);
        }

        private static RenderBranch Create(double x, double y, int angle, TreeBranch branch, VariableContext variables, Graphics g, Font font)
        {
            double x2 = x - branch.Position * Math.Cos(angle * Math.PI / 180);
            double y2 = y + branch.Position * Math.Sin(angle * Math.PI / 180);

            double x1 = x2 + branch.Length * Math.Cos((angle + branch.Angle) * Math.PI / 180);
            double y1 = y2 - branch.Length * Math.Sin((angle + branch.Angle) * Math.PI / 180);

            double a = Funcs.Atan(x1, y1, x2, y2);

            x2 = x1 + (branch.Length - 2) * Math.Cos(a);
            y2 = y1 - (branch.Length - 2) * Math.Sin(a);

            RenderBranch[] branches = branch.Branches.Select(i => Create(x2, y2, angle + branch.Angle - 180, i, variables, g, font)).ToArray();


            string description;

            double imageX = 0;
            double imageY = 0;
            Bitmap expressionImage = null;

            if (branch is ExpressionBranch)
            {
                string[] preVariables = variables.GetAllVariables();

                Expression[] expressions = new MathyLanguageService().Compile((branch as ExpressionBranch).Expression, variables);
                foreach (Expression expression in expressions)
                {
                    new MathyLanguageService().CreateEvaluator().Evaluate(expression, variables);
                }


                StringBuilder b = new StringBuilder();

                foreach (string variableName in variables.GetAllVariables().Where(i => !preVariables.Contains(i)))
                {
                    b.AppendFormat("{0}={1}", variableName, variables.GetValue(variableName));
                }


                description = branch.Description + "\r\n" + b.ToString();


                double xt = x2 + (branch as ExpressionBranch).ImageX * Math.Cos(a + Math.PI);
                double yt = y2 - (branch as ExpressionBranch).ImageX * Math.Sin(a + Math.PI);

                double d = (branch as ExpressionBranch).ImageY;

                imageX = xt + d * Math.Cos(a + Math.PI / 2);
                imageY = yt - d * Math.Sin(a + Math.PI / 2);

                expressionImage = new NodeVisualizer(expressions.Select(i => new NodeConverter().Convert(i)).ToArray()).VisulizeAsBitmap();
            }
            else
            {
                string variableName = (branch as VariableBranch).VariableName;
                object value = variables.GetValue(variableName);
                description = string.Format("{0}\r\n{1}={2}", branch.Description, variableName, value);
            }


            double dx;
            double dy;
            EvaluateDescriptionPosition(branch, description, x1, y1, x2, y2, g, font, out dx, out dy);


            return new RenderBranch()
            {
                X1 = x1,
                Y1 = y1,
                X2 = x2,
                Y2 = y2,
                IsVariable = branch is VariableBranch,
                ImageX = imageX,
                ImageY = imageY,
                Image = expressionImage,
                Branches = branches,
                DescriptionX = dx,
                DescriptionY = dy,
                Description = description
            };
        }

        private static void EvaluateDescriptionPosition(TreeBranch branch, string description, double x1, double y1, double x2, double y2, Graphics g, Font font, out double dx, out double dy)
        {
            double angle = Funcs.Atan(x1, y1, x2, y2);

            if (branch is ExpressionBranch)
            {
                ExpressionBranch e = branch as ExpressionBranch;

                double xt = x2 + e.DescriptionX * Math.Cos(angle + Math.PI);
                double yt = y2 - e.DescriptionX * Math.Sin(angle + Math.PI);

                double d = (branch as ExpressionBranch).DescriptionY;

                dx = xt + d * Math.Cos(angle + Math.PI / 2) + (branch as ExpressionBranch).DescriptionOffsetX;
                dy = yt - d * Math.Sin(angle + Math.PI / 2) + (branch as ExpressionBranch).DescriptionOffsetY;
            }
            else
            {
                dy = y1 - g.MeasureString(description, font).Height / 2;

                if (x2 < x1)
                {
                    dx = x1 + 5;
                }
                else
                {
                    dx = x1 - g.MeasureString(description, font).Width - 5;
                }
            }
        }
    }
}
