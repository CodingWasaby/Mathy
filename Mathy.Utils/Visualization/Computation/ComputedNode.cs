using Mathy.Planning;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;

namespace Mathy.Visualization.Computation
{
    class ComputedNode : Node
    {
        public Bitmap ExpressionImage { get; set; }

        public Step Step { get; set; }

        public List<string> ResultVariableNames { get; set; }

        public object[] Results { get; set; }

        public List<Branch> Branches { get; set; }


        private double branchesWidth;

        private double branchesHeight;


        public override void Measure(Graphics g, TreeStyle style)
        {
            foreach (Branch branch in Branches)
            {
                branch.Node.Measure(g, style);
            }


            if (Branches.Count > 0)
            {
                branchesWidth = Branches.Sum(i => i.Node.Width) + (Branches.Count - 1) * style.HorizontalNodeSpacing;
                branchesHeight = Branches.Max(i => i.Node.Height);
            }


            List<SizeF> lines = new List<SizeF>();
            lines.Add(g.MeasureString(Description, style.Font));
            lines.Add(new SizeF(ExpressionImage.Width, ExpressionImage.Height));


            int index = 0;

            foreach (object result in Results)
            {
                string s = GetValueString(result);
                if (Results.Length > 1)
                {
                    s = ResultVariableNames[index] + "=" + s;
                }

                lines.Add(g.MeasureString(GetValueString(s), style.ValueFont));
                index++;
            }


            NodeWidth = lines.Max(i => i.Width);
            NodeHeight = lines.Sum(i => i.Height);

            Width = Math.Max(branchesWidth, NodeWidth);
            Height = Branches.Count == 0 ? NodeHeight : branchesHeight + style.VerticalNodeSpacing + NodeHeight;
        }


        private int layoutTimes;

        public override void Layout(TreeStyle style, double left, double top)
        {
            layoutTimes++;

            if (layoutTimes == 1)
            {
                Left = left;
                Top = top;
            }
            else
            {
                double[] rect = MergeRects(Left, Top, Width, Height, left, top, Width, Height);
                Left = rect[0];
                Top = rect[1];
                Width = rect[2];
                Height = rect[3];
            }

            NodeLeft = Left + (Width - NodeWidth) / 2;
            NodeTop = Top;

            double x = Left + (Width - branchesWidth) / 2;
            double y = Top + NodeHeight + style.VerticalNodeSpacing;

            foreach (Branch branch in Branches)
            {
                branch.Node.Layout(style, x, y);
                x += branch.Node.Width + style.HorizontalNodeSpacing;
            }
        }

        public override void PostLayout(TreeStyle style)
        {
            foreach (Branch branch in Branches)
            {
                branch.Node.PostLayout(style);
                ComputeBranchLine(branch, style);
            }
        }

        private double[] MergeRects(double l, double t, double w, double h, double l1, double t1, double w1, double h1)
        {
            double left = Math.Min(l, l1);
            double top = Math.Min(t, t1);
            double right = Math.Max(l + w, l1 + w1);
            double bottom = Math.Max(t + h, t1 + h1);

            return new double[] { left, top, right - left, bottom - top };
        }

        private void ComputeBranchLine(Branch branch, TreeStyle style)
        {
            double x1 = branch.Node.NodeLeft + branch.Node.NodeWidth / 2;
            double y1 = branch.Node.NodeTop;
            double x2 = NodeLeft + NodeWidth / 2;
            double y2 = NodeTop + NodeHeight / 2;
            double angle = Funcs.Atan(x1, y1, x2, y2);


            branch.LineX1 = x1;
            branch.LineY1 = y1 - style.JointSpacing;


            double interX = x1 - (NodeTop + NodeHeight - y1) / Math.Tan(angle);

            if (interX >= NodeLeft && interX <= NodeLeft + NodeWidth)
            {
                double d = (NodeHeight / 2 + style.JointSpacing) / Math.Abs(Math.Sin(angle));

                branch.LineX2 = x2 - d * Math.Cos(angle);
                branch.LineY2 = y2 + d * Math.Sin(angle);
            }
            else
            {
                double d = (NodeWidth / 2 + style.JointSpacing) / Math.Abs(Math.Cos(angle));

                branch.LineX2 = x2 - d * Math.Cos(angle);
                branch.LineY2 = y2 + d * Math.Sin(angle);
            }


            if (Math.Abs(branch.LineX1 - branch.LineX2) <= 2 && Math.Abs(branch.LineY1 - branch.LineY2) >= 20)
            {
                double x = (branch.LineX1 + branch.LineX2) / 2;
                branch.LineX1 = x;
                branch.LineX2 = x;
            }


            ComputeArrowPath(branch, angle);
        }

