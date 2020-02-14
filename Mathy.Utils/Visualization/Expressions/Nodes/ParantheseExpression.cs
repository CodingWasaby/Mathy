using System;
using System.Drawing;

namespace Mathy.Visualization.Expressions.Nodes
{
    public class ParantheseExpression : Expression
    {
        public Expression Body { get; set; }

        public int ParantheseLevel { get; set; }

        private SizeF size;


        public override void FindMaxTop(float[] height)
        {
            float h = (float)Math.Max(size.Height / 2, Body.FindMaxTop());

            if (height[0] < h)
            {
                height[0] = h;
            }
        }

        public override void FindMaxBottom(float[] height)
        {
            float h = (float)Math.Max(size.Height / 2, Body.FindMaxBottom());

            if (height[0] < h)
            {
                height[0] = h;
            }
        }


        protected override void OnMeasure(MeasureSpec widthSpec, MeasureSpec heightSpec, Graphics g, Font font, Style style)
        {
            size = g.MeasureString(")", style.BoldFont);


            MeasureSpec width = null;

            if (widthSpec.Mode == MeasureSpecMode.Fixed)
            {
                width = MeasureSpec.MakeFixed(widthSpec.Size - size.Width * 2);
            }
            else if (widthSpec.Mode == MeasureSpecMode.AtMost)
            {
                width = MeasureSpec.MakeFixed(widthSpec.Size - size.Width * 2);
            }
            else if (widthSpec.Mode == MeasureSpecMode.Unspecified)
            {
                width = widthSpec;
            }

            Body.Measure(width, heightSpec, g, font, style);


            SetDesiredSize(
                (float)(widthSpec.Mode == MeasureSpecMode.Fixed ? widthSpec.Size : Body.DesiredWidth + size.Width * 2),
                (float)(Math.Max(size.Height / 2, Body.FindMaxTop()) + Math.Max(size.Height / 2, Body.FindMaxBottom())));
        }

        protected override void OnLayout(float left, float top, float width, float height, Graphics g, Font font, Style style)
        {
            float baseLine = DesiredHeight - Math.Max(size.Height / 2, Body.FindMaxBottom());

            Body.Layout(this, size.Width, baseLine - Body.FindMaxTop(), Body.DesiredWidth, Body.DesiredHeight, g, font, style);
        }

        protected override void OnDraw(Graphics g, Font font, Style style)
        {
            Body.Draw(g, font, style);

            float baseLine = (float)(Top + Height - Math.Max(size.Height / 2, Body.FindMaxBottom()) - size.Height / 2) + 1;


            Brush brush = style.LevelContext.GetBrush(ParantheseLevel);

            g.DrawString("(", style.BoldFont, brush, Left, baseLine);
            g.DrawString(")", style.BoldFont, brush, Left + Width - size.Width, baseLine);
        }
    }
}
