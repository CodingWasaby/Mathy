using System;
using System.Drawing;

namespace Mathy.Visualization.Expressions.Nodes
{
    public class LogExpression : Expression
    {
        public LogExpression(string name, Expression b, Expression x)
        {
            this.name = name;
            this.b = b;
            this.x = x;
        }


        private string name;

        private Expression b;

        private Expression x;

        private SizeF textSize;


        public override void FindMaxTop(float[] height)
        {
            float h = Math.Max(textSize.Height / 2, x.FindMaxTop());

            if (height[0] < h)
            {
                height[0] = h;
            }
        }

        public override void FindMaxBottom(float[] height)
        {
            float h = DesiredHeight - Math.Max(textSize.Height / 2, x.FindMaxTop());

            if (height[0] < h)
            {
                height[0] = h;
            }
        }


        protected override void OnMeasure(MeasureSpec widthSpec, MeasureSpec heightSpec, Graphics g, Font font, Style style)
        {
            textSize = g.MeasureString(name, font);

            if (b != null)
            {
                b.Measure(widthSpec, heightSpec, g, style.SmallFont, style);
            }

            x.Measure(widthSpec, heightSpec, g, font, style);


            float width;
            float height;

            if (b == null)
            {
                width = textSize.Width + x.DesiredWidth;
                height = Math.Max(textSize.Height / 2, x.FindMaxBottom()) + Math.Max(textSize.Height / 2, x.FindMaxTop());
            }
            else
            {
                width = textSize.Width + b.DesiredWidth + x.DesiredWidth;
                height = Math.Max(textSize.Height / 2, Math.Max(b.DesiredHeight, x.FindMaxBottom())) + Math.Max(textSize.Height / 2, x.FindMaxTop());
            }

            SetDesiredSize(width, height);
        }

        protected override void OnLayout(float left, float top, float width, float height, Graphics g, Font font, Style style)
        {
            float pos = textSize.Width;
            float baseLine = Math.Max(textSize.Height / 2, x.FindMaxTop());


            if (b != null)
            {
                b.Layout(this, pos, baseLine, b.DesiredWidth, b.DesiredHeight, g, style.SmallFont, style);
                pos += b.DesiredWidth;
            }


            x.Layout(this, pos, baseLine - x.FindMaxTop(), x.DesiredWidth, x.DesiredHeight, g, font, style);
        }

        protected override void OnDraw(Graphics g, Font font, Style style)
        {
            float y = Top + Math.Max(textSize.Height / 2, x.FindMaxTop()) - textSize.Height / 2;
            g.DrawString(name, font, Brushes.Black, Left, y);


            if (b != null)
            {
                b.Draw(g, style.SmallFont, style);
            }


            x.Draw(g, font, style);
        }
    }
}
