using System;
using System.Drawing;

namespace Mathy.Visualization.Expressions.Nodes
{
    public class IfElseExpression : Expression
    {
        public Expression Condition { get; set; }

        public Expression PositiveBranch { get; set; }

        public Expression NegativeBranch { get; set; }


        public override void FindMaxTop(float[] height)
        {
            float h = Condition.FindMaxTop() + 10 + PositiveBranch.DesiredHeight;

            if (height[0] < h)
            {
                height[0] = h;
            }
        }

        public override void FindMaxBottom(float[] height)
        {
            float h = Condition.FindMaxBottom() + 10 + NegativeBranch.DesiredHeight;

            if (height[0] < h)
            {
                height[0] = h;
            }
        }


        protected override void OnMeasure(MeasureSpec widthSpec, MeasureSpec heightSpec, Graphics g, Font font, Style style)
        {
            Condition.Measure(widthSpec, heightSpec, g, font, style);
            PositiveBranch.Measure(widthSpec, heightSpec, g, font, style);
            NegativeBranch.Measure(widthSpec, heightSpec, g, font, style);

            SetDesiredSize(
                Condition.DesiredWidth + 20 + Math.Max(PositiveBranch.DesiredWidth, NegativeBranch.DesiredWidth),
                Condition.DesiredHeight + 20 + PositiveBranch.DesiredHeight + NegativeBranch.DesiredHeight);
        }


        protected override void OnLayout(float left, float top, float width, float height, Graphics g, Font font, Style style)
        {
            float conditionTop = Condition.FindMaxTop();
            float conditionBottom = Condition.FindMaxBottom();

            float baseLine = conditionTop + 10 + PositiveBranch.DesiredHeight;

            Condition.Layout(this, 0, baseLine - Condition.FindMaxTop(), Condition.DesiredWidth, Condition.DesiredHeight, g, font, style);
            PositiveBranch.Layout(this, Condition.DesiredWidth + 20, 0, PositiveBranch.DesiredWidth, PositiveBranch.DesiredHeight, g, font, style);
            NegativeBranch.Layout(this, Condition.DesiredWidth + 20, DesiredHeight - NegativeBranch.DesiredHeight, NegativeBranch.DesiredWidth, NegativeBranch.DesiredHeight, g, font, style);
        }

        protected override void OnDraw(Graphics g, Font font, Style style)
        {
            Condition.Draw(g, font, style);
            PositiveBranch.Draw(g, font, style);
            NegativeBranch.Draw(g, font, style);

            float baseLine = Top + Condition.FindMaxTop() + 10 + PositiveBranch.Height;

            Pen p = new Pen(Brushes.Black);
            DrawLine(Left + Condition.Width + 2, baseLine - 1, Left + Condition.Width + 18, Top + PositiveBranch.FindMaxTop(), "T", g, p, style);
            DrawLine(Left + Condition.Width + 2, baseLine + 1, Left + Condition.Width + 18, Top + Height - NegativeBranch.FindMaxBottom(), "F", g, p, style);
            p.Dispose();
        }


        private void DrawLine(float x1, float y1, float x2, float y2, string text, Graphics g, Pen p, Style style)
        {
            double angle = -Math.Atan2(y2 - y1, x2 - x1);

            SizeF size = g.MeasureString(text, style.SmallFont);
            size = new SizeF(size.Width - 5, size.Height - 5);

            double dx1 = x1 + (x2 - x1) / 2 - size.Width / 2 - 1;
            double dx2 = x1 + (x2 - x1) / 2 + size.Width / 2 + 1;

            double dy1 = y1 - (dx1 - x1) * Math.Tan(angle);
            double dy2 = y2 - (dx2 - x2) * Math.Tan(angle);

            g.DrawLine(p, x1, y1, (float)dx1, (float)dy1);
            g.DrawLine(p, (float)dx2, (float)dy2, x2, y2);

            double xc = x1 + (x2 - x1) / 2;
            double yc = y1 - (xc - x1) * Math.Tan(angle);

            g.DrawString(text, style.SmallFont, Brushes.Black, (float)xc - size.Width / 2, (float)yc - size.Height / 2);
        }
    }
}
