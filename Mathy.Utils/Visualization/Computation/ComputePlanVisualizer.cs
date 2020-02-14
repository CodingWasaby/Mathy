using Mathy.Planning;
using Mathy.Visualization.Text;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Mathy.Visualization.Computation
{
    public class ComputePlanVisualizer
    {
        public static Bitmap Visualize(EvaluationContext context)
        {
            return Visualize(context, TreeStyle.CreateDefault());
        }

        public static Bitmap Visualize(EvaluationContext context, TreeStyle style)
        {
            ComputedNode root = CreateNodes(context);

            Bitmap bitmap = new Bitmap(1, 1);
            Graphics g = Graphics.FromImage(bitmap);
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
            root.Measure(g, style);

            int titleWidth = (int)(context.Plan.Title == null ? 0 : g.MeasureString(context.Plan.Title, style.TitleFont).Width);
            int titleHeight = (int)(context.Plan.Title == null ? 0 : g.MeasureString(context.Plan.Title, style.TitleFont).Height + style.TitleTopMargin + style.TitleBottomMargin);
            int imageWidth = (int)(Math.Max(titleWidth, root.Width) + style.LeftMargin + style.RightMargin);
            int imageHeight = (int)(root.Height + style.TopMargin + style.BottomMargin + titleHeight);
            root.Layout(style, style.LeftMargin + (imageWidth - style.LeftMargin - style.RightMargin - root.Width) / 2, style.TopMargin + titleHeight);
            root.PostLayout(style);
            g.Dispose();


            Bitmap renderedBitmap = new Bitmap(imageWidth, imageHeight);
            g = Graphics.FromImage(renderedBitmap);
            root.Draw(g, style);
            DrawTitle(g, style, context.Plan.Title, imageWidth);
            g.Dispose();

            return renderedBitmap;
        }

        private static void DrawTitle(Graphics g, TreeStyle style, string title, int imageWidth)
        {
            if (title != null)
            {
                SizeF size = g.MeasureString(title, style.TitleFont);
                g.DrawString(title, style.TitleFont, Brushes.Black, (float)((imageWidth - size.Width) / 2), style.TopMargin + style.TitleTopMargin);
            }
        }

        private static ComputedNode CreateNodes(EvaluationContext context)
        {
            ComputedNode root = null;

            List<ComputedNode> computedNodes = new List<ComputedNode>();

            foreach (Step step in context.Steps)
            {
                ComputedNode cn = new ComputedNode()
                {
                    Results = step.OutVariables.Select(i => context.GetValue(i)).ToArray(),
                    ExpressionImage = step.Image,
                    Description = step.SourceExpression.Title,
                    ResultVariableNames = step.OutVariables.ToList(),
                    Step = step
                };


                List<Branch> branches = new List<Branch>();

                foreach (SourceVariable variable in step.InSourceVariables)
                {
                    branches.Add(new Branch()
                    {
                        Node = new VariableNode()
                        {
                            Description = variable.Description,
                            Text = DecoratedText.Create(variable.Name),
                            Name = variable.Name,
                            Value = context.GetValue(variable.Name)
                        }
                    });
                }

                /*
                foreach (string variable in step.InTempVariables)
                {
                    branches.Add(new Branch() { Node = computedNodes.First(j => j.ResultVariableNames.Contains(variable)) });
                }
                 */


                foreach (Step fromStep in step.Froms)
                {
                    branches.Add(new Branch() { Node = computedNodes.First(j => j.Step == fromStep) });
                }


                List<Branch> distinctBranches = new List<Branch>();

                foreach (Branch branch in branches)
                {
                    if (!distinctBranches.Any(i => i.Node == branch.Node))
                    {
                        distinctBranches.Add(branch);
                        cn.InheritVariables(branch.Node);
                    }
                }

                cn.Branches = distinctBranches.ToList();


                computedNodes.Insert(0, cn);
                root = cn;
            }

            return root;
        }
    }
}