        private void ComputeArrowPath(Branch branch, double angle)
        {
            double d = 5;
            double l = 10;

            double px = branch.LineX2 - l * Math.Cos(angle);
            double py = branch.LineY2 + l * Math.Sin(angle);

            GraphicsPath arrowPath = new GraphicsPath();
            arrowPath.AddLine(
                (float)branch.LineX2,
                (float)branch.LineY2,
                (float)(px + d * Math.Cos(angle - Math.PI / 2)),
                (float)(py - d * Math.Sin(angle - Math.PI / 2)));
            arrowPath.AddLine(
                (float)(px + d * Math.Cos(angle - Math.PI / 2)),
                (float)(py - d * Math.Sin(angle - Math.PI / 2)),
                (float)(px + d * Math.Cos(angle + Math.PI / 2)),
                (float)(py - d * Math.Sin(angle + Math.PI / 2)));
            arrowPath.AddLine(
                (float)(px + d * Math.Cos(angle + Math.PI / 2)),
                (float)(py - d * Math.Sin(angle + Math.PI / 2)),
                (float)branch.LineX2,
                (float)branch.LineY2);
            branch.ArrowPath = arrowPath;
        }

        public override void Draw(Graphics g, TreeStyle style)
        {
            if (style.ExpressionFillColor != null)
            {
                Brush b = new SolidBrush(style.ExpressionFillColor);
                g.FillRectangle(b, (float)NodeLeft, (float)NodeTop, (float)NodeWidth, (float)NodeHeight);
                b.Dispose();
            }

            Pen p = new Pen(new SolidBrush(style.ExpressionBorderColor));
            g.DrawRectangle(p, (float)NodeLeft, (float)NodeTop, (float)NodeWidth, (float)NodeHeight);
            p.Dispose();

            foreach (Branch branch in Branches)
            {
                Brush b = new SolidBrush(branch.Node is VariableNode ? style.VariableArrowColor : style.ExpressionArrowColor);

                p = new Pen(b);
                g.DrawLine(p, (float)branch.LineX1, (float)branch.LineY1, (float)branch.LineX2, (float)branch.LineY2);
                p.Dispose();

                g.FillPath(b, branch.ArrowPath);
                b.Dispose();
            }

            p.Dispose();


            double y = NodeTop;

            g.DrawString(Description, style.Font, Brushes.Black, (float)NodeLeft, (float)y);
            y += g.MeasureString(Description, style.Font).Height;

            g.DrawImage(ExpressionImage, (float)NodeLeft, (float)y);
            y += ExpressionImage.Height;


            int index = 0;

            foreach (object result in Results)
            {
                string s = GetValueString(result);
                if (Results.Length > 1)
                {
                    s = ResultVariableNames[index] + "=" + s;
                }

                g.DrawString(s, style.ValueFont, Brushes.Black, (float)NodeLeft, (float)y);
                y += g.MeasureString(s, style.ValueFont).Height;
                index++;
            }

            foreach (Branch branch in Branches)
            {
                branch.Node.Draw(g, style);
            }


            if (Step.State == StepState.Skipped)
            {
                p = new Pen(Brushes.DarkRed, 2);
                g.TranslateTransform(-10, -10);
                g.DrawLine(p, (float)NodeLeft, (float)NodeTop, (float)(NodeLeft + 20), (float)(NodeTop + 20));
                g.DrawLine(p, (float)(NodeLeft + 20), (float)NodeTop, (float)NodeLeft, (float)(NodeTop + 20));
                g.TranslateTransform(10, 10);
                p.Dispose();
            }
        }

        public void InheritVariables(Node node)
        {
            if (node is VariableNode)
            {
                ResultVariableNames.Add((node as VariableNode).Name);
            }
            else
            {
                ResultVariableNames.AddRange((node as ComputedNode).ResultVariableNames);
            }
        }
    }
}
